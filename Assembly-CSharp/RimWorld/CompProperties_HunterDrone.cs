using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CE9 RID: 11497
	public class CompProperties_HunterDrone : CompProperties
	{
		// Token: 0x06010273 RID: 66163 RVA: 0x004AC85F File Offset: 0x004AAA5F
		public CompProperties_HunterDrone()
		{
			this.compClass = typeof(CompHunterDrone);
		}

		// Token: 0x06010274 RID: 66164 RVA: 0x004AC88A File Offset: 0x004AAA8A
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosionDamageType == null)
			{
				this.explosionDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x0400A8BD RID: 43197
		public float explosionRadius = 1.9f;

		// Token: 0x0400A8BE RID: 43198
		public DamageDef explosionDamageType;

		// Token: 0x0400A8BF RID: 43199
		public int explosionDamageAmount = 50;
	}
}
