using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E57 RID: 7767
	public class StorytellerComp_OnOffCycle : StorytellerComp
	{
		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x0600ACB0 RID: 44208 RVA: 0x00314F4F File Offset: 0x0031314F
		protected StorytellerCompProperties_OnOffCycle Props
		{
			get
			{
				return (StorytellerCompProperties_OnOffCycle)this.props;
			}
		}

		// Token: 0x0600ACB1 RID: 44209 RVA: 0x00314F5C File Offset: 0x0031315C
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float num = 1f;
			if (this.Props.acceptFractionByDaysPassedCurve != null)
			{
				num *= this.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedSinceSettleFloat);
			}
			if (this.Props.acceptPercentFactorPerThreatPointsCurve != null)
			{
				num *= this.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
			}
			if (this.Props.acceptPercentFactorPerProgressScoreCurve != null)
			{
				num *= this.Props.acceptPercentFactorPerProgressScoreCurve.Evaluate(StorytellerUtility.GetProgressScore(target));
			}
			float onDays = this.Props.onDays;
			float offDays = this.Props.offDays;
			if ((this.Props.onDaysNoTreeConnectors != 0f || this.Props.offDaysNoTreeConnectors != 0f) && Faction.OfPlayer.ideos != null)
			{
				bool flag = false;
				using (IEnumerator<Ideo> enumerator = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasMeme(MemeDefOf.TreeConnection))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					if (this.Props.onDaysNoTreeConnectors != 0f)
					{
						onDays = this.Props.onDaysNoTreeConnectors;
					}
					if (this.Props.offDaysNoTreeConnectors != 0f)
					{
						offDays = this.Props.offDaysNoTreeConnectors;
					}
				}
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, onDays, offDays, this.Props.minSpacingDays, this.Props.numIncidentsRange.min, this.Props.numIncidentsRange.max, num);
			int num2;
			for (int i = 0; i < incCount; i = num2 + 1)
			{
				FiringIncident firingIncident = this.GenerateIncident(target);
				if (firingIncident != null)
				{
					yield return firingIncident;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600ACB2 RID: 44210 RVA: 0x00314F74 File Offset: 0x00313174
		private FiringIncident GenerateIncident(IIncidentTarget target)
		{
			if (this.Props.IncidentCategory == null)
			{
				return null;
			}
			IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);
			IncidentDef def2;
			if ((float)GenDate.DaysPassedSinceSettle < this.Props.forceRaidEnemyBeforeDaysPassed)
			{
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
				{
					return null;
				}
				def2 = IncidentDefOf.RaidEnemy;
			}
			else if (this.Props.incident != null)
			{
				if (!this.Props.incident.Worker.CanFireNow(parms))
				{
					return null;
				}
				def2 = this.Props.incident;
			}
			else if (!base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms).TryRandomElementByWeight((IncidentDef def) => this.IncidentChanceFinal(def, target), out def2))
			{
				return null;
			}
			return new FiringIncident(def2, this, parms);
		}

		// Token: 0x0600ACB3 RID: 44211 RVA: 0x00315054 File Offset: 0x00313254
		public override string ToString()
		{
			if (this.Props.incident == null && this.Props.IncidentCategory == null)
			{
				return "";
			}
			return base.ToString() + " (" + ((this.Props.incident != null) ? this.Props.incident.defName : this.Props.IncidentCategory.defName) + ")";
		}
	}
}
