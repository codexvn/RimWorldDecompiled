using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002538 RID: 9528
	public class Thought_AttendedRitual : Thought_Memory
	{
		// Token: 0x17002002 RID: 8194
		// (get) Token: 0x0600D079 RID: 53369 RVA: 0x003D5300 File Offset: 0x003D3500
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.sourcePrecept.Named("RITUAL"));
			}
		}

		// Token: 0x17002003 RID: 8195
		// (get) Token: 0x0600D07A RID: 53370 RVA: 0x003D5327 File Offset: 0x003D3527
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.sourcePrecept.Named("RITUAL"));
			}
		}
	}
}
