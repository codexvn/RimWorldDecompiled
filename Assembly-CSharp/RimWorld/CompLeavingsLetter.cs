using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D1B RID: 11547
	public class CompLeavingsLetter : ThingComp
	{
		// Token: 0x17002936 RID: 10550
		// (get) Token: 0x0601039F RID: 66463 RVA: 0x004B155D File Offset: 0x004AF75D
		public CompProperties_LeavingsLetter Props
		{
			get
			{
				return (CompProperties_LeavingsLetter)this.props;
			}
		}

		// Token: 0x060103A0 RID: 66464 RVA: 0x004B156C File Offset: 0x004AF76C
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			if (!leavings.NullOrEmpty<Thing>())
			{
				CompLeavingsLetter.tmpLeavings.AddRange((this.Props.leavingsFilter != null) ? (from t in leavings
				where this.Props.leavingsFilter.Allows(t.def)
				select t) : leavings);
				if (CompLeavingsLetter.tmpLeavings.Count > 0)
				{
					Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(CompLeavingsLetter.tmpLeavings[0].Named("LEAVINGS1")), this.Props.letterText.Formatted(CompLeavingsLetter.tmpLeavings[0].Named("LEAVINGS1"), this.parent.Named("PARENT")), this.Props.letterDef, CompLeavingsLetter.tmpLeavings, null, null, null, null, 0, true);
					CompLeavingsLetter.tmpLeavings.Clear();
				}
			}
		}

		// Token: 0x0400A976 RID: 43382
		private static List<Thing> tmpLeavings = new List<Thing>();
	}
}
