using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001779 RID: 6009
	public class ShipJob_FlyAway : ShipJob
	{
		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x06008D05 RID: 36101 RVA: 0x0029F1B3 File Offset: 0x0029D3B3
		protected override bool ShouldEnd
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x06008D06 RID: 36102 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Interruptible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x06008D07 RID: 36103 RVA: 0x0029F1BB File Offset: 0x0029D3BB
		public override bool HasDestination
		{
			get
			{
				return this.destinationTile.Valid;
			}
		}

		// Token: 0x06008D08 RID: 36104 RVA: 0x0029F1C8 File Offset: 0x0029D3C8
		public override bool TryStart()
		{
			if (!base.TryStart())
			{
				return false;
			}
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return false;
			}
			if (!this.transportShip.ShuttleComp.AllRequiredThingsLoaded && this.dropMode != TransportShipDropMode.None && this.transportShip.TransporterComp.innerContainer.Any)
			{
				ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload);
				shipJob_Unload.dropMode = this.dropMode;
				this.transportShip.SetNextJob(shipJob_Unload);
				return false;
			}
			IntVec3 position = this.transportShip.shipThing.Position;
			Map map = this.transportShip.shipThing.Map;
			if (!this.transportShip.TransporterComp.LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.transportShip.TransporterComp));
			}
			this.transportShip.TransporterComp.TryRemoveLord(map);
			this.transportShip.ShuttleComp.SendLaunchedSignals();
			QuestUtility.SendQuestTargetSignals(this.transportShip.questTags, "FlewAway", this.transportShip.Named("SUBJECT"));
			ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			activeTransporter.Contents = new ActiveTransporterInfo();
			activeTransporter.Contents.innerContainer.TryAddRangeOrTransfer(this.transportShip.TransporterComp.GetDirectlyHeldThings(), true, true);
			activeTransporter.Rotation = this.transportShip.shipThing.Rotation;
			int groupID = this.transportShip.TransporterComp.groupID;
			if (!this.transportShip.shipThing.Destroyed)
			{
				this.transportShip.shipThing.DeSpawn(DestroyMode.Vanish);
			}
			FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(this.transportShip.def.leavingSkyfaller, activeTransporter);
			flyShipLeaving.groupID = groupID;
			if (!this.destinationTile.Valid)
			{
				flyShipLeaving.createWorldObject = false;
				if (!this.transportShip.shipThing.Destroyed && !Find.QuestManager.IsReservedByAnyQuest(this.transportShip))
				{
					this.transportShip.shipThing.Destroy(DestroyMode.QuestLogic);
				}
			}
			else
			{
				flyShipLeaving.createWorldObject = true;
				flyShipLeaving.worldObjectDef = this.transportShip.def.worldObject;
				flyShipLeaving.destinationTile = this.destinationTile;
				flyShipLeaving.arrivalAction = this.arrivalAction;
			}
			if (this.transportShip.shipThing.HasComp<CompShuttle>())
			{
				activeTransporter.Contents.SetShuttle(this.transportShip.shipThing);
			}
			GenSpawn.Spawn(flyShipLeaving, position, map, WipeMode.Vanish);
			this.initialized = true;
			return true;
		}

		// Token: 0x06008D09 RID: 36105 RVA: 0x0029F44C File Offset: 0x0029D64C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x04005BA3 RID: 23459
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x04005BA4 RID: 23460
		public TransportersArrivalAction arrivalAction;

		// Token: 0x04005BA5 RID: 23461
		public TransportShipDropMode dropMode = TransportShipDropMode.All;

		// Token: 0x04005BA6 RID: 23462
		private bool initialized;
	}
}
