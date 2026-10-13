using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D25 RID: 15653
	public class CaravanArrivalAction_Trade : CaravanArrivalAction
	{
		// Token: 0x1700351D RID: 13597
		// (get) Token: 0x060154E3 RID: 87267 RVA: 0x0065F10D File Offset: 0x0065D30D
		public override string Label
		{
			get
			{
				return "TradeWithSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x1700351E RID: 13598
		// (get) Token: 0x060154E4 RID: 87268 RVA: 0x0065F12E File Offset: 0x0065D32E
		public override string ReportString
		{
			get
			{
				return "CaravanTrading".Translate(this.settlement.Label);
			}
		}

		// Token: 0x060154E5 RID: 87269 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_Trade()
		{
		}

		// Token: 0x060154E6 RID: 87270 RVA: 0x0065F14F File Offset: 0x0065D34F
		public CaravanArrivalAction_Trade(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x060154E7 RID: 87271 RVA: 0x0065F160 File Offset: 0x0065D360
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Trade.CanTradeWith(caravan, this.settlement);
		}

		// Token: 0x060154E8 RID: 87272 RVA: 0x0065F1B0 File Offset: 0x0065D3B0
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
		}

		// Token: 0x060154E9 RID: 87273 RVA: 0x0065F1FD File Offset: 0x0065D3FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x060154EA RID: 87274 RVA: 0x0065F218 File Offset: 0x0065D418
		public static FloatMenuAcceptanceReport CanTradeWith(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_Trade.HasNegotiator(caravan, settlement);
		}

		// Token: 0x060154EB RID: 87275 RVA: 0x0065F288 File Offset: 0x0065D488
		public static bool HasNegotiator(Caravan caravan, Settlement settlement)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, settlement.Faction, settlement.TraderKind);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x060154EC RID: 87276 RVA: 0x0065F2C8 File Offset: 0x0065D4C8
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Trade>(() => CaravanArrivalAction_Trade.CanTradeWith(caravan, settlement), () => new CaravanArrivalAction_Trade(settlement), "TradeWith".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x0400ED82 RID: 60802
		private Settlement settlement;
	}
}
