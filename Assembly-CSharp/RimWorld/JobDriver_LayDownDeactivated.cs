using System;

namespace RimWorld
{
	// Token: 0x020012AD RID: 4781
	public class JobDriver_LayDownDeactivated : JobDriver_LayDown
	{
		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06007527 RID: 29991 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06007528 RID: 29992 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06007529 RID: 29993 RVA: 0x00002501 File Offset: 0x00000701
		public override bool LookForOtherJobs
		{
			get
			{
				return false;
			}
		}
	}
}
