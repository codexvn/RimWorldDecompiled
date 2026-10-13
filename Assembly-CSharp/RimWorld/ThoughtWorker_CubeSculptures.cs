using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F0 RID: 6384
	public class ThoughtWorker_CubeSculptures : ThoughtWorker
	{
		// Token: 0x0600912F RID: 37167 RVA: 0x002AB4DC File Offset: 0x002A96DC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Hediff_CubeInterest hediff_CubeInterest;
			if (!p.health.hediffSet.TryGetHediff<Hediff_CubeInterest>(out hediff_CubeInterest))
			{
				return false;
			}
			return hediff_CubeInterest.Sculptures > 0;
		}

		// Token: 0x06009130 RID: 37168 RVA: 0x002AB520 File Offset: 0x002A9720
		public override float MoodMultiplier(Pawn p)
		{
			if (!p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
			{
				return 1f;
			}
			return (float)p.health.hediffSet.GetFirstHediff<Hediff_CubeInterest>().Sculptures;
		}
	}
}
