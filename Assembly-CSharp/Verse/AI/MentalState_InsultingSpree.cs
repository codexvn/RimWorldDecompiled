using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F6D RID: 3949
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x0600619F RID: 24991 RVA: 0x001EE786 File Offset: 0x001EC986
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x060061A0 RID: 24992 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04004632 RID: 17970
		public Pawn target;

		// Token: 0x04004633 RID: 17971
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04004634 RID: 17972
		public int lastInsultTicks = -999999;
	}
}
