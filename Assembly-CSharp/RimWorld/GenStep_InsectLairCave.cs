using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002017 RID: 8215
	public class GenStep_InsectLairCave : GenStep
	{
		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x0600B4D4 RID: 46292 RVA: 0x0033E387 File Offset: 0x0033C587
		public InsectLairEntrance Entrance
		{
			get
			{
				return this.exit.entrance as InsectLairEntrance;
			}
		}

		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x0600B4D5 RID: 46293 RVA: 0x0033E399 File Offset: 0x0033C599
		public override int SeedPart
		{
			get
			{
				return 98236175;
			}
		}

		// Token: 0x0600B4D6 RID: 46294 RVA: 0x0033E3A0 File Offset: 0x0033C5A0
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.poiCells.Clear();
			ModuleBase moduleBase = new Perlin(0.1, 0.9, 1.5, 4, Rand.Int, QualityMode.Medium);
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.GetEdifice(map) == null && GenConstruct.CanBuildOnTerrain(TerrainDefOf.InsectSludge, intVec, map, Rot4.North, null, null) && moduleBase.GetValue(intVec) > 0.2f)
				{
					this.TryPlaceSludge(intVec, map);
				}
			}
			for (int i = 0; i < GenStep_InsectLairCave.NumThreatPOIsRange.max + GenStep_InsectLairCave.NumMiscPOIsRange.max + 1; i++)
			{
				IntVec3 poilocation = this.GetPOILocation(map);
				if (poilocation.IsValid)
				{
					this.poiCells.Add(poilocation);
				}
			}
			GenStep_InsectLairCave.GenerateRandomBurrowWalls(map);
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				if (Rand.Chance(0.02f) && intVec2.GetTerrain(map) == TerrainDefOf.InsectSludge)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.EggSac, null);
					thing.SetFaction(Faction.OfInsects, null);
					GenPlace.TryPlaceThing(thing, intVec2, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
			this.exit = (map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>() as PocketMapExit);
			if (this.exit == null)
			{
				Log.Error("No cave entrance found for Insect Lair POI generation.");
			}
			this.GenerateBossPOI(map);
			this.GenerateThreatPOIs(map);
			this.GenerateMiscPOIs(map);
			PocketMapParent pocketMapParent = map.Parent as PocketMapParent;
			Map target = ((pocketMapParent != null) ? pocketMapParent.sourceMap : null) ?? map;
			this.GenerateRandomHives(map, Mathf.RoundToInt(GenStep_InsectLairCave.HiveThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target))));
		}

		// Token: 0x0600B4D7 RID: 46295 RVA: 0x0033E5A0 File Offset: 0x0033C7A0
		private static void GenerateRandomBurrowWalls(Map map)
		{
			Perlin perlin = new Perlin(0.08, 2.0, 2.0, 2, Rand.Int, QualityMode.Medium);
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = perlin.GetValue(intVec);
				if (intVec.GetFirstBuilding(map) == null && value > 0.7f && GenSpawn.CanSpawnAt(ThingDefOf.BurrowWall, intVec, map, null, true))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.BurrowWall, null);
					if (GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Direct, null, null, null, 1))
					{
						thing.SetFaction(Faction.OfInsects, null);
					}
				}
			}
		}

		// Token: 0x0600B4D8 RID: 46296 RVA: 0x0033E674 File Offset: 0x0033C874
		private void GenerateBossPOI(Map map)
		{
			IntVec3 intVec = this.poiCells.OrderBy(delegate(IntVec3 c)
			{
				PocketMapExit pocketMapExit = this.exit;
				return -c.DistanceToSquared((pocketMapExit != null) ? pocketMapExit.Position : map.Center);
			}).First<IntVec3>();
			this.poiCells.Remove(intVec);
			this.ClearArea(intVec, map);
			List<IntVec3> cells;
			this.GeneratePOIWalls(intVec, map, out cells);
			this.GenerateBossRoom(intVec, map);
			string signalTag = "queenApproached-" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			CellRect rect = CellRect.FromCellList(cells).ExpandedBy(2).ClipInsideMap(map);
			RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			rectTrigger.destroyIfUnfogged = true;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
			signalAction_Letter.signalTag = signalTag;
			signalAction_Letter.letterDef = LetterDefOf.ThreatBig;
			signalAction_Letter.letterLabelKey = "LetterLabelInsectQueenWarning";
			signalAction_Letter.letterMessageKey = "LetterInsectQueenWarning";
			GenSpawn.Spawn(signalAction_Letter, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x0600B4D9 RID: 46297 RVA: 0x0033E7B0 File Offset: 0x0033C9B0
		private void GenerateThreatPOIs(Map map)
		{
			int randomInRange = GenStep_InsectLairCave.NumThreatPOIsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				GenStep_InsectLairCave.ThreatPOIType threatPOIType = Rand.EnumValue<GenStep_InsectLairCave.ThreatPOIType>();
				if (!this.poiCells.Empty<IntVec3>())
				{
					IntVec3 intVec = this.poiCells.RandomElement<IntVec3>();
					this.poiCells.Remove(intVec);
					this.ClearArea(intVec, map);
					List<IntVec3> list;
					this.GeneratePOIWalls(intVec, map, out list);
					switch (threatPOIType)
					{
					case GenStep_InsectLairCave.ThreatPOIType.InsectHives:
						this.GenerateInsectHivePOI(intVec, map);
						break;
					case GenStep_InsectLairCave.ThreatPOIType.InsectCocoons:
						this.GenerateInsectCocoonPOI(intVec, map);
						break;
					case GenStep_InsectLairCave.ThreatPOIType.Ambush:
						this.GenerateAmbushPOI(intVec, map);
						break;
					}
				}
			}
		}

		// Token: 0x0600B4DA RID: 46298 RVA: 0x0033E84C File Offset: 0x0033CA4C
		private void GenerateMiscPOIs(Map map)
		{
			int randomInRange = GenStep_InsectLairCave.NumMiscPOIsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				GenStep_InsectLairCave.MiscPOIType miscPOIType = Rand.EnumValue<GenStep_InsectLairCave.MiscPOIType>();
				if (!this.poiCells.Empty<IntVec3>())
				{
					IntVec3 intVec = this.poiCells.RandomElement<IntVec3>();
					this.poiCells.Remove(intVec);
					this.ClearArea(intVec, map);
					List<IntVec3> list;
					this.GeneratePOIWalls(intVec, map, out list);
					switch (miscPOIType)
					{
					case GenStep_InsectLairCave.MiscPOIType.EggSacs:
						this.GenerateEggSacs(intVec, map, Rand.Range(5, 10));
						break;
					case GenStep_InsectLairCave.MiscPOIType.BoomShrooms:
						this.GeneratePlants(ThingDefOf.Boomshroom, intVec, map, Rand.Range(5, 10));
						break;
					case GenStep_InsectLairCave.MiscPOIType.Mushrooms:
						this.GeneratePlants(ThingDefOf.Plant_Psilocap, intVec, map, Rand.Range(5, 10));
						this.GeneratePlants(ThingDefOf.Agarilux, intVec, map, Rand.Range(5, 10));
						break;
					case GenStep_InsectLairCave.MiscPOIType.Corpses:
						GenStep_UndercaveInterest.GenerateCorpsePile(map, intVec);
						break;
					case GenStep_InsectLairCave.MiscPOIType.HermeticCrate:
						this.GenerateHermeticCratePOI(intVec, map);
						break;
					}
				}
			}
		}

		// Token: 0x0600B4DB RID: 46299 RVA: 0x0033E948 File Offset: 0x0033CB48
		private void GenerateRandomHives(Map map, int count)
		{
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < count; i++)
			{
				Map map2 = map;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, map, null, true) && !c.InHorDistOf(this.exit.Position, 20f)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryFindRandomCell(map2, validator, out spawnCell))
				{
					HiveUtility.SpawnHive(spawnCell, map, WipeMode.VanishOrMoveAside, true, false, true, false, true, false, true);
				}
			}
		}

		// Token: 0x0600B4DC RID: 46300 RVA: 0x0033E9B8 File Offset: 0x0033CBB8
		private IntVec3 GetPOILocation(Map map)
		{
			Thing entrance = map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>();
			if (entrance == null)
			{
				Log.Error("No cave entrance found for Insect Lair POI generation.");
				return IntVec3.Invalid;
			}
			IntVec3 result;
			if (!CellFinder.TryFindRandomCell(map, delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					return false;
				}
				if (c.DistanceToEdge(map) < 10)
				{
					return false;
				}
				if (entrance.Position.InHorDistOf(c, 20f))
				{
					return false;
				}
				foreach (IntVec3 otherLoc in this.poiCells)
				{
					if (c.InHorDistOf(otherLoc, 15f))
					{
						return false;
					}
				}
				return map.reachability.CanReach(c, entrance.Position, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors);
			}, out result))
			{
				return IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x0600B4DD RID: 46301 RVA: 0x0033EA34 File Offset: 0x0033CC34
		private void ClearArea(IntVec3 loc, Map map)
		{
			foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(loc, map, 50, (IntVec3 c) => true))
			{
				Building edifice = c2.GetEdifice(map);
				if (edifice != null)
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600B4DE RID: 46302 RVA: 0x0033EAB4 File Offset: 0x0033CCB4
		private void GeneratePOIWalls(IntVec3 loc, Map map, out List<IntVec3> shape)
		{
			shape = GridShapeMaker.IrregularLump(loc, map, 154, (IntVec3 c) => true);
			foreach (IntVec3 cell in shape)
			{
				this.TryPlaceSludge(cell, map);
			}
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 intVec in shape)
			{
				bool flag = false;
				foreach (IntVec3 item in GenRadial.RadialCellsAround(intVec, 2f, false))
				{
					if (!shape.Contains(item))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(intVec);
				}
			}
			foreach (IntVec3 item2 in list)
			{
				shape.Remove(item2);
			}
			foreach (IntVec3 intVec2 in shape)
			{
				if (intVec2.DistanceToEdge(map) > 1)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.BurrowWall, null);
					if (GenPlace.TryPlaceThing(thing, intVec2, map, ThingPlaceMode.Direct, null, null, null, 1))
					{
						thing.SetFaction(Faction.OfInsects, null);
					}
				}
			}
		}

		// Token: 0x0600B4DF RID: 46303 RVA: 0x0033EC84 File Offset: 0x0033CE84
		private void GenerateEggSacs(IntVec3 loc, Map map, int num)
		{
			for (int i = 0; i < num; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.EggSac, null);
				thing.SetFaction(Faction.OfInsects, null);
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Radius, null, null, null, 5);
			}
		}

		// Token: 0x0600B4E0 RID: 46304 RVA: 0x0033ECCC File Offset: 0x0033CECC
		private void GenerateHermeticCratePOI(IntVec3 loc, Map map)
		{
			Rot4 rot = Rot4.Random;
			IntVec3 cell;
			if (!CellFinder.TryRandomClosewalkCellNear(loc, map, 5, out cell, (IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.AncientHermeticCrate, c, map, new Rot4?(rot), true)))
			{
				return;
			}
			RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, map, rot, ThingSetMakerDefOf.MapGen_HighValueCrate, true, null);
			int num = Rand.Range(3, 4);
			this.GenerateEggSacs(loc, map, num);
		}

		// Token: 0x0600B4E1 RID: 46305 RVA: 0x0033ED44 File Offset: 0x0033CF44
		private void GenerateBossRoom(IntVec3 loc, Map map)
		{
			GenStep_InsectLairCave.<>c__DisplayClass33_0 CS$<>8__locals1 = new GenStep_InsectLairCave.<>c__DisplayClass33_0();
			CS$<>8__locals1.map = map;
			IntVec3 intVec;
			if (!CellFinder.TryRandomClosewalkCellNear(loc, CS$<>8__locals1.map, 5, out intVec, null))
			{
				return;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.HiveQueen, Faction.OfInsects, null);
			pawn.TryGetComp<CompCanBeDormant>().ToSleep();
			GenSpawn.Spawn(pawn, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
			LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_HiveQueen(Faction.OfInsects, intVec, 12f, false, false), CS$<>8__locals1.map, Gen.YieldSingle<Pawn>(pawn));
			int num = Rand.Range(3, 4);
			this.GenerateEggSacs(loc, CS$<>8__locals1.map, num);
			int num2 = (int)GenStep_InsectLairCave.BossHivesThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
			for (int i = 0; i < num2; i++)
			{
				Map map2 = CS$<>8__locals1.map;
				int radius = 5;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = CS$<>8__locals1.<>9__0) == null)
				{
					extraValidator = (CS$<>8__locals1.<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, CS$<>8__locals1.map, null, true)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryRandomClosewalkCellNear(loc, map2, radius, out spawnCell, extraValidator))
				{
					HiveUtility.SpawnHive(spawnCell, CS$<>8__locals1.map, WipeMode.VanishOrMoveAside, false, false, true, true, true, false, true);
				}
			}
			if (this.Entrance != null && this.Entrance.spawnGravcore)
			{
				GenSpawn.Spawn(ThingDefOf.Gravcore, loc, CS$<>8__locals1.map, WipeMode.Vanish).SetForbidden(true, true);
				int num3 = Rand.Range(2, 4);
				for (int j = 0; j < num3; j++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ShipChunk_Mech, null), loc, CS$<>8__locals1.map, ThingPlaceMode.Radius, null, null, null, 5);
				}
				int k = GenStep_InsectLairCave.GravlitePanelCount.RandomInRange;
				while (k > 0)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.GravlitePanel, null);
					thing.stackCount = Mathf.Min(k, thing.def.stackLimit);
					k -= thing.stackCount;
					thing.SetForbidden(true, true);
					GenPlace.TryPlaceThing(thing, loc, CS$<>8__locals1.map, ThingPlaceMode.Near, null, null, null, 1);
				}
				return;
			}
			Rot4 rot = Rot4.Random;
			IntVec3 cell;
			if (!CellFinder.TryRandomClosewalkCellNear(loc, CS$<>8__locals1.map, 5, out cell, (IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.AncientHermeticCrate, c, CS$<>8__locals1.map, new Rot4?(rot), true)))
			{
				return;
			}
			RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, CS$<>8__locals1.map, rot, ThingSetMakerDefOf.MapGen_HighValueCrate, true, null);
		}

		// Token: 0x0600B4E2 RID: 46306 RVA: 0x0033EFA0 File Offset: 0x0033D1A0
		private void GenerateInsectHivePOI(IntVec3 loc, Map map)
		{
			int num = (int)GenStep_InsectLairCave.HivesFromThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < num; i++)
			{
				Map map2 = map;
				int radius = 5;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, map, null, true)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryRandomClosewalkCellNear(loc, map2, radius, out spawnCell, extraValidator))
				{
					HiveUtility.SpawnHive(spawnCell, map, WipeMode.VanishOrMoveAside, false, false, true, true, true, false, true);
				}
			}
			int num2 = Rand.Range(2, 4);
			this.GenerateEggSacs(loc, map, num2);
		}

		// Token: 0x0600B4E3 RID: 46307 RVA: 0x0033F03C File Offset: 0x0033D23C
		private void GenerateInsectCocoonPOI(IntVec3 loc, Map map)
		{
			IEnumerable<ThingDef> cocoonsToSpawn = CocoonInfestationUtility.GetCocoonsToSpawn(GenStep_InsectLairCave.CocoonThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap)));
			int nextCocoonGroupID = Find.UniqueIDsManager.GetNextCocoonGroupID();
			foreach (ThingDef def in cocoonsToSpawn)
			{
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.SetFaction(Faction.OfInsects, null);
				thing.TryGetComp<CompWakeUpDormant>().groupID = nextCocoonGroupID;
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Radius, null, null, null, 5);
			}
		}

		// Token: 0x0600B4E4 RID: 46308 RVA: 0x0033F0D4 File Offset: 0x0033D2D4
		private void GenerateAmbushPOI(IntVec3 loc, Map map)
		{
			GenSpawn.Spawn(ThingDefOf.InsectAmbush, loc, map, WipeMode.Vanish);
			int num = Rand.Range(5, 10);
			this.GenerateEggSacs(loc, map, num);
		}

		// Token: 0x0600B4E5 RID: 46309 RVA: 0x0033F104 File Offset: 0x0033D304
		private void GeneratePlants(ThingDef plantDef, IntVec3 loc, Map map, int count)
		{
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < count; i++)
			{
				Thing thing = ThingMaker.MakeThing(plantDef, null);
				Thing thing2 = thing;
				Map map2 = map;
				ThingPlaceMode mode = ThingPlaceMode.Radius;
				Action<Thing, int> placedAction = null;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 c) => c.GetPlant(map) == null));
				}
				Thing thing3;
				if (GenPlace.TryPlaceThing(thing2, loc, map2, mode, out thing3, placedAction, extraValidator, null, 5))
				{
					(thing as Plant).Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
				}
			}
		}

		// Token: 0x0600B4E6 RID: 46310 RVA: 0x0033F190 File Offset: 0x0033D390
		private void TryPlaceSludge(IntVec3 cell, Map map)
		{
			if (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.InsectSludge, cell, map, Rot4.North, null, null))
			{
				return;
			}
			map.terrainGrid.SetTerrain(cell, TerrainDefOf.InsectSludge);
			if (Rand.Chance(0.2f))
			{
				GenSpawn.Spawn(ThingDefOf.Filth_Slime, cell, map, WipeMode.Vanish);
			}
		}

		// Token: 0x04007BFD RID: 31741
		private const int MinDistBetweenPOIs = 15;

		// Token: 0x04007BFE RID: 31742
		private const int MinDistFromEdge = 10;

		// Token: 0x04007BFF RID: 31743
		private const int MinDistFromEntrance = 20;

		// Token: 0x04007C00 RID: 31744
		private const float SludgeThreshold = 0.2f;

		// Token: 0x04007C01 RID: 31745
		public const float SlimeChance = 0.2f;

		// Token: 0x04007C02 RID: 31746
		public const float SacChance = 0.02f;

		// Token: 0x04007C03 RID: 31747
		private const int BurrowWallSize = 154;

		// Token: 0x04007C04 RID: 31748
		private static readonly IntRange NumThreatPOIsRange = new IntRange(2, 3);

		// Token: 0x04007C05 RID: 31749
		private static readonly IntRange NumMiscPOIsRange = new IntRange(3, 5);

		// Token: 0x04007C06 RID: 31750
		private static readonly IntRange GravlitePanelCount = new IntRange(100, 150);

		// Token: 0x04007C07 RID: 31751
		private static readonly SimpleCurve HivesFromThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(5000f, 4f),
				true
			},
			{
				new CurvePoint(10000f, 8f),
				true
			}
		};

		// Token: 0x04007C08 RID: 31752
		private static readonly SimpleCurve CocoonThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(1000f, 500f),
				true
			},
			{
				new CurvePoint(10000f, 2500f),
				true
			}
		};

		// Token: 0x04007C09 RID: 31753
		private static readonly SimpleCurve HiveThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(5000f, 7f),
				true
			},
			{
				new CurvePoint(10000f, 10f),
				true
			}
		};

		// Token: 0x04007C0A RID: 31754
		private static readonly SimpleCurve BossHivesThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(5000f, 4f),
				true
			},
			{
				new CurvePoint(10000f, 8f),
				true
			}
		};

		// Token: 0x04007C0B RID: 31755
		private List<IntVec3> poiCells = new List<IntVec3>();

		// Token: 0x04007C0C RID: 31756
		private PocketMapExit exit;

		// Token: 0x02002018 RID: 8216
		private enum ThreatPOIType
		{
			// Token: 0x04007C0E RID: 31758
			InsectHives,
			// Token: 0x04007C0F RID: 31759
			InsectCocoons,
			// Token: 0x04007C10 RID: 31760
			Ambush
		}

		// Token: 0x02002019 RID: 8217
		private enum MiscPOIType
		{
			// Token: 0x04007C12 RID: 31762
			EggSacs,
			// Token: 0x04007C13 RID: 31763
			BoomShrooms,
			// Token: 0x04007C14 RID: 31764
			Mushrooms,
			// Token: 0x04007C15 RID: 31765
			Corpses,
			// Token: 0x04007C16 RID: 31766
			HermeticCrate
		}
	}
}
