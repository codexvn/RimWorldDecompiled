using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011C0 RID: 4544
	public class JobDriver_TendEntity : JobDriver
	{
		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x06006F1C RID: 28444 RVA: 0x00229653 File Offset: 0x00227853
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06006F1D RID: 28445 RVA: 0x0022965B File Offset: 0x0022785B
		private Pawn InnerPawn
		{
			get
			{
				Building_HoldingPlatform building_HoldingPlatform = this.Platform as Building_HoldingPlatform;
				if (building_HoldingPlatform == null)
				{
					return null;
				}
				return building_HoldingPlatform.HeldPawn;
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x06006F1E RID: 28446 RVA: 0x001E5D54 File Offset: 0x001E3F54
		private Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x06006F1F RID: 28447 RVA: 0x00229673 File Offset: 0x00227873
		protected bool IsMedicineInDoctorInventory
		{
			get
			{
				return this.MedicineUsed != null && this.pawn.inventory.Contains(this.MedicineUsed);
			}
		}

		// Token: 0x06006F20 RID: 28448 RVA: 0x00229695 File Offset: 0x00227895
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
		}

		// Token: 0x06006F21 RID: 28449 RVA: 0x002296AF File Offset: 0x002278AF
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
		}

		// Token: 0x06006F22 RID: 28450 RVA: 0x002296C8 File Offset: 0x002278C8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.usesMedicine)
			{
				int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
				if (num <= 0)
				{
					return false;
				}
				int stackCount = Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.InnerPawn));
				if (!this.pawn.Reserve(this.MedicineUsed, this.job, 10, stackCount, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006F23 RID: 28451 RVA: 0x0022976C File Offset: 0x0022796C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate()
			{
				Pawn innerPawn = this.InnerPawn;
				if (innerPawn == null || innerPawn.Destroyed)
				{
					return true;
				}
				if (this.MedicineUsed != null && innerPawn.playerSettings != null && !innerPawn.playerSettings.medCare.AllowsMedicine(this.MedicineUsed.def))
				{
					return true;
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget = innerPawn.TryGetComp<CompHoldingPlatformTarget>();
				return compHoldingPlatformTarget != null && compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release;
			});
			base.AddEndCondition(delegate
			{
				if (HealthAIUtility.ShouldBeTendedNowByPlayer(this.InnerPawn))
				{
					return JobCondition.Ongoing;
				}
				if (this.job.playerForced && this.InnerPawn.health.HasHediffsNeedingTend(false))
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			Toil reserveMedicine = null;
			Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			if (this.usesMedicine)
			{
				List<Toil> list = JobDriver_TendPatient.CollectMedicineToils(this.pawn, this.InnerPawn, this.job, gotoToil, out reserveMedicine);
				foreach (Toil toil in list)
				{
					yield return toil;
				}
				List<Toil>.Enumerator enumerator = default(List<Toil>.Enumerator);
			}
			yield return gotoToil;
			int ticks = (int)(1f / this.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true, -1) * 600f);
			Toil waitToil = Toils_General.WaitWith(TargetIndex.A, ticks, true, false, false, TargetIndex.A, PathEndMode.Touch);
			waitToil.activeSkill = (() => SkillDefOf.Medicine);
			waitToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.ClosestTouch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend, 1f);
			yield return Toils_Jump.JumpIf(waitToil, () => !this.usesMedicine || !this.IsMedicineInDoctorInventory);
			yield return Toils_Tend.PickupMedicine(TargetIndex.B, this.InnerPawn).FailOnDestroyedOrNull(TargetIndex.B);
			yield return waitToil;
			yield return Toils_Tend.FinalizeTend(this.InnerPawn);
			if (this.usesMedicine)
			{
				yield return JobDriver_TendPatient.FindMoreMedicineToil(this.pawn, this.InnerPawn, TargetIndex.B, this.job, reserveMedicine);
			}
			yield return Toils_Jump.Jump(gotoToil);
			yield break;
			yield break;
		}

		// Token: 0x06006F24 RID: 28452 RVA: 0x0022977C File Offset: 0x0022797C
		public override string GetReport()
		{
			return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.InnerPawn, this.job.targetB, this.job.targetC);
		}

		// Token: 0x04004D76 RID: 19830
		private bool usesMedicine;

		// Token: 0x04004D77 RID: 19831
		private const TargetIndex PlatformIndex = TargetIndex.A;

		// Token: 0x04004D78 RID: 19832
		private const TargetIndex MedicineIndex = TargetIndex.B;

		// Token: 0x04004D79 RID: 19833
		private const int MaxMedicineReservations = 10;
	}
}
