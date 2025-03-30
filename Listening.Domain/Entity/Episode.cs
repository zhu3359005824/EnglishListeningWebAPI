using Listening.Domain.Event;
using Listening.Domain.SentenceParser;
using Listening.Domain.ValueObject;
using ZHZ.Entity;

namespace Listening.Domain.Entity
{
    /// <summary>
    /// 播放的听力内容
    /// </summary>
    public class Episode : AggregateRootEntity
    {
        public Guid AlbumId { get; set; }

        /// <summary>
        /// 字幕内容
        /// </summary>
        public string SentenceContxt { get; set; }

        /// <summary>
        /// 字幕类型
        /// </summary>

        public string SentenceType { get; set; }

        public string EpisodeName { get; set; }

        public int ShowIndex { get; set; }

        public double RealSeconds { get; private set; }


        public Episode(Guid albumId, string sentenceContxt, string sentenceType, string episodeName) : base()
        {
            AlbumId = albumId;
            SentenceContxt = sentenceContxt;
            SentenceType = sentenceType;
            EpisodeName = episodeName;
            this.AddDomainEvent(new EpisodeCreatedEvent(this));
        }

        public IEnumerable<Sentence> GetSentenceContext()
        {
            var parser = ParserFactory.GetParser(SentenceType);
            if (parser == null)
            {
                throw new ArgumentNullException("parser为空");
            }

            return parser.Parse(SentenceContxt);

        }

        public Episode ChangeShowIndex(int value)
        {
            this.ShowIndex = value;
            this.AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
            return this;
        }

        public void SetRealSeconds(double seconds)
        {
            this.RealSeconds = seconds;
        }

        public override void SoftDelete()
        {
            base.SoftDelete();
            this.AddDomainEvent(new EpisodeDeletedEvent(this.Id));
        }




    }
}
