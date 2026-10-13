using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196B RID: 6507
	public class CompProperties_Facility : CompProperties
	{
		// Token: 0x060093C4 RID: 37828 RVA: 0x002B73E7 File Offset: 0x002B55E7
		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		// Token: 0x060093C5 RID: 37829 RVA: 0x002B7420 File Offset: 0x002B5620
		public override void ResolveReferences(ThingDef parentDef)
		{
			this.linkableBuildings = new List<ThingDef>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				CompProperties_AffectedByFacilities compProperties = allDefsListForReading[i].GetCompProperties<CompProperties_AffectedByFacilities>();
				if (compProperties != null && compProperties.linkableFacilities != null)
				{
					for (int j = 0; j < compProperties.linkableFacilities.Count; j++)
					{
						if (compProperties.linkableFacilities[j] == parentDef)
						{
							this.linkableBuildings.Add(allDefsListForReading[i]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060093C6 RID: 37830 RVA: 0x002B74A0 File Offset: 0x002B56A0
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.statOffsets == null)
			{
				yield break;
			}
			foreach (StatModifier statModifier in this.statOffsets)
			{
				if (ModsConfig.AnomalyActive && statModifier.stat == StatDefOf.ContainmentStrength)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Containment, "StatsReport_ContainmentStrengthOffset".Translate(), statModifier.value.ToString("F1"), "StatsReport_ContainmentStrengthOffset_Desc".Translate(), 500, null, null, false, false);
				}
				else if (ModsConfig.OdysseyActive)
				{
					if (statModifier.stat == StatDefOf.GravshipRange)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_GravshipRangeOffset".Translate(), statModifier.value.ToString("F0"), "StatsReport_GravshipRangeOffset_Desc".Translate(), 500, null, null, false, false);
					}
					else if (statModifier.stat == StatDefOf.SubstructureSupport)
					{
						StatDef substructureSupport = StatDefOf.SubstructureSupport;
						yield return new StatDrawEntry(substructureSupport.category, substructureSupport.LabelCap, statModifier.value.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), substructureSupport.description, substructureSupport.displayPriorityInCategory, null, null, false, false);
					}
				}
			}
			List<StatModifier>.Enumerator enumerator2 = default(List<StatModifier>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04005F41 RID: 24385
		[Unsaved(false)]
		public List<ThingDef> linkableBuildings;

		// Token: 0x04005F42 RID: 24386
		public List<StatModifier> statOffsets;

		// Token: 0x04005F43 RID: 24387
		public int maxSimultaneous = 1;

		// Token: 0x04005F44 RID: 24388
		public bool mustBePlacedAdjacent;

		// Token: 0x04005F45 RID: 24389
		public bool mustBePlacedAdjacentCardinalToBedHead;

		// Token: 0x04005F46 RID: 24390
		public bool mustBePlacedAdjacentCardinalToAndFacingBedHead;

		// Token: 0x04005F47 RID: 24391
		public bool mustBePlacedFacingThingLinear;

		// Token: 0x04005F48 RID: 24392
		public bool canLinkToMedBedsOnly;

		// Token: 0x04005F49 RID: 24393
		public float maxDistance = 8f;

		// Token: 0x04005F4A RID: 24394
		public float minDistance;

		// Token: 0x04005F4B RID: 24395
		public bool showMaxSimultaneous = true;

		// Token: 0x04005F4C RID: 24396
		public bool requiresLOS = true;
	}
}
