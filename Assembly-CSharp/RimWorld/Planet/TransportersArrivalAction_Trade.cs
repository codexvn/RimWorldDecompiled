using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E0F RID: 15887
	public class TransportersArrivalAction_Trade : TransportersArrivalAction_VisitSettlement
	{
		// Token: 0x06015AC8 RID: 88776 RVA: 0x0067C471 File Offset: 0x0067A671
		public TransportersArrivalAction_Trade()
		{
		}

		// Token: 0x06015AC9 RID: 88777 RVA: 0x0067C479 File Offset: 0x0067A679
		public TransportersArrivalAction_Trade(Settlement settlement, string arrivalMessageKey) : base(settlement, arrivalMessageKey)
		{
		}

		// Token: 0x06015ACA RID: 88778 RVA: 0x0067C484 File Offset: 0x0067A684
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportersArrivalAction_Trade.CanTradeWith(pods, this.settlement);
		}

		// Token: 0x06015ACB RID: 88779 RVA: 0x0067C4B0 File Offset: 0x0067A6B0
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Pawn pawn = null;
			int num = 0;
			while (num < transporters.Count && pawn == null)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)transporters[num].GetDirectlyHeldThings()))
				{
					Pawn pawn2 = thing as Pawn;
					if (pawn2 != null)
					{
						pawn = pawn2;
						break;
					}
				}
				num++;
			}
			base.Arrived(transporters, tile);
			if (pawn != null)
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan != null && CaravanArrivalAction_Trade.HasNegotiator(caravan, this.settlement))
				{
					CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
					Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
					Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
				}
			}
		}

		// Token: 0x06015ACC RID: 88780 RVA: 0x0067C588 File Offset: 0x0067A788
		public static FloatMenuAcceptanceReport CanTradeWith(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (!TransportersArrivalAction_VisitSettlement.CanVisit(pods, settlement))
			{
				return false;
			}
			if (settlement.Faction == null || settlement.Faction == Faction.OfPlayer)
			{
				return false;
			}
			bool flag = false;
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				CompTransporter compTransporter = thingHolder as CompTransporter;
				if (compTransporter != null && CaravanShuttleUtility.IsCaravanShuttle(compTransporter))
				{
					directlyHeldThings = compTransporter.parent.GetCaravan().GetDirectlyHeldThings();
				}
				foreach (Thing thing in ((IEnumerable<Thing>)directlyHeldThings))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.RaceProps.Humanlike && pawn.CanTradeWith(settlement.Faction, settlement.TraderKind).Accepted)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			return flag && !settlement.HasMap && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow;
		}
	}
}
