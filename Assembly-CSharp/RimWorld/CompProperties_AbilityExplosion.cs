using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200221B RID: 8731
	public class CompProperties_AbilityExplosion : CompProperties_AbilityEffect
	{
		// Token: 0x0600BEE7 RID: 48871 RVA: 0x00375C2C File Offset: 0x00373E2C
		public CompProperties_AbilityExplosion()
		{
			this.compClass = typeof(CompAbilityEffect_Explosion);
		}

		// Token: 0x0400830D RID: 33549
		public DamageDef damageDef;

		// Token: 0x0400830E RID: 33550
		public int damageAmount = -1;

		// Token: 0x0400830F RID: 33551
		public float armorPenetration = -1f;

		// Token: 0x04008310 RID: 33552
		public SoundDef soundExplode;

		// Token: 0x04008311 RID: 33553
		public float explosionRadius;

		// Token: 0x04008312 RID: 33554
		public bool doExplosionVFX = true;

		// Token: 0x04008313 RID: 33555
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04008314 RID: 33556
		public float preExplosionSpawnChance = 1f;

		// Token: 0x04008315 RID: 33557
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04008316 RID: 33558
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04008317 RID: 33559
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x04008318 RID: 33560
		public float postExplosionSpawnChance = 1f;

		// Token: 0x04008319 RID: 33561
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x0400831A RID: 33562
		public GasType? postExplosionGasType;

		// Token: 0x0400831B RID: 33563
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x0400831C RID: 33564
		public float explosionChanceToStartFire;

		// Token: 0x0400831D RID: 33565
		public bool explosionDamageFalloff;

		// Token: 0x0400831E RID: 33566
		public float screenShakeFactor = 1f;

		// Token: 0x0400831F RID: 33567
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x04008320 RID: 33568
		public ThingDef postExplosionSpawnSingleThingDef;
	}
}
