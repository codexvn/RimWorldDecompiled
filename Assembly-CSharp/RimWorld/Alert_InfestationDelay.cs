using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE0 RID: 12256
	public class Alert_InfestationDelay : Alert_ActionDelay
	{
		// Token: 0x06011443 RID: 70723 RVA: 0x004F26AF File Offset: 0x004F08AF
		public Alert_InfestationDelay()
		{
		}

		// Token: 0x06011444 RID: 70724 RVA: 0x004F27B0 File Offset: 0x004F09B0
		public Alert_InfestationDelay(SignalAction_Infestation infestationAction)
		{
			this.infestationAction = infestationAction;
		}

		// Token: 0x06011445 RID: 70725 RVA: 0x004F27C0 File Offset: 0x004F09C0
		public override AlertReport GetReport()
		{
			if (this.infestationAction == null)
			{
				return AlertReport.Inactive;
			}
			if (this.infestationAction.overrideLoc != null)
			{
				return AlertReport.CulpritIs(new GlobalTargetInfo(this.infestationAction.overrideLoc.Value, this.infestationAction.Map, false));
			}
			return AlertReport.Active;
		}

		// Token: 0x06011446 RID: 70726 RVA: 0x004F2819 File Offset: 0x004F0A19
		public override string GetLabel()
		{
			return "AlertInfestationArriving".Translate(this.infestationAction.delayTicks.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x06011447 RID: 70727 RVA: 0x004F2841 File Offset: 0x004F0A41
		public override TaggedString GetExplanation()
		{
			return "AlertInfestationArrivingDesc".Translate();
		}

		// Token: 0x0400B318 RID: 45848
		private SignalAction_Infestation infestationAction;
	}
}
