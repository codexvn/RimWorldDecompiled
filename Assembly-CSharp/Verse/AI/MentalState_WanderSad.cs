using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F7B RID: 3963
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x060061EE RID: 25070 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
