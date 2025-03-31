using Azure.Core;
using Listening.Domain.Entity;

namespace Listening.Main.WebAPI.Controllers.EpisodeController
{
    public record EpisodeModel(Guid id,string episodeName,Uri AudioUrl, string sentenceContxt, string sentenceType, string AlbumName, double RealSeconds, IEnumerable<SentenceModel> SentenceModels)
    {
        public static EpisodeModel? Create(Episode a, bool IsParse)
        {
            if (a == null) throw new ArgumentNullException("Album不存在");

            List<SentenceModel> list = new List<SentenceModel>();

            if (IsParse)
            {
                var sentences = a.GetSentenceContext();
                int i = 0;
                foreach (var s in sentences)
                {
                    SentenceModel sentenceModel = new SentenceModel(s.StartTime.Value.TotalSeconds, s.EndTime.Value.TotalSeconds, s.context);

                    list.Add(sentenceModel);
                    
                 
                }
            }

            string fileUri = a.AudioUrl.ToString();
            string httpBase = "http://127.0.0.1:8080";
            string prefixToRemove = "file:///E:/DDDProjectUpload";
            string httpUri;
            Uri AudioUrl;

            ///
            //需要在"E:\DDDProjectUpload"路径下cmd 然后输入http-server启动node.js服务,前端才可以正常访问文件
            //

            // 检查 fileUri 是否以指定前缀开头
            if (fileUri.StartsWith(prefixToRemove))
            {
                // 截取 fileUri 中除前缀之外的部分
                string remainingPath = fileUri.Substring(prefixToRemove.Length);
                // 拼接新的 http URI
               httpUri = httpBase + remainingPath;
                AudioUrl = new Uri(httpUri);

                return new EpisodeModel(a.Id, a.EpisodeName, AudioUrl, a.SentenceContxt, a.SentenceType, a.AlbumName, list[list.Count - 1].end,
                    list);
            }
            return null;


        }


        public static EpisodeModel[] Create(Episode[] items, bool IsParse)
        {
            return items.Select(e => Create(e, IsParse)!).ToArray();
        }
    }
}
