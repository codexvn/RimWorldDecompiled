using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001218 RID: 4632
	public class JobDriver_ActivityGoPassive : JobDriver
	{
		// Token: 0x06007129 RID: 28969 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600712A RID: 28970 RVA: 0x00230617 File Offset: 0x0022E817
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.AnomalyInstalled)
			{
				yield break;
			}
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				toil.actor.GetComp<CompActivity>().EnterPassiveState();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}
	}
}
