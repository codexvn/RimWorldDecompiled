using System;

namespace Verse
{
	// Token: 0x020008C7 RID: 2247
	public class Projectile_HellsphereCannon : Projectile
	{
		// Token: 0x060038E8 RID: 14568 RVA: 0x0012F858 File Offset: 0x0012DA58
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			IntVec3 position = base.Position;
			Map map2 = map;
			float radius = 4.9f;
			DamageDef damageDef = base.DamageDef;
			Thing launcher = this.launcher;
			int damageAmount = this.DamageAmount;
			float armorPenetration = this.ArmorPenetration;
			SoundDef explosionSound = null;
			ThingDef equipmentDef = this.equipmentDef;
			ThingDef def = this.def;
			Thing thing = this.intendedTarget.Thing;
			ThingDef postExplosionSpawnThingDef = null;
			float postExplosionSpawnChance = 0f;
			int postExplosionSpawnThingCount = 1;
			float explosionChanceToStartFire = this.def.projectile.explosionChanceToStartFire;
			float expolosionPropagationSpeed = base.DamageDef.expolosionPropagationSpeed;
			GenExplosion.DoExplosion(position, map2, radius, damageDef, launcher, damageAmount, armorPenetration, explosionSound, equipmentDef, def, thing, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, explosionChanceToStartFire, false, null, null, null, true, expolosionPropagationSpeed, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x04002A11 RID: 10769
		private const float ExtraExplosionRadius = 4.9f;
	}
}
