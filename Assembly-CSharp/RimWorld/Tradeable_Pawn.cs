using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FAB RID: 12203
	public class Tradeable_Pawn : Tradeable
	{
		// Token: 0x17002C7C RID: 11388
		// (get) Token: 0x06011305 RID: 70405 RVA: 0x004ED4A4 File Offset: 0x004EB6A4
		public override Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.AnyPawn, null);
			}
		}

		// Token: 0x17002C7D RID: 11389
		// (get) Token: 0x06011306 RID: 70406 RVA: 0x004ED4B4 File Offset: 0x004EB6B4
		public override string Label
		{
			get
			{
				string text = base.Label;
				if (this.AnyPawn.Name != null && !this.AnyPawn.Name.Numerical && !this.AnyPawn.RaceProps.Humanlike)
				{
					text = text + ", " + this.AnyPawn.def.label;
				}
				return string.Concat(new string[]
				{
					text,
					" (",
					this.AnyPawn.GetGenderLabel(),
					", ",
					Mathf.FloorToInt(this.AnyPawn.ageTracker.AgeBiologicalYearsFloat).ToString(),
					")"
				});
			}
		}

		// Token: 0x17002C7E RID: 11390
		// (get) Token: 0x06011307 RID: 70407 RVA: 0x004ED56C File Offset: 0x004EB76C
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyPawn.MainDesc(true, true) + "\n\n" + this.AnyPawn.DescriptionDetailed;
			}
		}

		// Token: 0x17002C7F RID: 11391
		// (get) Token: 0x06011308 RID: 70408 RVA: 0x004ED59E File Offset: 0x004EB79E
		private Pawn AnyPawn
		{
			get
			{
				return (Pawn)this.AnyThing;
			}
		}

		// Token: 0x06011309 RID: 70409 RVA: 0x004ED5AC File Offset: 0x004EB7AC
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerSells)
			{
				List<Pawn> list = this.thingsColony.Take(base.CountToTransferToDestination).Cast<Pawn>().ToList<Pawn>();
				for (int i = 0; i < list.Count; i++)
				{
					bool flag = GuestUtility.IsSellingToSlavery(list[i], TradeSession.trader.Faction);
					TradeSession.trader.GiveSoldThingToTrader(list[i], 1, TradeSession.playerNegotiator);
					if (flag)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SoldSlave, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer)), true);
					}
				}
				return;
			}
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				List<Pawn> list2 = this.thingsTrader.Take(base.CountToTransferToSource).Cast<Pawn>().ToList<Pawn>();
				for (int j = 0; j < list2.Count; j++)
				{
					TradeSession.trader.GiveSoldThingToPlayer(list2[j], 1, TradeSession.playerNegotiator);
				}
			}
		}
	}
}
