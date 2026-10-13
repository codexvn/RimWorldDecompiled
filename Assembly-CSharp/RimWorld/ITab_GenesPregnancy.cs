using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033A0 RID: 13216
	public class ITab_GenesPregnancy : ITab_Genes
	{
		// Token: 0x1700309B RID: 12443
		// (get) Token: 0x0601293B RID: 76091 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0601293C RID: 76092 RVA: 0x0057C910 File Offset: 0x0057AB10
		private GeneSet UnbornBabyHediffGeneset()
		{
			Pawn selPawnForGenes = base.SelPawnForGenes;
			if (selPawnForGenes != null)
			{
				foreach (Hediff hediff in selPawnForGenes.health.hediffSet.hediffs)
				{
					HediffWithParents hediffWithParents = hediff as HediffWithParents;
					if (hediffWithParents != null)
					{
						return hediffWithParents.geneSet;
					}
				}
			}
			return null;
		}

		// Token: 0x0601293D RID: 76093 RVA: 0x0057C984 File Offset: 0x0057AB84
		protected override void FillTab()
		{
			GeneUIUtility.DrawGenesInfo(new Rect(0f, 20f, this.size.x, this.size.y - 20f), null, 550f, ref this.size, ref this.scrollPosition, this.UnbornBabyHediffGeneset());
		}
	}
}
