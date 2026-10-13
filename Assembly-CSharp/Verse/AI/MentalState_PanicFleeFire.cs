using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F75 RID: 3957
	public class MentalState_PanicFleeFire : MentalState
	{
		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x060061DA RID: 25050 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x001EF400 File Offset: 0x001ED600
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(30, delta))
			{
				if (this.lastFireSeenTick < 0 || ThoughtWorker_Pyrophobia.NearFire(this.pawn))
				{
					this.lastFireSeenTick = Find.TickManager.TicksGame;
				}
				if (this.lastFireSeenTick >= 0 && Find.TickManager.TicksGame >= this.lastFireSeenTick + this.def.minTicksBeforeRecovery)
				{
					base.RecoverFromState();
				}
			}
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x001EF477 File Offset: 0x001ED677
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastFireSeenTick, "lastFireSeenTick", -1, false);
		}

		// Token: 0x04004647 RID: 17991
		private int lastFireSeenTick = -1;
	}
}
