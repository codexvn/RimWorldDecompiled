using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011F6 RID: 4598
	public class JobDriver_VisitSickPawn : JobDriver
	{
		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x06007056 RID: 28758 RVA: 0x0022DCC4 File Offset: 0x0022BEC4
		private Pawn Patient
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x06007057 RID: 28759 RVA: 0x0022DCEC File Offset: 0x0022BEEC
		private Thing Chair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06007058 RID: 28760 RVA: 0x0022DD10 File Offset: 0x0022BF10
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Patient, this.job, 1, -1, null, errorOnFailed, false) && (this.Chair == null || this.pawn.Reserve(this.Chair, this.job, 1, -1, null, errorOnFailed, false));
		}

		// Token: 0x06007059 RID: 28761 RVA: 0x0022DD70 File Offset: 0x0022BF70
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !this.Patient.InBed() || !this.Patient.Awake());
			if (this.Chair != null)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			if (this.Chair != null)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			}
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				Need_Joy joy = this.Patient.needs.joy;
				if (joy != null)
				{
					joy.GainJoy(this.job.def.joyGainRate * 0.000144f * (float)delta, this.job.def.joyKind);
				}
				if (this.pawn.IsHashIntervalTick(320, delta) && this.pawn.interactions.CanInteractNowWith(this.Patient, null))
				{
					InteractionDef intDef = (Rand.Value < 0.8f) ? InteractionDefOf.Chitchat : InteractionDefOf.DeepTalk;
					this.pawn.interactions.TryInteractWith(this.Patient, intDef);
				}
				this.pawn.rotationTracker.FaceCell(this.Patient.Position);
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				if (this.pawn.needs.joy != null && JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.None, 1f, null))
				{
					return;
				}
				Need_Joy joy2 = this.pawn.needs.joy;
				if (joy2 != null && joy2.CurLevelPercentage > 0.9999f)
				{
					Need_Joy joy3 = this.Patient.needs.joy;
					if (joy3 != null && joy3.CurLevelPercentage > 0.9999f)
					{
						base.EndJobWith(JobCondition.Succeeded);
					}
				}
			};
			toil.handlingFacing = true;
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			yield return toil;
			yield break;
		}

		// Token: 0x04004E1B RID: 19995
		private const TargetIndex PatientInd = TargetIndex.A;

		// Token: 0x04004E1C RID: 19996
		private const TargetIndex ChairInd = TargetIndex.B;
	}
}
