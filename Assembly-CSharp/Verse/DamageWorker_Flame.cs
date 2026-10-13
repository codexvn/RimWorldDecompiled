using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200066D RID: 1645
	public class DamageWorker_Flame : DamageWorker_AddInjury
	{
		// Token: 0x06002968 RID: 10600 RVA: 0x000DA4B4 File Offset: 0x000D86B4
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			Pawn pawn = victim as Pawn;
			if (pawn != null && pawn.Faction == Faction.OfPlayer)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			Map map = victim.Map;
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			if (map == null)
			{
				return damageResult;
			}
			if (!damageResult.deflected && !dinfo.InstantPermanentInjury && Rand.Chance(FireUtility.ChanceToAttachFireFromEvent(victim)))
			{
				victim.TryAttachFire(Rand.Range(0.15f, 0.25f), dinfo.Instigator);
			}
			if (victim.Destroyed && pawn == null)
			{
				foreach (IntVec3 c in victim.OccupiedRect())
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Ash, 1, FilthSourceFlags.None, true);
				}
			}
			return damageResult;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000DA594 File Offset: 0x000D8794
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes);
			if (this.def == DamageDefOf.Flame && Rand.Chance(FireUtility.ChanceToStartFireIn(c, explosion.Map, null)))
			{
				FireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f), explosion.instigator, null);
			}
		}
	}
}
