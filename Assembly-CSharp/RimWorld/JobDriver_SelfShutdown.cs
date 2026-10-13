using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012AE RID: 4782
	public class JobDriver_SelfShutdown : JobDriver
	{
		// Token: 0x0600752B RID: 29995 RVA: 0x0023CE8C File Offset: 0x0023B08C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return ModLister.CheckBiotech("Self-shutdown") && this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600752C RID: 29996 RVA: 0x0023CEB8 File Offset: 0x0023B0B8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = Toils_LayDown.SelfShutdown();
			toil.PlaySoundAtStart(SoundDefOf.MechSelfShutdown);
			yield return toil;
			yield break;
		}

		// Token: 0x04005049 RID: 20553
		public const TargetIndex RestSpotIndex = TargetIndex.A;
	}
}
