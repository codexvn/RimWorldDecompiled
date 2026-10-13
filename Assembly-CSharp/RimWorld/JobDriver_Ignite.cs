using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001295 RID: 4757
	public class JobDriver_Ignite : JobDriver
	{
		// Token: 0x0600747C RID: 29820 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600747D RID: 29821 RVA: 0x0023B159 File Offset: 0x00239359
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnBurningImmobile(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.natives.TryStartIgnite(base.TargetThingA);
			};
			yield return toil;
			yield break;
		}
	}
}
