using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Plugins
{
	/// <summary>
	/// Plugins that implement this interface will be given a chance to apply harmony patches to the game.
	/// </summary>
	[StableApi(ApiVersions.NextReleaseVersion)]
	public interface IPatcherPlugin : IPlugin
	{
		/// <summary>
		/// Implement the Harmony patching in this method.
		/// Exceptions thrown by this method will be caught and raise a warning to the user that they should disable this plugin.
		/// </summary>
		/// <param name="harmonyId">The id that should be used to create your own Harmony instance.</param>
		void ApplyPatches(string harmonyId);
	}
}
