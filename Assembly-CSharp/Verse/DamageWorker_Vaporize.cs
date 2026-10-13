using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000674 RID: 1652
	public class DamageWorker_Vaporize : DamageWorker_AddInjury
	{
		// Token: 0x0600297B RID: 10619 RVA: 0x000DAA5C File Offset: 0x000D8C5C
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			bool flag = c.DistanceTo(explosion.Position) <= 2.9f;
			Thing firstThing = c.GetFirstThing(explosion.Map, ThingDefOf.Filth_FireFoam);
			if (firstThing != null)
			{
				firstThing.Destroy(DestroyMode.Vanish);
			}
			base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes && flag);
			FireUtility.TryStartFireIn(c, explosion.Map, DamageWorker_Vaporize.FireSizeRange.RandomInRange, explosion.instigator, null);
			if (flag)
			{
				FleckMaker.ThrowSmoke(c.ToVector3Shifted(), explosion.Map, 2f);
			}
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000DAAE7 File Offset: 0x000D8CE7
		protected override void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			if (cell.DistanceTo(explosion.Position) <= 2.9f)
			{
				base.ExplosionDamageThing(explosion, t, damagedThings, ignoredThings, cell);
			}
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000DAB0A File Offset: 0x000D8D0A
		public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			base.ExplosionStart(explosion, cellsToAffect);
			Effecter effecter = EffecterDefOf.Vaporize_Heatwave.Spawn();
			effecter.Trigger(new TargetInfo(explosion.Position, explosion.Map, false), TargetInfo.Invalid, -1);
			effecter.Cleanup();
		}

		// Token: 0x040021C6 RID: 8646
		private const float VaporizeRadius = 2.9f;

		// Token: 0x040021C7 RID: 8647
		private static readonly FloatRange FireSizeRange = new FloatRange(0.4f, 0.8f);
	}
}
