using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001135 RID: 4405
	public class JobDriver_Slaughter : JobDriver
	{
		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06006B79 RID: 27513 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x0021D35A File Offset: 0x0021B55A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006B7B RID: 27515 RVA: 0x0021D37D File Offset: 0x0021B57D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.job.ignoreDesignations && !this.Victim.ShouldBeSlaughtered());
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false, false, TargetIndex.None, PathEndMode.Touch);
			yield return Toils_General.Do(delegate
			{
				ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim, 8, true);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredTarget();
				}
			});
			yield break;
		}

		// Token: 0x04004B9D RID: 19357
		public const int SlaughterDuration = 180;
	}
}
