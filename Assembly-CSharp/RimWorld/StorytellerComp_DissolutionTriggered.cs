using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E41 RID: 7745
	public class StorytellerComp_DissolutionTriggered : StorytellerComp
	{
		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x0600AC52 RID: 44114 RVA: 0x00314043 File Offset: 0x00312243
		private StorytellerCompProperties_DissolutionTriggered Props
		{
			get
			{
				return (StorytellerCompProperties_DissolutionTriggered)this.props;
			}
		}

		// Token: 0x0600AC53 RID: 44115 RVA: 0x00314050 File Offset: 0x00312250
		public override void Notify_DissolutionEvent(Thing thing)
		{
			if (thing.def == this.Props.thing)
			{
				IncidentParms incidentParms = new IncidentParms
				{
					target = thing.MapHeld,
					spawnCenter = thing.Position
				};
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target);
				if (Rand.Chance(base.IncidentChanceFinal(this.Props.incident, incidentParms.target)) && this.Props.incident.Worker.CanFireNow(incidentParms))
				{
					Find.Storyteller.incidentQueue.Add(this.Props.incident, Find.TickManager.TicksGame + this.Props.delayTicks, incidentParms, 0);
				}
			}
		}
	}
}
