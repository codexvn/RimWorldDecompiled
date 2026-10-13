using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A75 RID: 6773
	public class RoomPart_BarricadeDef : RoomPartDef
	{
		// Token: 0x060097FD RID: 38909 RVA: 0x002C5BF2 File Offset: 0x002C3DF2
		public RoomPart_BarricadeDef()
		{
			this.workerClass = typeof(RoomPart_Barricades);
		}

		// Token: 0x040065AC RID: 26028
		public ThingDef wallDef;

		// Token: 0x040065AD RID: 26029
		public ThingDef stuffDef;

		// Token: 0x040065AE RID: 26030
		public int steps = 2;

		// Token: 0x040065AF RID: 26031
		public int offset = 2;

		// Token: 0x040065B0 RID: 26032
		public float chancePerDoor = 0.75f;
	}
}
