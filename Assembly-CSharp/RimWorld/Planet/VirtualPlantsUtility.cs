using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CCD RID: 15565
	public static class VirtualPlantsUtility
	{
		// Token: 0x06015285 RID: 86661 RVA: 0x00655090 File Offset: 0x00653290
		public static bool CanEverEatVirtualPlants(Pawn p)
		{
			return p.RaceProps.Eats(FoodTypeFlags.Plant);
		}

		// Token: 0x06015286 RID: 86662 RVA: 0x0065509F File Offset: 0x0065329F
		public static bool CanEatVirtualPlantsNow(Pawn p)
		{
			return VirtualPlantsUtility.CanEatVirtualPlants(p, GenTicks.TicksAbs);
		}

		// Token: 0x06015287 RID: 86663 RVA: 0x006550AC File Offset: 0x006532AC
		public static bool CanEatVirtualPlants(Pawn p, int ticksAbs)
		{
			return p.Tile.Valid && !p.Dead && p.IsWorldPawn() && VirtualPlantsUtility.CanEverEatVirtualPlants(p) && VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(p.Tile, ticksAbs);
		}

		// Token: 0x06015288 RID: 86664 RVA: 0x006550EF File Offset: 0x006532EF
		public static bool EnvironmentAllowsEatingVirtualPlantsNowAt(PlanetTile tile)
		{
			return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, GenTicks.TicksAbs);
		}

		// Token: 0x06015289 RID: 86665 RVA: 0x006550FC File Offset: 0x006532FC
		public static bool EnvironmentAllowsEatingVirtualPlantsAt(PlanetTile tile, int ticksAbs)
		{
			return Find.WorldGrid[tile].PrimaryBiome.hasVirtualPlants && GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile) >= 0f;
		}

		// Token: 0x0601528A RID: 86666 RVA: 0x00655128 File Offset: 0x00653328
		public static void EatVirtualPlants(Pawn p)
		{
			float num = ThingDefOf.Plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition, null) * VirtualPlantsUtility.VirtualPlantNutritionRandomFactor.RandomInRange;
			p.needs.food.CurLevel += num;
		}

		// Token: 0x0601528B RID: 86667 RVA: 0x0065516C File Offset: 0x0065336C
		public static string GetVirtualPlantsStatusExplanationAt(PlanetTile tile, int ticksAbs)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ticksAbs == GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsCanGrazeNow".Translate());
			}
			else if (ticksAbs > GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsWillBeAbleToGraze".Translate());
			}
			else
			{
				stringBuilder.Append("AnimalsCanGraze".Translate());
			}
			stringBuilder.Append(": ");
			bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs);
			stringBuilder.Append(flag ? "Yes".Translate() : "No".Translate());
			if (flag)
			{
				float? approxDaysUntilPossibleToGraze = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, true);
				if (approxDaysUntilPossibleToGraze != null)
				{
					stringBuilder.Append("\n" + "PossibleToGrazeFor".Translate(approxDaysUntilPossibleToGraze.Value.ToString("0.#")));
				}
				else
				{
					stringBuilder.Append("\n" + "PossibleToGrazeForever".Translate());
				}
			}
			else
			{
				if (!Find.WorldGrid[tile].PrimaryBiome.hasVirtualPlants)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfBiome".Translate(Find.WorldGrid[tile].PrimaryBiome.label));
				}
				float? approxDaysUntilPossibleToGraze2 = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, false);
				if (approxDaysUntilPossibleToGraze2 != null)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfTemp".Translate(approxDaysUntilPossibleToGraze2.Value.ToString("0.#")));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601528C RID: 86668 RVA: 0x0065532C File Offset: 0x0065352C
		public static float? GetApproxDaysUntilPossibleToGraze(PlanetTile tile, int ticksAbs, bool untilNoLongerPossibleToGraze = false)
		{
			if (!untilNoLongerPossibleToGraze && !Find.WorldGrid[tile].PrimaryBiome.hasVirtualPlants)
			{
				return null;
			}
			float num = 0f;
			for (int i = 0; i < Mathf.CeilToInt(133.33334f); i++)
			{
				bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs + (int)(num * 60000f));
				if ((!untilNoLongerPossibleToGraze && flag) || (untilNoLongerPossibleToGraze && !flag))
				{
					return new float?(num);
				}
				num += 0.45f;
			}
			return null;
		}

		// Token: 0x0400EC0F RID: 60431
		private static readonly FloatRange VirtualPlantNutritionRandomFactor = new FloatRange(0.7f, 1f);
	}
}
