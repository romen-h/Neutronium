using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api;
using Neutronium.Api.Logging;
using Neutronium.Api.Meta;
using Neutronium.Api.Registry;
using Neutronium.Core.Logging.Api;
using Neutronium.Core.Registry;

namespace Neutronium.Core.Api
{
	[StableApi(ApiVersions.NextReleaseVersion)]
	public class ApiRoot : IApiRoot
	{
		private static readonly ILogger s_log = Neutronium.Core.Logging.Api.LoggerFactory.GetInternalLogger("Core.ApiRoot");

		[StableApi(ApiVersions.NextReleaseVersion)]
		public static ApiRoot Instance
		{ get; private set; }
		
		internal static void Initialize()
		{
			s_log.Info("Initializing...");
			Instance = new ApiRoot();
		}
		
		private ApiRoot()
		{ }
		
		public Version NeutroniumVersion => Main.NeutroniumVersion;
		
		public ILoggerFactory LoggerFactory
		{ get; private set; } = new LoggerFactory();
		
		public ILogger GetModLogger(string modStaticId) => LoggerFactory.GetLogger(modStaticId, null);
		
		public IRegistry GlobalRegistry => RegistryManager.GlobalRegistry;
		
		public IRegistry GetModRegistry(string modStaticId) => RegistryManager.GetModRegistry(modStaticId);
	}
}
