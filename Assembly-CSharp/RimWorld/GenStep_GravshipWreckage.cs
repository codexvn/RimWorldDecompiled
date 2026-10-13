using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200205B RID: 8283
	public class GenStep_GravshipWreckage : GenStep
	{
		// Token: 0x17001BCF RID: 7119
		// (get) Token: 0x0600B5F0 RID: 46576 RVA: 0x00344465 File Offset: 0x00342665
		public override int SeedPart
		{
			get
			{
				return 26145111;
			}
		}

		// Token: 0x0600B5F1 RID: 46577 RVA: 0x0034446C File Offset: 0x0034266C
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass11_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass11_0();
			CS$<>8__locals1.map = map;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CellRect cellRect;
			bool flag = MapGenUtility.TryGetRandomClearRect(20, 20, out cellRect, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<Generate>g__RectValidator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1);
			IntVec3 intVec = flag ? (cellRect.CenterCell + IntVec3.SouthEast) : CS$<>8__locals1.map.Center;
			GenStep_GravshipWreckage.SpawnCraters(CS$<>8__locals1.map, intVec);
			GenStep_GravshipWreckage.SpawnFilth(CS$<>8__locals1.map, intVec);
			GenStep_GravshipWreckage.SpawnShipChunks(CS$<>8__locals1.map, intVec);
			GenSpawn.Spawn(ThingDefOf.GravEngine, intVec, CS$<>8__locals1.map, WipeMode.Vanish).questTags = parms.sitePart.site.questTags;
			this.SpawnCorpses(CS$<>8__locals1.map, parms, intVec);
			GenStep_GravshipWreckage.SpawnPanels(CS$<>8__locals1.map, intVec);
			this.SpawnDefenders(CS$<>8__locals1.map, parms, intVec);
			if (flag)
			{
				MapGenerator.UsedRects.Add(cellRect.ExpandedBy(8));
			}
		}

		// Token: 0x0600B5F2 RID: 46578 RVA: 0x0034455C File Offset: 0x0034275C
		private static void SpawnCraters(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass12_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass12_0();
			CS$<>8__locals1.map = map;
			int randomInRange = GenStep_GravshipWreckage.NumCratersRange.RandomInRange;
			List<ThingDef> source = new List<ThingDef>
			{
				ThingDefOf.CraterSmall,
				ThingDefOf.CraterMedium,
				ThingDefOf.CraterLarge
			};
			for (int i = 0; i < randomInRange; i++)
			{
				ThingDef def = source.RandomElement<ThingDef>();
				IntVec3 loc;
				if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnCraters>g__Validator|0(c, def), CS$<>8__locals1.map, out loc, 7, 40))
				{
					break;
				}
				GenSpawn.Spawn(def, loc, CS$<>8__locals1.map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600B5F3 RID: 46579 RVA: 0x0034461C File Offset: 0x0034281C
		private static void SpawnFilth(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass13_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass13_0();
			CS$<>8__locals1.map = map;
			int randomInRange = GenStep_GravshipWreckage.NumFilthRange.RandomInRange;
			List<ThingDef> source = new List<ThingDef>
			{
				ThingDefOf.Filth_BlastMark,
				ThingDefOf.Filth_OilSmear
			};
			for (int i = 0; i < randomInRange; i++)
			{
				ThingDef def = source.RandomElement<ThingDef>();
				IntVec3 c2;
				if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnFilth>g__Validator|0(c, def), CS$<>8__locals1.map, out c2, 7, 40))
				{
					break;
				}
				FilthMaker.TryMakeFilth(c2, CS$<>8__locals1.map, def, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x0600B5F4 RID: 46580 RVA: 0x003446D0 File Offset: 0x003428D0
		private static void SpawnPanels(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass14_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass14_0();
			CS$<>8__locals1.map = map;
			int num = GenStep_GravshipWreckage.Panels.RandomInRange;
			CS$<>8__locals1.def = ThingDefOf.GravlitePanel;
			IntVec3 loc;
			while (num > 0 && RCellFinder.TryFindRandomCellNearWith(root, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnPanels>g__Validator|0), CS$<>8__locals1.map, out loc, 5, 40))
			{
				Thing thing = GenSpawn.Spawn(CS$<>8__locals1.def, loc, CS$<>8__locals1.map, WipeMode.Vanish);
				thing.stackCount = Mathf.Min(num, CS$<>8__locals1.def.stackLimit);
				thing.TrySetForbidden(true);
				num -= thing.stackCount;
			}
		}

		// Token: 0x0600B5F5 RID: 46581 RVA: 0x00344768 File Offset: 0x00342968
		private static void SpawnShipChunks(Map map, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass15_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass15_0();
			CS$<>8__locals1.map = map;
			int randomInRange = GenStep_GravshipWreckage.ChunkGroups.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 pos;
				if (RCellFinder.TryFindRandomCellNearWith(root, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnShipChunks>g__Validator|0), CS$<>8__locals1.map, out pos, 5, 40))
				{
					GenSpawn.SpawnIrregularLump(ThingDefOf.ShipChunk_Mech, pos, CS$<>8__locals1.map, GenStep_GravshipWreckage.ChunksPerGroup, GenStep_GravshipWreckage.ChunksDistRange, WipeMode.Vanish, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnShipChunks>g__Validator|0), null, null, null, null);
				}
			}
		}

		// Token: 0x0600B5F6 RID: 46582 RVA: 0x003447E8 File Offset: 0x003429E8
		private void SpawnCorpses(Map map, GenStepParams parms, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass16_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass16_0();
			CS$<>8__locals1.map = map;
			using (IEnumerator<Pawn> enumerator = this.GeneratePawns(parms, CS$<>8__locals1.map, GenStep_GravshipWreckage.DeadPointsRange.RandomInRange).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					IntVec3 loc;
					if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnCorpses>g__Validator|0(c, pawn.def), CS$<>8__locals1.map, out loc, 5, 40))
					{
						pawn.Destroy(DestroyMode.Vanish);
						break;
					}
					HealthUtility.SimulateKilled(pawn, DamageDefOf.Blunt, null, null, null);
					GenSpawn.Spawn(pawn.Corpse, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					pawn.Corpse.TrySetForbidden(true);
				}
			}
		}

		// Token: 0x0600B5F7 RID: 46583 RVA: 0x003448DC File Offset: 0x00342ADC
		private void SpawnDefenders(Map map, GenStepParams parms, IntVec3 root)
		{
			GenStep_GravshipWreckage.<>c__DisplayClass17_0 CS$<>8__locals1 = new GenStep_GravshipWreckage.<>c__DisplayClass17_0();
			CS$<>8__locals1.map = map;
			List<Pawn> list = new List<Pawn>();
			float points = (parms.sitePart != null) ? parms.sitePart.parms.points : GenStep_GravshipWreckage.DefaultPointsRange.RandomInRange;
			using (IEnumerator<Pawn> enumerator = this.GeneratePawns(parms, CS$<>8__locals1.map, points).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					IntVec3 loc;
					if (!RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => CS$<>8__locals1.<SpawnDefenders>g__Validator|0(c, pawn.def), CS$<>8__locals1.map, out loc, 5, 40))
					{
						pawn.Destroy(DestroyMode.Vanish);
						break;
					}
					GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					list.Add(pawn);
				}
			}
			if (!list.Any<Pawn>())
			{
				return;
			}
			LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_DefendPoint(root, new float?(11.9f), null, false, true), CS$<>8__locals1.map, list);
		}

		// Token: 0x0600B5F8 RID: 46584 RVA: 0x00344A18 File Offset: 0x00342C18
		private IEnumerable<Pawn> GeneratePawns(GenStepParams parms, Map map, float points)
		{
			float b = Faction.OfMechanoids.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null);
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				tile = map.Tile,
				faction = Faction.OfMechanoids,
				points = Mathf.Max(points, b)
			};
			if (parms.sitePart != null)
			{
				pawnGroupMakerParms.seed = new int?(Gen.HashCombineInt(SleepingMechanoidsSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms), (int)points));
			}
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true);
		}

		// Token: 0x04007CCA RID: 31946
		private static readonly FloatRange DefaultPointsRange = new FloatRange(340f, 1000f);

		// Token: 0x04007CCB RID: 31947
		private static readonly FloatRange DeadPointsRange = new FloatRange(340f, 600f);

		// Token: 0x04007CCC RID: 31948
		private const float WanderRadius = 11.9f;

		// Token: 0x04007CCD RID: 31949
		private static readonly IntRange ChunkGroups = new IntRange(2, 3);

		// Token: 0x04007CCE RID: 31950
		private static readonly IntRange ChunksPerGroup = new IntRange(1, 2);

		// Token: 0x04007CCF RID: 31951
		private static readonly IntRange ChunksDistRange = new IntRange(2, 4);

		// Token: 0x04007CD0 RID: 31952
		private static readonly IntRange Panels = new IntRange(100, 100);

		// Token: 0x04007CD1 RID: 31953
		private static readonly IntRange NumCratersRange = new IntRange(3, 6);

		// Token: 0x04007CD2 RID: 31954
		private static readonly IntRange NumFilthRange = new IntRange(10, 15);
	}
}
