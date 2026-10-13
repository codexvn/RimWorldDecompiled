using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002460 RID: 9312
	public class AssignedMech : IExposable
	{
		// Token: 0x0600C9D9 RID: 51673 RVA: 0x000024DF File Offset: 0x000006DF
		public AssignedMech()
		{
		}

		// Token: 0x0600C9DA RID: 51674 RVA: 0x003B165B File Offset: 0x003AF85B
		public AssignedMech(Pawn pawn)
		{
			this.pawn = pawn;
			this.tickAssigned = Find.TickManager.TicksGame;
		}

		// Token: 0x0600C9DB RID: 51675 RVA: 0x003B167A File Offset: 0x003AF87A
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.tickAssigned, "tickAssigned", 0, false);
		}

		// Token: 0x0400896E RID: 35182
		public Pawn pawn;

		// Token: 0x0400896F RID: 35183
		public int tickAssigned;
	}
}
