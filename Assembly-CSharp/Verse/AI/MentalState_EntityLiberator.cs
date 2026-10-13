using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F86 RID: 3974
	public class MentalState_EntityLiberator : MentalState
	{
		// Token: 0x06006219 RID: 25113 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x0600621A RID: 25114 RVA: 0x001EFCA1 File Offset: 0x001EDEA1
		public override void Notify_ReleasedTarget()
		{
			base.RecoverFromState();
		}
	}
}
