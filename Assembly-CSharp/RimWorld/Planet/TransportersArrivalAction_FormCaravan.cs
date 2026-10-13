using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E06 RID: 15878
	public class TransportersArrivalAction_FormCaravan : TransportersArrivalAction
	{
		// Token: 0x170035F7 RID: 13815
		// (get) Token: 0x06015A94 RID: 88724 RVA: 0x00002501 File Offset: 0x00000701
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06015A95 RID: 88725 RVA: 0x0067B8B3 File Offset: 0x00679AB3
		public TransportersArrivalAction_FormCaravan()
		{
		}

		// Token: 0x06015A96 RID: 88726 RVA: 0x0067B8C6 File Offset: 0x00679AC6
		public TransportersArrivalAction_FormCaravan(string arrivalMessageKey)
		{
			this.arrivalMessageKey = arrivalMessageKey;
		}

		// Token: 0x06015A97 RID: 88727 RVA: 0x0067B8E0 File Offset: 0x00679AE0
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportersArrivalAction_FormCaravan.CanFormCaravanAt(pods, destinationTile);
		}

		// Token: 0x06015A98 RID: 88728 RVA: 0x0067B90C File Offset: 0x00679B0C
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			TransportersArrivalAction_FormCaravan.tmpPawns.Clear();
			for (int i = 0; i < transporters.Count; i++)
			{
				ThingOwner innerContainer = transporters[i].innerContainer;
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null)
					{
						TransportersArrivalAction_FormCaravan.tmpPawns.Add(pawn);
						innerContainer.Remove(pawn);
					}
				}
			}
			PlanetTile startingTile;
			if (!GenWorldClosest.TryFindClosestPassableTile(tile, out startingTile))
			{
				startingTile = tile;
			}
			Caravan caravan = CaravanMaker.MakeCaravan(TransportersArrivalAction_FormCaravan.tmpPawns, Faction.OfPlayer, startingTile, true);
			if (transporters.IsShuttle())
			{
				CaravanInventoryUtility.GiveThing(caravan, transporters[0].RemoveShuttle());
			}
			for (int k = 0; k < transporters.Count; k++)
			{
				TransportersArrivalAction_FormCaravan.tmpContainedThings.Clear();
				TransportersArrivalAction_FormCaravan.tmpContainedThings.AddRange(transporters[k].innerContainer);
				for (int l = 0; l < TransportersArrivalAction_FormCaravan.tmpContainedThings.Count; l++)
				{
					transporters[k].innerContainer.Remove(TransportersArrivalAction_FormCaravan.tmpContainedThings[l]);
					CaravanInventoryUtility.GiveThing(caravan, TransportersArrivalAction_FormCaravan.tmpContainedThings[l]);
				}
			}
			TransportersArrivalAction_FormCaravan.tmpPawns.Clear();
			TransportersArrivalAction_FormCaravan.tmpContainedThings.Clear();
			Messages.Message(this.arrivalMessageKey.Translate(), caravan, MessageTypeDefOf.TaskCompletion, true);
			PeaceTalks peaceTalks = Find.WorldObjects.WorldObjectAt<PeaceTalks>(tile);
			if (peaceTalks != null)
			{
				peaceTalks.Notify_CaravanArrived(caravan);
			}
		}

		// Token: 0x06015A99 RID: 88729 RVA: 0x0067BA87 File Offset: 0x00679C87
		public static bool CanFormCaravanAt(IEnumerable<IThingHolder> pods, PlanetTile tile)
		{
			return TransportersArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer) && !Find.World.Impassable(tile) && tile.LayerDef.canFormCaravans;
		}

		// Token: 0x06015A9A RID: 88730 RVA: 0x0067BAB1 File Offset: 0x00679CB1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.arrivalMessageKey, "arrivalMessageKey", "MessageTransportPodsArrived", false);
		}

		// Token: 0x0400F0AC RID: 61612
		private string arrivalMessageKey = "MessageTransportPodsArrived";

		// Token: 0x0400F0AD RID: 61613
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400F0AE RID: 61614
		private static readonly List<Thing> tmpContainedThings = new List<Thing>();
	}
}
