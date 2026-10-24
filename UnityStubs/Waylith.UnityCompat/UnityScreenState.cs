using System;
using Waylith.Graphics;

namespace Waylith.UnityCompat;

public sealed class UnityScreenState
{
	public int Width { get; internal set; }

	public int Height { get; internal set; }

	public int WindowWidth { get; internal set; }

	public int WindowHeight { get; internal set; }

	public bool FullScreen { get; internal set; }

	public float Dpi => EngineWindow.Dpi;

	public VSyncMode VSyncMode { get; set; } = VSyncMode.TripleBuffer;

	public DisplayMode DisplayMode { get; internal set; }

	public event Action<VSyncMode>? VSyncModeChanged;

	public event Action<DisplayMode>? DisplayModeChanged;

	public void ApplyVSyncMode(VSyncMode mode)
	{
		VSyncMode = mode;
		this.VSyncModeChanged?.Invoke(mode);
	}

	public void ApplyDisplayMode(DisplayMode mode)
	{
		this.DisplayModeChanged?.Invoke(mode);
		DisplayMode = mode;
	}

	internal void InitializeDefaults()
	{
		if (Width <= 0)
		{
			Width = 1280;
		}
		if (Height <= 0)
		{
			Height = 720;
		}
		if (WindowWidth <= 0)
		{
			WindowWidth = Width;
		}
		if (WindowHeight <= 0)
		{
			WindowHeight = Height;
		}
	}
}
