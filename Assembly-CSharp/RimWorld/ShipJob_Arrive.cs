using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001778 RID: 6008
	public class ShipJob_Arrive : ShipJob
	{
		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x06008D00 RID: 36096 RVA: 0x0029EFD2 File Offset: 0x0029D1D2
		protected override bool ShouldEnd
		{
			get
			{
				TransportShip transportShip = this.transportShip;
				return ((transportShip != null) ? transportShip.shipThing : null) != null && this.transportShip.shipThing.Spawned;
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x06008D01 RID: 36097 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Interruptible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008D02 RID: 36098 RVA: 0x0029EFFC File Offset: 0x0029D1FC
		public override bool TryStart()
		{
			if (!base.TryStart())
			{
				Log.Error("Failed to start arrive ship job");
				return false;
			}
			Pawn pawn = this.mapOfPawn;
			MapParent mapParent;
			if (pawn == null)
			{
				mapParent = null;
			}
			else
			{
				Map mapHeld = pawn.MapHeld;
				mapParent = ((mapHeld != null) ? mapHeld.Parent : null);
			}
			MapParent mapParent2 = mapParent ?? this.mapParent;
			if (mapParent2 == null || !mapParent2.HasMap)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap == null)
				{
					Log.Error("Trying to start ShipJob_Arrive with a null map.");
					return false;
				}
				mapParent2 = anyPlayerHomeMap.Parent;
			}
			if (!this.cell.IsValid)
			{
				this.cell = DropCellFinder.GetBestShuttleLandingSpot(mapParent2.Map, this.factionForArrival ?? Faction.OfPlayer);
			}
			ThingOwner innerContainer = this.transportShip.TransporterComp.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn2 = innerContainer[i] as Pawn;
				if (pawn2 != null && pawn2.IsWorldPawn())
				{
					Find.WorldPawns.RemovePawn(pawn2);
				}
			}
			GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(this.transportShip.def.arrivingSkyfaller, this.transportShip.shipThing), this.cell, mapParent2.Map, WipeMode.Vanish);
			QuestUtility.SendQuestTargetSignals(this.transportShip.questTags, "Arrived", this.transportShip.Named("SUBJECT"));
			return true;
		}

		// Token: 0x06008D03 RID: 36099 RVA: 0x0029F140 File Offset: 0x0029D340
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapOfPawn, "mapOfPawn", false);
			Scribe_References.Look<Faction>(ref this.factionForArrival, "factionForArrival", false);
		}

		// Token: 0x04005B9F RID: 23455
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x04005BA0 RID: 23456
		public Pawn mapOfPawn;

		// Token: 0x04005BA1 RID: 23457
		public MapParent mapParent;

		// Token: 0x04005BA2 RID: 23458
		public Faction factionForArrival;
	}
}
