using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A1C RID: 14876
	public class QuestPart_AddShipJob_Arrive : QuestPart_AddShipJob
	{
		// Token: 0x0601460A RID: 83466 RVA: 0x00616FCD File Offset: 0x006151CD
		public override bool QuestPartReserves(Faction faction)
		{
			return this.factionForArrival != null && faction == this.factionForArrival;
		}

		// Token: 0x0601460B RID: 83467 RVA: 0x00616FE2 File Offset: 0x006151E2
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.factionForArrival == faction)
			{
				this.factionForArrival = null;
			}
		}

		// Token: 0x0601460C RID: 83468 RVA: 0x00616FF4 File Offset: 0x006151F4
		public override ShipJob GetShipJob()
		{
			if (this.mapOfPawn == null && this.mapParent.Destroyed)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
				this.cell = DropCellFinder.TryFindSafeLandingSpotCloseToColony(this.mapParent.Map, ThingDefOf.Shuttle.Size + new IntVec2(2, 2), null, 2);
			}
			ShipJob_Arrive shipJob_Arrive = (ShipJob_Arrive)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_Arrive.cell = this.cell;
			shipJob_Arrive.mapParent = this.mapParent;
			shipJob_Arrive.mapOfPawn = this.mapOfPawn;
			shipJob_Arrive.factionForArrival = this.factionForArrival;
			return shipJob_Arrive;
		}

		// Token: 0x0601460D RID: 83469 RVA: 0x0061709C File Offset: 0x0061529C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapOfPawn, "mapOfPawn", false);
			Scribe_References.Look<Faction>(ref this.factionForArrival, "factionForArrival", false);
		}

		// Token: 0x0400E111 RID: 57617
		public IntVec3 cell;

		// Token: 0x0400E112 RID: 57618
		public MapParent mapParent;

		// Token: 0x0400E113 RID: 57619
		public Pawn mapOfPawn;

		// Token: 0x0400E114 RID: 57620
		public Faction factionForArrival;
	}
}
