using System;
using System.Diagnostics;
using UnityStubs.Internal;
using Waylith.Rendering;

namespace UnityEngine.Profiling;

public class Profiler
{
	public const uint invalidProfilerArea = uint.MaxValue;

	public static bool enableBinaryLog
	{
		get
		{
			StubWarner.LogStub("enableBinaryLog", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Profiler.cs", 18);
			return false;
		}
		set
		{
			StubWarner.LogStub("enableBinaryLog", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Profiler.cs", 18);
		}
	}

	public static bool enabled
	{
		get
		{
			StubWarner.LogStub("enabled", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Profiler.cs", 19);
			return false;
		}
		set
		{
			StubWarner.LogStub("enabled", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Profiler.cs", 19);
		}
	}

	public static string logFile
	{
		get
		{
			StubWarner.LogStub("logFile", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Profiler.cs", 20);
			return null;
		}
		set
		{
			StubWarner.LogStub("logFile", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Profiler.cs", 20);
		}
	}

	public static long usedHeapSizeLong => GC.GetTotalMemory(forceFullCollection: false);

	[Conditional("ENABLE_PROFILER")]
	public static void BeginSample(string name)
	{
	}

	[Conditional("ENABLE_PROFILER")]
	public static void EndSample()
	{
	}

	public static long GetMonoHeapSizeLong()
	{
		return GC.GetGCMemoryInfo().TotalCommittedBytes;
	}

	public static long GetMonoUsedSizeLong()
	{
		return GC.GetTotalMemory(forceFullCollection: false);
	}

	public static long GetRuntimeMemorySizeLong(Object o)
	{
		if (!(o is Texture texture))
		{
			if (!(o is Mesh mesh))
			{
				if (o is AudioClip audioClip)
				{
					return (long)audioClip.samples * (long)audioClip.channels * 2;
				}
				return 0L;
			}
			return MeshBytes(mesh);
		}
		return TextureBytes(texture);
	}

	private static long TextureBytes(Texture texture)
	{
		long num = 0L;
		if (texture.CoreTexture is Waylith.Rendering.Texture2D texture2D)
		{
			TexturePixelData cpuData = texture2D.CpuData;
			if (cpuData != null)
			{
				for (int i = 0; i < Math.Max(1, cpuData.MipCount); i++)
				{
					num += cpuData.GetLevelSizeBytes(i);
				}
				return num;
			}
		}
		for (int j = 0; j < texture.mipmapCount; j++)
		{
			num += (long)Math.Max(1, texture.width >> j) * (long)Math.Max(1, texture.height >> j) * 4;
		}
		return num;
	}

	private static long MeshBytes(Mesh mesh)
	{
		long num = 0L;
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			num += mesh.GetIndexCount(i);
		}
		return (long)mesh.vertexCount * 36L + num * 4;
	}

	public static long GetTotalAllocatedMemoryLong()
	{
		return Environment.WorkingSet;
	}

	public static long GetTotalReservedMemoryLong()
	{
		using Process process = Process.GetCurrentProcess();
		return process.PrivateMemorySize64;
	}

	public static long GetTotalUnusedReservedMemoryLong()
	{
		return Math.Max(0L, GetTotalReservedMemoryLong() - GetTotalAllocatedMemoryLong());
	}
}
