using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FAC RID: 12204
	public class Tradeable_RoyalFavor : Tradeable
	{
		// Token: 0x17002C80 RID: 11392
		// (get) Token: 0x0601130B RID: 70411 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsFavor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002C81 RID: 11393
		// (get) Token: 0x0601130C RID: 70412 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsCurrency
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002C82 RID: 11394
		// (get) Token: 0x0601130D RID: 70413 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TraderWillTrade
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002C83 RID: 11395
		// (get) Token: 0x0601130E RID: 70414 RVA: 0x00002501 File Offset: 0x00000701
		public override bool IsThing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002C84 RID: 11396
		// (get) Token: 0x0601130F RID: 70415 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002C85 RID: 11397
		// (get) Token: 0x06011310 RID: 70416 RVA: 0x00002C42 File Offset: 0x00000E42
		public override Thing AnyThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002C86 RID: 11398
		// (get) Token: 0x06011311 RID: 70417 RVA: 0x004ED69B File Offset: 0x004EB89B
		public override string Label
		{
			get
			{
				return TradeSession.trader.Faction.def.royalFavorLabel;
			}
		}

		// Token: 0x17002C87 RID: 11399
		// (get) Token: 0x06011312 RID: 70418 RVA: 0x004ED6B1 File Offset: 0x004EB8B1
		public override string TipDescription
		{
			get
			{
				return "RoyalFavorDescription".Translate(TradeSession.trader.Faction.Named("FACTION"));
			}
		}

		// Token: 0x06011313 RID: 70419 RVA: 0x004ED6D6 File Offset: 0x004EB8D6
		public override int CostToInt(float cost)
		{
			return Mathf.CeilToInt(cost);
		}

		// Token: 0x06011314 RID: 70420 RVA: 0x004ED6DE File Offset: 0x004EB8DE
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				TradeSession.playerNegotiator.royalty.GainFavor(TradeSession.trader.Faction, base.CountToTransferToSource);
			}
		}

		// Token: 0x06011315 RID: 70421 RVA: 0x004ED708 File Offset: 0x004EB908
		public override void DrawIcon(Rect iconRect)
		{
			Faction faction = TradeSession.trader.Faction;
			GUI.color = faction.Color;
			Widgets.DrawTextureRotated(iconRect, faction.def.FactionIcon, 0f, null);
			GUI.color = Color.white;
		}

		// Token: 0x06011316 RID: 70422 RVA: 0x004ED74C File Offset: 0x004EB94C
		public override int CountHeldBy(Transactor trans)
		{
			if (trans == Transactor.Trader)
			{
				return 99999;
			}
			return 0;
		}

		// Token: 0x06011317 RID: 70423 RVA: 0x00002501 File Offset: 0x00000701
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
