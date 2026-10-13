using System;

namespace RimWorld
{
	// Token: 0x020012AC RID: 4780
	public class JobDriver_LayDownDormant : JobDriver_LayDown
	{
		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06007524 RID: 29988 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06007525 RID: 29989 RVA: 0x00002501 File Offset: 0x00000701
		public override bool LookForOtherJobs
		{
			get
			{
				return false;
			}
		}
	}
}
