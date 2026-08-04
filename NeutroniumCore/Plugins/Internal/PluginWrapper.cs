using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Neutronium.Core.Plugins.Api;

namespace Neutronium.Core.Plugins.Internal
{
	internal class PluginWrapper
	{
		internal readonly IPlugin Plugin;
		internal readonly string AssemblyPath;
		internal readonly string ModStaticId;
		internal readonly string Id;

        private readonly IAssetsLoaderPlugin _assetsLoader;
        internal bool IsAssetsPlugin => _assetsLoader != null;

        private readonly Harmony _harmony;
		private readonly IPatcherPlugin _patcher;
		internal bool IsPatcher => _harmony != null && _patcher != null;
		
		private readonly IDbInitializerPlugin _dbInitializer;
		internal bool IsDbInitializer => _dbInitializer != null;
        
		internal PluginWrapper(IPlugin plugin, string assemblyPath, string modStaticId)
		{
			Plugin = plugin;
			AssemblyPath = assemblyPath;
			ModStaticId = modStaticId;
			Id = $"{modStaticId}:{plugin.UniqueID}";

            if (Plugin is IAssetsLoaderPlugin assetsLoader)
            {
                _assetsLoader = assetsLoader;
            }

			if (Plugin is IPatcherPlugin patcher)
            {
				_harmony = new Harmony(ModStaticId);
				_patcher = patcher;
            }
			
			if (Plugin is IDbInitializerPlugin dbInitializer)
			{
				_dbInitializer = dbInitializer;
			}
		}
		
		internal void LoadAssets()
        {
			if (_assetsLoader == null) return;
			_assetsLoader.LoadAssets();;
        }
		
		internal void ApplyPatches()
        {
			if (_harmony == null || _patcher == null) return;
			_patcher.ApplyPatches(_harmony);
        }
		
		internal void BeforeDbInitialized()
		{
			if (_dbInitializer == null) return;
			_dbInitializer.BeforeDbInitialized();
		}
		
		internal void AfterDbInitialized()
		{
			if (_dbInitializer == null) return;
			_dbInitializer.AfterDbInitialized();
		}
		
		internal void AfterDbPostProcess()
		{
			if (_dbInitializer == null) return;
			_dbInitializer.AfterDbPostProcess();
		}
	}
}
