using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Logging
{
	[StableApi(ApiVersions.NextReleaseVersion)]
	public interface ILoggingApi
	{
		/// <summary>
		/// Returns a Neutronium logger for the calling mod.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		ILogger GetModLogger();

		/// <summary>
		/// Returns a namespaced Neutronium logger for the calling mod.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		ILogger GetModLogger(string category);
	}
}
