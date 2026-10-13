using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F7C RID: 3964
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x060061F0 RID: 25072 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
