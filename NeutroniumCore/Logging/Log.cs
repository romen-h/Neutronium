using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using static KSerialization.DebugLog;

namespace Neutronium.Core.Logging
{
	internal static class Log
	{
		private static readonly BlockingCollection<string> s_linesToWrite = new BlockingCollection<string>();

		private static Thread s_writerThread;

		private static string s_logFile;

		private static bool s_unityLogInitialized = false;

		internal static void Initialize()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
			    string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			    string appData = Path.GetDirectoryName(localAppData);
			    string localLowAppData = Path.Combine(appData, "LocalLow");
			    string gameLogFolder = Path.Combine(localLowAppData, "Klei", "Oxygen Not Included");
			    Directory.CreateDirectory(gameLogFolder);
			    s_logFile = Path.Combine(gameLogFolder, "neutronium.log");
			    File.WriteAllText(s_logFile, "", Encoding.UTF8);
			}

			s_writerThread = new Thread(WriterThread)
			{
				IsBackground = true
			};
			s_writerThread.Start();

			Info("Core.Logging", "Initialized.");
		}

		internal static void OnUnityInitialized()
		{
			s_unityLogInitialized = true;
		}

		internal static void Shutdown()
		{
			Info("Core.Logging", "Shutting down...");
			s_linesToWrite.CompleteAdding();
			if (!s_writerThread.Join(1000))
			{
				Trace.WriteLine("[Neutronium.Core] Writer thread did not shut down within 1 second.");
				s_writerThread.Abort();
			}
		}

		internal static void Submit(string id, LogLevel level, string message, Exception ex = null)
		{
			WriteToNeutroniumLog(id, level, message, ex);
			if (s_unityLogInitialized) WriteToUnityLog(id, level, message, ex);
		}

		private static void WriteToNeutroniumLog(string id, LogLevel level, string message, Exception ex)
		{
			System.DateTime now = System.DateTime.UtcNow;
			StringBuilder sb = new StringBuilder();
			sb.Append("[");
			sb.Append(now.ToString("HH:mm:ss.fff"));
			sb.Append("] [");
			sb.Append(level.ToString());
			sb.Append("] [");
			sb.Append(id);
			sb.AppendLine("]");
			sb.AppendLine(message);

			if (ex != null)
			{
				sb.AppendLine(ex.ToString());
			}

			string str = sb.ToString();
			s_linesToWrite.Add(str);
		}

		private static void WriteToUnityLog(string id, LogLevel level, string message, Exception ex)
		{
			string time = System.DateTime.UtcNow.ToString("HH:mm:ss.fff");
			string line = $"[{time}] [Neutronium.{id}] [{level}] {message}";

			switch (level)
			{
				case LogLevel.DEV:
				case LogLevel.DEBUG:
				case LogLevel.INFO:
					UnityEngine.Debug.Log(line);
					break;

				case LogLevel.WARN:
					UnityEngine.Debug.LogWarning(line);
					break;

				case LogLevel.ERROR:
					UnityEngine.Debug.LogError(line);
					if (ex != null) UnityEngine.Debug.LogException(ex);
					break;
				default:
					break;
			}
		}

		internal static void Debug(string id, string message) => Submit(id, LogLevel.DEBUG, message);

		internal static void Info(string id, string message) => Submit(id, LogLevel.INFO, message);

		internal static void Warn(string id, string message) => Submit(id, LogLevel.WARN, message);

		internal static void Error(string id, string message, Exception ex = null) => Submit(id, LogLevel.ERROR, message, ex);

		private static void WriterThread()
		{
			Thread.CurrentThread.Name = "Neutronium.Core Log Writer";
			Debug("Core.Logging", "Logging thread started.");

			foreach (var line in s_linesToWrite.GetConsumingEnumerable())
			{
				try
				{
					File.AppendAllText(s_logFile, line, Encoding.UTF8);
				}
				catch (Exception ex)
				{
					Trace.WriteLine("[Neutronium.Core] Failed to write log lines to log file.");
					Trace.WriteLine(ex.ToString());
				}
			}

			Trace.WriteLine("[Neutronium.Core] Logging thread stopped.");
		}
	}
}
