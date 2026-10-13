using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F72 RID: 3954
	public class MentalState_Binging : MentalState
	{
		// Token: 0x060061CD RID: 25037 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
