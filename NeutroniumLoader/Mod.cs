using HarmonyLib;
using KMod;
using System;
using UnityEngine;

namespace Neutronium.Loader
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			try
			{
				Bootstrapper bs;
				
				if (Application.platform == RuntimePlatform.WindowsPlayer)
				{
					bs = new WindowsBootstrapper();
				}
				else if (Application.platform == RuntimePlatform.LinuxPlayer)
				{
					throw new NotImplementedException("LinuxDoorstopBootstrapper not supported yet.");
				}
				else
				{
					throw new Exception($"Platform {Application.platform} is not supported by Neutronium.");
				}
				
				bs.Run();
			}
			catch (Exception ex)
			{
				Debug.LogError("NeutroniumLoader: Error while bootstrapping Doorstop.");
				Debug.LogException(ex);
			}

			base.OnLoad(harmony);
		}
	}
}
