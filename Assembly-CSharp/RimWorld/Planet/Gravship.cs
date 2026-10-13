using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C4A RID: 15434
	public class Gravship : WorldObject, IIncidentTarget, ILoadReferenceable
	{
		// Token: 0x170033E3 RID: 13283
		// (get) Token: 0x06014F3D RID: 85821 RVA: 0x0064595F File Offset: 0x00643B5F
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x170033E4 RID: 13284
		// (get) Token: 0x06014F3E RID: 85822 RVA: 0x00645971 File Offset: 0x00643B71
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x170033E5 RID: 13285
		// (get) Token: 0x06014F3F RID: 85823 RVA: 0x00645983 File Offset: 0x00643B83
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x170033E6 RID: 13286
		// (get) Token: 0x06014F40 RID: 85824 RVA: 0x0064599C File Offset: 0x00643B9C
		public IEnumerable<Pawn> Pawns
		{
			get
			{
				return this.pawns.Keys;
			}
		}

		// Token: 0x170033E7 RID: 13287
		// (get) Token: 0x06014F41 RID: 85825 RVA: 0x006459A9 File Offset: 0x00643BA9
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.things.Keys;
			}
		}

		// Token: 0x170033E8 RID: 13288
		// (get) Token: 0x06014F42 RID: 85826 RVA: 0x006459B6 File Offset: 0x00643BB6
		public override string Label
		{
			get
			{
				if (!this.engine.nameHidden)
				{
					return this.engine.RenamableLabel;
				}
				return base.Label;
			}
		}

		// Token: 0x170033E9 RID: 13289
		// (get) Token: 0x06014F43 RID: 85827 RVA: 0x006459D8 File Offset: 0x00643BD8
		public override Material Material
		{
			get
			{
				Material result;
				if ((result = this.cachedMat) == null)
				{
					result = (this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, 3550));
				}
				return result;
			}
		}

		// Token: 0x170033EA RID: 13290
		// (get) Token: 0x06014F44 RID: 85828 RVA: 0x00645A1D File Offset: 0x00643C1D
		public override Color ExpandingIconColor
		{
			get
			{
				Faction faction = base.Faction;
				if (faction == null)
				{
					return Color.white;
				}
				return faction.Color;
			}
		}

		// Token: 0x170033EB RID: 13291
		// (get) Token: 0x06014F45 RID: 85829 RVA: 0x00645A34 File Offset: 0x00643C34
		// (set) Token: 0x06014F46 RID: 85830 RVA: 0x00645A3C File Offset: 0x00643C3C
		public Rot4 Rotation
		{
			get
			{
				return this.rotation;
			}
			set
			{
				this.rotation = value;
			}
		}

		// Token: 0x170033EC RID: 13292
		// (get) Token: 0x06014F47 RID: 85831 RVA: 0x00645A48 File Offset: 0x00643C48
		private float TraveledPctStepPerTick
		{
			get
			{
				Vector3 start = this.Start;
				Vector3 end = this.End;
				if (start == end)
				{
					return 1f;
				}
				float num = GenMath.SphericalDistance(start.normalized, end.normalized);
				if (num == 0f)
				{
					return 1f;
				}
				return 0.00025f / num;
			}
		}

		// Token: 0x06014F48 RID: 85832 RVA: 0x00645A9C File Offset: 0x00643C9C
		private Gravship()
		{
			this.things = new Dictionary<Thing, PositionData>();
			this.pawns = new Dictionary<Pawn, PositionData>();
			this.powerOn = new Dictionary<Thing, bool>();
			this.connectParents = new Dictionary<Thing, Thing>();
			this.roofs = new Dictionary<IntVec3, RoofDef>();
			this.foundations = new Dictionary<IntVec3, TerrainDef>();
			this.terrains = new Dictionary<IntVec3, TerrainDef>();
			this.terrainColors = new Dictionary<IntVec3, ColorDef>();
			this.terrainDesignations = new Dictionary<IntVec3, Designation>();
			this.gases = new Dictionary<IntVec3, uint>();
			this.areas = new MoveableAreas();
			this.bounds = CellRect.Empty;
			this.occupiedRects = new List<CellRect>();
			this.roomTemperatures = new List<RoomTemperatureVacuum>();
			this.thrusters = new Dictionary<Thing, PositionData>();
			this.exteriorDoors = new Dictionary<Thing, PositionData>();
			this.gravFieldExtenderPositions = new List<IntVec3>();
		}

		// Token: 0x06014F49 RID: 85833 RVA: 0x00645BE4 File Offset: 0x00643DE4
		public Gravship(Building_GravEngine engine) : this()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.engine = engine;
			Map map = engine.Map;
			this.originalPosition = engine.Position;
			engine.ForceSubstructureDirty();
			HashSet<IntVec3> validSubstructure = engine.ValidSubstructure;
			this.storyState = new StoryState(this);
			map.storyState.CopyTo(this.storyState);
			this.autoSlaughterConfigs = map.autoSlaughterManager.configs;
			this.TransferZones(map, this.originalPosition, validSubstructure);
			HashSet<IntVec3> occupiedCells;
			this.CopyCellContents(map, this.originalPosition, validSubstructure, out occupiedCells);
			this.CopyAreas(map, this.originalPosition, validSubstructure);
			this.CopyStorageGroups(map);
			this.UpdateBoundingBoxes(occupiedCells);
			this.DetermineLaunchDirection();
			this.CheckAffectedGoodwill();
			this.engineToCenter = CellRect.FromCellList(engine.ValidSubstructure).CenterVector3 - this.originalPosition.ToVector3();
		}

		// Token: 0x06014F4A RID: 85834 RVA: 0x00645CC4 File Offset: 0x00643EC4
		public override void ExposeData()
		{
			base.ExposeData();
			Dictionary<IntVec3, RoofDef> dictionary = this.roofs;
			if (dictionary != null)
			{
				dictionary.RemoveAll((KeyValuePair<IntVec3, RoofDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, TerrainDef> dictionary2 = this.foundations;
			if (dictionary2 != null)
			{
				dictionary2.RemoveAll((KeyValuePair<IntVec3, TerrainDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, TerrainDef> dictionary3 = this.terrains;
			if (dictionary3 != null)
			{
				dictionary3.RemoveAll((KeyValuePair<IntVec3, TerrainDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, ColorDef> dictionary4 = this.terrainColors;
			if (dictionary4 != null)
			{
				dictionary4.RemoveAll((KeyValuePair<IntVec3, ColorDef> x) => x.Value == null);
			}
			Dictionary<IntVec3, Designation> dictionary5 = this.terrainDesignations;
			if (dictionary5 != null)
			{
				dictionary5.RemoveAll((KeyValuePair<IntVec3, Designation> x) => x.Value == null);
			}
			Scribe_References.Look<Building_GravEngine>(ref this.engine, "engine", false);
			Scribe_Collections.Look<Thing, PositionData>(ref this.things, "things", LookMode.Deep, LookMode.Deep);
			Scribe_Collections.Look<Pawn, PositionData>(ref this.pawns, "pawns", LookMode.Deep, LookMode.Deep);
			Scribe_Collections.Look<Thing, bool>(ref this.powerOn, "powerOn", LookMode.Reference, LookMode.Value, ref this.tmpPoweredThings, ref this.tmpPoweredThingsOn, true, false, false);
			Scribe_Collections.Look<Thing, Thing>(ref this.connectParents, "connectParents", LookMode.Reference, LookMode.Reference, ref this.tmpConnectedThings, ref this.tmpConnectedParents, true, false, false);
			Scribe_Collections.Look<IntVec3, RoofDef>(ref this.roofs, "roofs", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, TerrainDef>(ref this.foundations, "foundations", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, TerrainDef>(ref this.terrains, "terrains", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, ColorDef>(ref this.terrainColors, "terrainColors", LookMode.Value, LookMode.Def);
			Scribe_Collections.Look<IntVec3, uint>(ref this.gases, "gases", LookMode.Value, LookMode.Value);
			Scribe_Collections.Look<IntVec3, Designation>(ref this.terrainDesignations, "terrainDesignations", LookMode.Value, LookMode.Deep);
			Scribe_Deep.Look<MoveableAreas>(ref this.areas, "areas", Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.engineToCenter, "engineToCenter", default(Vector3), false);
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", default(Rot4), false);
			Scribe_Values.Look<CellRect>(ref this.bounds, "bounds", default(CellRect), false);
			Scribe_Collections.Look<CellRect>(ref this.occupiedRects, "occupiedRects", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<RoomTemperatureVacuum>(ref this.roomTemperatures, "roomTemperatures", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Thing, PositionData>(ref this.thrusters, "thrusters", LookMode.Reference, LookMode.Deep, ref this.tmpThrusters, ref this.tmpThrusterPositionData, true, false, false);
			Scribe_Collections.Look<Thing, PositionData>(ref this.exteriorDoors, "exteriorDoors", LookMode.Reference, LookMode.Deep, ref this.tmpExteriorDoors, ref this.tmpExteriorDoorPositionData, true, false, false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<PlanetTile>(ref this.initialTile, "initialTile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
			Scribe_Values.Look<bool>(ref this.arrived, "arrived", false, false);
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Collections.Look<AutoSlaughterConfig>(ref this.autoSlaughterConfigs, "autoSlaughterConfigs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.launchDirection, "launchDirection", default(IntVec3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (base.Spawned)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						CameraJumper.TryJump(this, CameraJumper.MovementMode.Pan);
					});
				}
				Current.Game.Gravship = this;
				if (this.connectParents == null)
				{
					this.connectParents = new Dictionary<Thing, Thing>();
				}
			}
		}

		// Token: 0x06014F4B RID: 85835 RVA: 0x00646066 File Offset: 0x00644266
		public bool ContainsThing(Thing thing)
		{
			return this.things.ContainsKey(thing);
		}

		// Token: 0x06014F4C RID: 85836 RVA: 0x00646074 File Offset: 0x00644274
		public bool ContainsPawn(Pawn pawn)
		{
			return this.pawns.ContainsKey(pawn);
		}

		// Token: 0x06014F4D RID: 85837 RVA: 0x00646084 File Offset: 0x00644284
		private void DetermineLaunchDirection()
		{
			if (this.launchDirection == IntVec3.Zero)
			{
				this.launchDirection = this.pilotConsole.Rotation.AsIntVec3;
			}
		}

		// Token: 0x06014F4E RID: 85838 RVA: 0x006460BC File Offset: 0x006442BC
		private void TransferZones(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
		{
			for (int i = oldMap.zoneManager.AllZones.Count - 1; i >= 0; i--)
			{
				Zone zone = oldMap.zoneManager.AllZones[i];
				Zone_Stockpile zone_Stockpile = zone as Zone_Stockpile;
				if (zone_Stockpile != null)
				{
					MoveableStockpile moveableStockpile = null;
					foreach (IntVec3 intVec in zone_Stockpile.Cells)
					{
						if (engineFloors.Contains(intVec))
						{
							if (moveableStockpile == null)
							{
								moveableStockpile = new MoveableStockpile(this, zone_Stockpile);
							}
							moveableStockpile.Add(intVec - origin);
						}
					}
					if (moveableStockpile != null)
					{
						this.areas.stockpiles.Add(moveableStockpile);
						zone.Delete();
					}
				}
				else
				{
					Zone_Growing zone_Growing = zone as Zone_Growing;
					if (zone_Growing != null)
					{
						MoveableGrowZone moveableGrowZone = null;
						foreach (IntVec3 intVec2 in zone_Growing.Cells)
						{
							if (engineFloors.Contains(intVec2))
							{
								if (moveableGrowZone == null)
								{
									moveableGrowZone = new MoveableGrowZone(this, zone_Growing);
								}
								moveableGrowZone.Add(intVec2 - origin);
							}
						}
						if (moveableGrowZone != null)
						{
							this.areas.growZones.Add(moveableGrowZone);
							zone.Delete();
						}
					}
				}
			}
		}

		// Token: 0x06014F4F RID: 85839 RVA: 0x00646220 File Offset: 0x00644420
		private void CopyAreas(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
		{
			using (List<Area>.Enumerator enumerator = oldMap.areaManager.AllAreas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Area area = enumerator.Current;
					if (area is Area_Home)
					{
						this.areas.homeArea = new MoveableArea_Allowed(this, area);
						foreach (IntVec3 intVec in area.ActiveCells)
						{
							if (engineFloors.Contains(intVec))
							{
								this.areas.homeArea.Add(intVec - origin);
							}
						}
						this.areas.homeArea.assignedPawns.AddRange(this.Pawns.Where(delegate(Pawn x)
						{
							Pawn_PlayerSettings playerSettings3 = x.playerSettings;
							return ((playerSettings3 != null) ? playerSettings3.AreaRestrictionInPawnCurrentMap : null) == area;
						}));
						using (List<Pawn>.Enumerator enumerator3 = this.areas.homeArea.assignedPawns.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn = enumerator3.Current;
								Pawn_PlayerSettings playerSettings = pawn.playerSettings;
								if (playerSettings != null)
								{
									playerSettings.Notify_MapRemoved(pawn.Map);
								}
							}
							continue;
						}
					}
					Area_Allowed area_Allowed = area as Area_Allowed;
					if (area_Allowed != null)
					{
						MoveableArea_Allowed moveableArea_Allowed = new MoveableArea_Allowed(this, area_Allowed);
						foreach (IntVec3 intVec2 in area.ActiveCells)
						{
							if (engineFloors.Contains(intVec2))
							{
								moveableArea_Allowed.Add(intVec2 - origin);
							}
						}
						moveableArea_Allowed.assignedPawns.AddRange(this.Pawns.Where(delegate(Pawn x)
						{
							Pawn_PlayerSettings playerSettings3 = x.playerSettings;
							return ((playerSettings3 != null) ? playerSettings3.AreaRestrictionInPawnCurrentMap : null) == area;
						}));
						this.areas.allowedAreas.Add(moveableArea_Allowed);
						using (List<Pawn>.Enumerator enumerator3 = moveableArea_Allowed.assignedPawns.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn2 = enumerator3.Current;
								Pawn_PlayerSettings playerSettings2 = pawn2.playerSettings;
								if (playerSettings2 != null)
								{
									playerSettings2.Notify_MapRemoved(pawn2.Map);
								}
							}
							continue;
						}
					}
					MoveableArea moveableArea = null;
					foreach (IntVec3 intVec3 in area.ActiveCells)
					{
						if (engineFloors.Contains(intVec3))
						{
							if (moveableArea == null)
							{
								moveableArea = new MoveableArea(this, area.Label, area.RenamableLabel, area.Color, area.ID);
							}
							moveableArea.Add(intVec3 - origin);
						}
					}
					if (moveableArea != null)
					{
						if (area is Area_NoRoof)
						{
							this.areas.noRoofArea = moveableArea;
						}
						else if (area is Area_BuildRoof)
						{
							this.areas.buildRoofArea = moveableArea;
						}
						else if (area is Area_SnowOrSandClear)
						{
							this.areas.snowClearArea = moveableArea;
						}
						else if (ModsConfig.BiotechActive && area is Area_PollutionClear)
						{
							this.areas.pollutionClearArea = moveableArea;
						}
					}
				}
			}
		}

		// Token: 0x06014F50 RID: 85840 RVA: 0x006465E4 File Offset: 0x006447E4
		private void CopyStorageGroups(Map oldMap)
		{
			foreach (StorageGroup storageGroup in oldMap.storageGroups.StorageGroupsForReading)
			{
				MoveableStorageGroup moveableStorageGroup = null;
				foreach (IStorageGroupMember storageGroupMember in storageGroup.members)
				{
					Thing thing = storageGroupMember as Thing;
					if (thing != null && this.things.ContainsKey(thing))
					{
						if (moveableStorageGroup == null)
						{
							moveableStorageGroup = new MoveableStorageGroup(storageGroup);
						}
						moveableStorageGroup.members.Add(storageGroupMember);
					}
				}
				if (moveableStorageGroup != null)
				{
					if (moveableStorageGroup.members.Any<IStorageGroupMember>())
					{
						this.areas.storageGroups.Add(moveableStorageGroup);
					}
					foreach (IStorageGroupMember member in moveableStorageGroup.members)
					{
						member.SetStorageGroup(null, false);
					}
				}
			}
		}

		// Token: 0x06014F51 RID: 85841 RVA: 0x00646710 File Offset: 0x00644910
		private void CopyCellContents(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors, out HashSet<IntVec3> outOccupiedCells)
		{
			HashSet<Room> hashSet = new HashSet<Room>();
			outOccupiedCells = new HashSet<IntVec3>();
			foreach (CompGravshipFacility compGravshipFacility in this.engine.GravshipComponents)
			{
				this.AddThing(compGravshipFacility.parent, compGravshipFacility.parent.Position - origin);
			}
			this.AddThing(this.engine, this.engine.Position - origin);
			foreach (IntVec3 intVec in engineFloors)
			{
				IntVec3 intVec2 = intVec - origin;
				List<Thing> list = oldMap.thingGrid.ThingsListAt(intVec);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					Thing thing = list[i];
					if (Gravship.ShouldBringOnGravship(thing, intVec))
					{
						Building building = thing as Building;
						if (building == null || !building.def.building.isAttachment)
						{
							foreach (Thing thing2 in GenConstruct.GetAttachedBuildings(thing))
							{
								this.AddThing(thing2, intVec2 + thing2.Rotation.Opposite.FacingCell);
							}
							this.AddThing(thing, intVec2);
						}
					}
				}
				outOccupiedCells.Add(intVec2);
				this.foundations[intVec2] = oldMap.terrainGrid.FoundationAt(intVec);
				this.terrains[intVec2] = oldMap.terrainGrid.TerrainAt(intVec);
				this.terrainColors[intVec2] = oldMap.terrainGrid.ColorAt(intVec);
				this.roofs[intVec2] = oldMap.roofGrid.RoofAt(intVec);
				foreach (Designation designation in oldMap.designationManager.AllDesignationsAt(intVec))
				{
					if (designation.def == DesignationDefOf.RemoveFloor || designation.def == DesignationDefOf.PaintFloor || designation.def == DesignationDefOf.RemovePaintFloor)
					{
						this.terrainDesignations[intVec2] = designation;
						oldMap.designationManager.RemoveDesignation(designation);
					}
				}
				this.gases[intVec2] = oldMap.gasGrid.GetDirect(intVec);
				Room room = intVec.GetRoom(oldMap);
				if (room != null && hashSet.Add(room) && !room.UsesOutdoorTemperature)
				{
					this.roomTemperatures.Add(new RoomTemperatureVacuum
					{
						roomCell = intVec2,
						temperature = room.Temperature,
						vacuum = room.Vacuum
					});
				}
			}
		}

		// Token: 0x06014F52 RID: 85842 RVA: 0x00646A60 File Offset: 0x00644C60
		private static bool ShouldBringOnGravship(Thing thing, IntVec3 cell)
		{
			return !(thing is Mote) && !(thing is Skyfaller) && thing.def.bringAlongOnGravship && !(cell != thing.Position) && (thing.def.category != ThingCategory.Ethereal || thing is Blueprint || thing is Frame);
		}

		// Token: 0x06014F53 RID: 85843 RVA: 0x00646AC4 File Offset: 0x00644CC4
		private void AddThing(Thing thing, IntVec3 offset)
		{
			if (thing is Mote || thing is Skyfaller || !thing.def.bringAlongOnGravship)
			{
				return;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (this.pawns.ContainsKey(pawn))
				{
					return;
				}
				this.pawns.Add(pawn, new PositionData(offset, pawn.Rotation, pawn.Drafted));
				Thing thing2;
				if (pawn.carryTracker.CarriedThing != null && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out thing2, null) && !thing2.Destroyed)
				{
					this.AddThing(thing2, offset);
				}
			}
			else
			{
				if (this.things.ContainsKey(thing))
				{
					return;
				}
				if (!this.engine.OnValidSubstructure(thing))
				{
					return;
				}
				this.things.Add(thing, new PositionData(offset, thing.Rotation, false));
			}
			CompPowerTrader compPowerTrader;
			if (thing.TryGetComp(out compPowerTrader))
			{
				this.powerOn.Add(thing, compPowerTrader.PowerOn);
			}
			CompPower compPower;
			if (thing.TryGetComp(out compPower))
			{
				Dictionary<Thing, Thing> dictionary = this.connectParents;
				CompPower connectParent = compPower.connectParent;
				dictionary.Add(thing, (connectParent != null) ? connectParent.parent : null);
			}
			CompGravshipThruster compGravshipThruster;
			if (thing.TryGetComp(out compGravshipThruster) && compGravshipThruster.CanBeActive)
			{
				this.thrusters.Add(thing, new PositionData(offset, thing.Rotation, false));
				this.launchDirection += thing.Rotation.AsIntVec3 * compGravshipThruster.Props.directionInfluence;
			}
			Building_Door building_Door = thing as Building_Door;
			if (building_Door != null)
			{
				foreach (IntVec3 cell in GenAdj.CellsAdjacentCardinal(building_Door))
				{
					if (!this.engine.ValidSubstructureAt(cell))
					{
						this.exteriorDoors.Add(thing, new PositionData(offset, thing.Rotation, false));
						break;
					}
				}
			}
			if (thing.def == ThingDefOf.GravFieldExtender)
			{
				this.gravFieldExtenderPositions.Add(offset);
			}
			if (thing.def == ThingDefOf.PilotConsole)
			{
				this.pilotConsole = (Building)thing;
			}
		}

		// Token: 0x06014F54 RID: 85844 RVA: 0x00646CDC File Offset: 0x00644EDC
		private void UpdateBoundingBoxes(HashSet<IntVec3> occupiedCells)
		{
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			this.bounds = CellRect.FromCellList(occupiedCells);
			for (int i = this.bounds.minX; i <= this.bounds.maxX; i++)
			{
				for (int j = this.bounds.minZ; j <= this.bounds.maxZ; j++)
				{
					IntVec3 item = new IntVec3(i, 0, j);
					if (occupiedCells.Contains(item) && !hashSet.Contains(item))
					{
						int num = 1;
						int num2 = 1;
						int num3 = i + 1;
						while (num3 <= this.bounds.maxX && occupiedCells.Contains(new IntVec3(num3, 0, j)))
						{
							num++;
							num3++;
						}
						bool flag = true;
						while (j + num2 <= this.bounds.maxZ && flag)
						{
							for (int k = 0; k < num; k++)
							{
								if (!occupiedCells.Contains(new IntVec3(i + k, 0, j + num2)) || hashSet.Contains(new IntVec3(i + 1, 0, j + num2)))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								num2++;
							}
						}
						for (int l = 0; l < num; l++)
						{
							for (int m = 0; m < num2; m++)
							{
								hashSet.Add(new IntVec3(i + l, 0, j + m));
							}
						}
						this.occupiedRects.Add(new CellRect(i, j, num, num2));
					}
				}
			}
		}

		// Token: 0x06014F55 RID: 85845 RVA: 0x00646E58 File Offset: 0x00645058
		private void CheckAffectedGoodwill()
		{
			Gravship.tmpFactionMembersKidnappedCount.Clear();
			foreach (KeyValuePair<Pawn, PositionData> keyValuePair in this.pawns)
			{
				Pawn pawn;
				PositionData positionData;
				keyValuePair.Deconstruct(out pawn, out positionData);
				Pawn pawn2 = pawn;
				if (GravshipUtility.ConsideredKidnapped(this.engine, pawn2))
				{
					Gravship.tmpFactionMembersKidnappedCount.TryAdd(pawn2.Faction, 0);
					Dictionary<Faction, int> dictionary = Gravship.tmpFactionMembersKidnappedCount;
					Faction faction = pawn2.Faction;
					int num = dictionary[faction];
					dictionary[faction] = num + 1;
				}
			}
			foreach (KeyValuePair<Faction, int> keyValuePair2 in Gravship.tmpFactionMembersKidnappedCount)
			{
				Faction faction;
				int num;
				keyValuePair2.Deconstruct(out faction, out num);
				Faction faction2 = faction;
				int num2 = num;
				faction2.TryAffectGoodwillWith(Faction.OfPlayer, -10 * num2, true, true, HistoryEventDefOf.PawnKidnappedOnGravShip, null);
			}
		}

		// Token: 0x06014F56 RID: 85846 RVA: 0x00646F6C File Offset: 0x0064516C
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x06014F57 RID: 85847 RVA: 0x00646F80 File Offset: 0x00645180
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.traveledPct += this.TraveledPctStepPerTick * (float)delta;
			if (this.traveledPct >= 1f)
			{
				this.traveledPct = 1f;
				MapParent mapParent = Find.WorldObjects.MapParentAt(this.destinationTile);
				if (((mapParent != null) ? mapParent.Map : null) != null)
				{
					GravshipUtility.ArriveExistingMap(this);
					return;
				}
				LongEventHandler.QueueLongEvent(delegate()
				{
					GravshipUtility.ArriveNewMap(this);
				}, "GeneratingMap", false, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
			}
		}

		// Token: 0x170033ED RID: 13293
		// (get) Token: 0x06014F58 RID: 85848 RVA: 0x0064700D File Offset: 0x0064520D
		public int ConstantRandSeed
		{
			get
			{
				return HashCode.Combine<int, int>(this.ID, 18948292);
			}
		}

		// Token: 0x170033EE RID: 13294
		// (get) Token: 0x06014F59 RID: 85849 RVA: 0x0064701F File Offset: 0x0064521F
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x170033EF RID: 13295
		// (get) Token: 0x06014F5A RID: 85850 RVA: 0x00647027 File Offset: 0x00645227
		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from gravship", 13291050);
				return null;
			}
		}

		// Token: 0x170033F0 RID: 13296
		// (get) Token: 0x06014F5B RID: 85851 RVA: 0x0064703C File Offset: 0x0064523C
		public float PlayerWealthForStoryteller
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<Pawn, PositionData> keyValuePair in this.pawns)
				{
					Pawn pawn;
					PositionData positionData;
					keyValuePair.Deconstruct(out pawn, out positionData);
					Pawn pawn2 = pawn;
					num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn2);
					if (pawn2.Faction == Faction.OfPlayer)
					{
						float num2 = pawn2.MarketValue;
						if (pawn2.IsSlave)
						{
							num2 *= 0.75f;
						}
						num += num2;
					}
				}
				return num * 0.7f;
			}
		}

		// Token: 0x170033F1 RID: 13297
		// (get) Token: 0x06014F5C RID: 85852 RVA: 0x006470D8 File Offset: 0x006452D8
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return from x in this.Pawns
				where x.Faction == Faction.OfPlayer
				select x;
			}
		}

		// Token: 0x170033F2 RID: 13298
		// (get) Token: 0x06014F5D RID: 85853 RVA: 0x00647104 File Offset: 0x00645304
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.GravshipPointsRandomFactorRange;
			}
		}

		// Token: 0x170033F3 RID: 13299
		// (get) Token: 0x06014F5E RID: 85854 RVA: 0x0064710B File Offset: 0x0064530B
		public CellRect Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170033F4 RID: 13300
		// (get) Token: 0x06014F5F RID: 85855 RVA: 0x00647113 File Offset: 0x00645313
		public Building_GravEngine Engine
		{
			get
			{
				return this.engine;
			}
		}

		// Token: 0x170033F5 RID: 13301
		// (get) Token: 0x06014F60 RID: 85856 RVA: 0x0064711B File Offset: 0x0064531B
		public Building PilotConsole
		{
			get
			{
				return this.pilotConsole;
			}
		}

		// Token: 0x170033F6 RID: 13302
		// (get) Token: 0x06014F61 RID: 85857 RVA: 0x00647123 File Offset: 0x00645323
		public Dictionary<Thing, bool> PoweredOn
		{
			get
			{
				return this.powerOn;
			}
		}

		// Token: 0x170033F7 RID: 13303
		// (get) Token: 0x06014F62 RID: 85858 RVA: 0x0064712B File Offset: 0x0064532B
		public Dictionary<Thing, Thing> ConnectParents
		{
			get
			{
				return this.connectParents;
			}
		}

		// Token: 0x170033F8 RID: 13304
		// (get) Token: 0x06014F63 RID: 85859 RVA: 0x00647133 File Offset: 0x00645333
		public IEnumerable<IntVec3> GravFieldExtenderPositions
		{
			get
			{
				return this.GetRotatedValues(this.gravFieldExtenderPositions);
			}
		}

		// Token: 0x170033F9 RID: 13305
		// (get) Token: 0x06014F64 RID: 85860 RVA: 0x00647141 File Offset: 0x00645341
		public Dictionary<Thing, PositionData.Data> ThingPlacements
		{
			get
			{
				return this.GetPlacementValues<Thing>(this.things, ref this.tmpThingPlacements, ref this.tmpThingsRot);
			}
		}

		// Token: 0x170033FA RID: 13306
		// (get) Token: 0x06014F65 RID: 85861 RVA: 0x0064715B File Offset: 0x0064535B
		public Dictionary<Pawn, PositionData.Data> PawnPlacements
		{
			get
			{
				return this.GetPlacementValues<Pawn>(this.pawns, ref this.tmpPawns, ref this.tmpPawnsRot);
			}
		}

		// Token: 0x170033FB RID: 13307
		// (get) Token: 0x06014F66 RID: 85862 RVA: 0x00647175 File Offset: 0x00645375
		public Dictionary<Thing, PositionData>.KeyCollection Thrusters
		{
			get
			{
				return this.thrusters.Keys;
			}
		}

		// Token: 0x170033FC RID: 13308
		// (get) Token: 0x06014F67 RID: 85863 RVA: 0x00647182 File Offset: 0x00645382
		public Dictionary<Thing, PositionData.Data> ThrusterPlacements
		{
			get
			{
				return this.GetPlacementValues<Thing>(this.thrusters, ref this.tmpThrusterPlacements, ref this.tmpThrusterRot);
			}
		}

		// Token: 0x170033FD RID: 13309
		// (get) Token: 0x06014F68 RID: 85864 RVA: 0x0064719C File Offset: 0x0064539C
		public Dictionary<Thing, PositionData.Data> ExteriorDoorPlacements
		{
			get
			{
				return this.GetPlacementValues<Thing>(this.exteriorDoors, ref this.tmpExteriorDoorPlacements, ref this.tmpExteriorDoorRot);
			}
		}

		// Token: 0x170033FE RID: 13310
		// (get) Token: 0x06014F69 RID: 85865 RVA: 0x006471B6 File Offset: 0x006453B6
		public Dictionary<IntVec3, TerrainDef> Foundations
		{
			get
			{
				return this.GetRotatedValues<TerrainDef>(this.foundations, ref this.tmpFoundations, ref this.tmpFoundationRot);
			}
		}

		// Token: 0x170033FF RID: 13311
		// (get) Token: 0x06014F6A RID: 85866 RVA: 0x006471D0 File Offset: 0x006453D0
		public Dictionary<IntVec3, TerrainDef> Terrains
		{
			get
			{
				return this.GetRotatedValues<TerrainDef>(this.terrains, ref this.tmpTerrains, ref this.tmpTerrainRot);
			}
		}

		// Token: 0x17003400 RID: 13312
		// (get) Token: 0x06014F6B RID: 85867 RVA: 0x006471EA File Offset: 0x006453EA
		public IEnumerable<ValueTuple<IntVec3, RoofDef>> Roofs
		{
			get
			{
				return this.GetRotatedValues<RoofDef>(this.roofs);
			}
		}

		// Token: 0x17003401 RID: 13313
		// (get) Token: 0x06014F6C RID: 85868 RVA: 0x006471F8 File Offset: 0x006453F8
		public IEnumerable<ValueTuple<IntVec3, ColorDef>> TerrainColors
		{
			get
			{
				return this.GetRotatedValues<ColorDef>(this.terrainColors);
			}
		}

		// Token: 0x17003402 RID: 13314
		// (get) Token: 0x06014F6D RID: 85869 RVA: 0x00647206 File Offset: 0x00645406
		public IEnumerable<ValueTuple<IntVec3, Designation>> TerrainDesignations
		{
			get
			{
				return this.GetRotatedValues<Designation>(this.terrainDesignations);
			}
		}

		// Token: 0x17003403 RID: 13315
		// (get) Token: 0x06014F6E RID: 85870 RVA: 0x00647214 File Offset: 0x00645414
		public IEnumerable<ValueTuple<IntVec3, uint>> Gases
		{
			get
			{
				return this.GetRotatedValues<uint>(this.gases);
			}
		}

		// Token: 0x17003404 RID: 13316
		// (get) Token: 0x06014F6F RID: 85871 RVA: 0x00647222 File Offset: 0x00645422
		public IEnumerable<CellRect> OccupiedRects
		{
			get
			{
				foreach (CellRect cellRect in this.occupiedRects)
				{
					IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(cellRect.Min, this.rotation);
					IntVec3 adjustedLocalPosition2 = PrefabUtility.GetAdjustedLocalPosition(cellRect.Max, this.rotation);
					yield return CellRect.FromLimits(adjustedLocalPosition, adjustedLocalPosition2);
				}
				List<CellRect>.Enumerator enumerator = default(List<CellRect>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17003405 RID: 13317
		// (get) Token: 0x06014F70 RID: 85872 RVA: 0x00647232 File Offset: 0x00645432
		public IEnumerable<RoomTemperatureVacuum.Data> RoomTemperatures
		{
			get
			{
				foreach (RoomTemperatureVacuum roomTemperatureVacuum in this.roomTemperatures)
				{
					yield return new RoomTemperatureVacuum.Data
					{
						local = PrefabUtility.GetAdjustedLocalPosition(roomTemperatureVacuum.roomCell, this.rotation),
						temperature = roomTemperatureVacuum.temperature,
						vacuum = roomTemperatureVacuum.vacuum
					};
				}
				List<RoomTemperatureVacuum>.Enumerator enumerator = default(List<RoomTemperatureVacuum>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06014F71 RID: 85873 RVA: 0x00647244 File Offset: 0x00645444
		private Dictionary<T, PositionData.Data> GetPlacementValues<T>(Dictionary<T, PositionData> dictionary, ref Dictionary<T, PositionData.Data> working, ref Rot4 prevRotation) where T : Thing
		{
			if (working != null && prevRotation == this.rotation)
			{
				return working;
			}
			if (working == null)
			{
				working = new Dictionary<T, PositionData.Data>(dictionary.Count);
			}
			working.Clear();
			foreach (KeyValuePair<T, PositionData> keyValuePair in dictionary)
			{
				T t;
				PositionData positionData;
				keyValuePair.Deconstruct(out t, out positionData);
				T t2 = t;
				PositionData positionData2 = positionData;
				IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(positionData2.position, this.rotation);
				IntVec2 size = t2.def.size;
				bool flag = true;
				if (!t2.def.rotatable && size.x == size.z)
				{
					GenAdj.AdjustForRotation(ref adjustedLocalPosition, ref size, this.rotation);
					flag = false;
				}
				else if (!t2.def.rotatable && t2.def.category != ThingCategory.Building)
				{
					flag = false;
				}
				Rot4 rot = (!flag) ? t2.def.defaultPlacingRot : this.rotation.Rotated(positionData2.relativeRotation);
				working[t2] = new PositionData.Data
				{
					local = adjustedLocalPosition,
					rotation = rot,
					drafted = positionData2.drafted
				};
			}
			prevRotation = this.rotation;
			return working;
		}

		// Token: 0x06014F72 RID: 85874 RVA: 0x006473D0 File Offset: 0x006455D0
		private Dictionary<IntVec3, T> GetRotatedValues<T>(Dictionary<IntVec3, T> dictionary, ref Dictionary<IntVec3, T> working, ref Rot4 prevRotation)
		{
			if (working != null && prevRotation == this.rotation)
			{
				return working;
			}
			if (working == null)
			{
				working = new Dictionary<IntVec3, T>(dictionary.Count);
			}
			working.Clear();
			foreach (KeyValuePair<IntVec3, T> keyValuePair in dictionary)
			{
				IntVec3 intVec;
				T t;
				keyValuePair.Deconstruct(out intVec, out t);
				IntVec3 local = intVec;
				T value = t;
				working[PrefabUtility.GetAdjustedLocalPosition(local, this.rotation)] = value;
			}
			prevRotation = this.rotation;
			return working;
		}

		// Token: 0x06014F73 RID: 85875 RVA: 0x0064747C File Offset: 0x0064567C
		private IEnumerable<ValueTuple<IntVec3, T>> GetRotatedValues<T>(Dictionary<IntVec3, T> dictionary)
		{
			foreach (KeyValuePair<IntVec3, T> keyValuePair in dictionary)
			{
				IntVec3 intVec;
				T t;
				keyValuePair.Deconstruct(out intVec, out t);
				IntVec3 local = intVec;
				T item = t;
				yield return new ValueTuple<IntVec3, T>(PrefabUtility.GetAdjustedLocalPosition(local, this.rotation), item);
			}
			Dictionary<IntVec3, T>.Enumerator enumerator = default(Dictionary<IntVec3, T>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06014F74 RID: 85876 RVA: 0x00647493 File Offset: 0x00645693
		private IEnumerable<IntVec3> GetRotatedValues(List<IntVec3> list)
		{
			foreach (IntVec3 local in list)
			{
				yield return PrefabUtility.GetAdjustedLocalPosition(local, this.rotation);
			}
			List<IntVec3>.Enumerator enumerator = default(List<IntVec3>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0400E97C RID: 59772
		private Rot4 rotation;

		// Token: 0x0400E97D RID: 59773
		private CellRect bounds;

		// Token: 0x0400E97E RID: 59774
		private Building_GravEngine engine;

		// Token: 0x0400E97F RID: 59775
		private Building pilotConsole;

		// Token: 0x0400E980 RID: 59776
		private Dictionary<Thing, PositionData> things;

		// Token: 0x0400E981 RID: 59777
		private Dictionary<Pawn, PositionData> pawns;

		// Token: 0x0400E982 RID: 59778
		private Dictionary<Thing, bool> powerOn;

		// Token: 0x0400E983 RID: 59779
		private Dictionary<Thing, Thing> connectParents;

		// Token: 0x0400E984 RID: 59780
		private Dictionary<IntVec3, RoofDef> roofs;

		// Token: 0x0400E985 RID: 59781
		private Dictionary<IntVec3, TerrainDef> foundations;

		// Token: 0x0400E986 RID: 59782
		private Dictionary<IntVec3, TerrainDef> terrains;

		// Token: 0x0400E987 RID: 59783
		private Dictionary<IntVec3, ColorDef> terrainColors;

		// Token: 0x0400E988 RID: 59784
		private Dictionary<IntVec3, Designation> terrainDesignations;

		// Token: 0x0400E989 RID: 59785
		private Dictionary<IntVec3, uint> gases;

		// Token: 0x0400E98A RID: 59786
		private List<CellRect> occupiedRects;

		// Token: 0x0400E98B RID: 59787
		private List<RoomTemperatureVacuum> roomTemperatures;

		// Token: 0x0400E98C RID: 59788
		private List<IntVec3> gravFieldExtenderPositions;

		// Token: 0x0400E98D RID: 59789
		private StoryState storyState;

		// Token: 0x0400E98E RID: 59790
		public MoveableAreas areas;

		// Token: 0x0400E98F RID: 59791
		public List<AutoSlaughterConfig> autoSlaughterConfigs;

		// Token: 0x0400E990 RID: 59792
		private Dictionary<Thing, PositionData> thrusters;

		// Token: 0x0400E991 RID: 59793
		private Dictionary<Thing, PositionData> exteriorDoors;

		// Token: 0x0400E992 RID: 59794
		[Unsaved(false)]
		public IntVec3 originalPosition;

		// Token: 0x0400E993 RID: 59795
		public Vector3 engineToCenter;

		// Token: 0x0400E994 RID: 59796
		public IntVec3 launchDirection;

		// Token: 0x0400E995 RID: 59797
		[Unsaved(false)]
		public Capture capture;

		// Token: 0x0400E996 RID: 59798
		[Unsaved(false)]
		public List<LayerSubMesh> bakedIndoorMasks = new List<LayerSubMesh>();

		// Token: 0x0400E997 RID: 59799
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x0400E998 RID: 59800
		private PlanetTile initialTile = PlanetTile.Invalid;

		// Token: 0x0400E999 RID: 59801
		private float traveledPct;

		// Token: 0x0400E99A RID: 59802
		private bool arrived;

		// Token: 0x0400E99B RID: 59803
		private Material cachedMat;

		// Token: 0x0400E99C RID: 59804
		private const float TravelSpeed = 0.00025f;

		// Token: 0x0400E99D RID: 59805
		private List<Thing> tmpPoweredThings = new List<Thing>();

		// Token: 0x0400E99E RID: 59806
		private List<bool> tmpPoweredThingsOn = new List<bool>();

		// Token: 0x0400E99F RID: 59807
		private List<Thing> tmpConnectedThings = new List<Thing>();

		// Token: 0x0400E9A0 RID: 59808
		private List<Thing> tmpConnectedParents = new List<Thing>();

		// Token: 0x0400E9A1 RID: 59809
		private List<Thing> tmpThrusters = new List<Thing>();

		// Token: 0x0400E9A2 RID: 59810
		private List<PositionData> tmpThrusterPositionData = new List<PositionData>();

		// Token: 0x0400E9A3 RID: 59811
		private List<Thing> tmpExteriorDoors = new List<Thing>();

		// Token: 0x0400E9A4 RID: 59812
		private List<PositionData> tmpExteriorDoorPositionData = new List<PositionData>();

		// Token: 0x0400E9A5 RID: 59813
		private static readonly Dictionary<Faction, int> tmpFactionMembersKidnappedCount = new Dictionary<Faction, int>();

		// Token: 0x0400E9A6 RID: 59814
		private Dictionary<Thing, PositionData.Data> tmpThingPlacements;

		// Token: 0x0400E9A7 RID: 59815
		private Rot4 tmpThingsRot;

		// Token: 0x0400E9A8 RID: 59816
		private Dictionary<Pawn, PositionData.Data> tmpPawns;

		// Token: 0x0400E9A9 RID: 59817
		private Rot4 tmpPawnsRot;

		// Token: 0x0400E9AA RID: 59818
		private Dictionary<Thing, PositionData.Data> tmpThrusterPlacements;

		// Token: 0x0400E9AB RID: 59819
		private Rot4 tmpThrusterRot;

		// Token: 0x0400E9AC RID: 59820
		private Dictionary<Thing, PositionData.Data> tmpExteriorDoorPlacements;

		// Token: 0x0400E9AD RID: 59821
		private Rot4 tmpExteriorDoorRot;

		// Token: 0x0400E9AE RID: 59822
		private Dictionary<IntVec3, TerrainDef> tmpFoundations;

		// Token: 0x0400E9AF RID: 59823
		private Rot4 tmpFoundationRot;

		// Token: 0x0400E9B0 RID: 59824
		private Dictionary<IntVec3, TerrainDef> tmpTerrains;

		// Token: 0x0400E9B1 RID: 59825
		private Rot4 tmpTerrainRot;
	}
}
