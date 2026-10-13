using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200151F RID: 5407
	public class LearningGiver_Reading : LearningGiver
	{
		// Token: 0x060080C1 RID: 32961 RVA: 0x0026C8AC File Offset: 0x0026AAAC
		public override bool CanDo(Pawn pawn)
		{
			Book book;
			return base.CanDo(pawn) && BookUtility.CanReadNow(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn, 0.05f) && BookUtility.TryGetRandomBookToRead(pawn, out book);
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x0026C8E4 File Offset: 0x0026AAE4
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
