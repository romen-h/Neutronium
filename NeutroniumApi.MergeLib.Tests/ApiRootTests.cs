using Neutronium.MergeLib.Api;

namespace NeutroniumApi.MergeLib.Tests;

[TestClass]
public class ApiRootTests
{
	private static IApiRoot api;
	
	[ClassInitialize]
	public static void Init(TestContext ctx)
	{
		NeutroniumMod mod = new NeutroniumMod("test");
		api = mod.GetApi();
	}
	
	[TestMethod]
	public void ApiExists()
	{
		Assert.IsNotNull(api);
	}
	
	[TestMethod]
	public void GetLoggerFactoryTwice()
	{
		var loggerFactory1 = api.LoggerFactory;
		var loggerFactory2 = api.LoggerFactory;
		Assert.IsTrue(Object.ReferenceEquals(loggerFactory1, loggerFactory2));
	}
	
	[TestMethod]
	public void GetModLogger()
	{
		var logger = api.GetModLogger("test");
		Assert.IsNotNull(logger);
		Assert.IsTrue(logger.Enabled);
	}
}