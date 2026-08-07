using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Logging;

namespace Neutronium.Core.Logging
{
	internal class LoggingApi : ILoggingApi
	{
		private readonly string _modId;
		
		internal LoggingApi(string modId)
		{
			_modId = modId;
		}
		
		public ILogger GetModLogger() => LoggerFactory.GetModLogger(_modId, null);

		public ILogger GetModLogger(string category) => LoggerFactory.GetModLogger(_modId, category);
	}
}
