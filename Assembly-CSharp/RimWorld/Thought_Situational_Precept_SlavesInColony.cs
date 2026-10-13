using System;

namespace RimWorld
{
	// Token: 0x02001804 RID: 6148
	public class Thought_Situational_Precept_SlavesInColony : Thought_Situational
	{
		// Token: 0x06008E92 RID: 36498 RVA: 0x002A3730 File Offset: 0x002A1930
		public override float MoodOffset()
		{
			return this.BaseMoodOffset * (float)FactionUtility.GetSlavesInFactionCount(this.pawn.Faction);
		}
	}
}
