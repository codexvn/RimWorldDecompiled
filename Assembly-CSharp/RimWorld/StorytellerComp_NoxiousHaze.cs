using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E53 RID: 7763
	public class StorytellerComp_NoxiousHaze : StorytellerComp
	{
		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x0600AC9A RID: 44186 RVA: 0x00314BCD File Offset: 0x00312DCD
		protected StorytellerCompProperties_NoxiousHaze Props
		{
			get
			{
				return (StorytellerCompProperties_NoxiousHaze)this.props;
			}
		}

		// Token: 0x0600AC9B RID: 44187 RVA: 0x00314BDA File Offset: 0x00312DDA
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = (Map)target;
			float num = WorldPollutionUtility.CalculateNearbyPollutionScore(map.Tile);
			if (num < GameConditionDefOf.NoxiousHaze.minNearbyPollution)
			{
				yield break;
			}
			float num2 = GameConditionDefOf.NoxiousHaze.mtbOverNearbyPollutionCurve.Evaluate(num);
			float num3 = -1f;
			if (map.storyState.lastFireTicks.ContainsKey(IncidentDefOf.NoxiousHaze))
			{
				num3 = (Find.TickManager.TicksGame - map.storyState.lastFireTicks[IncidentDefOf.NoxiousHaze]).TicksToDays();
			}
			if (num3 >= 0f && num3 < this.Props.lastFireMinMTBThreshold * num2)
			{
				yield break;
			}
			if (num3 > this.Props.lastFireMaxMTBThreshold * num2 || Rand.MTBEventOccurs(num2, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(IncidentDefOf.NoxiousHaze.category, target);
				yield return new FiringIncident(IncidentDefOf.NoxiousHaze, this, parms);
			}
			yield break;
		}
	}
}
