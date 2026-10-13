using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200180E RID: 6158
	public class ThoughtWorker_Precept_NoRecentHumanMeat : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06008EBE RID: 36542 RVA: 0x002A3EC8 File Offset: 0x002A20C8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			if (((needs != null) ? needs.food : null) == null)
			{
				return false;
			}
			int num = Mathf.Max(0, p.mindState.lastHumanMeatIngestedTick);
			return Find.TickManager.TicksGame - num > 480000;
		}

		// Token: 0x06008EBF RID: 36543 RVA: 0x002A3F1A File Offset: 0x002A211A
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 8.Named("HUMANMEATREQUIREDINTERVAL");
			yield break;
		}

		// Token: 0x04005C11 RID: 23569
		public const int MinDaysSinceLastHumanMeatForThought = 8;
	}
}
