using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001719 RID: 5913
	public class LordToilData_ReimplantXenogerm : LordToilData
	{
		// Token: 0x06008B3D RID: 35645 RVA: 0x00297B08 File Offset: 0x00295D08
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.waitSpot, "waitSpot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x04005A7A RID: 23162
		public IntVec3 waitSpot;

		// Token: 0x04005A7B RID: 23163
		public Pawn target;
	}
}
