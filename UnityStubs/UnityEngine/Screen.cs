using System;
using Waylith;
using Waylith.Graphics;
using Waylith.UnityCompat;

namespace UnityEngine;

public class Screen
{
	public static Resolution currentResolution
	{
		get
		{
			Resolution result = default(Resolution);
			result.width = width;
			result.height = height;
			result.refreshRateRatio = new RefreshRate
			{
				numerator = (uint)Math.Round(EngineWindow.CurrentResolution.RefreshRate),
				denominator = 1u
			};
			return result;
		}
	}

	public static float dpi => UnityRuntime.Current?.Screen.Dpi ?? 96f;

	public static bool fullScreen
	{
		get
		{
			return EngineWindow.FullScreen;
		}
		set
		{
			fullScreenMode = ((!value) ? FullScreenMode.Windowed : ((fullScreenMode != 0) ? FullScreenMode.FullScreenWindow : FullScreenMode.ExclusiveFullScreen));
		}
	}

	public static FullScreenMode fullScreenMode
	{
		get
		{
			return EngineWindow.DisplayMode switch
			{
				DisplayMode.ExclusiveFullscreen => FullScreenMode.ExclusiveFullScreen, 
				DisplayMode.Borderless => FullScreenMode.FullScreenWindow, 
				_ => FullScreenMode.Windowed, 
			};
		}
		set
		{
			DisplayMode displayMode = value switch
			{
				FullScreenMode.ExclusiveFullScreen => DisplayMode.ExclusiveFullscreen, 
				FullScreenMode.Windowed => DisplayMode.Windowed, 
				_ => DisplayMode.Borderless, 
			};
			if (EngineWindow.DisplayMode != displayMode)
			{
				EngineWindow.SetDisplayMode(displayMode);
			}
			if (UnityRuntime.Current != null)
			{
				UnityRuntime.Current.ConfigureScreen(width, height, displayMode != DisplayMode.Windowed);
			}
		}
	}

	public static int height => UnityRuntime.Current?.Screen.Height ?? 720;

	public static Resolution[] resolutions
	{
		get
		{
			Waylith.Resolution[] availableResolutions = EngineWindow.AvailableResolutions;
			if (!fullScreen || availableResolutions.Length == 0)
			{
				return new Resolution[1] { currentResolution };
			}
			Resolution[] array = new Resolution[availableResolutions.Length];
			for (int i = 0; i < availableResolutions.Length; i++)
			{
				array[i] = new Resolution
				{
					width = availableResolutions[i].Width,
					height = availableResolutions[i].Height,
					refreshRateRatio = new RefreshRate
					{
						numerator = (uint)Math.Round(availableResolutions[i].RefreshRate),
						denominator = 1u
					}
				};
			}
			return array;
		}
	}

	public static int width => UnityRuntime.Current?.Screen.Width ?? 1280;

	public static Rect safeArea => new Rect(0f, 0f, width, height);

	public static int windowWidth => UnityRuntime.Current?.Screen.WindowWidth ?? 1280;

	public static int windowHeight => UnityRuntime.Current?.Screen.WindowHeight ?? 720;

	public static void SetResolution(int width, int height, bool fullscreen)
	{
		fullScreen = fullscreen;
		ApplyResolution(width, height);
	}

	public static void SetResolution(int width, int height, FullScreenMode fullscreenMode)
	{
		fullScreenMode = fullscreenMode;
		ApplyResolution(width, height);
	}

	private static void ApplyResolution(int width, int height)
	{
		if (width > 0 && height > 0)
		{
			double num = ((EngineWindow.Width > 0) ? ((double)EngineWindow.WindowWidth / (double)EngineWindow.Width) : 1.0);
			EngineWindow.SetResolution((int)Math.Round((double)width * num), (int)Math.Round((double)height * num));
		}
	}
}
