using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E73 RID: 7795
	public class StorytellerComp_ThreatsGenerator : StorytellerComp
	{
		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x0600AD1E RID: 44318 RVA: 0x0031635F File Offset: 0x0031455F
		protected StorytellerCompProperties_ThreatsGenerator Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatsGenerator)this.props;
			}
		}

		// Token: 0x0600AD1F RID: 44319 RVA: 0x0031636C File Offset: 0x0031456C
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = target as Map;
			foreach (FiringIncident firingIncident in ThreatsGenerator.MakeIntervalIncidents(this.Props.parms, target, (map != null) ? map.generationTick : 0))
			{
				firingIncident.source = this;
				yield return firingIncident;
			}
			IEnumerator<FiringIncident> enumerator = null;
			yield break;
			yield break;
		}
	}
}
