using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E43 RID: 7747
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x0600AC56 RID: 44118 RVA: 0x00314123 File Offset: 0x00312323
		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		// Token: 0x0600AC57 RID: 44119 RVA: 0x00314130 File Offset: 0x00312330
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = target as Map;
			if (this.Props.minDanger != StoryDanger.None && (map == null || map.dangerWatcher.DangerRating < this.Props.minDanger))
			{
				yield break;
			}
			if (this.Props.minWealth > 0f && (map == null || map.wealthWatcher.WealthTotal < this.Props.minWealth))
			{
				yield break;
			}
			float num = StorytellerUtility.AllyIncidentFraction(this.Props.fullAlliesOnly);
			if (num <= 0f)
			{
				yield break;
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, 60f, 0f, this.Props.minSpacingDays, this.Props.baseIncidentsPerYear, this.Props.baseIncidentsPerYear, num);
			int num2;
			for (int i = 0; i < incCount; i = num2 + 1)
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600AC58 RID: 44120 RVA: 0x00314147 File Offset: 0x00312347
		public override string ToString()
		{
			return base.ToString() + " (" + this.Props.incident.defName + ")";
		}
	}
}
