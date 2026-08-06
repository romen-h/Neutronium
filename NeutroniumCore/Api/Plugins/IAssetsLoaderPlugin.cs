using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Plugins
{
    /// <summary>
    /// Plugins that implement this interface will be given a chance to load assets before plugin patching.
    /// </summary>
    [StableApi(ApiVersions.NextReleaseVersion)]
    public interface IAssetsLoaderPlugin : IPlugin
    {
		/// <summary>
		/// Implement the asset loading in this method.
		/// Exceptions thrown by this method will be caught and raise a warning to the user that they should disable this plugin.
		/// </summary>
		[StableApi(ApiVersions.NextReleaseVersion)]
        void LoadAssets();
    }
}
