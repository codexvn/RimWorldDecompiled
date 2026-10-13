using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F95 RID: 3989
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x0600625C RID: 25180 RVA: 0x001F0C38 File Offset: 0x001EEE38
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x001F0C63 File Offset: 0x001EEE63
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hitTargetAtLeastOnce = true;
			}
		}

		// Token: 0x04004673 RID: 18035
		public Thing target;

		// Token: 0x04004674 RID: 18036
		protected bool hitTargetAtLeastOnce;
	}
}
