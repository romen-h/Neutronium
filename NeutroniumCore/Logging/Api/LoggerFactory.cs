using System.Collections.Concurrent;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Logging.Api
{
	[StableApi(ApiVersions.Alpha_Milestone1)]
	public static class LoggerFactory
	{
		private static readonly ConcurrentDictionary<string, Logging.Internal.Logger> s_loggers = new ConcurrentDictionary<string, Internal.Logger>();

        [StableApi(ApiVersions.Alpha_Milestone1)]
        public static ILogger GetLogger(string modStaticId, string category = null)
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

            if (!s_loggers.TryGetValue(loggerId, out Logging.Internal.Logger logger))
            {
                logger = new Logging.Internal.Logger(loggerId);
                s_loggers[loggerId] = logger;
            }
			
			return logger;
        }
        
		internal static ILogger GetInternalLogger(string id)
		{
			if (!s_loggers.TryGetValue(id, out Logging.Internal.Logger logger))
			{
				logger = new Logging.Internal.Logger(id);
				s_loggers[id] = logger;
			}

			return logger;
		}
	}
}
