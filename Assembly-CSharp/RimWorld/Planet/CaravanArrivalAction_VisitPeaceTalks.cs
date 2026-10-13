using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D2A RID: 15658
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x17003521 RID: 13601
		// (get) Token: 0x060154FF RID: 87295 RVA: 0x0065F6A9 File Offset: 0x0065D8A9
		public override string Label
		{
			get
			{
				return "VisitPeaceTalks".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x17003522 RID: 13602
		// (get) Token: 0x06015500 RID: 87296 RVA: 0x0065F6CA File Offset: 0x0065D8CA
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x06015501 RID: 87297 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x06015502 RID: 87298 RVA: 0x0065F6EB File Offset: 0x0065D8EB
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x06015503 RID: 87299 RVA: 0x0065F6FC File Offset: 0x0065D8FC
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.peaceTalks != null && this.peaceTalks.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, this.peaceTalks);
		}

		// Token: 0x06015504 RID: 87300 RVA: 0x0065F74A File Offset: 0x0065D94A
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x06015505 RID: 87301 RVA: 0x0065F758 File Offset: 0x0065D958
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x06015506 RID: 87302 RVA: 0x0065F771 File Offset: 0x0065D971
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x06015507 RID: 87303 RVA: 0x0065F784 File Offset: 0x0065D984
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(peaceTalks.Label), caravan, peaceTalks.Tile, peaceTalks, null);
		}

		// Token: 0x0400ED8A RID: 60810
		private PeaceTalks peaceTalks;
	}
}
