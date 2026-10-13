using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F76 RID: 3958
	public class MentalState_Terror : MentalState
	{
		// Token: 0x060061DF RID: 25055 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
