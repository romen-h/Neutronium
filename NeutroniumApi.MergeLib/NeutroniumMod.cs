using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Neutronium.MergeLib.Internal;

namespace Neutronium.MergeLib.Api
{
	public class NeutroniumMod
	{
		private static bool s_initialized = false;
		private static NeutroniumMod? s_instance = null;
		
		private readonly string _modStaticId;
		
		private IApiRoot _apiRoot = null;
		
		public bool IsNeutroniumInitialized
		{ get; private set; } = false;
		
		public NeutroniumMod(string modStaticId)
		{
			if (s_instance != null) throw new InvalidOperationException("Do not create more than one instance of NeutroniumMod.");
			s_instance = this;
			InitializeApi();
		}
		
		private void InitializeApi()
		{
			Type? remoteApiRoot = RemoteTypes.FindType("Neutronium.Core.Api.ApiRoot");
			if (remoteApiRoot == null) return;
			
			PropertyInfo? instanceProperty = remoteApiRoot?.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
			if (instanceProperty == null) return;
			
			object? instance = instanceProperty.GetValue(null);

			_apiRoot = IApiRoot_Wrapper.Wrap(instance);
			
			IsNeutroniumInitialized = _apiRoot != null;
		}
		
		public IApiRoot GetApi() => _apiRoot;
	}
}
