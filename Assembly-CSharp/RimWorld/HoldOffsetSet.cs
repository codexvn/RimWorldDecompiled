using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199F RID: 6559
	public class HoldOffsetSet
	{
		// Token: 0x06009467 RID: 37991 RVA: 0x002B96DC File Offset: 0x002B78DC
		public HoldOffset Pick(Rot4 rotation)
		{
			if (rotation == Rot4.North)
			{
				return this.northDefault;
			}
			if (rotation == Rot4.East)
			{
				return this.east;
			}
			if (rotation == Rot4.South)
			{
				return this.south;
			}
			if (rotation == Rot4.West)
			{
				return this.west;
			}
			return null;
		}

		// Token: 0x04006042 RID: 24642
		public HoldOffset northDefault;

		// Token: 0x04006043 RID: 24643
		public HoldOffset east;

		// Token: 0x04006044 RID: 24644
		public HoldOffset south;

		// Token: 0x04006045 RID: 24645
		public HoldOffset west;
	}
}
