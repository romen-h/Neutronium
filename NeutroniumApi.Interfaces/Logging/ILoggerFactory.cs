using Neutronium.Api.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Api.Logging
{
	[StableApi(ApiVersions.NextReleaseVersion)]
	[WrapInterface]
	public interface ILoggerFactory
	{
		[StableApi(ApiVersions.NextReleaseVersion)]
		ILogger GetLogger(string modStaticId, string category = null);
	}
}
