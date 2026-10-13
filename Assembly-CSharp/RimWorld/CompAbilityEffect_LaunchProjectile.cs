using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200224A RID: 8778
	public class CompAbilityEffect_LaunchProjectile : CompAbilityEffect
	{
		// Token: 0x17001D42 RID: 7490
		// (get) Token: 0x0600BF9B RID: 49051 RVA: 0x00378B50 File Offset: 0x00376D50
		public new CompProperties_AbilityLaunchProjectile Props
		{
			get
			{
				return (CompProperties_AbilityLaunchProjectile)this.props;
			}
		}

		// Token: 0x0600BF9C RID: 49052 RVA: 0x00378B5D File Offset: 0x00376D5D
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			this.LaunchProjectile(target);
		}

		// Token: 0x0600BF9D RID: 49053 RVA: 0x00378B70 File Offset: 0x00376D70
		private void LaunchProjectile(LocalTargetInfo target)
		{
			if (this.Props.projectileDef != null)
			{
				Pawn pawn = this.parent.pawn;
				((Projectile)GenSpawn.Spawn(this.Props.projectileDef, pawn.Position, pawn.Map, WipeMode.Vanish)).Launch(pawn, pawn.DrawPos, target, target, ProjectileHitFlags.IntendedTarget, this.parent.verb.preventFriendlyFire, null, null);
			}
		}

		// Token: 0x0600BF9E RID: 49054 RVA: 0x00378BD9 File Offset: 0x00376DD9
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return target.Pawn != null;
		}
	}
}
