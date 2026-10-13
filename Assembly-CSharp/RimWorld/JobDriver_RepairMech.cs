using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012F8 RID: 4856
	public class JobDriver_RepairMech : JobDriver
	{
		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x0600771E RID: 30494 RVA: 0x00243924 File Offset: 0x00241B24
		protected Pawn Mech
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x0600771F RID: 30495 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool Remote
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06007720 RID: 30496 RVA: 0x0024394A File Offset: 0x00241B4A
		protected int TicksPerHeal
		{
			get
			{
				return Mathf.RoundToInt(1f / this.pawn.GetStatValue(StatDefOf.MechRepairSpeed, true, -1) * 120f);
			}
		}

		// Token: 0x06007721 RID: 30497 RVA: 0x0024396F File Offset: 0x00241B6F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Mech, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007722 RID: 30498 RVA: 0x00243992 File Offset: 0x00241B92
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckBiotech("Mech repair"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnForbidden(TargetIndex.A);
			this.FailOn(() => this.Mech.IsAttacking());
			if (!this.Remote)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			}
			Toil toil = this.Remote ? Toils_General.Wait(int.MaxValue, TargetIndex.None) : Toils_General.WaitWith(TargetIndex.A, int.MaxValue, false, true, true, TargetIndex.None, PathEndMode.Touch);
			toil.WithEffect(EffecterDefOf.MechRepairing, TargetIndex.A, null);
			toil.PlaySustainerOrSound(this.Remote ? SoundDefOf.RepairMech_Remote : SoundDefOf.RepairMech_Touch, 1f);
			toil.AddPreInitAction(delegate
			{
				this.ticksToNextRepair = this.TicksPerHeal;
			});
			toil.handlingFacing = true;
			toil.tickIntervalAction = delegate(int delta)
			{
				this.ticksToNextRepair -= delta;
				if (this.ticksToNextRepair <= 0)
				{
					this.Mech.needs.energy.CurLevel -= this.Mech.GetStatValue(StatDefOf.MechEnergyLossPerHP, true, -1) * (float)delta;
					MechRepairUtility.RepairTick(this.Mech, delta);
					this.ticksToNextRepair = this.TicksPerHeal;
				}
				this.pawn.rotationTracker.FaceTarget(this.Mech);
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Learn(SkillDefOf.Crafting, 0.05f * (float)delta, false, false);
				}
			};
			toil.AddFinishAction(delegate
			{
				Pawn_JobTracker jobs = this.Mech.jobs;
				if (((jobs != null) ? jobs.curJob : null) != null)
				{
					this.Mech.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			});
			toil.AddEndCondition(delegate
			{
				if (!MechRepairUtility.CanRepair(this.Mech))
				{
					return JobCondition.Succeeded;
				}
				return JobCondition.Ongoing;
			});
			if (!this.Remote)
			{
				toil.activeSkill = (() => SkillDefOf.Crafting);
			}
			yield return toil;
			yield break;
		}

		// Token: 0x06007723 RID: 30499 RVA: 0x002439A2 File Offset: 0x00241BA2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToNextRepair, "ticksToNextRepair", 0, false);
		}

		// Token: 0x04005147 RID: 20807
		private const TargetIndex MechInd = TargetIndex.A;

		// Token: 0x04005148 RID: 20808
		private const int DefaultTicksPerHeal = 120;

		// Token: 0x04005149 RID: 20809
		protected int ticksToNextRepair;
	}
}
