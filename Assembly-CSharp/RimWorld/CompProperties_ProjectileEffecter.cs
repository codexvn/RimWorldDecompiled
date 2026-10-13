using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197B RID: 6523
	public class CompProperties_ProjectileEffecter : CompProperties
	{
		// Token: 0x06009405 RID: 37893 RVA: 0x002B825B File Offset: 0x002B645B
		public CompProperties_ProjectileEffecter()
		{
			this.compClass = typeof(Comp_ProjectileEffecter);
		}

		// Token: 0x06009406 RID: 37894 RVA: 0x002B8273 File Offset: 0x002B6473
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!typeof(Projectile).IsAssignableFrom(parentDef.thingClass))
			{
				yield return base.GetType().Name + " is only meant to be used on Projectile derived Things";
			}
			yield break;
		}

		// Token: 0x04005F94 RID: 24468
		public EffecterDef effecterDef;
	}
}
