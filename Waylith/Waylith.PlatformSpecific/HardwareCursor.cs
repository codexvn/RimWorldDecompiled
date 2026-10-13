using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Brutal.GlfwApi;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.Numerics;

namespace Waylith.PlatformSpecific;

public static class HardwareCursor
{
	private static readonly LogCategory Log = LogCategory.Make("Cursor");

	private static readonly Lock Gate = new Lock();

	private const int DefaultCursorSize = 32;

	private const int MinCursorSize = 16;

	private const int MaxCursorSize = 128;

	private static readonly Dictionary<object, GlfwCursor> Cache = new Dictionary<object, GlfwCursor>();

	private static readonly Dictionary<GlfwStandardCursor, GlfwCursor> StandardCursorCache = new Dictionary<GlfwStandardCursor, GlfwCursor>();

	private static bool _initialized;

	private static int _requestGen;

	private static int _appliedGen;

	private static bool _wantDefault = true;

	private static GlfwStandardCursor? _wantStandardCursor;

	private static object? _wantKey;

	private static byte[]? _wantPixels;

	private static int _wantWidth;

	private static int _wantHeight;

	private static int _wantHotX;

	private static int _wantHotY;

	private static int _appliedTargetSize = 32;

	private static HardwareCursorMode _wantMode = HardwareCursorMode.Normal;

	private static HardwareCursorMode _appliedMode = HardwareCursorMode.Normal;

	private static bool? _rawMotionSupported;

	private static bool RawMotionSupported
	{
		get
		{
			bool valueOrDefault = _rawMotionSupported == true;
			if (!_rawMotionSupported.HasValue)
			{
				valueOrDefault = Glfw.RawMotionSupported();
				_rawMotionSupported = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	public static bool Visible { get; private set; } = true;

	public static bool Locked { get; private set; }

	public static void SetMode(HardwareCursorMode mode)
	{
		using (Gate.EnterScope())
		{
			if (_wantMode != mode)
			{
				_wantMode = mode;
				_requestGen++;
				EnsureInitializedLocked();
			}
		}
	}

	public static void SetState(bool visible, bool locked)
	{
		Visible = visible;
		Locked = locked;
		HardwareCursorMode mode = ((!locked) ? ((!visible) ? HardwareCursorMode.Hidden : HardwareCursorMode.Normal) : (visible ? HardwareCursorMode.ConstrainedVisible : HardwareCursorMode.LockedHidden));
		SetMode(mode);
	}

	public static void Reset()
	{
		using (Gate.EnterScope())
		{
			_wantDefault = true;
			_wantStandardCursor = null;
			_wantKey = null;
			_wantPixels = null;
			_requestGen++;
			EnsureInitializedLocked();
		}
	}

	public static void SetStandard(GlfwStandardCursor cursor)
	{
		using (Gate.EnterScope())
		{
			_wantDefault = false;
			_wantStandardCursor = cursor;
			_wantKey = null;
			_wantPixels = null;
			_requestGen++;
			EnsureInitializedLocked();
		}
	}

	public static void SetImage(object key, byte[] rgba, int width, int height, int hotX, int hotY)
	{
		if (width <= 0 || height <= 0 || rgba.Length < (long)width * (long)height * 4)
		{
			Reset();
			return;
		}
		using (Gate.EnterScope())
		{
			_wantDefault = false;
			_wantStandardCursor = null;
			_wantKey = key;
			_wantPixels = rgba;
			_wantWidth = width;
			_wantHeight = height;
			_wantHotX = System.Math.Clamp(hotX, 0, width - 1);
			_wantHotY = System.Math.Clamp(hotY, 0, height - 1);
			_requestGen++;
			EnsureInitializedLocked();
		}
	}

	private static void EnsureInitializedLocked()
	{
		if (!_initialized)
		{
			if (!EngineApplication.Instance.Headless)
			{
				EngineApplication.Instance.PrePollEvents += OnPrePollEvents;
			}
			_initialized = true;
		}
	}

	private static void OnPrePollEvents(EngineApplication app)
	{
		GlfwWindow window = app.Window;
		float x = (window.IsNull() ? 1f : MathF.Max(window.ContentScale.X, window.ContentScale.Y));
		int num = (int)MathF.Round(32f * MathF.Max(x, 0.01f));
		int requestGen;
		bool wantDefault;
		GlfwStandardCursor? wantStandardCursor;
		object wantKey;
		byte[] wantPixels;
		int wantWidth;
		int wantHeight;
		int wantHotX;
		int wantHotY;
		HardwareCursorMode wantMode;
		using (Gate.EnterScope())
		{
			if (_appliedGen == _requestGen && num == _appliedTargetSize)
			{
				return;
			}
			requestGen = _requestGen;
			wantDefault = _wantDefault;
			wantStandardCursor = _wantStandardCursor;
			wantKey = _wantKey;
			wantPixels = _wantPixels;
			wantWidth = _wantWidth;
			wantHeight = _wantHeight;
			wantHotX = _wantHotX;
			wantHotY = _wantHotY;
			wantMode = _wantMode;
		}
		try
		{
			Apply(app, wantDefault, wantStandardCursor, wantKey, wantPixels, wantWidth, wantHeight, wantHotX, wantHotY, num, wantMode);
		}
		catch (Exception ex)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(24, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Failed to apply cursor: ");
				message.AppendFormatted(ex.Message);
			}
			log.Warning(message, "OnPrePollEvents", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\PlatformSpecific\\HardwareCursor.cs", 183);
		}
		finally
		{
			using (Gate.EnterScope())
			{
				_appliedGen = requestGen;
			}
		}
	}

	private static void Apply(EngineApplication app, bool wantDefault, GlfwStandardCursor? wantStandardCursor, object? key, byte[]? pixels, int width, int height, int hotX, int hotY, int targetSize, HardwareCursorMode mode)
	{
		GlfwWindow window = app.Window;
		if (window.IsNull())
		{
			return;
		}
		if (mode != _appliedMode)
		{
			if (RawMotionSupported)
			{
				window.SetInputMode(GlfwInputMode.RawMouseMotion, (mode == HardwareCursorMode.LockedHidden) ? 1 : 0);
			}
			window.SetInputMode(GlfwInputMode.Cursor, (int)ToGlfwCursorMode(mode));
			_appliedMode = mode;
		}
		if (targetSize != _appliedTargetSize)
		{
			foreach (GlfwCursor value3 in Cache.Values)
			{
				value3.Dispose();
			}
			Cache.Clear();
			_appliedTargetSize = targetSize;
		}
		if (wantDefault)
		{
			window.SetCursor(default(GlfwCursor));
			return;
		}
		if (wantStandardCursor.HasValue)
		{
			GlfwStandardCursor valueOrDefault = wantStandardCursor.GetValueOrDefault();
			if (!StandardCursorCache.TryGetValue(valueOrDefault, out var value))
			{
				value = Glfw.CreateCursor(valueOrDefault);
				StandardCursorCache[valueOrDefault] = value;
			}
			window.SetCursor(value);
			return;
		}
		if (key == null || pixels == null)
		{
			window.SetCursor(default(GlfwCursor));
			return;
		}
		if (!Cache.TryGetValue(key, out var value2))
		{
			(byte[] pixels, int width, int height, int hotX, int hotY) tuple = ShrinkToTarget(pixels, width, height, hotX, hotY, targetSize);
			byte[] item = tuple.pixels;
			int item2 = tuple.width;
			int item3 = tuple.height;
			int item4 = tuple.hotX;
			int item5 = tuple.hotY;
			value2 = CreateCursor(item, item2, item3, item4, item5);
			if (value2.IsNull())
			{
				Log.Warning("glfwCreateCursor failed; falling back to the default cursor.", "Apply", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\PlatformSpecific\\HardwareCursor.cs", 249);
				window.SetCursor(default(GlfwCursor));
				return;
			}
			Cache[key] = value2;
		}
		window.SetCursor(value2);
	}

	private static GlfwCursorMode ToGlfwCursorMode(HardwareCursorMode mode)
	{
		return mode switch
		{
			HardwareCursorMode.Hidden => GlfwCursorMode.Hidden, 
			HardwareCursorMode.LockedHidden => GlfwCursorMode.Disabled, 
			HardwareCursorMode.ConstrainedVisible => GlfwCursorMode.Captured, 
			_ => GlfwCursorMode.Normal, 
		};
	}

	private static (byte[] pixels, int width, int height, int hotX, int hotY) ShrinkToTarget(byte[] pixels, int width, int height, int hotX, int hotY, int targetSize)
	{
		int num = TargetCursorSize(targetSize);
		int num2 = System.Math.Max(width, height);
		if (num2 <= num)
		{
			return (pixels: pixels, width: width, height: height, hotX: hotX, hotY: hotY);
		}
		float num3 = (float)num / (float)num2;
		int num4 = System.Math.Max(1, (int)MathF.Round((float)width * num3));
		int num5 = System.Math.Max(1, (int)MathF.Round((float)height * num3));
		byte[] item = DownscaleRgba(pixels, width, height, num4, num5);
		int item2 = System.Math.Clamp((int)MathF.Round((float)hotX * (float)num4 / (float)width), 0, num4 - 1);
		int item3 = System.Math.Clamp((int)MathF.Round((float)hotY * (float)num5 / (float)height), 0, num5 - 1);
		return (pixels: item, width: num4, height: num5, hotX: item2, hotY: item3);
	}

	private static int TargetCursorSize(int requested)
	{
		InlineArray2<string> buffer = default(InlineArray2<string>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<string>, string>(ref buffer, 0) = "WAYLITH_CURSOR_SIZE";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<string>, string>(ref buffer, 1) = "XCURSOR_SIZE";
		ReadOnlySpan<string> readOnlySpan = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<string>, string>(in buffer, 2);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			if (int.TryParse(Environment.GetEnvironmentVariable(readOnlySpan[i]), out var result) && result > 0)
			{
				return System.Math.Clamp(result, 16, 128);
			}
		}
		return System.Math.Clamp(requested, 16, 128);
	}

	private static byte[] DownscaleRgba(byte[] src, int sw, int sh, int dw, int dh)
	{
		byte[] array = new byte[dw * dh * 4];
		for (int i = 0; i < dh; i++)
		{
			int num = i * sh / dh;
			int num2 = System.Math.Max(num + 1, (i + 1) * sh / dh);
			for (int j = 0; j < dw; j++)
			{
				int num3 = j * sw / dw;
				int num4 = System.Math.Max(num3 + 1, (j + 1) * sw / dw);
				double num5 = 0.0;
				double num6 = 0.0;
				double num7 = 0.0;
				double num8 = 0.0;
				int num9 = 0;
				for (int k = num; k < num2; k++)
				{
					for (int l = num3; l < num4; l++)
					{
						int num10 = (k * sw + l) * 4;
						double num11 = (int)src[num10 + 3];
						num5 += (double)(int)src[num10] * num11;
						num6 += (double)(int)src[num10 + 1] * num11;
						num7 += (double)(int)src[num10 + 2] * num11;
						num8 += num11;
						num9++;
					}
				}
				int num12 = (i * dw + j) * 4;
				if (num8 > 0.0)
				{
					array[num12] = (byte)System.Math.Clamp(num5 / num8, 0.0, 255.0);
					array[num12 + 1] = (byte)System.Math.Clamp(num6 / num8, 0.0, 255.0);
					array[num12 + 2] = (byte)System.Math.Clamp(num7 / num8, 0.0, 255.0);
				}
				array[num12 + 3] = (byte)System.Math.Clamp(num8 / (double)num9, 0.0, 255.0);
			}
		}
		return array;
	}

	private unsafe static GlfwCursor CreateCursor(byte[] rgba, int width, int height, int hotX, int hotY)
	{
		fixed (byte* pixels = rgba)
		{
			GlfwCursor.CreateInfo createInfo = default(GlfwCursor.CreateInfo);
			createInfo.Image = new GlfwImage
			{
				Width = width,
				Height = height,
				Pixels = pixels
			};
			createInfo.Hot = new int2(hotX, hotY);
			GlfwCursor.CreateInfo info = createInfo;
			return Glfw.CreateCursor(in info);
		}
	}
}
