using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BAE RID: 11182
	public class CompAnalyzableBiosignature : CompAnalyzable
	{
		// Token: 0x1700272C RID: 10028
		// (get) Token: 0x0600FA04 RID: 64004 RVA: 0x0048B433 File Offset: 0x00489633
		public new CompProperties_CompAnalyzableBiosignature Props
		{
			get
			{
				return (CompProperties_CompAnalyzableBiosignature)this.props;
			}
		}

		// Token: 0x1700272D RID: 10029
		// (get) Token: 0x0600FA05 RID: 64005 RVA: 0x0048B440 File Offset: 0x00489640
		public override int AnalysisID
		{
			get
			{
				return this.biosignature;
			}
		}

		// Token: 0x1700272E RID: 10030
		// (get) Token: 0x0600FA06 RID: 64006 RVA: 0x0048B448 File Offset: 0x00489648
		public override NamedArgument? ExtraNamedArg
		{
			get
			{
				return new NamedArgument?(AnomalyUtility.GetBiosignatureName(this.biosignature).Colorize(ColoredText.NameColor).Named("BIOSIGNATURE"));
			}
		}

		// Token: 0x0600FA07 RID: 64007 RVA: 0x0048B46E File Offset: 0x0048966E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x0400A460 RID: 42080
		public int biosignature;
	}
}
