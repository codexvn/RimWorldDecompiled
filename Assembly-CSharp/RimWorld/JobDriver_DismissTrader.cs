using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200124F RID: 4687
	public class JobDriver_DismissTrader : JobDriver
	{
		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x060072AE RID: 29358 RVA: 0x000024E7 File Offset: 0x000006E7
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x060072AF RID: 29359 RVA: 0x002355AF File Offset: 0x002337AF
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Trader, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060072B0 RID: 29360 RVA: 0x002355D2 File Offset: 0x002337D2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOn(() => !this.Trader.CanTradeNow);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (!this.Trader.CanTradeNow)
				{
					return;
				}
				this.Trader.mindState.traderDismissed = true;
			};
			yield return toil;
			yield break;
		}
	}
}
