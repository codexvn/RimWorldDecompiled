using System;

namespace RimWorld
{
	// Token: 0x02002BB6 RID: 11190
	public class CompCauseGameCondition_AncientReactor : CompCauseGameCondition
	{
		// Token: 0x1700273A RID: 10042
		// (get) Token: 0x0600FA3D RID: 64061 RVA: 0x0048BF15 File Offset: 0x0048A115
		public override bool Active
		{
			get
			{
				return !this.HackableComp.IsHacked;
			}
		}

		// Token: 0x1700273B RID: 10043
		// (get) Token: 0x0600FA3E RID: 64062 RVA: 0x0048BF28 File Offset: 0x0048A128
		public CompHackable HackableComp
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableComp) == null)
				{
					result = (this.hackableComp = this.parent.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x0400A472 RID: 42098
		private CompHackable hackableComp;
	}
}
