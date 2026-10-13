using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200383C RID: 14396
	public class GeneGizmo_DeathrestCapacity : Gizmo
	{
		// Token: 0x06013E66 RID: 81510 RVA: 0x0047FECB File Offset: 0x0047E0CB
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x06013E67 RID: 81511 RVA: 0x005E86E3 File Offset: 0x005E68E3
		public GeneGizmo_DeathrestCapacity(Gene_Deathrest gene)
		{
			this.gene = gene;
			this.Order = -100f;
		}

		// Token: 0x06013E68 RID: 81512 RVA: 0x005E8700 File Offset: 0x005E6900
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect position = rect.ContractedBy(6f);
			float num = position.height / 3f;
			Widgets.DrawWindowBackground(rect);
			GUI.BeginGroup(position);
			Widgets.Label(new Rect(0f, 0f, position.width, num), "Deathrest".Translate().CapitalizeFirst());
			if (this.gene.DeathrestNeed != null)
			{
				this.gene.DeathrestNeed.DrawOnGUI(new Rect(0f, num, position.width, num + 2f), int.MaxValue, 2f, false, true, new Rect?(new Rect(0f, 0f, position.width, num * 2f)), false);
			}
			Rect rect2 = new Rect(0f, num * 2f, position.width, Text.LineHeight);
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(rect2, string.Format("{0}: {1} / {2}", "Buildings".Translate().CapitalizeFirst(), this.gene.CurrentCapacity, this.gene.DeathrestCapacity));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect2, "DeathrestCapacityDesc".Translate() + "\n\n" + "PawnIsConnectedToBuildings".Translate(this.gene.pawn.Named("PAWN"), this.gene.CurrentCapacity.Named("CURRENT"), this.gene.DeathrestCapacity.Named("MAX")));
			}
			GUI.EndGroup();
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0400DBF7 RID: 56311
		protected Gene_Deathrest gene;

		// Token: 0x0400DBF8 RID: 56312
		private const float Padding = 6f;

		// Token: 0x0400DBF9 RID: 56313
		private const float Width = 140f;
	}
}
