using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B44 RID: 11076
	[StaticConstructorOnStartup]
	public class Gizmo_EnergyShieldStatus : Gizmo
	{
		// Token: 0x0600F7A1 RID: 63393 RVA: 0x0047FEB8 File Offset: 0x0047E0B8
		public Gizmo_EnergyShieldStatus()
		{
			this.Order = -100f;
		}

		// Token: 0x0600F7A2 RID: 63394 RVA: 0x0047FECB File Offset: 0x0047E0CB
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x0600F7A3 RID: 63395 RVA: 0x0047FED4 File Offset: 0x0047E0D4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Widgets.Label(rect3, this.shield.IsApparel ? this.shield.parent.LabelCap : "ShieldInbuilt".Translate().Resolve());
			Rect rect4 = rect2;
			rect4.yMin = rect2.y + rect2.height / 2f;
			float fillPercent = this.shield.Energy / Mathf.Max(1f, this.shield.parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true, -1));
			Widgets.FillableBar(rect4, fillPercent, Gizmo_EnergyShieldStatus.FullShieldBarTex, Gizmo_EnergyShieldStatus.EmptyShieldBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, (this.shield.Energy * 100f).ToString("F0") + " / " + (this.shield.parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true, -1) * 100f).ToString("F0"));
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect2, "ShieldPersonalTip".Translate());
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0400A2EA RID: 41706
		public CompShield shield;

		// Token: 0x0400A2EB RID: 41707
		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x0400A2EC RID: 41708
		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
	}
}
