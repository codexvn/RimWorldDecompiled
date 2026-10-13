using System;

namespace Verse
{
	// Token: 0x02000D88 RID: 3464
	public class Verb_ArcSprayProjectile : Verb_ArcSpray
	{
		// Token: 0x06005863 RID: 22627 RVA: 0x001C8074 File Offset: 0x001C6274
		protected override void HitCell(IntVec3 cell)
		{
			base.HitCell(cell);
			Map map = this.caster.Map;
			if (GenSight.LineOfSight(this.caster.Position, cell, map, true, null, 0, 0))
			{
				((Projectile)GenSpawn.Spawn(this.verbProps.defaultProjectile, this.caster.Position, map, WipeMode.Vanish)).Launch(this.caster, this.caster.DrawPos, cell, cell, ProjectileHitFlags.IntendedTarget, this.preventFriendlyFire, null, null);
			}
		}
	}
}
