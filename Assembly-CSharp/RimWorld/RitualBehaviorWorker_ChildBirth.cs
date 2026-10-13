using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026D8 RID: 9944
	public class RitualBehaviorWorker_ChildBirth : RitualBehaviorWorker
	{
		// Token: 0x0600D9F0 RID: 55792 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_ChildBirth()
		{
		}

		// Token: 0x0600D9F1 RID: 55793 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_ChildBirth(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x1700215B RID: 8539
		// (get) Token: 0x0600D9F2 RID: 55794 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ChecksReservations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700215C RID: 8540
		// (get) Token: 0x0600D9F3 RID: 55795 RVA: 0x00407B1F File Offset: 0x00405D1F
		public override string descriptionOverride
		{
			get
			{
				if (Find.Storyteller.difficulty.babiesAreHealthy)
				{
					return "ChildBirthAlwaysHealthy_Desc".Translate();
				}
				return null;
			}
		}

		// Token: 0x0600D9F4 RID: 55796 RVA: 0x00407B44 File Offset: 0x00405D44
		public override void Tick(LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.assignments.FirstAssignedPawn("mother");
			Hediff_Labor hediff_Labor;
			if (pawn == null)
			{
				hediff_Labor = null;
			}
			else
			{
				Pawn_HealthTracker health = pawn.health;
				if (health == null)
				{
					hediff_Labor = null;
				}
				else
				{
					HediffSet hediffSet = health.hediffSet;
					hediff_Labor = ((hediffSet != null) ? hediffSet.GetFirstHediff<Hediff_Labor>() : null);
				}
			}
			Hediff_Labor hediff_Labor2 = hediff_Labor;
			float value;
			if (hediff_Labor2 != null)
			{
				value = hediff_Labor2.Progress;
			}
			else
			{
				Hediff_LaborPushing hediff_LaborPushing;
				if (pawn == null)
				{
					hediff_LaborPushing = null;
				}
				else
				{
					Pawn_HealthTracker health2 = pawn.health;
					if (health2 == null)
					{
						hediff_LaborPushing = null;
					}
					else
					{
						HediffSet hediffSet2 = health2.hediffSet;
						hediff_LaborPushing = ((hediffSet2 != null) ? hediffSet2.GetFirstHediff<Hediff_LaborPushing>() : null);
					}
				}
				Hediff_LaborPushing hediff_LaborPushing2 = hediff_LaborPushing;
				if (hediff_LaborPushing2 != null)
				{
					value = hediff_LaborPushing2.Progress;
				}
				else
				{
					value = 1f;
				}
			}
			ritual.progressBarOverride = new float?(value);
			base.Tick(ritual);
		}

		// Token: 0x0600D9F5 RID: 55797 RVA: 0x00407BDD File Offset: 0x00405DDD
		public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return "UntilBabyIsBorn".Translate();
		}

		// Token: 0x0600D9F6 RID: 55798 RVA: 0x00407BF0 File Offset: 0x00405DF0
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(assignments.FirstAssignedPawn("mother"), (Building_Bed)target.Thing);
			return new LordJob_Ritual_ChildBirth(target, ritual, obligation, this.def.stages, assignments, null, new IntVec3?(bedSleepingSlotPosFor));
		}

		// Token: 0x0600D9F7 RID: 55799 RVA: 0x00407C38 File Offset: 0x00405E38
		public override bool ShouldInitAsSpectator(Pawn pawn, RitualRoleAssignments assignments)
		{
			Pawn pawn2 = assignments.FirstAssignedPawn("mother");
			if (pawn2 == null)
			{
				return false;
			}
			using (List<DirectPawnRelation>.Enumerator enumerator = LovePartnerRelationUtility.ExistingLovePartners(pawn2, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.otherPawn == pawn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D9F8 RID: 55800 RVA: 0x00407CA4 File Offset: 0x00405EA4
		public override bool TargetStillAllowed(TargetInfo selectedTarget, LordJob_Ritual ritual)
		{
			if (!base.TargetStillAllowed(selectedTarget, ritual))
			{
				return false;
			}
			Pawn pawn = ritual.assignments.FirstAssignedPawn("mother");
			Pawn pawn2 = ritual.assignments.FirstAssignedPawn("doctor");
			Building_Bed building_Bed = (Building_Bed)selectedTarget.Thing;
			ReservationManager reservationManager = building_Bed.Map.reservationManager;
			bool flag = reservationManager.ReservedBy(building_Bed, pawn, null);
			bool flag2 = reservationManager.ReservedBy(building_Bed, pawn2, null);
			return (ritual.StageIndex < 2 || flag2 || flag) && (!flag || PregnancyUtility.IsUsableBedFor(pawn, pawn, building_Bed)) && (!flag2 || PregnancyUtility.IsUsableBedFor(pawn, pawn2, building_Bed)) && RestUtility.CanUseBedNow(building_Bed, pawn, false, false, null);
		}
	}
}
