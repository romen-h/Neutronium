using Neutronium.Api.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Api.Logging
{
	[StableApi(ApiVersions.Alpha_Milestone1)]
	[WrapInterface]
	public interface ILoggerFactory
	{
		[StableApi(ApiVersions.Alpha_Milestone1)]
		ILogger GetLogger(string modStaticId, string category = null);
	}
}
