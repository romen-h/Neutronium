namespace NeutroniumApi.MergeLib.Tests
{
	[TestClass]
	public sealed class NeutroniumCoreSetup
	{
		[AssemblyInitialize]
		public static void AssemblyInit(TestContext context)
		{
			// This method is called once for the test assembly, before any tests are run.
			Doorstop.Entrypoint.Test();
		}
	}
}
