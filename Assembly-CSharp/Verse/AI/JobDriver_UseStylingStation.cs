using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000EA6 RID: 3750
	public class JobDriver_UseStylingStation : JobDriver
	{
		// Token: 0x06005E74 RID: 24180 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005E75 RID: 24181 RVA: 0x001E083B File Offset: 0x001DEA3B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_StyleChange.DoLookChange(TargetIndex.A, this.pawn);
			yield return Toils_StyleChange.FinalizeLookChange();
			yield break;
		}
	}
}
