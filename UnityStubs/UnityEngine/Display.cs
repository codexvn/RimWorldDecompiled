using UnityStubs.Internal;

namespace UnityEngine;

public sealed class Display
{
	public delegate void DisplaysUpdatedDelegate();

	internal nint nativeDisplay;

	public static Display[] displays = new Display[1]
	{
		new Display()
	};

	private static readonly Display _mainDisplay = displays[0];

	public int renderingWidth => Screen.width;

	public int renderingHeight => Screen.height;

	public int systemWidth => Screen.currentResolution.width;

	public int systemHeight => Screen.currentResolution.height;

	public RenderBuffer colorBuffer => default(RenderBuffer);

	public RenderBuffer depthBuffer => default(RenderBuffer);

	public bool active => true;

	public bool requiresBlitToBackbuffer => false;

	public bool requiresSrgbBlitToBackbuffer => false;

	public static Display main => _mainDisplay;

	public static int activeEditorGameViewTarget { get; internal set; } = -1;

	public static event DisplaysUpdatedDelegate onDisplaysUpdated
	{
		add
		{
		}
		remove
		{
		}
	}

	internal Display()
	{
	}

	internal Display(nint nativeDisplay)
	{
		this.nativeDisplay = nativeDisplay;
	}

	public void Activate()
	{
	}

	public void Activate(int width, int height, RefreshRate refreshRate)
	{
		StubWarner.LogStub("Activate", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Display.cs", 31);
	}

	public void Activate(int width, int height, int refreshRate)
	{
		StubWarner.LogStub("Activate", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Display.cs", 32);
	}

	public void SetParams(int width, int height, int x, int y)
	{
		StubWarner.LogStub("SetParams", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Display.cs", 33);
	}

	public void SetRenderingResolution(int w, int h)
	{
		Screen.SetResolution(w, h, Screen.fullScreen);
	}

	public static bool MultiDisplayLicense()
	{
		return false;
	}

	public static Vector3 RelativeMouseAt(Vector3 inputMouseCoordinates)
	{
		return new Vector3(inputMouseCoordinates.x, inputMouseCoordinates.y, 0f);
	}
}
