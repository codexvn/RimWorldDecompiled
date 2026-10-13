using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E0C RID: 15884
	public class TransportersArrivalAction_LandInSpecificCell : TransportersArrivalAction
	{
		// Token: 0x170035FA RID: 13818
		// (get) Token: 0x06015AB7 RID: 88759 RVA: 0x00002501 File Offset: 0x00000701
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06015AB8 RID: 88760 RVA: 0x0067B2C7 File Offset: 0x006794C7
		public TransportersArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x06015AB9 RID: 88761 RVA: 0x0067C10E File Offset: 0x0067A30E
		public TransportersArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x06015ABA RID: 88762 RVA: 0x0067C124 File Offset: 0x0067A324
		public TransportersArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell, Rot4 rotation, bool landInShuttle)
		{
			this.mapParent = mapParent;
			this.cell = cell;
			this.rotation = rotation;
			this.landInShuttle = landInShuttle;
		}

		// Token: 0x06015ABB RID: 88763 RVA: 0x0067C14C File Offset: 0x0067A34C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.landInShuttle, "landInShuttle", false, false);
		}

		// Token: 0x06015ABC RID: 88764 RVA: 0x0067C1B8 File Offset: 0x0067A3B8
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this.mapParent);
		}

		// Token: 0x06015ABD RID: 88765 RVA: 0x0067C20C File Offset: 0x0067A40C
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			if (this.landInShuttle)
			{
				if (transporters.Count > 1)
				{
					Log.Error("Shuttles can only have one transporter in group");
				}
				TransportersArrivalActionUtility.DropShuttle(transporters.FirstOrDefault<ActiveTransporterInfo>(), this.mapParent.Map, this.cell, new Rot4?(this.rotation), null);
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06015ABE RID: 88766 RVA: 0x0067C2BC File Offset: 0x0067A4BC
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false))));
		}

		// Token: 0x0400F0B8 RID: 61624
		private MapParent mapParent;

		// Token: 0x0400F0B9 RID: 61625
		private IntVec3 cell;

		// Token: 0x0400F0BA RID: 61626
		private Rot4 rotation;

		// Token: 0x0400F0BB RID: 61627
		private bool landInShuttle;
	}
}
