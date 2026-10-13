using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4B RID: 7243
	[StaticConstructorOnStartup]
	public class GuardianShipGizmo : Gizmo
	{
		// Token: 0x0600A142 RID: 41282 RVA: 0x002E604B File Offset: 0x002E424B
		public GuardianShipGizmo(QuestPart_GuardianShipDelay delay)
		{
			this.delay = delay;
			this.Order = -100f;
		}

		// Token: 0x0600A143 RID: 41283 RVA: 0x002E6065 File Offset: 0x002E4265
		public override float GetWidth(float maxWidth)
		{
			return 110f;
		}

		// Token: 0x0600A144 RID: 41284 RVA: 0x002E606C File Offset: 0x002E426C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, "GuardianShipPayment".Translate(this.delay.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)));
			GenUI.ResetLabelAlign();
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04006F75 RID: 28533
		private QuestPart_GuardianShipDelay delay;
	}
}
