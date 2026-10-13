using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ABD RID: 10941
	public class SignalAction_Incident : SignalAction
	{
		// Token: 0x0600F3CC RID: 62412 RVA: 0x0046D440 File Offset: 0x0046B640
		protected override void DoAction(SignalArgs args)
		{
			if (this.incident.Worker.CanFireNow(this.incidentParms))
			{
				this.incident.Worker.TryExecute(this.incidentParms);
			}
		}

		// Token: 0x0600F3CD RID: 62413 RVA: 0x0046D471 File Offset: 0x0046B671
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
		}

		// Token: 0x0400A098 RID: 41112
		public IncidentDef incident;

		// Token: 0x0400A099 RID: 41113
		public IncidentParms incidentParms;
	}
}
