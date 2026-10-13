using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020008C6 RID: 2246
	public class Projectile_Explosive : Projectile
	{
		// Token: 0x060038E3 RID: 14563 RVA: 0x0012F46F File Offset: 0x0012D66F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x0012F489 File Offset: 0x0012D689
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.ticksToDetonation > 0)
			{
				this.ticksToDetonation -= delta;
				if (this.ticksToDetonation <= 0)
				{
					this.Explode();
				}
			}
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x0012F4B8 File Offset: 0x0012D6B8
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			if (blockedByShield || this.def.projectile.explosionDelay == 0)
			{
				this.Explode();
				return;
			}
			this.landed = true;
			this.ticksToDetonation = this.def.projectile.explosionDelay;
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this, base.DamageDef, this.launcher.Faction, this.launcher);
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x0012F51C File Offset: 0x0012D71C
		protected virtual void Explode()
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.def.projectile.explosionEffect != null)
			{
				Effecter effecter = this.def.projectile.explosionEffect.Spawn();
				if (this.def.projectile.explosionEffectLifetimeTicks != 0)
				{
					map.effecterMaintainer.AddEffecterToMaintain(effecter, base.Position.ToVector3().ToIntVec3(), this.def.projectile.explosionEffectLifetimeTicks);
				}
				else
				{
					effecter.Trigger(new TargetInfo(base.Position, map, false), new TargetInfo(base.Position, map, false), -1);
					effecter.Cleanup();
				}
			}
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = this.def.projectile.explosionRadius;
			DamageDef damageDef = base.DamageDef;
			Thing launcher = this.launcher;
			int damageAmount = this.DamageAmount;
			float armorPenetration = this.ArmorPenetration;
			SoundDef soundExplode = this.def.projectile.soundExplode;
			ThingDef equipmentDef = this.equipmentDef;
			ThingDef def = this.def;
			Thing thing = this.intendedTarget.Thing;
			ThingDef postExplosionSpawnThingDef = this.def.projectile.postExplosionSpawnThingDef ?? (this.def.projectile.explosionSpawnsSingleFilth ? null : this.def.projectile.filth);
			ThingDef postExplosionSpawnThingDefWater = this.def.projectile.postExplosionSpawnThingDefWater;
			float postExplosionSpawnChance = this.def.projectile.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = this.def.projectile.postExplosionSpawnThingCount;
			GasType? postExplosionGasType = this.def.projectile.postExplosionGasType;
			ThingDef preExplosionSpawnThingDef = this.def.projectile.preExplosionSpawnThingDef;
			float preExplosionSpawnChance = this.def.projectile.preExplosionSpawnChance;
			int preExplosionSpawnThingCount = this.def.projectile.preExplosionSpawnThingCount;
			bool applyDamageToExplosionCellsNeighbors = this.def.projectile.applyDamageToExplosionCellsNeighbors;
			float explosionChanceToStartFire = this.def.projectile.explosionChanceToStartFire;
			bool explosionDamageFalloff = this.def.projectile.explosionDamageFalloff;
			float? direction = new float?(this.origin.AngleToFlat(this.destination));
			float expolosionPropagationSpeed = base.DamageDef.expolosionPropagationSpeed;
			float screenShakeFactor = this.def.projectile.screenShakeFactor;
			bool doExplosionVFX = this.def.projectile.doExplosionVFX;
			ThingDef preExplosionSpawnSingleThingDef = this.def.projectile.preExplosionSpawnSingleThingDef;
			ThingDef postExplosionSpawnSingleThingDef = this.def.projectile.postExplosionSpawnSingleThingDef;
			GenExplosion.DoExplosion(position, map2, explosionRadius, damageDef, launcher, damageAmount, armorPenetration, soundExplode, equipmentDef, def, thing, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, null, 255, applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, explosionChanceToStartFire, explosionDamageFalloff, direction, null, null, doExplosionVFX, expolosionPropagationSpeed, 0f, true, postExplosionSpawnThingDefWater, screenShakeFactor, null, null, postExplosionSpawnSingleThingDef, preExplosionSpawnSingleThingDef);
			if (this.def.projectile.explosionSpawnsSingleFilth && this.def.projectile.filth != null && this.def.projectile.filthCount.TrueMax > 0 && Rand.Chance(this.def.projectile.filthChance) && !base.Position.Filled(map))
			{
				FilthMaker.TryMakeFilth(base.Position, map, this.def.projectile.filth, this.def.projectile.filthCount.RandomInRange, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x04002A10 RID: 10768
		private int ticksToDetonation;
	}
}
