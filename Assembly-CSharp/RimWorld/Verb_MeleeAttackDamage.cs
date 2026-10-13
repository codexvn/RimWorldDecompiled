using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003787 RID: 14215
	public class Verb_MeleeAttackDamage : Verb_MeleeAttack
	{
		// Token: 0x06013B14 RID: 80660 RVA: 0x005D9D6A File Offset: 0x005D7F6A
		private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
		{
			float num = this.verbProps.AdjustedMeleeDamageAmount(this, this.CasterPawn);
			float armorPenetration = this.verbProps.AdjustedArmorPenetration(this, this.CasterPawn);
			DamageDef def = this.verbProps.meleeDamageDef;
			BodyPartGroupDef bodyPartGroupDef = null;
			HediffDef hediffDef = null;
			QualityCategory weaponQuality = QualityCategory.Normal;
			num = Rand.Range(num * 0.8f, num * 1.2f);
			if (this.CasterIsPawn)
			{
				bodyPartGroupDef = this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool);
				if (num >= 1f)
				{
					if (base.HediffCompSource != null)
					{
						hediffDef = base.HediffCompSource.Def;
					}
				}
				else
				{
					num = 1f;
					def = DamageDefOf.Blunt;
				}
			}
			ThingDef source;
			if (base.EquipmentSource != null)
			{
				source = base.EquipmentSource.def;
				base.EquipmentSource.TryGetQuality(out weaponQuality);
			}
			else
			{
				source = this.CasterPawn.def;
			}
			Vector3 direction = (target.Thing.Position - this.CasterPawn.Position).ToVector3();
			Pawn pawn = this.caster as Pawn;
			bool instigatorGuilty = pawn == null || !pawn.Drafted;
			DamageInfo damageInfo = new DamageInfo(def, num, armorPenetration, -1f, this.caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null, instigatorGuilty, true, QualityCategory.Normal, true, false);
			damageInfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			damageInfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
			damageInfo.SetWeaponHediff(hediffDef);
			damageInfo.SetAngle(direction);
			damageInfo.SetTool(this.tool);
			damageInfo.SetWeaponQuality(weaponQuality);
			yield return damageInfo;
			if (this.tool != null && this.tool.extraMeleeDamages != null)
			{
				foreach (ExtraDamage extraDamage in this.tool.extraMeleeDamages)
				{
					if (Rand.Chance(extraDamage.chance))
					{
						num = extraDamage.amount;
						num = Rand.Range(num * 0.8f, num * 1.2f);
						damageInfo = new DamageInfo(extraDamage.def, num, extraDamage.AdjustedArmorPenetration(this, this.CasterPawn), -1f, this.caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
						damageInfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
						damageInfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
						damageInfo.SetWeaponHediff(hediffDef);
						damageInfo.SetAngle(direction);
						yield return damageInfo;
					}
				}
				List<ExtraDamage>.Enumerator enumerator = default(List<ExtraDamage>.Enumerator);
			}
			if (this.surpriseAttack && ((this.verbProps.surpriseAttack != null && !this.verbProps.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraDamage>()) || (this.tool != null && this.tool.surpriseAttack != null && !this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraDamage>())))
			{
				IEnumerable<ExtraDamage> enumerable = Enumerable.Empty<ExtraDamage>();
				if (this.verbProps.surpriseAttack != null && this.verbProps.surpriseAttack.extraMeleeDamages != null)
				{
					enumerable = enumerable.Concat(this.verbProps.surpriseAttack.extraMeleeDamages);
				}
				if (this.tool != null && this.tool.surpriseAttack != null && !this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraDamage>())
				{
					enumerable = enumerable.Concat(this.tool.surpriseAttack.extraMeleeDamages);
				}
				foreach (ExtraDamage extraDamage2 in enumerable)
				{
					int num2 = GenMath.RoundRandom(extraDamage2.AdjustedDamageAmount(this, this.CasterPawn));
					float armorPenetration2 = extraDamage2.AdjustedArmorPenetration(this, this.CasterPawn);
					DamageInfo damageInfo2 = new DamageInfo(extraDamage2.def, (float)num2, armorPenetration2, -1f, this.caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					damageInfo2.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
					damageInfo2.SetWeaponBodyPartGroup(bodyPartGroupDef);
					damageInfo2.SetWeaponHediff(hediffDef);
					damageInfo2.SetAngle(direction);
					yield return damageInfo2;
				}
				IEnumerator<ExtraDamage> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06013B15 RID: 80661 RVA: 0x005D9D84 File Offset: 0x005D7F84
		protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			DamageWorker.DamageResult result = new DamageWorker.DamageResult();
			foreach (DamageInfo dinfo in this.DamageInfosToApply(target))
			{
				if (target.ThingDestroyed)
				{
					break;
				}
				result = target.Thing.TakeDamage(dinfo);
			}
			return result;
		}

		// Token: 0x0400DA0E RID: 55822
		private const float MeleeDamageRandomFactorMin = 0.8f;

		// Token: 0x0400DA0F RID: 55823
		private const float MeleeDamageRandomFactorMax = 1.2f;
	}
}
