using System.IO;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace DaysOn.Facade
{
    public class EncodedMessagePatternConvertor : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            var loggingEvent = state as LoggingEvent;
            if (loggingEvent == null)
            {
                return;
            }

            var encodedMessage = loggingEvent.RenderedMessage.Replace("\r", " ").Replace("\n", " ");

            writer.Write(encodedMessage);
        }
    }

    public class CustomPatternLayout : PatternLayout
    {
        public CustomPatternLayout()
        {
            AddConverter(new ConverterInfo { Name = "encodedmessage", Type = typeof(EncodedMessagePatternConvertor) });
        }
    }
}