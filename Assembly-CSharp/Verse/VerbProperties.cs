using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000268 RID: 616
	public class VerbProperties
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0004970A File Offset: 0x0004790A
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0004971C File Offset: 0x0004791C
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x00049734 File Offset: 0x00047934
		public bool Ranged
		{
			get
			{
				return this.LaunchesProjectile || typeof(Verb_ShootBeam).IsAssignableFrom(this.verbClass) || typeof(Verb_SpewFire).IsAssignableFrom(this.verbClass) || typeof(Verb_Spray).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00049790 File Offset: 0x00047990
		public string AccuracySummaryString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.accuracyTouch.ToStringPercent(),
					" - ",
					this.accuracyShort.ToStringPercent(),
					" - ",
					this.accuracyMedium.ToStringPercent(),
					" - ",
					this.accuracyLong.ToStringPercent()
				});
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x000497F8 File Offset: 0x000479F8
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00049810 File Offset: 0x00047A10
		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass) || this.defaultProjectile.GetCompProperties<CompProperties_Explosive>() != null);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x00049870 File Offset: 0x00047A70
		public float ForcedMissRadius
		{
			get
			{
				if (this.isMortar && this.forcedMissRadiusClassicMortars >= 0f)
				{
					Storyteller storyteller = Find.Storyteller;
					if (((storyteller != null) ? storyteller.difficulty : null) != null && Find.Storyteller.difficulty.classicMortars)
					{
						return this.forcedMissRadiusClassicMortars;
					}
				}
				return this.forcedMissRadius;
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000498C4 File Offset: 0x00047AC4
		public float AdjustedRange(Verb ownerVerb, Thing attacker)
		{
			float num = (this.rangeStat == null) ? this.range : attacker.GetStatValue(this.rangeStat, true, -1);
			CompUniqueWeapon compUniqueWeapon;
			if (((ownerVerb != null) ? ownerVerb.EquipmentSource : null) == null || !ownerVerb.EquipmentSource.TryGetComp(out compUniqueWeapon) || !compUniqueWeapon.IgnoreAccuracyMaluses)
			{
				Map mapHeld = attacker.MapHeld;
				if (mapHeld != null && mapHeld.weatherManager.CurWeatherMaxRangeCap >= 0f)
				{
					num = Mathf.Min(num, attacker.Map.weatherManager.CurWeatherMaxRangeCap);
				}
			}
			return num;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00049954 File Offset: 0x00047B54
		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee damage amount for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 5469809);
				return 0f;
			}
			return this.AdjustedMeleeDamageAmount(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000499AC File Offset: 0x00047BAC
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00049A04 File Offset: 0x00047C04
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, equipmentStuff, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00049A5C File Offset: 0x00047C5C
		public float AdjustedArmorPenetration(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate armor penetration for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 9865767);
				return 0f;
			}
			return this.AdjustedArmorPenetration(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00049AB4 File Offset: 0x00047CB4
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValue = equipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true, -1);
				num *= statValue;
			}
			return num;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00049B08 File Offset: 0x00047D08
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValueAbstract = equipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, equipmentStuff);
				num *= statValueAbstract;
			}
			return num;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00049B5C File Offset: 0x00047D5C
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, null);
			}
			return 0f;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00049B9C File Offset: 0x00047D9C
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, equipmentStuff, null);
			}
			return 0f;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00049BEC File Offset: 0x00047DEC
		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee selection weight for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 385716351);
				return 0f;
			}
			return this.AdjustedMeleeSelectionWeight(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource, ownerVerb.DirectOwner is Pawn);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00049C50 File Offset: 0x00047E50
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00049CEC File Offset: 0x00047EEC
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00049D8C File Offset: 0x00047F8C
		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate cooldown for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 19485711);
				return 0f;
			}
			return this.AdjustedCooldown(ownerVerb.tool, attacker, ownerVerb.EquipmentSource);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00049DDC File Offset: 0x00047FDC
		public float AdjustedCooldown(Tool tool, Pawn attacker, Thing equipment)
		{
			float num = this.defaultCooldownTime;
			if (tool != null)
			{
				num = tool.AdjustedCooldown(equipment);
			}
			else if (equipment != null && !this.IsMeleeAttack)
			{
				num = equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true, -1);
			}
			if (attacker != null)
			{
				if (this.IsMeleeAttack)
				{
					num *= attacker.GetStatValue(StatDefOf.MeleeCooldownFactor, true, -1);
				}
				else
				{
					num *= attacker.GetStatValue(StatDefOf.RangedCooldownFactor, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00049E44 File Offset: 0x00048044
		public float AdjustedCooldown(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff)
		{
			float num = this.defaultCooldownTime;
			if (tool != null)
			{
				num = tool.AdjustedCooldown(equipment, equipmentStuff);
			}
			else if (equipment != null && !this.IsMeleeAttack)
			{
				num = equipment.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, equipmentStuff);
			}
			if (attacker != null)
			{
				if (this.IsMeleeAttack)
				{
					num *= attacker.GetStatValue(StatDefOf.MeleeCooldownFactor, true, -1);
				}
				else
				{
					num *= attacker.GetStatValue(StatDefOf.RangedCooldownFactor, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00049EAE File Offset: 0x000480AE
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker)
		{
			return this.AdjustedCooldown(ownerVerb, attacker).SecondsToTicks();
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00049EC0 File Offset: 0x000480C0
		private float AdjustedAccuracy(RangeCategory cat, Thing equipment)
		{
			if (equipment != null)
			{
				StatDef stat = null;
				switch (cat)
				{
				case RangeCategory.Touch:
					stat = StatDefOf.AccuracyTouch;
					break;
				case RangeCategory.Short:
					stat = StatDefOf.AccuracyShort;
					break;
				case RangeCategory.Medium:
					stat = StatDefOf.AccuracyMedium;
					break;
				case RangeCategory.Long:
					stat = StatDefOf.AccuracyLong;
					break;
				}
				return equipment.GetStatValue(stat, true, -1);
			}
			switch (cat)
			{
			case RangeCategory.Touch:
				return this.accuracyTouch;
			case RangeCategory.Short:
				return this.accuracyShort;
			case RangeCategory.Medium:
				return this.accuracyMedium;
			case RangeCategory.Long:
				return this.accuracyLong;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00049F4B File Offset: 0x0004814B
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker) + ((ownerVerb.BurstShotCount - 1) * ownerVerb.TicksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00049F74 File Offset: 0x00048174
		public float GetDamageFactorFor(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate damage factor for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 94324562);
				return 1f;
			}
			return this.GetDamageFactorFor(ownerVerb.tool, attacker, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00049FC4 File Offset: 0x000481C4
		public float GetDamageFactorFor(Tool tool, Pawn attacker, HediffComp_VerbGiver hediffCompSource)
		{
			float num = 1f;
			if (attacker != null)
			{
				if (hediffCompSource != null && hediffCompSource.parent.Part != null)
				{
					num *= PawnCapacityUtility.CalculatePartEfficiency(hediffCompSource.Pawn.health.hediffSet, hediffCompSource.parent.Part, true, null);
				}
				else if (attacker != null && this.AdjustedLinkedBodyPartsGroup(tool) != null)
				{
					float num2 = PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(attacker.health.hediffSet, this.AdjustedLinkedBodyPartsGroup(tool));
					if (this.AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(tool))
					{
						num2 = Mathf.Max(num2, 0.4f);
					}
					num *= num2;
				}
				if (attacker != null && this.IsMeleeAttack)
				{
					num *= attacker.ageTracker.CurLifeStage.meleeDamageFactor;
					num *= attacker.GetStatValue(StatDefOf.MeleeDamageFactor, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0004A080 File Offset: 0x00048280
		public BodyPartGroupDef AdjustedLinkedBodyPartsGroup(Tool tool)
		{
			if (tool != null)
			{
				return tool.linkedBodyPartsGroup;
			}
			return this.linkedBodyPartsGroup;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0004A092 File Offset: 0x00048292
		public bool AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(Tool tool)
		{
			if (tool != null)
			{
				return tool.ensureLinkedBodyPartsGroupAlwaysUsable;
			}
			return this.ensureLinkedBodyPartsGroupAlwaysUsable;
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0004A0A4 File Offset: 0x000482A4
		public float EffectiveMinRange(LocalTargetInfo target, Thing caster)
		{
			return this.EffectiveMinRange(VerbUtility.AllowAdjacentShot(target, caster));
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0004A0B4 File Offset: 0x000482B4
		public float EffectiveMinRange(bool allowAdjacentShot)
		{
			float num = this.minRange;
			if (!allowAdjacentShot && !this.IsMeleeAttack && this.LaunchesProjectile)
			{
				num = Mathf.Max(num, 1.421f);
			}
			return num;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0004A0E8 File Offset: 0x000482E8
		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float value;
			if (dist <= 3f)
			{
				value = this.AdjustedAccuracy(RangeCategory.Touch, equipment);
			}
			else if (dist <= 12f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Touch, equipment), this.AdjustedAccuracy(RangeCategory.Short, equipment), (dist - 3f) / 9f);
			}
			else if (dist <= 25f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Short, equipment), this.AdjustedAccuracy(RangeCategory.Medium, equipment), (dist - 12f) / 13f);
			}
			else if (dist <= 40f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Medium, equipment), this.AdjustedAccuracy(RangeCategory.Long, equipment), (dist - 25f) / 15f);
			}
			else
			{
				value = this.AdjustedAccuracy(RangeCategory.Long, equipment);
			}
			return Mathf.Clamp(value, 0.01f, 1f);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0004A1AD File Offset: 0x000483AD
		public float GetForceMissFactorFor(Thing equipment, Pawn caster)
		{
			if (equipment.def.building != null && equipment.def.building.IsMortar)
			{
				return caster.GetStatValueForPawn(StatDefOf.MortarMissRadiusFactor, caster, true);
			}
			return 1f;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0004A1E4 File Offset: 0x000483E4
		public void DrawRadiusRing(IntVec3 center, Verb verb = null)
		{
			if (Find.CurrentMap == null || Find.World.renderer.wantedMode == WorldRenderMode.Planet)
			{
				return;
			}
			if (!this.IsMeleeAttack && this.targetable)
			{
				float num = this.EffectiveMinRange(true);
				float num2 = (verb != null) ? verb.EffectiveRange : this.range;
				if (num > 0f && num < GenRadial.MaxRadialPatternRadius)
				{
					GenDraw.DrawRadiusRing(center, num);
				}
				if (num2 < (float)(Find.CurrentMap.Size.x + Find.CurrentMap.Size.z) && num2 < GenRadial.MaxRadialPatternRadius)
				{
					Func<IntVec3, bool> predicate = null;
					if (this.drawHighlightWithLineOfSight)
					{
						predicate = ((IntVec3 c) => GenSight.LineOfSight(center, c, Find.CurrentMap));
					}
					GenDraw.DrawRadiusRing(center, num2, Color.white, predicate);
				}
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0004A2B8 File Offset: 0x000484B8
		public override string ToString()
		{
			string str;
			if (!this.label.NullOrEmpty())
			{
				str = this.label;
			}
			else
			{
				str = "range=" + this.range.ToString() + ", defaultProjectile=" + this.defaultProjectile.ToStringSafe<ThingDef>();
			}
			return "VerbProperties(" + str + ")";
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0004A311 File Offset: 0x00048511
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0004A31E File Offset: 0x0004851E
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (parent.race != null && this.linkedBodyPartsGroup != null && !parent.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(this.linkedBodyPartsGroup)))
			{
				string[] array = new string[5];
				array[0] = "has verb with linkedBodyPartsGroup ";
				int num = 1;
				BodyPartGroupDef bodyPartGroupDef = this.linkedBodyPartsGroup;
				array[num] = ((bodyPartGroupDef != null) ? bodyPartGroupDef.ToString() : null);
				array[2] = " but body ";
				int num2 = 3;
				BodyDef body = parent.race.body;
				array[num2] = ((body != null) ? body.ToString() : null);
				array[4] = " has no parts with that group.";
				yield return string.Concat(array);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null && this.forcedMissRadius > 0f != this.CausesExplosion)
			{
				yield return "has incorrect forcedMiss settings; explosive projectiles and only explosive projectiles should have forced miss enabled";
			}
			yield break;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0004A335 File Offset: 0x00048535
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x04000B89 RID: 2953
		public VerbCategory category = VerbCategory.Misc;

		// Token: 0x04000B8A RID: 2954
		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		// Token: 0x04000B8B RID: 2955
		[MustTranslate]
		public string label;

		// Token: 0x04000B8C RID: 2956
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x04000B8D RID: 2957
		public bool isPrimary = true;

		// Token: 0x04000B8E RID: 2958
		public bool violent = true;

		// Token: 0x04000B8F RID: 2959
		public float minRange;

		// Token: 0x04000B90 RID: 2960
		public float range = 1.42f;

		// Token: 0x04000B91 RID: 2961
		public StatDef rangeStat;

		// Token: 0x04000B92 RID: 2962
		public int burstShotCount = 1;

		// Token: 0x04000B93 RID: 2963
		public int ticksBetweenBurstShots = 15;

		// Token: 0x04000B94 RID: 2964
		public bool showBurstShotStats = true;

		// Token: 0x04000B95 RID: 2965
		public float noiseRadius = 3f;

		// Token: 0x04000B96 RID: 2966
		public bool hasStandardCommand;

		// Token: 0x04000B97 RID: 2967
		public bool targetable = true;

		// Token: 0x04000B98 RID: 2968
		public bool nonInterruptingSelfCast;

		// Token: 0x04000B99 RID: 2969
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x04000B9A RID: 2970
		public bool requireLineOfSight = true;

		// Token: 0x04000B9B RID: 2971
		public bool mustCastOnOpenGround;

		// Token: 0x04000B9C RID: 2972
		public bool forceNormalTimeSpeed = true;

		// Token: 0x04000B9D RID: 2973
		public bool onlyManualCast;

		// Token: 0x04000B9E RID: 2974
		public bool stopBurstWithoutLos = true;

		// Token: 0x04000B9F RID: 2975
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04000BA0 RID: 2976
		public float commonality = 1f;

		// Token: 0x04000BA1 RID: 2977
		public Intelligence minIntelligence;

		// Token: 0x04000BA2 RID: 2978
		public float consumeFuelPerShot;

		// Token: 0x04000BA3 RID: 2979
		public float consumeFuelPerBurst;

		// Token: 0x04000BA4 RID: 2980
		public bool stunTargetOnCastStart;

		// Token: 0x04000BA5 RID: 2981
		public string invalidTargetPawn;

		// Token: 0x04000BA6 RID: 2982
		public float commonalityVsEdificeFactor = 1f;

		// Token: 0x04000BA7 RID: 2983
		public SimpleCurve flammabilityAttachFireChanceCurve;

		// Token: 0x04000BA8 RID: 2984
		public bool useableInPocketMaps = true;

		// Token: 0x04000BA9 RID: 2985
		public bool useableInVacuum = true;

		// Token: 0x04000BAA RID: 2986
		[MustTranslate]
		public string mouseTargetingText;

		// Token: 0x04000BAB RID: 2987
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04000BAC RID: 2988
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04000BAD RID: 2989
		public float warmupTime;

		// Token: 0x04000BAE RID: 2990
		public float defaultCooldownTime;

		// Token: 0x04000BAF RID: 2991
		public string commandIcon;

		// Token: 0x04000BB0 RID: 2992
		public SoundDef soundCast;

		// Token: 0x04000BB1 RID: 2993
		public SoundDef soundCastTail;

		// Token: 0x04000BB2 RID: 2994
		public SoundDef soundAiming;

		// Token: 0x04000BB3 RID: 2995
		public float muzzleFlashScale;

		// Token: 0x04000BB4 RID: 2996
		public ThingDef impactMote;

		// Token: 0x04000BB5 RID: 2997
		public FleckDef impactFleck;

		// Token: 0x04000BB6 RID: 2998
		public bool drawAimPie = true;

		// Token: 0x04000BB7 RID: 2999
		public EffecterDef warmupEffecter;

		// Token: 0x04000BB8 RID: 3000
		public bool drawHighlightWithLineOfSight;

		// Token: 0x04000BB9 RID: 3001
		public ThingDef aimingLineMote;

		// Token: 0x04000BBA RID: 3002
		public float? aimingLineMoteFixedLength;

		// Token: 0x04000BBB RID: 3003
		public ThingDef aimingChargeMote;

		// Token: 0x04000BBC RID: 3004
		public float aimingChargeMoteOffset;

		// Token: 0x04000BBD RID: 3005
		public ThingDef aimingTargetMote;

		// Token: 0x04000BBE RID: 3006
		public EffecterDef aimingTargetEffecter;

		// Token: 0x04000BBF RID: 3007
		public Color explosionRadiusRingColor = Color.white;

		// Token: 0x04000BC0 RID: 3008
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04000BC1 RID: 3009
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x04000BC2 RID: 3010
		public DamageDef meleeDamageDef;

		// Token: 0x04000BC3 RID: 3011
		public int meleeDamageBaseAmount = 1;

		// Token: 0x04000BC4 RID: 3012
		public float meleeArmorPenetrationBase = -1f;

		// Token: 0x04000BC5 RID: 3013
		public bool ai_IsWeapon = true;

		// Token: 0x04000BC6 RID: 3014
		public bool ai_IsBuildingDestroyer;

		// Token: 0x04000BC7 RID: 3015
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x04000BC8 RID: 3016
		public bool ai_RangedAlawaysShootGroundBelowTarget;

		// Token: 0x04000BC9 RID: 3017
		public bool ai_IsDoorDestroyer;

		// Token: 0x04000BCA RID: 3018
		public bool ai_ProjectileLaunchingIgnoresMeleeThreats;

		// Token: 0x04000BCB RID: 3019
		public float ai_TargetHasRangedAttackScoreOffset;

		// Token: 0x04000BCC RID: 3020
		public ThingDef defaultProjectile;

		// Token: 0x04000BCD RID: 3021
		private float forcedMissRadius;

		// Token: 0x04000BCE RID: 3022
		private float forcedMissRadiusClassicMortars = -1f;

		// Token: 0x04000BCF RID: 3023
		public bool forcedMissEvenDispersal;

		// Token: 0x04000BD0 RID: 3024
		private bool isMortar;

		// Token: 0x04000BD1 RID: 3025
		public float accuracyTouch = 1f;

		// Token: 0x04000BD2 RID: 3026
		public float accuracyShort = 1f;

		// Token: 0x04000BD3 RID: 3027
		public float accuracyMedium = 1f;

		// Token: 0x04000BD4 RID: 3028
		public float accuracyLong = 1f;

		// Token: 0x04000BD5 RID: 3029
		public bool canGoWild = true;

		// Token: 0x04000BD6 RID: 3030
		public DamageDef beamDamageDef;

		// Token: 0x04000BD7 RID: 3031
		public float beamWidth = 1f;

		// Token: 0x04000BD8 RID: 3032
		public float beamMaxDeviation;

		// Token: 0x04000BD9 RID: 3033
		public FleckDef beamGroundFleckDef;

		// Token: 0x04000BDA RID: 3034
		public EffecterDef beamEndEffecterDef;

		// Token: 0x04000BDB RID: 3035
		public ThingDef beamMoteDef;

		// Token: 0x04000BDC RID: 3036
		public float beamFleckChancePerTick;

		// Token: 0x04000BDD RID: 3037
		public float beamCurvature;

		// Token: 0x04000BDE RID: 3038
		public float beamChanceToStartFire;

		// Token: 0x04000BDF RID: 3039
		public float beamChanceToAttachFire;

		// Token: 0x04000BE0 RID: 3040
		public float beamStartOffset;

		// Token: 0x04000BE1 RID: 3041
		public float beamFullWidthRange;

		// Token: 0x04000BE2 RID: 3042
		public FleckDef beamLineFleckDef;

		// Token: 0x04000BE3 RID: 3043
		public SimpleCurve beamLineFleckChanceCurve;

		// Token: 0x04000BE4 RID: 3044
		public FloatRange beamFireSizeRange = FloatRange.ZeroToOne;

		// Token: 0x04000BE5 RID: 3045
		public SoundDef soundCastBeam;

		// Token: 0x04000BE6 RID: 3046
		public bool beamTargetsGround;

		// Token: 0x04000BE7 RID: 3047
		public bool beamSetsGroundOnFire;

		// Token: 0x04000BE8 RID: 3048
		public float beamTotalDamage;

		// Token: 0x04000BE9 RID: 3049
		public bool beamHitsNeighborCells;

		// Token: 0x04000BEA RID: 3050
		public bool beamCantHitWithinMinRange;

		// Token: 0x04000BEB RID: 3051
		public bool beamHitsNeighborCellsRequiresLOS;

		// Token: 0x04000BEC RID: 3052
		public bool ai_BeamIsIncendiary;

		// Token: 0x04000BED RID: 3053
		public float sprayWidth;

		// Token: 0x04000BEE RID: 3054
		public float sprayArching;

		// Token: 0x04000BEF RID: 3055
		public int sprayNumExtraCells;

		// Token: 0x04000BF0 RID: 3056
		public int sprayThicknessCells = 1;

		// Token: 0x04000BF1 RID: 3057
		public EffecterDef sprayEffecterDef;

		// Token: 0x04000BF2 RID: 3058
		public Color? highlightColor;

		// Token: 0x04000BF3 RID: 3059
		public Color? secondaryHighlightColor;

		// Token: 0x04000BF4 RID: 3060
		public ThingDef spawnDef;

		// Token: 0x04000BF5 RID: 3061
		public TaleDef colonyWideTaleDef;

		// Token: 0x04000BF6 RID: 3062
		public int affectedCellCount;

		// Token: 0x04000BF7 RID: 3063
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x04000BF8 RID: 3064
		public RulePackDef rangedFireRulepack;

		// Token: 0x04000BF9 RID: 3065
		public SoundDef soundLanding;

		// Token: 0x04000BFA RID: 3066
		public EffecterDef flightEffecterDef;

		// Token: 0x04000BFB RID: 3067
		public bool flyWithCarriedThing = true;

		// Token: 0x04000BFC RID: 3068
		public MechWorkModeDef workModeDef;

		// Token: 0x04000BFD RID: 3069
		public const float DefaultArmorPenetrationPerDamage = 0.015f;

		// Token: 0x04000BFE RID: 3070
		private const float VerbSelectionWeightFactor_BodyPart = 0.3f;

		// Token: 0x04000BFF RID: 3071
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;
	}
}
