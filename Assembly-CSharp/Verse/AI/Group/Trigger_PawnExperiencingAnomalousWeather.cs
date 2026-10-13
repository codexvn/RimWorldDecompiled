using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010E5 RID: 4325
	public class Trigger_PawnExperiencingAnomalousWeather : Trigger
	{
		// Token: 0x0600694C RID: 26956 RVA: 0x00213048 File Offset: 0x00211248
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodRage, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity > this.weatherHediffThreshold)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04004A7F RID: 19071
		private float weatherHediffThreshold = 0.15f;
	}
}
