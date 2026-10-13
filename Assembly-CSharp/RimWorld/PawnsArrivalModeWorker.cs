using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAE RID: 6830
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x06009900 RID: 39168 RVA: 0x002C95EC File Offset: 0x002C77EC
		public virtual bool CanUseWith(IncidentParms parms)
		{
			if (parms.faction != null)
			{
				if (this.def.minTechLevel != TechLevel.Undefined && parms.faction.def.techLevel < this.def.minTechLevel)
				{
					return false;
				}
				List<PawnsArrivalModeDef> arrivalModeWhitelist = parms.faction.def.arrivalModeWhitelist;
				List<PawnsArrivalModeDef> arrivalModeBlacklist = parms.faction.def.arrivalModeBlacklist;
				if (!arrivalModeWhitelist.NullOrEmpty<PawnsArrivalModeDef>() && !arrivalModeWhitelist.Contains(this.def))
				{
					return false;
				}
				if (!arrivalModeBlacklist.NullOrEmpty<PawnsArrivalModeDef>() && arrivalModeBlacklist.Contains(this.def))
				{
					return false;
				}
			}
			if (parms.raidArrivalModeForQuickMilitaryAid && !this.def.forQuickMilitaryAid)
			{
				return false;
			}
			if (parms.raidStrategy != null && !parms.raidStrategy.arriveModes.Contains(this.def))
			{
				return false;
			}
			Map map = parms.target as Map;
			return map == null || this.CanUseOnMap(map);
		}

		// Token: 0x06009901 RID: 39169 RVA: 0x002C96D4 File Offset: 0x002C78D4
		public virtual bool CanUseOnMap(Map map)
		{
			return (this.def.biomeWhitelist.NullOrEmpty<BiomeDef>() || this.def.biomeWhitelist.Contains(map.Biome)) && (this.def.biomeBlacklist.NullOrEmpty<BiomeDef>() || !this.def.biomeBlacklist.Contains(map.Biome)) && (!map.Biome.onlyAllowWhitelistedArrivalModes || (!this.def.biomeWhitelist.NullOrEmpty<BiomeDef>() && this.def.biomeWhitelist.Contains(map.Biome))) && (!map.Tile.Valid || this.CanUseOnTile(map.Tile)) && (!this.def.walkIn || map.CanEverExit);
		}

		// Token: 0x06009902 RID: 39170 RVA: 0x002C97AC File Offset: 0x002C79AC
		public virtual bool CanUseOnTile(PlanetTile tile)
		{
			return (this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || this.def.layerWhitelist.Contains(tile.LayerDef)) && (this.def.layerBlacklist.NullOrEmpty<PlanetLayerDef>() || !this.def.layerBlacklist.Contains(tile.LayerDef)) && (!tile.LayerDef.onlyAllowWhitelistedArrivalModes || (!this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && this.def.layerWhitelist.Contains(tile.LayerDef)));
		}

		// Token: 0x06009903 RID: 39171 RVA: 0x002C9850 File Offset: 0x002C7A50
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			if (this.def.selectionWeightCurvesPerFaction != null && parms.faction != null)
			{
				List<FactionCurve> selectionWeightCurvesPerFaction = this.def.selectionWeightCurvesPerFaction;
				for (int i = 0; i < selectionWeightCurvesPerFaction.Count; i++)
				{
					if (selectionWeightCurvesPerFaction[i].faction == parms.faction.def)
					{
						return selectionWeightCurvesPerFaction[i].Evaluate(parms.points);
					}
				}
			}
			if (this.def.selectionWeightCurve != null)
			{
				return this.def.selectionWeightCurve.Evaluate(parms.points);
			}
			return 0f;
		}

		// Token: 0x06009904 RID: 39172
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x06009905 RID: 39173 RVA: 0x002C98E4 File Offset: 0x002C7AE4
		public virtual void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x06009906 RID: 39174
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);

		// Token: 0x040067E1 RID: 26593
		public PawnsArrivalModeDef def;
	}
}
