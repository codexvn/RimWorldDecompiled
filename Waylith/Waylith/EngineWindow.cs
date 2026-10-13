using System;
using Brutal.GlfwApi;
using Waylith.Graphics;

namespace Waylith;

public static class EngineWindow
{
	public static int Width { get; internal set; } = 1280;

	public static int Height { get; internal set; } = 720;

	public static int WindowWidth { get; internal set; } = 1280;

	public static int WindowHeight { get; internal set; } = 720;

	public static int WindowPositionX { get; internal set; }

	public static int WindowPositionY { get; internal set; }

	public static float Dpi { get; internal set; } = 96f;

	public static bool IsFocused { get; internal set; } = true;

	public static bool IsHovered { get; internal set; } = true;

	public static DisplayMode DisplayMode { get; internal set; } = DisplayMode.Windowed;

	public static bool FullScreen => DisplayMode != DisplayMode.Windowed;

	public static Resolution[] AvailableResolutions { get; internal set; } = Array.Empty<Resolution>();

	public static Resolution CurrentResolution => new Resolution(Width, Height, CurrentMonitorRefreshRate());

	public static bool SizeLocked { get; set; }

	public static bool PositionLocked { get; set; }

	public static bool RunInBackground { get; set; } = true;

	public static int TargetFrameRate { get; set; } = -1;

	public static event Action<VSyncMode>? VSyncModeChanged;

	public static event Action<int, int>? ResolutionChanged;

	public static event Action<DisplayMode>? DisplayModeChanged;

	public static event Action<int, int>? WindowPositionChanged;

	public static void SetVSyncMode(VSyncMode mode)
	{
		EngineWindow.VSyncModeChanged?.Invoke(mode);
	}

	public static void SetWindowedPosition(int x, int y)
	{
		if (!PositionLocked)
		{
			EngineWindow.WindowPositionChanged?.Invoke(x, y);
		}
	}

	public static void SetResolution(int width, int height)
	{
		if (!SizeLocked)
		{
			WindowWidth = width;
			WindowHeight = height;
			EngineWindow.ResolutionChanged?.Invoke(width, height);
		}
	}

	public static void SetDisplayMode(DisplayMode mode)
	{
		if (!SizeLocked)
		{
			EngineWindow.DisplayModeChanged?.Invoke(mode);
			DisplayMode = mode;
		}
	}

	private static double CurrentMonitorRefreshRate()
	{
		EngineApplication instance = EngineApplication.Instance;
		if (instance == null || instance.Headless)
		{
			return 60.0;
		}
		if (Glfw.PInvoke.GetPlatform() == 393219)
		{
			double num = 0.0;
			ReadOnlySpan<GlfwMonitor> monitors = Glfw.GetMonitors();
			for (int i = 0; i < monitors.Length; i++)
			{
				num = System.Math.Max(num, monitors[i].VideoMode.RefreshRate);
			}
			if (num > 0.0)
			{
				return num;
			}
		}
		GlfwMonitor glfwMonitor = instance.CurrentMonitor();
		return (!glfwMonitor.IsNull()) ? glfwMonitor.VideoMode.RefreshRate : 0;
	}
}
