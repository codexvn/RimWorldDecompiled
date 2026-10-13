using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AA5 RID: 15013
	public class QuestNode_IsFlesh : QuestNode_RaceProperty
	{
		// Token: 0x060147DA RID: 83930 RVA: 0x0061E14E File Offset: 0x0061C34E
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsFlesh;
		}
	}
}
