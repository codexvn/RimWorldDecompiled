using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B3E RID: 11070
	public class FlameThrower : Projectile
	{
		// Token: 0x0600F78B RID: 63371 RVA: 0x0047F754 File Offset: 0x0047D954
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			IntVec3 position = base.Position;
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			Find.BattleLog.Add(new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef));
			if (hitThing != null)
			{
				Pawn pawn = hitThing as Pawn;
				if (pawn == null || Rand.Chance(FireUtility.ChanceToAttachFireFromEvent(pawn)))
				{
					hitThing.TryAttachFire(FlameThrower.FireSizeRange.RandomInRange, this.launcher);
				}
			}
			FireUtility.TryStartFireIn(position, map, FlameThrower.FireSizeRange.RandomInRange, this.launcher, null);
			if (Rand.Chance(FlameThrower.ChanceToMakeFilth))
			{
				FilthMaker.TryMakeFilth(position, map, ThingDefOf.Filth_Fuel, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x0400A2E3 RID: 41699
		private static FloatRange FireSizeRange = new FloatRange(0.55f, 0.85f);

		// Token: 0x0400A2E4 RID: 41700
		private static float ChanceToMakeFilth = 0.25f;
	}
}
