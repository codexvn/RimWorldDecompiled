using System;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200373B RID: 14139
	public class StatWorker_CaravanRidingSpeedFactor : StatWorker
	{
		// Token: 0x060139BB RID: 80315 RVA: 0x005D290C File Offset: 0x005D0B0C
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.HasThing && req.Thing.def.race != null)
			{
				stringBuilder.Append("StatsReport_CaravanRideableAge".Translate()).Append(": ").Append(CaravanRideableUtility.RideableLifeStagesDesc(req.Thing.def.race)).AppendLine();
			}
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			return stringBuilder.ToString();
		}
	}
}
