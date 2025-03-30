using FileService.Infrastructure;
using MediaEncoder.Domain;
using MediaEncoder.Infrastructure;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System.Diagnostics;
using System.Net;
using ZHZ.EventBus;
using ZHZ.Tools;

namespace MediaEncoder.WebAPI
{
    public class EncoderBackgroundService : BackgroundService
    {

        private readonly MediaEncoderDbContext _mediaEncoderDbContext;
        private readonly MyDbContext _uploadDbcontext;
        private readonly IMediaEncoderRepository _mediaEncoderRepository;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediaEncoder _mediaEncoder;

        private readonly MediaEncoderFactory _mediaEncoderFactory;

        private readonly List<RedLockMultiplexer> redLockMultiplexerList;
        private readonly IEventBus _eventBus;

        private readonly IHttpClientFactory _httpClientFactory;

        public EncoderBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            var sp = serviceScopeFactory.CreateScope();
            _mediaEncoderDbContext = sp.ServiceProvider.GetRequiredService<MediaEncoderDbContext>();
            _mediaEncoderRepository = sp.ServiceProvider.GetRequiredService<IMediaEncoderRepository>();
            _logger = sp.ServiceProvider.GetRequiredService<ILogger>();
            _mediaEncoder = sp.ServiceProvider.GetRequiredService<IMediaEncoder>();
            _serviceScopeFactory = serviceScopeFactory;
            _mediaEncoderFactory = sp.ServiceProvider.GetRequiredService<MediaEncoderFactory>();
            //生产环境中，RedLock需要五台服务器才能体现价值，测试环境无所谓
            IConnectionMultiplexer connectionMultiplexer = sp.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            this.redLockMultiplexerList = new List<RedLockMultiplexer> { new RedLockMultiplexer(connectionMultiplexer) };
            _eventBus = sp.ServiceProvider.GetRequiredService<IEventBus>();
            _uploadDbcontext = sp.ServiceProvider.GetRequiredService<MyDbContext>();

            _httpClientFactory = sp.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _serviceScopeFactory.CreateScope().Dispose();

        }




        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //获取所有处于Ready状态的任务
                //ToListAsync()可以避免在循环中再用DbContext去查询数据导致的“There is already an open DataReader associated with this Connection which must be closed first.”
                var readyItems = await _mediaEncoderRepository.FindAsync(ItemStatus.Ready);
                foreach (EncodingItem readyItem in readyItems)
                {
                    try
                    {
                        await ProcessItemAsync(readyItem, stoppingToken);//因为转码比较消耗cpu等资源，因此串行转码
                    }
                    catch (Exception ex)
                    {
                        readyItem.Fail(ex);
                    }
                    await this._mediaEncoderDbContext.SaveChangesAsync(stoppingToken);
                }
                await Task.Delay(5000);//暂停5s，避免没有任务的时候CPU空转
            }
        }

        /// <summary>
        /// 构建转码后的目标文件
        /// </summary>
        /// <param name="encodingItem"></param>
        /// <returns></returns>
        private static FileInfo BuildDestFileInfo(EncodingItem encodingItem)
        {
            string outputType = encodingItem.OutType;
            string tempDir = Path.GetTempPath();
            string destFullPath = Path.Combine(tempDir, Guid.NewGuid() + "." + outputType);

            return new FileInfo(destFullPath);
        }

        /// <summary>
        /// 计算文件散列值
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private static string ComputeSHA256Hash(FileInfo fileInfo)
        {
            using (FileStream fs = fileInfo.OpenRead())
            {
                return HashHelper.ComputeSha256Hash(fs);
            }
        }


        /// <summary>
        /// 进行转码
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <param name="outputType"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<bool> EncodeAsync(FileInfo srcFile, FileInfo destFile, string outputType, CancellationToken ct)
        {
            var encoder = _mediaEncoderFactory.Create(outputType);
            if (encoder == null)
            {
                Debug.Fail(string.Format("找不到转码器,目标格式{0}", outputType));
                return false;
            }

            try
            {
                await encoder.EncodeAsync(srcFile, destFile, outputType, null, ct);
            }
            catch (Exception ex)
            {
                Debug.Fail($"转码失败{ex.Message}");
                return false;
            }
            return true;
        }


        private async Task<(bool ok, FileInfo srcFile, Uri destUrl)> GetUploadFile(EncodingItem encodingItem, CancellationToken ct)
        {
            var srcName = encodingItem.FileName;
            var src = _uploadDbcontext.UploadItems.Where(t => t.FileName == srcName).ToList();

            var srcUrl = src[0].SourceUrl;

            //开始下载源文件
            string tempDir = Path.Combine(Path.GetTempPath(), "MediaEncodingDir");
            //源文件的临时保存路径
            string sourceFullPath = Path.Combine(tempDir, Guid.NewGuid() + "."
                + Path.GetExtension(encodingItem.FileName));

            Uri destUrl = new Uri(sourceFullPath);


            FileInfo sourceFile = new FileInfo(sourceFullPath);
            Guid id = encodingItem.Id;
            sourceFile.Directory!.Create();//创建可能不存在的文件夹
            Debug.WriteLine($"Id={id}，准备从{encodingItem.SourceUrl}下载到{sourceFullPath}");

            HttpClient client = _httpClientFactory.CreateClient();
            var statusCode = await client.DownloadFileAsync(srcUrl, sourceFullPath);


            if (statusCode != HttpStatusCode.OK)
            {
                Debug.WriteLine($"下载Id={id}，Url={encodingItem.SourceUrl}失败，{statusCode}");
                sourceFile.Delete();
                return (false, sourceFile, null);
            }
            else
            {
                return (true, sourceFile, destUrl);
            }



        }



        private async Task ProcessItemAsync(EncodingItem encItem, CancellationToken ct)
        {
            Guid id = encItem.Id;
            var expiry = TimeSpan.FromSeconds(30);
            //Redis分布式锁来避免两个转码服务器处理同一个转码任务的问题
            var redlockFactory = RedLockFactory.Create(redLockMultiplexerList);
            string lockKey = $"MediaEncoder.EncodingItem.{id}";
            //用RedLock分布式锁，锁定对EncodingItem的访问
            using var redLock = await redlockFactory.CreateLockAsync(lockKey, expiry);
            if (!redLock.IsAcquired)
            {
                Console.WriteLine($"获取{lockKey}锁失败，已被抢走");
                //获得锁失败，锁已经被别人抢走了，说明这个任务被别的实例处理了（有可能有服务器集群来分担转码压力）
                return;//再去抢下一个
            }
            encItem.Start();
            await _mediaEncoderDbContext.SaveChangesAsync(ct);//立即保存一下状态的修改
                                                              //发出一次集成事件

            (var downloadOk, var srcFile, var destUrl) = await GetUploadFile(encItem, ct);
            if (!downloadOk)
            {
                encItem.Fail($"下载失败");
                return;
            }

            FileInfo destFile = BuildDestFileInfo(encItem);
            try
            {
                Console.WriteLine($"下载Id={id}成功，开始计算Hash值");
                long fileSize = srcFile.Length;
                string srcFileHash = ComputeSHA256Hash(srcFile);
                //如果之前存在过和这个文件大小、hash一样的文件，就认为重复了
                EncodingItem? prevInstance = await _mediaEncoderRepository.FindOneFinishAsync(srcFileHash, fileSize);
                if (prevInstance != null)
                {
                    Console.WriteLine($"检查Id={id}Hash值成功，发现已经存在相同大小和Hash值的旧任务Id={prevInstance.Id}，返回！");
                    _eventBus.Publish("MediaEncoding.Duplicated", new { encItem.Id, encItem.SourceSystem, OutputUrl = prevInstance.OutputUrl });
                    encItem.Complete(prevInstance.OutputUrl!);
                    return;
                }
                //开始转码
                Console.WriteLine($"Id={id}开始转码，源路径:{srcFile},目标路径:{destFile}");
                string outputFormat = encItem.OutType;
                var encodingOK = await EncodeAsync(srcFile, destFile, outputFormat, ct); ;
                if (!encodingOK)
                {
                    encItem.Fail($"转码失败");
                    return;
                }
                //开始上传
                Console.WriteLine($"Id={id}转码成功，开始准备上传");

                encItem.Complete(destUrl);
                encItem.ChangeFileMeta(fileSize, srcFileHash);
                Console.WriteLine($"Id={id}转码结果上传成功");
                //发出集成事件和领域事件
            }
            finally
            {
                srcFile.Delete();
                destFile.Delete();
            }
        }















    }
}
