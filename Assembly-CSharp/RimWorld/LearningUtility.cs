using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001522 RID: 5410
	public static class LearningUtility
	{
		// Token: 0x060080CB RID: 32971 RVA: 0x0026CB5F File Offset: 0x0026AD5F
		public static float SchoolDeskLearningRate(Thing schoolDesk)
		{
			return 1f + (float)LearningUtility.ConnectedBlackboards(schoolDesk) * 0.2f;
		}

		// Token: 0x060080CC RID: 32972 RVA: 0x0026CB74 File Offset: 0x0026AD74
		public static int ConnectedBlackboards(Thing desk)
		{
			int num = 0;
			CompAffectedByFacilities compAffectedByFacilities = desk.TryGetComp<CompAffectedByFacilities>();
			if (compAffectedByFacilities != null)
			{
				using (List<Thing>.Enumerator enumerator = compAffectedByFacilities.LinkedFacilitiesListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def == ThingDefOf.Blackboard)
						{
							num++;
							if (num >= 3)
							{
								break;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060080CD RID: 32973 RVA: 0x0026CBE4 File Offset: 0x0026ADE4
		public static float LearningRateFactor(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			float num = pawn.GetStatValue(StatDefOf.LearningRateFactor, true, -1);
			if (curJob.targetA.HasThing && curJob.targetA.Thing.def == ThingDefOf.SchoolDesk)
			{
				num *= LearningUtility.SchoolDeskLearningRate(curJob.targetA.Thing);
			}
			return num;
		}

		// Token: 0x060080CE RID: 32974 RVA: 0x0026CC3E File Offset: 0x0026AE3E
		public static bool LearningTickCheckEnd(Pawn pawn, int delta, bool forced = false)
		{
			pawn.needs.learning.Learn(1.2E-05f * LearningUtility.LearningRateFactor(pawn) * (float)delta);
			if (LearningUtility.LearningSatisfied(pawn) && !forced)
			{
				pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				return true;
			}
			return false;
		}

		// Token: 0x060080CF RID: 32975 RVA: 0x0026CC7B File Offset: 0x0026AE7B
		public static bool LearningSatisfied(Pawn pawn)
		{
			return pawn.needs.learning.CurLevel >= 0.999f;
		}

		// Token: 0x04005607 RID: 22023
		public const float NeedSatisfiedPerTick = 1.2E-05f;

		// Token: 0x04005608 RID: 22024
		public const float StartJobMaxLearning = 0.9f;

		// Token: 0x04005609 RID: 22025
		private const float EndJobMaxLearning = 0.999f;

		// Token: 0x0400560A RID: 22026
		public const float LearningRateBonusOffset_Blackboard = 0.2f;

		// Token: 0x0400560B RID: 22027
		public const int MaxConnectedBlackboards = 3;
	}
}
