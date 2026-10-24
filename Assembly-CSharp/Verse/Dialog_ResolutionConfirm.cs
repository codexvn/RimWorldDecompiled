using RimWorld;
using UnityEngine;

namespace Verse;

public class Dialog_ResolutionConfirm : Window
{
	private float startTime;

	private IntVec2 oldRes;

	private bool oldFullscreen;

	private FullScreenMode oldFullScreenMode;

	private float oldUIScale;

	private const float RevertTime = 10f;

	private float TimeUntilRevert => startTime + 10f - Time.realtimeSinceStartup;

	public override Vector2 InitialSize => new Vector2(500f, 300f);

	private Dialog_ResolutionConfirm()
	{
		startTime = Time.realtimeSinceStartup;
		closeOnAccept = false;
		closeOnCancel = false;
		absorbInputAroundWindow = true;
	}

	public Dialog_ResolutionConfirm(FullScreenMode oldFullScreenMode, IntVec2 oldRes)
		: this()
	{
		oldFullscreen = oldFullScreenMode != FullScreenMode.Windowed;
		this.oldFullScreenMode = oldFullScreenMode;
		this.oldRes = oldRes;
		oldUIScale = Prefs.UIScalePreference;
	}

	public Dialog_ResolutionConfirm(bool oldFullscreen)
		: this()
	{
		this.oldFullscreen = oldFullscreen;
		oldFullScreenMode = (oldFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
		oldRes = new IntVec2(Screen.width, Screen.height);
		oldUIScale = Prefs.UIScalePreference;
	}

	public Dialog_ResolutionConfirm(IntVec2 oldRes)
		: this()
	{
		oldFullscreen = Screen.fullScreen;
		oldFullScreenMode = Screen.fullScreenMode;
		this.oldRes = oldRes;
		oldUIScale = Prefs.UIScalePreference;
	}

	public Dialog_ResolutionConfirm(float oldUIScale)
		: this()
	{
		oldFullscreen = Screen.fullScreen;
		oldFullScreenMode = Screen.fullScreenMode;
		oldRes = new IntVec2(Screen.width, Screen.height);
		this.oldUIScale = oldUIScale;
	}

	public override void DoWindowContents(Rect inRect)
	{
		Text.Font = GameFont.Small;
		string label = "ConfirmResolutionChange".Translate(Mathf.CeilToInt(TimeUntilRevert));
		Widgets.Label(new Rect(0f, 0f, inRect.width, inRect.height), label);
		if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionKeep".Translate(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			Close();
		}
		if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionRevert".Translate(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			Revert();
			Close();
		}
	}

	private void Revert()
	{
		if (Prefs.LogVerbose)
		{
			Log.Message("Reverting screen settings to " + oldRes.x + "x" + oldRes.z + ", fs=" + oldFullscreen);
		}
		ResolutionUtility.SetResolutionRaw(oldRes.x, oldRes.z, oldFullScreenMode);
		Prefs.FullScreenMode = oldFullScreenMode;
		Prefs.ScreenWidth = oldRes.x;
		Prefs.ScreenHeight = oldRes.z;
		Prefs.UIScalePreference = oldUIScale;
		GenUI.ClearLabelWidthCache();
	}

	public override void WindowUpdate()
	{
		if (TimeUntilRevert <= 0f)
		{
			Revert();
			Close();
		}
	}
}
