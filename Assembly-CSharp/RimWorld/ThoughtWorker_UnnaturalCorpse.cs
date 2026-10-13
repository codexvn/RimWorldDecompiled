using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EF RID: 6383
	public class ThoughtWorker_UnnaturalCorpse : ThoughtWorker
	{
		// Token: 0x0600912D RID: 37165 RVA: 0x002AB4A4 File Offset: 0x002A96A4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(p, out unnaturalCorpseTracker))
			{
				return false;
			}
			return ThoughtState.ActiveDefault;
		}
	}
}
