using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E14 RID: 15892
	public class TransportersArrivalAction_VisitSpace : TransportersArrivalAction
	{
		// Token: 0x170035FE RID: 13822
		// (get) Token: 0x06015AE9 RID: 88809 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06015AEA RID: 88810 RVA: 0x0067B2C7 File Offset: 0x006794C7
		public TransportersArrivalAction_VisitSpace()
		{
		}

		// Token: 0x06015AEB RID: 88811 RVA: 0x0067CDB3 File Offset: 0x0067AFB3
		public TransportersArrivalAction_VisitSpace(MapParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x06015AEC RID: 88812 RVA: 0x0067CDC2 File Offset: 0x0067AFC2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x06015AED RID: 88813 RVA: 0x0067CDDC File Offset: 0x0067AFDC
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.parent != null && this.parent.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_VisitSpace.CanVisit(pods, this.parent);
		}

		// Token: 0x06015AEE RID: 88814 RVA: 0x0067CE2A File Offset: 0x0067B02A
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.parent.HasMap;
		}

		// Token: 0x06015AEF RID: 88815 RVA: 0x0067CE3C File Offset: 0x0067B03C
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			IntVec3 size = Find.World.info.initialMapSize;
			if (this.parent.def.overrideMapSize != null)
			{
				size = this.parent.def.overrideMapSize.Value;
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.parent.Tile, size, this.parent.def, null, false);
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(orGenerateMap, false, true);
			if (transporters.IsShuttle())
			{
				TransportersArrivalActionUtility.DropShuttle(transporters[0], orGenerateMap, near, null, null);
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, orGenerateMap);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06015AF0 RID: 88816 RVA: 0x0067CF20 File Offset: 0x0067B120
		private static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, MapParent parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06015AF1 RID: 88817 RVA: 0x0067CF49 File Offset: 0x0067B149
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, MapParent parent)
		{
			TransportersArrivalAction_VisitSpace.<>c__DisplayClass10_0 CS$<>8__locals1 = new TransportersArrivalAction_VisitSpace.<>c__DisplayClass10_0();
			CS$<>8__locals1.parent = parent;
			CS$<>8__locals1.pods = pods;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter = CS$<>8__locals1.<>9__2) == null)
			{
				acceptanceReportGetter = (CS$<>8__locals1.<>9__2 = (() => TransportersArrivalAction_VisitSpace.CanVisit(CS$<>8__locals1.pods, CS$<>8__locals1.parent)));
			}
			Func<TransportersArrivalAction_VisitSpace> arrivalActionGetter;
			if ((arrivalActionGetter = CS$<>8__locals1.<>9__3) == null)
			{
				arrivalActionGetter = (CS$<>8__locals1.<>9__3 = (() => new TransportersArrivalAction_VisitSpace(CS$<>8__locals1.parent)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_VisitSpace>(acceptanceReportGetter, arrivalActionGetter, "LaunchTo".Translate(CS$<>8__locals1.parent.Named("LOCATION")), launchAction, CS$<>8__locals1.parent.Tile, new Action<Action>(CS$<>8__locals1.<GetFloatMenuOptions>g__UIConfirmationCallback|0)))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400F0D5 RID: 61653
		private MapParent parent;
	}
}
