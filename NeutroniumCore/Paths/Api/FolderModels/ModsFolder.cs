using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Paths.Api.FolderModels
{
    [StableApi(ApiVersions.Alpha_Milestone1)]
    public class ModsFolder : Folder
    {
        [StableApi(ApiVersions.Alpha_Milestone1)]
        public readonly Folder Dev;
        
        [StableApi(ApiVersions.Alpha_Milestone1)]
        public readonly Folder Local;
        
        [StableApi(ApiVersions.Alpha_Milestone1)]
        public readonly Folder Steam;
        
        internal ModsFolder(string path) : base(path)
        {
            Dev = new Folder(Path.Combine(path, "Dev"));
            Local = new Folder(Path.Combine(path, "Local"));
            Steam = new Folder(Path.Combine(path, "Steam"));
        }
    }
}
