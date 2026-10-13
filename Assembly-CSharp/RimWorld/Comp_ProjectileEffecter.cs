using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197D RID: 6525
	public class Comp_ProjectileEffecter : ThingComp
	{
		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x0600940F RID: 37903 RVA: 0x002B8377 File Offset: 0x002B6577
		public CompProperties_ProjectileEffecter Props
		{
			get
			{
				return (CompProperties_ProjectileEffecter)this.props;
			}
		}

		// Token: 0x06009410 RID: 37904 RVA: 0x002B8384 File Offset: 0x002B6584
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned)
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
				return;
			}
			Projectile projectile = this.parent as Projectile;
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.Spawn(projectile.Position, projectile.intendedTarget.Cell, this.parent.MapHeld, 1f);
			}
			Effecter effecter2 = this.effecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x04005F9B RID: 24475
		private Effecter effecter;
	}
}
