using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001517 RID: 5399
	public class LearningGiver
	{
		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x060080A2 RID: 32930 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanGiveDesire
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060080A3 RID: 32931 RVA: 0x0026C2AC File Offset: 0x0026A4AC
		public virtual bool CanDo(Pawn pawn)
		{
			return ModsConfig.BiotechActive && (pawn.DevelopmentalStage.Child() && !pawn.Downed && !PawnUtility.WillSoonHaveBasicNeed(pawn, 0.1f) && pawn.needs.learning != null) && pawn.needs.learning.CurLevel < 0.9f;
		}

		// Token: 0x060080A4 RID: 32932 RVA: 0x0026C30A File Offset: 0x0026A50A
		public virtual Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(this.def.jobDef);
		}

		// Token: 0x040055FB RID: 22011
		public const float NeedThresholdOffsetStart = 0.1f;

		// Token: 0x040055FC RID: 22012
		public const float NeedThresholdOffsetStop = -0.05f;

		// Token: 0x040055FD RID: 22013
		public LearningDesireDef def;
	}
}
