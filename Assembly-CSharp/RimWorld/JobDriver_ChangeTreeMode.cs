using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015A3 RID: 5539
	public class JobDriver_ChangeTreeMode : JobDriver
	{
		// Token: 0x060082F8 RID: 33528 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060082F9 RID: 33529 RVA: 0x002751B3 File Offset: 0x002733B3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 120, true, false, false, TargetIndex.None, PathEndMode.Touch);
			yield return Toils_General.Do(delegate
			{
				this.job.targetA.Thing.TryGetComp<CompTreeConnection>().FinalizeMode();
			}).PlaySustainerOrSound(SoundDefOf.DryadCasteSet, 1f);
			yield return Toils_General.Wait(60, TargetIndex.A);
			yield break;
		}

		// Token: 0x040056AA RID: 22186
		private const int WaitTicks = 120;
	}
}
