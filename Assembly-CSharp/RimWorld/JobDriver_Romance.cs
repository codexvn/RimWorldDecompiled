using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001310 RID: 4880
	public class JobDriver_Romance : JobDriver
	{
		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x060077C3 RID: 30659 RVA: 0x002459B0 File Offset: 0x00243BB0
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060077C4 RID: 30660 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060077C5 RID: 30661 RVA: 0x002459D6 File Offset: 0x00243BD6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			Toil finalGoto = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Jump.JumpIf(finalGoto, () => !this.OtherPawn.Awake());
			Toil toil2 = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			toil2.socialMode = RandomSocialMode.Off;
			yield return toil2;
			finalGoto.socialMode = RandomSocialMode.Off;
			yield return finalGoto;
			yield return Toils_General.Do(delegate
			{
				if (!this.OtherPawn.Awake())
				{
					this.OtherPawn.jobs.SuspendCurrentJob(JobCondition.InterruptForced, true, null);
					if (!this.pawn.interactions.CanInteractNowWith(this.OtherPawn, InteractionDefOf.RomanceAttempt))
					{
						Messages.Message("RomanceFailedUnexpected".Translate(this.pawn, this.OtherPawn), MessageTypeDefOf.NegativeEvent, false);
					}
				}
			});
			yield return Toils_Interpersonal.Interact(TargetIndex.A, this.job.interaction);
			yield break;
		}

		// Token: 0x0400518E RID: 20878
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
