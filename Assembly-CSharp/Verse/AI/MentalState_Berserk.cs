using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F71 RID: 3953
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x060061C9 RID: 25033 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x060061CB RID: 25035 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
