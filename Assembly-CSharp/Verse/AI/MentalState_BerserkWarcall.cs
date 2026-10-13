using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F81 RID: 3969
	public class MentalState_BerserkWarcall : MentalState
	{
		// Token: 0x06006207 RID: 25095 RVA: 0x001EFA0F File Offset: 0x001EDC0F
		public override bool ForceHostileTo(Thing t)
		{
			if (this.sourceFaction == null)
			{
				return t.HostileTo(this.pawn);
			}
			return t.HostileTo(this.sourceFaction);
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x001EFA32 File Offset: 0x001EDC32
		public override bool ForceHostileTo(Faction f)
		{
			return f.HostileTo(this.sourceFaction);
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
