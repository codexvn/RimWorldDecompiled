using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F74 RID: 3956
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x060061D8 RID: 25048 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
