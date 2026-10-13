using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FEF RID: 12271
	public class Alert_NoBabyFeeders : Alert
	{
		// Token: 0x06011481 RID: 70785 RVA: 0x004F39FD File Offset: 0x004F1BFD
		public Alert_NoBabyFeeders()
		{
			this.defaultLabel = "AlertNoBabyFeeder".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x06011482 RID: 70786 RVA: 0x004F3A28 File Offset: 0x004F1C28
		public override TaggedString GetExplanation()
		{
			return "AlertNoBabyFeederDesc".Translate(Faction.OfPlayer.Named("FACTION"));
		}

		// Token: 0x06011483 RID: 70787 RVA: 0x004F3A43 File Offset: 0x004F1C43
		public override AlertReport GetReport()
		{
			if (Alert_NoBabyFeeders.MapWithNoBabyFeeder(Alert_NoBabyFeeders.tmpPawnList) != null)
			{
				return AlertReport.CulpritsAre(Alert_NoBabyFeeders.tmpPawnList);
			}
			return false;
		}

		// Token: 0x06011484 RID: 70788 RVA: 0x004F3A64 File Offset: 0x004F1C64
		private static Map MapWithNoBabyFeeder(List<Pawn> babiesOut)
		{
			if (babiesOut != null)
			{
				babiesOut.Clear();
			}
			foreach (Map map in Find.Maps)
			{
				List<Pawn> freeColonistsAndPrisonersSpawned = map.mapPawns.FreeColonistsAndPrisonersSpawned;
				for (int i = 0; i < freeColonistsAndPrisonersSpawned.Count; i++)
				{
					Pawn pawn = freeColonistsAndPrisonersSpawned[i];
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
					{
						if (!pawn.Spawned)
						{
							IThingHolder parentHolder = pawn.ParentHolder;
							if (parentHolder != null && !(parentHolder is Pawn_CarryTracker))
							{
								goto IL_DA;
							}
						}
						if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Urgent, delegate(Pawn _baby, Pawn _mom)
						{
							ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
							return ChildcareUtility.CanHaulBabyToMomNow(_mom, _mom, _baby, true, out breastfeedFailReason2);
						}, null))
						{
							if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Childcare, delegate(Pawn _baby, Pawn _mom)
							{
								ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
								return ChildcareUtility.CanHaulBabyToMomNow(_mom, _mom, _baby, true, out breastfeedFailReason2);
							}, null))
							{
								if (babiesOut == null)
								{
									return map;
								}
								babiesOut.Add(pawn);
							}
						}
					}
					IL_DA:;
				}
				if (babiesOut != null && babiesOut.Count > 0)
				{
					return map;
				}
			}
			return null;
		}

		// Token: 0x0400B32E RID: 45870
		private static List<Pawn> tmpPawnList = new List<Pawn>(32);
	}
}
