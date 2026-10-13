using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200087F RID: 2175
	public class TriggerUnfogged : Thing
	{
		// Token: 0x06003721 RID: 14113 RVA: 0x001278E3 File Offset: 0x00125AE3
		protected override void Tick()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (base.Position.Fogged(base.Map))
			{
				this.everFogged = true;
				return;
			}
			if (this.everFogged)
			{
				this.Activated();
				return;
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x0012791F File Offset: 0x00125B1F
		public void Activated()
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, false));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x00127946 File Offset: 0x00125B46
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.everFogged, "everFogged", false, false);
		}

		// Token: 0x04002958 RID: 10584
		public string signalTag;

		// Token: 0x04002959 RID: 10585
		private bool everFogged;
	}
}
