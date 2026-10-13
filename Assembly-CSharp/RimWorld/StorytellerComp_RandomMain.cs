using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E5E RID: 7774
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x0600ACCF RID: 44239 RVA: 0x00315637 File Offset: 0x00313837
		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		// Token: 0x0600ACD0 RID: 44240 RVA: 0x00315644 File Offset: 0x00313844
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float num = this.Props.mtbDays;
			if (target.Tile.Valid && target.Tile.LayerDef.isSpace)
			{
				num *= this.Props.spaceMtbDayFactor;
			}
			if (!Rand.MTBEventOccurs(num, 60000f, 1000f))
			{
				yield break;
			}
			if (target.Tile.Valid && target.Tile.LayerDef.isSpace && GenTicks.TicksGame - Find.Storyteller.LastIncidentTick < GenDate.DaysToTicks(this.Props.spaceMinSpacingDays))
			{
				yield break;
			}
			bool flag = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
			List<IncidentCategoryDef> list = new List<IncidentCategoryDef>();
			IncidentParms parms;
			IncidentDef incidentDef;
			for (;;)
			{
				IncidentCategoryDef incidentCategoryDef = this.ChooseRandomCategory(target, list);
				parms = this.GenerateParms(incidentCategoryDef, target);
				if (base.TrySelectRandomIncident(base.UsableIncidentsInCategory(incidentCategoryDef, parms), out incidentDef, target))
				{
					break;
				}
				list.Add(incidentCategoryDef);
				if (list.Count >= this.Props.categoryWeights.Count)
				{
					goto IL_190;
				}
			}
			if (!this.Props.skipThreatBigIfRaidBeacon || !flag || incidentDef.category != IncidentCategoryDefOf.ThreatBig)
			{
				yield return new FiringIncident(incidentDef, this, parms);
			}
			IL_190:
			yield break;
		}

		// Token: 0x0600ACD1 RID: 44241 RVA: 0x0031565C File Offset: 0x0031385C
		private IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig) && (float)(Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick) > 60000f * this.Props.maxThreatBigIntervalDays)
			{
				return IncidentCategoryDefOf.ThreatBig;
			}
			return (from cw in this.Props.categoryWeights
			where !skipCategories.Contains(cw.category)
			select cw).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
		}

		// Token: 0x0600ACD2 RID: 44242 RVA: 0x00315700 File Offset: 0x00313900
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
			if (incidentParms.points >= 0f)
			{
				incidentParms.points *= this.Props.randomPointsFactorRange.RandomInRange;
			}
			return incidentParms;
		}
	}
}
