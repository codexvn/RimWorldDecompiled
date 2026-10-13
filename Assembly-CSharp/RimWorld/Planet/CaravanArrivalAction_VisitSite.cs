using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D2E RID: 15662
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x17003525 RID: 13605
		// (get) Token: 0x06015517 RID: 87319 RVA: 0x0065F9F5 File Offset: 0x0065DBF5
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x17003526 RID: 13606
		// (get) Token: 0x06015518 RID: 87320 RVA: 0x0065FA02 File Offset: 0x0065DC02
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x06015519 RID: 87321 RVA: 0x0065EA59 File Offset: 0x0065CC59
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x0601551A RID: 87322 RVA: 0x0065FA0F File Offset: 0x0065DC0F
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x0601551B RID: 87323 RVA: 0x0065FA20 File Offset: 0x0065DC20
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitSite.CanVisit(caravan, this.site);
		}

		// Token: 0x0601551C RID: 87324 RVA: 0x0065FA70 File Offset: 0x0065DC70
		public override void Arrived(Caravan caravan)
		{
			if (!this.site.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoEnter(caravan, this.site);
				}, "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			this.DoEnter(caravan, this.site);
		}

		// Token: 0x0601551D RID: 87325 RVA: 0x0065FACC File Offset: 0x0065DCCC
		private void DoEnter(Caravan caravan, Site site)
		{
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			bool draftColonists = site.Faction == null || site.Faction.HostileTo(Faction.OfPlayer);
			bool flag = !site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, site.PreferredMapSize, null, null, false);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsSite".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst());
				LetterDef letterDef;
				LookTargets lookTargets2;
				this.AppendThreatInfo(stringBuilder, site, orGenerateMap, out letterDef, out lookTargets2);
				TaggedString taggedString = null;
				if (site.parts.Any((SitePart part) => part.def.considerEnteringAsAttack))
				{
					SettlementUtility.AffectRelationsOnAttacked(site, ref taggedString);
				}
				if (!taggedString.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						if (stringBuilder[stringBuilder.Length - 1] != '\n')
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLineTagged(taggedString);
				}
				List<HediffDef> list = null;
				foreach (SitePart sitePart in site.parts)
				{
					if (!sitePart.def.arrivedLetterHediffHyperlinks.NullOrEmpty<HediffDef>())
					{
						if (list == null)
						{
							list = new List<HediffDef>();
						}
						foreach (HediffDef item in sitePart.def.arrivedLetterHediffHyperlinks)
						{
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
				}
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("LetterLabelCaravanEnteredMap".Translate(site), stringBuilder.ToString(), letterDef ?? LetterDefOf.NeutralEvent, lookTargets2.IsValid() ? lookTargets2 : lookTargets, null, null, null);
				choiceLetter.hyperlinkHediffDefs = list;
				Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
			}
			else
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(site), "LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null, 0, true);
			}
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, draftColonists, null);
		}

		// Token: 0x0601551E RID: 87326 RVA: 0x0065FD88 File Offset: 0x0065DF88
		private void AppendThreatInfo(StringBuilder sb, Site site, Map map, out LetterDef letterDef, out LookTargets allLookTargets)
		{
			allLookTargets = new LookTargets();
			CaravanArrivalAction_VisitSite.tmpUsedDefs.Clear();
			CaravanArrivalAction_VisitSite.tmpDefs.Clear();
			for (int i = 0; i < site.parts.Count; i++)
			{
				CaravanArrivalAction_VisitSite.tmpDefs.Add(site.parts[i].def);
			}
			letterDef = null;
			for (int j = 0; j < CaravanArrivalAction_VisitSite.tmpDefs.Count; j++)
			{
				LetterDef letterDef2;
				LookTargets lookTargets;
				string arrivedLetterPart = CaravanArrivalAction_VisitSite.tmpDefs[j].Worker.GetArrivedLetterPart(map, out letterDef2, out lookTargets);
				if (arrivedLetterPart != null)
				{
					if (!CaravanArrivalAction_VisitSite.tmpUsedDefs.Contains(CaravanArrivalAction_VisitSite.tmpDefs[j]))
					{
						CaravanArrivalAction_VisitSite.tmpUsedDefs.Add(CaravanArrivalAction_VisitSite.tmpDefs[j]);
						if (sb.Length > 0)
						{
							sb.AppendLine();
							sb.AppendLine();
						}
						sb.Append(arrivedLetterPart);
					}
					if (letterDef == null)
					{
						letterDef = letterDef2;
					}
					if (lookTargets.IsValid())
					{
						allLookTargets = new LookTargets(allLookTargets.targets.Concat(lookTargets.targets));
					}
				}
			}
		}

		// Token: 0x0601551F RID: 87327 RVA: 0x0065FE99 File Offset: 0x0065E099
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x06015520 RID: 87328 RVA: 0x0065FEB4 File Offset: 0x0065E0B4
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x06015521 RID: 87329 RVA: 0x0065FF0C File Offset: 0x0065E10C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site, null);
		}

		// Token: 0x0400ED90 RID: 60816
		private Site site;

		// Token: 0x0400ED91 RID: 60817
		private static List<SitePartDef> tmpDefs = new List<SitePartDef>();

		// Token: 0x0400ED92 RID: 60818
		private static List<SitePartDef> tmpUsedDefs = new List<SitePartDef>();
	}
}
