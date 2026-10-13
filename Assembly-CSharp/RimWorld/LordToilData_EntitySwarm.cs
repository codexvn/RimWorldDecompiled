using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F6 RID: 5878
	public class LordToilData_EntitySwarm : LordToilData
	{
		// Token: 0x06008AA3 RID: 35491 RVA: 0x00295814 File Offset: 0x00293A14
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastMoved, "lastMoved", 0, false);
		}

		// Token: 0x04005A44 RID: 23108
		public IntVec3 pos;

		// Token: 0x04005A45 RID: 23109
		public IntVec3 dest;

		// Token: 0x04005A46 RID: 23110
		public int lastMoved;
	}
}
