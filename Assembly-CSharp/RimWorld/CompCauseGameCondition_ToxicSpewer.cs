using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BC3 RID: 11203
	public class CompCauseGameCondition_ToxicSpewer : CompCauseGameCondition
	{
		// Token: 0x0600FA95 RID: 64149 RVA: 0x0048CF1C File Offset: 0x0048B11C
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Active)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 3451 != 0)
			{
				return;
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(caravans[i].Tile, base.MyTile) < (float)base.Props.worldRange)
				{
					List<Pawn> pawnsListForReading = caravans[i].PawnsListForReading;
					for (int j = 0; j < pawnsListForReading.Count; j++)
					{
						ToxicUtility.DoPawnToxicDamage(pawnsListForReading[j], 1f);
					}
				}
			}
		}
	}
}
