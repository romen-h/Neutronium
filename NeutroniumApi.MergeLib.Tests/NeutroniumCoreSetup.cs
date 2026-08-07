using Neutronium.MergeLib;
using Neutronium.MergeLib.Api;

namespace NeutroniumApi.MergeLib.Tests
{
	[TestClass]
	public sealed class NeutroniumCoreSetup
	{
		public static IApiRoot? Api
		{ get; private set; }

		[AssemblyInitialize]
		public static void AssemblyInit(TestContext context)
		{
			// This method is called once for the test assembly, before any tests are run.
			Doorstop.Entrypoint.Test();

			Api = NeutroniumApiClient.GetApi("NeutroniumApi.MergeLib.Tests");
		}
	}
}
