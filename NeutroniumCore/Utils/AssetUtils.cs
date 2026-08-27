using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Neutronium.Core.Logging;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using ILogger = Neutronium.Api.Logging.ILogger;

namespace Neutronium.Core.Utils
{
	internal static class AssetUtils
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.Utils.Assets");
		
		internal static TextAsset? LoadTextAsset(string textFilePath)
		{
			if (textFilePath == null) throw new ArgumentNullException(nameof(textFilePath));

			try
			{
				byte[] bytes = File.ReadAllBytes(textFilePath);
				return new TextAsset(bytes);
			}
			catch (Exception ex)
			{
				s_log.Error($"Failed to create TextAsset.\nFile: {textFilePath}", ex);
				return null;
			}
		}
		
		internal static Texture? LoadTexture(string textureFilePath)
		{
			if (textureFilePath == null) throw new ArgumentNullException(nameof(textureFilePath));

			try
			{
				byte[] bytes = File.ReadAllBytes(textureFilePath);
				Texture2D texture = new Texture2D(2, 2);
				if (!texture.LoadImage(bytes)) throw new Exception("Failed to load texture data.");
				return texture;
			}
			catch (Exception ex)
			{
				s_log.Error($"Failed to create Texture.\nFile: {textureFilePath}", ex);
				return null;
			}
		}
	}
}
