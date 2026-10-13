using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002042 RID: 8258
	public class GenStep_Settlement : GenStep_Scatterer
	{
		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600B573 RID: 46451 RVA: 0x0034272E File Offset: 0x0034092E
		private bool WillPostProcess
		{
			get
			{
				return this.postProcessSettlementParams != null;
			}
		}

		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x0600B574 RID: 46452 RVA: 0x00342739 File Offset: 0x00340939
		public override int SeedPart
		{
			get
			{
				return 1806208471;
			}
		}

		// Token: 0x0600B575 RID: 46453 RVA: 0x00342740 File Offset: 0x00340940
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			int min = GenStep_Settlement.SettlementSizeRange.min;
			CellRect cellRect = new CellRect(c.x - min / 2, c.z - min / 2, min, min);
			CellRect within = map.BoundsRect(12);
			return cellRect.FullyContainedWithin(within);
		}

		// Token: 0x0600B576 RID: 46454 RVA: 0x003427CC File Offset: 0x003409CC
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			int randomInRange = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			int randomInRange2 = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			CellRect cellRect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			Faction faction;
			if (this.overrideFaction != null)
			{
				faction = this.overrideFaction;
			}
			else if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			cellRect.ClipInsideMap(map);
			ResolveParams resolveParams = new ResolveParams
			{
				sitePart = parms.sitePart,
				rect = cellRect,
				faction = faction,
				settlementDontGeneratePawns = new bool?(!this.generatePawns),
				thingSetMakerDef = this.lootThingSetMaker,
				lootMarketValue = this.lootMarketValue
			};
			if (this.postProcessSettlementParams != null)
			{
				this.postProcessSettlementParams.faction = faction;
			}
			MapGenerator.SetVar<CellRect>("SettlementRect", cellRect);
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = 1 + this.requiredGravcoreRooms;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.globalSettings.requiredGravcoreRooms = this.requiredGravcoreRooms;
			BaseGen.symbolStack.Push("settlement", resolveParams, null);
			if (faction != null && faction == Faction.OfEmpire)
			{
				BaseGen.globalSettings.minThroneRooms = 1;
				BaseGen.globalSettings.minLandingPads = 1;
			}
			List<Building> previous = null;
			if (this.WillPostProcess)
			{
				previous = new List<Building>(map.listerThings.GetThingsOfType<Building>());
			}
			BaseGen.Generate();
			if (faction != null && faction == Faction.OfEmpire && BaseGen.globalSettings.landingPadsGenerated == 0)
			{
				CellRect cellRect2;
				GenStep_Settlement.GenerateLandingPadNearby(resolveParams.rect, map, faction, out cellRect2);
			}
			if (this.WillPostProcess)
			{
				List<Building> placed = (from b in map.listerThings.GetThingsOfType<Building>()
				where !previous.Contains(b)
				select b).ToList<Building>();
				previous.Clear();
				MapGenUtility.PostProcessSettlement(map, placed, this.postProcessSettlementParams);
			}
		}

		// Token: 0x0600B577 RID: 46455 RVA: 0x003429E4 File Offset: 0x00340BE4
		public static void GenerateLandingPadNearby(CellRect rect, Map map, Faction faction, out CellRect usedRect)
		{
			ResolveParams resolveParams = default(ResolveParams);
			List<CellRect> usedRects;
			MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out usedRects);
			GenStep_Settlement.tmpCandidates.Clear();
			int size = 9;
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.maxX + 1, 0, rect.CenterCell.z));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.minX - size, 0, rect.CenterCell.z));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.CenterCell.x, 0, rect.maxZ + 1));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.CenterCell.x, 0, rect.minZ - size));
			IntVec3 intVec;
			if (!GenStep_Settlement.tmpCandidates.Where(delegate(IntVec3 x)
			{
				CellRect r = new CellRect(x.x, x.z, size, size);
				return r.InBounds(map) && (usedRects == null || !usedRects.Any((CellRect y) => y.Overlaps(r)));
			}).TryRandomElement(out intVec))
			{
				usedRect = CellRect.Empty;
				return;
			}
			resolveParams.rect = new CellRect(intVec.x, intVec.z, size, size);
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("landingPad", resolveParams, null);
			BaseGen.Generate();
			usedRect = resolveParams.rect;
		}

		// Token: 0x04007C91 RID: 31889
		private static readonly IntRange SettlementSizeRange = new IntRange(34, 38);

		// Token: 0x04007C92 RID: 31890
		public const string RectVarName = "SettlementRect";

		// Token: 0x04007C93 RID: 31891
		public bool generatePawns = true;

		// Token: 0x04007C94 RID: 31892
		public MapGenUtility.PostProcessSettlementParams postProcessSettlementParams;

		// Token: 0x04007C95 RID: 31893
		public int requiredGravcoreRooms;

		// Token: 0x04007C96 RID: 31894
		public ThingSetMakerDef lootThingSetMaker;

		// Token: 0x04007C97 RID: 31895
		public float? lootMarketValue;

		// Token: 0x04007C98 RID: 31896
		public Faction overrideFaction;

		// Token: 0x04007C99 RID: 31897
		private static readonly List<IntVec3> tmpCandidates = new List<IntVec3>();
	}
}
