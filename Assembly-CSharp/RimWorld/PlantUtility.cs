using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B33 RID: 11059
	public static class PlantUtility
	{
		// Token: 0x0600F757 RID: 63319 RVA: 0x0047E290 File Offset: 0x0047C490
		public static bool GrowthSeasonNow(Map map, ThingDef plantDef)
		{
			float outdoorTemp = map.mapTemperature.OutdoorTemp;
			return outdoorTemp > plantDef.plant.minGrowthTemperature && outdoorTemp < plantDef.plant.maxGrowthTemperature;
		}

		// Token: 0x0600F758 RID: 63320 RVA: 0x0047E2C8 File Offset: 0x0047C4C8
		public static bool GrowthSeasonNow(IntVec3 c, Map map, ThingDef plantDef)
		{
			if (c.GetRoomOrAdjacent(map, RegionType.Set_All) == null)
			{
				return false;
			}
			float temperature = c.GetTemperature(map);
			return temperature > plantDef.plant.minGrowthTemperature && temperature < plantDef.plant.maxGrowthTemperature;
		}

		// Token: 0x0600F759 RID: 63321 RVA: 0x0047E308 File Offset: 0x0047C508
		public static bool SnowAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSnowDepth(map) < 0.2f;
		}

		// Token: 0x0600F75A RID: 63322 RVA: 0x0047E318 File Offset: 0x0047C518
		public static bool SandAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSandDepth(map) < 0.2f;
		}

		// Token: 0x0600F75B RID: 63323 RVA: 0x0047E328 File Offset: 0x0047C528
		public static bool CanNowPlantAt(this ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
		{
			if (!plantDef.CanEverPlantAt(c, map, canWipePlantsExceptTree, false))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				if (map.designationManager.DesignationOn(thing, DesignationDefOf.Uninstall) != null)
				{
					return false;
				}
				if (map.designationManager.DesignationOn(thing, DesignationDefOf.Deconstruct) != null)
				{
					return false;
				}
				Building building = thing as Building;
				Blueprint_Install blueprint_Install;
				if (building != null && map.listerBuildings.TryGetReinstallBlueprint(building, out blueprint_Install))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F75C RID: 63324 RVA: 0x0047E3D4 File Offset: 0x0047C5D4
		public static AcceptanceReport CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map, out Thing blockingThing, bool canWipePlantsExceptTree = false, bool checkMapTemperature = true, bool writeNoReason = false)
		{
			blockingThing = null;
			if (plantDef.category != ThingCategory.Plant)
			{
				Log.Error("Checking CanGrowAt with " + ((plantDef != null) ? plantDef.ToString() : null) + " which is not a plant.");
			}
			if (!c.InBounds(map))
			{
				if (!writeNoReason)
				{
					return "OutOfBounds".Translate();
				}
				return false;
			}
			else
			{
				TerrainDef terrain = c.GetTerrain(map);
				if (!plantDef.plant.completelyIgnoreFertility && map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin)
				{
					if (!writeNoReason)
					{
						return "MessageWarningNotEnoughFertility".Translate();
					}
					return false;
				}
				else if (checkMapTemperature && (map.TileInfo.MinTemperature > plantDef.plant.maxGrowthTemperature || map.TileInfo.MaxTemperature < plantDef.plant.minGrowthTemperature))
				{
					if (!writeNoReason)
					{
						return "CannotPlantExtremeTemp".Translate();
					}
					return false;
				}
				else
				{
					if (c.IsPolluted(map))
					{
						if (plantDef.plant.pollution == Pollution.CleanOnly)
						{
							if (!writeNoReason)
							{
								return "MessageWarningPollutedCell".Translate();
							}
							return false;
						}
					}
					else if (plantDef.plant.pollution == Pollution.PollutedOnly)
					{
						if (!writeNoReason)
						{
							return "MessageWarningNotPollutedCell".Translate();
						}
						return false;
					}
					if (!plantDef.plant.terraformable || CompTerraformer.CanEverConvertCell(c, map, null))
					{
						List<Thing> list = map.thingGrid.ThingsListAt(c);
						bool flag = false;
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i] is Building_PlantGrower)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (plantDef.plant.WildTerrainTags.Count > 0 && !plantDef.plant.WildTerrainTags.Overlaps(terrain.tags.OrElseEmptyEnumerable<string>()))
							{
								if (!writeNoReason)
								{
									return "CannotPlantMissingTerrainTag".Translate();
								}
								return false;
							}
							else if (plantDef.plant.terrainBlacklist != null && plantDef.plant.terrainBlacklist.Contains(terrain))
							{
								if (!writeNoReason)
								{
									return "CannotPlantMissingTerrainTag".Translate(terrain);
								}
								return false;
							}
						}
						int j = 0;
						while (j < list.Count)
						{
							Thing thing = list[j];
							if (!flag && thing.def.BlocksPlanting(canWipePlantsExceptTree))
							{
								blockingThing = thing;
								if (!writeNoReason)
								{
									return "BlockedBy".Translate(thing);
								}
								return false;
							}
							else
							{
								if (plantDef.passability == Traversability.Impassable)
								{
									if (thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building)
									{
										blockingThing = thing;
										if (!writeNoReason)
										{
											return "BlockedBy".Translate(thing);
										}
										return false;
									}
									else if (thing.def.category == ThingCategory.Plant && canWipePlantsExceptTree && thing.def.plant.IsTree)
									{
										blockingThing = thing;
										if (!writeNoReason)
										{
											return "BlockedBy".Translate(thing);
										}
										return false;
									}
								}
								j++;
							}
						}
						if (plantDef.passability == Traversability.Impassable)
						{
							for (int k = 0; k < 4; k++)
							{
								IntVec3 c2 = c + GenAdj.CardinalDirections[k];
								if (c2.InBounds(map))
								{
									Building edifice = c2.GetEdifice(map);
									if (edifice != null && edifice.def.IsDoor)
									{
										blockingThing = edifice;
										if (!writeNoReason)
										{
											return "BlockedBy".Translate(edifice);
										}
										return false;
									}
								}
							}
						}
						return true;
					}
					if (!writeNoReason)
					{
						return "MessageCannotBePlacedOn".Translate(terrain);
					}
					return false;
				}
			}
		}

		// Token: 0x0600F75D RID: 63325 RVA: 0x0047E7AC File Offset: 0x0047C9AC
		public static bool CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false, bool checkMapTemperature = true)
		{
			Thing thing;
			return plantDef.CanEverPlantAt(c, map, out thing, canWipePlantsExceptTree, checkMapTemperature, true).Accepted;
		}

		// Token: 0x0600F75E RID: 63326 RVA: 0x0047E7D0 File Offset: 0x0047C9D0
		public static void LogPlantProportions()
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			foreach (ThingDef key in Find.CurrentMap.wildPlantSpawner.AllWildPlants)
			{
				dictionary.Add(key, 0f);
			}
			float num = 0f;
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Plant plant = c.GetPlant(Find.CurrentMap);
				if (plant != null && dictionary.ContainsKey(plant.def))
				{
					Dictionary<ThingDef, float> dictionary2 = dictionary;
					ThingDef key2 = plant.def;
					float num2 = dictionary2[key2];
					dictionary2[key2] = num2 + 1f;
					num += 1f;
				}
			}
			foreach (ThingDef thingDef in Find.CurrentMap.wildPlantSpawner.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary3 = dictionary;
				ThingDef key2 = thingDef;
				dictionary3[key2] /= num;
			}
			Dictionary<ThingDef, float> dictionary4 = PlantUtility.CalculateDesiredPlantProportions(Find.CurrentMap.Biome);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("PLANT           EXPECTED             FOUND");
			foreach (ThingDef thingDef2 in Find.CurrentMap.wildPlantSpawner.AllWildPlants)
			{
				stringBuilder.AppendLine(thingDef2.LabelCap + "       " + dictionary4[thingDef2].ToStringPercent() + "        " + dictionary[thingDef2].ToStringPercent());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600F75F RID: 63327 RVA: 0x0047E9E4 File Offset: 0x0047CBE4
		private static Dictionary<ThingDef, float> CalculateDesiredPlantProportions(BiomeDef biome)
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			float num = 0f;
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.plant != null)
				{
					float num2 = biome.CommonalityOfPlant(thingDef);
					dictionary.Add(thingDef, num2);
					num += num2;
				}
			}
			foreach (ThingDef thingDef2 in biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary2 = dictionary;
				ThingDef key = thingDef2;
				dictionary2[key] /= num;
			}
			return dictionary;
		}

		// Token: 0x0600F760 RID: 63328 RVA: 0x0047EAAC File Offset: 0x0047CCAC
		public static IEnumerable<ThingDef> ValidPlantTypesForGrowers(List<IPlantToGrowSettable> sel)
		{
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef plantDef = enumerator.Current;
					if (sel.TrueForAll((IPlantToGrowSettable x) => PlantUtility.CanSowOnGrower(plantDef, x)))
					{
						yield return plantDef;
					}
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600F761 RID: 63329 RVA: 0x0047EABC File Offset: 0x0047CCBC
		public static bool CanSowOnGrower(ThingDef plantDef, object obj)
		{
			IPlantToGrowSettable plantToGrowSettable = obj as IPlantToGrowSettable;
			if (plantToGrowSettable != null && !PollutionUtility.CanPlantAt(plantDef, plantToGrowSettable))
			{
				return false;
			}
			if (obj is Zone)
			{
				return plantDef.plant.sowTags.Contains("Ground");
			}
			Thing thing = obj as Thing;
			return thing != null && thing.def.building != null && plantDef.plant.sowTags.Contains(thing.def.building.sowTag);
		}

		// Token: 0x0600F762 RID: 63330 RVA: 0x0047EB38 File Offset: 0x0047CD38
		public static Thing AdjacentSowBlocker(ThingDef plantDef, IntVec3 c, Map map)
		{
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Plant plant = c2.GetPlant(map);
					if (plant != null && (plant.def.plant.blockAdjacentSow || (plantDef.plant.blockAdjacentSow && plant.sown)))
					{
						return plant;
					}
				}
			}
			return null;
		}

		// Token: 0x0600F763 RID: 63331 RVA: 0x0047EBA1 File Offset: 0x0047CDA1
		public static byte GetWindExposure(Plant plant)
		{
			return (byte)Mathf.Min(255f * plant.def.plant.topWindExposure, 255f);
		}

		// Token: 0x0600F764 RID: 63332 RVA: 0x0047EBC4 File Offset: 0x0047CDC4
		public static void SetWindExposureColors(Color32[] colors, Plant plant)
		{
			colors[1].a = (colors[2].a = PlantUtility.GetWindExposure(plant));
			colors[0].a = (colors[3].a = 0);
		}

		// Token: 0x0600F765 RID: 63333 RVA: 0x0047EC10 File Offset: 0x0047CE10
		public static void LogFallColorForYear()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Fall color amounts for each latitude and each day of the year");
			stringBuilder.AppendLine("---------------------------------------");
			stringBuilder.Append("Lat".PadRight(6));
			for (int i = -90; i <= 90; i += 10)
			{
				stringBuilder.Append((i.ToString() + "d").PadRight(6));
			}
			stringBuilder.AppendLine();
			for (int j = 0; j < 60; j += 5)
			{
				stringBuilder.Append(j.ToString().PadRight(6));
				for (int k = -90; k <= 90; k += 10)
				{
					stringBuilder.Append(PlantFallColors.GetFallColorFactor((float)k, j).ToString("F3").PadRight(6));
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600F766 RID: 63334 RVA: 0x0047ECE6 File Offset: 0x0047CEE6
		public static float GrowthRateFactorFor_Fertility(ThingDef def, float fertilityAtCell)
		{
			if (def.plant.completelyIgnoreFertility)
			{
				return 1f;
			}
			return fertilityAtCell * def.plant.fertilitySensitivity + (1f - def.plant.fertilitySensitivity);
		}

		// Token: 0x0600F767 RID: 63335 RVA: 0x0047ED1C File Offset: 0x0047CF1C
		public static float GrowthRateFactorFor_Light(ThingDef def, float glow)
		{
			if (def.plant.growMinGlow == def.plant.growOptimalGlow && glow == def.plant.growOptimalGlow)
			{
				return 1f;
			}
			return GenMath.InverseLerp(def.plant.growMinGlow, def.plant.growOptimalGlow, glow);
		}

		// Token: 0x0600F768 RID: 63336 RVA: 0x0047ED74 File Offset: 0x0047CF74
		public static float GrowthRateFactorFor_Temperature(ThingDef plant, float cellTemp)
		{
			if (cellTemp < plant.plant.minOptimalGrowthTemperature)
			{
				return Mathf.InverseLerp(plant.plant.minGrowthTemperature, plant.plant.minOptimalGrowthTemperature, cellTemp);
			}
			if (cellTemp > plant.plant.maxOptimalGrowthTemperature)
			{
				return Mathf.InverseLerp(plant.plant.maxGrowthTemperature, plant.plant.maxOptimalGrowthTemperature, cellTemp);
			}
			return 1f;
		}

		// Token: 0x0600F769 RID: 63337 RVA: 0x0047EDDC File Offset: 0x0047CFDC
		public static float NutritionFactorFromGrowth(ThingDef def, float plantGrowth)
		{
			if (def.plant.Sowable)
			{
				return plantGrowth;
			}
			return Mathf.Lerp(0.5f, 1f, plantGrowth);
		}

		// Token: 0x0600F76A RID: 63338 RVA: 0x0047EE00 File Offset: 0x0047D000
		public static bool PawnWillingToCutPlant_Job(Thing plant, Pawn pawn)
		{
			return !plant.def.plant.IsTree || !plant.def.plant.treeLoversCareIfChopped || new HistoryEvent(HistoryEventDefOf.CutTree, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
		}

		// Token: 0x0600F76B RID: 63339 RVA: 0x0047EE4D File Offset: 0x0047D04D
		public static bool TreeMarkedForExtraction(Thing plant)
		{
			return plant.def.plant.IsTree && plant.MapHeld.designationManager.DesignationOn(plant, DesignationDefOf.ExtractTree) != null;
		}
	}
}
