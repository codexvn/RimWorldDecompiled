using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B29 RID: 6953
	public class WeaponTraitDef : Def
	{
		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x06009B4B RID: 39755 RVA: 0x002D12B7 File Offset: 0x002CF4B7
		public WeaponTraitWorker Worker
		{
			get
			{
				if (!ModLister.CheckRoyaltyOrOdyssey("Weapon traits"))
				{
					return null;
				}
				if (this.worker == null)
				{
					this.worker = (WeaponTraitWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x06009B4C RID: 39756 RVA: 0x002D12F8 File Offset: 0x002CF4F8
		public bool Overlaps(WeaponTraitDef other)
		{
			return other == this || (!this.exclusionTags.NullOrEmpty<string>() && !other.exclusionTags.NullOrEmpty<string>() && this.exclusionTags.Any((string x) => other.exclusionTags.Contains(x)));
		}

		// Token: 0x06009B4D RID: 39757 RVA: 0x002D1355 File Offset: 0x002CF555
		public override IEnumerable<string> ConfigErrors()
		{
			if (!typeof(WeaponTraitWorker).IsAssignableFrom(this.workerClass))
			{
				yield return string.Format("WeaponTraitDef {0} has worker class {1}, which is not deriving from {2}", this.defName, this.workerClass, typeof(WeaponTraitWorker).FullName);
			}
			if (this.commonality <= 0f)
			{
				yield return string.Format("WeaponTraitDef {0} has a commonality <= 0.", this.defName);
			}
			yield break;
		}

		// Token: 0x04006BA0 RID: 27552
		public Type workerClass = typeof(WeaponTraitWorker);

		// Token: 0x04006BA1 RID: 27553
		public WeaponCategoryDef weaponCategory;

		// Token: 0x04006BA2 RID: 27554
		public List<string> exclusionTags;

		// Token: 0x04006BA3 RID: 27555
		public float commonality;

		// Token: 0x04006BA4 RID: 27556
		public bool canGenerateAlone = true;

		// Token: 0x04006BA5 RID: 27557
		public DamageDef damageDefOverride;

		// Token: 0x04006BA6 RID: 27558
		public List<ExtraDamage> extraDamages;

		// Token: 0x04006BA7 RID: 27559
		public List<StatModifier> statOffsets;

		// Token: 0x04006BA8 RID: 27560
		public List<StatModifier> statFactors;

		// Token: 0x04006BA9 RID: 27561
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x04006BAA RID: 27562
		public float marketValueOffset;

		// Token: 0x04006BAB RID: 27563
		public float burstShotSpeedMultiplier = 1f;

		// Token: 0x04006BAC RID: 27564
		public float burstShotCountMultiplier = 1f;

		// Token: 0x04006BAD RID: 27565
		public float additionalStoppingPower;

		// Token: 0x04006BAE RID: 27566
		public bool ignoresAccuracyMaluses;

		// Token: 0x04006BAF RID: 27567
		public ColorDef forcedColor;

		// Token: 0x04006BB0 RID: 27568
		[MustTranslate]
		public List<string> traitAdjectives = new List<string>();

		// Token: 0x04006BB1 RID: 27569
		public List<HediffDef> equippedHediffs;

		// Token: 0x04006BB2 RID: 27570
		public List<HediffDef> bondedHediffs;

		// Token: 0x04006BB3 RID: 27571
		public ThoughtDef bondedThought;

		// Token: 0x04006BB4 RID: 27572
		public ThoughtDef killThought;

		// Token: 0x04006BB5 RID: 27573
		public bool neverBond;

		// Token: 0x04006BB6 RID: 27574
		public CompProperties_EquippableAbilityReloadable abilityProps;

		// Token: 0x04006BB7 RID: 27575
		private WeaponTraitWorker worker;
	}
}
