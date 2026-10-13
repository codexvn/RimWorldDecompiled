using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBE RID: 8126
	public class GenStep_Monolith : GenStep_Scatterer
	{
		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x0600B330 RID: 45872 RVA: 0x003355A3 File Offset: 0x003337A3
		public override int SeedPart
		{
			get
			{
				return 345173948;
			}
		}

		// Token: 0x0600B331 RID: 45873 RVA: 0x003355AC File Offset: 0x003337AC
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			if (loc.Fogged(map))
			{
				return false;
			}
			CellRect cellRect = CellRect.CenteredOn(loc, 5);
			int newZ = cellRect.minZ - 1;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				IntVec3 c = new IntVec3(i, 0, newZ);
				if (!c.InBounds(map) || !c.Walkable(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B332 RID: 45874 RVA: 0x00335616 File Offset: 0x00333816
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			GenStep_Monolith.GenerateMonolith(loc, map);
		}

		// Token: 0x0600B333 RID: 45875 RVA: 0x00335620 File Offset: 0x00333820
		public static void GenerateMonolith(IntVec3 loc, Map map)
		{
			if (!Find.Anomaly.GenerateMonolith)
			{
				return;
			}
			GenSpawn.Spawn((Building_VoidMonolith)ThingMaker.MakeThing(ThingDefOf.VoidMonolith, null), loc, map, WipeMode.Vanish);
			foreach (IntVec3 c4 in GridShapeMaker.IrregularLump(loc, map, 30, null))
			{
				map.terrainGrid.SetTerrain(c4, TerrainDefOf.BrokenAsphalt);
			}
			int randomInRange = GenStep_Monolith.BloodFilthRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				Map map2 = map;
				int squareRadius = 4;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 c2;
				if (CellFinder.TryFindRandomCellNear(loc, map2, squareRadius, validator, out c2, -1))
				{
					FilthMaker.TryMakeFilth(c2, map, ThingDefOf.Filth_Blood, 1, FilthSourceFlags.None, true);
				}
			}
			int randomInRange2 = GenStep_Monolith.TwistedFleshFilthRange.RandomInRange;
			Predicate<IntVec3> <>9__1;
			for (int j = 0; j < randomInRange2; j++)
			{
				Map map3 = map;
				int squareRadius2 = 4;
				Predicate<IntVec3> validator2;
				if ((validator2 = <>9__1) == null)
				{
					validator2 = (<>9__1 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 c3;
				if (CellFinder.TryFindRandomCellNear(loc, map3, squareRadius2, validator2, out c3, -1))
				{
					FilthMaker.TryMakeFilth(c3, map, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
				}
			}
			int randomInRange3 = GenStep_Monolith.CorpsesCountRange.RandomInRange;
			Predicate<IntVec3> <>9__2;
			for (int k = 0; k < randomInRange3; k++)
			{
				Map map4 = map;
				int squareRadius3 = 4;
				Predicate<IntVec3> validator3;
				if ((validator3 = <>9__2) == null)
				{
					validator3 = (<>9__2 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 loc2;
				if (CellFinder.TryFindRandomCellNear(loc, map4, squareRadius3, validator3, out loc2, -1))
				{
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Drifter, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
					pawn.health.SetDead();
					pawn.apparel.DestroyAll(DestroyMode.Vanish);
					pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					Corpse corpse = pawn.MakeCorpse(null, null);
					corpse.Age = Mathf.RoundToInt((float)(GenStep_Monolith.CorpseAgeRangeDays.RandomInRange * 60000));
					corpse.GetComp<CompRottable>().RotProgress += (float)corpse.Age;
					GenSpawn.Spawn(pawn.Corpse, loc2, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04007AC0 RID: 31424
		private const int DebrisRadius = 4;

		// Token: 0x04007AC1 RID: 31425
		private const int AsphaltSize = 30;

		// Token: 0x04007AC2 RID: 31426
		private const int ClearRadius = 5;

		// Token: 0x04007AC3 RID: 31427
		private static readonly IntRange BloodFilthRange = new IntRange(1, 4);

		// Token: 0x04007AC4 RID: 31428
		private static readonly IntRange TwistedFleshFilthRange = new IntRange(3, 6);

		// Token: 0x04007AC5 RID: 31429
		private static readonly IntRange CorpsesCountRange = new IntRange(1, 3);

		// Token: 0x04007AC6 RID: 31430
		private static readonly IntRange CorpseAgeRangeDays = new IntRange(50, 200);
	}
}
