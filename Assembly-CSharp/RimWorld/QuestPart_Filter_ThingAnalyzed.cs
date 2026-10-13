using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBD RID: 7101
	public class QuestPart_Filter_ThingAnalyzed : QuestPart_Filter
	{
		// Token: 0x06009E11 RID: 40465 RVA: 0x002DBDA4 File Offset: 0x002D9FA4
		protected override bool Pass(SignalArgs args)
		{
			CompProperties_CompAnalyzableUnlockResearch compProperties_CompAnalyzableUnlockResearch = this.thingDef.CompDefForAssignableFrom<CompAnalyzable>() as CompProperties_CompAnalyzableUnlockResearch;
			if (compProperties_CompAnalyzableUnlockResearch == null)
			{
				return false;
			}
			AnalysisDetails analysisDetails;
			Find.AnalysisManager.TryGetAnalysisProgress(compProperties_CompAnalyzableUnlockResearch.analysisID, out analysisDetails);
			return analysisDetails != null && analysisDetails.Satisfied;
		}

		// Token: 0x06009E12 RID: 40466 RVA: 0x002DBDE5 File Offset: 0x002D9FE5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x04006D6F RID: 28015
		public ThingDef thingDef;
	}
}
