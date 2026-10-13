using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003638 RID: 13880
	public static class FleshbeastUtility
	{
		// Token: 0x170031C8 RID: 12744
		// (get) Token: 0x06013459 RID: 78937 RVA: 0x005B7B09 File Offset: 0x005B5D09
		public static IEnumerable<PawnKindDef> AllFleshbeasts
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					yield break;
				}
				yield return PawnKindDefOf.Bulbfreak;
				yield return PawnKindDefOf.Fingerspike;
				yield return PawnKindDefOf.Trispike;
				yield return PawnKindDefOf.Toughspike;
				yield break;
			}
		}

		// Token: 0x0601345A RID: 78938 RVA: 0x005B7B12 File Offset: 0x005B5D12
		public static FleshbeastUtility.MeatExplosionSize ExplosionSizeFor(Pawn pawn)
		{
			if ((double)pawn.BodySize >= 3.5)
			{
				return FleshbeastUtility.MeatExplosionSize.Large;
			}
			return FleshbeastUtility.MeatExplosionSize.Normal;
		}

		// Token: 0x0601345B RID: 78939 RVA: 0x005B7B2C File Offset: 0x005B5D2C
		public static bool TryGiveMutation(Pawn pawn, HediffDef mutationDef)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (mutationDef.defaultInstallPart == null)
			{
				Log.ErrorOnce("Attempted to use mutation hediff which didn't specify a default install part (hediff: " + mutationDef.label, 194783821);
				return false;
			}
			List<BodyPartRecord> list = (from part in pawn.RaceProps.body.GetPartsWithDef(mutationDef.defaultInstallPart)
			where pawn.health.hediffSet.HasMissingPartFor(part)
			select part).ToList<BodyPartRecord>();
			List<BodyPartRecord> list2 = (from part in pawn.RaceProps.body.GetPartsWithDef(mutationDef.defaultInstallPart)
			where !pawn.health.hediffSet.HasDirectlyAddedPartFor(part)
			select part).ToList<BodyPartRecord>();
			BodyPartRecord bodyPartRecord = null;
			if (list.Any<BodyPartRecord>())
			{
				bodyPartRecord = list.RandomElement<BodyPartRecord>();
			}
			else if (list2.Any<BodyPartRecord>())
			{
				bodyPartRecord = list2.RandomElement<BodyPartRecord>();
			}
			if (bodyPartRecord == null)
			{
				return false;
			}
			MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, bodyPartRecord, pawn.PositionHeld, pawn.MapHeld);
			pawn.health.RestorePart(bodyPartRecord, null, true);
			pawn.health.AddHediff(mutationDef, bodyPartRecord, null, null);
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, pawn, null);
			hediff.Severity = 0.2f;
			pawn.health.AddHediff(hediff, null, null, null);
			for (int i = 0; i < 3; i++)
			{
				pawn.health.DropBloodFilth();
			}
			FleshbeastUtility.MeatSplatter(3, pawn.PositionHeld, pawn.MapHeld, FleshbeastUtility.MeatExplosionSize.Normal);
			return true;
		}

		// Token: 0x0601345C RID: 78940 RVA: 0x005B7CCC File Offset: 0x005B5ECC
		public static void MeatSplatter(int filthCount, IntVec3 pos, Map map, FleshbeastUtility.MeatExplosionSize size = FleshbeastUtility.MeatExplosionSize.Normal)
		{
			if (size == FleshbeastUtility.MeatExplosionSize.Small)
			{
				EffecterDefOf.MeatExplosionSmall.Spawn(pos, map, 1f).Cleanup();
			}
			else if (size == FleshbeastUtility.MeatExplosionSize.Normal)
			{
				EffecterDefOf.MeatExplosion.Spawn(pos, map, 1f).Cleanup();
			}
			else if (size == FleshbeastUtility.MeatExplosionSize.Large)
			{
				EffecterDefOf.MeatExplosionLarge.Spawn(pos, map, 1f).Cleanup();
			}
			CellRect cellRect = new CellRect(pos.x, pos.z, 3, 3).ClipInsideMap(map);
			for (int i = 0; i < filthCount; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				ThingDef filthDef = Rand.Bool ? ThingDefOf.Filth_Blood : ThingDefOf.Filth_TwistedFlesh;
				if (randomCell.InBounds(map) && GenSight.LineOfSight(randomCell, pos, map))
				{
					FilthMaker.TryMakeFilth(randomCell, map, filthDef, 1, FilthSourceFlags.None, true);
				}
			}
		}

		// Token: 0x0601345D RID: 78941 RVA: 0x005B7D90 File Offset: 0x005B5F90
		public static Pawn SpawnFleshbeastFromPawn(Pawn pawn, bool randomKind = false, bool dropGear = false, params PawnKindDef[] ignoreKinds)
		{
			if (!ModsConfig.AnomalyActive || pawn == null)
			{
				return null;
			}
			IntVec3 positionHeld = pawn.PositionHeld;
			Map mapHeld = pawn.MapHeld;
			EffecterDefOf.MeatExplosion.Spawn(pawn.PositionHeld, pawn.MapHeld, 1f).Cleanup();
			if (dropGear)
			{
				pawn.Strip(false);
			}
			if (pawn.Corpse != null)
			{
				pawn.Corpse.Destroy(DestroyMode.Vanish);
			}
			else
			{
				pawn.Destroy(DestroyMode.Vanish);
			}
			PawnKindDef pawnKindDef;
			if (randomKind)
			{
				pawnKindDef = (from x in FleshbeastUtility.AllFleshbeasts
				where !ignoreKinds.Contains(x)
				select x).RandomElement<PawnKindDef>();
			}
			else
			{
				pawnKindDef = FleshbeastUtility.FleshbeastForAnimal(pawn);
			}
			PawnKindDef kind = pawnKindDef;
			Faction ofEntities = Faction.OfEntities;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedBiologicalAge = new float?(0f);
			float? fixedChronologicalAge = new float?(0f);
			Pawn pawn2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, ofEntities, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			CompInspectStringEmergence compInspectStringEmergence = pawn2.TryGetComp<CompInspectStringEmergence>();
			if (compInspectStringEmergence != null)
			{
				compInspectStringEmergence.sourcePawn = pawn;
			}
			return (Pawn)GenSpawn.Spawn(pawn2, positionHeld, mapHeld, WipeMode.Vanish);
		}

		// Token: 0x0601345E RID: 78942 RVA: 0x005B7EF4 File Offset: 0x005B60F4
		public static Pawn SpawnFleshbeastFromGround(IntVec3 cell, Map map, IntRange delayTicks, float maxPoints = -1f)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return null;
			}
			PawnKindDef kindDef;
			if (maxPoints <= 0f || !(from x in FleshbeastUtility.AllFleshbeasts
			where x.combatPower <= maxPoints
			select x).TryRandomElement(out kindDef))
			{
				kindDef = FleshbeastUtility.AllFleshbeasts.RandomElement<PawnKindDef>();
			}
			Pawn pawn = PawnGenerator.GeneratePawn(kindDef, Faction.OfEntities, null);
			if (pawn != null)
			{
				FleshbeastGroundSpawner fleshbeastGroundSpawner = (FleshbeastGroundSpawner)ThingMaker.MakeThing(ThingDefOf.FleshbeastGroundSpawner, null);
				fleshbeastGroundSpawner.Init(pawn, delayTicks);
				GenSpawn.Spawn(fleshbeastGroundSpawner, cell, map, WipeMode.Vanish);
			}
			return pawn;
		}

		// Token: 0x0601345F RID: 78943 RVA: 0x005B7F88 File Offset: 0x005B6188
		public static Thing SpawnFleshbeastsFromPitBurrowEmergence(IntVec3 cell, Map map, float points, IntRange emergenceDelay, IntRange spawnDelay, bool assaultColony = true)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return null;
			}
			List<Pawn> fleshbeastsForPoints = FleshbeastUtility.GetFleshbeastsForPoints(points, map, false);
			BuildingGroundSpawner buildingGroundSpawner = (BuildingGroundSpawner)ThingMaker.MakeThing(ThingDefOf.PitBurrowSpawner, null);
			buildingGroundSpawner.emergeDelay = new IntRange?(emergenceDelay);
			PitBurrow pitBurrow = (PitBurrow)buildingGroundSpawner.ThingToSpawn;
			pitBurrow.emergingFleshbeasts = fleshbeastsForPoints;
			pitBurrow.emergeDelay = spawnDelay.RandomInRange;
			pitBurrow.assaultColony = assaultColony;
			GenSpawn.Spawn(buildingGroundSpawner, cell, map, WipeMode.Vanish);
			return buildingGroundSpawner;
		}

		// Token: 0x06013460 RID: 78944 RVA: 0x005B7FF4 File Offset: 0x005B61F4
		public static Thing SpawnPawnAsFlyer(Pawn pawn, Map map, IntVec3 rootCell, int jumpDist = 5, bool requiresLOS = true)
		{
			if (!pawn.Spawned)
			{
				GenSpawn.Spawn(pawn, rootCell, map, WipeMode.Vanish);
			}
			FleshbeastUtility.tmpTakenCells.Clear();
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomCellNearWith(rootCell, (IntVec3 c) => !c.Fogged(map) && c.Standable(map) && !FleshbeastUtility.tmpTakenCells.Contains(c) && c.GetFirstPawn(map) == null && (!requiresLOS || GenSight.LineOfSight(rootCell, c, map, true, null, 0, 0)), map, out intVec, 5, jumpDist))
			{
				pawn.rotationTracker.FaceCell(intVec);
				FleshbeastUtility.tmpTakenCells.Add(intVec);
				PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, null, null, default(LocalTargetInfo));
				if (pawnFlyer != null)
				{
					GenSpawn.Spawn(pawnFlyer, intVec, map, WipeMode.Vanish);
				}
				return pawnFlyer;
			}
			return null;
		}

		// Token: 0x06013461 RID: 78945 RVA: 0x005B80B6 File Offset: 0x005B62B6
		private static bool IsValidProstheticShoulder(Pawn pawn, BodyPartRecord part)
		{
			return pawn.health.hediffSet.HasDirectlyAddedPartFor(part) && pawn.health.hediffSet.GetDirectlyAddedPartFor(part).def != HediffDefOf.Tentacle;
		}

		// Token: 0x06013462 RID: 78946 RVA: 0x005B80ED File Offset: 0x005B62ED
		private static PawnKindDef FleshbeastForAnimal(Pawn animal)
		{
			if (animal.BodySize < 0.75f)
			{
				return PawnKindDefOf.Fingerspike;
			}
			if (animal.BodySize >= 3.5f)
			{
				return PawnKindDefOf.Bulbfreak;
			}
			if (!Rand.Bool)
			{
				return PawnKindDefOf.Trispike;
			}
			return PawnKindDefOf.Toughspike;
		}

		// Token: 0x06013463 RID: 78947 RVA: 0x005B8128 File Offset: 0x005B6328
		public static float ExistingFleshBeastThreat(Map map)
		{
			float num = 0f;
			foreach (Pawn pawn in map.mapPawns.PawnsInFaction(Faction.OfEntities))
			{
				if (pawn.kindDef.IsFleshBeast())
				{
					num += pawn.kindDef.combatPower;
				}
			}
			return num;
		}

		// Token: 0x06013464 RID: 78948 RVA: 0x005B81A0 File Offset: 0x005B63A0
		public static bool IsFleshBeast(this PawnKindDef pawnKind)
		{
			return pawnKind.RaceProps.FleshType == FleshTypeDefOf.Fleshbeast;
		}

		// Token: 0x06013465 RID: 78949 RVA: 0x005B81B4 File Offset: 0x005B63B4
		public static List<Pawn> GetFleshbeastsForPoints(float points, Map map, bool allowDreadmeld = false)
		{
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = (allowDreadmeld ? PawnGroupKindDefOf.FleshbeastsWithDreadmeld : PawnGroupKindDefOf.Fleshbeasts);
			pawnGroupMakerParms.tile = map.Tile;
			pawnGroupMakerParms.faction = Faction.OfEntities;
			pawnGroupMakerParms.points = ((points > 0f) ? points : StorytellerUtility.DefaultThreatPointsNow(map));
			pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, pawnGroupMakerParms.faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, null) * 1.05f);
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true).ToList<Pawn>();
		}

		// Token: 0x06013466 RID: 78950 RVA: 0x005B8244 File Offset: 0x005B6444
		public static IEnumerable<Pawn> SplitFleshbeast(Pawn fleshbeast)
		{
			DeathActionProperties_Divide divide = fleshbeast.RaceProps.deathAction as DeathActionProperties_Divide;
			if (divide == null)
			{
				yield return fleshbeast;
			}
			else
			{
				int num;
				for (int i = 0; i < divide.dividePawnCount; i = num + 1)
				{
					IEnumerable<Pawn> enumerable = FleshbeastUtility.SplitFleshbeast(PawnGenerator.GeneratePawn(new PawnGenerationRequest(divide.dividePawnKindOptions.RandomElement<PawnKindDef>(), fleshbeast.Faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)));
					foreach (Pawn pawn in enumerable)
					{
						yield return pawn;
					}
					IEnumerator<Pawn> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06013467 RID: 78951 RVA: 0x005B8254 File Offset: 0x005B6454
		public static void DoFleshbeastResponse(CompGrowsFleshmassTendrils source, IntVec3 position)
		{
			Map map = source.parent.Map;
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.Fleshmass_Active))
			{
				if (thing.Position.InHorDistOf(position, 30f))
				{
					foreach (IntVec3 b in GenAdj.AdjacentCells)
					{
						if ((thing.Position + b).Standable(map))
						{
							list.Add(thing);
							break;
						}
					}
				}
			}
			if (list.Empty<Thing>())
			{
				return;
			}
			SoundDefOf.FleshmassBirth.PlayOneShot(new TargetInfo(position, map, false));
			Find.CameraDriver.shaker.DoShake(1f);
			List<Pawn> fleshbeastsForPoints = FleshbeastUtility.GetFleshbeastsForPoints(FleshbeastUtility.FleshbeastResponsePointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(map)), map, false);
			List<PawnFlyer> list2 = new List<PawnFlyer>();
			List<IntVec3> list3 = new List<IntVec3>();
			Predicate<IntVec3> <>9__0;
			foreach (Pawn pawn in fleshbeastsForPoints)
			{
				Thing thing2 = list.RandomElement<Thing>();
				GenSpawn.Spawn(pawn, thing2.Position, map, WipeMode.Vanish);
				IntVec3 position2 = thing2.Position;
				Map map2 = map;
				int squareRadius = 2;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 cell) => !cell.Fogged(map) && cell.Walkable(map) && !cell.Impassable(map)));
				}
				IntVec3 intVec;
				CellFinder.TryFindRandomCellNear(position2, map2, squareRadius, validator, out intVec, -1);
				pawn.rotationTracker.FaceCell(intVec);
				list2.Add(PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, null, null, default(LocalTargetInfo)));
				list3.Add(thing2.Position);
			}
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), map, null);
			SpawnRequest spawnRequest = new SpawnRequest(list2.Cast<Thing>().ToList<Thing>(), list3, 1, 2f / (float)fleshbeastsForPoints.Count, lord);
			spawnRequest.spawnEffect = EffecterDefOf.MeatExplosionSmall;
			map.deferredSpawner.AddRequest(spawnRequest, true);
			Find.LetterStack.ReceiveLetter("FleshmassResponseLabel".Translate(), "FleshmassResponseText".Translate(), LetterDefOf.ThreatBig, fleshbeastsForPoints, null, null, null, null, 0, true);
		}

		// Token: 0x0400D5E8 RID: 54760
		private const float BloodLossSeverity = 0.2f;

		// Token: 0x0400D5E9 RID: 54761
		private const int BloodFilthCount = 3;

		// Token: 0x0400D5EA RID: 54762
		private static List<IntVec3> tmpTakenCells = new List<IntVec3>();

		// Token: 0x0400D5EB RID: 54763
		private const int FleshbeastBirthRange = 30;

		// Token: 0x0400D5EC RID: 54764
		private const float FleshbeastBirthShakeMagnitude = 1f;

		// Token: 0x0400D5ED RID: 54765
		private static readonly SimpleCurve FleshbeastResponsePointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(200f, 150f),
				true
			},
			{
				new CurvePoint(500f, 250f),
				true
			},
			{
				new CurvePoint(1000f, 400f),
				true
			},
			{
				new CurvePoint(5000f, 650f),
				true
			}
		};

		// Token: 0x02003639 RID: 13881
		public enum MeatExplosionSize
		{
			// Token: 0x0400D5EF RID: 54767
			Small,
			// Token: 0x0400D5F0 RID: 54768
			Normal,
			// Token: 0x0400D5F1 RID: 54769
			Large
		}
	}
}
