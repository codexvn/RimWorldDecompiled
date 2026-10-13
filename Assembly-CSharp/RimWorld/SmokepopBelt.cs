using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B59 RID: 11097
	public class SmokepopBelt : Apparel
	{
		// Token: 0x0600F82B RID: 63531 RVA: 0x004830DC File Offset: 0x004812DC
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			base.Notify_BulletImpactNearby(impactData);
			Pawn wearer = base.Wearer;
			if (wearer == null || wearer.Dead)
			{
				return;
			}
			if (!impactData.bullet.DamageDef.isExplosive && impactData.bullet.Launcher is Building_Turret && impactData.bullet.Launcher != null && impactData.bullet.Launcher.HostileTo(base.Wearer) && !wearer.IsColonist && wearer.Spawned && !wearer.Position.AnyGas(wearer.Map, GasType.BlindSmoke))
			{
				Verb_SmokePop.Pop(this.TryGetComp<CompApparelReloadable>());
			}
		}

		// Token: 0x0600F82C RID: 63532 RVA: 0x0048317B File Offset: 0x0048137B
		public override float GetSpecialApparelScoreOffset()
		{
			return this.GetStatValue(StatDefOf.PackRadius, true, -1) * this.ApparelScorePerBeltRadius;
		}

		// Token: 0x0400A331 RID: 41777
		private float ApparelScorePerBeltRadius = 0.046f;
	}
}
