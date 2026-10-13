using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D2C RID: 15660
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x17003523 RID: 13603
		// (get) Token: 0x0601550B RID: 87307 RVA: 0x0065F819 File Offset: 0x0065DA19
		public override string Label
		{
			get
			{
				return "VisitSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x17003524 RID: 13604
		// (get) Token: 0x0601550C RID: 87308 RVA: 0x0065F83A File Offset: 0x0065DA3A
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0601550D RID: 87309 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x0601550E RID: 87310 RVA: 0x0065F85B File Offset: 0x0065DA5B
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0601550F RID: 87311 RVA: 0x0065F86C File Offset: 0x0065DA6C
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
			return CaravanArrivalAction_VisitSettlement.CanVisit(caravan, this.settlement);
		}

		// Token: 0x06015510 RID: 87312 RVA: 0x0065F8BC File Offset: 0x0065DABC
		public override void Arrived(Caravan caravan)
		{
			if (caravan.IsPlayerControlled)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.settlement), "LetterCaravanEnteredMap".Translate(caravan.Label, this.settlement).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06015511 RID: 87313 RVA: 0x0065F929 File Offset: 0x0065DB29
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06015512 RID: 87314 RVA: 0x0065F942 File Offset: 0x0065DB42
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x06015513 RID: 87315 RVA: 0x0065F960 File Offset: 0x0065DB60
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x0400ED8D RID: 60813
		private Settlement settlement;
	}
}
