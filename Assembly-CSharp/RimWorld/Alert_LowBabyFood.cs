using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FED RID: 12269
	public class Alert_LowBabyFood : Alert
	{
		// Token: 0x06011478 RID: 70776 RVA: 0x004F3690 File Offset: 0x004F1890
		public Alert_LowBabyFood()
		{
			this.defaultLabel = "AlertLowBabyFood".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
			this.babyFoodList = (from def in DefDatabase<ThingDef>.AllDefs
			where def.IsNutritionGivingIngestible && def.ingestible.HumanEdible && def.ingestible.babiesCanIngest
			select def.label).ToCommaListOr(false);
		}

		// Token: 0x06011479 RID: 70777 RVA: 0x004F3738 File Offset: 0x004F1938
		public override TaggedString GetExplanation()
		{
			float? num;
			if (!this.LowBabyFoodNutrition(this.babiesOutExplanation, out num))
			{
				return string.Empty;
			}
			return "AlertLowBabyFoodDesc".Translate(this.babiesOutExplanation.Count.ToStringCached(), Mathf.FloorToInt(num.Value / (0.25f * (float)this.babiesOutExplanation.Count)).ToStringCached(), this.babyFoodList);
		}

		// Token: 0x0601147A RID: 70778 RVA: 0x004F37B4 File Offset: 0x004F19B4
		public override AlertReport GetReport()
		{
			if ((float)Find.TickManager.TicksGame < 150000f)
			{
				return false;
			}
			float? num;
			return new AlertReport
			{
				active = this.LowBabyFoodNutrition(this.babiesOutReport, out num),
				culpritsPawns = this.babiesOutReport
			};
		}

		// Token: 0x0601147B RID: 70779 RVA: 0x004F3808 File Offset: 0x004F1A08
		private bool LowBabyFoodNutrition(List<Pawn> lowFoodBabiesOut, out float? babyFoodNutrition)
		{
			foreach (Map map in Find.Maps)
			{
				using (new ProfilerBlock("Populate lowFoodBabiesOut"))
				{
					lowFoodBabiesOut.Clear();
					List<Pawn> freeColonistsAndPrisoners;
					using (new ProfilerBlock("FreeColonistsAndPrisoners"))
					{
						freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
					}
					for (int i = 0; i < freeColonistsAndPrisoners.Count; i++)
					{
						Pawn pawn = freeColonistsAndPrisoners[i];
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason) && pawn.SpawnedParentOrMe is Pawn)
						{
							Predicate<Pawn, Pawn> feederPredicate = delegate(Pawn _baby, Pawn _mom)
							{
								ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
								return ChildcareUtility.CanMomBreastfeedBaby(_mom, _baby, out breastfeedFailReason2) && ChildcareUtility.CanHaulBabyToMomNow(_mom, _mom, _baby, true, out breastfeedFailReason2);
							};
							if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Urgent, feederPredicate, null) && !pawn.mindState.AnyAutofeeder(AutofeedMode.Childcare, feederPredicate, null))
							{
								lowFoodBabiesOut.Add(pawn);
							}
						}
					}
				}
				if (lowFoodBabiesOut.Count > 0)
				{
					float totalHumanBabyEdibleNutrition = map.resourceCounter.TotalHumanBabyEdibleNutrition;
					if (totalHumanBabyEdibleNutrition < 1f * (float)lowFoodBabiesOut.Count)
					{
						babyFoodNutrition = new float?(totalHumanBabyEdibleNutrition);
						return true;
					}
				}
			}
			babyFoodNutrition = null;
			return false;
		}

		// Token: 0x0400B325 RID: 45861
		private const float NutritionPerDayPerBaby = 0.25f;

		// Token: 0x0400B326 RID: 45862
		private const float NutritionThresholdPerBaby = 1f;

		// Token: 0x0400B327 RID: 45863
		private List<Pawn> babiesOutReport = new List<Pawn>(8);

		// Token: 0x0400B328 RID: 45864
		private List<Pawn> babiesOutExplanation = new List<Pawn>(8);

		// Token: 0x0400B329 RID: 45865
		private string babyFoodList;
	}
}
