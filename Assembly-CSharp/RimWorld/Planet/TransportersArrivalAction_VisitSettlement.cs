using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E0D RID: 15885
	public class TransportersArrivalAction_VisitSettlement : TransportersArrivalAction_FormCaravan
	{
		// Token: 0x06015ABF RID: 88767 RVA: 0x0067C316 File Offset: 0x0067A516
		public TransportersArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06015AC0 RID: 88768 RVA: 0x0067C31E File Offset: 0x0067A51E
		public TransportersArrivalAction_VisitSettlement(Settlement settlement, string arrivalMessageKey) : base(arrivalMessageKey)
		{
			this.settlement = settlement;
		}

		// Token: 0x06015AC1 RID: 88769 RVA: 0x0067C32E File Offset: 0x0067A52E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06015AC2 RID: 88770 RVA: 0x0067C348 File Offset: 0x0067A548
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_VisitSettlement.CanVisit(pods, this.settlement);
		}

		// Token: 0x06015AC3 RID: 88771 RVA: 0x0067C396 File Offset: 0x0067A596
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Visitable)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06015AC4 RID: 88772 RVA: 0x0067C3CC File Offset: 0x0067A5CC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Settlement settlement, bool isShuttle)
		{
			return TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_VisitSettlement>(() => TransportersArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportersArrivalAction_VisitSettlement(settlement, isShuttle ? "MessageShuttleArrived" : "MessageTransportPodsArrived"), "VisitSettlement".Translate(settlement.Label), launchAction, settlement.Tile, null);
		}

		// Token: 0x0400F0BC RID: 61628
		protected Settlement settlement;
	}
}
