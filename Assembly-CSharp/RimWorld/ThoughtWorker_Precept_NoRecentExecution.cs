using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200180C RID: 6156
	public class ThoughtWorker_Precept_NoRecentExecution : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06008EB3 RID: 36531 RVA: 0x002A3DB0 File Offset: 0x002A1FB0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist || p.IsSlave)
			{
				return false;
			}
			int num = Mathf.Max(0, p.Faction.lastExecutionTick);
			return Find.TickManager.TicksGame - num > 1800000;
		}

		// Token: 0x06008EB4 RID: 36532 RVA: 0x002A3DFE File Offset: 0x002A1FFE
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 30.Named("MINDAYSLASTEXECUTION");
			yield break;
		}

		// Token: 0x04005C0D RID: 23565
		private const int MinDaysSinceLastExecutionForThought = 30;
	}
}
