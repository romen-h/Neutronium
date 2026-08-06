using Neutronium.MergeLib.Api;

namespace NeutroniumApi.MergeLib.Tests;

[TestClass]
public class ApiRootTests
{
	private static IApiRoot api;
	
	[ClassInitialize]
	public static void Init(TestContext ctx)
	{
		api = NeutroniumCoreSetup.Mod.GetApi();
	}
	
	[TestMethod]
	public void ApiExists()
	{
		Assert.IsTrue(NeutroniumCoreSetup.Mod.IsNeutroniumInitialized);
		Assert.IsNotNull(api);
	}
}