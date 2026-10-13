using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E76 RID: 7798
	public class StorytellerComp_Triggered : StorytellerComp
	{
		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x0600AD2B RID: 44331 RVA: 0x0031654F File Offset: 0x0031474F
		private StorytellerCompProperties_Triggered Props
		{
			get
			{
				return (StorytellerCompProperties_Triggered)this.props;
			}
		}

		// Token: 0x0600AD2C RID: 44332 RVA: 0x0031655C File Offset: 0x0031475C
		public override void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist)
			{
				return;
			}
			if (ev == AdaptationEvent.Died || ev == AdaptationEvent.Kidnapped || ev == AdaptationEvent.LostBecauseMapClosed || ev == AdaptationEvent.Downed)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
				{
					if (pawn.RaceProps.Humanlike && !pawn.IsPrisoner)
					{
						if (pawn != p && !pawn.Downed)
						{
							return;
						}
						if (ModsConfig.BiotechActive && pawn.Deathresting && !SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(pawn))
						{
							return;
						}
					}
				}
				if (ModsConfig.AnomalyActive && (DeathRefusalUtility.HasPlayerControlledDeathRefusal(p) || DeathRefusalUtility.PlayerHasCorpseWithDeathRefusal()))
				{
					return;
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.Props.incident.category, anyPlayerHomeMap);
					if (this.Props.incident.Worker.CanFireNow(parms))
					{
						QueuedIncident qi = new QueuedIncident(new FiringIncident(this.Props.incident, this, parms), Find.TickManager.TicksGame + this.Props.delayTicks, 0);
						Find.Storyteller.incidentQueue.Add(qi);
					}
				}
			}
		}

		// Token: 0x0600AD2D RID: 44333 RVA: 0x003166A4 File Offset: 0x003148A4
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentDef incident = this.Props.incident;
			return str + str2 + ((incident != null) ? incident.ToString() : null);
		}
	}
}
