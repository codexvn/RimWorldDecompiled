using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Waylith.Rendering;

namespace UnityStubs;

public static class ImportDefaults
{
	private static string[] _officialRoots;

	private static Func<string[]> _officialRootsProvider;

	private static readonly List<string> BorderMipMapTextures = new List<string>();

	public static void SetOfficialRootsProvider(Func<string[]> provider)
	{
		_officialRootsProvider = provider;
		_officialRoots = null;
	}

	public static bool IsOfficial(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		if (_officialRoots == null)
		{
			string[] array = _officialRootsProvider?.Invoke();
			if (array == null)
			{
				return false;
			}
			_officialRoots = Array.ConvertAll(array, Normalize);
		}
		string text = Normalize(path);
		string[] officialRoots = _officialRoots;
		foreach (string value in officialRoots)
		{
			if (text.StartsWith(value, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsOfficialUiTexture(string path)
	{
		if (path != null && (path.Contains("/Textures/UI/", StringComparison.OrdinalIgnoreCase) || path.Contains("\\Textures\\UI\\", StringComparison.OrdinalIgnoreCase)))
		{
			return IsOfficial(path);
		}
		return false;
	}

	public static void BorderMipMaps(string texPath)
	{
		if (string.IsNullOrEmpty(texPath))
		{
			return;
		}
		string text = "/Textures/" + texPath.Replace('\\', '/').Trim('/');
		lock (BorderMipMapTextures)
		{
			if (!BorderMipMapTextures.Contains<string>(text, StringComparer.OrdinalIgnoreCase))
			{
				BorderMipMapTextures.Add(text);
			}
		}
		if (GpuTextureService.WantsBorderMipMaps == null)
		{
			GpuTextureService.WantsBorderMipMaps = WantsBorderMipMaps;
		}
	}

	private static bool WantsBorderMipMaps(string fullPath)
	{
		string text = fullPath.Replace('\\', '/');
		string text2 = text;
		int length = Path.GetExtension(text).Length;
		text = text2.Substring(0, text2.Length - length);
		lock (BorderMipMapTextures)
		{
			foreach (string borderMipMapTexture in BorderMipMapTextures)
			{
				if (text.EndsWith(borderMipMapTexture, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void KeepStereo(AudioClip clip)
	{
		clip?.KeepStereo();
	}

	private static string Normalize(string path)
	{
		string text = Path.GetFullPath(path).Replace('\\', '/');
		if (!text.EndsWith('/'))
		{
			return text + "/";
		}
		return text;
	}
}
