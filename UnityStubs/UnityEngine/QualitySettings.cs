using UnityStubs.Internal;
using Waylith.Graphics;
using Waylith.UnityCompat;

namespace UnityEngine;

public class QualitySettings : Object
{
	public static ColorSpace activeColorSpace => ColorSpace.Gamma;

	public static int antiAliasing
	{
		get
		{
			StubWarner.LogStub("antiAliasing", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\QualitySettings.cs", 14);
			return 0;
		}
	}

	public static int globalTextureMipmapLimit
	{
		get
		{
			return 0;
		}
		set
		{
			StubWarner.LogStub("globalTextureMipmapLimit", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\QualitySettings.cs", 15);
		}
	}

	public static int masterTextureLimit
	{
		get
		{
			return globalTextureMipmapLimit;
		}
		set
		{
			globalTextureMipmapLimit = value;
		}
	}

	public static int vSyncCount
	{
		get
		{
			return ((UnityRuntime.Current?.Screen.VSyncMode ?? VSyncMode.TripleBuffer) != VSyncMode.Off) ? 1 : 0;
		}
		set
		{
			StubWarner.LogStub("vSyncCount", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\QualitySettings.cs", 20);
		}
	}

	public static AnisotropicFiltering anisotropicFiltering { get; set; } = AnisotropicFiltering.Enable;

	public static bool streamingMipmapsActive
	{
		get
		{
			return false;
		}
		set
		{
			StubWarner.LogStub("streamingMipmapsActive", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\QualitySettings.cs", 23);
		}
	}

	public static float streamingMipmapsMemoryBudget { get; set; } = 512f;
}
