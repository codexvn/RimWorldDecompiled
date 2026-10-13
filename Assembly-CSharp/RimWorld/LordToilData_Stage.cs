using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001729 RID: 5929
	public class LordToilData_Stage : LordToilData
	{
		// Token: 0x06008B84 RID: 35716 RVA: 0x0029909C File Offset: 0x0029729C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}

		// Token: 0x04005AA9 RID: 23209
		public IntVec3 stagingPoint;
	}
}
