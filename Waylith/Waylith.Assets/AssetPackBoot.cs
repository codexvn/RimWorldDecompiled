using System;
using System.Diagnostics;
using System.IO;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.CLI;
using Waylith.Logging;
using Waylith.Rendering;

namespace Waylith.Assets;

public static class AssetPackBoot
{
	public static AssetPack? Open(string dataPath, ICommandLine commandLine)
	{
		if (commandLine.HasSwitch("NoAssetPack"))
		{
			return null;
		}
		string text = commandLine.GetFollowingValue("AssetPack") ?? Path.Combine(AppContext.BaseDirectory, "Assets.zip");
		bool shouldAppend;
		try
		{
			AssetPack assetPack = AssetPack.TryOpen(text);
			if (assetPack == null)
			{
				return null;
			}
			if (assetPack.ContentHashes.Count == 0)
			{
				LogCategory log = RenderLog.Log;
				LogCategory logCategory = log;
				LogString<Warning> message = new LogString<Warning>(68, 2, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Asset pack '");
					message.AppendFormatted(text);
					message.AppendLiteral("' has no manifest, none of its ");
					message.AppendFormatted(assetPack.Count);
					message.AppendLiteral(" entries will be visible.");
				}
				logCategory.Warning(message, "Open", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Assets\\AssetPackBoot.cs", 28);
			}
			ContentPack.Use(assetPack, dataPath, AssetPackManifest.ReadStamps(text));
			RuntimeShaderCompiler.UseAssetPack(assetPack);
			AssetBakeHooks.BootComplete += LogUsage;
			VerboseLog.Log($"Asset pack: {assetPack.Count} entries from '{text}'.");
			return assetPack;
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			LogCategory logCategory2 = log;
			LogString<Error> message2 = new LogString<Error>(30, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("Failed to open asset pack '");
				message2.AppendFormatted(text);
				message2.AppendLiteral("': ");
				message2.AppendFormatted(ex.Message);
			}
			logCategory2.Error(message2, "Open", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Assets\\AssetPackBoot.cs", 38);
			return null;
		}
	}

	public static void LogUsage()
	{
		if (ContentPack.Pack == null)
		{
			return;
		}
		if (AssetPack.CorruptEntries > 0)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(88, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Asset packs: ");
				message.AppendFormatted(AssetPack.CorruptEntries);
				message.AppendLiteral(" damaged entries were skipped, verifying the game files should repair them.");
			}
			log.Warning(message, "LogUsage", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Assets\\AssetPackBoot.cs", 49);
		}
		VerboseLog.Log($"Asset pack: {GpuTextureService.PackTextureHits} texture hits, {GpuTextureService.PackTextureMisses} misses, {AssetPack.CorruptEntries} damaged entries; decompressed {(double)AssetPack.DecodedBytes / 1048576.0:F0} MB in {(double)AssetPack.DecodeTicks * 1000.0 / (double)Stopwatch.Frequency:F0} ms CPU time over {(double)(AssetPack.DecodeLastTicks - AssetPack.DecodeFirstTicks) * 1000.0 / (double)Stopwatch.Frequency:F0} ms wall time");
	}
}
