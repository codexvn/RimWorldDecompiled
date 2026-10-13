using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A81 RID: 6785
	public class StructureLayoutDef : LayoutDef
	{
		// Token: 0x040065EE RID: 26094
		public ThingDef wallDef;

		// Token: 0x040065EF RID: 26095
		public ThingDef wallStuffDef;

		// Token: 0x040065F0 RID: 26096
		public ThingDef doorDef;

		// Token: 0x040065F1 RID: 26097
		public ThingDef doorStuffDef;

		// Token: 0x040065F2 RID: 26098
		public ThingDef wallLampDef;

		// Token: 0x040065F3 RID: 26099
		public float wallLampChancePerPosition = 0.6f;

		// Token: 0x040065F4 RID: 26100
		public ThingDef exteriorDoorDef;

		// Token: 0x040065F5 RID: 26101
		public ThingDef exteriorDoorStuffDef;

		// Token: 0x040065F6 RID: 26102
		public TerrainDef terrainDef;

		// Token: 0x040065F7 RID: 26103
		public TerrainDef defaultAffordanceTerrainDef;

		// Token: 0x040065F8 RID: 26104
		public LayoutRoomDef importantRoomDef;

		// Token: 0x040065F9 RID: 26105
		public float roomToExteriorDoorRatio = 0.33f;

		// Token: 0x040065FA RID: 26106
		public bool spawnDoors = true;

		// Token: 0x040065FB RID: 26107
		public bool clearDoorFaction = true;

		// Token: 0x040065FC RID: 26108
		public bool canConnectRoomsExternally = true;

		// Token: 0x040065FD RID: 26109
		public float areaPrunePercent = 0.3f;

		// Token: 0x040065FE RID: 26110
		public bool canDisconnectRooms = true;

		// Token: 0x040065FF RID: 26111
		public bool ensureOneDoorUnlocked;

		// Token: 0x04006600 RID: 26112
		public LayoutRoomDef corridorDef;

		// Token: 0x04006601 RID: 26113
		public IntRange wallDamageRange = IntRange.Invalid;

		// Token: 0x04006602 RID: 26114
		public CorridorShape corridorShapes = CorridorShape.All;

		// Token: 0x04006603 RID: 26115
		public TerrainDef surroundingTerrainDef;

		// Token: 0x04006604 RID: 26116
		public TerrainDef surroundingScatterTerrainDef;

		// Token: 0x04006605 RID: 26117
		public IntRange surroundingScatterRange = new IntRange(2, 6);

		// Token: 0x04006606 RID: 26118
		public List<ScatteredPrefabs> scatteredPrefabs = new List<ScatteredPrefabs>();
	}
}
