using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Registry
{
	[StableApi(ApiVersions.Alpha_Milestone1)]
	[WrapInterface]
	public interface IRegistry
	{
		[StableApi(ApiVersions.Alpha_Milestone1)]
		bool KeyExists(string key);
		
		[StableApi(ApiVersions.Alpha_Milestone1)]
		object GetValue(string key);

		[StableApi(ApiVersions.Alpha_Milestone1)]
		bool TryGetValue(string key, out object value);

		[StableApi(ApiVersions.Alpha_Milestone1)]
		void SetValue(string key, object value);
	}
}
