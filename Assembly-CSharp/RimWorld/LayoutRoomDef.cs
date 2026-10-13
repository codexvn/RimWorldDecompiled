using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A71 RID: 6769
	public class LayoutRoomDef : Def
	{
		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x060097E6 RID: 38886 RVA: 0x002C54F4 File Offset: 0x002C36F4
		public RoomContentsWorker ContentsWorker
		{
			get
			{
				return this.GetWorker(ref this.workerInt);
			}
		}

		// Token: 0x060097E7 RID: 38887 RVA: 0x002C5504 File Offset: 0x002C3704
		public bool CanResolve(LayoutRoom room)
		{
			int area = room.Area;
			CellRect cellRect;
			return (room.requiredDef == this || !this.dontPlaceRandomly) && ((this.minSingleRectHeight <= 0 && this.minSingleRectWidth <= 0) || room.TryGetRectOfSize(this.minSingleRectWidth, this.minSingleRectHeight, out cellRect)) && this.SatisfiesMinAdjRooms(room) && (area >= this.areaSizeRange.min && area <= this.areaSizeRange.max && room.connections.Count >= this.minConnectedRooms) && (!this.requiresSingleRectRoom || room.rects.Count == 1);
		}

		// Token: 0x060097E8 RID: 38888 RVA: 0x002C55AC File Offset: 0x002C37AC
		private bool SatisfiesMinAdjRooms(LayoutRoom room)
		{
			if (this.minAdjRooms <= 0)
			{
				return true;
			}
			if (room.connections.Count < this.minAdjRooms)
			{
				return false;
			}
			int num = 0;
			foreach (LayoutRoom layoutRoom in room.connections)
			{
				int num2 = num;
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					foreach (CellRect other in room.rects)
					{
						if (cellRect.OverlapsCardinal(other))
						{
							num++;
							break;
						}
					}
					if (num != num2)
					{
						break;
					}
				}
				if (num >= this.minAdjRooms)
				{
					break;
				}
			}
			return num >= this.minAdjRooms;
		}

		// Token: 0x060097E9 RID: 38889 RVA: 0x002C56C8 File Offset: 0x002C38C8
		public void ResolveSketch(LayoutRoomParams parms)
		{
			SketchResolveParams parms2 = default(SketchResolveParams);
			foreach (CellRect cellRect in parms.room.rects)
			{
				if (!this.floorTypes.NullOrEmpty<TerrainDef>())
				{
					TerrainDef def = this.floorTypes.RandomElement<TerrainDef>();
					foreach (IntVec3 pos in cellRect)
					{
						parms.sketch.AddTerrain(def, pos, true);
					}
				}
				if (this.edgeTerrain != null)
				{
					foreach (IntVec3 intVec in cellRect.ContractedBy(1).EdgeCells)
					{
						bool flag = true;
						foreach (CellRect lhs in parms.room.rects)
						{
							if (!(lhs == cellRect) && lhs.ContractedBy(1).Contains(intVec))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							parms.sketch.AddTerrain(this.edgeTerrain, intVec, true);
						}
					}
				}
				parms2.rect = new CellRect?(cellRect);
				parms2.sketch = parms.sketch;
				if (this.sketchResolverDef != null)
				{
					this.sketchResolverDef.Resolve(parms2);
				}
			}
		}

		// Token: 0x060097EA RID: 38890 RVA: 0x002C58C0 File Offset: 0x002C3AC0
		public void PreResolveContents(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContentsWorker contentsWorker = this.ContentsWorker;
			if (contentsWorker == null)
			{
				return;
			}
			contentsWorker.PreFillRooms(map, room, faction, threatPoints);
		}

		// Token: 0x060097EB RID: 38891 RVA: 0x002C58D7 File Offset: 0x002C3AD7
		public void PostResolveContents(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContentsWorker contentsWorker = this.ContentsWorker;
			if (contentsWorker == null)
			{
				return;
			}
			contentsWorker.PostFillRooms(map, room, faction, threatPoints);
		}

		// Token: 0x060097EC RID: 38892 RVA: 0x002C58EE File Offset: 0x002C3AEE
		public void ResolveContents(Map map, LayoutRoom room, float? threatPoints = null, Faction faction = null)
		{
			RoomContentsWorker contentsWorker = this.ContentsWorker;
			if (contentsWorker == null)
			{
				return;
			}
			contentsWorker.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060097ED RID: 38893 RVA: 0x002C5905 File Offset: 0x002C3B05
		private RoomContentsWorker GetWorker(ref RoomContentsWorker worker)
		{
			if (this.roomContentsWorkerType == null)
			{
				return null;
			}
			if (worker != null)
			{
				return worker;
			}
			worker = (RoomContentsWorker)Activator.CreateInstance(this.roomContentsWorkerType);
			worker.Initialize(this);
			return worker;
		}

		// Token: 0x04006582 RID: 25986
		public SketchResolverDef sketchResolverDef;

		// Token: 0x04006583 RID: 25987
		public IntRange areaSizeRange = new IntRange(25, int.MaxValue);

		// Token: 0x04006584 RID: 25988
		public bool requiresSingleRectRoom;

		// Token: 0x04006585 RID: 25989
		public List<TerrainDef> floorTypes;

		// Token: 0x04006586 RID: 25990
		public TerrainDef edgeTerrain;

		// Token: 0x04006587 RID: 25991
		public int minSingleRectWidth;

		// Token: 0x04006588 RID: 25992
		public int minSingleRectHeight;

		// Token: 0x04006589 RID: 25993
		public Type roomContentsWorkerType = typeof(RoomContentsWorker);

		// Token: 0x0400658A RID: 25994
		public bool canBeInMixedRoom;

		// Token: 0x0400658B RID: 25995
		public bool dontPlaceRandomly;

		// Token: 0x0400658C RID: 25996
		public bool isValidPlayerSpawnRoom = true;

		// Token: 0x0400658D RID: 25997
		public bool dontDestroyWallsDoors;

		// Token: 0x0400658E RID: 25998
		public SimpleCurve threatPointsScaleCurve;

		// Token: 0x0400658F RID: 25999
		public int minConnectedRooms;

		// Token: 0x04006590 RID: 26000
		public int minAdjRooms;

		// Token: 0x04006591 RID: 26001
		public bool spawnJunk = true;

		// Token: 0x04006592 RID: 26002
		public bool canMergeWithAdjacentRoom;

		// Token: 0x04006593 RID: 26003
		public bool canRemoveBorderDoors;

		// Token: 0x04006594 RID: 26004
		public bool canRemoveBorderWalls;

		// Token: 0x04006595 RID: 26005
		public RoofDef roofDef;

		// Token: 0x04006596 RID: 26006
		public bool noRoof;

		// Token: 0x04006597 RID: 26007
		public FloatRange? itemsPer100CellsRange;

		// Token: 0x04006598 RID: 26008
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x04006599 RID: 26009
		public List<LayoutScatterParms> scatter = new List<LayoutScatterParms>();

		// Token: 0x0400659A RID: 26010
		public List<LayoutScatterTerrainParms> scatterTerrain = new List<LayoutScatterTerrainParms>();

		// Token: 0x0400659B RID: 26011
		public List<LayoutFillEdgesParms> fillEdges = new List<LayoutFillEdgesParms>();

		// Token: 0x0400659C RID: 26012
		public List<LayoutFillInteriorParms> fillInterior = new List<LayoutFillInteriorParms>();

		// Token: 0x0400659D RID: 26013
		public List<LayoutWallAttatchmentParms> wallAttachments = new List<LayoutWallAttatchmentParms>();

		// Token: 0x0400659E RID: 26014
		public List<LayoutPrefabParms> prefabs = new List<LayoutPrefabParms>();

		// Token: 0x0400659F RID: 26015
		public List<LayoutPartParms> parts = new List<LayoutPartParms>();

		// Token: 0x040065A0 RID: 26016
		[Unsaved(false)]
		private RoomContentsWorker workerInt;
	}
}
