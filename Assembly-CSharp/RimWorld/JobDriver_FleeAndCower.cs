using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200127F RID: 4735
	public class JobDriver_FleeAndCower : JobDriver_Flee
	{
		// Token: 0x060073D8 RID: 29656 RVA: 0x00238DA4 File Offset: 0x00236FA4
		public override string GetReport()
		{
			if (this.pawn.CurJob == this.job && this.pawn.Position == this.job.GetTarget(TargetIndex.A).Cell)
			{
				return "ReportCowering".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x00238E00 File Offset: 0x00237000
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.defaultCompleteMode = ToilCompleteMode.Delay;
			toil2.defaultDuration = 1200;
			toil2.tickIntervalAction = delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(35, delta) && SelfDefenseUtility.ShouldStartFleeing(this.pawn))
				{
					base.EndJobWith(JobCondition.InterruptForced);
				}
			};
			yield return toil2;
			yield break;
			yield break;
		}

		// Token: 0x04004FC2 RID: 20418
		private const int CowerTicks = 1200;

		// Token: 0x04004FC3 RID: 20419
		private const int CheckFleeAgainIntervalTicks = 35;
	}
}
