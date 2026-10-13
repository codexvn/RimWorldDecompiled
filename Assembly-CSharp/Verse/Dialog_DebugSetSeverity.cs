using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B4E RID: 2894
	public class Dialog_DebugSetSeverity : Window
	{
		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x0017B976 File Offset: 0x00179B76
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 125f);
			}
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x0017BC58 File Offset: 0x00179E58
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + Dialog_DebugSetSeverity.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x0017BD08 File Offset: 0x00179F08
		public Dialog_DebugSetSeverity(Hediff hediff) : base(null)
		{
			this.hediff = hediff;
			this.maxSeverity = ((hediff.def.maxSeverity >= float.MaxValue) ? 1f : hediff.def.maxSeverity);
			this.sliderValue = Mathf.InverseLerp(hediff.def.minSeverity, this.maxSeverity, hediff.Severity);
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x0017BDB4 File Offset: 0x00179FB4
		public override void DoWindowContents(Rect inRect)
		{
			if (this.hediff == null || !this.hediff.pawn.health.hediffSet.hediffs.Contains(this.hediff))
			{
				this.Close(true);
			}
			Widgets.DrawWindowBackground(inRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(new Rect(0f, 0f, inRect.width, 40f), this.hediff.def.defName + " severity");
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = inRect;
			position.y = 40f;
			position.height = 10f;
			position.x += 20f;
			position.width -= 40f;
			this.sliderValue = GUI.HorizontalSlider(position, this.sliderValue, 0f, 1f);
			Rect rect = inRect;
			rect.y = inRect.height - 10f - this.BottomButtonSize.y;
			rect.height = this.BottomButtonSize.y;
			rect.x += 20f;
			rect.width -= 40f;
			float num = Mathf.Round(this.sliderValue * 100f);
			if (Widgets.ButtonText(rect, string.Format("Set to {0}%", num), true, true, true, null))
			{
				float severity = Mathf.Lerp(this.hediff.def.minSeverity, this.maxSeverity, Mathf.Clamp01(num / 100f));
				this.hediff.Severity = severity;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x040034FC RID: 13564
		private Hediff hediff;

		// Token: 0x040034FD RID: 13565
		private float maxSeverity;

		// Token: 0x040034FE RID: 13566
		private float sliderValue;

		// Token: 0x040034FF RID: 13567
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04003500 RID: 13568
		private readonly Vector2 BottomButtonSize = new Vector2(100f, 20f);

		// Token: 0x04003501 RID: 13569
		private const float HeaderLabelHeight = 40f;

		// Token: 0x04003502 RID: 13570
		private const float Padding = 20f;
	}
}
