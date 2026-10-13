using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DEE RID: 11758
	public class CompRitualSignalSender : ThingComp, IThingGlower
	{
		// Token: 0x060108C9 RID: 67785 RVA: 0x004C5803 File Offset: 0x004C3A03
		public bool ShouldBeLitNow()
		{
			return this.ritualTarget;
		}

		// Token: 0x060108CA RID: 67786 RVA: 0x004C580B File Offset: 0x004C3A0B
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(30))
			{
				bool flag = this.ritualTarget;
				this.ritualTarget = this.parent.IsRitualTarget();
				if (flag != this.ritualTarget)
				{
					this.parent.BroadcastCompSignal("RitualTargetChanged");
				}
			}
		}

		// Token: 0x060108CB RID: 67787 RVA: 0x004C584B File Offset: 0x004C3A4B
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ritualTarget, "ritualTarget", false, false);
		}

		// Token: 0x0400AC93 RID: 44179
		public bool ritualTarget;

		// Token: 0x0400AC94 RID: 44180
		private const int CheckInterval = 30;

		// Token: 0x0400AC95 RID: 44181
		public const string RitualTargetChangedSignal = "RitualTargetChanged";
	}
}
