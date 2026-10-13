using System;

namespace Verse
{
	// Token: 0x02000172 RID: 370
	public class PawnRitualReference : IExposable
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x000024DF File Offset: 0x000006DF
		public PawnRitualReference()
		{
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0003241F File Offset: 0x0003061F
		public PawnRitualReference(Pawn p)
		{
			this.pawn = p;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0003242E File Offset: 0x0003062E
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x040005A1 RID: 1441
		public Pawn pawn;
	}
}
