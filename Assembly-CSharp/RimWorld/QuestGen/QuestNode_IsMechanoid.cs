using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AA7 RID: 15015
	public class QuestNode_IsMechanoid : QuestNode_RaceProperty
	{
		// Token: 0x060147DE RID: 83934 RVA: 0x0061E15E File Offset: 0x0061C35E
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsMechanoid;
		}
	}
}
