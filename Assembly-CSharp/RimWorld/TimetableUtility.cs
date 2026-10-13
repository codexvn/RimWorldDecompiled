using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020024E3 RID: 9443
	public static class TimetableUtility
	{
		// Token: 0x0600CE3F RID: 52799 RVA: 0x003C8E64 File Offset: 0x003C7064
		public static TimeAssignmentDef GetTimeAssignment(this Pawn pawn)
		{
			if (pawn.timetable == null)
			{
				return TimeAssignmentDefOf.Anything;
			}
			return pawn.timetable.CurrentAssignment;
		}
	}
}
