using System;
using System.IO;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Waylith.Diagnostics;
using Waylith.Graphics;
using Waylith.Runtime;

namespace Waylith.Hardware;

public static class GpuPerformanceProfileStore
{
	private const uint Magic = 1196446278u;

	private const string FileName = "gpu-profile.bin";

	private const string DirectoryName = "HardwareProfile";

	public static bool TryLoad(GraphicsDevice graphicsDevice, out GpuPerformanceProfile profile)
	{
		profile = default(GpuPerformanceProfile);
		try
		{
			string text = ResolvePath();
			if (text == null || ColdBoot.Enabled || !File.Exists(text))
			{
				return false;
			}
			var (num, num2, num3) = ReadDeviceKey(graphicsDevice);
			using FileStream input = File.OpenRead(text);
			using BinaryReader binaryReader = new BinaryReader(input);
			if (binaryReader.ReadUInt32() != 1196446278)
			{
				return false;
			}
			if (binaryReader.ReadInt32() != 4)
			{
				return false;
			}
			if (binaryReader.ReadInt32() != num)
			{
				return false;
			}
			if (binaryReader.ReadInt32() != num2)
			{
				return false;
			}
			if (binaryReader.ReadInt32() != num3)
			{
				return false;
			}
			float aluGFlopsPerSecond = binaryReader.ReadSingle();
			float memoryBandwidthGBPerSecond = binaryReader.ReadSingle();
			int aluSaturationThreads = binaryReader.ReadInt32();
			profile = new GpuPerformanceProfile
			{
				AluGFlopsPerSecond = aluGFlopsPerSecond,
				MemoryBandwidthGBPerSecond = memoryBandwidthGBPerSecond,
				AluSaturationThreads = aluSaturationThreads,
				Measured = true
			};
			return true;
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(56, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("GpuPerformanceProfileStore: load failed, will re-probe: ");
				message.AppendFormatted(ex.Message);
			}
			log.Warning(message, "TryLoad", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Hardware\\GpuPerformanceProfileStore.cs", 55);
			return false;
		}
	}

	public static void Save(GraphicsDevice graphicsDevice, GpuPerformanceProfile profile)
	{
		if (!profile.Measured)
		{
			return;
		}
		try
		{
			string text = ResolvePath();
			if (text == null)
			{
				return;
			}
			Directory.CreateDirectory(Path.GetDirectoryName(text));
			(int vendorId, int deviceId, int driverVersion) tuple = ReadDeviceKey(graphicsDevice);
			int item = tuple.vendorId;
			int item2 = tuple.deviceId;
			int item3 = tuple.driverVersion;
			string text2 = text + ".tmp";
			using (FileStream output = File.Create(text2))
			{
				using BinaryWriter binaryWriter = new BinaryWriter(output);
				binaryWriter.Write(1196446278u);
				binaryWriter.Write(4);
				binaryWriter.Write(item);
				binaryWriter.Write(item2);
				binaryWriter.Write(item3);
				binaryWriter.Write(profile.AluGFlopsPerSecond);
				binaryWriter.Write(profile.MemoryBandwidthGBPerSecond);
				binaryWriter.Write(profile.AluSaturationThreads);
			}
			File.Move(text2, text, overwrite: true);
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(53, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("GpuPerformanceProfileStore: save failed (non-fatal): ");
				message.AppendFormatted(ex.Message);
			}
			log.Warning(message, "Save", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Hardware\\GpuPerformanceProfileStore.cs", 91);
		}
	}

	private static (int vendorId, int deviceId, int driverVersion) ReadDeviceKey(GraphicsDevice graphicsDevice)
	{
		VkPhysicalDeviceProperties properties = graphicsDevice.PhysicalDevice.GetProperties();
		return (vendorId: properties.VendorID, deviceId: properties.DeviceID, driverVersion: properties.DriverVersion);
	}

	private static string? ResolvePath()
	{
		string text = WaylithRuntime.Current?.App.CacheDataPath;
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		return Path.Combine(text, "HardwareProfile", "gpu-profile.bin");
	}
}
