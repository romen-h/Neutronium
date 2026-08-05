using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Neutronium.Api.Logging;

namespace Neutronium.Core.Logging.Api
{
	internal class Logger : ILogger
	{
		private readonly string _id;

		public string Id => _id;

		internal Logger(string id)
		{
			_id = id;
		}

		[Conditional("DEV")]
		internal void Dev(string message) => Log.Submit(_id, LogLevel.DEV, message);

		public void Debug(string message) => Log.Submit(_id, LogLevel.DEBUG, message);
		
		public void Info(string message) => Log.Submit(_id, LogLevel.INFO, message);

		public void Warn(string message) => Log.Submit(_id, LogLevel.WARN, message);

		public void Error(string message, Exception ex = null) => Log.Submit(_id, LogLevel.ERROR, message, ex);
	}
}
