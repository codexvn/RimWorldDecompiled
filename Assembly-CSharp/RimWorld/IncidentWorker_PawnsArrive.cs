using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E08 RID: 7688
	public abstract class IncidentWorker_PawnsArrive : IncidentWorker
	{
		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x0600AB23 RID: 43811 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool MustHaveSettlementOnLayer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AB24 RID: 43812 RVA: 0x0030EA80 File Offset: 0x0030CC80
		protected IEnumerable<Faction> CandidateFactions(IncidentParms parms, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, parms, desperate)
			select f;
		}

		// Token: 0x0600AB25 RID: 43813 RVA: 0x0030EAC4 File Offset: 0x0030CCC4
		public virtual bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			Map map = (Map)parms.target;
			return !f.IsPlayer && !f.defeated && !f.temporary && (desperate || (f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) && f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.SeasonalTemp))) && (!this.MustHaveSettlementOnLayer || f.Hidden || !map.Tile.Valid || Find.WorldObjects.AnyFactionSettlementOnLayer(f, map.Tile.Layer)) && (f.def.arrivalLayerWhitelist.NullOrEmpty<PlanetLayerDef>() || f.def.arrivalLayerWhitelist.Contains(map.Tile.LayerDef)) && (f.def.arrivalLayerBlacklist.NullOrEmpty<PlanetLayerDef>() || !f.def.arrivalLayerBlacklist.Contains(map.Tile.LayerDef)) && (!map.Tile.LayerDef.onlyAllowWhitelistedArrivals || (!f.def.arrivalLayerWhitelist.NullOrEmpty<PlanetLayerDef>() && f.def.arrivalLayerWhitelist.Contains(map.Tile.LayerDef)));
		}

		// Token: 0x0600AB26 RID: 43814 RVA: 0x0030EC29 File Offset: 0x0030CE29
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return parms.faction != null || this.CandidateFactions(parms, false).Any<Faction>();
		}

		// Token: 0x0600AB27 RID: 43815 RVA: 0x0030EC44 File Offset: 0x0030CE44
		public string DebugListingOfGroupSources()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IncidentParms parms = new IncidentParms
			{
				target = Find.CurrentMap
			};
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				stringBuilder.Append(faction.Name);
				if (this.FactionCanBeGroupSource(faction, parms, false))
				{
					stringBuilder.Append("    YES");
				}
				else if (this.FactionCanBeGroupSource(faction, parms, true))
				{
					stringBuilder.Append("    YES-DESPERATE");
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
