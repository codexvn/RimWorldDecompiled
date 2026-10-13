using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E10 RID: 15888
	public class TransportersArrivalAction_VisitSite : TransportersArrivalAction
	{
		// Token: 0x170035FB RID: 13819
		// (get) Token: 0x06015ACD RID: 88781 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06015ACE RID: 88782 RVA: 0x0067B2C7 File Offset: 0x006794C7
		public TransportersArrivalAction_VisitSite()
		{
		}

		// Token: 0x06015ACF RID: 88783 RVA: 0x0067C6DC File Offset: 0x0067A8DC
		public TransportersArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
		{
			this.site = site;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06015AD0 RID: 88784 RVA: 0x0067C6F2 File Offset: 0x0067A8F2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06015AD1 RID: 88785 RVA: 0x0067C71C File Offset: 0x0067A91C
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_VisitSite.CanVisit(pods, this.site);
		}

		// Token: 0x06015AD2 RID: 88786 RVA: 0x0067C76A File Offset: 0x0067A96A
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.site.HasMap;
		}

		// Token: 0x06015AD3 RID: 88787 RVA: 0x0067C77C File Offset: 0x0067A97C
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			bool flag = !this.site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.site.Tile, this.site.PreferredMapSize, null, null, false);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			}
			if (this.site.Faction != null && this.site.Faction != Faction.OfPlayer && this.site.MainSitePartDef.considerEnteringAsAttack)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.site.Faction, Faction.OfPlayer.GoodwillToMakeHostile(this.site.Faction), true, true, HistoryEventDefOf.AttackedSettlement, null);
			}
			if (transporters.IsShuttle())
			{
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
			}
			else
			{
				Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
			}
			this.arrivalMode.Worker.TravellingTransportersArrived(transporters, orGenerateMap);
		}

		// Token: 0x06015AD4 RID: 88788 RVA: 0x0067C8CC File Offset: 0x0067AACC
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x06015AD5 RID: 88789 RVA: 0x0067C932 File Offset: 0x0067AB32
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Site site)
		{
			TransportersArrivalAction_VisitSite.<>c__DisplayClass11_0 CS$<>8__locals1 = new TransportersArrivalAction_VisitSite.<>c__DisplayClass11_0();
			CS$<>8__locals1.site = site;
			CS$<>8__locals1.pods = pods;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter = CS$<>8__locals1.<>9__2) == null)
			{
				acceptanceReportGetter = (CS$<>8__locals1.<>9__2 = (() => TransportersArrivalAction_VisitSite.CanVisit(CS$<>8__locals1.pods, CS$<>8__locals1.site)));
			}
			Func<TransportersArrivalAction_VisitSite> arrivalActionGetter;
			if ((arrivalActionGetter = CS$<>8__locals1.<>9__3) == null)
			{
				arrivalActionGetter = (CS$<>8__locals1.<>9__3 = (() => new TransportersArrivalAction_VisitSite(CS$<>8__locals1.site, PawnsArrivalModeDefOf.EdgeDrop)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_VisitSite>(acceptanceReportGetter, arrivalActionGetter, "DropAtEdge".Translate(), launchAction, CS$<>8__locals1.site.Tile, new Action<Action>(CS$<>8__locals1.<GetFloatMenuOptions>g__UIConfirmationCallback|0)))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2;
			if ((acceptanceReportGetter2 = CS$<>8__locals1.<>9__4) == null)
			{
				acceptanceReportGetter2 = (CS$<>8__locals1.<>9__4 = (() => TransportersArrivalAction_VisitSite.CanVisit(CS$<>8__locals1.pods, CS$<>8__locals1.site)));
			}
			Func<TransportersArrivalAction_VisitSite> arrivalActionGetter2;
			if ((arrivalActionGetter2 = CS$<>8__locals1.<>9__5) == null)
			{
				arrivalActionGetter2 = (CS$<>8__locals1.<>9__5 = (() => new TransportersArrivalAction_VisitSite(CS$<>8__locals1.site, PawnsArrivalModeDefOf.CenterDrop)));
			}
			foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_VisitSite>(acceptanceReportGetter2, arrivalActionGetter2, "DropInCenter".Translate(), launchAction, CS$<>8__locals1.site.Tile, new Action<Action>(CS$<>8__locals1.<GetFloatMenuOptions>g__UIConfirmationCallback|0)))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400F0C0 RID: 61632
		private Site site;

		// Token: 0x0400F0C1 RID: 61633
		private PawnsArrivalModeDef arrivalMode;
	}
}
