using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200268E RID: 9870
	public class RitualAttachableOutcomeEffectWorker_NearbyFactionGoodwill : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8DB RID: 55515 RVA: 0x004042E0 File Offset: 0x004024E0
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			List<RitualOutcomePossibility> outcomeChances = jobRitual.Ritual.outcomeEffect.def.outcomeChances;
			int positivityIndex = outcomeChances.MaxBy((RitualOutcomePossibility c) => c.positivityIndex).positivityIndex;
			int positivityIndex2 = (from c in outcomeChances
			where c.positivityIndex >= 0
			select c).MinBy((RitualOutcomePossibility c) => c.positivityIndex).positivityIndex;
			int num = RitualAttachableOutcomeEffectWorker_NearbyFactionGoodwill.GoodwillRange.Lerped((float)(outcome.positivityIndex - positivityIndex2) / (float)(positivityIndex - positivityIndex2));
			PlanetTile tile = jobRitual.Map.Tile;
			float num2 = float.PositiveInfinity;
			Settlement settlement = null;
			foreach (WorldObject worldObject in Find.WorldObjects.AllSettlementsOnLayer(tile.Layer))
			{
				Settlement settlement2 = worldObject as Settlement;
				if (settlement2 != null && worldObject.Faction.CanChangeGoodwillFor(Faction.OfPlayer, num))
				{
					float num3 = Find.WorldGrid.ApproxDistanceInTiles(tile, worldObject.Tile);
					if (num3 < num2)
					{
						num2 = num3;
						settlement = settlement2;
					}
				}
			}
			if (settlement != null && settlement.Faction.TryAffectGoodwillWith(Faction.OfPlayer, num, true, true, HistoryEventDefOf.RitualDone, null))
			{
				letterLookTargets = new LookTargets((letterLookTargets.targets ?? new List<GlobalTargetInfo>()).Concat(Gen.YieldSingle<GlobalTargetInfo>(settlement)));
				extraOutcomeDesc = this.def.letterInfoText.Formatted(num.Named("AMOUNT"), settlement.Faction.Named("FACTION"));
				return;
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x04009302 RID: 37634
		public static readonly IntRange GoodwillRange = new IntRange(10, 20);
	}
}
