using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Paths.Api.FolderModels
{
	public class GameFolder : Folder
	{
		/// <summary>
		/// The path to the OxygenNotIncluded_Data folder.
		/// </summary>
		public readonly GameDataFolder OxygenNotIncluded_Data;

		/// <summary>
		/// The path to the game executable file.
		/// </summary>
		public readonly string GameExecutable;
		
		internal GameFolder(string path) : base(path)
		{
			OxygenNotIncluded_Data = new GameDataFolder(Path.Combine(path, "OxygenNotIncluded_Data"));
			
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				GameExecutable = Path.Combine(path, "OxygenNotIncluded.exe");
			}
			else
			{
				GameExecutable = Path.Combine(path, "OxygenNotIncluded");
			}
		}
	}
}
