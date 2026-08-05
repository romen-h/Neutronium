using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Plugins
{
	internal class PluginFile
	{
		internal readonly string FilePath;

		internal readonly string Platform;

		internal readonly string ModStaticId;

		internal PluginFile(string platform, string modStaticId, string filePath)
		{
			if (string.IsNullOrWhiteSpace(platform)) throw new ArgumentNullException(nameof(platform));
			if (string.IsNullOrWhiteSpace(modStaticId)) throw new ArgumentNullException(nameof(modStaticId));
			if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
			
			Platform = platform;
			ModStaticId = modStaticId;
			FilePath = filePath;
		}
	}
}
