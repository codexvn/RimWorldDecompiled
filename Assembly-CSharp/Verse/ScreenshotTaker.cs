using System;
using System.IO;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000D57 RID: 3415
	public static class ScreenshotTaker
	{
		// Token: 0x060056F0 RID: 22256 RVA: 0x001BF390 File Offset: 0x001BD590
		public static void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (KeyBindingDefOf.TakeScreenshot.JustPressed || ScreenshotTaker.takeScreenshot)
			{
				ScreenshotTaker.TakeShot();
				ScreenshotTaker.takeScreenshot = false;
			}
			if (Time.frameCount == ScreenshotTaker.lastShotFrame + 1)
			{
				if (ScreenshotTaker.suppressMessage)
				{
					ScreenshotTaker.suppressMessage = false;
					return;
				}
				Messages.Message("MessageScreenshotSavedAs".Translate(ScreenshotTaker.lastShotFilePath), MessageTypeDefOf.TaskCompletion, false);
			}
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x001BF403 File Offset: 0x001BD603
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = (ScreenshotTaker.suppressMessage = true);
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x001BF414 File Offset: 0x001BD614
		private static void TakeShot()
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				try
				{
					SteamScreenshots.TriggerScreenshot();
					return;
				}
				catch (Exception ex)
				{
					string str = "Could not take Steam screenshot. Steam offline? Taking normal screenshot. Exception: ";
					Exception ex2 = ex;
					Log.Warning(str + ((ex2 != null) ? ex2.ToString() : null));
					ScreenshotTaker.TakeNonSteamShot(null);
					return;
				}
			}
			ScreenshotTaker.TakeNonSteamShot(null);
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x001BF474 File Offset: 0x001BD674
		public static void TakeNonSteamShot(string fileName = null)
		{
			string screenshotFolderPath = GenFilePaths.ScreenshotFolderPath;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(screenshotFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				string text;
				if (fileName != null)
				{
					text = string.Format("{0}{1}{2}.png", screenshotFolderPath, Path.DirectorySeparatorChar, fileName);
				}
				else
				{
					do
					{
						ScreenshotTaker.screenshotCount++;
						text = string.Concat(new string[]
						{
							screenshotFolderPath,
							Path.DirectorySeparatorChar.ToString(),
							"screenshot",
							ScreenshotTaker.screenshotCount.ToString(),
							".png"
						});
					}
					while (File.Exists(text));
				}
				ScreenCapture.CaptureScreenshot(text);
				ScreenshotTaker.lastShotFrame = Time.frameCount;
				ScreenshotTaker.lastShotFilePath = text;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to save screenshot in " + screenshotFolderPath + "\nException follows:\n" + ex.ToString());
			}
		}

		// Token: 0x04003F49 RID: 16201
		private static int lastShotFrame = -999;

		// Token: 0x04003F4A RID: 16202
		private static int screenshotCount = 0;

		// Token: 0x04003F4B RID: 16203
		private static string lastShotFilePath;

		// Token: 0x04003F4C RID: 16204
		private static bool suppressMessage;

		// Token: 0x04003F4D RID: 16205
		private static bool takeScreenshot;
	}
}
