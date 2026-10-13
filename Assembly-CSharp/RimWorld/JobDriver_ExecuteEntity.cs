using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001269 RID: 4713
	public class JobDriver_ExecuteEntity : JobDriver
	{
		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x0600734A RID: 29514 RVA: 0x00229653 File Offset: 0x00227853
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x0600734B RID: 29515 RVA: 0x002373B3 File Offset: 0x002355B3
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

		// Token: 0x0600734C RID: 29516 RVA: 0x002373CB File Offset: 0x002355CB
		public override string GetReport()
		{
			return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.InnerPawn, this.job.targetB, this.job.targetC);
		}

		// Token: 0x0600734D RID: 29517 RVA: 0x00237403 File Offset: 0x00235603
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600734E RID: 29518 RVA: 0x00237426 File Offset: 0x00235626
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
				if (this.job.ignoreDesignations)
				{
					return false;
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget = innerPawn.TryGetComp<CompHoldingPlatformTarget>();
				return compHoldingPlatformTarget == null || compHoldingPlatformTarget.containmentMode != EntityContainmentMode.Execute;
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil toil = Toils_General.Do(delegate
			{
				Messages.Message("MessageEntityExecuted".Translate(this.pawn.Named("EXECUTIONER"), this.InnerPawn.Named("VICTIM")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
				ExecutionUtility.DoExecutionByCut(this.pawn, this.InnerPawn, 8, true);
				MentalState mentalState = this.pawn.MentalState;
				if (mentalState == null)
				{
					return;
				}
				mentalState.Notify_SlaughteredTarget();
			});
			toil.activeSkill = (() => SkillDefOf.Melee);
			yield return toil;
			yield break;
		}

		// Token: 0x04004F84 RID: 20356
		private const TargetIndex PlatformIndex = TargetIndex.A;
	}
}
