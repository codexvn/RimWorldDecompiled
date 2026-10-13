using System;

namespace Verse.AI
{
	// Token: 0x02000F8B RID: 3979
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x0600622C RID: 25132 RVA: 0x001F032A File Offset: 0x001EE52A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<bool>(ref this.alreadyHauledCorpse, "alreadyHauledCorpse", false, false);
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x001F0358 File Offset: 0x001EE558
		public override void MentalStateTick(int delta)
		{
			if (this.alreadyHauledCorpse)
			{
				base.MentalStateTick(delta);
				return;
			}
			bool flag = false;
			if (this.pawn.IsHashIntervalTick(500, delta) && !CorpseObsessionMentalStateUtility.IsCorpseValid(this.corpse, this.pawn, false))
			{
				this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
				if (this.corpse == null)
				{
					base.RecoverFromState();
					flag = true;
				}
			}
			if (!flag)
			{
				base.MentalStateTick(delta);
			}
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x001F03C9 File Offset: 0x001EE5C9
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x001F03E3 File Offset: 0x001EE5E3
		public void Notify_CorpseHauled()
		{
			this.alreadyHauledCorpse = true;
		}

		// Token: 0x04004660 RID: 18016
		public Corpse corpse;

		// Token: 0x04004661 RID: 18017
		public bool alreadyHauledCorpse;

		// Token: 0x04004662 RID: 18018
		private const int AnyCorpseStillValidCheckInterval = 500;
	}
}
