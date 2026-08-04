using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Registry.Interfaces
{
	[StableApi(ApiVersions.Alpha_Milestone2)]
	public interface IRegistry
	{
		[StableApi(ApiVersions.Alpha_Milestone2)]
		bool KeyExists(string key);
		
		[StableApi(ApiVersions.Alpha_Milestone2)]
		object GetValue(string key);

		[StableApi(ApiVersions.Alpha_Milestone2)]
		bool TryGetValue(string key, out object value);

		[StableApi(ApiVersions.Alpha_Milestone2)]
		void SetValue(string key, object value);
	}
}
