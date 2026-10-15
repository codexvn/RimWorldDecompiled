using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class AnimalProductionUtility
{
	[CompilerGenerated]
	private sealed class _003CAnimalProductionStats_003Ed__0 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef d;

		public ThingDef _003C_003E3__d;

		private CompProperties_Milkable _003Cmilkable_003E5__2;

		private CompProperties_Shearable _003Cshearable_003E5__3;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CAnimalProductionStats_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cmilkable_003E5__2 = null;
			_003Cshearable_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			CompProperties_EggLayer compProperties;
			IntRange intRange;
			float num2;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				float num = GestationDaysLitter(d);
				if (num > 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GestationTime".Translate(), "PeriodDays".Translate(num.ToString("#.##")), "Stat_Animal_GestationTimeDesc".Translate(), 10000);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00f5;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00f5;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0162;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MeatPerDayDuringGrowth".Translate(), MeatPerDayDuringGrowth(d).ToString("#.##"), "Stat_Animal_MeatPerDayDuringGrowthDesc".Translate(), 9960);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0235;
			case 5:
				_003C_003E1__state = -1;
				if (Herbivore(d))
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GrassToMaintain".Translate(), GrassToMaintain(d).ToString("#.##"), "Stat_Animal_GrassToMaintainDesc".Translate(), 9950);
					_003C_003E1__state = 6;
					return true;
				}
				goto IL_031c;
			case 6:
				_003C_003E1__state = -1;
				goto IL_031c;
			case 7:
				_003C_003E1__state = -1;
				goto IL_03a7;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggNutrition".Translate(), EggNutrition(d).ToStringByStyle(ToStringStyle.FloatMaxTwo), "Stat_Animal_EggNutritionDesc".Translate(), 9920);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggNutritionYearly".Translate(), EggNutritionPerYear(d).ToStringByStyle(ToStringStyle.FloatMaxTwo), "Stat_Animal_EggNutritionYearlyDesc".Translate(), 9910);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggMarketValue".Translate(), EggMarketValue(d).ToStringMoney(), "Stat_Animal_EggMarketValueDesc".Translate(), 9900);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggMarketValueYearly".Translate(), EggMarketValuePerYear(d).ToStringMoney(), "Stat_Animal_EggMarketValueYearlyDesc".Translate(), 9890);
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				goto IL_0566;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkAmount".Translate(), _003Cmilkable_003E5__2.milkAmount.ToString(), "Stat_Animal_MilkAmountDesc".Translate(), 9870);
				_003C_003E1__state = 14;
				return true;
			case 14:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkGrowthTime".Translate(), "PeriodDays".Translate(_003Cmilkable_003E5__2.milkIntervalDays), "Stat_Animal_MilkGrowthTimeDesc".Translate(), 9860);
				_003C_003E1__state = 15;
				return true;
			case 15:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkPerYear".Translate(), MilkPerYear(d).ToString("F0"), "Stat_Animal_MilkPerYearDesc".Translate(), 9850);
				_003C_003E1__state = 16;
				return true;
			case 16:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkValue".Translate(), MilkMarketValue(d).ToStringMoney(), "Stat_Animal_MilkValueDesc".Translate(), 9840);
				_003C_003E1__state = 17;
				return true;
			case 17:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkValuePerYear".Translate(), MilkMarketValuePerYear(d).ToStringMoney(), "Stat_Animal_MilkValuePerYearDesc".Translate(), 9830);
				_003C_003E1__state = 18;
				return true;
			case 18:
				_003C_003E1__state = -1;
				goto IL_07c6;
			case 19:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolAmount".Translate(), _003Cshearable_003E5__3.woolAmount.ToString(), "Stat_Animal_WoolAmountDesc".Translate(), 9810);
				_003C_003E1__state = 20;
				return true;
			case 20:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolGrowthTime".Translate(), "PeriodDays".Translate(_003Cshearable_003E5__3.shearIntervalDays), "Stat_Animal_WoolGrowthTimeDesc".Translate(), 9800);
				_003C_003E1__state = 21;
				return true;
			case 21:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolPerYear".Translate(), WoolPerYear(d).ToString("F0"), "Stat_Animal_WoolPerYearDesc".Translate(), 9790);
				_003C_003E1__state = 22;
				return true;
			case 22:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolValue".Translate(), WoolMarketValue(d).ToStringMoney(), "Stat_Animal_WoolValueDesc".Translate(), 9780);
				_003C_003E1__state = 23;
				return true;
			case 23:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolValuePerYear".Translate(), WoolMarketValuePerYear(d).ToStringMoney(), "Stat_Animal_WoolValuePerYearDesc".Translate(), 9770);
				_003C_003E1__state = 24;
				return true;
			case 24:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_031c:
				compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				if (compProperties != null)
				{
					ThingDef thingDef = compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef;
					if (thingDef != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggType".Translate(), thingDef.LabelCap, "Stat_Animal_EggTypeDesc".Translate(), 9940, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(thingDef)));
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_03a7;
				}
				goto IL_0566;
				IL_00f5:
				intRange = OffspringRange(d);
				if (intRange != IntRange.One)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_LitterSize".Translate(), intRange.ToString(), "Stat_Animal_LitterSizeDesc".Translate(), 9990);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0162;
				IL_03a7:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggsPerYear".Translate(), EggsPerYear(d).ToStringByStyle(ToStringStyle.FloatMaxTwo), "Stat_Animal_EggsPerYearDesc".Translate(), 9930);
				_003C_003E1__state = 8;
				return true;
				IL_0566:
				_003Cmilkable_003E5__2 = d.GetCompProperties<CompProperties_Milkable>();
				if (_003Cmilkable_003E5__2 != null)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkType".Translate(), _003Cmilkable_003E5__2.milkDef.LabelCap, "Stat_Animal_MilkTypeDesc".Translate(), 9880, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(_003Cmilkable_003E5__2.milkDef)));
					_003C_003E1__state = 13;
					return true;
				}
				goto IL_07c6;
				IL_0235:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_AdultMeatAmount".Translate(), AdultMeatAmount(d).ToString("F0"), "Stat_Animal_AdultMeatAmountDesc".Translate(), 9970, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(d.race.meatDef)));
				_003C_003E1__state = 5;
				return true;
				IL_07c6:
				_003Cshearable_003E5__3 = d.GetCompProperties<CompProperties_Shearable>();
				if (_003Cshearable_003E5__3 != null)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolType".Translate(), _003Cshearable_003E5__3.woolDef.LabelCap, "Stat_Animal_WoolTypeDesc".Translate(), 9820, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(_003Cshearable_003E5__3.woolDef)));
					_003C_003E1__state = 19;
					return true;
				}
				break;
				IL_0162:
				num2 = DaysToAdulthood(d);
				if (num2 > 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GrowthTime".Translate(), "PeriodDays".Translate(num2), "Stat_Animal_GrowthTimeDesc".Translate(), 9980);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0235;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CAnimalProductionStats_003Ed__0 _003CAnimalProductionStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAnimalProductionStats_003Ed__ = this;
			}
			else
			{
				_003CAnimalProductionStats_003Ed__ = new _003CAnimalProductionStats_003Ed__0(0);
			}
			_003CAnimalProductionStats_003Ed__.d = _003C_003E3__d;
			return _003CAnimalProductionStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CAnimalProductionStats_003Ed__0))]
	public static IEnumerable<StatDrawEntry> AnimalProductionStats(ThingDef d)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAnimalProductionStats_003Ed__0(-2)
		{
			_003C_003E3__d = d
		};
	}

	public static float AdultMeatAmount(ThingDef d)
	{
		return d.GetStatValueAbstract(StatDefOf.MeatAmount);
	}

	public static float AdultLeatherAmount(ThingDef d)
	{
		return d.GetStatValueAbstract(StatDefOf.LeatherAmount);
	}

	public static bool Herbivore(ThingDef d)
	{
		return (d.race.foodType & FoodTypeFlags.Plant) != 0;
	}

	public static float NutritionToGestate(ThingDef d)
	{
		LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
		return 0f + GestationDaysEach(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
	}

	public static float NutritionToAdulthood(ThingDef d)
	{
		float num = 0f;
		num += NutritionToGestate(d);
		for (int i = 1; i < d.race.lifeStageAges.Count; i++)
		{
			LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
			float num2 = (lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge) * 60f;
			num += num2 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
		}
		return num;
	}

	public static float GestationDaysEach(ThingDef d)
	{
		if (d.HasComp(typeof(CompEggLayer)))
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			return compProperties.eggLayIntervalDays / compProperties.eggCountRange.Average;
		}
		return d.race.gestationPeriodDays / ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f);
	}

	public static float GestationDaysLitter(ThingDef d)
	{
		if (d.HasComp(typeof(CompEggLayer)))
		{
			return d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays;
		}
		return d.race.gestationPeriodDays;
	}

	public static IntRange OffspringRange(ThingDef d)
	{
		CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
		if (compProperties != null)
		{
			return compProperties.eggCountRange;
		}
		if (d.race.litterSizeCurve != null)
		{
			int min = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.First().x), 1);
			int max = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.Last().x), 1);
			return new IntRange(min, max);
		}
		return new IntRange(1, 1);
	}

	public static float GrassNutritionPerDay()
	{
		ThingDef plant_Grass = ThingDefOf.Plant_Grass;
		return plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition) / (plant_Grass.plant.growDays / 0.5f);
	}

	public static float GrassToMaintain(ThingDef d)
	{
		if (!Herbivore(d))
		{
			return -1f;
		}
		return 2.6666667E-05f * d.race.baseHungerRate * 60000f / GrassNutritionPerDay();
	}

	public static float EggsPerYear(ThingDef d)
	{
		CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
		if (compProperties == null)
		{
			return 0f;
		}
		return 60f / compProperties.eggLayIntervalDays * compProperties.eggCountRange.Average;
	}

	public static float EggNutritionPerYear(ThingDef d)
	{
		return EggsPerYear(d) * EggNutrition(d);
	}

	public static float EggNutrition(ThingDef d)
	{
		CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
		if (compProperties == null)
		{
			return -1f;
		}
		return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).GetStatValueAbstract(StatDefOf.Nutrition);
	}

	public static float EggMarketValue(ThingDef d)
	{
		CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
		if (compProperties == null)
		{
			return -1f;
		}
		return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).BaseMarketValue;
	}

	public static float EggMarketValuePerYear(ThingDef d)
	{
		return EggsPerYear(d) * EggMarketValue(d);
	}

	public static float DaysToAdulthood(ThingDef d)
	{
		return d.race.lifeStageAges.Last().minAge * 60f;
	}

	public static float MeatPerDayDuringGrowth(ThingDef d)
	{
		float num = AdultMeatAmount(d);
		float num2 = DaysToAdulthood(d);
		return num / num2;
	}

	public static float MilkPerYear(ThingDef d)
	{
		CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
		if (compProperties == null)
		{
			return 0f;
		}
		return 60f / (float)compProperties.milkIntervalDays * (float)compProperties.milkAmount;
	}

	public static float MilkMarketValue(ThingDef d)
	{
		return d.GetCompProperties<CompProperties_Milkable>()?.milkDef.BaseMarketValue ?? (-1f);
	}

	public static float MilkMarketValuePerYear(ThingDef d)
	{
		return MilkPerYear(d) * MilkMarketValue(d);
	}

	public static float MilkNutrition(ThingDef d)
	{
		CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
		if (compProperties == null)
		{
			return 0f;
		}
		if (!compProperties.milkDef.IsIngestible)
		{
			return 0f;
		}
		return compProperties.milkDef.ingestible.CachedNutrition;
	}

	public static float MilkNutritionPerYear(ThingDef d)
	{
		return MilkPerYear(d) * MilkNutrition(d);
	}

	public static float WoolPerYear(ThingDef d)
	{
		CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
		if (compProperties == null)
		{
			return 0f;
		}
		return 60f / (float)compProperties.shearIntervalDays * (float)compProperties.woolAmount;
	}

	public static float WoolMarketValue(ThingDef d)
	{
		return d.GetCompProperties<CompProperties_Shearable>()?.woolDef.BaseMarketValue ?? 0f;
	}

	public static float WoolMarketValuePerYear(ThingDef d)
	{
		return WoolPerYear(d) * WoolMarketValue(d);
	}
}
