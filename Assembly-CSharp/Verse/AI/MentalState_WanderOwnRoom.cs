using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F79 RID: 3961
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x060061E8 RID: 25064 RVA: 0x001EF530 File Offset: 0x001ED730
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			if (this.pawn.ownership.OwnedBed != null)
			{
				this.target = this.pawn.ownership.OwnedBed.Position;
				return;
			}
			this.target = this.pawn.Position;
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x001EF584 File Offset: 0x001ED784
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x060061EA RID: 25066 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04004648 RID: 17992
		public IntVec3 target;
	}
}
