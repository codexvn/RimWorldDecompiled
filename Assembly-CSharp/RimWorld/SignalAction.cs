using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AB8 RID: 10936
	public abstract class SignalAction : Thing
	{
		// Token: 0x0600F3B4 RID: 62388 RVA: 0x0046CD83 File Offset: 0x0046AF83
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600F3B5 RID: 62389
		protected abstract void DoAction(SignalArgs args);

		// Token: 0x0600F3B6 RID: 62390 RVA: 0x0046CDBA File Offset: 0x0046AFBA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x0400A087 RID: 41095
		public string signalTag;
	}
}
