using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200120C RID: 4620
	public class JobDriver_Skydreaming : JobDriver
	{
		// Token: 0x060070DA RID: 28890 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060070DB RID: 28891 RVA: 0x0022F867 File Offset: 0x0022DA67
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnChildLearningConditions<JobDriver_Skydreaming>();
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				LearningUtility.LearningTickCheckEnd(this.pawn, delta, false);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.learningDuration;
			toil.FailOn(() => this.pawn.Position.Roofed(this.pawn.Map));
			yield return toil;
			yield break;
		}
	}
}
