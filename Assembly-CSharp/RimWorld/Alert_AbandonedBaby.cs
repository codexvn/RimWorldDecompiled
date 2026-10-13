using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FF1 RID: 12273
	public class Alert_AbandonedBaby : Alert
	{
		// Token: 0x0601148A RID: 70794 RVA: 0x004F3BE8 File Offset: 0x004F1DE8
		public Alert_AbandonedBaby()
		{
			this.defaultLabel = "AlertAbandonedBaby".Translate();
			this.defaultExplanation = "AlertAbandonedBabyDesc".Translate("DesignatorAdopt".Translate());
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0601148B RID: 70795 RVA: 0x004F3C42 File Offset: 0x004F1E42
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(Alert_AbandonedBaby.AbandonedBabies());
		}

		// Token: 0x0601148C RID: 70796 RVA: 0x004F3C50 File Offset: 0x004F1E50
		private static List<Pawn> AbandonedBabies()
		{
			Alert_AbandonedBaby.tmpAllBabiesList.Clear();
			Alert_AbandonedBaby.tmpAbandonedBabiesList.Clear();
			using (new ProfilerBlock("AllBabies"))
			{
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.AllPawns)
					{
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
						{
							Alert_AbandonedBaby.tmpAllBabiesList.Add(pawn);
						}
					}
				}
			}
			using (new ProfilerBlock("AbandonedBabiesFilter"))
			{
				foreach (Pawn pawn2 in Alert_AbandonedBaby.tmpAllBabiesList)
				{
					if (pawn2.Faction != Faction.OfPlayer)
					{
						if (!pawn2.Spawned)
						{
							IThingHolder parentHolder = pawn2.ParentHolder;
							if (parentHolder != null && !(parentHolder is Pawn_CarryTracker))
							{
								continue;
							}
						}
						Map mapHeld = pawn2.MapHeld;
						bool flag = true;
						foreach (Pawn pawn3 in pawn2.mindState.Autofeeders())
						{
							ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
							if (pawn3.MapHeld == mapHeld && ChildcareUtility.CanFeedBaby(pawn3, pawn2, out breastfeedFailReason))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							using (new ProfilerBlock("AdoptableBy"))
							{
								if (!pawn2.AdoptableBy(Faction.OfPlayer, null))
								{
									continue;
								}
							}
							Alert_AbandonedBaby.tmpAbandonedBabiesList.Add(pawn2);
						}
					}
				}
			}
			return Alert_AbandonedBaby.tmpAbandonedBabiesList;
		}

		// Token: 0x0400B332 RID: 45874
		private static List<Pawn> tmpAllBabiesList = new List<Pawn>(32);

		// Token: 0x0400B333 RID: 45875
		private static List<Pawn> tmpAbandonedBabiesList = new List<Pawn>(32);
	}
}
