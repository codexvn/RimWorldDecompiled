using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B39 RID: 11065
	public class Beam : Bullet
	{
		// Token: 0x170026D9 RID: 9945
		// (get) Token: 0x0600F77E RID: 63358 RVA: 0x0047F1BC File Offset: 0x0047D3BC
		public override Vector3 ExactPosition
		{
			get
			{
				return this.destination + Vector3.up * this.def.Altitude;
			}
		}

		// Token: 0x0600F77F RID: 63359 RVA: 0x0047F1E0 File Offset: 0x0047D3E0
		public override void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			base.Launch(launcher, origin, usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, targetCoverDef);
			Vector3 offsetA = (this.ExactPosition - launcher.Position.ToVector3Shifted()).Yto0().normalized * this.def.projectile.beamStartOffset;
			if (this.def.projectile.beamMoteDef != null)
			{
				MoteMaker.MakeInteractionOverlay(this.def.projectile.beamMoteDef, launcher, usedTarget.ToTargetInfo(base.Map), offsetA, Vector3.zero);
			}
			this.ImpactSomething();
		}
	}
}
