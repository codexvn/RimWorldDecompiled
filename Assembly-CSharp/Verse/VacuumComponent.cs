using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000463 RID: 1123
	[StaticConstructorOnStartup]
	public class VacuumComponent : MapComponent, ICellBoolGiver, IDisposable
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x00094950 File Offset: 0x00092B50
		private CellBoolDrawer Drawer
		{
			get
			{
				CellBoolDrawer result;
				if ((result = this.drawerInt) == null)
				{
					result = (this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3600, 0.33f));
				}
				return result;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000949A0 File Offset: 0x00092BA0
		private Material WarningMat
		{
			get
			{
				return VacuumComponent.IntWarningMat;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x000949A7 File Offset: 0x00092BA7
		private Material VacuumOverlayMat0
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayMat0;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x000949AE File Offset: 0x00092BAE
		private Material VacuumOverlayTransitionMat0
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayTransitionMat0;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x000949B5 File Offset: 0x00092BB5
		private Material VacuumOverlayMat1
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayMat1;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x000949BC File Offset: 0x00092BBC
		private Material VacuumOverlayTransitionMat1
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayTransitionMat1;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x000949C3 File Offset: 0x00092BC3
		private bool ActiveOnMap
		{
			get
			{
				return ModLister.OdysseyInstalled && this.map.Biome.inVacuum;
			}
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000949E0 File Offset: 0x00092BE0
		public VacuumComponent(Map map) : base(map)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				return;
			}
			this.InitializeIfRequired();
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x00094A6C File Offset: 0x00092C6C
		private void InitializeIfRequired()
		{
			if (!this.ActiveOnMap)
			{
				return;
			}
			this.map.events.RegionsRoomsChanged += this.Dirty;
			this.map.events.DoorOpened += delegate(Building_Door _)
			{
				this.Dirty();
			};
			this.map.events.DoorClosed += delegate(Building_Door _)
			{
				this.Dirty();
			};
			this.map.events.TerrainChanged += delegate(IntVec3 _)
			{
				this.Dirty();
			};
			this.map.events.RoofChanged += delegate(IntVec3 _)
			{
				this.Dirty();
			};
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x00094B0E File Offset: 0x00092D0E
		public override void FinalizeInit()
		{
			if (this.ActiveOnMap)
			{
				this.RebuildData();
			}
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x00094B1E File Offset: 0x00092D1E
		public override void MapComponentTick()
		{
			if (!this.ActiveOnMap)
			{
				return;
			}
			if (!this.map.IsHashIntervalTick(250))
			{
				return;
			}
			if (this.dirty)
			{
				this.RebuildData();
			}
			this.ExchangeRoomVacuum();
			this.UpdateVacuumOverlay();
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x00094B58 File Offset: 0x00092D58
		private bool HasDirectPathToVacuum(Room room, out HashSet<IntVec3> warning)
		{
			VacuumComponent.frontier.Enqueue(room);
			Room room2;
			while (VacuumComponent.frontier.TryDequeue(out room2))
			{
				VacuumComponent.visited.Add(room2);
				if (room2.ExposedToSpace)
				{
					warning = VacuumComponent.exteriorCells[room2];
					VacuumComponent.visited.Clear();
					VacuumComponent.exteriorCells.Clear();
					VacuumComponent.frontier.Clear();
					return true;
				}
				VacuumComponent.GroupData groupData;
				if (this.roomGroups.TryGetValue(room2, out groupData))
				{
					using (Dictionary<Room, HashSet<IntVec3>>.Enumerator enumerator = groupData.adjacent.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Room, HashSet<IntVec3>> keyValuePair = enumerator.Current;
							Room room3;
							HashSet<IntVec3> hashSet;
							keyValuePair.Deconstruct(out room3, out hashSet);
							Room room4 = room3;
							HashSet<IntVec3> hashSet2 = hashSet;
							if (!VacuumComponent.visited.Contains(room4) && !VacuumComponent.frontier.Contains(room4))
							{
								foreach (IntVec3 c in hashSet2)
								{
									Building edifice = c.GetEdifice(this.map);
									if (edifice != null)
									{
										Building_Door building_Door = edifice as Building_Door;
										if (building_Door == null || !building_Door.Open)
										{
											continue;
										}
									}
									VacuumComponent.exteriorCells[room4] = hashSet2;
									VacuumComponent.frontier.Enqueue(room4);
									break;
								}
							}
						}
						continue;
					}
					break;
				}
			}
			VacuumComponent.visited.Clear();
			VacuumComponent.exteriorCells.Clear();
			VacuumComponent.frontier.Clear();
			warning = null;
			return false;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x00094CE4 File Offset: 0x00092EE4
		private void ExchangeRoomVacuum()
		{
			this.warningCells.Clear();
			this.warningMatrices.Clear();
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room2 = room;
				VacuumComponent.GroupData groupData2 = groupData;
				this.roomVacuums[room2] = room2.UnsanitizedVacuum;
				foreach (KeyValuePair<Room, HashSet<IntVec3>> keyValuePair2 in groupData2.adjacent)
				{
					HashSet<IntVec3> hashSet;
					keyValuePair2.Deconstruct(out room, out hashSet);
					Room room3 = room;
					this.roomVacuums[room3] = room3.UnsanitizedVacuum;
				}
			}
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room4 = room;
				VacuumComponent.GroupData groupData3 = groupData;
				if (groupData3.adjacent.Count != 0 && !room4.IsDoorway)
				{
					float num = this.roomVacuums[room4];
					int num2 = room4.CellCount;
					float num3 = (float)room4.CellCount * num;
					int num4 = room4.CellCount;
					if (!room4.ExposedToSpace)
					{
						foreach (KeyValuePair<Room, HashSet<IntVec3>> keyValuePair2 in groupData3.adjacent)
						{
							HashSet<IntVec3> hashSet;
							keyValuePair2.Deconstruct(out room, out hashSet);
							Room room5 = room;
							HashSet<IntVec3> hashSet2 = hashSet;
							num2 += room5.CellCount;
							num3 += (float)room5.CellCount * this.roomVacuums[room5];
							if (!room5.ExposedToSpace)
							{
								num4 += room5.CellCount;
							}
							else
							{
								foreach (IntVec3 item in hashSet2)
								{
									if (this.warningCells.Add(item))
									{
										this.warningMatrices.Add(Matrix4x4.Translate(item.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay)));
									}
								}
							}
						}
						float num5 = Mathf.Clamp(num3 / (float)num2, -0.1f, 1f);
						if (groupData3.hasDirectPathToVacuum)
						{
							if (room4.Vacuum < 1f)
							{
								foreach (IntVec3 item2 in groupData3.directWarnings)
								{
									if (this.warningCells.Add(item2))
									{
										this.warningMatrices.Add(Matrix4x4.Translate(item2.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay)));
									}
								}
								float num6 = Mathf.Min(0.1f / ((float)num4 / 100f), 1f);
								num5 = Mathf.Clamp(num5 + num6, -0.1f, 1f);
								float num7 = Mathf.Min(0.5f / ((float)num4 / 100f), 1f);
								float num8 = (num5 - num) * 1.0416666f * num7;
								room4.Vacuum = num + num8;
							}
						}
						else if (!Mathf.Approximately(num, num5))
						{
							float num9 = Mathf.Min(0.5f / ((float)num4 / 100f), 1f);
							float num10 = (num5 - num) * 1.0416666f * num9;
							room4.Vacuum = num + num10;
						}
					}
				}
			}
			this.roomVacuums.Clear();
			this.Drawer.SetDirty();
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0009510C File Offset: 0x0009330C
		private void RebuildData()
		{
			using (ProfilerBlock.Scope("VacuumComponent.RebuildData"))
			{
				this.dirty = false;
				this.ClearBuffer();
				this.MergeRoomsIntoGroups();
				this.RegenerateVacuumOverlay();
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x00095160 File Offset: 0x00093360
		public void Dirty()
		{
			this.dirty = true;
			this.SetDrawerDirty();
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x00095170 File Offset: 0x00093370
		private void ClearBuffer()
		{
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData item;
				keyValuePair.Deconstruct(out room, out item);
				FullPool<VacuumComponent.GroupData>.Return(item);
			}
			this.roomGroups.Clear();
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000951D8 File Offset: 0x000933D8
		private void MergeRoomsIntoGroups()
		{
			foreach (Room room in this.map.regionGrid.AllRooms)
			{
				if (!room.ExposedToSpace)
				{
					VacuumComponent.GroupData orAddGroup = this.GetOrAddGroup(room);
					if (room.IsDoorway)
					{
						Building_Door door = room.Cells.First<IntVec3>().GetDoor(this.map);
						if (door == null || !door.ExchangeVacuum)
						{
							continue;
						}
						CellRect cellRect = door.OccupiedRect();
						using (CellRect.Enumerator enumerator2 = cellRect.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								IntVec3 intVec = enumerator2.Current;
								for (int i = 0; i < 4; i++)
								{
									IntVec3 intVec2 = intVec + GenAdj.CardinalDirections[i];
									if (!cellRect.Contains(intVec2))
									{
										Room room2 = intVec2.GetRoom(this.map);
										if (room2 != null && room2 != room)
										{
											HashSet<IntVec3> hashSet;
											if (!orAddGroup.adjacent.TryGetValue(room2, out hashSet))
											{
												hashSet = (orAddGroup.adjacent[room2] = SimplePool<HashSet<IntVec3>>.Get());
											}
											hashSet.Add(intVec);
										}
									}
								}
							}
							continue;
						}
					}
					foreach (IntVec3 intVec3 in room.BorderCellsCardinal)
					{
						Building edifice = intVec3.GetEdifice(this.map);
						if (edifice != null && edifice.ExchangeVacuum)
						{
							CellRect cellRect2 = edifice.OccupiedRect();
							foreach (IntVec3 a in cellRect2)
							{
								for (int j = 0; j < 4; j++)
								{
									IntVec3 intVec4 = a + GenAdj.CardinalDirections[j];
									if (!cellRect2.Contains(intVec4))
									{
										Room room3 = intVec4.GetRoom(this.map);
										if (room3 != null && room3 != room)
										{
											HashSet<IntVec3> hashSet2;
											if (!orAddGroup.adjacent.TryGetValue(room3, out hashSet2))
											{
												hashSet2 = (orAddGroup.adjacent[room3] = SimplePool<HashSet<IntVec3>>.Get());
											}
											hashSet2.Add(intVec3);
										}
									}
								}
							}
						}
					}
				}
			}
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room4;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room4, out groupData);
				Room room5 = room4;
				VacuumComponent.GroupData groupData2 = groupData;
				HashSet<IntVec3> directWarnings;
				if (this.HasDirectPathToVacuum(room5, out directWarnings))
				{
					groupData2.hasDirectPathToVacuum = true;
					groupData2.directWarnings = directWarnings;
				}
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0009550C File Offset: 0x0009370C
		private VacuumComponent.GroupData GetOrAddGroup(Room room)
		{
			VacuumComponent.GroupData result;
			if (this.roomGroups.TryGetValue(room, out result))
			{
				return result;
			}
			VacuumComponent.GroupData groupData = FullPool<VacuumComponent.GroupData>.Get();
			this.roomGroups[room] = groupData;
			return groupData;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0009553F File Offset: 0x0009373F
		public void SetDrawerDirty()
		{
			this.Drawer.SetDirty();
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0009554C File Offset: 0x0009374C
		private void RegenerateVacuumOverlay()
		{
			this.vacuumBoxMatrices.Clear();
			this.vacuumBoxIntensities.Clear();
			this.vacuumedCells.Clear();
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room2 = room;
				room2.vacuumOverlayDrawn = VacuumComponent.RoomVacuumVisible(room2);
				room2.vacuumOverlayRects = 0;
				if (room2.vacuumOverlayDrawn)
				{
					float vacuum = room2.Vacuum;
					foreach (IntVec3 key in room2.Cells)
					{
						this.vacuumedCells.Add(key, vacuum);
					}
					foreach (Region region in room2.Regions)
					{
						foreach (CellRect cellRect in region.EnumerateRectangleCover())
						{
							Vector3 centerVector = cellRect.CenterVector3;
							centerVector.y = AltitudeLayer.Gas.AltitudeFor();
							Matrix4x4 item = Matrix4x4.TRS(centerVector, Quaternion.identity, new Vector3((float)cellRect.Width, 1f, (float)cellRect.Height));
							room2.vacuumOverlayRects++;
							this.vacuumBoxMatrices.Add(item);
							this.vacuumBoxIntensities.Add(vacuum);
						}
					}
				}
			}
			this.vacuumTransitionMatrices.Clear();
			this.vacuumTransitionIntensities.Clear();
			foreach (KeyValuePair<IntVec3, float> keyValuePair2 in this.vacuumedCells)
			{
				IntVec3 intVec;
				float num;
				keyValuePair2.Deconstruct(out intVec, out num);
				IntVec3 a = intVec;
				float item2 = num;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 key2 = a + GenAdj.CardinalDirections[i];
					if (!this.vacuumedCells.ContainsKey(key2))
					{
						Matrix4x4 item3 = Matrix4x4.TRS(key2.ToVector3ShiftedWithAltitude(AltitudeLayer.Gas), new Rot4(i).AsQuat, Vector3.one);
						this.vacuumTransitionMatrices.Add(item3);
						this.vacuumTransitionIntensities.Add(item2);
					}
				}
			}
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0009583C File Offset: 0x00093A3C
		private void UpdateVacuumOverlay()
		{
			int num = 0;
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room2 = room;
				if (VacuumComponent.RoomVacuumVisible(room2) != room2.vacuumOverlayDrawn)
				{
					this.RegenerateVacuumOverlay();
					break;
				}
				for (int i = 0; i < room2.vacuumOverlayRects; i++)
				{
					this.vacuumBoxIntensities[num++] = room2.Vacuum;
				}
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000958DC File Offset: 0x00093ADC
		private static bool RoomVacuumVisible(Room room)
		{
			return !room.ExposedToSpace && room.UnsanitizedVacuum > 0.5f && (!room.IsDoorway || room.Door.Open);
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x0009590C File Offset: 0x00093B0C
		public override void MapComponentDraw()
		{
			if (!this.map.Biome.inVacuum)
			{
				return;
			}
			if (Find.PlaySettings.showVacuumOverlay && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				this.Drawer.MarkForDraw();
				if (this.warningMatrices.Count > 0)
				{
					Graphics.DrawMeshInstanced(MeshPool.plane08, 0, this.WarningMat, this.warningMatrices);
				}
			}
			using (ProfilerBlock.Scope("VacuumComponent.Drawer.CellBoolDrawerUpdate()"))
			{
				this.Drawer.CellBoolDrawerUpdate();
			}
			if (WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			if (this.vacuumBoxMatrices.Count > 0)
			{
				if (this.vacuumBoxPropBlock == null)
				{
					this.vacuumBoxPropBlock = new MaterialPropertyBlock();
				}
				this.vacuumBoxPropBlock.Clear();
				if (this.vacuumBoxIntensities.Count > 0)
				{
					this.vacuumBoxPropBlock.SetFloatArray(VacuumComponent.VacuumPercent, this.vacuumBoxIntensities);
				}
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayMat0, this.vacuumBoxMatrices, this.vacuumBoxPropBlock);
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayMat1, this.vacuumBoxMatrices, this.vacuumBoxPropBlock);
			}
			if (this.vacuumTransitionMatrices.Count > 0)
			{
				if (this.vacuumTransitionPropBlock == null)
				{
					this.vacuumTransitionPropBlock = new MaterialPropertyBlock();
				}
				this.vacuumTransitionPropBlock.Clear();
				if (this.vacuumTransitionIntensities.Count > 0)
				{
					this.vacuumTransitionPropBlock.SetFloatArray(VacuumComponent.VacuumPercent, this.vacuumTransitionIntensities);
				}
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayTransitionMat0, this.vacuumTransitionMatrices, this.vacuumTransitionPropBlock);
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayTransitionMat1, this.vacuumTransitionMatrices, this.vacuumTransitionPropBlock);
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x000440FD File Offset: 0x000422FD
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x00095AC4 File Offset: 0x00093CC4
		public bool GetCellBool(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			Building edifice = c.GetEdifice(this.map);
			return !c.Fogged(this.map) && (edifice == null || edifice.ExchangeVacuum);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00095B0C File Offset: 0x00093D0C
		public Color GetCellExtraColor(int index)
		{
			float vacuum = this.map.cellIndices.IndexToCell(index).GetVacuum(this.map);
			if (vacuum < 0.5f)
			{
				return Color.Lerp(Color.green, Color.yellow, Mathf.InverseLerp(0f, 0.5f, vacuum));
			}
			return Color.Lerp(Color.yellow, Color.red, Mathf.InverseLerp(0.5f, 1f, vacuum));
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x00095B7D File Offset: 0x00093D7D
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.InitializeIfRequired();
			}
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x00095B8D File Offset: 0x00093D8D
		public void Dispose()
		{
			this.ClearBuffer();
		}

		// Token: 0x0400198B RID: 6539
		private CellBoolDrawer drawerInt;

		// Token: 0x0400198C RID: 6540
		private bool dirty = true;

		// Token: 0x0400198D RID: 6541
		private const int UpdateInterval = 250;

		// Token: 0x0400198E RID: 6542
		private const float VacuumDeltaPerSecondFactor = 1.0416666f;

		// Token: 0x0400198F RID: 6543
		private const float VacuumFactorPerHundredCells = 0.5f;

		// Token: 0x04001990 RID: 6544
		private const float DirectVacuumPenaltyPerHundredCells = 0.1f;

		// Token: 0x04001991 RID: 6545
		private const float WarningLevel = 0.5f;

		// Token: 0x04001992 RID: 6546
		private readonly Dictionary<Room, VacuumComponent.GroupData> roomGroups = new Dictionary<Room, VacuumComponent.GroupData>();

		// Token: 0x04001993 RID: 6547
		private static readonly Material IntWarningMat = MatLoader.LoadMat("Map/OxygenOverlay/Warning", -1);

		// Token: 0x04001994 RID: 6548
		private static readonly Material IntVacuumOverlayMat0 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlay", -1);

		// Token: 0x04001995 RID: 6549
		private static readonly Material IntVacuumOverlayTransitionMat0 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlayTransition", -1);

		// Token: 0x04001996 RID: 6550
		private static readonly Material IntVacuumOverlayMat1 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlay2", -1);

		// Token: 0x04001997 RID: 6551
		private static readonly Material IntVacuumOverlayTransitionMat1 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlayTransition2", -1);

		// Token: 0x04001998 RID: 6552
		private static readonly int VacuumPercent = Shader.PropertyToID("_VacuumPercent");

		// Token: 0x04001999 RID: 6553
		private readonly List<Matrix4x4> vacuumBoxMatrices = new List<Matrix4x4>();

		// Token: 0x0400199A RID: 6554
		private readonly List<float> vacuumBoxIntensities = new List<float>();

		// Token: 0x0400199B RID: 6555
		private MaterialPropertyBlock vacuumBoxPropBlock;

		// Token: 0x0400199C RID: 6556
		private readonly Dictionary<IntVec3, float> vacuumedCells = new Dictionary<IntVec3, float>();

		// Token: 0x0400199D RID: 6557
		private readonly List<Matrix4x4> vacuumTransitionMatrices = new List<Matrix4x4>();

		// Token: 0x0400199E RID: 6558
		private readonly List<float> vacuumTransitionIntensities = new List<float>();

		// Token: 0x0400199F RID: 6559
		private readonly Dictionary<Room, float> roomVacuums = new Dictionary<Room, float>();

		// Token: 0x040019A0 RID: 6560
		private readonly HashSet<IntVec3> warningCells = new HashSet<IntVec3>();

		// Token: 0x040019A1 RID: 6561
		private readonly List<Matrix4x4> warningMatrices = new List<Matrix4x4>();

		// Token: 0x040019A2 RID: 6562
		private MaterialPropertyBlock vacuumTransitionPropBlock;

		// Token: 0x040019A3 RID: 6563
		private static readonly HashSet<Room> visited = new HashSet<Room>();

		// Token: 0x040019A4 RID: 6564
		private static readonly Dictionary<Room, HashSet<IntVec3>> exteriorCells = new Dictionary<Room, HashSet<IntVec3>>();

		// Token: 0x040019A5 RID: 6565
		private static readonly Queue<Room> frontier = new Queue<Room>();

		// Token: 0x02000464 RID: 1124
		private class GroupData : IFullPoolable
		{
			// Token: 0x06001D8E RID: 7566 RVA: 0x00095C2C File Offset: 0x00093E2C
			public void Reset()
			{
				foreach (KeyValuePair<Room, HashSet<IntVec3>> keyValuePair in this.adjacent)
				{
					Room room;
					HashSet<IntVec3> hashSet;
					keyValuePair.Deconstruct(out room, out hashSet);
					HashSet<IntVec3> hashSet2 = hashSet;
					hashSet2.Clear();
					SimplePool<HashSet<IntVec3>>.Return(hashSet2);
				}
				this.adjacent.Clear();
				this.directWarnings = null;
				this.hasDirectPathToVacuum = false;
			}

			// Token: 0x040019A6 RID: 6566
			public readonly Dictionary<Room, HashSet<IntVec3>> adjacent = new Dictionary<Room, HashSet<IntVec3>>();

			// Token: 0x040019A7 RID: 6567
			public HashSet<IntVec3> directWarnings;

			// Token: 0x040019A8 RID: 6568
			public bool hasDirectPathToVacuum;
		}
	}
}
