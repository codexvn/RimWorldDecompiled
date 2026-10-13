using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000EA4 RID: 3748
	public class JobDriver_OpenStylingStationDialog : JobDriver
	{
		// Token: 0x06005E68 RID: 24168 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005E69 RID: 24169 RVA: 0x001E0706 File Offset: 0x001DE906
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				Find.WindowStack.Add(new Dialog_StylingStation(this.pawn, this.job.targetA.Thing));
			});
			yield break;
		}
	}
}
