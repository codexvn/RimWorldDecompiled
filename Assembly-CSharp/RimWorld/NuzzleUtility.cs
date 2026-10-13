using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002403 RID: 9219
	public static class NuzzleUtility
	{
		// Token: 0x0600C7BA RID: 51130 RVA: 0x003A4210 File Offset: 0x003A2410
		public static float GetNuzzleMTBHours(Pawn pawn)
		{
			float num = pawn.RaceProps.nuzzleMtbHours;
			if (ModsConfig.OdysseyActive)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Comfort))
				{
					num *= 0.75f;
				}
			}
			return num;
		}

		// Token: 0x0600C7BB RID: 51131 RVA: 0x003A4252 File Offset: 0x003A2452
		public static int GetNuzzleStageIndex(Pawn pawn)
		{
			if (ModsConfig.OdysseyActive)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Comfort))
				{
					return 1;
				}
			}
			return 0;
		}
	}
}
