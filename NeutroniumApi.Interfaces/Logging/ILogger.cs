using System;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Logging
{
	/// <summary>
	/// An interface for writing messages to the Neutronium log.
	/// </summary>
	[StableApi(ApiVersions.Alpha_Milestone1)]
	[WrapInterface]
	public interface ILogger
	{
		/// <summary>
		/// Gets the id of this logger.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		string Id
		{ get; }

		/// <summary>
		/// Logs a message with debug status.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		void Debug(string message);

		/// <summary>
		/// Logs a message with info status.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		void Info(string message);

		/// <summary>
		/// Logs a message with warning status.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		void Warn(string message);

		/// <summary>
		/// Logs a message with error status.
		/// An exception can be provided optionally.
		/// </summary>
		[StableApi(ApiVersions.Alpha_Milestone1)]
		void Error(string message, Exception exception = null);
	}
}
