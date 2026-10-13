using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200251D RID: 9501
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x0600CF9E RID: 53150 RVA: 0x000024DF File Offset: 0x000006DF
		public DirectPawnRelation()
		{
		}

		// Token: 0x0600CF9F RID: 53151 RVA: 0x003CF6E1 File Offset: 0x003CD8E1
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x0600CFA0 RID: 53152 RVA: 0x003CF6FE File Offset: 0x003CD8FE
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
		}

		// Token: 0x04008D5C RID: 36188
		public PawnRelationDef def;

		// Token: 0x04008D5D RID: 36189
		public Pawn otherPawn;

		// Token: 0x04008D5E RID: 36190
		public int startTicks;
	}
}
