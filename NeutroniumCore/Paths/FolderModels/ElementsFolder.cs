using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Neutronium.Core.Paths.FolderModels
{
	public class ElementsFolder : Folder
	{
		public readonly string SolidYaml;
		public readonly string LiquidYaml;
		public readonly string GasYaml;
		public readonly string SpecialYaml;
		
		internal ElementsFolder(string path) : base(path)
		{
			SolidYaml = Path.Combine(path, "solid.yaml");
			LiquidYaml = Path.Combine(path, "liquid.yaml");
			GasYaml = Path.Combine(path, "gas.yaml");
			SpecialYaml = Path.Combine(path, "special.yaml");
		}
	}
}
