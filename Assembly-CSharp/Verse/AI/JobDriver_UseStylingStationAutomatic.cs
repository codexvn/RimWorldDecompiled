using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000EA8 RID: 3752
	public class JobDriver_UseStylingStationAutomatic : JobDriver
	{
		// Token: 0x06005E7F RID: 24191 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005E80 RID: 24192 RVA: 0x001E0967 File Offset: 0x001DEB67
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				yield break;
			}
			this.FailOn(() => !this.pawn.style.LookChangeDesired);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_StyleChange.SetupLookChangeData();
			yield return Toils_StyleChange.DoLookChange(TargetIndex.A, this.pawn);
			yield return Toils_StyleChange.FinalizeLookChange();
			yield break;
		}
	}
}
