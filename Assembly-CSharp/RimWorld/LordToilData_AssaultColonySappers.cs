using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016E6 RID: 5862
	public class LordToilData_AssaultColonySappers : LordToilData
	{
		// Token: 0x06008A63 RID: 35427 RVA: 0x0029480C File Offset: 0x00292A0C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sapperDest, "sapperDest", default(IntVec3), false);
		}

		// Token: 0x04005A30 RID: 23088
		public IntVec3 sapperDest = IntVec3.Invalid;
	}
}
