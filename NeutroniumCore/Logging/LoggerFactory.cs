using System.Collections.Concurrent;
using Neutronium.Api.Logging;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Logging
{
	internal class LoggerFactory
	{
		private static readonly ConcurrentDictionary<string, Logger> s_loggers = new ConcurrentDictionary<string, Logger>();

        internal static ILogger GetModLogger(string modStaticId, string? category = null)
        {
            string loggerId;
			if (category != null)
            {
				
				loggerId = $"Mod.{modStaticId}.{category}";
            }
			else
            {
				loggerId = $"Mod.{modStaticId}";
            }

            if (!s_loggers.TryGetValue(loggerId, out Logger logger))
            {
                logger = new Logger(loggerId);
                s_loggers[loggerId] = logger;
            }
			
			return logger;
        }
        
		internal static ILogger GetInternalLogger(string id)
		{
			if (!s_loggers.TryGetValue(id, out Logger logger))
			{
				logger = new Logger(id);
				s_loggers[id] = logger;
			}

			return logger;
		}
	}
}
