using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002073 RID: 8307
	public class GenStep_Outpost : GenStep
	{
		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x0600B630 RID: 46640 RVA: 0x00345644 File Offset: 0x00343844
		private bool WillPostProcess
		{
			get
			{
				return this.postProcessSettlementParams != null;
			}
		}

		// Token: 0x17001BD6 RID: 7126
		// (get) Token: 0x0600B631 RID: 46641 RVA: 0x0034564F File Offset: 0x0034384F
		public override int SeedPart
		{
			get
			{
				return 398638181;
			}
		}

		// Token: 0x0600B632 RID: 46642 RVA: 0x00345658 File Offset: 0x00343858
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_Outpost.<>c__DisplayClass19_0 CS$<>8__locals1 = new GenStep_Outpost.<>c__DisplayClass19_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CellRect rectToDefend;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend) && !MapGenUtility.TryGetClosestClearRectTo(out rectToDefend, new IntVec2(this.size, this.size), CS$<>8__locals1.map.Center, new Func<CellRect, bool>(CS$<>8__locals1.<Generate>g__Validator|0), -1f, 0.7f, 8) && !MapGenUtility.TryGetRandomClearRect(this.size, this.size, out rectToDefend, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<Generate>g__Validator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				Log.Error("Failed to find location for outpost");
				return;
			}
			Faction faction;
			if (this.overrideFaction != null)
			{
				faction = this.overrideFaction;
			}
			else if (CS$<>8__locals1.map.ParentFaction == null || CS$<>8__locals1.map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = CS$<>8__locals1.map.ParentFaction;
			}
			ResolveParams resolveParams = new ResolveParams
			{
				rect = (this.forcedRect ?? this.GetOutpostRect(rectToDefend, CS$<>8__locals1.usedRects, CS$<>8__locals1.map)),
				faction = faction,
				edgeDefenseWidth = new int?(2),
				edgeDefenseTurretsCount = new int?(Rand.RangeInclusive(0, 1)),
				edgeDefenseMortarsCount = new int?(0),
				settlementDontGeneratePawns = new bool?(this.settlementDontGeneratePawns),
				attackWhenPlayerBecameEnemy = new bool?(this.attackWhenPlayerBecameEnemy),
				pawnGroupKindDef = this.pawnGroupKindDef
			};
			if (parms.sitePart != null)
			{
				resolveParams.bedCount = ((parms.sitePart.expectedEnemyCount == -1) ? null : new int?(parms.sitePart.expectedEnemyCount));
				resolveParams.sitePart = parms.sitePart;
				resolveParams.settlementPawnGroupPoints = new float?(parms.sitePart.parms.threatPoints);
				resolveParams.settlementPawnGroupSeed = new int?(OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms));
			}
			else
			{
				resolveParams.settlementPawnGroupPoints = new float?(this.defaultPawnGroupPointsRange.RandomInRange);
			}
			resolveParams.allowGeneratingThronerooms = new bool?(this.allowGeneratingThronerooms);
			if (this.generateLoot)
			{
				if (parms.sitePart != null)
				{
					resolveParams.lootMarketValue = new float?(parms.sitePart.parms.lootMarketValue);
				}
				else
				{
					resolveParams.lootMarketValue = null;
				}
			}
			else
			{
				resolveParams.lootMarketValue = new float?(0f);
			}
			BaseGen.globalSettings.map = CS$<>8__locals1.map;
			BaseGen.globalSettings.minBuildings = this.requiredWorshippedTerminalRooms + this.requiredGravcoreRooms + 1;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.globalSettings.requiredWorshippedTerminalRooms = this.requiredWorshippedTerminalRooms;
			BaseGen.globalSettings.requiredGravcoreRooms = this.requiredGravcoreRooms;
			BaseGen.globalSettings.maxFarms = (this.allowGeneratingFarms ? -1 : 0);
			BaseGen.symbolStack.Push("settlement", resolveParams, null);
			if (faction != null && faction == Faction.OfEmpire)
			{
				BaseGen.globalSettings.minThroneRooms = (this.allowGeneratingThronerooms ? 1 : 0);
				BaseGen.globalSettings.minLandingPads = 1;
			}
			CS$<>8__locals1.previous = null;
			if (this.WillPostProcess)
			{
				CS$<>8__locals1.previous = new List<Building>(CS$<>8__locals1.map.listerThings.GetThingsOfType<Building>());
			}
			BaseGen.Generate();
			if (faction != null && faction == Faction.OfEmpire && BaseGen.globalSettings.landingPadsGenerated == 0)
			{
				CellRect item;
				GenStep_Settlement.GenerateLandingPadNearby(resolveParams.rect, CS$<>8__locals1.map, faction, out item);
				CS$<>8__locals1.usedRects.Add(item);
			}
			if (this.WillPostProcess)
			{
				List<Building> placed = (from b in CS$<>8__locals1.map.listerThings.GetThingsOfType<Building>()
				where !CS$<>8__locals1.previous.Contains(b)
				select b).ToList<Building>();
				CS$<>8__locals1.previous.Clear();
				MapGenUtility.PostProcessSettlement(CS$<>8__locals1.map, placed, this.postProcessSettlementParams);
			}
			if (this.unfogged)
			{
				foreach (IntVec3 item2 in resolveParams.rect)
				{
					MapGenerator.rootsToUnfog.Add(item2);
				}
			}
			CS$<>8__locals1.usedRects.Add(resolveParams.rect);
		}

		// Token: 0x0600B633 RID: 46643 RVA: 0x00345ABC File Offset: 0x00343CBC
		private CellRect GetOutpostRect(CellRect rectToDefend, List<CellRect> usedRects, Map map)
		{
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.maxX + 1, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.minZ - 1 - this.size, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.maxZ + 1, this.size, this.size));
			CellRect mapRect = new CellRect(0, 0, map.Size.x, map.Size.z);
			GenStep_Outpost.possibleRects.RemoveAll((CellRect x) => !x.FullyContainedWithin(mapRect));
			if (!GenStep_Outpost.possibleRects.Any<CellRect>())
			{
				return rectToDefend;
			}
			IEnumerable<CellRect> source = from x in GenStep_Outpost.possibleRects
			where !usedRects.Any((CellRect y) => x.Overlaps(y))
			select x;
			if (!source.Any<CellRect>())
			{
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size * 2, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.maxX + 1 + this.size, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.minZ - 1 - this.size * 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.maxZ + 1 + this.size, this.size, this.size));
			}
			if (source.Any<CellRect>())
			{
				return source.RandomElement<CellRect>();
			}
			return GenStep_Outpost.possibleRects.RandomElement<CellRect>();
		}

		// Token: 0x04007CF6 RID: 31990
		public int size = 16;

		// Token: 0x04007CF7 RID: 31991
		public int requiredWorshippedTerminalRooms;

		// Token: 0x04007CF8 RID: 31992
		public int requiredGravcoreRooms;

		// Token: 0x04007CF9 RID: 31993
		public bool allowGeneratingThronerooms = true;

		// Token: 0x04007CFA RID: 31994
		public bool settlementDontGeneratePawns;

		// Token: 0x04007CFB RID: 31995
		public bool allowGeneratingFarms = true;

		// Token: 0x04007CFC RID: 31996
		public bool generateLoot = true;

		// Token: 0x04007CFD RID: 31997
		public MapGenUtility.PostProcessSettlementParams postProcessSettlementParams;

		// Token: 0x04007CFE RID: 31998
		public bool unfogged;

		// Token: 0x04007CFF RID: 31999
		public bool attackWhenPlayerBecameEnemy;

		// Token: 0x04007D00 RID: 32000
		public FloatRange defaultPawnGroupPointsRange = SymbolResolver_Settlement.DefaultPawnsPoints;

		// Token: 0x04007D01 RID: 32001
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x04007D02 RID: 32002
		public CellRect? forcedRect;

		// Token: 0x04007D03 RID: 32003
		public Faction overrideFaction;

		// Token: 0x04007D04 RID: 32004
		private const float MaxWaterOverlap = 0.1f;

		// Token: 0x04007D05 RID: 32005
		private static readonly List<CellRect> possibleRects = new List<CellRect>();
	}
}
