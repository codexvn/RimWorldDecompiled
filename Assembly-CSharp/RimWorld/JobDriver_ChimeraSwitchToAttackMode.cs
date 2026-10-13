using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001239 RID: 4665
	public class JobDriver_ChimeraSwitchToAttackMode : JobDriver
	{
		// Token: 0x0600721F RID: 29215 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007220 RID: 29216 RVA: 0x002339DF File Offset: 0x00231BDF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.initAction = delegate()
			{
				Lord lord = this.pawn.GetLord();
				if (((lord != null) ? lord.LordJob : null) is LordJob_ChimeraAssault)
				{
					lord.ReceiveMemo("StalkToAttack");
				}
			};
			yield return toil;
			yield break;
		}
	}
}
