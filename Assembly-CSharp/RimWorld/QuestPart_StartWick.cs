using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB5 RID: 7349
	public class QuestPart_StartWick : QuestPart
	{
		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x0600A3BE RID: 41918 RVA: 0x002EF7F7 File Offset: 0x002ED9F7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.explosiveThing;
				if (this.initiator != null)
				{
					yield return this.initiator;
				}
				yield break;
			}
		}

		// Token: 0x0600A3BF RID: 41919 RVA: 0x002EF808 File Offset: 0x002EDA08
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.explosiveThing != null)
			{
				CompExplosive compExplosive = this.explosiveThing.TryGetComp<CompExplosive>();
				if (compExplosive != null)
				{
					compExplosive.StartWick(this.initiator);
				}
			}
		}

		// Token: 0x0600A3C0 RID: 41920 RVA: 0x002EF852 File Offset: 0x002EDA52
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.explosiveThing, "explosiveThing", false);
			Scribe_References.Look<Thing>(ref this.initiator, "initiator", false);
		}

		// Token: 0x0400711D RID: 28957
		public string inSignal;

		// Token: 0x0400711E RID: 28958
		public Thing explosiveThing;

		// Token: 0x0400711F RID: 28959
		public Thing initiator;
	}
}
