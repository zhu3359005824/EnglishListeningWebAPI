using FileService.Domain.Entity;
using ZHZ.Tools;

namespace FileService.Domain
{
    public class FileDomainService
    {
        private readonly IFileRepository _fileRepository;
        private readonly ICloundClient? _localcloundClient;
        private readonly ICloundClient? _publicloundClient;

        public FileDomainService(IFileRepository fileRepository, IEnumerable<ICloundClient> cloundClients)
        {
            _fileRepository = fileRepository;
            _localcloundClient = cloundClients.FirstOrDefault(c => c.type == CloundClientType.Local);
            _publicloundClient = cloundClients.FirstOrDefault(c => c.type == CloundClientType.Public);

        }

        public async Task<UploadItem> UploadItemAsync(Stream stream, string fileName)
        {
            long fileByteSize = stream.Length;

            string fileSHA256Hash = HashHelper.ComputeSHA256HashUsingStream(stream);

            UploadItem oldItem = await _fileRepository.FindOneAsync(fileByteSize, fileSHA256Hash);

            if (oldItem == null)
            {
                DateTime CreateDateTime = DateTime.Today;

                //存储的文件夹
                string key = $"{CreateDateTime.Year}/{CreateDateTime.Month}/{CreateDateTime.Day}/{fileSHA256Hash}/{fileName}";

                stream.Position = 0;

                string fullPath = $"E:/DDDProjectUpload/{key}";

                //创建目录
                var directory = Path.GetDirectoryName(fullPath);

                if (!Directory.Exists(directory))

                {

                    Directory.CreateDirectory(directory);

                }
                //保存文件
                using FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite);
                await stream.CopyToAsync(fileStream);



                stream.Position = 0;

                Guid id = Guid.NewGuid();
                Uri srcUrl = new Uri(fullPath);
                UploadItem uploadItem = new UploadItem(id, CreateDateTime, fileName, fileSHA256Hash, fileByteSize, srcUrl);

                return uploadItem;


            }
            else
            {
                return oldItem;
            }


        }

    }
}
