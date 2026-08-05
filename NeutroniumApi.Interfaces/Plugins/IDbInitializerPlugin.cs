using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Plugins
{
	[StableApi(ApiVersions.Alpha_Milestone1)]
	public interface IDbInitializerPlugin : IPlugin
	{
		[StableApi(ApiVersions.Alpha_Milestone1)]
		void BeforeDbInitialized();

		[StableApi(ApiVersions.Alpha_Milestone1)]
		void AfterDbInitialized();

		[StableApi(ApiVersions.Alpha_Milestone1)]
		void AfterDbPostProcess();
	}
}
