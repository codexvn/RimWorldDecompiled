using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020020B9 RID: 8377
	public abstract class TileMutatorWorker_AbandonedColony : TileMutatorWorker
	{
		// Token: 0x0600B75E RID: 46942 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_AbandonedColony(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B75F RID: 46943
		protected abstract Faction GetFaction();

		// Token: 0x0600B760 RID: 46944 RVA: 0x0034CD88 File Offset: 0x0034AF88
		public override void GenerateCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Faction faction = this.GetFaction();
			MapGenUtility.PostProcessSettlementParams postProcessSettlementParams = new MapGenUtility.PostProcessSettlementParams
			{
				clearBuildingFaction = true,
				faction = faction,
				damageBuildings = true,
				canDamageWalls = false,
				noFuel = true,
				ageCorpses = true
			};
			new GenStep_Settlement
			{
				generatePawns = false,
				overrideFaction = faction,
				postProcessSettlementParams = postProcessSettlementParams,
				count = 1,
				lootThingSetMaker = ThingSetMakerDefOf.MapGen_AbandonedColonyStockpile,
				lootMarketValue = new float?(500f)
			}.Generate(map, default(GenStepParams));
			CellRect var = MapGenerator.GetVar<CellRect>("SettlementRect");
			MapGenUtility.ScatterCorpses(var, map, faction, TileMutatorWorker_AbandonedColony.CorpsesRange, TileMutatorWorker_AbandonedColony.DeadDaysRange);
			MapGenUtility.DestroyTurrets(map);
			MapGenUtility.DestroyProcessedFood(map);
			MapGenUtility.ForbidAllItems(map);
			MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Add(var);
		}

		// Token: 0x04007DF1 RID: 32241
		private static readonly IntRange CorpsesRange = new IntRange(0, 3);

		// Token: 0x04007DF2 RID: 32242
		private static readonly FloatRange DeadDaysRange = new FloatRange(20f, 60f);

		// Token: 0x04007DF3 RID: 32243
		private const float LootMarketValue = 500f;
	}
}
