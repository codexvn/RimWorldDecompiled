using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020005C5 RID: 1477
	public class PathFinderMapData : IDisposable
	{
		// Token: 0x060025EC RID: 9708 RVA: 0x000C79C4 File Offset: 0x000C5BC4
		public PathFinderMapData(Map map)
		{
			this.map = map;
			this.cellDeltas.Capacity = map.cellIndices.NumGridCells;
			this.cellDeltaSet.EnsureCapacity(map.cellIndices.NumGridCells);
			this.normalCost = new CostSource(map, map.pathing.Normal);
			this.fenceBlockedCost = new CostSource(map, map.pathing.FenceBlocked);
			this.flyingCost = new CostSource(map, map.pathing.Flying);
			this.areas = new AreaSource(map);
			this.perceptual = new PerceptualSource(map);
			this.connectivity = new ConnectivitySource(map);
			this.water = new WaterSource(map);
			this.fences = new FenceSource(map);
			this.buildings = new BuildingSource(map);
			this.factions = new FactionSource(map);
			this.fogged = new FogSource(map);
			this.persistentDanger = new PersistentDangerSource(map);
			this.darknessDanger = new DarknessSource(map);
			this.sources = new List<IPathFinderDataSource>
			{
				this.normalCost,
				this.fenceBlockedCost,
				this.flyingCost,
				this.areas,
				this.perceptual,
				this.connectivity,
				this.water,
				this.fences,
				this.buildings,
				this.factions,
				this.fogged,
				this.persistentDanger,
				this.darknessDanger
			};
			map.events.BuildingSpawned += this.Notify_BuildingChanged;
			map.events.BuildingDespawned += this.Notify_BuildingChanged;
			map.events.BuildingHitPointsChanged += this.Notify_BuildingChanged;
			map.events.ThingSpawned += this.Notify_ThingSpawnedDespawned;
			map.events.ThingDespawned += this.Notify_ThingSpawnedDespawned;
			map.events.ReservationAdded += this.Notify_Reservation;
			map.events.ReservationRemoved += this.Notify_Reservation;
			map.events.HaulEnrouteAdded += this.Notify_HaulEnroute;
			map.events.HaulEnrouteReleased += this.Notify_HaulReleased;
			map.events.FactionRemoved += this.Notify_FactionRemoved;
			map.events.TerrainChanged += this.Notify_CellDelta;
			map.events.PathCostRecalculate += this.Notify_CellDelta;
			map.events.CellFogChanged += this.Event_FogChanged;
			map.events.MapFogged += this.Notify_MapDirtied;
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000C7D05 File Offset: 0x000C5F05
		private void Notify_HaulEnroute(Thing enroute, Pawn pawn, ThingDef stuff, int count)
		{
			this.Notify_CellDelta(enroute.OccupiedRect());
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000C7D05 File Offset: 0x000C5F05
		private void Notify_HaulReleased(Thing enroute, Pawn pawn)
		{
			this.Notify_CellDelta(enroute.OccupiedRect());
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000C7D14 File Offset: 0x000C5F14
		private void Notify_ThingSpawnedDespawned(Thing thing)
		{
			if (!thing.def.pathfinderDangerous)
			{
				return;
			}
			AttachableThing attachableThing = thing as AttachableThing;
			if (attachableThing != null && attachableThing.parent is Pawn)
			{
				return;
			}
			if (thing.def.size.Area == 1)
			{
				this.Notify_CellDelta(thing.Position);
				return;
			}
			this.Notify_CellDelta(thing.OccupiedRect());
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000C7D74 File Offset: 0x000C5F74
		public void Dispose()
		{
			foreach (IPathFinderDataSource pathFinderDataSource in this.sources)
			{
				pathFinderDataSource.Dispose();
			}
			this.emptyByteGrid.Dispose();
			this.emptyBoolGrid.Dispose();
			this.emptyUShortGrid.Dispose();
			this.emptyBitGrid.Dispose();
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000C7DF0 File Offset: 0x000C5FF0
		public CellConnection CellConnectionsAt(int index)
		{
			return this.connectivity.Data[index];
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000C7E14 File Offset: 0x000C6014
		private void Notify_Reservation(ReservationManager.Reservation reservation)
		{
			if (reservation.Target.HasThing)
			{
				this.Notify_CellDelta(reservation.Target.Thing.OccupiedRect());
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000C7E4C File Offset: 0x000C604C
		public void Notify_CellDelta(IntVec3 cell)
		{
			if (cell.InBounds(this.map))
			{
				if (this.cellDeltaSet.Add(cell))
				{
					this.cellDeltas.Add(cell);
					return;
				}
			}
			else
			{
				Log.Warning(string.Format("{0} was notified using an out-of-bounds cell: {1}", this, cell));
			}
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000C7E98 File Offset: 0x000C6098
		private void Notify_MapDirtied()
		{
			this.lastGatherTick = -1;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000C7D05 File Offset: 0x000C5F05
		private void Notify_BuildingChanged(Building building)
		{
			this.Notify_CellDelta(building.OccupiedRect());
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000C7EA1 File Offset: 0x000C60A1
		private void Event_FogChanged(IntVec3 cell, bool _)
		{
			this.Notify_CellDelta(cell);
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000C7EAA File Offset: 0x000C60AA
		public void Notify_CellDelta(CellRect rect)
		{
			this.cellRectDeltas.Add(rect);
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000C7EB8 File Offset: 0x000C60B8
		public void Notify_AreaDelta(Area area, IntVec3 cell)
		{
			this.areas.Notify_AreaDelta(area, cell);
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000C7EC7 File Offset: 0x000C60C7
		public void Notify_FactionRemoved(Faction faction)
		{
			this.factions.Notify_Removed(faction);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000C7ED8 File Offset: 0x000C60D8
		public bool GatherData(IEnumerable<PathRequest> requests)
		{
			bool result;
			using (ProfilerBlock.Scope("PathFinderMapData.GatherData"))
			{
				if (this.lastGatherTick == GenTicks.TicksGame)
				{
					result = false;
				}
				else
				{
					foreach (CellRect cellRect in this.cellRectDeltas)
					{
						foreach (IntVec3 intVec in cellRect.Cells)
						{
							if (intVec.InBounds(this.map) && this.cellDeltaSet.Add(intVec))
							{
								this.cellDeltas.Add(intVec);
							}
						}
					}
					bool anyChanged = this.cellDeltas.Any<IntVec3>();
					this.cellRectDeltas.Clear();
					if (this.lastGatherTick >= 0)
					{
						using (ProfilerBlock.Scope("incremental"))
						{
							Parallel.ForEach<IPathFinderDataSource>(this.sources, delegate(IPathFinderDataSource source)
							{
								if (source.UpdateIncrementally(requests, this.cellDeltas))
								{
									anyChanged = true;
								}
							});
							goto IL_152;
						}
					}
					anyChanged = true;
					using (ProfilerBlock.Scope("recompute"))
					{
						Parallel.ForEach<IPathFinderDataSource>(this.sources, delegate(IPathFinderDataSource source)
						{
							source.ComputeAll(requests);
						});
					}
					IL_152:
					this.cellDeltas.Clear();
					this.cellDeltaSet.Clear();
					this.lastGatherTick = GenTicks.TicksGame;
					result = anyChanged;
				}
			}
			return result;
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000C80EC File Offset: 0x000C62EC
		public void ParameterizePathJob(ref PathFinderJob job)
		{
			job.connectivity = this.connectivity.Data;
			job.fences = this.fences.Data;
			job.buildings = this.buildings.Buildings;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000C8124 File Offset: 0x000C6324
		public void ParameterizeGridJob(PathRequest request, ref PathFinder.MapGridRequest query, ref PathGridJob job, ref PathFinder.GridJobOutput output)
		{
			Faction requesterFaction = request.RequesterFaction;
			Lord requesterLord = request.RequesterLord;
			job.traverseParams = query.traverseParams;
			job.tuning = query.tuning;
			job.indicies = this.map.cellIndices;
			job.grid = output.grid;
			job.pathGridDirect = (request.TraverseParms.fenceBlocked ? this.fenceBlockedCost.Data : this.normalCost.Data);
			job.building = this.buildings.Buildings;
			job.buildingDestroyable = this.buildings.Destroyable;
			job.player = this.buildings.Player;
			job.water = this.water.Data;
			job.fence = this.fences.Data;
			job.factionCosts = this.factions[requesterFaction];
			job.buildingHitPoints = this.buildings.Hitpoints;
			job.fogged = this.fogged.Data;
			job.darknessDanger = this.darknessDanger.Data;
			job.persistentDanger = this.persistentDanger.Data;
			Pawn pawn = request.pawn;
			if (pawn != null && pawn.Drafted)
			{
				job.perceptualCost = this.perceptual.CostDrafted;
			}
			else
			{
				job.perceptualCost = this.perceptual.CostUndrafted;
			}
			if (requesterLord != null)
			{
				NativeBitArray walkGrid = requesterLord.LordJob.GetWalkGrid(request.pawn);
				job.lordGrid = (walkGrid.IsCreated ? walkGrid.AsReadOnly() : this.emptyBitGrid.AsReadOnly());
			}
			else
			{
				job.lordGrid = this.emptyBitGrid.AsReadOnly();
			}
			job.allowedGrid = ((request.area != null) ? this.areas.DataForArea(request.area).AsReadOnly() : this.emptyBitGrid.AsReadOnly());
			job.custom = ((request.customizer != null) ? request.customizer.GetOffsetGrid().AsReadOnly() : this.emptyUShortGrid.AsReadOnly());
			AvoidGrid avoidGrid = query.avoidGrid;
			job.avoidGrid = ((avoidGrid != null) ? avoidGrid.Grid : this.emptyByteGrid.AsReadOnly());
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000C8354 File Offset: 0x000C6554
		public void LogCell(IntVec3 cell)
		{
			int num = this.map.cellIndices.CellToIndex(cell);
			string text = string.Concat(new string[]
			{
				string.Format("Cell: {0}\n", cell),
				string.Format("Cost: {0} (fenced: {1}, flying: {2})\n", this.normalCost.Data[num], this.fenceBlockedCost.Data[num], this.flyingCost.Data[num]),
				string.Format("Connections: {0}\n", this.CellConnectionsAt(num)),
				string.Format("Building: {0}\n", this.buildings.Buildings.IsSet(num)),
				string.Format("Player: {0}\n", this.buildings.Player.IsSet(num)),
				string.Format("Fence: {0}", this.fences.Data.IsSet(num))
			});
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (this.factions.HasFactionData(faction))
				{
					text += string.Format("\n    [{0}]: {1}", faction.def.defName, this.factions[faction][num]);
				}
			}
			Log.Message(text);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000C8508 File Offset: 0x000C6708
		public void RegisterSource(IPathFinderDataSource source)
		{
			if (this.sources.Contains(source))
			{
				throw new InvalidOperationException(string.Format("Source {0} is already registered in {1}", source, this));
			}
			this.sources.Add(source);
		}

		// Token: 0x04001F56 RID: 8022
		private readonly Map map;

		// Token: 0x04001F57 RID: 8023
		private readonly CostSource normalCost;

		// Token: 0x04001F58 RID: 8024
		private readonly CostSource fenceBlockedCost;

		// Token: 0x04001F59 RID: 8025
		private readonly CostSource flyingCost;

		// Token: 0x04001F5A RID: 8026
		private readonly AreaSource areas;

		// Token: 0x04001F5B RID: 8027
		private readonly PerceptualSource perceptual;

		// Token: 0x04001F5C RID: 8028
		private readonly ConnectivitySource connectivity;

		// Token: 0x04001F5D RID: 8029
		private readonly WaterSource water;

		// Token: 0x04001F5E RID: 8030
		private readonly FenceSource fences;

		// Token: 0x04001F5F RID: 8031
		private readonly BuildingSource buildings;

		// Token: 0x04001F60 RID: 8032
		private readonly FactionSource factions;

		// Token: 0x04001F61 RID: 8033
		private readonly FogSource fogged;

		// Token: 0x04001F62 RID: 8034
		private readonly PersistentDangerSource persistentDanger;

		// Token: 0x04001F63 RID: 8035
		private readonly DarknessSource darknessDanger;

		// Token: 0x04001F64 RID: 8036
		private readonly List<IPathFinderDataSource> sources;

		// Token: 0x04001F65 RID: 8037
		private int lastGatherTick = -1;

		// Token: 0x04001F66 RID: 8038
		private readonly List<IntVec3> cellDeltas = new List<IntVec3>();

		// Token: 0x04001F67 RID: 8039
		private readonly HashSet<IntVec3> cellDeltaSet = new HashSet<IntVec3>();

		// Token: 0x04001F68 RID: 8040
		private readonly List<CellRect> cellRectDeltas = new List<CellRect>();

		// Token: 0x04001F69 RID: 8041
		private NativeArray<byte> emptyByteGrid = NativeArrayUtility.EmptyArray<byte>();

		// Token: 0x04001F6A RID: 8042
		private NativeArray<bool> emptyBoolGrid = NativeArrayUtility.EmptyArray<bool>();

		// Token: 0x04001F6B RID: 8043
		private NativeArray<ushort> emptyUShortGrid = NativeArrayUtility.EmptyArray<ushort>();

		// Token: 0x04001F6C RID: 8044
		private NativeBitArray emptyBitGrid = NativeArrayUtility.EmptyBitArray();
	}
}
