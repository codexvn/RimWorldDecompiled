using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F7A RID: 3962
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x060061EC RID: 25068 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
