using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Paths.Api.FolderModels
{
    [StableApi(ApiVersions.Alpha_Milestone1)]
    public class RootFolder : Folder
    {
        [StableApi((ApiVersions.Alpha_Milestone1))]
        public readonly ModsFolder Mods;

		[StableApi(ApiVersions.Alpha_Milestone1)]
		public readonly string KPlayerPrefs;
        
        internal readonly string MovedFlag;
        
        internal RootFolder(string path) : base(path)
        {
            Mods = new ModsFolder(Path.Combine(path, "mods"));
            KPlayerPrefs = Path.Combine(path, "kplayerprefs.yaml");
            MovedFlag = Path.Combine(path, "OneDrive Fix.txt");
        }
    }
}
