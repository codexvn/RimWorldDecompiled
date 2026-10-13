using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AA6 RID: 15014
	public class QuestNode_IsHumanlike : QuestNode_RaceProperty
	{
		// Token: 0x060147DC RID: 83932 RVA: 0x0061E156 File Offset: 0x0061C356
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Humanlike;
		}
	}
}
