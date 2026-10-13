using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166F RID: 5743
	public class LordToilData_SanguophageMeeting : LordToilData
	{
		// Token: 0x06008761 RID: 34657 RVA: 0x00286ED0 File Offset: 0x002850D0
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksInMeeting, "ticksInMeeting", 0, false);
		}

		// Token: 0x0400586C RID: 22636
		public int ticksInMeeting;
	}
}
