using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF2 RID: 11506
	public class CompInspectStringSchooldesk : CompInspectString
	{
		// Token: 0x060102A0 RID: 66208 RVA: 0x004AD3F8 File Offset: 0x004AB5F8
		public override string CompInspectStringExtra()
		{
			float num = LearningUtility.SchoolDeskLearningRate(this.parent);
			if (num > 1f)
			{
				return base.Props.inspectString + ": " + (num - 1f).ToStringPercent();
			}
			return null;
		}
	}
}
