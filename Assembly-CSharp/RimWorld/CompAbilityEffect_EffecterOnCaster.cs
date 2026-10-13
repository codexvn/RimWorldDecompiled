using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002218 RID: 8728
	public class CompAbilityEffect_EffecterOnCaster : CompAbilityEffect
	{
		// Token: 0x17001D20 RID: 7456
		// (get) Token: 0x0600BEE0 RID: 48864 RVA: 0x00375AA0 File Offset: 0x00373CA0
		public new CompProperties_AbilityEffecterOnCaster Props
		{
			get
			{
				return (CompProperties_AbilityEffecterOnCaster)this.props;
			}
		}

		// Token: 0x0600BEE1 RID: 48865 RVA: 0x00375AB0 File Offset: 0x00373CB0
		public override void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
			base.PostApplied(targets, map);
			Effecter effecter = this.Props.effecterDef.Spawn(this.parent.pawn, map, this.Props.scale);
			if (this.Props.maintainTicks > 0)
			{
				map.effecterMaintainer.AddEffecterToMaintain(effecter, new TargetInfo(this.parent.pawn), this.parent.pawn, this.Props.maintainTicks);
				return;
			}
			effecter.Cleanup();
		}
	}
}
