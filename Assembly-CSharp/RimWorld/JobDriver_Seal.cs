using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001312 RID: 4882
	public class JobDriver_Seal : JobDriver
	{
		// Token: 0x060077D1 RID: 30673 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060077D2 RID: 30674 RVA: 0x00245C3B File Offset: 0x00243E3B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 300, true, false, false, TargetIndex.None, PathEndMode.Touch);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				AncientHatch ancientHatch = this.job.GetTarget(TargetIndex.A).Thing as AncientHatch;
				if (ancientHatch == null)
				{
					return;
				}
				ancientHatch.Seal();
			};
			yield return toil;
			yield break;
		}

		// Token: 0x04005194 RID: 20884
		private const TargetIndex HatchInd = TargetIndex.A;
	}
}
