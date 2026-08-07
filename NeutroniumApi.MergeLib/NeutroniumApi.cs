using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Neutronium.MergeLib.Api;
using Neutronium.MergeLib.Internal;
using UnityEngine;

namespace Neutronium.MergeLib
{
	public class NeutroniumApiClient
	{
		public static IApiRoot? GetApi(string modStaticId)
		{
			UnityEngine.ILogger? unityLogger = null;
			try
			{
				unityLogger = Debug.unityLogger;
			}
			catch
			{ }

			Type? remoteApiRoot = RemoteTypes.FindType("Neutronium.Core.ApiRoot");
			if (remoteApiRoot == null)
			{
				unityLogger?.LogError($"NeutroniumApi.MergeLib:{modStaticId}", "Could not find ApiRoot class in NeutroniumCore.");
				return null;
			}

			MethodInfo? getApiMethod = remoteApiRoot?.GetMethod("GetApi", BindingFlags.Public | BindingFlags.Static, null, [typeof(string)], null);
			if (getApiMethod == null)
			{
				unityLogger?.LogError($"NeutroniumApi.MergeLib:{modStaticId}", "Could not find ApiRoot.GetApi method in NeutroniumCore.");
				return null;
			}

			object? apiRoot = null;
			try
			{
				apiRoot = getApiMethod.Invoke(null, [modStaticId]);
			}
			catch (Exception ex)
			{
				unityLogger?.LogError($"NeutroniumApi.MergeLib:{modStaticId}", "Error while trying to get API from NeutroniumCore.");
				unityLogger?.LogException(ex);
				return null;
			}

			return IApiRoot_Wrapper.Wrap(apiRoot);
		}
	}
}
