namespace Listening.Domain.SentenceParser
{
    public class ParserFactory
    {
        private static List<ISentenceParser> sentenceParsers = new List<ISentenceParser>();

        static ParserFactory()
        {
            var parsers = typeof(ISentenceParser).Assembly.GetTypes().Where(t => typeof(ISentenceParser)
            .IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var item in parsers)
            {
                ISentenceParser parser = (ISentenceParser)Activator.CreateInstance(item);
                sentenceParsers.Add(parser);
            }

        }

        public static ISentenceParser? GetParser(string sentenceType)
        {
            foreach (var item in sentenceParsers)
            {
                if (item.CanParse(sentenceType))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
