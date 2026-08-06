using Neutronium.Core;

namespace Doorstop
{
	/// <summary>
	/// This class is the entry point that Doorstop uses to load Neutronium before the game launches.
	/// </summary>
	public class Entrypoint
	{
		public static void Start()
		{
			Main.OnEntrypoint();
		}
		
		public static void Test()
		{
			Main.OnTest();
		}
	}
}
