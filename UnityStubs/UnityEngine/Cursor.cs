using System;
using Waylith.PlatformSpecific;

namespace UnityEngine;

public class Cursor
{
	private static CursorLockMode _lockState = CursorLockMode.None;

	private static bool _visible = true;

	private static Texture2D _lastTexture;

	private static Vector2 _lastHotspot;

	private static bool _hasLast;

	public static CursorLockMode lockState
	{
		get
		{
			return _lockState;
		}
		set
		{
			_lockState = value;
			ApplyState();
		}
	}

	public static bool visible
	{
		get
		{
			return _visible;
		}
		set
		{
			_visible = value;
			ApplyState();
		}
	}

	private static void ApplyState()
	{
		HardwareCursor.SetState(_visible, _lockState != CursorLockMode.None);
	}

	public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
	{
		if (_hasLast && (object)texture == _lastTexture && hotspot == _lastHotspot)
		{
			return;
		}
		_lastTexture = texture;
		_lastHotspot = hotspot;
		_hasLast = true;
		if (texture == null)
		{
			HardwareCursor.Reset();
			return;
		}
		byte[] rgba;
		try
		{
			rgba = texture.Rgba32TopDown().ToArray();
		}
		catch (InvalidOperationException ex)
		{
			Debug.LogWarning("Invalid texture used for cursor '" + texture.name + "': " + ex.Message);
			HardwareCursor.Reset();
			return;
		}
		HardwareCursor.SetImage(texture, rgba, texture.width, texture.height, (int)MathF.Round(hotspot.x), (int)MathF.Round(hotspot.y));
	}
}
