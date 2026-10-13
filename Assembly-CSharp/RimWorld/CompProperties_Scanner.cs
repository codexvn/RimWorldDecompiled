using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F05 RID: 12037
	public class CompProperties_Scanner : CompProperties
	{
		// Token: 0x06010FEE RID: 69614 RVA: 0x004E3D9E File Offset: 0x004E1F9E
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.scanFindMtbDays <= 0f)
			{
				yield return "scanFindMtbDays not set";
			}
			yield break;
		}

		// Token: 0x0400B0B5 RID: 45237
		public float scanFindMtbDays;

		// Token: 0x0400B0B6 RID: 45238
		public float scanFindGuaranteedDays = -1f;

		// Token: 0x0400B0B7 RID: 45239
		public StatDef scanSpeedStat;

		// Token: 0x0400B0B8 RID: 45240
		public SoundDef soundWorking;
	}
}
