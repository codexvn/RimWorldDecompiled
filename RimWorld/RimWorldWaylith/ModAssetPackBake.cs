using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using UnityStubs;
using Verse;
using Waylith.AssetBake;
using Waylith.Logging;

namespace RimWorldWaylith;

internal static class ModAssetPackBake
{
	public static bool Requested(out List<string> modRoots)
	{
		modRoots = null;
		if (!GenCommandLine.TryGetCommandLineArg("BakeModAssetPacks", out var value) || string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		modRoots = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(Path.GetFullPath).ToList();
		return modRoots.Count > 0;
	}

	public static int Run(List<string> modRoots)
	{
		bool flag = false;
		foreach (string modRoot in modRoots)
		{
			try
			{
				flag |= !BakeMod(modRoot);
			}
			catch (Exception value)
			{
				Log.Error($"BakeModAssetPacks: {modRoot}: {value}");
				flag = true;
			}
		}
		return flag ? 1 : 0;
	}

	private static bool BakeMod(string modRoot)
	{
		if (!Directory.Exists(modRoot))
		{
			Log.Error("BakeModAssetPacks: mod folder not found: " + modRoot);
			return false;
		}
		List<string> list = ModContentPack.AllLoadFolders(modRoot, LoadFolders(modRoot)).Where(ModAssetPacks.HasBakeableContent).ToList();
		if (list.Count == 0)
		{
			Log.Message("BakeModAssetPacks: " + modRoot + " has no load folder with textures or sounds, nothing to bake");
			return true;
		}
		bool flag = true;
		foreach (string item in list)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			ContentBakeResult contentBakeResult = ModAssetPacks.BakeForUpload(item, delegate(string file)
			{
				VerboseLog.Log("BakeModAssetPacks: " + file);
			});
			foreach (string error in contentBakeResult.Errors)
			{
				Log.Error("BakeModAssetPacks: " + item + ": " + error);
			}
			Log.Message($"BakeModAssetPacks: {item}: {contentBakeResult.Entries} entries ({contentBakeResult.Reused} reused, {contentBakeResult.Deduplicated} deduplicated, {contentBakeResult.Failed} failed) in {stopwatch.Elapsed.TotalSeconds:F1}s{(contentBakeResult.Unchanged ? ", unchanged" : "")}");
			flag &= contentBakeResult.Succeeded && contentBakeResult.Failed == 0;
		}
		return flag;
	}

	private static ModLoadFolders LoadFolders(string modRoot)
	{
		ModLoadFolders modLoadFolders = new ModLoadFolders();
		string text = GenFile.ResolveCaseInsensitiveFilePath(modRoot, "LoadFolders.xml");
		if (!File.Exists(text))
		{
			return modLoadFolders;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(text);
		modLoadFolders.LoadDataFromXmlCustom(xmlDocument.DocumentElement);
		return modLoadFolders;
	}
}
