using Neutronium.Core.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Neutronium.Core.Paths.Api.FolderModels
{
	public class GameDataFolder : Folder
	{
		public readonly Folder Managed;
		public readonly Folder Plugins;
		public readonly Folder Resources;
		public readonly Folder StreamingAssets;
		
		internal GameDataFolder(string path) : base(path)
		{
			Managed = new Folder(Path.Combine(path, "Managed"));
			Plugins = new Folder(Path.Combine(path, "Plugins"));
			Resources = new Folder(Path.Combine(path, "Resources"));
			StreamingAssets = new Folder(Path.Combine(path, "StreamingAssets"));
		}
	}
}
