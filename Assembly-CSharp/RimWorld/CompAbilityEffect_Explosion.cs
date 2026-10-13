using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200221C RID: 8732
	public class CompAbilityEffect_Explosion : CompAbilityEffect
	{
		// Token: 0x17001D22 RID: 7458
		// (get) Token: 0x0600BEE8 RID: 48872 RVA: 0x00375C97 File Offset: 0x00373E97
		private new CompProperties_AbilityExplosion Props
		{
			get
			{
				return (CompProperties_AbilityExplosion)this.props;
			}
		}

		// Token: 0x17001D23 RID: 7459
		// (get) Token: 0x0600BEE9 RID: 48873 RVA: 0x00375CA4 File Offset: 0x00373EA4
		private Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x0600BEEA RID: 48874 RVA: 0x00375CB4 File Offset: 0x00373EB4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			int num = this.Props.damageAmount;
			float num2 = this.Props.armorPenetration;
			if (num == -1)
			{
				num = this.Props.damageDef.defaultDamage;
			}
			if (Mathf.Approximately(num2, -1f))
			{
				num2 = this.Props.damageDef.defaultArmorPenetration;
			}
			IntVec3 position = this.Pawn.Position;
			Map mapHeld = this.Pawn.MapHeld;
			float explosionRadius = this.Props.explosionRadius;
			DamageDef damageDef = this.Props.damageDef;
			Thing pawn = this.Pawn;
			int damAmount = num;
			float armorPenetration = num2;
			SoundDef soundExplode = this.Props.soundExplode;
			Verb verb = this.parent.verb;
			ThingDef weapon;
			if (verb == null)
			{
				weapon = null;
			}
			else
			{
				ThingWithComps equipmentSource = verb.EquipmentSource;
				weapon = ((equipmentSource != null) ? equipmentSource.def : null);
			}
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = this.Props.postExplosionSpawnThingDef;
			ThingDef postExplosionSpawnThingDefWater = this.Props.postExplosionSpawnThingDefWater;
			float postExplosionSpawnChance = this.Props.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = this.Props.postExplosionSpawnThingCount;
			GasType? postExplosionGasType = this.Props.postExplosionGasType;
			ThingDef preExplosionSpawnThingDef = this.Props.preExplosionSpawnThingDef;
			float preExplosionSpawnChance = this.Props.preExplosionSpawnChance;
			int preExplosionSpawnThingCount = this.Props.preExplosionSpawnThingCount;
			bool applyDamageToExplosionCellsNeighbors = this.Props.applyDamageToExplosionCellsNeighbors;
			float explosionChanceToStartFire = this.Props.explosionChanceToStartFire;
			bool explosionDamageFalloff = this.Props.explosionDamageFalloff;
			float expolosionPropagationSpeed = this.Props.damageDef.expolosionPropagationSpeed;
			float screenShakeFactor = this.Props.screenShakeFactor;
			bool doExplosionVFX = this.Props.doExplosionVFX;
			ThingDef preExplosionSpawnSingleThingDef = this.Props.preExplosionSpawnSingleThingDef;
			ThingDef postExplosionSpawnSingleThingDef = this.Props.postExplosionSpawnSingleThingDef;
			GenExplosion.DoExplosion(position, mapHeld, explosionRadius, damageDef, pawn, damAmount, armorPenetration, soundExplode, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, null, 255, applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, explosionChanceToStartFire, explosionDamageFalloff, null, null, null, doExplosionVFX, expolosionPropagationSpeed, 0f, true, postExplosionSpawnThingDefWater, screenShakeFactor, null, null, postExplosionSpawnSingleThingDef, preExplosionSpawnSingleThingDef);
		}
	}
}
