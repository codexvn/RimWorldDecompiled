using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003741 RID: 14145
	public class StatWorker_MarketValue : StatWorker
	{
		// Token: 0x060139CD RID: 80333 RVA: 0x005D33B4 File Offset: 0x005D15B4
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.HasThing && req.Thing is Pawn)
			{
				return base.GetValueUnfinalized(StatRequest.For(req.BuildableDef, req.StuffDef, QualityCategory.Normal), applyPostProcess) * PriceUtility.PawnQualityPriceFactor((Pawn)req.Thing, null) + PriceUtility.PawnQualityPriceOffset((Pawn)req.Thing, null);
			}
			float num;
			if (req.StatBases.StatListContains(StatDefOf.MarketValue))
			{
				if (this.stat != StatDefOf.MarketValue)
				{
					return StatDefOf.MarketValue.Worker.GetValueUnfinalized(req, true);
				}
				num = base.GetValueUnfinalized(req, true);
			}
			else
			{
				if (req.HasThing)
				{
					IFixedBaseMarketValue fixedBaseMarketValue = req.Thing as IFixedBaseMarketValue;
					if (fixedBaseMarketValue != null)
					{
						num = fixedBaseMarketValue.BaseMarketValue;
						goto IL_FC;
					}
				}
				if (req.HasThing && req.Thing.StyleSourcePrecept is Precept_Relic)
				{
					num = StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, ThingDefOf.Steel);
				}
				else
				{
					num = StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, req.StuffDef);
				}
			}
			IL_FC:
			ThingWithComps thingWithComps = req.Thing as ThingWithComps;
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				foreach (ThingComp thingComp in allComps)
				{
					num += thingComp.GetStatOffset(this.stat);
				}
				foreach (ThingComp thingComp2 in allComps)
				{
					num *= thingComp2.GetStatFactor(this.stat);
				}
			}
			return num;
		}

		// Token: 0x060139CE RID: 80334 RVA: 0x005D356C File Offset: 0x005D176C
		public static float CalculatedBaseMarketValue(BuildableDef def, ThingDef stuffDef)
		{
			float num = 0f;
			RecipeDef recipeDef = StatWorker_MarketValue.CalculableRecipe(def);
			float num2;
			int num3;
			if (recipeDef != null)
			{
				num2 = recipeDef.workAmount;
				num3 = recipeDef.products[0].count;
				if (recipeDef.ingredients != null)
				{
					for (int i = 0; i < recipeDef.ingredients.Count; i++)
					{
						IngredientCount ingredientCount = recipeDef.ingredients[i];
						int num4 = ingredientCount.CountRequiredOfFor(ingredientCount.FixedIngredient, recipeDef, null);
						num += (float)num4 * ingredientCount.FixedIngredient.BaseMarketValue;
					}
				}
			}
			else
			{
				num2 = Mathf.Max(def.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef), def.GetStatValueAbstract(StatDefOf.WorkToBuild, stuffDef));
				num3 = 1;
				if (def.CostList != null)
				{
					for (int j = 0; j < def.CostList.Count; j++)
					{
						ThingDefCountClass thingDefCountClass = def.CostList[j];
						num += (float)thingDefCountClass.count * thingDefCountClass.thingDef.BaseMarketValue;
					}
				}
				if (def.CostStuffCount > 0)
				{
					if (stuffDef != null)
					{
						num += (float)def.CostStuffCount / stuffDef.VolumePerUnit * stuffDef.GetStatValueAbstract(StatDefOf.MarketValue, null);
					}
					else
					{
						num += (float)def.CostStuffCount * 2f;
					}
				}
			}
			if (num2 > 2f)
			{
				num += num2 * 0.0036f;
			}
			return num / (float)num3;
		}

		// Token: 0x060139CF RID: 80335 RVA: 0x005D36C0 File Offset: 0x005D18C0
		public static RecipeDef CalculableRecipe(BuildableDef def)
		{
			if (def.CostList.NullOrEmpty<ThingDefCountClass>() && def.CostStuffCount <= 0)
			{
				List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					RecipeDef recipeDef = allDefsListForReading[i];
					if (recipeDef.products != null && recipeDef.products.Count == 1 && recipeDef.products[0].thingDef == def)
					{
						for (int j = 0; j < recipeDef.ingredients.Count; j++)
						{
							if (!recipeDef.ingredients[j].IsFixedIngredient)
							{
								return null;
							}
						}
						return recipeDef;
					}
				}
			}
			return null;
		}

		// Token: 0x060139D0 RID: 80336 RVA: 0x005D375C File Offset: 0x005D195C
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.HasThing && req.Thing is Pawn)
			{
				Pawn pawn = (Pawn)req.Thing;
				stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
				PriceUtility.PawnQualityPriceFactor(pawn, stringBuilder);
				PriceUtility.PawnQualityPriceOffset(pawn, stringBuilder);
				return stringBuilder.ToString();
			}
			if (req.StatBases.StatListContains(StatDefOf.MarketValue))
			{
				return base.GetExplanationUnfinalized(req, numberSense);
			}
			stringBuilder.Append("StatsReport_MarketValueFromStuffsAndWork".TranslateSimple().TrimEnd('.') + ": " + StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, (!req.HasThing || !(req.Thing.StyleSourcePrecept is Precept_Relic)) ? req.StuffDef : ThingDefOf.Steel).ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense));
			stringBuilder.AppendLine();
			ThingWithComps thingWithComps = req.Thing as ThingWithComps;
			if (thingWithComps != null)
			{
				foreach (ThingComp thingComp in thingWithComps.AllComps)
				{
					thingComp.GetStatsExplanation(this.stat, stringBuilder, "");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400D952 RID: 55634
		public const float ValuePerWork = 0.0036f;

		// Token: 0x0400D953 RID: 55635
		private const float DefaultGuessStuffCost = 2f;
	}
}
