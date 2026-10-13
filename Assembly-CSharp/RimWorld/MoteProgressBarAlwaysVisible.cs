using System;

namespace RimWorld
{
	// Token: 0x02002B1C RID: 11036
	public class MoteProgressBarAlwaysVisible : MoteProgressBar
	{
		// Token: 0x17002699 RID: 9881
		// (get) Token: 0x0600F69C RID: 63132 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool OnlyShowForClosestZoom
		{
			get
			{
				return false;
			}
		}
	}
}
