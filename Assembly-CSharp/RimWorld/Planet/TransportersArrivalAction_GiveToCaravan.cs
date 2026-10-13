using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E0A RID: 15882
	public class TransportersArrivalAction_GiveToCaravan : TransportersArrivalAction
	{
		// Token: 0x170035F9 RID: 13817
		// (get) Token: 0x06015AAB RID: 88747 RVA: 0x00002501 File Offset: 0x00000701
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06015AAC RID: 88748 RVA: 0x0067B2C7 File Offset: 0x006794C7
		public TransportersArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x06015AAD RID: 88749 RVA: 0x0067BF17 File Offset: 0x0067A117
		public TransportersArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06015AAE RID: 88750 RVA: 0x0067BF26 File Offset: 0x0067A126
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x06015AAF RID: 88751 RVA: 0x0067BF40 File Offset: 0x0067A140
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.caravan != null && !Find.WorldGrid.IsNeighborOrSame(this.caravan.Tile, destinationTile))
			{
				return false;
			}
			return TransportersArrivalAction_GiveToCaravan.CanGiveTo(pods, this.caravan);
		}

		// Token: 0x06015AB0 RID: 88752 RVA: 0x0067BF94 File Offset: 0x0067A194
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				TransportersArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
				TransportersArrivalAction_GiveToCaravan.tmpContainedThings.AddRange(transporters[i].innerContainer);
				for (int j = 0; j < TransportersArrivalAction_GiveToCaravan.tmpContainedThings.Count; j++)
				{
					transporters[i].innerContainer.Remove(TransportersArrivalAction_GiveToCaravan.tmpContainedThings[j]);
					this.caravan.AddPawnOrItem(TransportersArrivalAction_GiveToCaravan.tmpContainedThings[j], true);
				}
			}
			TransportersArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrivedAndAddedToCaravan".Translate(this.caravan.Name), this.caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06015AB1 RID: 88753 RVA: 0x0067C05A File Offset: 0x0067A25A
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x06015AB2 RID: 88754 RVA: 0x0067C078 File Offset: 0x0067A278
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_GiveToCaravan>(() => TransportersArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportersArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(caravan.Label), launchAction, caravan.Tile, null);
		}

		// Token: 0x0400F0B4 RID: 61620
		private Caravan caravan;

		// Token: 0x0400F0B5 RID: 61621
		private static readonly List<Thing> tmpContainedThings = new List<Thing>();
	}
}
