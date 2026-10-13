using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003870 RID: 14448
	public static class FishingUtility
	{
		// Token: 0x06013F95 RID: 81813 RVA: 0x005ED5BC File Offset: 0x005EB7BC
		public static string FishPopulationLabel(float population)
		{
			if (population < 50f)
			{
				return "FishPopulation_VeryLow".Translate();
			}
			if (population < 200f)
			{
				return "FishPopulation_Low".Translate();
			}
			if (population < 300f)
			{
				return "FishPopulation_Moderate".Translate();
			}
			return "FishPopulation_Healthy".Translate();
		}

		// Token: 0x06013F96 RID: 81814 RVA: 0x005ED620 File Offset: 0x005EB820
		public static WaterBody GetWaterBody(this IntVec3 c, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			return map.waterBodyTracker.WaterBodyAt(c);
		}

		// Token: 0x06013F97 RID: 81815 RVA: 0x005ED638 File Offset: 0x005EB838
		public static List<NegativeFishingOutcomeDef> GetNegativeFishingOutcomes(Pawn pawn, IntVec3 cell)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			FishingUtility.tmpNegativeCatches.Clear();
			if (!DebugSettings.alwaysNegativeCatches)
			{
				if (!Rand.Chance(0.02f))
				{
					return FishingUtility.tmpNegativeCatches;
				}
				if (pawn.Map.waterBodyTracker.lastNegativeCatchTick != 0 && GenTicks.TicksGame - pawn.Map.waterBodyTracker.lastNegativeCatchTick <= 300000)
				{
					return FishingUtility.tmpNegativeCatches;
				}
			}
			WaterBody waterBody = cell.GetWaterBody(pawn.Map);
			foreach (NegativeFishingOutcomeDef negativeFishingOutcomeDef in DefDatabase<NegativeFishingOutcomeDef>.AllDefsListForReading)
			{
				if (waterBody.CommonFish.Contains(negativeFishingOutcomeDef.fishType) || waterBody.UncommonFish.Contains(negativeFishingOutcomeDef.fishType))
				{
					FishingUtility.tmpNegativeCatches.Add(negativeFishingOutcomeDef);
				}
			}
			return FishingUtility.tmpNegativeCatches;
		}

		// Token: 0x06013F98 RID: 81816 RVA: 0x005ED728 File Offset: 0x005EB928
		public static List<Thing> GetCatchesFor(Pawn pawn, IntVec3 cell, bool animalFishing, out bool rare)
		{
			FishingUtility.tmpCatches.Clear();
			rare = false;
			if (!ModsConfig.OdysseyActive)
			{
				return FishingUtility.tmpCatches;
			}
			WaterBody waterBody = cell.GetWaterBody(pawn.Map);
			if (waterBody == null || waterBody.waterBodyType == WaterBodyType.None)
			{
				return FishingUtility.tmpCatches;
			}
			ThingDef def;
			if (Rand.Chance(FishingUtility.PollutionToxfishChanceCurve.Evaluate(waterBody.PollutionPct)))
			{
				def = ThingDefOf.Fish_Toxfish;
				rare = false;
			}
			else
			{
				if (!animalFishing && pawn.Map.Biome.fishTypes.rareCatchesSetMaker != null && (DebugSettings.alwaysRareCatches || pawn.Map.waterBodyTracker.lastRareCatchTick == 0 || GenTicks.TicksGame - pawn.Map.waterBodyTracker.lastRareCatchTick > 300000) && Rand.Chance(0.01f))
				{
					FishingUtility.tmpCatches.AddRange(pawn.Map.Biome.fishTypes.rareCatchesSetMaker.root.Generate());
					if (FishingUtility.tmpCatches.Any<Thing>())
					{
						rare = true;
						return FishingUtility.tmpCatches;
					}
				}
				rare = false;
				ThingDef thingDef;
				if ((animalFishing || !Rand.Chance(0.05f) || !waterBody.UncommonFish.TryRandomElement(out thingDef)) && !waterBody.CommonFishIncludingExtras.TryRandomElement(out thingDef))
				{
					return FishingUtility.tmpCatches;
				}
				def = thingDef;
			}
			float x = pawn.Map.waterBodyTracker.FishPopulationAt(cell);
			float num;
			if (animalFishing)
			{
				float statValueAbstract = def.GetStatValueAbstract(StatDefOf.Nutrition, null);
				num = Mathf.Ceil((float)FoodUtility.WillIngestStackCountOf(pawn, def, statValueAbstract));
			}
			else
			{
				num = Mathf.Min(new float[]
				{
					FishingUtility.PopulationToFishYieldCurve.Evaluate(x) * pawn.GetStatValue(StatDefOf.FishingYield, true, -1)
				});
			}
			float num2 = pawn.Map.waterBodyTracker.FishPopulationAt(cell);
			if (num > num2)
			{
				num = num2;
			}
			int stackCount = Mathf.Max(1, Mathf.RoundToInt(num));
			Thing thing = ThingMaker.MakeThing(def, null);
			thing.stackCount = stackCount;
			FishingUtility.tmpCatches.Add(thing);
			return FishingUtility.tmpCatches;
		}

		// Token: 0x0400DC85 RID: 56453
		public const float FishPopulationRegenerationFactorPerDay = 0.025f;

		// Token: 0x0400DC86 RID: 56454
		public const int BaseFishingDurationTicks = 7500;

		// Token: 0x0400DC87 RID: 56455
		public const int AnimalFishingDurationTicks = 1500;

		// Token: 0x0400DC88 RID: 56456
		public const float ChanceToCatchUncommonFish = 0.05f;

		// Token: 0x0400DC89 RID: 56457
		public const float ChanceForRareCatch = 0.01f;

		// Token: 0x0400DC8A RID: 56458
		private const int RareCatchCooldownTicks = 300000;

		// Token: 0x0400DC8B RID: 56459
		public const float ChanceForNegativeCatch = 0.02f;

		// Token: 0x0400DC8C RID: 56460
		private const int NegativeCatchCooldownTicks = 120000;

		// Token: 0x0400DC8D RID: 56461
		public const int MinFishPopulationForFishing = 10;

		// Token: 0x0400DC8E RID: 56462
		public const int MinFishPopulationPerWaterBody = 20;

		// Token: 0x0400DC8F RID: 56463
		public static readonly SimpleCurve FishPopulationFactorPerBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(74.9f, 0f),
				true
			},
			{
				new CurvePoint(75f, 0.03f),
				true
			},
			{
				new CurvePoint(400f, 0.04f),
				true
			},
			{
				new CurvePoint(1600f, 0.1f),
				true
			},
			{
				new CurvePoint(3200f, 0.24f),
				true
			},
			{
				new CurvePoint(6000f, 0.8f),
				true
			},
			{
				new CurvePoint(20000f, 1f),
				true
			}
		};

		// Token: 0x0400DC90 RID: 56464
		public static readonly SimpleCurve ChanceForCommonFishFromWaterBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(74.9f, 0f),
				true
			},
			{
				new CurvePoint(75f, 0.05f),
				true
			},
			{
				new CurvePoint(100f, 0.5f),
				true
			},
			{
				new CurvePoint(200f, 1f),
				true
			}
		};

		// Token: 0x0400DC91 RID: 56465
		public static readonly SimpleCurve ChanceForUncommonFishFromWaterBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(399.9f, 0f),
				true
			},
			{
				new CurvePoint(400f, 0.1f),
				true
			},
			{
				new CurvePoint(1600f, 0.25f),
				true
			},
			{
				new CurvePoint(6000f, 0.7f),
				true
			}
		};

		// Token: 0x0400DC92 RID: 56466
		public static readonly SimpleCurve PopulationToFishYieldCurve = new SimpleCurve
		{
			{
				new CurvePoint(25f, 1f),
				true
			},
			{
				new CurvePoint(100f, 3f),
				true
			},
			{
				new CurvePoint(300f, 6f),
				true
			}
		};

		// Token: 0x0400DC93 RID: 56467
		public static readonly SimpleCurve PollutionToxfishChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.05f, 0f),
				true
			},
			{
				new CurvePoint(0.8f, 0.6f),
				true
			}
		};

		// Token: 0x0400DC94 RID: 56468
		private static List<NegativeFishingOutcomeDef> tmpNegativeCatches = new List<NegativeFishingOutcomeDef>();

		// Token: 0x0400DC95 RID: 56469
		private static List<Thing> tmpCatches = new List<Thing>();
	}
}
