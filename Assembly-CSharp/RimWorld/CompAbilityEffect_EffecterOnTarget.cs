using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200221A RID: 8730
	public class CompAbilityEffect_EffecterOnTarget : CompAbilityEffect
	{
		// Token: 0x17001D21 RID: 7457
		// (get) Token: 0x0600BEE4 RID: 48868 RVA: 0x00375B63 File Offset: 0x00373D63
		public new CompProperties_AbilityEffecterOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityEffecterOnTarget)this.props;
			}
		}

		// Token: 0x0600BEE5 RID: 48869 RVA: 0x00375B70 File Offset: 0x00373D70
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Effecter effecter;
			if (target.HasThing)
			{
				effecter = this.Props.effecterDef.Spawn(target.Thing, this.parent.pawn.Map, this.Props.scale);
			}
			else
			{
				effecter = this.Props.effecterDef.Spawn(target.Cell, this.parent.pawn.Map, this.Props.scale);
			}
			if (this.Props.maintainForTicks > 0)
			{
				this.parent.AddEffecterToMaintain(effecter, target.Cell, this.Props.maintainForTicks, null);
				return;
			}
			effecter.Cleanup();
		}
	}
}
