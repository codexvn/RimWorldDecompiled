using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using Waylith.Assets;
using Waylith.Utils;

namespace UnityStubs;

public static class ContentPacks
{
	public const string PackFileName = "Assets.zip";

	private static readonly HashSet<string> Mounted = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

	public static void MountLoadFolder(string folder)
	{
		if (string.IsNullOrEmpty(folder))
		{
			return;
		}
		string text = Path.TrimEndingDirectorySeparator(Path.GetFullPath(folder));
		lock (Mounted)
		{
			if (!Mounted.Add(text))
			{
				return;
			}
		}
		string text2 = Path.Combine(text, "Assets.zip");
		AssetPack assetPack = null;
		try
		{
			assetPack = AssetPack.TryOpen(text2);
			if (assetPack != null)
			{
				if (assetPack.ContentHashes.Count == 0)
				{
					Debug.LogWarning("Asset pack " + text2 + " has no manifest, none of its entries will be visible");
				}
				ContentPack.Mount(text, assetPack, AssetPackManifest.ReadStamps(text2));
			}
		}
		catch (Exception ex)
		{
			assetPack?.Dispose();
			Debug.LogWarning("Asset pack " + text2 + " could not be opened and will be ignored: " + ex.Message);
		}
	}

	public static void Unmount(string folder)
	{
		if (!string.IsNullOrEmpty(folder))
		{
			string text = Path.TrimEndingDirectorySeparator(Path.GetFullPath(folder));
			lock (Mounted)
			{
				Mounted.Remove(text);
			}
			ContentPack.Unmount(text);
		}
	}

	public static bool IsPackSidecar(string fullPath)
	{
		string fileName = Path.GetFileName(fullPath);
		if (fileName.Length > "Assets.zip".Length)
		{
			return fileName.StartsWith("Assets.zip.", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public static bool IsPackedSource(string fullPath)
	{
		if (ContentPack.HasFile(fullPath))
		{
			return true;
		}
		InlineArray3<string> buffer = default(InlineArray3<string>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<string>, string>(ref buffer, 0) = ".dds";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<string>, string>(ref buffer, 1) = "-premul.dds";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<string>, string>(ref buffer, 2) = ".nbmesh";
		ReadOnlySpan<string> readOnlySpan = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray3<string>, string>(in buffer, 3);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			string suffix = readOnlySpan[i];
			if (ContentPack.TryGetEntryName(fullPath, suffix, out AssetPack pack, out string name) && pack.Contains(name))
			{
				return true;
			}
		}
		return false;
	}

	public static IEnumerable<string> EnumerateFiles(string directory, bool recursive)
	{
		return ResourceFiles.EnumerateFiles(directory, recursive);
	}

	public static bool FileExists(string path)
	{
		return ResourceFiles.FileExists(path);
	}

	public static bool DirectoryExists(string path)
	{
		return ResourceFiles.DirectoryExists(path);
	}
}
