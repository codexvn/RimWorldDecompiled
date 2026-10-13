using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002008 RID: 8200
	public class GenStep_SettlementPawnsLoot : GenStep
	{
		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x0600B4A1 RID: 46241 RVA: 0x0033D1D4 File Offset: 0x0033B3D4
		public override int SeedPart
		{
			get
			{
				return 4615611;
			}
		}

		// Token: 0x0600B4A2 RID: 46242 RVA: 0x0033D1DC File Offset: 0x0033B3DC
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect cellRect;
			if (!MapGenerator.TryGetVar<CellRect>("SpawnRect", out cellRect))
			{
				Log.Error("GenStep_SettlementPawnsLoot tried to execute but no SpawnRect was found in the map generator. This CellRect must be set.");
				return;
			}
			Faction faction = this.GetFaction(map);
			if (this.generatePawns)
			{
				Lord lord = LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, cellRect.CenterCell, 25000, false), map, null);
				CellRect rect = cellRect;
				Faction faction2 = faction;
				Lord lord2 = lord;
				PawnGroupKindDef settlement = PawnGroupKindDefOf.Settlement;
				PawnGroupMakerParms pawnGroupMakerParams = null;
				bool flag = this.requiresRoof;
				MapGenUtility.GeneratePawns(map, rect, faction2, lord2, settlement, pawnGroupMakerParams, null, null, null, flag);
			}
			FloatRange? floatRange = this.lootMarketValue;
			if (floatRange == null || !floatRange.GetValueOrDefault().IsZeros)
			{
				ThingSetMakerDef thingSetMakerDef;
				if ((thingSetMakerDef = this.lootThingSetMaker) == null)
				{
					thingSetMakerDef = (faction.def.settlementLootMaker ?? ThingSetMakerDefOf.MapGen_AbandonedColonyStockpile);
				}
				ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
				CellRect rect2 = cellRect;
				ThingSetMakerDef setMakerDef = thingSetMakerDef2;
				FloatRange? marketValueRange = this.lootMarketValue;
				Faction faction3 = faction;
				bool flag = this.requiresRoof;
				MapGenUtility.GenerateLoot(map, rect2, setMakerDef, marketValueRange, null, faction3, flag);
			}
		}

		// Token: 0x0600B4A3 RID: 46243 RVA: 0x0033D2C4 File Offset: 0x0033B4C4
		private Faction GetFaction(Map map)
		{
			Faction result;
			if (this.factionDef != null)
			{
				result = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			}
			else if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				result = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				result = map.ParentFaction;
			}
			return result;
		}

		// Token: 0x04007BD5 RID: 31701
		public FactionDef factionDef;

		// Token: 0x04007BD6 RID: 31702
		public bool generatePawns = true;

		// Token: 0x04007BD7 RID: 31703
		public ThingSetMakerDef lootThingSetMaker;

		// Token: 0x04007BD8 RID: 31704
		public FloatRange? lootMarketValue;

		// Token: 0x04007BD9 RID: 31705
		public bool requiresRoof;
	}
}
