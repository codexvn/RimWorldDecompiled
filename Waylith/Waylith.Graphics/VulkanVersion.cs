using System;
using Brutal.VulkanApi;

namespace Waylith.Graphics;

public static class VulkanVersion
{
	public const int V1_0 = 4194304;

	public const int V1_1 = 4198400;

	public const int V1_2 = 4202496;

	public const int V1_3 = 4206592;

	public static int Major(int version)
	{
		return (version >> 22) & 0x7F;
	}

	public static int Minor(int version)
	{
		return (version >> 12) & 0x3FF;
	}

	public static int Patch(int version)
	{
		return version & 0xFFF;
	}

	public static int WithoutPatch(int version)
	{
		return version & -4096;
	}

	public static bool AtLeast(int version, int required)
	{
		return WithoutPatch(version) >= WithoutPatch(required);
	}

	public static int Min(int a, int b)
	{
		if (WithoutPatch(a) > WithoutPatch(b))
		{
			return b;
		}
		return a;
	}

	public static string Format(int version)
	{
		return $"{Major(version)}.{Minor(version)}.{Patch(version)}";
	}

	public unsafe static int QueryLoaderVersion()
	{
		fixed (byte* ptr = (ReadOnlySpan<byte>)new byte[27]
		{
			118, 107, 69, 110, 117, 109, 101, 114, 97, 116,
			101, 73, 110, 115, 116, 97, 110, 99, 101, 86,
			101, 114, 115, 105, 111, 110, 0
		})
		{
			delegate* unmanaged[Cdecl]<int*, VkResult> invoke = (delegate* unmanaged[Cdecl]<int*, VkResult>)Vulkan.GetInstanceProcAddr.Invoke(default(VkInstance), ptr).Invoke;
			if (invoke == (delegate* unmanaged[Cdecl]<int*, VkResult>)null)
			{
				return 4194304;
			}
			int num = 0;
			if (invoke(&num) != 0 || num == 0)
			{
				return 4194304;
			}
			return num;
		}
	}

	public static uint MaxSpirvVersion(int apiVersion)
	{
		if (AtLeast(apiVersion, 4206592))
		{
			return 67072u;
		}
		if (AtLeast(apiVersion, 4202496))
		{
			return 66816u;
		}
		if (AtLeast(apiVersion, 4198400))
		{
			return 66304u;
		}
		return 65536u;
	}

	public static string FormatSpirv(uint version)
	{
		return $"{(version >> 16) & 0xFF}.{(version >> 8) & 0xFF}";
	}
}
