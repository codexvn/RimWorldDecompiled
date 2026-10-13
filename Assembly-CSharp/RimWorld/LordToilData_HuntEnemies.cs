using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001708 RID: 5896
	public class LordToilData_HuntEnemies : LordToilData
	{
		// Token: 0x06008AE8 RID: 35560 RVA: 0x00296B88 File Offset: 0x00294D88
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}

		// Token: 0x04005A5C RID: 23132
		public IntVec3 fallbackLocation = IntVec3.Invalid;
	}
}
