using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002274 RID: 8820
	[StaticConstructorOnStartup]
	public class Gizmo_MechResurrectionCharges : Gizmo
	{
		// Token: 0x0600C044 RID: 49220 RVA: 0x0037B17F File Offset: 0x0037937F
		public Gizmo_MechResurrectionCharges(CompAbilityEffect_ResurrectMech ability)
		{
			this.ability = ability;
			this.Order = -100f;
		}

		// Token: 0x0600C045 RID: 49221 RVA: 0x0037B199 File Offset: 0x00379399
		public override float GetWidth(float maxWidth)
		{
			return Gizmo_MechResurrectionCharges.Width;
		}

		// Token: 0x0600C046 RID: 49222 RVA: 0x0037B1A0 File Offset: 0x003793A0
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect3, "MechResurrectionCharges".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect4 = rect;
			rect4.y += rect3.height - 5f;
			rect4.height = rect.height / 2f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, this.ability.ChargesRemaining.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x040083BE RID: 33726
		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x040083BF RID: 33727
		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x040083C0 RID: 33728
		private static readonly float Width = 110f;

		// Token: 0x040083C1 RID: 33729
		private CompAbilityEffect_ResurrectMech ability;
	}
}
