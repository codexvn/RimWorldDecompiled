using System;

namespace RimWorld
{
	// Token: 0x02001D48 RID: 7496
	public class GameCondition_NoSunlight_Instant : GameCondition_NoSunlight
	{
		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x0600A7F3 RID: 42995 RVA: 0x00002501 File Offset: 0x00000701
		public override int TransitionTicks
		{
			get
			{
				return 0;
			}
		}
	}
}
