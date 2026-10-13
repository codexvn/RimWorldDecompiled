using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183B RID: 6203
	public class ThoughtWorker_Precept_NoPsychicRituals : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06008F46 RID: 36678 RVA: 0x002A5724 File Offset: 0x002A3924
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!p.IsColonist)
			{
				return false;
			}
			return Find.TickManager.TicksGame > Find.IdeoManager.lastPsychicRitualPerformedTick + 360000;
		}

		// Token: 0x06008F47 RID: 36679 RVA: 0x002A5764 File Offset: 0x002A3964
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 360000.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION");
			yield break;
		}

		// Token: 0x04005C27 RID: 23591
		public const int TicksSinceLastPsychicRitualActiveTicks = 360000;
	}
}
