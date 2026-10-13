using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBA RID: 7866
	public class LayoutSketch : Sketch
	{
		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x0600AE75 RID: 44661 RVA: 0x0031C5AB File Offset: 0x0031A7AB
		protected virtual ThingDef WallThing
		{
			get
			{
				return this.wall ?? ThingDefOf.Wall;
			}
		}

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x0600AE76 RID: 44662 RVA: 0x0031C5BC File Offset: 0x0031A7BC
		protected virtual ThingDef DoorThing
		{
			get
			{
				return this.door ?? ThingDefOf.Door;
			}
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x0600AE77 RID: 44663 RVA: 0x0031C5CD File Offset: 0x0031A7CD
		public virtual ThingDef WallLampThing
		{
			get
			{
				return this.wallLamp;
			}
		}

		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x0600AE78 RID: 44664 RVA: 0x0031C5D5 File Offset: 0x0031A7D5
		protected virtual TerrainDef FloorTerrain
		{
			get
			{
				return this.floor ?? TerrainDefOf.PavedTile;
			}
		}

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x0600AE79 RID: 44665 RVA: 0x0031C5E6 File Offset: 0x0031A7E6
		public virtual TerrainDef DefaultAffordanceTerrain
		{
			get
			{
				return this.defaultAffordanceTerrain;
			}
		}

		// Token: 0x0600AE7A RID: 44666 RVA: 0x0031C5EE File Offset: 0x0031A7EE
		protected virtual ThingDef GetWallStuff(int roomId)
		{
			return this.wallStuff;
		}

		// Token: 0x0600AE7B RID: 44667 RVA: 0x0031C5F6 File Offset: 0x0031A7F6
		protected virtual ThingDef GetDoorStuff(int roomId)
		{
			return this.doorStuff;
		}

		// Token: 0x0600AE7C RID: 44668 RVA: 0x0031C5FE File Offset: 0x0031A7FE
		public void FlushLayoutToSketch()
		{
			this.FlushLayoutToSketch(IntVec3.Zero);
		}

		// Token: 0x0600AE7D RID: 44669 RVA: 0x0031C60C File Offset: 0x0031A80C
		public void FlushLayoutToSketch(IntVec3 at)
		{
			this.structureLayout.container = this.structureLayout.container.MovedBy(at);
			foreach (LayoutRoom layoutRoom in this.structureLayout.Rooms)
			{
				for (int i = 0; i < layoutRoom.rects.Count; i++)
				{
					layoutRoom.rects[i] = layoutRoom.rects[i].MovedBy(at);
				}
			}
			for (int j = this.structureLayout.container.minX; j <= this.structureLayout.container.maxX; j++)
			{
				for (int k = this.structureLayout.container.minZ; k <= this.structureLayout.container.maxZ; k++)
				{
					IntVec3 intVec = new IntVec3(j, 0, k);
					int roomIdAt = this.structureLayout.GetRoomIdAt(intVec);
					if (this.structureLayout.IsWallAt(intVec))
					{
						base.AddThing(this.WallThing, at + intVec, Rot4.North, this.GetWallStuff(roomIdAt), 1, null, null, true, 2f);
					}
					if (this.structureLayout.IsFloorAt(intVec) || this.structureLayout.IsDoorAt(intVec) || this.structureLayout.IsWallAt(intVec))
					{
						base.AddTerrain(this.FloorTerrain, at + intVec, true);
					}
					if (this.structureLayout.IsDoorAt(intVec))
					{
						base.AddThing(this.DoorThing, at + intVec, Rot4.North, this.GetDoorStuff(roomIdAt), 1, null, null, true, 2f);
					}
				}
			}
		}

		// Token: 0x040077F5 RID: 30709
		public StructureLayout structureLayout;

		// Token: 0x040077F6 RID: 30710
		public ThingDef wall;

		// Token: 0x040077F7 RID: 30711
		public ThingDef wallStuff;

		// Token: 0x040077F8 RID: 30712
		public ThingDef door;

		// Token: 0x040077F9 RID: 30713
		public ThingDef doorStuff;

		// Token: 0x040077FA RID: 30714
		public TerrainDef floor;

		// Token: 0x040077FB RID: 30715
		public TerrainDef defaultAffordanceTerrain;

		// Token: 0x040077FC RID: 30716
		public ThingDef wallLamp;
	}
}
