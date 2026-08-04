using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Plugins.Api
{
	[StableApi(ApiVersions.Alpha_Milestone2)]
	public interface IDbInitializerPlugin : IPlugin
	{
		void BeforeDbInitialized();
		
		void AfterDbInitialized();
		
		void AfterDbPostProcess();
	}
}
