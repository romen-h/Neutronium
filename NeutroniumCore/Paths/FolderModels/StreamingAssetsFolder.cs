using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Neutronium.Core.Paths.FolderModels
{
	public class StreamingAssetsFolder : Folder
	{
		public readonly ElementsFolder Elements;
		
		internal StreamingAssetsFolder(string path) : base(path)
		{
			Elements = new ElementsFolder(Path.Combine(path, "elements"));
		}
	}
}
