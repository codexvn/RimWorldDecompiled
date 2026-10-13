using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003450 RID: 13392
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Energy : PawnColumnWorker
	{
		// Token: 0x06012D62 RID: 77154 RVA: 0x00595B80 File Offset: 0x00593D80
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.IsGestating())
			{
				return;
			}
			Widgets.FillableBar(rect.ContractedBy(4f), pawn.needs.energy.CurLevelPercentage, PawnColumnWorker_Energy.EnergyBarTex, BaseContent.ClearTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, Mathf.RoundToInt(pawn.needs.energy.CurLevel).ToString() + " / " + Mathf.RoundToInt(pawn.needs.energy.MaxLevel).ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06012D63 RID: 77155 RVA: 0x00595C24 File Offset: 0x00593E24
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 120);
		}

		// Token: 0x06012D64 RID: 77156 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0400C367 RID: 50023
		private const int Width = 120;

		// Token: 0x0400C368 RID: 50024
		private const int BarPadding = 4;

		// Token: 0x0400C369 RID: 50025
		public static readonly Texture2D EnergyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(252, byte.MaxValue, byte.MaxValue, 65));
	}
}
