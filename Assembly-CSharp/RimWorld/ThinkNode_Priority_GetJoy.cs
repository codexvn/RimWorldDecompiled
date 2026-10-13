using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017DC RID: 6108
	public class ThinkNode_Priority_GetJoy : ThinkNode_Priority
	{
		// Token: 0x06008E14 RID: 36372 RVA: 0x002A2158 File Offset: 0x002A0358
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < 5000)
			{
				return 0f;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn))
			{
				return 0f;
			}
			float curLevel = pawn.needs.joy.CurLevel;
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment;
			if (!timeAssignmentDef.allowJoy)
			{
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				if (curLevel < 0.35f)
				{
					return 6f;
				}
				return 0f;
			}
			else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				if (curLevel < 0.95f)
				{
					return 7f;
				}
				return 0f;
			}
			else if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				if (curLevel < 0.95f)
				{
					return 2f;
				}
				return 0f;
			}
			else
			{
				if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
				{
					return 0f;
				}
				throw new NotImplementedException();
			}
		}

		// Token: 0x04005BEB RID: 23531
		private const int GameStartNoJoyTicks = 5000;
	}
}
