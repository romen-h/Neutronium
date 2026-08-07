using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Registry;

namespace Neutronium.Core.Registry
{
	internal class RegistryApi : IRegistryApi
	{
		private readonly string _modId;
		
		internal RegistryApi(string modId)
		{
			_modId = modId;
		}
		
		public IRegistry GlobalRegistry => RegistryManager.GlobalRegistry;

		public IRegistry GetMyModRegistry() => RegistryManager.GetModRegistry(_modId);
		
		public IRegistry GetOtherModRegistry(string modStaticId) => RegistryManager.GetModRegistry(modStaticId);
	}
}
