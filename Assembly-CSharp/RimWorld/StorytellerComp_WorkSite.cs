using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E78 RID: 7800
	public class StorytellerComp_WorkSite : StorytellerComp
	{
		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x0600AD30 RID: 44336 RVA: 0x003166F0 File Offset: 0x003148F0
		public StorytellerCompProperties_WorkSite Props
		{
			get
			{
				return (StorytellerCompProperties_WorkSite)this.props;
			}
		}

		// Token: 0x0600AD31 RID: 44337 RVA: 0x003166FD File Offset: 0x003148FD
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Find.TickManager.TicksGame < 600000)
			{
				yield break;
			}
			float num = QuestNode_Root_WorkSite.BestAppearanceFrequency();
			if (this.Props.incident == null || !this.Props.incident.TargetAllowed(target) || num <= 0f)
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(this.Props.baseMtbDays / num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x0600AD32 RID: 44338 RVA: 0x00316714 File Offset: 0x00314914
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentDef incident = this.Props.incident;
			return str + str2 + ((incident != null) ? incident.ToString() : null);
		}
	}
}
