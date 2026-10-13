using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001806 RID: 6150
	public class ThoughtWorker_Precept_Slavery_NoSlavesInColony : ThoughtWorker_Precept
	{
		// Token: 0x06008E96 RID: 36502 RVA: 0x002A3782 File Offset: 0x002A1982
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.IsColonist && !p.IsSlave && !p.IsPrisoner && FactionUtility.GetSlavesInFactionCount(p.Faction) <= 0;
		}
	}
}
