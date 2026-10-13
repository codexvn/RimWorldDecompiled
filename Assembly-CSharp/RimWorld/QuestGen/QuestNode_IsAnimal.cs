using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AA4 RID: 15012
	public class QuestNode_IsAnimal : QuestNode_RaceProperty
	{
		// Token: 0x060147D8 RID: 83928 RVA: 0x0061E13E File Offset: 0x0061C33E
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Animal;
		}
	}
}
