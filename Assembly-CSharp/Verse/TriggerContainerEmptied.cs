using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200087E RID: 2174
	public class TriggerContainerEmptied : Thing
	{
		// Token: 0x0600371E RID: 14110 RVA: 0x00127848 File Offset: 0x00125A48
		protected override void Tick()
		{
			if (!base.Spawned || !this.IsHashIntervalTick(60))
			{
				return;
			}
			Thing thing = this.container;
			CompThingContainer compThingContainer = (thing != null) ? thing.TryGetComp<CompThingContainer>() : null;
			if (compThingContainer == null)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!compThingContainer.innerContainer.Any)
			{
				Find.SignalManager.SendSignal(new Signal(this.signalTag, false));
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x001278B8 File Offset: 0x00125AB8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_References.Look<Thing>(ref this.container, "container", false);
		}

		// Token: 0x04002956 RID: 10582
		public string signalTag;

		// Token: 0x04002957 RID: 10583
		public Thing container;
	}
}
