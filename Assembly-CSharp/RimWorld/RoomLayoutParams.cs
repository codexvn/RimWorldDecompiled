using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F42 RID: 8002
	public struct RoomLayoutParams
	{
		// Token: 0x04007927 RID: 31015
		public LayoutStructureSketch sketch;

		// Token: 0x04007928 RID: 31016
		public CellRect container;

		// Token: 0x04007929 RID: 31017
		public int minRoomWidth;

		// Token: 0x0400792A RID: 31018
		public int minRoomHeight;

		// Token: 0x0400792B RID: 31019
		public float areaPrunePercent;

		// Token: 0x0400792C RID: 31020
		public IntRange? maxMergeRoomsRange;

		// Token: 0x0400792D RID: 31021
		public int entranceCount;

		// Token: 0x0400792E RID: 31022
		public bool canRemoveRooms;

		// Token: 0x0400792F RID: 31023
		public bool generateDoors;

		// Token: 0x04007930 RID: 31024
		public bool canDisconnectRooms;

		// Token: 0x04007931 RID: 31025
		public LayoutRoomDef corridor;

		// Token: 0x04007932 RID: 31026
		public int corridorExpansion;

		// Token: 0x04007933 RID: 31027
		public CorridorShape corridorShapes;

		// Token: 0x04007934 RID: 31028
		public bool singleRoom;
	}
}
