using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Neutronium.Api.Logging;

namespace Neutronium.Core.Logging
{
	internal class Logger : ILogger
	{
		private readonly string _id;

		public string Id => _id;
		
		public bool Enabled
		{ get; set; } = true;

		internal Logger(string id)
		{
			_id = id;
		}

		[Conditional("DEV")]
		internal void Dev(string message)
		{
			if (!Enabled) return;
			Log.Submit(_id, LogLevel.DEV, message);
		}

		public void Debug(string message)
		{
			if (!Enabled) return;
			Log.Submit(_id, LogLevel.DEBUG, message);
		}
		
		public void Info(string message)
		{
			if (!Enabled) return;
			Log.Submit(_id, LogLevel.INFO, message);
		}

		public void Warn(string message)
		{
			if (!Enabled) return;
			Log.Submit(_id, LogLevel.WARN, message);
		}
		
		public void Error(string message)
		{
			if (!Enabled) return;
			Log.Submit(_id, LogLevel.ERROR, message, null);
		}

		public void Error(string message, Exception ex)
		{
			if (!Enabled) return;
			Log.Submit(_id, LogLevel.ERROR, message, ex);
		}
	}
}
