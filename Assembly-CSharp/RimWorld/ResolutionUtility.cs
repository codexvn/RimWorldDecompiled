using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031B0 RID: 12720
	public static class ResolutionUtility
	{
		// Token: 0x17002E76 RID: 11894
		// (get) Token: 0x06011E46 RID: 73286 RVA: 0x00533320 File Offset: 0x00531520
		public static Resolution NativeResolution
		{
			get
			{
				Resolution[] resolutions = Screen.resolutions;
				if (resolutions.Length == 0)
				{
					return Screen.currentResolution;
				}
				Resolution result = resolutions[0];
				for (int i = 1; i < resolutions.Length; i++)
				{
					if (resolutions[i].width > result.width || (resolutions[i].width == result.width && resolutions[i].height > result.height))
					{
						result = resolutions[i];
					}
				}
				return result;
			}
		}

		// Token: 0x17002E77 RID: 11895
		// (get) Token: 0x06011E47 RID: 73287 RVA: 0x0053339B File Offset: 0x0053159B
		public static bool BorderlessFullscreen
		{
			get
			{
				if (ResolutionUtility.borderlessFullscreenCached == null)
				{
					ResolutionUtility.borderlessFullscreenCached = new bool?(Environment.GetCommandLineArgs().Contains("-popupwindow"));
				}
				return ResolutionUtility.borderlessFullscreenCached.Value;
			}
		}

		// Token: 0x06011E48 RID: 73288 RVA: 0x005333CC File Offset: 0x005315CC
		public static void SafeSetResolution(Resolution res)
		{
			if (Screen.width == res.width && Screen.height == res.height)
			{
				return;
			}
			IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
			ResolutionUtility.SetResolutionRaw(res.width, res.height, Screen.fullScreen);
			Prefs.ScreenWidth = res.width;
			Prefs.ScreenHeight = res.height;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
		}

		// Token: 0x06011E49 RID: 73289 RVA: 0x00533448 File Offset: 0x00531648
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen == fullScreen)
			{
				return;
			}
			bool fullScreen2 = Screen.fullScreen;
			Screen.fullScreen = fullScreen;
			Prefs.FullScreen = fullScreen;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
		}

		// Token: 0x06011E4A RID: 73290 RVA: 0x00533480 File Offset: 0x00531680
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScale == newScale)
			{
				return;
			}
			float uiscale = Prefs.UIScale;
			Prefs.UIScale = newScale;
			GenUI.ClearLabelWidthCache();
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscale));
		}

		// Token: 0x06011E4B RID: 73291 RVA: 0x005334B7 File Offset: 0x005316B7
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}

		// Token: 0x06011E4C RID: 73292 RVA: 0x005334D4 File Offset: 0x005316D4
		public static void SetResolutionRaw(int w, int h, bool fullScreen)
		{
			if (Application.isBatchMode)
			{
				return;
			}
			if (w <= 0 || h <= 0)
			{
				Log.Error("Tried to set resolution to " + w.ToString() + "x" + h.ToString());
				return;
			}
			if (Screen.width != w || Screen.height != h || Screen.fullScreen != fullScreen)
			{
				Screen.SetResolution(w, h, fullScreen);
			}
		}

		// Token: 0x06011E4D RID: 73293 RVA: 0x00533534 File Offset: 0x00531734
		public static void SetNativeResolutionRaw()
		{
			Resolution nativeResolution = ResolutionUtility.NativeResolution;
			ResolutionUtility.SetResolutionRaw(nativeResolution.width, nativeResolution.height, !ResolutionUtility.BorderlessFullscreen);
		}

		// Token: 0x06011E4E RID: 73294 RVA: 0x00533564 File Offset: 0x00531764
		public static float GetRecommendedUIScale(int screenWidth, int screenHeight)
		{
			if (screenWidth == 0 || screenHeight == 0)
			{
				Resolution currentResolution = Screen.currentResolution;
				screenWidth = currentResolution.width;
				screenHeight = currentResolution.height;
			}
			if (screenWidth <= 1024 || screenHeight <= 768)
			{
				return 1f;
			}
			for (int i = Dialog_Options.UIScales.Length - 1; i >= 0; i--)
			{
				int num = Mathf.FloorToInt((float)screenWidth / Dialog_Options.UIScales[i]);
				int num2 = Mathf.FloorToInt((float)screenHeight / Dialog_Options.UIScales[i]);
				if (num >= 1700 && num2 >= 910)
				{
					return Dialog_Options.UIScales[i];
				}
			}
			return 1f;
		}

		// Token: 0x06011E4F RID: 73295 RVA: 0x005335F4 File Offset: 0x005317F4
		public static void Update()
		{
			if (RealTime.frameCount % 30 == 0 && !LongEventHandler.AnyEventNowOrWaiting)
			{
				bool flag = false;
				if (!Screen.fullScreen)
				{
					if (Screen.width != Prefs.ScreenWidth)
					{
						Prefs.ScreenWidth = Screen.width;
						flag = true;
					}
					if (Screen.height != Prefs.ScreenHeight)
					{
						Prefs.ScreenHeight = Screen.height;
						flag = true;
					}
				}
				if (UI.screenWidth < 1024 && UI.screenHeight < 768 && !Prefs.DevMode)
				{
					int screenWidth = UI.screenWidth;
					int screenHeight = UI.screenHeight;
					Prefs.UIScale = ResolutionUtility.GetRecommendedUIScale(0, 0);
					Log.ErrorOnce(string.Format("Resolution too small ({0}x{1}), resetting UI scale to {2}.", screenWidth, screenHeight, Prefs.UIScale), 72627836);
					flag = true;
				}
				if (flag)
				{
					Prefs.Save();
				}
			}
		}

		// Token: 0x0400B913 RID: 47379
		private static bool? borderlessFullscreenCached;

		// Token: 0x0400B914 RID: 47380
		public const int MinResolutionWidth = 1024;

		// Token: 0x0400B915 RID: 47381
		public const int MinResolutionHeight = 768;

		// Token: 0x0400B916 RID: 47382
		public const int MinRecommendedResolutionWidth = 1700;

		// Token: 0x0400B917 RID: 47383
		public const int MinRecommendedResolutionHeight = 910;
	}
}
