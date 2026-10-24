using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;
using Waylith;
using Waylith.Graphics;

namespace RimWorldWaylith;

internal static class WindowPlacement
{
	private const int TrackEveryFrames = 30;

	private static int _savedX = int.MinValue;

	private static int _savedY = int.MinValue;

	public static void Restore()
	{
		try
		{
			string text = EarlyPrefs.PrefsFilePath();
			if (File.Exists(text))
			{
				XElement root = XDocument.Load(text).Root;
				if (int.TryParse(root?.Element("windowPositionX")?.Value, out var result) && int.TryParse(root?.Element("windowPositionY")?.Value, out var result2) && result != int.MinValue && result2 != int.MinValue)
				{
					_savedX = result;
					_savedY = result2;
					EngineWindow.SetWindowedPosition(result, result2);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not restore the window position from prefs: " + ex.Message);
		}
	}

	public static void Track()
	{
		if (UnityEngine.Time.frameCount % 30 == 0 && EngineWindow.DisplayMode == DisplayMode.Windowed && Prefs.IsInitialized && !LongEventHandler.AnyEventNowOrWaiting)
		{
			int windowPositionX = EngineWindow.WindowPositionX;
			int windowPositionY = EngineWindow.WindowPositionY;
			if (windowPositionX != _savedX || windowPositionY != _savedY || !Prefs.HasWindowPosition)
			{
				_savedX = windowPositionX;
				_savedY = windowPositionY;
				Prefs.WindowPositionX = windowPositionX;
				Prefs.WindowPositionY = windowPositionY;
				Prefs.Save();
			}
		}
	}
}
