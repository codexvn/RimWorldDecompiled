using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E3F RID: 15935
	public class WorldObjectsHolder : IExposable
	{
		// Token: 0x17003664 RID: 13924
		// (get) Token: 0x06015C6D RID: 89197 RVA: 0x00681AF1 File Offset: 0x0067FCF1
		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		// Token: 0x17003665 RID: 13925
		// (get) Token: 0x06015C6E RID: 89198 RVA: 0x00681AF9 File Offset: 0x0067FCF9
		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		// Token: 0x17003666 RID: 13926
		// (get) Token: 0x06015C6F RID: 89199 RVA: 0x00681B01 File Offset: 0x0067FD01
		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		// Token: 0x17003667 RID: 13927
		// (get) Token: 0x06015C70 RID: 89200 RVA: 0x00681B09 File Offset: 0x0067FD09
		public List<TravellingTransporters> TravellingTransporters
		{
			get
			{
				return this.travellingTransporters;
			}
		}

		// Token: 0x17003668 RID: 13928
		// (get) Token: 0x06015C71 RID: 89201 RVA: 0x00681B11 File Offset: 0x0067FD11
		public List<Settlement> SettlementBases
		{
			get
			{
				return this.settlementBases;
			}
		}

		// Token: 0x17003669 RID: 13929
		// (get) Token: 0x06015C72 RID: 89202 RVA: 0x00681B19 File Offset: 0x0067FD19
		public List<DestroyedSettlement> DestroyedSettlements
		{
			get
			{
				return this.destroyedSettlements;
			}
		}

		// Token: 0x1700366A RID: 13930
		// (get) Token: 0x06015C73 RID: 89203 RVA: 0x00681B21 File Offset: 0x0067FD21
		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		// Token: 0x1700366B RID: 13931
		// (get) Token: 0x06015C74 RID: 89204 RVA: 0x00681B29 File Offset: 0x0067FD29
		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		// Token: 0x1700366C RID: 13932
		// (get) Token: 0x06015C75 RID: 89205 RVA: 0x00681B31 File Offset: 0x0067FD31
		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		// Token: 0x1700366D RID: 13933
		// (get) Token: 0x06015C76 RID: 89206 RVA: 0x00681B39 File Offset: 0x0067FD39
		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		// Token: 0x1700366E RID: 13934
		// (get) Token: 0x06015C77 RID: 89207 RVA: 0x00681B41 File Offset: 0x0067FD41
		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		// Token: 0x1700366F RID: 13935
		// (get) Token: 0x06015C78 RID: 89208 RVA: 0x00681B4E File Offset: 0x0067FD4E
		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		// Token: 0x17003670 RID: 13936
		// (get) Token: 0x06015C79 RID: 89209 RVA: 0x00681B5B File Offset: 0x0067FD5B
		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		// Token: 0x17003671 RID: 13937
		// (get) Token: 0x06015C7A RID: 89210 RVA: 0x00681B68 File Offset: 0x0067FD68
		public int PlayerControlledCaravansCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.caravans.Count; i++)
				{
					if (this.caravans[i].IsPlayerControlled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06015C7B RID: 89211 RVA: 0x00681BA5 File Offset: 0x0067FDA5
		public List<WorldObject> AllWorldObjectsOnLayer(PlanetLayer layer)
		{
			return this.layerWorldObjects.GetValueOrDefault(layer, WorldObjectsHolder.EmptyWorldObjectList);
		}

		// Token: 0x06015C7C RID: 89212 RVA: 0x00681BB8 File Offset: 0x0067FDB8
		public List<WorldObject> AllSettlementsOnLayer(PlanetLayer layer)
		{
			List<WorldObject> list;
			if (!this.layerWorldObjects.TryGetValue(layer, out list))
			{
				return WorldObjectsHolder.EmptyWorldObjectList;
			}
			WorldObjectsHolder.tmpSettlements.Clear();
			foreach (WorldObject worldObject in list)
			{
				if (worldObject is Settlement)
				{
					WorldObjectsHolder.tmpSettlements.Add(worldObject);
				}
			}
			return WorldObjectsHolder.tmpSettlements;
		}

		// Token: 0x06015C7D RID: 89213 RVA: 0x00681C38 File Offset: 0x0067FE38
		public bool AnyFactionSettlementOnLayer(Faction faction, PlanetLayer layer)
		{
			List<WorldObject> list;
			if (!this.layerWorldObjects.TryGetValue(layer, out list))
			{
				return false;
			}
			foreach (WorldObject worldObject in list)
			{
				Settlement settlement = worldObject as Settlement;
				if (settlement != null && settlement.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015C7E RID: 89214 RVA: 0x00681CAC File Offset: 0x0067FEAC
		public bool AnyFactionSettlementOnRootSurface(Faction faction)
		{
			return this.AnyFactionSettlementOnLayer(faction, Find.WorldGrid.Surface);
		}

		// Token: 0x06015C7F RID: 89215 RVA: 0x00681CC0 File Offset: 0x0067FEC0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
				for (int i = this.worldObjects.Count - 1; i >= 0; i--)
				{
					if (!this.worldObjects[i].def.saved)
					{
						WorldObjectsHolder.tmpUnsavedWorldObjects.Add(this.worldObjects[i]);
						this.worldObjects.RemoveAt(i);
					}
				}
			}
			Scribe_Collections.Look<WorldObject>(ref this.worldObjects, "worldObjects", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.worldObjects.AddRange(WorldObjectsHolder.tmpUnsavedWorldObjects);
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.worldObjects.RemoveAll((WorldObject wo) => wo == null);
				this.Recache();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.worldObjects.RemoveAll((WorldObject wo) => wo == null || wo.def == null) != 0)
				{
					Log.Error("Some WorldObjects had null def after loading.");
				}
				for (int j = this.worldObjects.Count - 1; j >= 0; j--)
				{
					try
					{
						this.worldObjects[j].SpawnSetup();
					}
					catch (Exception arg)
					{
						Log.Error(string.Format("Exception spawning WorldObject: {0}", arg));
						this.worldObjects.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06015C80 RID: 89216 RVA: 0x00681E40 File Offset: 0x00680040
		public void Add(WorldObject o)
		{
			if (this.worldObjects.Contains(o))
			{
				Log.Error(string.Format("Tried to add world object {0} to world, but it's already here.", o));
				return;
			}
			if (!o.Tile.Valid && !(o is PocketMapParent))
			{
				Log.Error(string.Format("Tried to add world object {0} but its tile is not set. Setting to 0.", o));
				o.Tile = new PlanetTile(0);
			}
			this.worldObjects.Add(o);
			this.AddToCache(o);
			o.SpawnSetup();
			o.PostAdd();
		}

		// Token: 0x06015C81 RID: 89217 RVA: 0x00681EBF File Offset: 0x006800BF
		public void Remove(WorldObject o)
		{
			if (!this.worldObjects.Contains(o))
			{
				Log.Error(string.Format("Tried to remove world object {0} from world, but it's not here.", o));
				return;
			}
			this.worldObjects.Remove(o);
			this.RemoveFromCache(o);
			o.PostRemove();
		}

		// Token: 0x06015C82 RID: 89218 RVA: 0x00681EFC File Offset: 0x006800FC
		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].DoTick();
			}
		}

		// Token: 0x06015C83 RID: 89219 RVA: 0x00681F48 File Offset: 0x00680148
		private void AddToCache(WorldObject o)
		{
			this.worldObjectsHashSet.Add(o);
			if (o.Tile.Valid)
			{
				List<WorldObject> list;
				if (!this.layerWorldObjects.TryGetValue(o.Tile.Layer, out list))
				{
					list = (this.layerWorldObjects[o.Tile.Layer] = new List<WorldObject>());
				}
				list.Add(o);
			}
			Caravan caravan = o as Caravan;
			if (caravan != null)
			{
				this.caravans.Add(caravan);
			}
			Settlement settlement = o as Settlement;
			if (settlement != null)
			{
				this.settlements.Add(settlement);
				this.settlementBases.Add(settlement);
			}
			TravellingTransporters travellingTransporters = o as TravellingTransporters;
			if (travellingTransporters != null)
			{
				this.travellingTransporters.Add(travellingTransporters);
			}
			DestroyedSettlement destroyedSettlement = o as DestroyedSettlement;
			if (destroyedSettlement != null)
			{
				this.destroyedSettlements.Add(destroyedSettlement);
			}
			RoutePlannerWaypoint routePlannerWaypoint = o as RoutePlannerWaypoint;
			if (routePlannerWaypoint != null)
			{
				this.routePlannerWaypoints.Add(routePlannerWaypoint);
			}
			MapParent mapParent = o as MapParent;
			if (mapParent != null)
			{
				this.mapParents.Add(mapParent);
			}
			Site site = o as Site;
			if (site != null)
			{
				this.sites.Add(site);
			}
			PeaceTalks peaceTalks = o as PeaceTalks;
			if (peaceTalks != null)
			{
				this.peaceTalks.Add(peaceTalks);
			}
			ExpandableLandmarksUtility.Notify_WorldObjectsChanged();
		}

		// Token: 0x06015C84 RID: 89220 RVA: 0x0068208C File Offset: 0x0068028C
		private void RemoveFromCache(WorldObject o)
		{
			this.worldObjectsHashSet.Remove(o);
			List<WorldObject> list;
			if (o.Tile.Valid && this.layerWorldObjects.TryGetValue(o.Tile.Layer, out list))
			{
				list.Remove(o);
			}
			Caravan caravan = o as Caravan;
			if (caravan != null)
			{
				this.caravans.Remove(caravan);
			}
			Settlement settlement = o as Settlement;
			if (settlement != null)
			{
				this.settlements.Remove(settlement);
				this.settlementBases.Remove(settlement);
			}
			TravellingTransporters travellingTransporters = o as TravellingTransporters;
			if (travellingTransporters != null)
			{
				this.travellingTransporters.Remove(travellingTransporters);
			}
			DestroyedSettlement destroyedSettlement = o as DestroyedSettlement;
			if (destroyedSettlement != null)
			{
				this.destroyedSettlements.Remove(destroyedSettlement);
			}
			RoutePlannerWaypoint routePlannerWaypoint = o as RoutePlannerWaypoint;
			if (routePlannerWaypoint != null)
			{
				this.routePlannerWaypoints.Remove(routePlannerWaypoint);
			}
			MapParent mapParent = o as MapParent;
			if (mapParent != null)
			{
				this.mapParents.Remove(mapParent);
			}
			Site site = o as Site;
			if (site != null)
			{
				this.sites.Remove(site);
			}
			PeaceTalks peaceTalks = o as PeaceTalks;
			if (peaceTalks != null)
			{
				this.peaceTalks.Remove(peaceTalks);
			}
			ExpandableLandmarksUtility.Notify_WorldObjectsChanged();
		}

		// Token: 0x06015C85 RID: 89221 RVA: 0x006821B8 File Offset: 0x006803B8
		private void Recache()
		{
			this.worldObjectsHashSet.Clear();
			this.layerWorldObjects.Clear();
			this.caravans.Clear();
			this.settlements.Clear();
			this.travellingTransporters.Clear();
			this.settlementBases.Clear();
			this.destroyedSettlements.Clear();
			this.routePlannerWaypoints.Clear();
			this.mapParents.Clear();
			this.sites.Clear();
			this.peaceTalks.Clear();
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				this.AddToCache(this.worldObjects[i]);
			}
		}

		// Token: 0x06015C86 RID: 89222 RVA: 0x00682266 File Offset: 0x00680466
		public bool Contains(WorldObject o)
		{
			return o != null && this.worldObjectsHashSet.Contains(o);
		}

		// Token: 0x06015C87 RID: 89223 RVA: 0x00682279 File Offset: 0x00680479
		public IEnumerable<WorldObject> ObjectsAt(PlanetTile tileID)
		{
			if (!tileID.Valid)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.worldObjects.Count; i = num + 1)
			{
				if (this.worldObjects[i].Tile == tileID)
				{
					yield return this.worldObjects[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06015C88 RID: 89224 RVA: 0x00682290 File Offset: 0x00680490
		public bool AnyWorldObjectOnLayer(PlanetLayer layer)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile.Layer == layer)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015C89 RID: 89225 RVA: 0x006822D4 File Offset: 0x006804D4
		public bool AnyWorldObjectAt(PlanetTile tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015C8A RID: 89226 RVA: 0x00682313 File Offset: 0x00680513
		public bool AnyWorldObjectAt<T>(PlanetTile tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		// Token: 0x06015C8B RID: 89227 RVA: 0x00682324 File Offset: 0x00680524
		public T WorldObjectAt<T>(PlanetTile tile) where T : WorldObject
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i] is T)
				{
					return this.worldObjects[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06015C8C RID: 89228 RVA: 0x00682394 File Offset: 0x00680594
		public bool TryGetWorldObjectAt<T>(PlanetTile tile, out T wo) where T : WorldObject
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i] is T)
				{
					wo = (this.worldObjects[i] as T);
					return true;
				}
			}
			wo = default(T);
			return false;
		}

		// Token: 0x06015C8D RID: 89229 RVA: 0x00682409 File Offset: 0x00680609
		public bool AnyWorldObjectAt(PlanetTile tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		// Token: 0x06015C8E RID: 89230 RVA: 0x00682418 File Offset: 0x00680618
		public WorldObject WorldObjectAt(PlanetTile tile, WorldObjectDef def)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i].def == def)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x06015C8F RID: 89231 RVA: 0x00682476 File Offset: 0x00680676
		public bool AnySettlementAt(PlanetTile tile)
		{
			return this.SettlementAt(tile) != null;
		}

		// Token: 0x06015C90 RID: 89232 RVA: 0x00682484 File Offset: 0x00680684
		public Settlement SettlementAt(PlanetTile tile)
		{
			for (int i = 0; i < this.settlements.Count; i++)
			{
				if (this.settlements[i].Tile == tile)
				{
					return this.settlements[i];
				}
			}
			return null;
		}

		// Token: 0x06015C91 RID: 89233 RVA: 0x006824CE File Offset: 0x006806CE
		public bool AnySettlementBaseAt(PlanetTile tile)
		{
			return this.SettlementBaseAt(tile) != null;
		}

		// Token: 0x06015C92 RID: 89234 RVA: 0x006824DC File Offset: 0x006806DC
		public Settlement SettlementBaseAt(PlanetTile tile)
		{
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (this.settlementBases[i].Tile == tile)
				{
					return this.settlementBases[i];
				}
			}
			return null;
		}

		// Token: 0x06015C93 RID: 89235 RVA: 0x00682526 File Offset: 0x00680726
		public bool AnySiteAt(PlanetTile tile)
		{
			return this.SiteAt(tile) != null;
		}

		// Token: 0x06015C94 RID: 89236 RVA: 0x00682534 File Offset: 0x00680734
		public Site SiteAt(PlanetTile tile)
		{
			for (int i = 0; i < this.sites.Count; i++)
			{
				if (this.sites[i].Tile == tile)
				{
					return this.sites[i];
				}
			}
			return null;
		}

		// Token: 0x06015C95 RID: 89237 RVA: 0x0068257E File Offset: 0x0068077E
		public bool AnyDestroyedSettlementAt(PlanetTile tile)
		{
			return this.DestroyedSettlementAt(tile) != null;
		}

		// Token: 0x06015C96 RID: 89238 RVA: 0x0068258C File Offset: 0x0068078C
		public DestroyedSettlement DestroyedSettlementAt(PlanetTile tile)
		{
			for (int i = 0; i < this.destroyedSettlements.Count; i++)
			{
				if (this.destroyedSettlements[i].Tile == tile)
				{
					return this.destroyedSettlements[i];
				}
			}
			return null;
		}

		// Token: 0x06015C97 RID: 89239 RVA: 0x006825D6 File Offset: 0x006807D6
		public bool AnyMapParentAt(PlanetTile tile)
		{
			return this.MapParentAt(tile) != null;
		}

		// Token: 0x06015C98 RID: 89240 RVA: 0x006825E4 File Offset: 0x006807E4
		public MapParent MapParentAt(PlanetTile tile)
		{
			for (int i = 0; i < this.mapParents.Count; i++)
			{
				if (this.mapParents[i].Tile == tile)
				{
					return this.mapParents[i];
				}
			}
			return null;
		}

		// Token: 0x06015C99 RID: 89241 RVA: 0x0068262E File Offset: 0x0068082E
		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, PlanetTile tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		// Token: 0x06015C9A RID: 89242 RVA: 0x0068263C File Offset: 0x0068083C
		public WorldObject WorldObjectOfDefAt(WorldObjectDef def, PlanetTile tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].def == def && this.worldObjects[i].Tile == tile)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		// Token: 0x06015C9B RID: 89243 RVA: 0x0068269C File Offset: 0x0068089C
		public bool AnyGeneratedWorldLocationAt(PlanetTile tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].isGeneratedLocation && this.worldObjects[i].Tile == tile)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015C9C RID: 89244 RVA: 0x006826F0 File Offset: 0x006808F0
		public Caravan PlayerControlledCaravanAt(PlanetTile tile)
		{
			for (int i = 0; i < this.caravans.Count; i++)
			{
				if (this.caravans[i].Tile == tile && this.caravans[i].IsPlayerControlled)
				{
					return this.caravans[i];
				}
			}
			return null;
		}

		// Token: 0x06015C9D RID: 89245 RVA: 0x00682750 File Offset: 0x00680950
		public bool AnySettlementBaseAtOrAdjacent(PlanetTile tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				Settlement settlement = this.settlementBases[i];
				if (settlement.Tile.Layer == tile.Layer && worldGrid.IsNeighborOrSame(settlement.Tile, tile))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015C9E RID: 89246 RVA: 0x006827B0 File Offset: 0x006809B0
		public bool AnySettlementBaseAtOrAdjacent(PlanetTile tile, out WorldObject wo)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				Settlement settlement = this.settlementBases[i];
				if (settlement.Tile.Layer == tile.Layer && worldGrid.IsNeighborOrSame(settlement.Tile, tile))
				{
					wo = settlement;
					return true;
				}
			}
			wo = null;
			return false;
		}

		// Token: 0x06015C9F RID: 89247 RVA: 0x00682818 File Offset: 0x00680A18
		public RoutePlannerWaypoint RoutePlannerWaypointAt(PlanetTile tile)
		{
			for (int i = 0; i < this.routePlannerWaypoints.Count; i++)
			{
				if (this.routePlannerWaypoints[i].Tile == tile)
				{
					return this.routePlannerWaypoints[i];
				}
			}
			return null;
		}

		// Token: 0x06015CA0 RID: 89248 RVA: 0x00682864 File Offset: 0x00680A64
		public void GetPlayerControlledCaravansAt(PlanetTile tile, List<Caravan> outCaravans)
		{
			outCaravans.Clear();
			for (int i = 0; i < this.caravans.Count; i++)
			{
				Caravan caravan = this.caravans[i];
				if (caravan.Tile == tile && caravan.IsPlayerControlled)
				{
					outCaravans.Add(caravan);
				}
			}
		}

		// Token: 0x0400F17B RID: 61819
		private List<WorldObject> worldObjects = new List<WorldObject>();

		// Token: 0x0400F17C RID: 61820
		private readonly Dictionary<PlanetLayer, List<WorldObject>> layerWorldObjects = new Dictionary<PlanetLayer, List<WorldObject>>();

		// Token: 0x0400F17D RID: 61821
		private readonly HashSet<WorldObject> worldObjectsHashSet = new HashSet<WorldObject>();

		// Token: 0x0400F17E RID: 61822
		private readonly List<Caravan> caravans = new List<Caravan>();

		// Token: 0x0400F17F RID: 61823
		private readonly List<Settlement> settlements = new List<Settlement>();

		// Token: 0x0400F180 RID: 61824
		private readonly List<TravellingTransporters> travellingTransporters = new List<TravellingTransporters>();

		// Token: 0x0400F181 RID: 61825
		private readonly List<Settlement> settlementBases = new List<Settlement>();

		// Token: 0x0400F182 RID: 61826
		private readonly List<DestroyedSettlement> destroyedSettlements = new List<DestroyedSettlement>();

		// Token: 0x0400F183 RID: 61827
		private readonly List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x0400F184 RID: 61828
		private readonly List<MapParent> mapParents = new List<MapParent>();

		// Token: 0x0400F185 RID: 61829
		private readonly List<Site> sites = new List<Site>();

		// Token: 0x0400F186 RID: 61830
		private readonly List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		// Token: 0x0400F187 RID: 61831
		private static readonly List<WorldObject> EmptyWorldObjectList = new List<WorldObject>();

		// Token: 0x0400F188 RID: 61832
		private static readonly List<WorldObject> tmpSettlements = new List<WorldObject>();

		// Token: 0x0400F189 RID: 61833
		private List<PlanetTile> tmpReservedTiles = new List<PlanetTile>();

		// Token: 0x0400F18A RID: 61834
		private List<ILoadReferenceable> tmpTileReservers = new List<ILoadReferenceable>();

		// Token: 0x0400F18B RID: 61835
		private static readonly List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		// Token: 0x0400F18C RID: 61836
		private static readonly List<WorldObject> tmpWorldObjects = new List<WorldObject>();
	}
}
