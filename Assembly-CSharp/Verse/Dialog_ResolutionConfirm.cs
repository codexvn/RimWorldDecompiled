using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C3F RID: 3135
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x0019CDFA File Offset: 0x0019AFFA
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x0019CE0E File Offset: 0x0019B00E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x0019CE1F File Offset: 0x0019B01F
		private Dialog_ResolutionConfirm() : base(null)
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x0019CE48 File Offset: 0x0019B048
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x0019CE77 File Offset: 0x0019B077
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x0019CE9C File Offset: 0x0019B09C
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x0019CECC File Offset: 0x0019B0CC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string label = "ConfirmResolutionChange".Translate(Mathf.CeilToInt(this.TimeUntilRevert));
			Widgets.Label(new Rect(0f, 0f, inRect.width, inRect.height), label);
			if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionKeep".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionRevert".Translate(), true, true, true, null))
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x0019CFE8 File Offset: 0x0019B1E8
		private void Revert()
		{
			if (Prefs.LogVerbose)
			{
				Log.Message(string.Concat(new string[]
				{
					"Reverting screen settings to ",
					this.oldRes.x.ToString(),
					"x",
					this.oldRes.z.ToString(),
					", fs=",
					this.oldFullscreen.ToString()
				}));
			}
			ResolutionUtility.SetResolutionRaw(this.oldRes.x, this.oldRes.z, this.oldFullscreen);
			Prefs.FullScreen = this.oldFullscreen;
			Prefs.ScreenWidth = this.oldRes.x;
			Prefs.ScreenHeight = this.oldRes.z;
			Prefs.UIScale = this.oldUIScale;
			GenUI.ClearLabelWidthCache();
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x0019D0B4 File Offset: 0x0019B2B4
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x040039C0 RID: 14784
		private float startTime;

		// Token: 0x040039C1 RID: 14785
		private IntVec2 oldRes;

		// Token: 0x040039C2 RID: 14786
		private bool oldFullscreen;

		// Token: 0x040039C3 RID: 14787
		private float oldUIScale;

		// Token: 0x040039C4 RID: 14788
		private const float RevertTime = 10f;
	}
}
