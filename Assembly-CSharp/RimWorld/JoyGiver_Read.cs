using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014F2 RID: 5362
	public class JoyGiver_Read : JoyGiver
	{
		// Token: 0x06007FEA RID: 32746 RVA: 0x00268131 File Offset: 0x00266331
		public override bool CanBeGivenTo(Pawn pawn)
		{
			return BookUtility.CanReadNow(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn, 0.05f) && base.CanBeGivenTo(pawn);
		}

		// Token: 0x06007FEB RID: 32747 RVA: 0x00268154 File Offset: 0x00266354
		public override Job TryGiveJob(Pawn pawn)
		{
			Book t;
			if (BookUtility.TryGetRandomBookToRead(pawn, out t))
			{
				return JobMaker.MakeJob(this.def.jobDef, t);
			}
			return null;
		}
	}
}
