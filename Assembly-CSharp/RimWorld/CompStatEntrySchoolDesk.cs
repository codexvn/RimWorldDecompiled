using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200214A RID: 8522
	public class CompStatEntrySchoolDesk : CompStatEntry
	{
		// Token: 0x17001C54 RID: 7252
		// (get) Token: 0x0600BA2A RID: 47658 RVA: 0x0035B417 File Offset: 0x00359617
		private CompProperties_StatEntry Props
		{
			get
			{
				return (CompProperties_StatEntry)this.props;
			}
		}

		// Token: 0x0600BA2B RID: 47659 RVA: 0x0035B543 File Offset: 0x00359743
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			float f = LearningUtility.SchoolDeskLearningRate(this.parent);
			int num = LearningUtility.ConnectedBlackboards(this.parent);
			StringBuilder stringBuilder = new StringBuilder(this.Props.reportText);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + 1f.ToStringPercent());
			stringBuilder.AppendLine();
			if (num > 0)
			{
				TaggedString taggedString = "StatsReport_Connected".Translate(Find.ActiveLanguageWorker.Pluralize(ThingDefOf.Blackboard.label, -1));
				taggedString += string.Concat(new string[]
				{
					" ",
					num.ToString(),
					" / ",
					3.ToString(),
					": x",
					(1f + (float)num * 0.2f).ToStringPercent()
				});
				stringBuilder.AppendLine(taggedString);
			}
			stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + f.ToStringPercent());
			yield return new StatDrawEntry(this.Props.statCategoryDef, this.Props.statLabel, f.ToStringPercent(), stringBuilder.ToString(), this.Props.displayPriorityInCategory, null, null, false, false);
			yield break;
		}
	}
}
