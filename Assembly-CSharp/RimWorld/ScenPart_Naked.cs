using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200287F RID: 10367
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x0600E2E3 RID: 58083 RVA: 0x0042695C File Offset: 0x00424B5C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x0600E2E4 RID: 58084 RVA: 0x004269A0 File Offset: 0x00424BA0
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600E2E5 RID: 58085 RVA: 0x004269B8 File Offset: 0x00424BB8
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
