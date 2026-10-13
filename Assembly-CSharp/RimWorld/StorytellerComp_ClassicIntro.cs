using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E33 RID: 7731
	public class StorytellerComp_ClassicIntro : StorytellerComp
	{
		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x0600AC1D RID: 44061 RVA: 0x0031376A File Offset: 0x0031196A
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x0600AC1E RID: 44062 RVA: 0x0031377C File Offset: 0x0031197C
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (target != Find.Maps.Find((Map x) => x.IsPlayerHome))
			{
				yield break;
			}
			if (this.IntervalsPassed == 150)
			{
				IncidentDef visitorGroup = IncidentDefOf.VisitorGroup;
				if (visitorGroup.TargetAllowed(target))
				{
					yield return new FiringIncident(visitorGroup, this, null)
					{
						parms = 
						{
							target = target,
							points = (float)Rand.Range(40, 100)
						}
					};
				}
			}
			if (this.IntervalsPassed == 204)
			{
				IncidentCategoryDef threatCategory = Find.Storyteller.difficulty.allowIntroThreats ? IncidentCategoryDefOf.ThreatSmall : IncidentCategoryDefOf.Misc;
				IncidentDef incidentDef;
				if ((from def in DefDatabase<IncidentDef>.AllDefs
				where def.TargetAllowed(target) && def.category == threatCategory
				select def).TryRandomElementByWeight((IncidentDef def) => this.IncidentChanceFinal(def, target), out incidentDef))
				{
					yield return new FiringIncident(incidentDef, this, null)
					{
						parms = StorytellerUtility.DefaultParmsNow(incidentDef.category, target)
					};
				}
			}
			IncidentDef incidentDef2;
			if (this.IntervalsPassed == 264 && (from def in DefDatabase<IncidentDef>.AllDefs
			where def.TargetAllowed(target) && def.category == IncidentCategoryDefOf.Misc
			select def).TryRandomElementByWeight((IncidentDef def) => this.IncidentChanceFinal(def, target), out incidentDef2))
			{
				yield return new FiringIncident(incidentDef2, this, null)
				{
					parms = StorytellerUtility.DefaultParmsNow(incidentDef2.category, target)
				};
			}
			if (this.IntervalsPassed == 324)
			{
				IncidentDef incidentDef3 = IncidentDefOf.RaidEnemy;
				if (!Find.Storyteller.difficulty.allowIntroThreats)
				{
					incidentDef3 = (from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(target) && def.category == IncidentCategoryDefOf.Misc
					select def).RandomElementByWeightWithFallback((IncidentDef def) => this.IncidentChanceFinal(def, target), null);
				}
				if (incidentDef3 != null && incidentDef3.TargetAllowed(target))
				{
					yield return new FiringIncident(incidentDef3, this, null)
					{
						parms = this.GenerateParms(incidentDef3.category, target),
						parms = 
						{
							points = 40f,
							raidForceOneDowned = true,
							raidNeverFleeIndividual = true
						}
					};
				}
			}
			yield break;
		}
	}
}
