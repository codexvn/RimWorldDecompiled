using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E46 RID: 7750
	public class StorytellerComp_GauranlenPodSpawn : StorytellerComp
	{
		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x0600AC63 RID: 44131 RVA: 0x00314396 File Offset: 0x00312596
		protected StorytellerCompProperties_GauranlenPodSpawn Props
		{
			get
			{
				return (StorytellerCompProperties_GauranlenPodSpawn)this.props;
			}
		}

		// Token: 0x0600AC64 RID: 44132 RVA: 0x003143A3 File Offset: 0x003125A3
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!ModsConfig.IdeologyActive || this.Props.daysBetweenPodSpawns == 0f || Faction.OfPlayer.ideos == null)
			{
				yield break;
			}
			if ((float)GenDate.DaysPassed < this.Props.minDaysPassed)
			{
				yield break;
			}
			int num = 1;
			if (this.Props.countdownFactorAnyConnectors > 1)
			{
				using (IEnumerator<Ideo> enumerator = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasMeme(MemeDefOf.TreeConnection))
						{
							num = this.Props.countdownFactorAnyConnectors;
							break;
						}
					}
				}
			}
			Find.IdeoManager.ticksToNextGauranlenSpawn -= 1000 * num;
			if (Find.IdeoManager.ticksToNextGauranlenSpawn <= 0)
			{
				Find.IdeoManager.ticksToNextGauranlenSpawn = (int)(this.Props.daysBetweenPodSpawns * 60000f);
				yield return new FiringIncident(this.Props.incident, this, this.GenerateParms(this.Props.incident.category, target));
			}
			yield break;
		}
	}
}
