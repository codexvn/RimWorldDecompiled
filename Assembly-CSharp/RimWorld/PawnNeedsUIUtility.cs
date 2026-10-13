using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003230 RID: 12848
	public static class PawnNeedsUIUtility
	{
		// Token: 0x060120F9 RID: 73977 RVA: 0x0054777E File Offset: 0x0054597E
		public static void SortInDisplayOrder(List<Need> needs)
		{
			needs.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
		}

		// Token: 0x060120FA RID: 73978 RVA: 0x005477A8 File Offset: 0x005459A8
		public static Thought GetLeadingThoughtInGroup(List<Thought> thoughtsInGroup)
		{
			Thought result = null;
			int num = -1;
			for (int i = 0; i < thoughtsInGroup.Count; i++)
			{
				if (thoughtsInGroup[i].CurStageIndex > num)
				{
					num = thoughtsInGroup[i].CurStageIndex;
					result = thoughtsInGroup[i];
				}
			}
			return result;
		}

		// Token: 0x060120FB RID: 73979 RVA: 0x005477F0 File Offset: 0x005459F0
		public static void GetThoughtGroupsInDisplayOrder(Need_Mood mood, List<Thought> outThoughtGroupsPresent)
		{
			mood.thoughts.GetDistinctMoodThoughtGroups(outThoughtGroupsPresent);
			for (int i = outThoughtGroupsPresent.Count - 1; i >= 0; i--)
			{
				if (!outThoughtGroupsPresent[i].VisibleInNeedsTab)
				{
					outThoughtGroupsPresent.RemoveAt(i);
				}
			}
			foreach (Thought thought in outThoughtGroupsPresent)
			{
				thought.cachedMoodOffsetOfGroup = mood.thoughts.MoodOffsetOfGroup(thought);
			}
			outThoughtGroupsPresent.SortByDescending((Thought t) => t.cachedMoodOffsetOfGroup, (Thought t) => t.GetHashCode());
		}
	}
}
