using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BDD RID: 15325
	public class QuestNode_TradeRequest_RandomOfferDuration : QuestNode
	{
		// Token: 0x06014CDC RID: 85212 RVA: 0x0063B7B8 File Offset: 0x006399B8
		public static int RandomOfferDurationTicks(PlanetTile tileIdFrom, PlanetTile tileIdTo, out int travelTicks)
		{
			int randomInRange = SiteTuning.QuestSiteTimeoutDaysRange.RandomInRange;
			travelTicks = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(tileIdFrom, tileIdTo, null);
			float num = (float)travelTicks / 60000f;
			int num2 = Mathf.CeilToInt(Mathf.Max(num + 6f, num * 1.35f));
			if (num2 > SiteTuning.QuestSiteTimeoutDaysRange.max)
			{
				return -1;
			}
			int num3 = Mathf.Max(randomInRange, num2);
			return 60000 * num3;
		}

		// Token: 0x06014CDD RID: 85213 RVA: 0x0063B820 File Offset: 0x00639A20
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			PlanetTile tile = map.Tile;
			PlanetTile tile2 = this.settlement.GetValue(slate).Tile;
			if (!tile2.Valid || !tile.Valid || tile.Layer != tile2.Layer || !tile.LayerDef.SurfaceTiles)
			{
				return;
			}
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
		}

		// Token: 0x06014CDE RID: 85214 RVA: 0x0063B8D4 File Offset: 0x00639AD4
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map.Tile.Layer != this.settlement.GetValue(slate).Tile.Layer)
			{
				return false;
			}
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x0400E795 RID: 59285
		private const float MinNonTravelTimeFractionOfTravelTime = 0.35f;

		// Token: 0x0400E796 RID: 59286
		private const float MinNonTravelTimeDays = 6f;

		// Token: 0x0400E797 RID: 59287
		public SlateRef<Settlement> settlement;

		// Token: 0x0400E798 RID: 59288
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E799 RID: 59289
		[NoTranslate]
		public SlateRef<string> storeEstimatedTravelTimeAs;
	}
}
