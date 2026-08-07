using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api;
using Neutronium.Api.Elements;
using Neutronium.Api.Logging;
using Neutronium.Api.Meta;
using Neutronium.Api.Registry;
using Neutronium.Core.Elements;
using Neutronium.Core.Logging;
using Neutronium.Core.Registry;

namespace Neutronium.Core
{
	public class ApiRoot : IApiRoot
	{
		private readonly string _modId;
		
		public static IApiRoot GetApi(string modStaticId) => new ApiRoot(modStaticId);
		
		internal ApiRoot(string modId)
		{
			_modId = modId;
			Logging = new LoggingApi(modId);
			Registry = new RegistryApi(modId);
			Elements = new ElementsApi(modId);
		}
		
		public Version NeutroniumVersion => Main.NeutroniumVersion;

		public ILoggingApi Logging
		{ get; }

		public IRegistryApi Registry
		{ get; }
		
		public IElementsApi Elements
		{ get; }
	}
}
