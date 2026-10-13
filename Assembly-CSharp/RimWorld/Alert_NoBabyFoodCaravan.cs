using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FF2 RID: 12274
	public class Alert_NoBabyFoodCaravan : Alert
	{
		// Token: 0x0601148E RID: 70798 RVA: 0x004F3EEA File Offset: 0x004F20EA
		public Alert_NoBabyFoodCaravan()
		{
			this.defaultLabel = "AlertNoBabyFoodCaravan".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0601148F RID: 70799 RVA: 0x004F3F21 File Offset: 0x004F2121
		public override TaggedString GetExplanation()
		{
			return "AlertNoBabyFoodCaravanDesc".Translate(Faction.OfPlayer.Named("FACTION"));
		}

		// Token: 0x06011490 RID: 70800 RVA: 0x004F3F3C File Offset: 0x004F213C
		public override AlertReport GetReport()
		{
			return new AlertReport
			{
				active = this.LowBabyFoodNutrition(this.caravansReportOut),
				culpritsCaravans = this.caravansReportOut
			};
		}

		// Token: 0x06011491 RID: 70801 RVA: 0x004F3F74 File Offset: 0x004F2174
		private bool LowBabyFoodNutrition(List<Caravan> foodlessCaravansOut)
		{
			foodlessCaravansOut.Clear();
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				bool flag = false;
				for (int i = 0; i < caravan.PawnsListForReading.Count; i++)
				{
					Pawn pawn = caravan.PawnsListForReading[i];
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
					{
						Predicate<Pawn, Pawn> feederPredicate = delegate(Pawn _baby, Pawn _mom)
						{
							ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
							return ChildcareUtility.CanMomBreastfeedBaby(_mom, _baby, out breastfeedFailReason2);
						};
						if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Urgent, feederPredicate, caravan.PawnsListForReading) && !pawn.mindState.AnyAutofeeder(AutofeedMode.Childcare, feederPredicate, caravan.PawnsListForReading))
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					if (!CaravanInventoryUtility.AllInventoryItems(caravan).Any((Thing thing) => thing.def.IsNutritionGivingIngestibleForHumanlikeBabies))
					{
						foodlessCaravansOut.Add(caravan);
					}
				}
			}
			return foodlessCaravansOut.Count > 0;
		}

		// Token: 0x0400B334 RID: 45876
		private List<Caravan> caravansReportOut = new List<Caravan>(8);
	}
}
