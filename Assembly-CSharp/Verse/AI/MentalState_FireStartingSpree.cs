using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F7D RID: 3965
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x060061F2 RID: 25074 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
