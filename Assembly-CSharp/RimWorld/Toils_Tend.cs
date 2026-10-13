using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011C8 RID: 4552
	public class Toils_Tend
	{
		// Token: 0x06006F5A RID: 28506 RVA: 0x0022A7A4 File Offset: 0x002289A4
		public static Toil ReserveMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = ToilMaker.MakeToil("ReserveMedicine");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false);
				if (num <= 0 || !actor.Reserve(thing, curJob, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(injured)), null, true, false))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x0022A80C File Offset: 0x00228A0C
		public static Toil PickupMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = ToilMaker.MakeToil("PickupMedicine");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = Medicine.GetMedicineCountToFullyHeal(injured);
				if (actor.carryTracker.CarriedThing != null)
				{
					num -= actor.carryTracker.CarriedThing.stackCount;
				}
				int num2 = Mathf.Min(actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false), num);
				if (num2 > 0)
				{
					actor.carryTracker.TryStartCarry(thing, num2, true);
				}
				curJob.count = num - num2;
				if (thing.Spawned)
				{
					toil.actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				curJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x0022A868 File Offset: 0x00228A68
		public static Toil FinalizeTend(Pawn patient)
		{
			Toil toil = ToilMaker.MakeToil("FinalizeTend");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Medicine medicine = (Medicine)actor.CurJob.targetB.Thing;
				if (actor.skills != null)
				{
					float num = patient.RaceProps.Animal ? 175f : 500f;
					float num2 = (medicine == null) ? 0.5f : medicine.def.MedicineTendXpGainFactor;
					actor.skills.Learn(SkillDefOf.Medicine, num * num2, false, false);
				}
				TendUtility.DoTend(actor, patient, medicine);
				if (medicine != null && medicine.Destroyed)
				{
					actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
				}
				if (toil.actor.CurJob.endAfterTendedOnce)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04004D9E RID: 19870
		public const int MaxMedicineReservations = 10;
	}
}
