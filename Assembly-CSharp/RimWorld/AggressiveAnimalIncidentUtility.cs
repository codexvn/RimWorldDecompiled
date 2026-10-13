using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D71 RID: 7537
	public static class AggressiveAnimalIncidentUtility
	{
		// Token: 0x0600A8EA RID: 43242 RVA: 0x0030593C File Offset: 0x00303B3C
		public static float AnimalWeight(PawnKindDef animal, float points)
		{
			points = Mathf.Max(points, 70f);
			if (animal.combatPower * 2f > points)
			{
				return 0f;
			}
			int num = Mathf.Min(Mathf.RoundToInt(points / animal.combatPower), 100);
			return Mathf.Clamp01(Mathf.InverseLerp(100f, 10f, (float)num));
		}

		// Token: 0x0600A8EB RID: 43243 RVA: 0x00305998 File Offset: 0x00303B98
		public static bool TryFindAggressiveAnimalKind(float points, PlanetTile tile, out PawnKindDef animalKind)
		{
			List<PawnKindDef> animals = (from k in DefDatabase<PawnKindDef>.AllDefs
			where AggressiveAnimalIncidentUtility.CanArriveManhunter(k) && (!tile.Valid || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k).ToList<PawnKindDef>();
			return AggressiveAnimalIncidentUtility.TryGetHabitatAnimal(tile, out animalKind) || AggressiveAnimalIncidentUtility.TryGetAnimalFromList(points, animals, out animalKind);
		}

		// Token: 0x0600A8EC RID: 43244 RVA: 0x003059E8 File Offset: 0x00303BE8
		public static bool TryFindAggressiveAnimalKind(float points, Map map, out PawnKindDef animalKind)
		{
			if (AggressiveAnimalIncidentUtility.TryGetHabitatAnimal(map.Tile, out animalKind))
			{
				return true;
			}
			bool polluted = ModsConfig.BiotechActive && map.Tile.Valid && Rand.Value < WildAnimalSpawner.PollutionAnimalSpawnChanceFromPollutionCurve.Evaluate(Find.WorldGrid[map.Tile].pollution);
			List<PawnKindDef> list = (from k in map.Biomes.SelectMany((BiomeDef b) => b.AllWildAnimals)
			where AggressiveAnimalIncidentUtility.CanArriveWithPollution(k, map, polluted) && AggressiveAnimalIncidentUtility.CanArriveManhunter(k)
			select k).ToList<PawnKindDef>();
			if (polluted && list.Count == 0)
			{
				polluted = false;
				list = (from k in map.Biomes.SelectMany((BiomeDef b) => b.AllWildAnimals)
				where AggressiveAnimalIncidentUtility.CanArriveWithPollution(k, map, polluted) && AggressiveAnimalIncidentUtility.CanArriveManhunter(k)
				select k).ToList<PawnKindDef>();
			}
			if (AggressiveAnimalIncidentUtility.TryGetAnimalFromList(points, list, out animalKind))
			{
				return true;
			}
			list = (from k in DefDatabase<PawnKindDef>.AllDefs
			where AggressiveAnimalIncidentUtility.CanArriveManhunter(k) && AggressiveAnimalIncidentUtility.CanArriveWithPollution(k, map, polluted) && (!map.Tile.Valid || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(map.Tile, k.race))
			select k).ToList<PawnKindDef>();
			return AggressiveAnimalIncidentUtility.TryGetAnimalFromList(points, list, out animalKind);
		}

		// Token: 0x0600A8ED RID: 43245 RVA: 0x00305B3C File Offset: 0x00303D3C
		private static bool TryGetAnimalFromList(float points, List<PawnKindDef> animals, out PawnKindDef animalKind)
		{
			if (animals.Any<PawnKindDef>())
			{
				if (animals.TryRandomElementByWeight((PawnKindDef a) => AggressiveAnimalIncidentUtility.AnimalWeight(a, points), out animalKind))
				{
					return true;
				}
				if (points > animals.Min((PawnKindDef a) => a.combatPower) * 2f)
				{
					animalKind = animals.MaxBy((PawnKindDef a) => a.combatPower);
					return true;
				}
			}
			animalKind = null;
			return false;
		}

		// Token: 0x0600A8EE RID: 43246 RVA: 0x00305BD8 File Offset: 0x00303DD8
		private static bool TryGetHabitatAnimal(PlanetTile tile, out PawnKindDef animalKind)
		{
			animalKind = null;
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!tile.Valid)
			{
				return false;
			}
			if (Find.WorldGrid[tile].Mutators.Contains(TileMutatorDefOf.AnimalHabitat))
			{
				TileMutatorWorker_AnimalHabitat tileMutatorWorker_AnimalHabitat = (TileMutatorWorker_AnimalHabitat)TileMutatorDefOf.AnimalHabitat.Worker;
				animalKind = tileMutatorWorker_AnimalHabitat.GetAnimalKind(tile);
				if (AggressiveAnimalIncidentUtility.CanArriveManhunter(animalKind) && Rand.Chance(0.5f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A8EF RID: 43247 RVA: 0x00305C49 File Offset: 0x00303E49
		public static int GetAnimalsCount(PawnKindDef animalKind, float points)
		{
			return Mathf.Clamp(Mathf.RoundToInt(points / animalKind.combatPower), 2, 100);
		}

		// Token: 0x0600A8F0 RID: 43248 RVA: 0x00305C60 File Offset: 0x00303E60
		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, PlanetTile tile, float points, int animalCount = 0)
		{
			List<Pawn> list = new List<Pawn>();
			int num = (animalCount > 0) ? animalCount : AggressiveAnimalIncidentUtility.GetAnimalsCount(animalKind, points);
			for (int i = 0; i < num; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600A8F1 RID: 43249 RVA: 0x00305D30 File Offset: 0x00303F30
		public static List<Pawn> GenerateAnimals(List<PawnKindDef> animalKinds, PlanetTile tile)
		{
			List<Pawn> list = new List<Pawn>();
			foreach (PawnKindDef kind in animalKinds)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600A8F2 RID: 43250 RVA: 0x00305E1C File Offset: 0x0030401C
		[DebugOutput]
		public static void ManhunterResults()
		{
			List<PawnKindDef> candidates = (from k in DefDatabase<PawnKindDef>.AllDefs.Where(new Func<PawnKindDef, bool>(AggressiveAnimalIncidentUtility.CanArriveManhunter))
			orderby -k.combatPower
			select k).ToList<PawnKindDef>();
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add(20f * Mathf.Pow(1.25f, (float)i));
			}
			DebugTables.MakeTablesDialog<float, PawnKindDef>(list, (float points) => points.ToString("F0") + " pts", candidates, (PawnKindDef candidate) => candidate.defName + " (" + candidate.combatPower.ToString("F0") + ")", delegate(float points, PawnKindDef candidate)
			{
				float num = candidates.Sum((PawnKindDef k) => AggressiveAnimalIncidentUtility.AnimalWeight(k, points));
				float num2 = AggressiveAnimalIncidentUtility.AnimalWeight(candidate, points);
				if (num2 != 0f)
				{
					return (num2 * 100f / num).ToString("F0") + string.Format("%, {0}", Mathf.Max(Mathf.RoundToInt(points / candidate.combatPower), 1));
				}
				return "0%";
			}, "");
		}

		// Token: 0x0600A8F3 RID: 43251 RVA: 0x00305EFA File Offset: 0x003040FA
		private static bool CanArriveManhunter(PawnKindDef kind)
		{
			return kind.RaceProps.Animal && kind.canArriveManhunter && kind.RaceProps.CanPassFences;
		}

		// Token: 0x0600A8F4 RID: 43252 RVA: 0x00305F20 File Offset: 0x00304120
		private static bool CanArriveWithPollution(PawnKindDef k, Map map, bool polluted)
		{
			if (polluted)
			{
				return map.Biomes.Any((BiomeDef b) => b.CommonalityOfPollutionAnimal(k) > 0f);
			}
			if (map.TileInfo.IsCoastal)
			{
				return map.Biomes.Any((BiomeDef b) => b.CommonalityOfAnimal(k) > 0f || b.CommonalityOfCoastalAnimal(k) > 0f);
			}
			return map.Biomes.Any((BiomeDef b) => b.CommonalityOfAnimal(k) > 0f);
		}

		// Token: 0x040074ED RID: 29933
		public const int MinAnimalCount = 2;

		// Token: 0x040074EE RID: 29934
		public const int MaxAnimalCount = 100;

		// Token: 0x040074EF RID: 29935
		public const float MinPoints = 70f;
	}
}
