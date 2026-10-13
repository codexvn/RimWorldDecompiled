using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000908 RID: 2312
	public class Tool
	{
		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003BF0 RID: 15344 RVA: 0x0013A907 File Offset: 0x00138B07
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x0013A928 File Offset: 0x00138B28
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where this.capacities.Contains(x.requiredCapacity)
				select x;
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x0013A940 File Offset: 0x00138B40
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0013A96C File Offset: 0x00138B6C
		public float AdjustedBaseMeleeDamageAmount(Thing ownerEquipment, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipment != null)
			{
				num *= ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true, -1);
				if (ownerEquipment.Stuff != null && damageDef != null)
				{
					num *= ownerEquipment.Stuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0013A9BC File Offset: 0x00138BBC
		public float AdjustedBaseMeleeDamageAmount(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipmentStuff != null)
			{
				num *= ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, ownerEquipmentStuff);
				if (ownerEquipmentStuff != null && damageDef != null)
				{
					num *= ownerEquipmentStuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0013A9FE File Offset: 0x00138BFE
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true, -1));
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0013AA1E File Offset: 0x00138C1E
		public float AdjustedCooldown(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_CooldownMultiplier, ownerEquipmentStuff));
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x0013AA3D File Offset: 0x00138C3D
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x0013AA45 File Offset: 0x00138C45
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x0013AA53 File Offset: 0x00138C53
		public IEnumerable<string> ConfigErrors()
		{
			if (this.id.NullOrEmpty())
			{
				yield return "tool has null id (power=" + this.power.ToString("0.##") + ")";
			}
			yield break;
		}

		// Token: 0x04002AFC RID: 11004
		[Unsaved(false)]
		public string id;

		// Token: 0x04002AFD RID: 11005
		[MustTranslate]
		public string label;

		// Token: 0x04002AFE RID: 11006
		[MustTranslate]
		public string labelNoLocation;

		// Token: 0x04002AFF RID: 11007
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04002B00 RID: 11008
		public bool labelUsedInLogging = true;

		// Token: 0x04002B01 RID: 11009
		public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

		// Token: 0x04002B02 RID: 11010
		public float power;

		// Token: 0x04002B03 RID: 11011
		public float armorPenetration = -1f;

		// Token: 0x04002B04 RID: 11012
		public float cooldownTime;

		// Token: 0x04002B05 RID: 11013
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04002B06 RID: 11014
		public HediffDef hediff;

		// Token: 0x04002B07 RID: 11015
		public float chanceFactor = 1f;

		// Token: 0x04002B08 RID: 11016
		public bool alwaysTreatAsWeapon;

		// Token: 0x04002B09 RID: 11017
		public List<ExtraDamage> extraMeleeDamages;

		// Token: 0x04002B0A RID: 11018
		public SoundDef soundMeleeHit;

		// Token: 0x04002B0B RID: 11019
		public SoundDef soundMeleeMiss;

		// Token: 0x04002B0C RID: 11020
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04002B0D RID: 11021
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x04002B0E RID: 11022
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
