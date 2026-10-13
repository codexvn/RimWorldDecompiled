using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F77 RID: 3959
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x060061E1 RID: 25057 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
