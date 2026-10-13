using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E1 RID: 14305
	public class LayoutThing
	{
		// Token: 0x0400DB9E RID: 56222
		public ThingDef thing;

		// Token: 0x0400DB9F RID: 56223
		public IntVec3 offset;

		// Token: 0x0400DBA0 RID: 56224
		public Rot4 rotation;

		// Token: 0x0400DBA1 RID: 56225
		public ThingDef filthDef;

		// Token: 0x0400DBA2 RID: 56226
		public int filthExpandBy;

		// Token: 0x0400DBA3 RID: 56227
		public float filthChance = 0.5f;
	}
}
