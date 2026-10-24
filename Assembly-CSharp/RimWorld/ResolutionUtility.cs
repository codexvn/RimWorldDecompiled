using UnityEngine;
using Verse;
using Waylith;

namespace RimWorld;

public static class ResolutionUtility
{
	public const int MinResolutionWidth = 1024;

	public const int MinResolutionHeight = 768;

	public const int MinRecommendedResolutionWidth = 1700;

	public const int MinRecommendedResolutionHeight = 910;

	public static float DpiScale
	{
		get
		{
			if (!Prefs.RespectSystemScaling)
			{
				return 1f;
			}
			return Screen.dpi / 96f;
		}
	}

	public static UnityEngine.Resolution NativeResolution
	{
		get
		{
			UnityEngine.Resolution[] resolutions = Screen.resolutions;
			if (resolutions.Length == 0)
			{
				return Screen.currentResolution;
			}
			UnityEngine.Resolution result = resolutions[0];
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

	public static bool BorderlessFullscreen => false;

	public static float ResolveUIScale(float preference)
	{
		float b = Mathf.Min((float)Screen.width / 1024f, (float)Screen.height / 768f);
		return Mathf.Min(preference * DpiScale, Mathf.Max(Dialog_Options.UIScales[0] * DpiScale, b));
	}

	public static void SafeSetResolution(UnityEngine.Resolution res)
	{
		if (Screen.width != res.width || Screen.height != res.height)
		{
			IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
			SetResolutionRaw(res.width, res.height, Screen.fullScreen);
			Prefs.ScreenWidth = res.width;
			Prefs.ScreenHeight = res.height;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
		}
	}

	public static void SafeSetFullscreen(bool fullScreen)
	{
		if (Screen.fullScreen != fullScreen)
		{
			bool fullScreen2 = Screen.fullScreen;
			Screen.fullScreen = fullScreen;
			Prefs.FullScreen = fullScreen;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
		}
	}

	public static void SafeSetFullScreenMode(FullScreenMode mode)
	{
		FullScreenMode fullScreenMode = Screen.fullScreenMode;
		if (fullScreenMode != mode)
		{
			IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
			Screen.fullScreenMode = mode;
			if (fullScreenMode == FullScreenMode.Windowed)
			{
				UnityEngine.Resolution nativeResolution = NativeResolution;
				SetResolutionRaw(nativeResolution.width, nativeResolution.height, mode);
				Prefs.ScreenWidth = nativeResolution.width;
				Prefs.ScreenHeight = nativeResolution.height;
			}
			Prefs.FullScreenMode = mode;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreenMode, oldRes));
		}
	}

	public static void SafeSetUIScale(float newScale)
	{
		if (Prefs.UIScalePreference != newScale)
		{
			float uIScalePreference = Prefs.UIScalePreference;
			Prefs.UIScalePreference = newScale;
			GenUI.ClearLabelWidthCache();
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(uIScalePreference));
		}
	}

	public static bool UIScaleSafeWithResolution(float scale, int w, int h)
	{
		scale *= DpiScale;
		if ((float)w / scale >= 1024f)
		{
			return (float)h / scale >= 768f;
		}
		return false;
	}

	public static void SetResolutionRaw(int w, int h, bool fullScreen)
	{
		if (!Application.isBatchMode)
		{
			if (w <= 0 || h <= 0)
			{
				Log.Error("Tried to set resolution to " + w + "x" + h);
			}
			else if (Screen.width != w || Screen.height != h || Screen.fullScreen != fullScreen)
			{
				Screen.SetResolution(w, h, fullScreen);
			}
		}
	}

	public static void SetResolutionRaw(int w, int h, FullScreenMode mode)
	{
		if (!Application.isBatchMode)
		{
			if (w <= 0 || h <= 0)
			{
				Log.Error("Tried to set resolution to " + w + "x" + h);
			}
			else if (Screen.width != w || Screen.height != h || Screen.fullScreenMode != mode)
			{
				Screen.SetResolution(w, h, mode);
			}
		}
	}

	public static void SetNativeResolutionRaw()
	{
		UnityEngine.Resolution nativeResolution = NativeResolution;
		SetResolutionRaw(nativeResolution.width, nativeResolution.height, !BorderlessFullscreen);
	}

	public static float GetRecommendedUIScale(int screenWidth, int screenHeight)
	{
		if (screenWidth == 0 || screenHeight == 0)
		{
			UnityEngine.Resolution currentResolution = Screen.currentResolution;
			screenWidth = currentResolution.width;
			screenHeight = currentResolution.height;
		}
		if (screenWidth <= 1024 || screenHeight <= 768)
		{
			return 1f;
		}
		int num = Dialog_Options.UIScales.Length - 1;
		while (num >= 0 && Dialog_Options.UIScales[num] >= 1f)
		{
			float num2 = Dialog_Options.UIScales[num] * DpiScale;
			int num3 = Mathf.FloorToInt((float)screenWidth / num2);
			int num4 = Mathf.FloorToInt((float)screenHeight / num2);
			if (num3 >= 1700 && num4 >= 910)
			{
				return Dialog_Options.UIScales[num];
			}
			num--;
		}
		return 1f;
	}

	public static void Update()
	{
		if (RealTime.frameCount % 30 != 0 || LongEventHandler.AnyEventNowOrWaiting)
		{
			return;
		}
		bool flag = false;
		if (Screen.fullScreenMode != Prefs.FullScreenMode)
		{
			Prefs.FullScreenMode = Screen.fullScreenMode;
			if (Screen.fullScreen)
			{
				UnityEngine.Resolution nativeResolution = NativeResolution;
				Prefs.ScreenWidth = nativeResolution.width;
				Prefs.ScreenHeight = nativeResolution.height;
			}
			flag = true;
		}
		if (!Screen.fullScreen)
		{
			if (Screen.windowWidth != Prefs.ScreenWidth)
			{
				Prefs.ScreenWidth = Screen.windowWidth;
				flag = true;
			}
			if (Screen.windowHeight != Prefs.ScreenHeight)
			{
				Prefs.ScreenHeight = Screen.windowHeight;
				flag = true;
			}
		}
		if (!EngineApplication.Instance.Headless && UI.screenWidth < 1024 && UI.screenHeight < 768 && !Prefs.DevMode)
		{
			int screenWidth = UI.screenWidth;
			int screenHeight = UI.screenHeight;
			Prefs.UIScalePreference = GetRecommendedUIScale(0, 0);
			Log.ErrorOnce($"Resolution too small ({screenWidth}x{screenHeight}), resetting UI scale to {Prefs.UIScalePreference}.", 72627836);
			flag = true;
		}
		if (flag)
		{
			Prefs.Save();
		}
	}
}
