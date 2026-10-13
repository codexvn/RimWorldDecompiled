using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002949 RID: 10569
	public class BroadshieldPack : Apparel
	{
		// Token: 0x0600E789 RID: 59273 RVA: 0x00437770 File Offset: 0x00435970
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			base.Notify_BulletImpactNearby(impactData);
			Pawn wearer = base.Wearer;
			if (wearer == null || wearer.Dead)
			{
				return;
			}
			if (!impactData.bullet.DamageDef.isExplosive && CompProjectileInterceptor.InterceptsProjectile(BroadshieldPack.BroadshieldProjectileInterceptorProperties(), impactData.bullet) && impactData.bullet.Launcher != null && impactData.bullet.Launcher.HostileTo(base.Wearer) && !wearer.IsColonist && wearer.Spawned && !this.NearbyActiveBroadshield())
			{
				Verb_DeployBroadshield.Deploy(this.TryGetComp<CompApparelReloadable>());
			}
		}

		// Token: 0x0600E78A RID: 59274 RVA: 0x00437804 File Offset: 0x00435A04
		private bool NearbyActiveBroadshield()
		{
			float radius = BroadshieldPack.BroadshieldProjectileInterceptorProperties().radius;
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(base.PositionHeld, base.MapHeld, radius, true))
			{
				CompProjectileInterceptor compProjectileInterceptor = thing.TryGetComp<CompProjectileInterceptor>();
				if (thing.def == ThingDefOf.BroadshieldProjector && compProjectileInterceptor != null && compProjectileInterceptor.Active)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E78B RID: 59275 RVA: 0x00437888 File Offset: 0x00435A88
		private static CompProperties_ProjectileInterceptor BroadshieldProjectileInterceptorProperties()
		{
			return ThingDefOf.BroadshieldProjector.GetCompProperties<CompProperties_ProjectileInterceptor>();
		}
	}
}
