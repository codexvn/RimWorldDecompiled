using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003862 RID: 14434
	public class CompHeatPusherEffecter : ThingComp
	{
		// Token: 0x170032F5 RID: 13045
		// (get) Token: 0x06013F47 RID: 81735 RVA: 0x005EBCBD File Offset: 0x005E9EBD
		public CompProperties_HeatPusherEffecter Props
		{
			get
			{
				return (CompProperties_HeatPusherEffecter)this.props;
			}
		}

		// Token: 0x170032F6 RID: 13046
		// (get) Token: 0x06013F48 RID: 81736 RVA: 0x005EBCCA File Offset: 0x005E9ECA
		public bool ShouldEmit
		{
			get
			{
				return this.pusherComp.ShouldPushHeatNow;
			}
		}

		// Token: 0x06013F49 RID: 81737 RVA: 0x005EBCD7 File Offset: 0x005E9ED7
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.pusherComp = this.parent.GetComp<CompHeatPusher>();
		}

		// Token: 0x06013F4A RID: 81738 RVA: 0x005EBCF4 File Offset: 0x005E9EF4
		public override void CompTick()
		{
			if (this.ShouldEmit && this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.SpawnAttached(this.parent, this.parent.Map, 1f);
			}
			else if (!this.ShouldEmit && this.effecter != null)
			{
				this.effecter.Cleanup();
				this.effecter = null;
			}
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x0400DC59 RID: 56409
		private Effecter effecter;

		// Token: 0x0400DC5A RID: 56410
		private CompHeatPusher pusherComp;
	}
}
