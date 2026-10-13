using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001334 RID: 4916
	public class JobDriver_TradeWithPawn : JobDriver
	{
		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x060078CC RID: 30924 RVA: 0x000024E7 File Offset: 0x000006E7
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x060078CD RID: 30925 RVA: 0x00248D0B File Offset: 0x00246F0B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Trader, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x00248D2E File Offset: 0x00246F2E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOn(() => !this.Trader.CanTradeNow);
			Toil trade = ToilMaker.MakeToil("MakeNewToils");
			trade.initAction = delegate()
			{
				Pawn actor = trade.actor;
				if (this.Trader.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(actor, this.Trader, false));
				}
			};
			yield return trade;
			yield break;
		}
	}
}
