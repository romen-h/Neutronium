using Neutronium.MergeLib.Api;

namespace NeutroniumApi.MergeLib.Tests;

[TestClass]
public class ApiRootTests
{
	private static IApiRoot api;
	
	[ClassInitialize]
	public static void Init(TestContext ctx)
	{
		api = NeutroniumCoreSetup.Api;
	}
	
	[TestMethod]
	public void ApiExists()
	{
		Assert.IsNotNull(api);
	}
}