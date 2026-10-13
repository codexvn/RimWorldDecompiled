using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003062 RID: 12386
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x17002D00 RID: 11520
		// (get) Token: 0x0601163E RID: 71230 RVA: 0x004FD12A File Offset: 0x004FB32A
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}

		// Token: 0x0601163F RID: 71231 RVA: 0x004FD131 File Offset: 0x004FB331
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}
	}
}
