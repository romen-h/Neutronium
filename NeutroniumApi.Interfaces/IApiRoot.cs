using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Logging;
using Neutronium.Api.Meta;

namespace Neutronium.Api
{
	[StableApi(ApiVersions.Alpha_Milestone1)]
	[WrapInterface]
	public interface IApiRoot
	{
		ILoggerFactory LoggerFactory
		{ get; }
	}
}
