using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityStubs.Internal;
using UnityStubs.Rendering;
using Waylith.Rendering;

namespace UnityEngine;

public static class ScreenCapture
{
	public enum StereoScreenCaptureMode
	{
		LeftEye = 1,
		RightEye,
		BothEyes
	}

	private static readonly List<string> PendingFiles = new List<string>();

	private static bool _warnedUnreadable;

	public static void CaptureScreenshot(string filename)
	{
		CaptureScreenshot(filename, 1, StereoScreenCaptureMode.LeftEye);
	}

	public static void CaptureScreenshot(string filename, int superSize)
	{
		CaptureScreenshot(filename, superSize, StereoScreenCaptureMode.LeftEye);
	}

	public static void CaptureScreenshot(string filename, StereoScreenCaptureMode stereoCaptureMode)
	{
		CaptureScreenshot(filename, 1, stereoCaptureMode);
	}

	public static Texture2D CaptureScreenshotAsTexture()
	{
		return CaptureScreenshotAsTexture(1, StereoScreenCaptureMode.LeftEye);
	}

	public static Texture2D CaptureScreenshotAsTexture(int superSize)
	{
		return CaptureScreenshotAsTexture(superSize, StereoScreenCaptureMode.LeftEye);
	}

	public static Texture2D CaptureScreenshotAsTexture(StereoScreenCaptureMode stereoCaptureMode)
	{
		return CaptureScreenshotAsTexture(1, stereoCaptureMode);
	}

	public static void CaptureScreenshotIntoRenderTexture(RenderTexture renderTexture)
	{
		ArgumentNullException.ThrowIfNull(renderTexture, "renderTexture");
		IScreenCaptureBackend current = ScreenCaptureBackend.Current;
		if (current == null || !current.TryCopyScreenInto(renderTexture))
		{
			WarnUnreadable("ScreenCapture.CaptureScreenshotIntoRenderTexture");
		}
	}

	private static void CaptureScreenshot(string filename, int superSize, StereoScreenCaptureMode stereoCaptureMode)
	{
		WarnIfSuperSized(superSize);
		IScreenCaptureBackend current = ScreenCaptureBackend.Current;
		if (current == null)
		{
			StubWarner.LogStub("CaptureScreenshot", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ScreenCapture.cs", 40);
		}
		else if (current.RequestCapture())
		{
			PendingFiles.Add(filename);
		}
	}

	private static Texture2D CaptureScreenshotAsTexture(int superSize, StereoScreenCaptureMode stereoCaptureMode)
	{
		WarnIfSuperSized(superSize);
		IScreenCaptureBackend current = ScreenCaptureBackend.Current;
		if (current != null && current.TryReadScreen(out var rgbaTopDown, out var width, out var height))
		{
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: false);
			texture2D.CoreTexture.ReplaceCpuData(width, height, LoadedTextureFormat.Rgba32, new byte[1][] { rgbaTopDown }, 1, linear: false);
			texture2D.Apply(updateMipmaps: false);
			return texture2D;
		}
		WarnUnreadable("ScreenCapture.CaptureScreenshotAsTexture");
		return new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, mipChain: false);
	}

	internal static void WarnUnreadable(string caller)
	{
		if (!_warnedUnreadable)
		{
			Debug.LogWarning(caller + ": the screen is only readable at the end of a rendered frame (after WaitForEndOfFrame), nothing was read. Further misses aren't logged.");
		}
		_warnedUnreadable = true;
	}

	private static void WarnIfSuperSized(int superSize)
	{
		if (superSize > 1)
		{
			StubWarner.LogStub("WarnIfSuperSized", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ScreenCapture.cs", 73);
		}
	}

	internal static void WritePendingScreenshots()
	{
		if (PendingFiles.Count == 0)
		{
			return;
		}
		IScreenCaptureBackend current = ScreenCaptureBackend.Current;
		if (current == null)
		{
			return;
		}
		if (!current.TryReadScreen(out var rgba, out var width, out var height))
		{
			current.RequestCapture();
			return;
		}
		string[] files = PendingFiles.ToArray();
		PendingFiles.Clear();
		Task.Run(delegate
		{
			byte[] bytes = ImageConversion.EncodePng(rgba, width, height);
			string[] array = files;
			foreach (string text in array)
			{
				try
				{
					File.WriteAllBytes(text, bytes);
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to store screenshot (" + text + "): " + ex.Message);
				}
			}
		});
	}
}
