using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Registry
{
	[StableApi(ApiVersions.NextReleaseVersion)]
	public interface IRegistryApi
	{
		/// <summary>
		/// A global singleton registry for mods to interop and share data through.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		[GetOnce]
		IRegistry GlobalRegistry
		{ get; }

		/// <summary>
		/// Returns the singleton registry created for this mod.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		IRegistry GetMyModRegistry();
		
		/// <summary>
		/// Returns the singleton registry created for another mod with the given static ID.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
		IRegistry GetOtherModRegistry(string modStaticId);
	}
}
