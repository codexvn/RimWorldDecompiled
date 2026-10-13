using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001215 RID: 4629
	public class JobDriver_ActivateMonolith : JobDriver
	{
		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06007114 RID: 28948 RVA: 0x00230170 File Offset: 0x0022E370
		private Building_VoidMonolith VoidMonolith
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Building_VoidMonolith;
			}
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x00230198 File Offset: 0x0022E398
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (base.TargetThingC != null)
			{
				this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			}
			return (base.TargetThingB == null || this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false)) && this.pawn.Reserve(this.VoidMonolith, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.VoidMonolith.InteractionCell, this.job, errorOnFailed);
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x00230242 File Offset: 0x0022E442
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(delegate()
			{
				string text;
				string text2;
				return !this.VoidMonolith.CanActivate(out text, out text2);
			});
			if (Find.Anomaly.NextLevelDef == MonolithLevelDefOf.VoidAwakened)
			{
				yield return Toils_General.Do(delegate
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("VoidAwakeningConfirmationText".Translate(), delegate()
					{
					}, delegate()
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					}, false, null, WindowLayer.Dialog));
				});
			}
			if (base.TargetThingB != null)
			{
				Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, true).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				yield return getToHaulTarget;
				yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, true);
				yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.B);
				getToHaulTarget = null;
			}
			if (this.job.targetC == null)
			{
				this.job.targetC = base.TargetThingA.SpawnedParentOrMe;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.InteractionCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
			int ticks = Mathf.RoundToInt(600f);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, true, true, false, TargetIndex.A, PathEndMode.Touch);
			toil.WithEffect(EffecterDefOf.MonolithStage2, () => base.TargetA, null);
			Toil toil2 = toil;
			toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, new Action(delegate()
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}));
			if (Find.Anomaly.LevelDef.activateSound != null)
			{
				toil.PlaySustainerOrSound(Find.Anomaly.LevelDef.activateSound, 1f);
			}
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				if (base.TargetThingB != null)
				{
					this.pawn.carryTracker.DestroyCarriedThing();
				}
				this.VoidMonolith.Activate(this.pawn);
			});
			yield return Toils_General.Wait(360, TargetIndex.A);
			yield break;
		}

		// Token: 0x04004E70 RID: 20080
		private const int ActivateSeconds = 10;

		// Token: 0x04004E71 RID: 20081
		private const TargetIndex MonolithIndex = TargetIndex.A;

		// Token: 0x04004E72 RID: 20082
		private const TargetIndex ActivationItemIndex = TargetIndex.B;
	}
}
