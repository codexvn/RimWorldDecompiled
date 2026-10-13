using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DDF RID: 15839
	public static class SiteMaker
	{
		// Token: 0x060159E1 RID: 88545 RVA: 0x00678995 File Offset: 0x00676B95
		public static Site MakeSite(SitePartDef sitePart, PlanetTile tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null, WorldObjectDef worldObjectDef = null)
		{
			return SiteMaker.MakeSite((sitePart != null) ? Gen.YieldSingle<SitePartDef>(sitePart) : null, tile, faction, ifHostileThenMustRemainHostile, threatPoints, worldObjectDef);
		}

		// Token: 0x060159E2 RID: 88546 RVA: 0x006789B0 File Offset: 0x00676BB0
		public static Site MakeSite(IEnumerable<SitePartDef> siteParts, PlanetTile tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null, WorldObjectDef worldObjectDef = null)
		{
			float num = threatPoints ?? StorytellerUtility.DefaultSiteThreatPointsNow();
			List<SitePartDefWithParams> siteParts2;
			SiteMakerHelper.GenerateDefaultParams(num, tile, faction, siteParts, out siteParts2);
			Site site = SiteMaker.MakeSite(siteParts2, tile, faction, ifHostileThenMustRemainHostile, worldObjectDef);
			site.desiredThreatPoints = num;
			return site;
		}

		// Token: 0x060159E3 RID: 88547 RVA: 0x006789F8 File Offset: 0x00676BF8
		public static Site MakeSite(IEnumerable<SitePartDefWithParams> siteParts, PlanetTile tile, Faction faction, bool ifHostileThenMustRemainHostile = true, WorldObjectDef worldObjectDef = null)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(worldObjectDef ?? WorldObjectDefOf.Site);
			site.Tile = tile;
			site.SetFaction(faction);
			if (ifHostileThenMustRemainHostile && faction != null && faction.HostileTo(Faction.OfPlayer))
			{
				site.factionMustRemainHostile = true;
			}
			if (siteParts != null)
			{
				foreach (SitePartDefWithParams sitePartDefWithParams in siteParts)
				{
					site.AddPart(new SitePart(site, sitePartDefWithParams.def, sitePartDefWithParams.parms));
				}
			}
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x060159E4 RID: 88548 RVA: 0x00678AA0 File Offset: 0x00676CA0
		public static Site TryMakeSite_SingleSitePart(IEnumerable<SitePartDef> singleSitePartCandidates, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartCandidates, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x060159E5 RID: 88549 RVA: 0x00678ACC File Offset: 0x00676CCC
		public static Site TryMakeSite_SingleSitePart(string singleSitePartTag, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartTag, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x060159E6 RID: 88550 RVA: 0x00678AF8 File Offset: 0x00676CF8
		public static Site TryMakeSite_MultipleSiteParts(IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsCandidates, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x060159E7 RID: 88551 RVA: 0x00678B24 File Offset: 0x00676D24
		public static Site TryMakeSite_MultipleSiteParts(List<string> sitePartsTags, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsTags, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x060159E8 RID: 88552 RVA: 0x00678B50 File Offset: 0x00676D50
		public static Site TryMakeSite(IEnumerable<SitePartDef> siteParts, PlanetTile tile, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			Faction faction;
			if (!SiteMakerHelper.TryFindRandomFactionFor(siteParts, out faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}
	}
}
