using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001537 RID: 5431
	public abstract class BabyPlayGiver
	{
		// Token: 0x06008105 RID: 33029 RVA: 0x000024DF File Offset: 0x000006DF
		public BabyPlayGiver()
		{
		}

		// Token: 0x06008106 RID: 33030
		public abstract bool CanDo(Pawn pawn, Pawn baby);

		// Token: 0x06008107 RID: 33031 RVA: 0x0026D945 File Offset: 0x0026BB45
		public virtual Job TryGiveJob(Pawn pawn, Pawn baby)
		{
			Job job = JobMaker.MakeJob(this.def.jobDef, baby);
			job.count = 1;
			return job;
		}

		// Token: 0x0400561F RID: 22047
		public BabyPlayDef def;
	}
}
