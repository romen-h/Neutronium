using Neutronium.MergeLib.Api;

namespace NeutroniumApi.MergeLib.Tests;

[TestClass]
public class LoggingTests
{
	private static IApiRoot api;

	[ClassInitialize]
	public static void Init(TestContext ctx)
	{
		api = NeutroniumCoreSetup.Mod.GetApi();
	}

	[TestMethod]
	public void GetLoggerFactoryTwice()
	{
		var loggerFactory1 = api.LoggerFactory;
		var loggerFactory2 = api.LoggerFactory;
		Assert.IsTrue(Object.ReferenceEquals(loggerFactory1, loggerFactory2));
	}
	
	[TestMethod]
	public void GetCustomLogger()
	{
		var loggerFactory = api.LoggerFactory;
		Assert.IsNotNull(loggerFactory);
		
		var customLogger = loggerFactory.GetLogger("test", null);
		Assert.IsNotNull(customLogger);
		Assert.IsTrue(customLogger.Enabled);
	}

	[TestMethod]
	public void GetModLogger()
	{
		var logger = api.GetModLogger("test");
		Assert.IsNotNull(logger);
		Assert.IsTrue(logger.Enabled);
	}
	
	[TestMethod]
	public void DebugLog()
	{
		var logger = api.GetModLogger("NeutroniumApi.MergeLib.Tests");
		
		logger.Debug("Debug message.");
	}

	[TestMethod]
	public void InfoLog()
	{
		var logger = api.GetModLogger("NeutroniumApi.MergeLib.Tests");

		logger.Info("Info message.");
	}
	
	[TestMethod]
	public void WarnLog()
	{
		var logger = api.GetModLogger("NeutroniumApi.MergeLib.Tests");

		logger.Warn("Warning message.");
	}

	[TestMethod]
	public void ErrorLog()
	{
		var logger = api.GetModLogger("NeutroniumApi.MergeLib.Tests");

		logger.Error("Error message.");
	}

	[TestMethod]
	public void ErrorLogWithException()
	{
		var logger = api.GetModLogger("NeutroniumApi.MergeLib.Tests");

		logger.Error("Exception message.", new Exception("Example Exception"));
	}
}