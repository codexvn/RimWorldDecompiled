using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E39 RID: 7737
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x0600AC34 RID: 44084 RVA: 0x00313BCB File Offset: 0x00311DCB
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x0600AC35 RID: 44085 RVA: 0x00313BD8 File Offset: 0x00311DD8
		private float DeepDrillInfestationMTBDaysPerDrill
		{
			get
			{
				Difficulty difficulty = Find.Storyteller.difficulty;
				if (difficulty.deepDrillInfestationChanceFactor <= 0f)
				{
					return -1f;
				}
				return this.Props.baseMtbDaysPerDrill / difficulty.deepDrillInfestationChanceFactor;
			}
		}

		// Token: 0x0600AC36 RID: 44086 RVA: 0x00313C15 File Offset: 0x00311E15
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = (Map)target;
			StorytellerComp_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, StorytellerComp_DeepDrillInfestation.tmpDrills);
			if (!StorytellerComp_DeepDrillInfestation.tmpDrills.Any<Thing>())
			{
				yield break;
			}
			float mtb = this.DeepDrillInfestationMTBDaysPerDrill;
			if (mtb < 0f)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < StorytellerComp_DeepDrillInfestation.tmpDrills.Count; i = num + 1)
			{
				if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
				{
					IncidentParms parms = this.GenerateParms(IncidentCategoryDefOf.DeepDrillInfestation, target);
					IncidentDef def;
					if (base.UsableIncidentsInCategory(IncidentCategoryDefOf.DeepDrillInfestation, parms).TryRandomElement(out def))
					{
						yield return new FiringIncident(def, this, parms);
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04007667 RID: 30311
		private static List<Thing> tmpDrills = new List<Thing>();
	}
}
