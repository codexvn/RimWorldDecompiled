using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD6 RID: 12246
	public class Alert_Custom : Alert
	{
		// Token: 0x0601141C RID: 70684 RVA: 0x004F1E7D File Offset: 0x004F007D
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x0601141D RID: 70685 RVA: 0x004F1E85 File Offset: 0x004F0085
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x0601141E RID: 70686 RVA: 0x004F1E92 File Offset: 0x004F0092
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x0400B306 RID: 45830
		public string label;

		// Token: 0x0400B307 RID: 45831
		public string explanation;

		// Token: 0x0400B308 RID: 45832
		public AlertReport report;
	}
}
