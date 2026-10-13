using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D55 RID: 11605
	public class FocusStrengthOffset_Lit : FocusStrengthOffset
	{
		// Token: 0x060104E5 RID: 66789 RVA: 0x004B5748 File Offset: 0x004B3948
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Lit".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x060104E6 RID: 66790 RVA: 0x004B5797 File Offset: 0x004B3997
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Lit".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060104E7 RID: 66791 RVA: 0x004B537B File Offset: 0x004B357B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x060104E8 RID: 66792 RVA: 0x004B57C8 File Offset: 0x004B39C8
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			CompGlower compGlower = parent.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}
	}
}
