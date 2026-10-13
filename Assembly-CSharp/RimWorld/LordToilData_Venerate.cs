using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200172D RID: 5933
	public class LordToilData_Venerate : LordToilData
	{
		// Token: 0x06008B95 RID: 35733 RVA: 0x002994A9 File Offset: 0x002976A9
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentNearVeneratorTicks, "currentNearVeneratorTicks", 0, false);
			Scribe_Values.Look<int>(ref this.lastNearVeneratorIndex, "lastNearVeneratorIndex", 0, false);
		}

		// Token: 0x04005AAE RID: 23214
		public int currentNearVeneratorTicks = -1;

		// Token: 0x04005AAF RID: 23215
		public int lastNearVeneratorIndex = -1;
	}
}
