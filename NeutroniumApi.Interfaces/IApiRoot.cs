using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Logging;
using Neutronium.Api.Meta;
using Neutronium.Api.Registry;

namespace Neutronium.Api
{
	/// <summary>
	/// The root Neutronium API object to access all features of Neutronium.
	/// </summary>
	/// <remarks>
	/// This API is guaranteed to be binary compatible:<br/>
	/// - No signatures will be removed after being released.<br/>
	/// - No signatures will change after released.<br/>
	/// - New signatures can be added in future versions.<br/>
	/// - Deprecated signatures will be annotated with [Obsolete] and behave safely with legacy behaviour as much as possible.
	/// </remarks>
	[StableApi(ApiVersions.NextReleaseVersion)]
	[WrapInterface]
	public interface IApiRoot
	{
		[StableApi(ApiVersions.NextReleaseVersion)]
		[GetOnce]
		Version NeutroniumVersion
		{ get; }

		/// <summary>
		/// A Neutronium logger factory interface for creating namespaced loggers.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		[GetOnce]
		ILoggerFactory LoggerFactory
		{ get; }

		/// <summary>
		/// Returns a Neutronium logger for the given mod static id.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		ILogger GetModLogger(string modStaticId);

		/// <summary>
		/// A global singleton registry for mods to interop and share data through.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		[GetOnce]
		IRegistry GlobalRegistry
		{ get; }

		/// <summary>
		/// Returns the singleton registry created for the mod with a specific modStaticId.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		IRegistry GetModRegistry(string modStaticId);
	}
}
