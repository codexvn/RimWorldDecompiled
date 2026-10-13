using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002028 RID: 8232
	public class GenStep_UndercaveInterest : GenStep
	{
		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x0600B50F RID: 46351 RVA: 0x0033FE4A File Offset: 0x0033E04A
		public override int SeedPart
		{
			get
			{
				return 26098423;
			}
		}

		// Token: 0x0600B510 RID: 46352 RVA: 0x0033FE54 File Offset: 0x0033E054
		public override void Generate(Map map, GenStepParams parms)
		{
			Thing pitGateExit = map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>();
			Pawn dreadmeld = map.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => p.kindDef == PawnKindDefOf.Dreadmeld);
			int randomInRange = GenStep_UndercaveInterest.InterestPointCountRange.RandomInRange;
			List<IntVec3> interestPoints = new List<IntVec3>();
			Predicate<IntVec3> <>9__1;
			for (int i = 0; i < randomInRange; i++)
			{
				Map map2 = map;
				Predicate<IntVec3> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((IntVec3 c) => c.Standable(map) && c.DistanceToEdge(map) > 5 && (pitGateExit == null || !c.InHorDistOf(pitGateExit.Position, 10f)) && (dreadmeld == null || !c.InHorDistOf(dreadmeld.Position, 10f)) && !interestPoints.Any((IntVec3 p) => c.InHorDistOf(p, 10f))));
				}
				IntVec3 item;
				if (CellFinder.TryFindRandomCell(map2, validator, out item))
				{
					interestPoints.Add(item);
				}
			}
			foreach (IntVec3 intVec in interestPoints)
			{
				GenStep_UndercaveInterest.UnderCaveInterestKind underCaveInterestKind = Gen.RandomEnumValue<GenStep_UndercaveInterest.UnderCaveInterestKind>(false);
				foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(intVec, map, 20, null))
				{
					foreach (Thing thing in c2.GetThingList(map).ToList<Thing>())
					{
						if (thing.def.destroyable)
						{
							Building edifice = c2.GetEdifice(map);
							bool? flag;
							if (edifice == null)
							{
								flag = null;
							}
							else
							{
								ThingDef def = edifice.def;
								if (def == null)
								{
									flag = null;
								}
								else
								{
									BuildingProperties building = def.building;
									flag = ((building != null) ? new bool?(building.isNaturalRock) : null);
								}
							}
							bool? flag2 = flag;
							if (!flag2.GetValueOrDefault())
							{
								Building edifice2 = c2.GetEdifice(map);
								if (((edifice2 != null) ? edifice2.def : null) != ThingDefOf.Fleshmass)
								{
									continue;
								}
							}
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
				switch (underCaveInterestKind)
				{
				case GenStep_UndercaveInterest.UnderCaveInterestKind.MushroomPatch:
					this.GenerateMushroomPatch(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.ChemfuelGenerator:
					this.GenerateChemfuel(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.InsectHive:
					this.GenerateHive(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.CorpseGear:
					this.GenerateCorpseGear(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.CorpsePile:
					GenStep_UndercaveInterest.GenerateCorpsePile(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.SleepingFleshbeasts:
					this.GenerateSleepingFleshbeasts(map, intVec);
					break;
				}
			}
		}

		// Token: 0x0600B511 RID: 46353 RVA: 0x0034014C File Offset: 0x0033E34C
		private void GenerateMushroomPatch(Map map, IntVec3 cell)
		{
			List<ThingDef> source = new List<ThingDef>
			{
				ThingDefOf.Plant_HealrootWild,
				ThingDefOf.Glowstool,
				ThingDefOf.Bryolux,
				ThingDefOf.Agarilux
			};
			foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(cell, map, GenStep_UndercaveInterest.PatchSizeRange.RandomInRange, null))
			{
				ThingDef thingDef = source.RandomElement<ThingDef>();
				if (GenSpawn.CanSpawnAt(thingDef, intVec, map, null, true))
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoilRich);
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					if (Rand.Chance(0.7f) && GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Direct, null, null, null, 1))
					{
						Plant plant = thing as Plant;
						if (plant != null)
						{
							plant.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
						}
					}
				}
			}
		}

		// Token: 0x0600B512 RID: 46354 RVA: 0x00340264 File Offset: 0x0033E464
		private void GenerateChemfuel(Map map, IntVec3 cell)
		{
			if (!GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.AncientGenerator ?? ThingDefOf.ChemfuelPoweredGenerator, null), cell, map, ThingPlaceMode.Direct, null, null, null, 1))
			{
				return;
			}
			int randomInRange = GenStep_UndercaveInterest.ChemfuelCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Chemfuel, null);
				thing.stackCount = GenStep_UndercaveInterest.ChemfuelStackCountRange.RandomInRange;
				GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 2);
			}
			foreach (IntVec3 c in GridShapeMaker.IrregularLump(cell, map, 20, null))
			{
				if (c.GetEdifice(map) == null)
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Fuel, 1, FilthSourceFlags.None, true);
				}
			}
		}

		// Token: 0x0600B513 RID: 46355 RVA: 0x0034034C File Offset: 0x0033E54C
		private void GenerateHive(Map map, IntVec3 cell)
		{
			HiveUtility.SpawnHive(cell, map, WipeMode.VanishOrMoveAside, false, false, true, false, true, false, true);
			int randomInRange = GenStep_UndercaveInterest.JellyCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.InsectJelly, null);
				thing.stackCount = GenStep_UndercaveInterest.JellyStackCountRange.RandomInRange;
				thing.SetForbidden(true, true);
				GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 3);
			}
		}

		// Token: 0x0600B514 RID: 46356 RVA: 0x003403C0 File Offset: 0x0033E5C0
		private void GenerateCorpseGear(Map map, IntVec3 cell)
		{
			List<ThingDef> list = new List<ThingDef>();
			list.Add(ThingDefOf.MedicineIndustrial);
			list.Add(ThingDefOf.MealSurvivalPack);
			Faction faction;
			Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Drifter, faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			pawn.health.SetDead();
			Corpse corpse = pawn.MakeCorpse(null, null);
			corpse.Age = Mathf.RoundToInt((float)(GenStep_UndercaveInterest.CorpseAgeRangeDays.RandomInRange * 60000));
			corpse.GetComp<CompRottable>().RotProgress += (float)corpse.Age;
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			GenSpawn.Spawn(corpse, cell, map, WipeMode.Vanish);
			Thing thing = ThingMaker.MakeThing(list.RandomElement<ThingDef>(), null);
			thing.stackCount = GenStep_UndercaveInterest.GearStackCountRange.RandomInRange;
			GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 1);
		}

		// Token: 0x0600B515 RID: 46357 RVA: 0x0034052C File Offset: 0x0033E72C
		public static void GenerateCorpsePile(Map map, IntVec3 cell)
		{
			int randomInRange = GenStep_UndercaveInterest.CorpseCountRange.RandomInRange;
			Faction faction;
			Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false);
			int age = Mathf.RoundToInt((float)(GenStep_UndercaveInterest.CorpseAgeRangeDays.RandomInRange * 60000));
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Drifter, faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.Kill(null, null);
				pawn.Corpse.Age = age;
				pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
				GenPlace.TryPlaceThing(pawn.Corpse, cell, map, ThingPlaceMode.Radius, null, null, null, 4);
				pawn.Corpse.SetForbidden(true, true);
			}
		}

		// Token: 0x0600B516 RID: 46358 RVA: 0x00340694 File Offset: 0x0033E894
		private void GenerateSleepingFleshbeasts(Map map, IntVec3 cell)
		{
			int randomInRange = GenStep_UndercaveInterest.NumFleshbeastsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn thing = PawnGenerator.GeneratePawn(PawnKindDefOf.Fingerspike, Faction.OfEntities, null);
				GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 4);
				CompCanBeDormant compCanBeDormant;
				if (thing.TryGetComp(out compCanBeDormant))
				{
					compCanBeDormant.ToSleep();
				}
			}
		}

		// Token: 0x04007C3B RID: 31803
		private static readonly IntRange InterestPointCountRange = new IntRange(3, 5);

		// Token: 0x04007C3C RID: 31804
		private const int InterestPointSize = 20;

		// Token: 0x04007C3D RID: 31805
		private const float MinDistApart = 10f;

		// Token: 0x04007C3E RID: 31806
		private const float PatchDensity = 0.7f;

		// Token: 0x04007C3F RID: 31807
		private static readonly IntRange PatchSizeRange = new IntRange(50, 70);

		// Token: 0x04007C40 RID: 31808
		private static readonly IntRange ChemfuelCountRange = new IntRange(3, 5);

		// Token: 0x04007C41 RID: 31809
		private static readonly IntRange ChemfuelStackCountRange = new IntRange(10, 20);

		// Token: 0x04007C42 RID: 31810
		private const int ChemfuelSpawnRadius = 2;

		// Token: 0x04007C43 RID: 31811
		private const int ChemfuelPuddleSize = 20;

		// Token: 0x04007C44 RID: 31812
		private static readonly IntRange JellyCountRange = new IntRange(2, 3);

		// Token: 0x04007C45 RID: 31813
		private static readonly IntRange JellyStackCountRange = new IntRange(15, 40);

		// Token: 0x04007C46 RID: 31814
		private const int JellySpawnRadius = 3;

		// Token: 0x04007C47 RID: 31815
		private static readonly IntRange CorpseAgeRangeDays = new IntRange(15, 120);

		// Token: 0x04007C48 RID: 31816
		private const int GearSpawnRadius = 1;

		// Token: 0x04007C49 RID: 31817
		private static readonly IntRange GearStackCountRange = new IntRange(2, 5);

		// Token: 0x04007C4A RID: 31818
		private static readonly IntRange CorpseCountRange = new IntRange(3, 6);

		// Token: 0x04007C4B RID: 31819
		private const int CorpseSpawnRadius = 4;

		// Token: 0x04007C4C RID: 31820
		private static readonly IntRange NumFleshbeastsRange = new IntRange(2, 4);

		// Token: 0x04007C4D RID: 31821
		private const int SleepingFleshbeastSpawnRadius = 4;

		// Token: 0x02002029 RID: 8233
		private enum UnderCaveInterestKind
		{
			// Token: 0x04007C4F RID: 31823
			MushroomPatch,
			// Token: 0x04007C50 RID: 31824
			ChemfuelGenerator,
			// Token: 0x04007C51 RID: 31825
			InsectHive,
			// Token: 0x04007C52 RID: 31826
			CorpseGear,
			// Token: 0x04007C53 RID: 31827
			CorpsePile,
			// Token: 0x04007C54 RID: 31828
			SleepingFleshbeasts
		}
	}
}
