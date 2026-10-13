using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002256 RID: 8790
	public class CompAbilityEffect_PowerBeam : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D49 RID: 7497
		// (get) Token: 0x0600BFC8 RID: 49096 RVA: 0x0037977A File Offset: 0x0037797A
		public new CompProperties_PowerBeam Props
		{
			get
			{
				return (CompProperties_PowerBeam)this.props;
			}
		}

		// Token: 0x0600BFC9 RID: 49097 RVA: 0x00379788 File Offset: 0x00377988
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			powerBeam.duration = this.Props.durationTicks;
			powerBeam.instigator = this.parent.pawn;
			powerBeam.StartStrike();
		}
	}
}
