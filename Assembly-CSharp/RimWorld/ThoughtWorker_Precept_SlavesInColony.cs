using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001805 RID: 6149
	public class ThoughtWorker_Precept_SlavesInColony : ThoughtWorker_Precept
	{
		// Token: 0x06008E94 RID: 36500 RVA: 0x002A3752 File Offset: 0x002A1952
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.IsColonist && !p.IsSlave && !p.IsPrisoner && FactionUtility.GetSlavesInFactionCount(p.Faction) > 0;
		}
	}
}
