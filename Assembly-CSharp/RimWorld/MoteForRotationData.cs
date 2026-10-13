using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B8 RID: 6584
	public class MoteForRotationData
	{
		// Token: 0x060094DB RID: 38107 RVA: 0x002BA8F0 File Offset: 0x002B8AF0
		public ThingDef GetForRotation(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.north;
			case 1:
				return this.east;
			case 2:
				return this.south;
			case 3:
				return this.west;
			default:
				return null;
			}
		}

		// Token: 0x040060A8 RID: 24744
		public ThingDef north;

		// Token: 0x040060A9 RID: 24745
		public ThingDef south;

		// Token: 0x040060AA RID: 24746
		public ThingDef east;

		// Token: 0x040060AB RID: 24747
		public ThingDef west;
	}
}
