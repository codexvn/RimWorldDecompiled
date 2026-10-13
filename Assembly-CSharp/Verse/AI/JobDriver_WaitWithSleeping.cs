using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E91 RID: 3729
	public class JobDriver_WaitWithSleeping : JobDriver_Wait
	{
		// Token: 0x06005DF9 RID: 24057 RVA: 0x001DF02C File Offset: 0x001DD22C
		public override void DecorateWaitToil(Toil wait)
		{
			wait.FailOn(delegate()
			{
				Pawn pawn = base.TargetThingB as Pawn;
				return pawn == null || pawn.Dead || pawn.Awake();
			});
		}

		// Token: 0x06005DFA RID: 24058 RVA: 0x001DF041 File Offset: 0x001DD241
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}
	}
}
