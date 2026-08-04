using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Plugins.Api
{
	/// <summary>
	/// Plugins that implement this interface will be given a chance to apply harmony patches to the game.
	/// </summary>
	[StableApi(ApiVersions.Alpha_Milestone2)]
	public interface IPatcherPlugin : IPlugin
	{
		/// <summary>
		/// Implement the Harmony patching in this method.
		/// Exceptions thrown by this method will be caught and raise a warning to the user that they should disable this plugin.
		/// </summary>
		void ApplyPatches(Harmony harmony);
	}
}
