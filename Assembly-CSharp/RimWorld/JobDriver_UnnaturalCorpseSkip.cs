using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200133E RID: 4926
	public class JobDriver_UnnaturalCorpseSkip : JobDriver
	{
		// Token: 0x06007909 RID: 30985 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x00249A77 File Offset: 0x00247C77
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				SkipUtility.SkipTo(this.pawn, base.TargetA.Cell, this.pawn.MapHeld);
				this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
			};
			yield return toil;
			yield break;
		}

		// Token: 0x0400522E RID: 21038
		private const TargetIndex DestIndex = TargetIndex.A;
	}
}
