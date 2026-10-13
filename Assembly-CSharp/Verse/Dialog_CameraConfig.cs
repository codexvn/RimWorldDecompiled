using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DC RID: 476
	[StaticConstructorOnStartup]
	public class Dialog_CameraConfig : Window
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x0003D43B File Offset: 0x0003B63B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(260f, 300f);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0003D44C File Offset: 0x0003B64C
		private CameraMapConfig Config
		{
			get
			{
				return Find.CameraDriver.config;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0000FF5A File Offset: 0x0000E15A
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0003D458 File Offset: 0x0003B658
		public Dialog_CameraConfig() : base(null)
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.draggable = true;
			this.layer = WindowLayer.Super;
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.preventCameraMotion = false;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.drawInScreenshotMode = false;
			this.Reset();
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0003D4B8 File Offset: 0x0003B6B8
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(4f, 0f, rect.width, 30f), "Camera config");
			Rect rect2 = new Rect(4f, 36f, rect.width - 8f, 30f);
			Widgets.HorizontalSlider(rect2, ref this.Config.moveSpeedScale, Dialog_CameraConfig.MoveScaleFactorRange, "Pan speed " + this.Config.moveSpeedScale.ToString(), 0.005f);
			rect2.y += 36f;
			Widgets.HorizontalSlider(rect2, ref this.Config.zoomSpeed, Dialog_CameraConfig.ZoomScaleFactorRange, "Zoom speed " + this.Config.zoomSpeed.ToString(), 0.1f);
			rect2.y += 36f;
			Widgets.FloatRange(rect2, this.GetHashCode(), ref this.Config.sizeRange, 0f, 100f, "ZoomRange", ToStringStyle.FloatOne, 1f, GameFont.Small, null, 0f);
			rect2.y += 36f;
			bool flag = this.Config.zoomPreserveFactor > 0f;
			Widgets.CheckboxLabeled(rect2, "Continuous zoom", ref flag, false, null, null, false, false);
			this.Config.zoomPreserveFactor = (flag ? 1f : 0f);
			rect2.y += 30f;
			Widgets.CheckboxLabeled(rect2, "Smooth zoom", ref this.Config.smoothZoom, false, null, null, false, false);
			rect2.y += 30f;
			Widgets.CheckboxLabeled(rect2, "Follow selected pawns", ref this.Config.followSelected, false, null, null, false, false);
			rect2.y += 30f;
			Widgets.CheckboxLabeled(rect2, "Auto pan while paused", ref this.Config.autoPanWhilePaused, false, null, null, false, false);
			GUI.BeginGroup(new Rect(4f, rect2.yMax, rect.width - 8f, 9999f));
			Rect rect3 = new Rect((rect.width - 8f) / 2f - 15f, 0f, 30f, 30f);
			Widgets.DrawTextureRotated(rect3.center, Dialog_CameraConfig.ArrowTex, -this.Config.autoPanTargetAngle * 57.29578f, 0.4f, null);
			Rect rect4 = new Rect(0f, rect3.yMax + 3f, rect.width - 8f, 30f);
			float num = this.Config.autoPanTargetAngle;
			num = Widgets.HorizontalSlider(rect4, num, 0f, 6.2831855f, false, "Auto pan angle " + (num * 57.29578f).ToString("F0") + "°", "0°", "360°", 0.01f);
			if (num != this.Config.autoPanTargetAngle)
			{
				this.Config.autoPanTargetAngle = (this.Config.autoPanAngle = num);
			}
			float yMax = rect4.yMax;
			Rect rect5 = new Rect(0f, yMax + 6f, rect.width - 8f, 30f);
			float num2 = this.Config.autoPanSpeed;
			num2 = Widgets.HorizontalSlider(rect5, num2, 0f, 5f, false, "Auto pan speed " + this.Config.autoPanSpeed.ToString(), "0", "10", 0.05f);
			if (num2 != this.Config.autoPanSpeed)
			{
				this.Config.autoPanSpeed = num2;
			}
			yMax = rect5.yMax;
			GUI.EndGroup();
			Rect rect6 = new Rect(4f, rect2.yMax + yMax + 10f, rect.width - 8f, 30f);
			if (ModsConfig.OdysseyActive)
			{
				Widgets.Label(new Rect(4f, rect2.yMax + yMax + 10f, rect.width - 8f, 30f), "Camera Config (Gravship)");
				rect6.y += 30f;
				rect6.height = 30f;
				Widgets.CheckboxLabeled(rect6, "Free camera", ref this.Config.gravshipFreeCam, false, null, null, false, false);
				rect6.y += 30f;
				Widgets.CheckboxLabeled(rect6, "Pan on cutscene start", ref this.Config.gravshipPanOnCutsceneStart, false, null, null, false, false);
				rect6.y += 30f;
				Widgets.CheckboxLabeled(rect6, "Enable custom zoom range", ref this.Config.gravshipEnableOverrideSizeRange, false, null, null, false, false);
				rect6.y += 30f;
				if (this.Config.gravshipEnableOverrideSizeRange)
				{
					Widgets.FloatRange(rect6, Gen.HashCombine<Dialog_CameraConfig>(100, this), ref this.Config.gravshipOverrideSizeRange, 0f, 100f, "ZoomRangeGravshipCutscene", ToStringStyle.FloatOne, 1f, GameFont.Small, null, 0f);
					rect6.y += 36f;
				}
			}
			Rect rect7 = new Rect(0f, rect6.y, rect.width, 30f);
			Rect rect8 = rect7;
			rect8.xMax = rect7.width / 3f;
			if (Widgets.ButtonText(rect8, "Reset", true, true, true, null))
			{
				this.Reset();
			}
			rect8.x += rect7.width / 3f;
			if (Widgets.ButtonText(rect8, "Save", true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_CameraConfigList_Save(this.Config));
			}
			rect8.x += rect7.width / 3f;
			if (Widgets.ButtonText(rect8, "Load", true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_CameraConfigList_Load(delegate(CameraMapConfig c)
				{
					this.Config.moveSpeedScale = c.moveSpeedScale;
					this.Config.zoomSpeed = c.zoomSpeed;
					this.Config.sizeRange = c.sizeRange;
					this.Config.zoomPreserveFactor = c.zoomPreserveFactor;
					this.Config.smoothZoom = c.smoothZoom;
					this.Config.followSelected = c.followSelected;
					this.Config.autoPanTargetAngle = (this.Config.autoPanAngle = c.autoPanTargetAngle);
					this.Config.autoPanSpeed = c.autoPanSpeed;
					this.Config.fileName = c.fileName;
					this.Config.autoPanWhilePaused = c.autoPanWhilePaused;
					this.Config.gravshipFreeCam = c.gravshipFreeCam;
					this.Config.gravshipPanOnCutsceneStart = c.gravshipPanOnCutsceneStart;
					this.Config.gravshipEnableOverrideSizeRange = c.gravshipEnableOverrideSizeRange;
					this.Config.gravshipOverrideSizeRange = c.gravshipOverrideSizeRange;
				}));
			}
			if (Event.current.type == EventType.Layout)
			{
				this.windowRect.height = rect7.yMax + this.Margin * 2f;
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0003DB24 File Offset: 0x0003BD24
		private void Reset()
		{
			Find.CameraDriver.config = new CameraMapConfig_Normal();
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0003DB38 File Offset: 0x0003BD38
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(5f, 5f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x04000783 RID: 1923
		private static readonly FloatRange MoveScaleFactorRange = new FloatRange(0f, 2f);

		// Token: 0x04000784 RID: 1924
		private static readonly FloatRange ZoomScaleFactorRange = new FloatRange(0.1f, 10f);

		// Token: 0x04000785 RID: 1925
		private const float SliderHeight = 30f;

		// Token: 0x04000786 RID: 1926
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/TutorArrowRight", true);
	}
}
