using System;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Logging
{
	/// <summary>
	/// An interface for writing messages to the Neutronium log.
	/// </summary>
	[StableApi(ApiVersions.NextReleaseVersion)]
	[WrapInterface]
	public interface ILogger
	{
		/// <summary>
		/// Gets the id of this logger.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		string Id
		{ get; }

		/// <summary>
		/// Toggles whether this logger is enabled.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		bool Enabled
		{ get; set; }

		/// <summary>
		/// Logs a message with debug status.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		void Debug(string message);

		/// <summary>
		/// Logs a message with info status.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		void Info(string message);

		/// <summary>
		/// Logs a message with warning status.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		void Warn(string message);

		/// <summary>
		/// Logs a message with error status.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		void Error(string message);
		
		/// <summary>
		/// Logs a message with error status and exception details.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		void Error(string message, Exception exception);
	}
}
