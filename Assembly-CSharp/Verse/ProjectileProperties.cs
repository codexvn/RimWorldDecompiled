using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000249 RID: 585
	public class ProjectileProperties
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x000460B5 File Offset: 0x000442B5
		public float SpeedTilesPerTick
		{
			get
			{
				return this.speed / 100f;
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000460C4 File Offset: 0x000442C4
		public int GetDamageAmount(Thing weapon, StringBuilder explanation = null)
		{
			float damageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true, -1) : 1f;
			return this.GetDamageAmount(damageMultiplier, weapon, explanation);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x000460F4 File Offset: 0x000442F4
		public int GetDamageAmount(ThingDef weapon, ThingDef weaponStuff, StringBuilder explanation = null)
		{
			float damageMultiplier = (weapon != null) ? weapon.GetStatValueAbstract(StatDefOf.RangedWeapon_DamageMultiplier, weaponStuff) : 1f;
			return this.GetDamageAmount(damageMultiplier, null, explanation);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00046124 File Offset: 0x00044324
		public int GetDamageAmount(float damageMultiplier, Thing weapon, StringBuilder explanation = null)
		{
			int num;
			if (this.damageAmountBase != -1)
			{
				num = this.damageAmountBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					Log.ErrorOnce("Failed to find sane damage amount", 91094882);
					return 1;
				}
				num = this.damageDef.defaultDamage;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToString());
			}
			num = Mathf.RoundToInt((float)num * damageMultiplier);
			if (!Mathf.Approximately(damageMultiplier, 1f))
			{
				if (explanation != null)
				{
					explanation.AppendLine();
				}
				if (explanation != null)
				{
					explanation.AppendLine(StatDefOf.RangedWeapon_DamageMultiplier.LabelCap + ": " + damageMultiplier.ToStringPercent());
				}
				if (weapon != null && explanation != null)
				{
					explanation.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_DamageMultiplier, weapon));
				}
			}
			if (explanation != null)
			{
				explanation.AppendLine();
			}
			if (explanation != null)
			{
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num.ToString());
			}
			return num;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0004623C File Offset: 0x0004443C
		public float GetArmorPenetration(Thing weapon = null, StringBuilder explanation = null)
		{
			if (this.damageDef.armorCategory == null)
			{
				return 0f;
			}
			float num;
			if (this.damageAmountBase != -1 || this.armorPenetrationBase >= 0f)
			{
				num = this.armorPenetrationBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					return 0f;
				}
				num = this.damageDef.defaultArmorPenetration;
			}
			if (num < 0f)
			{
				num = (float)this.GetDamageAmount(null, null) * 0.015f;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringPercent());
			}
			if (weapon != null)
			{
				float statValue = weapon.GetStatValue(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier, true, -1);
				num *= statValue;
				if (!Mathf.Approximately(statValue, 1f))
				{
					if (explanation != null)
					{
						explanation.AppendLine();
					}
					if (explanation != null)
					{
						explanation.AppendLine(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier.LabelCap + ": " + statValue.ToStringPercent());
					}
					if (explanation != null)
					{
						explanation.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier, weapon));
					}
				}
			}
			if (explanation != null)
			{
				explanation.AppendLine();
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_FinalValue".Translate() + ": " + num.ToStringPercent());
			}
			return num;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00046385 File Offset: 0x00044585
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.alwaysFreeIntercept && this.flyOverhead)
			{
				yield return "alwaysFreeIntercept and flyOverhead are both true";
			}
			if (this.damageAmountBase == -1 && this.damageDef != null && this.damageDef.defaultDamage == -1)
			{
				yield return "no damage amount specified for projectile";
			}
			if (this.filth != null && this.filthCount.TrueMax <= 0)
			{
				yield return "has filth but filthCount <= 0.";
			}
			if (parent.thingClass == typeof(Projectile_SpawnsThing) && this.spawnsThingDef == null)
			{
				yield return "Projectile_SpawnsThing has no spawnsThingDef";
			}
			yield break;
		}

		// Token: 0x04000A30 RID: 2608
		public float speed = 5f;

		// Token: 0x04000A31 RID: 2609
		public bool flyOverhead;

		// Token: 0x04000A32 RID: 2610
		public bool alwaysFreeIntercept;

		// Token: 0x04000A33 RID: 2611
		public DamageDef damageDef;

		// Token: 0x04000A34 RID: 2612
		private int damageAmountBase = -1;

		// Token: 0x04000A35 RID: 2613
		private float armorPenetrationBase = -1f;

		// Token: 0x04000A36 RID: 2614
		public float stoppingPower = 0.5f;

		// Token: 0x04000A37 RID: 2615
		public List<ExtraDamage> extraDamages;

		// Token: 0x04000A38 RID: 2616
		public float arcHeightFactor;

		// Token: 0x04000A39 RID: 2617
		public float shadowSize;

		// Token: 0x04000A3A RID: 2618
		public EffecterDef landedEffecter;

		// Token: 0x04000A3B RID: 2619
		public float spinRate;

		// Token: 0x04000A3C RID: 2620
		public bool useGraphicClass;

		// Token: 0x04000A3D RID: 2621
		public SoundDef soundHitThickRoof;

		// Token: 0x04000A3E RID: 2622
		public SoundDef soundExplode;

		// Token: 0x04000A3F RID: 2623
		public SoundDef soundImpactAnticipate;

		// Token: 0x04000A40 RID: 2624
		public SoundDef soundAmbient;

		// Token: 0x04000A41 RID: 2625
		public SoundDef soundImpact;

		// Token: 0x04000A42 RID: 2626
		public float explosionRadius;

		// Token: 0x04000A43 RID: 2627
		public float explosionRadiusDisplayPadding;

		// Token: 0x04000A44 RID: 2628
		public int explosionDelay;

		// Token: 0x04000A45 RID: 2629
		public bool doExplosionVFX = true;

		// Token: 0x04000A46 RID: 2630
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04000A47 RID: 2631
		public float preExplosionSpawnChance = 1f;

		// Token: 0x04000A48 RID: 2632
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04000A49 RID: 2633
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04000A4A RID: 2634
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x04000A4B RID: 2635
		public float postExplosionSpawnChance = 1f;

		// Token: 0x04000A4C RID: 2636
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04000A4D RID: 2637
		public GasType? postExplosionGasType;

		// Token: 0x04000A4E RID: 2638
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04000A4F RID: 2639
		public float explosionChanceToStartFire;

		// Token: 0x04000A50 RID: 2640
		public bool explosionDamageFalloff;

		// Token: 0x04000A51 RID: 2641
		public float screenShakeFactor = 1f;

		// Token: 0x04000A52 RID: 2642
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x04000A53 RID: 2643
		public ThingDef postExplosionSpawnSingleThingDef;

		// Token: 0x04000A54 RID: 2644
		public ThingDef filth;

		// Token: 0x04000A55 RID: 2645
		public float filthChance = 1f;

		// Token: 0x04000A56 RID: 2646
		public IntRange filthCount;

		// Token: 0x04000A57 RID: 2647
		public int numExtraHitCells;

		// Token: 0x04000A58 RID: 2648
		public bool explosionSpawnsSingleFilth;

		// Token: 0x04000A59 RID: 2649
		public TerrainDef spawnTerrain;

		// Token: 0x04000A5A RID: 2650
		public float terrainChance = 1f;

		// Token: 0x04000A5B RID: 2651
		public bool terrainReplacesFloors;

		// Token: 0x04000A5C RID: 2652
		public EffecterDef explosionEffect;

		// Token: 0x04000A5D RID: 2653
		public int explosionEffectLifetimeTicks;

		// Token: 0x04000A5E RID: 2654
		public ThingDef spawnsThingDef;

		// Token: 0x04000A5F RID: 2655
		public bool tryAdjacentFreeSpaces;

		// Token: 0x04000A60 RID: 2656
		public PawnKindDef spawnsPawnKind;

		// Token: 0x04000A61 RID: 2657
		public ThingDef beamMoteDef;

		// Token: 0x04000A62 RID: 2658
		public float beamStartOffset;

		// Token: 0x04000A63 RID: 2659
		public bool ai_IsIncendiary;
	}
}
