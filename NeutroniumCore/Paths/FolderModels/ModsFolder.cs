using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Paths.FolderModels
{
    public class ModsFolder : Folder
    {
        public readonly Folder Dev;
        
        public readonly Folder Local;
        
        public readonly Folder Steam;
        
        internal ModsFolder(string path) : base(path)
        {
            Dev = new Folder(Path.Combine(path, "Dev"));
            Local = new Folder(Path.Combine(path, "Local"));
            Steam = new Folder(Path.Combine(path, "Steam"));
        }
    }
}
