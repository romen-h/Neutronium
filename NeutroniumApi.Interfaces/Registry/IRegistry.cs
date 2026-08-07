using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Registry
{
	[StableApi(ApiVersions.NextReleaseVersion)]
	public interface IRegistry
	{
		[StableApi(ApiVersions.NextReleaseVersion)]
		bool ContainsKey(string key);
		
		[StableApi(ApiVersions.NextReleaseVersion)]
		object GetValue(string key);

		//[StableApi(ApiVersions.NextReleaseVersion)]
		//bool TryGetValue(string key, out object value);

		[StableApi(ApiVersions.NextReleaseVersion)]
		void SetValue(string key, object value);
	}
}
