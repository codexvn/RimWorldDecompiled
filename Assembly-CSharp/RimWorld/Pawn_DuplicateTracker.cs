using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020024B8 RID: 9400
	public class Pawn_DuplicateTracker : IExposable
	{
		// Token: 0x0600CC4A RID: 52298 RVA: 0x003BDC83 File Offset: 0x003BBE83
		public Pawn_DuplicateTracker()
		{
		}

		// Token: 0x0600CC4B RID: 52299 RVA: 0x003BDC96 File Offset: 0x003BBE96
		public Pawn_DuplicateTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CC4C RID: 52300 RVA: 0x003BDCB0 File Offset: 0x003BBEB0
		public void Notify_PawnKilled()
		{
			Find.PawnDuplicator.RemoveDuplicate(this.duplicateOf, this.pawn);
		}

		// Token: 0x0600CC4D RID: 52301 RVA: 0x003BDCC8 File Offset: 0x003BBEC8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.duplicateOf, "duplicateOf", int.MinValue, false);
		}

		// Token: 0x04008B41 RID: 35649
		private Pawn pawn;

		// Token: 0x04008B42 RID: 35650
		public int duplicateOf = int.MinValue;
	}
}
