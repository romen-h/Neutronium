using Neutronium.MergeLib.Api;

namespace NeutroniumApi.MergeLib.Tests;

[TestClass]
public class LoggingTests
{
	private static IApiRoot api;

	[ClassInitialize]
	public static void Init(TestContext ctx)
	{
		api = NeutroniumCoreSetup.Api;
	}

	[TestMethod]
	public void GetModLogger()
	{
		var logger = api.Logging.GetModLogger();
		Assert.IsNotNull(logger);
		Assert.IsTrue(logger.Enabled);
	}
	
	[TestMethod]
	public void DebugLog()
	{
		var logger = api.Logging.GetModLogger("DebugLogTest");
		
		logger.Debug("Debug message.");
	}

	[TestMethod]
	public void InfoLog()
	{
		var logger = api.Logging.GetModLogger("InfoLogTest");

		logger.Info("Info message.");
	}
	
	[TestMethod]
	public void WarnLog()
	{
		var logger = api.Logging.GetModLogger("WarnLogTest");

		logger.Warn("Warning message.");
	}

	[TestMethod]
	public void ErrorLog()
	{
		var logger = api.Logging.GetModLogger("ErrorLogTest");

		logger.Error("Error message.");
	}

	[TestMethod]
	public void ErrorLogWithException()
	{
		var logger = api.Logging.GetModLogger("ExceptionLogTest");

		logger.Error("Exception message.", new Exception("Example Exception"));
	}
}