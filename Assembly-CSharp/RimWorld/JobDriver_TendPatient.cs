using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011C3 RID: 4547
	public class JobDriver_TendPatient : JobDriver
	{
		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06006F35 RID: 28469 RVA: 0x001E5D54 File Offset: 0x001E3F54
		protected Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06006F36 RID: 28470 RVA: 0x00229C37 File Offset: 0x00227E37
		protected Pawn Deliveree
		{
			get
			{
				return this.job.targetA.Pawn;
			}
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x06006F37 RID: 28471 RVA: 0x00229C49 File Offset: 0x00227E49
		protected bool IsMedicineInDoctorInventory
		{
			get
			{
				return this.MedicineUsed != null && this.pawn.inventory.Contains(this.MedicineUsed);
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x06006F38 RID: 28472 RVA: 0x00229C6B File Offset: 0x00227E6B
		protected Pawn_InventoryTracker MedicineHolderInventory
		{
			get
			{
				Thing medicineUsed = this.MedicineUsed;
				return ((medicineUsed != null) ? medicineUsed.ParentHolder : null) as Pawn_InventoryTracker;
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x06006F39 RID: 28473 RVA: 0x00229C84 File Offset: 0x00227E84
		protected Pawn OtherPawnMedicineHolder
		{
			get
			{
				return this.job.targetC.Pawn;
			}
		}

		// Token: 0x06006F3A RID: 28474 RVA: 0x00229C96 File Offset: 0x00227E96
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
			Scribe_Values.Look<PathEndMode>(ref this.pathEndMode, "pathEndMode", PathEndMode.None, false);
		}

		// Token: 0x06006F3B RID: 28475 RVA: 0x00229CC4 File Offset: 0x00227EC4
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
			if (this.Deliveree == this.pawn)
			{
				this.pathEndMode = PathEndMode.OnCell;
				return;
			}
			if (this.Deliveree.InBed())
			{
				this.pathEndMode = PathEndMode.InteractionCell;
				return;
			}
			if (this.Deliveree != this.pawn)
			{
				this.pathEndMode = PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06006F3C RID: 28476 RVA: 0x00229D28 File Offset: 0x00227F28
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.MedicineUsed != null)
			{
				int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
				if (num <= 0 || !this.pawn.Reserve(this.MedicineUsed, this.job, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.Deliveree)), null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006F3D RID: 28477 RVA: 0x00229DC8 File Offset: 0x00227FC8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => (this.MedicineUsed != null && this.pawn.Faction == Faction.OfPlayer && this.Deliveree.playerSettings != null && !this.Deliveree.playerSettings.medCare.AllowsMedicine(this.MedicineUsed.def)) || (this.pawn == this.Deliveree && this.pawn.Faction == Faction.OfPlayer && this.pawn.playerSettings != null && !this.pawn.playerSettings.selfTend));
			base.AddEndCondition(delegate
			{
				if (this.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(this.Deliveree))
				{
					return JobCondition.Ongoing;
				}
				if ((this.job.playerForced || this.pawn.Faction != Faction.OfPlayer) && this.Deliveree.health.HasHediffsNeedingTend(false))
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			this.FailOnAggroMentalState(TargetIndex.A);
			Toil reserveMedicine = null;
			Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, this.pathEndMode, false);
			if (this.usesMedicine)
			{
				List<Toil> list = JobDriver_TendPatient.CollectMedicineToils(this.pawn, this.Deliveree, this.job, gotoToil, out reserveMedicine);
				foreach (Toil toil in list)
				{
					yield return toil;
				}
				List<Toil>.Enumerator enumerator = default(List<Toil>.Enumerator);
			}
			yield return gotoToil;
			int ticks = (int)(1f / this.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true, -1) * 600f);
			Toil waitToil;
			if (!this.job.draftedTend || this.pawn == base.TargetPawnA)
			{
				waitToil = Toils_General.Wait(ticks, TargetIndex.None);
			}
			else
			{
				waitToil = Toils_General.WaitWith(TargetIndex.A, ticks, false, true, false, TargetIndex.A, this.pathEndMode);
				waitToil.AddFinishAction(delegate
				{
					if (this.Deliveree != null && this.Deliveree != this.pawn && this.Deliveree.CurJob != null && (this.Deliveree.CurJob.def == JobDefOf.Wait || this.Deliveree.CurJob.def == JobDefOf.Wait_MaintainPosture))
					{
						this.Deliveree.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				});
			}
			waitToil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend, 1f);
			waitToil.activeSkill = (() => SkillDefOf.Medicine);
			waitToil.handlingFacing = true;
			waitToil.tickIntervalAction = delegate(int delta)
			{
				if (this.pawn == this.Deliveree && this.pawn.Faction != Faction.OfPlayer && this.pawn.IsHashIntervalTick(100, delta) && !this.pawn.Position.Fogged(this.pawn.Map))
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.HealingCross, 0.42f);
				}
				if (this.pawn != this.Deliveree)
				{
					this.pawn.rotationTracker.FaceTarget(this.Deliveree);
				}
			};
			waitToil.FailOn(() => this.pawn != this.Deliveree && !this.pawn.CanReachImmediate(this.Deliveree.SpawnedParentOrMe, this.pathEndMode));
			yield return Toils_Jump.JumpIf(waitToil, () => !this.usesMedicine || !this.IsMedicineInDoctorInventory);
			yield return Toils_Tend.PickupMedicine(TargetIndex.B, this.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
			yield return waitToil;
			yield return Toils_Tend.FinalizeTend(this.Deliveree);
			if (this.usesMedicine)
			{
				yield return JobDriver_TendPatient.FindMoreMedicineToil(this.pawn, this.Deliveree, TargetIndex.B, this.job, reserveMedicine);
			}
			yield return Toils_Jump.Jump(gotoToil);
			yield break;
			yield break;
		}

		// Token: 0x06006F3E RID: 28478 RVA: 0x00229DD8 File Offset: 0x00227FD8
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.Faction != Faction.OfPlayer && this.pawn == this.Deliveree)
			{
				this.pawn.jobs.CheckForJobOverride(0f, true);
			}
		}

		// Token: 0x06006F3F RID: 28479 RVA: 0x00229E38 File Offset: 0x00228038
		public static List<Toil> CollectMedicineToils(Pawn doctor, Pawn patient, Job job, Toil gotoToil, out Toil reserveMedicine)
		{
			JobDriver_TendPatient.<>c__DisplayClass22_0 CS$<>8__locals1 = new JobDriver_TendPatient.<>c__DisplayClass22_0();
			CS$<>8__locals1.doctor = doctor;
			JobDriver_TendPatient.tmpCollectToils.Clear();
			CS$<>8__locals1.medicineUsed = job.targetB.Thing;
			JobDriver_TendPatient.<>c__DisplayClass22_0 CS$<>8__locals2 = CS$<>8__locals1;
			Thing medicineUsed = CS$<>8__locals1.medicineUsed;
			CS$<>8__locals2.medicineHolderInventory = (((medicineUsed != null) ? medicineUsed.ParentHolder : null) as Pawn_InventoryTracker);
			CS$<>8__locals1.otherPawnMedicineHolder = job.targetC.Pawn;
			reserveMedicine = Toils_Tend.ReserveMedicine(TargetIndex.B, patient).FailOnDespawnedNullOrForbidden(TargetIndex.B);
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Jump.JumpIf(gotoToil, () => CS$<>8__locals1.medicineUsed != null && CS$<>8__locals1.doctor.inventory.Contains(CS$<>8__locals1.medicineUsed)));
			Toil toil = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch, false).FailOn(delegate()
			{
				Pawn otherPawnMedicineHolder = CS$<>8__locals1.otherPawnMedicineHolder;
				Pawn_InventoryTracker medicineHolderInventory2 = CS$<>8__locals1.medicineHolderInventory;
				return otherPawnMedicineHolder != ((medicineHolderInventory2 != null) ? medicineHolderInventory2.pawn : null) || CS$<>8__locals1.otherPawnMedicineHolder.IsForbidden(CS$<>8__locals1.doctor);
			});
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Haul.CheckItemCarriedByOtherPawn(CS$<>8__locals1.medicineUsed, TargetIndex.C, toil));
			JobDriver_TendPatient.tmpCollectToils.Add(reserveMedicine);
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B));
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Tend.PickupMedicine(TargetIndex.B, patient).FailOnDestroyedOrNull(TargetIndex.B));
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Haul.CheckForGetOpportunityDuplicate(reserveMedicine, TargetIndex.B, TargetIndex.None, true, null));
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Jump.Jump(gotoToil));
			JobDriver_TendPatient.tmpCollectToils.Add(toil);
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_General.Wait(25, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.C, false, -0.5f));
			List<Toil> list = JobDriver_TendPatient.tmpCollectToils;
			Thing medicineUsed2 = CS$<>8__locals1.medicineUsed;
			ThingOwner innerContainer = CS$<>8__locals1.doctor.inventory.innerContainer;
			Pawn_InventoryTracker medicineHolderInventory = CS$<>8__locals1.medicineHolderInventory;
			list.Add(Toils_Haul.TakeFromOtherInventory(medicineUsed2, innerContainer, (medicineHolderInventory != null) ? medicineHolderInventory.innerContainer : null, Medicine.GetMedicineCountToFullyHeal(patient), TargetIndex.B));
			return JobDriver_TendPatient.tmpCollectToils;
		}

		// Token: 0x06006F40 RID: 28480 RVA: 0x00229FC8 File Offset: 0x002281C8
		public static Toil FindMoreMedicineToil(Pawn doctor, Pawn patient, TargetIndex medicineIndex, Job job, Toil reserveMedicine)
		{
			Toil toil = ToilMaker.MakeToil("FindMoreMedicineToil");
			toil.initAction = delegate()
			{
				if (job.GetTarget(medicineIndex).Thing.DestroyedOrNull())
				{
					Thing thing = HealthAIUtility.FindBestMedicine(doctor, patient, false);
					if (thing != null)
					{
						job.SetTarget(medicineIndex, thing);
						doctor.jobs.curDriver.JumpToToil(reserveMedicine);
					}
				}
			};
			return toil;
		}

		// Token: 0x04004D84 RID: 19844
		private bool usesMedicine;

		// Token: 0x04004D85 RID: 19845
		private PathEndMode pathEndMode;

		// Token: 0x04004D86 RID: 19846
		public const int BaseTendDuration = 600;

		// Token: 0x04004D87 RID: 19847
		private const int TicksBetweenSelfTendMotes = 100;

		// Token: 0x04004D88 RID: 19848
		private const TargetIndex MedicineIndex = TargetIndex.B;

		// Token: 0x04004D89 RID: 19849
		private const TargetIndex MedicineHolderIndex = TargetIndex.C;

		// Token: 0x04004D8A RID: 19850
		private static List<Toil> tmpCollectToils = new List<Toil>();
	}
}
