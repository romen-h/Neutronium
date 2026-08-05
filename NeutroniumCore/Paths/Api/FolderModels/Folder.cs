using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Neutronium.Core.Logging;
using Neutronium.Core.Meta;

namespace Neutronium.Core.Paths.Api.FolderModels
{
    public class Folder
    {
        protected readonly string _path;

		public override string ToString() => _path;

		public static implicit operator string(Folder folder) => folder._path;
        
        internal Folder(string path)
        {
            _path = path;
        }
        
        internal void CopyTo(string dest)
        {
            foreach (string subFolder in Directory.GetDirectories(_path, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(subFolder.Replace(_path, dest));
            }
            
            foreach (string file in Directory.GetFiles(_path, "*", SearchOption.AllDirectories))
            {
	            try
	            {
		            File.Copy(file, file.Replace(_path, dest), true);
				}
	            catch (Exception ex)
	            {
                    Log.Error("Core.FilePaths", $"Failed to copy file: {file}", ex);
	            }
            }
        }
    }
}
