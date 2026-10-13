using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B4D RID: 2893
	public class Dialog_DebugSetHediffRemaining : Window
	{
		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004A9A RID: 19098 RVA: 0x0017B969 File Offset: 0x00179B69
		private HediffComp_Disappears Comp
		{
			get
			{
				return this.hediff.TryGetComp<HediffComp_Disappears>();
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x0017B976 File Offset: 0x00179B76
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 125f);
			}
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0017B987 File Offset: 0x00179B87
		public Dialog_DebugSetHediffRemaining(Hediff hediff, IWindowDrawing customWindowDrawing = null) : base(customWindowDrawing)
		{
			this.hediff = hediff;
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0017B9C8 File Offset: 0x00179BC8
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + Dialog_DebugSetHediffRemaining.InitialPositionShift;
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

		// Token: 0x06004A9E RID: 19102 RVA: 0x0017BA78 File Offset: 0x00179C78
		public override void DoWindowContents(Rect inRect)
		{
			if (this.hediff == null || this.Comp == null || !this.hediff.pawn.health.hediffSet.hediffs.Contains(this.hediff))
			{
				this.Close(true);
			}
			Widgets.DrawWindowBackground(inRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(new Rect(0f, 0f, inRect.width, 40f), this.hediff.def.defName + " time remaining");
			Text.Anchor = TextAnchor.UpperLeft;
			float num = 190f;
			float num2 = inRect.width / 2f - num / 2f;
			if (Widgets.ButtonText(new Rect(num2, 50f, 40f, 30f), "1d", true, true, true, null))
			{
				this.SetTicks(60000);
			}
			float num3 = num2 + 50f;
			if (Widgets.ButtonText(new Rect(num3, 50f, 40f, 30f), "12h", true, true, true, null))
			{
				this.SetTicks(30000);
			}
			float num4 = num3 + 50f;
			if (Widgets.ButtonText(new Rect(num4, 50f, 40f, 30f), "6h", true, true, true, null))
			{
				this.SetTicks(15000);
			}
			if (Widgets.ButtonText(new Rect(num4 + 50f, 50f, 40f, 30f), "1h", true, true, true, null))
			{
				this.SetTicks(2500);
			}
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x0017BC21 File Offset: 0x00179E21
		private void SetTicks(int ticks)
		{
			this.Comp.ticksToDisappear = ticks;
			SoundDefOf.Click.PlayOneShotOnCamera(null);
			this.Close(true);
		}

		// Token: 0x040034F6 RID: 13558
		private Hediff hediff;

		// Token: 0x040034F7 RID: 13559
		private const float HeaderLabelHeight = 40f;

		// Token: 0x040034F8 RID: 13560
		private const float ButtonHeight = 30f;

		// Token: 0x040034F9 RID: 13561
		private const float ButtonWidth = 40f;

		// Token: 0x040034FA RID: 13562
		private const float Padding = 10f;

		// Token: 0x040034FB RID: 13563
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);
	}
}
