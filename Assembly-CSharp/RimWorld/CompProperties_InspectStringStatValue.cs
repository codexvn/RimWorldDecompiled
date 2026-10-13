using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF6 RID: 11510
	public class CompProperties_InspectStringStatValue : CompProperties_InspectString
	{
		// Token: 0x060102A9 RID: 66217 RVA: 0x004AD528 File Offset: 0x004AB728
		public CompProperties_InspectStringStatValue()
		{
			this.compClass = typeof(CompInspectStringStatValue);
		}

		// Token: 0x0400A8D5 RID: 43221
		public StatDef stat;

		// Token: 0x0400A8D6 RID: 43222
		public ToStringNumberSense? numberSense;
	}
}
