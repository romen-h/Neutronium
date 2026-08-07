using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Elements;
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
	public interface IApiRoot
	{
		/// <summary>
		/// The version of Neutronium that is currently loaded.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		[GetOnce]
		Version NeutroniumVersion
		{ get; }

		/// <summary>
		/// API for interacting with the Neutronium log.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		[GetOnce]
		ILoggingApi Logging
		{ get; }

		/// <summary>
		/// API for interacting with the Neutronium registry.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		[GetOnce]
		IRegistryApi Registry
		{ get; }
		
		/// <summary>
		/// API for creating and getting modded elements.
		/// </summary>
		[PreviewApi]
		[GetOnce]
		IElementsApi Elements
		{ get; }
	}
}
