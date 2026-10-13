using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C88 RID: 11400
	public class CompFireBurst : ThingComp
	{
		// Token: 0x17002862 RID: 10338
		// (get) Token: 0x0600FFE0 RID: 65504 RVA: 0x004A2100 File Offset: 0x004A0300
		private CompProperties_FireBurst Props
		{
			get
			{
				return (CompProperties_FireBurst)this.props;
			}
		}

		// Token: 0x17002863 RID: 10339
		// (get) Token: 0x0600FFE1 RID: 65505 RVA: 0x004A210D File Offset: 0x004A030D
		private CompExplosive CompExplosive
		{
			get
			{
				if (this.compExplosive == null)
				{
					this.compExplosive = this.parent.TryGetComp<CompExplosive>();
				}
				return this.compExplosive;
			}
		}

		// Token: 0x0600FFE2 RID: 65506 RVA: 0x004A2130 File Offset: 0x004A0330
		public override void CompTick()
		{
			if (this.CompExplosive.wickStarted)
			{
				FireBurstUtility.ThrowFuelTick(this.parent.Position, this.Props.radius, this.parent.Map);
				if (this.CompExplosive.wickTicksLeft <= this.Props.ticksAwayFromDetonate && this.effecter == null)
				{
					this.effecter = EffecterDefOf.Fire_Burst.Spawn(this.parent.Position, this.parent.Map, 1f);
				}
			}
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x0400A756 RID: 42838
		private CompExplosive compExplosive;

		// Token: 0x0400A757 RID: 42839
		private Effecter effecter;
	}
}
