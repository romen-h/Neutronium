using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Plugins
{
	[StableApi(ApiVersions.NextReleaseVersion)]
	public interface IDbInitializerPlugin : IPlugin
	{
		[StableApi(ApiVersions.NextReleaseVersion)]
		void BeforeDbInitialized();

		[StableApi(ApiVersions.NextReleaseVersion)]
		void AfterDbInitialized();

		[StableApi(ApiVersions.NextReleaseVersion)]
		void AfterDbPostProcess();
	}
}
