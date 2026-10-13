using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017DD RID: 6109
	public class ThinkNode_Priority_Learn : ThinkNode_Priority
	{
		// Token: 0x06008E16 RID: 36374 RVA: 0x002A2240 File Offset: 0x002A0440
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Pawn_TimetableTracker timetable = pawn.timetable;
			if (!(((timetable != null) ? timetable.CurrentAssignment : null) ?? TimeAssignmentDefOf.Anything).allowJoy)
			{
				return 0f;
			}
			if (pawn.learning == null)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < 5000)
			{
				return 0f;
			}
			if (!pawn.DevelopmentalStage.Child())
			{
				return 0f;
			}
			if (LearningUtility.LearningSatisfied(pawn))
			{
				return 0f;
			}
			return 9.1f;
		}

		// Token: 0x04005BEC RID: 23532
		private const int GameStartNoLearningTicks = 5000;
	}
}
