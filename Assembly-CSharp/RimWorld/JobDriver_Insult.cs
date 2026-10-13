using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200129D RID: 4765
	public class JobDriver_Insult : JobDriver
	{
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x060074B3 RID: 29875 RVA: 0x0023BA3F File Offset: 0x00239C3F
		private Pawn Target
		{
			get
			{
				return (Pawn)((Thing)this.pawn.CurJob.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x0023BA5C File Offset: 0x00239C5C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.InsultingSpreeDelayToil();
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield return this.InteractToil();
			yield break;
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x0023BA6C File Offset: 0x00239C6C
		private Toil InteractToil()
		{
			return Toils_General.Do(delegate
			{
				if (this.pawn.interactions.TryInteractWith(this.Target, InteractionDefOf.Insult))
				{
					MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
					if (mentalState_InsultingSpree != null)
					{
						mentalState_InsultingSpree.lastInsultTicks = Find.TickManager.TicksGame;
						if (mentalState_InsultingSpree.target == this.Target)
						{
							mentalState_InsultingSpree.insultedTargetAtLeastOnce = true;
						}
					}
				}
			});
		}

		// Token: 0x060074B7 RID: 29879 RVA: 0x0023BA7F File Offset: 0x00239C7F
		private Toil InsultingSpreeDelayToil()
		{
			Toil toil = ToilMaker.MakeToil("InsultingSpreeDelayToil");
			toil.initAction = new Action(this.<InsultingSpreeDelayToil>g__WaitAction|6_0);
			toil.tickIntervalAction = delegate(int delta)
			{
				this.<InsultingSpreeDelayToil>g__WaitAction|6_0();
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x0023BB24 File Offset: 0x00239D24
		[CompilerGenerated]
		private void <InsultingSpreeDelayToil>g__WaitAction|6_0()
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || Find.TickManager.TicksGame - mentalState_InsultingSpree.lastInsultTicks >= 1200)
			{
				this.pawn.jobs.curDriver.ReadyForNextToil();
			}
		}

		// Token: 0x04005023 RID: 20515
		private const TargetIndex TargetInd = TargetIndex.A;
	}
}
