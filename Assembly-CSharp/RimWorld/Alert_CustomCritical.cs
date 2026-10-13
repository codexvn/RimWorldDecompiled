using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD7 RID: 12247
	public class Alert_CustomCritical : Alert_Critical
	{
		// Token: 0x06011420 RID: 70688 RVA: 0x004F1EA2 File Offset: 0x004F00A2
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x06011421 RID: 70689 RVA: 0x004F1EAA File Offset: 0x004F00AA
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x06011422 RID: 70690 RVA: 0x004F1EB7 File Offset: 0x004F00B7
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x0400B309 RID: 45833
		public string label;

		// Token: 0x0400B30A RID: 45834
		public string explanation;

		// Token: 0x0400B30B RID: 45835
		public AlertReport report;
	}
}
