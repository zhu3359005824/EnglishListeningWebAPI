using Listening.Domain.Entity;

namespace Listening.Main.WebAPI.Controllers.EpisodeController
{
    public record EpisodeModel(Guid id, string sentenceContxt, string sentenceType, Guid albumId, double RealSeconds, IEnumerable<SentenceModel> SentenceModels)
    {
        public static EpisodeModel? Create(Episode a, bool IsParse)
        {
            if (a == null) throw new ArgumentNullException("Album不存在");

            List<SentenceModel> list = new List<SentenceModel>();

            if (IsParse)
            {
                var sentences = a.GetSentenceContext();

                foreach (var s in sentences)
                {
                    SentenceModel sentenceModel = new SentenceModel(s.StartTime.Value.TotalSeconds, s.EndTime.Value.TotalSeconds, s.context);

                    list.Add(sentenceModel);
                }

            }

            return new EpisodeModel(a.Id, a.SentenceContxt, a.SentenceType, a.AlbumId, a.RealSeconds,
                list);
        }


        public static EpisodeModel[] Create(Episode[] items, bool IsParse)
        {
            return items.Select(e => Create(e, IsParse)!).ToArray();
        }
    }
}
