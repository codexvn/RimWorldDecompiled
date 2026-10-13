using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B42 RID: 11074
	public class Spark : Projectile
	{
		// Token: 0x0600F79D RID: 63389 RVA: 0x0047FDB4 File Offset: 0x0047DFB4
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			Thing instigator = this.launcher;
			Fire fire = this.launcher as Fire;
			if (fire != null)
			{
				instigator = fire.instigator;
			}
			FireUtility.TryStartFireIn(base.Position, map, 0.1f, instigator, null);
		}
	}
}
