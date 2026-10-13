using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B38 RID: 11064
	public class Plant_Psilocap : Plant
	{
		// Token: 0x0600F77C RID: 63356 RVA: 0x0047F0EC File Offset: 0x0047D2EC
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.Destroyed)
			{
				return;
			}
			for (int i = 0; i < base.Map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = base.Map.mapPawns.AllPawnsSpawned[i];
				if (pawn.Position.DistanceTo(base.Position) <= 2f)
				{
					RaceProperties raceProps = pawn.RaceProps;
					if ((raceProps.Humanlike || raceProps.Animal) && raceProps.IsFlesh)
					{
						Hediff orAddHediff = pawn.health.GetOrAddHediff(HediffDefOf.PsilocapHigh, null, null, null);
						if (orAddHediff.Severity <= 0.2f)
						{
							orAddHediff.Severity += 0.00033333333f * (float)delta;
						}
					}
				}
			}
		}

		// Token: 0x0400A2D8 RID: 41688
		private const float HediffSeverityIncreasePerSecond = 0.02f;

		// Token: 0x0400A2D9 RID: 41689
		private const float HediffSeverityIncreasePerTick = 0.00033333333f;

		// Token: 0x0400A2DA RID: 41690
		private const float HediffSeverityMaxFromAmbient = 0.2f;
	}
}
