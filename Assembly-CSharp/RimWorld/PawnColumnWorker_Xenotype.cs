using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003446 RID: 13382
	public class PawnColumnWorker_Xenotype : PawnColumnWorker_Icon
	{
		// Token: 0x06012D34 RID: 77108 RVA: 0x005955F2 File Offset: 0x005937F2
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Pawn_GeneTracker genes = pawn.genes;
			if (genes == null)
			{
				return null;
			}
			return genes.XenotypeIcon;
		}

		// Token: 0x06012D35 RID: 77109 RVA: 0x00595605 File Offset: 0x00593805
		protected override Color GetIconColor(Pawn pawn)
		{
			return XenotypeDef.IconColor;
		}

		// Token: 0x06012D36 RID: 77110 RVA: 0x0059560C File Offset: 0x0059380C
		protected override string GetIconTip(Pawn pawn)
		{
			if (pawn.genes != null)
			{
				return pawn.genes.XenotypeLabelCap;
			}
			return null;
		}
	}
}
