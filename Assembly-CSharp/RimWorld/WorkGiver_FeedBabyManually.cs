using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015C2 RID: 5570
	public abstract class WorkGiver_FeedBabyManually : WorkGiver_Scanner
	{
		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x060083A7 RID: 33703 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x060083A8 RID: 33704 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060083A9 RID: 33705 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060083AA RID: 33706 RVA: 0x00274EA6 File Offset: 0x002730A6
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x060083AB RID: 33707 RVA: 0x00277F34 File Offset: 0x00276134
		protected bool CanCreateManualFeedingJob(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(pawn2, out breastfeedFailReason))
			{
				return false;
			}
			if (ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn, pawn2, false, out breastfeedFailReason))
			{
				return false;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2 = null;
			if (!ChildcareUtility.CanSuckleNow(pawn2, out breastfeedFailReason2))
			{
				if (forced)
				{
					JobFailReason.Is(breastfeedFailReason2.Value.Translate(null, null, pawn2), null);
				}
				return false;
			}
			if (!ChildcareUtility.CanHaulBaby(pawn, pawn2, out breastfeedFailReason))
			{
				return false;
			}
			if (!ChildcareUtility.CanHaulBabyNow(pawn, pawn2, forced, out breastfeedFailReason2))
			{
				if (forced)
				{
					JobFailReason.Is(breastfeedFailReason2.Value.Translate(pawn, null, pawn2), null);
				}
				return false;
			}
			if (!forced && !ChildcareUtility.WantsSuckle(pawn2, out breastfeedFailReason))
			{
				return false;
			}
			Pawn pawn3;
			if (!forced && this.AutoBreastfeederAvailable(pawn2, forced, out pawn3))
			{
				return false;
			}
			bool result;
			if (pawn2 == null)
			{
				result = (null != null);
			}
			else
			{
				Pawn_NeedsTracker needs = pawn2.needs;
				result = (((needs != null) ? needs.food : null) != null);
			}
			return result;
		}

		// Token: 0x060083AC RID: 33708 RVA: 0x00278008 File Offset: 0x00276208
		private bool AutoBreastfeederAvailable(Pawn baby, bool forced, out Pawn feeder)
		{
			Pawn pawn = WorkGiver_FeedBabyManually.cachedBabyAutoBreastfeeder.TryGetValue(baby, null);
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn, baby, forced, out breastfeedFailReason))
			{
				feeder = pawn;
				return true;
			}
			WorkGiver_FeedBabyManually.cachedBabyAutoBreastfeeder[baby] = null;
			foreach (Pawn pawn2 in ChildcareUtility.CanBreastfeedMothers(baby, false))
			{
				if (ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn2, baby, forced, out breastfeedFailReason))
				{
					WorkGiver_FeedBabyManually.cachedBabyAutoBreastfeeder[baby] = pawn2;
					feeder = pawn2;
					return true;
				}
			}
			feeder = null;
			return false;
		}

		// Token: 0x040056EF RID: 22255
		private static Dictionary<Pawn, Pawn> cachedBabyAutoBreastfeeder = new Dictionary<Pawn, Pawn>();
	}
}
