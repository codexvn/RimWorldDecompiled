using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E30 RID: 7728
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x0600AC10 RID: 44048 RVA: 0x003135BF File Offset: 0x003117BF
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		// Token: 0x0600AC11 RID: 44049 RVA: 0x003135CC File Offset: 0x003117CC
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float num = this.Props.mtbDays;
			if (this.Props.mtbDaysFactorByDaysPassedCurve != null)
			{
				num *= this.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedSinceSettleFloat);
			}
			if (Rand.MTBEventOccurs(num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.category, target);
				IncidentDef def;
				if (base.TrySelectRandomIncident(base.UsableIncidentsInCategory(this.Props.category, parms), out def, target))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x0600AC12 RID: 44050 RVA: 0x003135E3 File Offset: 0x003117E3
		public override string ToString()
		{
			return string.Format("{0} {1}", base.ToString(), this.Props.category);
		}
	}
}
