using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AC4 RID: 10948
	public class SignalAction_StartWick : SignalAction_Delay
	{
		// Token: 0x17002622 RID: 9762
		// (get) Token: 0x0600F3E6 RID: 62438 RVA: 0x0046DB4A File Offset: 0x0046BD4A
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null && this.thingWithWick.def == ThingDefOf.AncientFuelNode)
				{
					this.cachedAlert = new Alert_FuelNodeIgnition(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x17002623 RID: 9763
		// (get) Token: 0x0600F3E7 RID: 62439 RVA: 0x0046DB78 File Offset: 0x0046BD78
		public override bool ShouldRemoveNow
		{
			get
			{
				if (this.thingWithWick == null || this.thingWithWick.Destroyed)
				{
					return true;
				}
				CompExplosive compExplosive = this.thingWithWick.TryGetComp<CompExplosive>();
				return compExplosive == null || compExplosive.wickStarted;
			}
		}

		// Token: 0x0600F3E8 RID: 62440 RVA: 0x0046DBB8 File Offset: 0x0046BDB8
		protected override void Complete()
		{
			base.Complete();
			if (this.ShouldRemoveNow)
			{
				return;
			}
			CompExplosive compExplosive = this.thingWithWick.TryGetComp<CompExplosive>();
			if (compExplosive != null)
			{
				compExplosive.StartWick(this.instigator);
			}
			if (this.delayTicks > 0)
			{
				Messages.Message("MessageFuelNodeDelayActivated".Translate(this.thingWithWick.LabelShort), this.thingWithWick, MessageTypeDefOf.NegativeEvent, true);
				return;
			}
			Messages.Message("MessageFuelNodeTriggered".Translate(this.thingWithWick.LabelShort), this.thingWithWick, MessageTypeDefOf.ThreatBig, true);
		}

		// Token: 0x0600F3E9 RID: 62441 RVA: 0x0046DC63 File Offset: 0x0046BE63
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_References.Look<Thing>(ref this.thingWithWick, "thingWithWick", false);
		}

		// Token: 0x0400A0B3 RID: 41139
		public Thing instigator;

		// Token: 0x0400A0B4 RID: 41140
		public Thing thingWithWick;

		// Token: 0x0400A0B5 RID: 41141
		private Alert_ActionDelay cachedAlert;
	}
}
