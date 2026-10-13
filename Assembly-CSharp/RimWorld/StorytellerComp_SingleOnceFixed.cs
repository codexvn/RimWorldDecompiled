using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E6F RID: 7791
	public class StorytellerComp_SingleOnceFixed : StorytellerComp
	{
		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x0600AD10 RID: 44304 RVA: 0x0031376A File Offset: 0x0031196A
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x0600AD11 RID: 44305 RVA: 0x003160E2 File Offset: 0x003142E2
		private StorytellerCompProperties_SingleOnceFixed Props
		{
			get
			{
				return (StorytellerCompProperties_SingleOnceFixed)this.props;
			}
		}

		// Token: 0x0600AD12 RID: 44306 RVA: 0x003160EF File Offset: 0x003142EF
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			int num = this.IntervalsPassed;
			if (this.Props.minColonistCount > 0)
			{
				if (target.StoryState.lastFireTicks.ContainsKey(this.Props.incident))
				{
					yield break;
				}
				if (PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count < this.Props.minColonistCount)
				{
					yield break;
				}
				num -= target.StoryState.GetTicksFromColonistCount(this.Props.minColonistCount) / 1000;
			}
			if (num == this.Props.fireAfterDaysPassed * 60)
			{
				if (this.Props.skipIfColonistHasMinTitle != null)
				{
					List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
					for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count; i++)
					{
						if (allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].royalty != null && allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].royalty.AllTitlesForReading.Any<RoyalTitle>() && allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].royalty.MainTitle().seniority >= this.Props.skipIfColonistHasMinTitle.seniority)
						{
							yield break;
						}
					}
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (!this.Props.skipIfOnExtremeBiome || (anyPlayerHomeMap != null && !anyPlayerHomeMap.Biome.isExtremeBiome))
				{
					IncidentDef incident = this.Props.incident;
					if (incident.TargetAllowed(target))
					{
						FiringIncident firingIncident = new FiringIncident(incident, this, this.GenerateParms(incident.category, target));
						yield return firingIncident;
					}
				}
			}
			yield break;
		}
	}
}
