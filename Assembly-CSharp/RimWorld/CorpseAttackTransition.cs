using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002908 RID: 10504
	public class CorpseAttackTransition : MusicTransition
	{
		// Token: 0x0600E5DD RID: 58845 RVA: 0x0043119F File Offset: 0x0042F39F
		public override bool IsTransitionSatisfied()
		{
			return ModsConfig.AnomalyActive && base.IsTransitionSatisfied() && Find.Anomaly.HasActiveAwokenCorpse();
		}
	}
}
