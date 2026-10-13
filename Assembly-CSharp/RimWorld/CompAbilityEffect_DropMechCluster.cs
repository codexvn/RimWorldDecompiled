using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002210 RID: 8720
	public class CompAbilityEffect_DropMechCluster : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D1C RID: 7452
		// (get) Token: 0x0600BECD RID: 48845 RVA: 0x00375747 File Offset: 0x00373947
		public new CompProperties_DropMechCluster Props
		{
			get
			{
				return (CompProperties_DropMechCluster)this.props;
			}
		}

		// Token: 0x0600BECE RID: 48846 RVA: 0x00375754 File Offset: 0x00373954
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(this.Props.points, this.parent.pawn.Map, this.Props.startDormant, false);
			MechClusterUtility.SpawnCluster(target.Cell, this.parent.pawn.Map, sketch, true, false, null);
		}

		// Token: 0x0600BECF RID: 48847 RVA: 0x003757B7 File Offset: 0x003739B7
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return ModsConfig.RoyaltyActive && Faction.OfMechanoids != null && !Faction.OfMechanoids.deactivated && base.CanApplyOn(target, dest);
		}
	}
}
