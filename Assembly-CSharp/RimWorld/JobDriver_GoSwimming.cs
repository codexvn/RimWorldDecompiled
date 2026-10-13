using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011DC RID: 4572
	public class JobDriver_GoSwimming : JobDriver
	{
		// Token: 0x06006FB6 RID: 28598 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x0022BD54 File Offset: 0x00229F54
		private bool NextDestIsOutdoorsAndNotEnjoyable()
		{
			if (base.Map == null)
			{
				return false;
			}
			if (!this.job.targetA.IsValid)
			{
				return false;
			}
			Room room = this.job.targetA.Cell.GetRoom(base.Map);
			return room != null && room.PsychologicallyOutdoors && !JoyGiver_GoSwimming.HappyToSwimOutsideOnMap(base.Map);
		}

		// Token: 0x06006FB8 RID: 28600 RVA: 0x0022BDB6 File Offset: 0x00229FB6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(new Func<bool>(this.NextDestIsOutdoorsAndNotEnjoyable));
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			toil.tickIntervalAction = delegate(int delta)
			{
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				this.CheckForSwimmingPose();
				this.pawn.mindState.lastSwamTick = GenTicks.TicksGame;
			};
			yield return toil;
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickIntervalAction = delegate(int delta)
			{
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				this.CheckForSwimmingPose();
				this.pawn.mindState.lastSwamTick = GenTicks.TicksGame;
				JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, null);
			};
			goToil.AddFinishAction(new Action(this.CheckForSwimmingPose));
			yield return goToil;
			Toil toil2 = Toils_General.Wait(240, TargetIndex.None);
			toil2.tickIntervalAction = delegate(int delta)
			{
				this.CheckForSwimmingPose();
				JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, null);
				this.pawn.mindState.lastSwamTick = GenTicks.TicksGame;
			};
			yield return toil2;
			Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
			toil3.initAction = delegate()
			{
				if (this.job.targetQueueA.Count <= 0)
				{
					return;
				}
				Pawn_HealthTracker health = this.pawn.health;
				Hediff hediff;
				if (((health != null) ? health.hediffSet : null) != null && this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.Heatstroke, out hediff))
				{
					this.pawn.health.RemoveHediff(hediff);
				}
				LocalTargetInfo targetA = this.job.targetQueueA[0];
				this.job.targetQueueA.RemoveAt(0);
				this.job.targetA = targetA;
				this.JumpToToil(goToil);
			};
			yield return toil3;
			yield break;
		}

		// Token: 0x06006FB9 RID: 28601 RVA: 0x0022BDC6 File Offset: 0x00229FC6
		private void CheckForSwimmingPose()
		{
			if (this.pawn.Position.GetTerrain(this.pawn.Map).IsWater)
			{
				this.job.swimming = true;
				return;
			}
			this.job.swimming = false;
		}
	}
}
