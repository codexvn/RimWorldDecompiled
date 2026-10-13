using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002B4D RID: 11085
	public class FlyShipLeaving : Skyfaller, IActiveTransporter, IThingHolder
	{
		// Token: 0x170026E3 RID: 9955
		// (get) Token: 0x0600F7CF RID: 63439 RVA: 0x00480442 File Offset: 0x0047E642
		// (set) Token: 0x0600F7D0 RID: 63440 RVA: 0x0048045A File Offset: 0x0047E65A
		public ActiveTransporterInfo Contents
		{
			get
			{
				return ((ActiveTransporter)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveTransporter)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x0600F7D1 RID: 63441 RVA: 0x00480E48 File Offset: 0x0047F048
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.alreadyLeft, "alreadyLeft", false, false);
			Scribe_Values.Look<bool>(ref this.createWorldObject, "createWorldObject", true, false);
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjectDef");
		}

		// Token: 0x0600F7D2 RID: 63442 RVA: 0x00480ED0 File Offset: 0x0047F0D0
		protected override void LeaveMap()
		{
			if (this.alreadyLeft || !this.createWorldObject)
			{
				if (this.Contents != null)
				{
					foreach (Thing thing in ((IEnumerable<Thing>)this.Contents.innerContainer))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							pawn.ExitMap(false, Rot4.Invalid);
						}
					}
					this.Contents.innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.QuestLogic);
				}
				base.LeaveMap();
				return;
			}
			if (this.groupID < 0)
			{
				Log.Error(string.Format("Drop pod left the map, but its group ID is {0}", this.groupID));
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!this.destinationTile.Valid)
			{
				Log.Error(string.Format("Drop pod left the map, but its destination tile is {0}", this.destinationTile));
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, base.Map);
			if (lord != null)
			{
				base.Map.lordManager.RemoveLord(lord);
			}
			TravellingTransporters travellingTransporters = (TravellingTransporters)WorldObjectMaker.MakeWorldObject(this.worldObjectDef ?? WorldObjectDefOf.TravellingTransporters);
			travellingTransporters.SetFaction(Faction.OfPlayer);
			travellingTransporters.destinationTile = this.destinationTile;
			travellingTransporters.arrivalAction = this.arrivalAction;
			PlanetTile planetTile = base.Map.Tile;
			if (planetTile.Layer != this.destinationTile.Layer)
			{
				planetTile = this.destinationTile.Layer.GetClosestTile(planetTile);
			}
			travellingTransporters.Tile = planetTile;
			Find.WorldObjects.Add(travellingTransporters);
			FlyShipLeaving.tmpActiveTransporters.Clear();
			FlyShipLeaving.tmpActiveTransporters.AddRange(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ActiveTransporter));
			for (int i = 0; i < FlyShipLeaving.tmpActiveTransporters.Count; i++)
			{
				FlyShipLeaving flyShipLeaving = FlyShipLeaving.tmpActiveTransporters[i] as FlyShipLeaving;
				if (flyShipLeaving != null && flyShipLeaving.groupID == this.groupID)
				{
					flyShipLeaving.alreadyLeft = true;
					travellingTransporters.AddTransporter(flyShipLeaving.Contents, true);
					flyShipLeaving.Contents = null;
					flyShipLeaving.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0400A303 RID: 41731
		public int groupID = -1;

		// Token: 0x0400A304 RID: 41732
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x0400A305 RID: 41733
		public TransportersArrivalAction arrivalAction;

		// Token: 0x0400A306 RID: 41734
		public bool createWorldObject = true;

		// Token: 0x0400A307 RID: 41735
		public WorldObjectDef worldObjectDef;

		// Token: 0x0400A308 RID: 41736
		private bool alreadyLeft;

		// Token: 0x0400A309 RID: 41737
		private static List<Thing> tmpActiveTransporters = new List<Thing>();
	}
}
