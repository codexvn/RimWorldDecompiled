using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003034 RID: 12340
	public class Alert_TimedRaidsArriving : Alert
	{
		// Token: 0x0601158E RID: 71054 RVA: 0x004F9EE4 File Offset: 0x004F80E4
		private void CalculateTargets()
		{
			this.timedRaidsArrivingSoon.Clear();
			this.targets.Clear();
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				foreach (WorldObjectComp worldObjectComp in worldObject.AllComps)
				{
					TimedDetectionRaids timedDetectionRaids = worldObjectComp as TimedDetectionRaids;
					if (timedDetectionRaids != null && this.ShouldAlertTimedRaids(timedDetectionRaids))
					{
						this.timedRaidsArrivingSoon.Add(timedDetectionRaids);
						if (!this.targets.Contains(worldObject))
						{
							this.targets.Add(worldObject);
						}
					}
				}
			}
		}

		// Token: 0x0601158F RID: 71055 RVA: 0x004F9FCC File Offset: 0x004F81CC
		private bool ShouldAlertTimedRaids(TimedDetectionRaids timedDetectionRaids)
		{
			return timedDetectionRaids.alertRaidsArrivingIn && timedDetectionRaids.DetectionCountdownStarted && timedDetectionRaids.RaidsSentCount == 0;
		}

		// Token: 0x06011590 RID: 71056 RVA: 0x004F9FEC File Offset: 0x004F81EC
		public override string GetLabel()
		{
			return "AlertTimedRaidsArrivingIn".Translate(this.timedRaidsArrivingSoon.MinBy((TimedDetectionRaids t) => t.TicksLeftToSendRaids).TicksLeftToSendRaids.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x06011591 RID: 71057 RVA: 0x004FA044 File Offset: 0x004F8244
		public override TaggedString GetExplanation()
		{
			return "AlertTimedRaidsArrivingInDesc".Translate((from t in this.timedRaidsArrivingSoon
			select "AlertTimedRaidsArrivingAt".Translate(t.parent.Label, t.TicksLeftToSendRaids.ToStringTicksToPeriodVerbose(true, true)).Resolve()).ToLineList("  - ", false));
		}

		// Token: 0x06011592 RID: 71058 RVA: 0x004FA095 File Offset: 0x004F8295
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3A8 RID: 45992
		private List<TimedDetectionRaids> timedRaidsArrivingSoon = new List<TimedDetectionRaids>();

		// Token: 0x0400B3A9 RID: 45993
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
