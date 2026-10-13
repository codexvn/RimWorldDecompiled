using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C31 RID: 3121
	public class Dialog_RechargeSettings : Window
	{
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06005001 RID: 20481 RVA: 0x0019C439 File Offset: 0x0019A639
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(450f, 300f);
			}
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x0019C44C File Offset: 0x0019A64C
		public Dialog_RechargeSettings(MechanitorControlGroup controlGroup) : base(null)
		{
			this.controlGroup = controlGroup;
			this.range = controlGroup.mechRechargeThresholds;
			this.title = "MechRechargeSettingsTitle".Translate();
			this.text = "MechRechargeSettingsExplanation".Translate();
			this.forcePause = true;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x0019C4AC File Offset: 0x0019A6AC
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			Rect rect = new Rect(inRect.x, num, inRect.width, 30f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			num += rect.height + 17f;
			Rect rect2 = new Rect(inRect.x, num, inRect.width, Text.CalcHeight(this.text, inRect.width));
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
			num += rect2.height + 17f;
			Widgets.FloatRange(new Rect(inRect.x, num, inRect.width, 30f), this.GetHashCode(), ref this.range, 0f, 1f, null, ToStringStyle.PercentZero, 0.05f, GameFont.Small, new Color?(Color.white), 0f);
			this.range.min = GenMath.RoundTo(this.range.min, 0.01f);
			this.range.max = GenMath.RoundTo(this.range.max, 0.01f);
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 2f - Window.CloseButSize.x / 2f, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "Reset".Translate(), true, true, true, null))
			{
				this.range = MechanitorControlGroup.DefaultMechRechargeThresholds;
			}
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "OK".Translate(), true, true, true, null))
			{
				this.controlGroup.mechRechargeThresholds = this.range;
				this.Close(true);
			}
		}

		// Token: 0x040039AD RID: 14765
		private FloatRange range;

		// Token: 0x040039AE RID: 14766
		private MechanitorControlGroup controlGroup;

		// Token: 0x040039AF RID: 14767
		private string title;

		// Token: 0x040039B0 RID: 14768
		private string text;

		// Token: 0x040039B1 RID: 14769
		private const float HeaderHeight = 30f;

		// Token: 0x040039B2 RID: 14770
		private const float SliderHeight = 30f;
	}
}
