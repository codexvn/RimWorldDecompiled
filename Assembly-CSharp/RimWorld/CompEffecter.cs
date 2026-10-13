using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195F RID: 6495
	public class CompEffecter : ThingComp
	{
		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x0600939A RID: 37786 RVA: 0x002B6CDB File Offset: 0x002B4EDB
		public CompProperties_EffecterBase Props
		{
			get
			{
				return (CompProperties_EffecterBase)this.props;
			}
		}

		// Token: 0x0600939B RID: 37787 RVA: 0x002B6CE8 File Offset: 0x002B4EE8
		protected virtual bool ShouldShowEffecter()
		{
			return this.parent.Spawned && this.parent.MapHeld == Find.CurrentMap;
		}

		// Token: 0x0600939C RID: 37788 RVA: 0x002B6D0C File Offset: 0x002B4F0C
		public override void CompTick()
		{
			base.CompTick();
			if (!this.ShouldShowEffecter())
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
				return;
			}
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
			}
			Effecter effecter2 = this.effecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x04005EFB RID: 24315
		private Effecter effecter;
	}
}
