using System;

namespace Verse.AI.Group
{
	// Token: 0x020010A8 RID: 4264
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x060068B2 RID: 26802 RVA: 0x00211250 File Offset: 0x0020F450
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}

		// Token: 0x04004A3C RID: 19004
		public IntVec3 dest;
	}
}
