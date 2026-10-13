using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200290A RID: 10506
	public class HorrorMonolithAdvancedTransition : MusicTransition
	{
		// Token: 0x0600E5E4 RID: 58852 RVA: 0x00431323 File Offset: 0x0042F523
		public override bool IsTransitionSatisfied()
		{
			return ModsConfig.AnomalyActive && base.IsTransitionSatisfied() && Find.Anomaly.Level > 0 && Find.Anomaly.TicksSinceLastLevelChange <= 3600;
		}

		// Token: 0x040099C2 RID: 39362
		private const int TransitionTicksDuration = 3600;
	}
}
