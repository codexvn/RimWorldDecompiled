using System;

namespace Verse
{
	// Token: 0x020006B8 RID: 1720
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x000E13E3 File Offset: 0x000DF5E3
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002AFC RID: 11004 RVA: 0x000E13F0 File Offset: 0x000DF5F0
		// (set) Token: 0x06002AFD RID: 11005 RVA: 0x000E13F8 File Offset: 0x000DF5F8
		public bool IsPermanent
		{
			get
			{
				return this.isPermanentInt;
			}
			set
			{
				if (value == this.isPermanentInt)
				{
					return;
				}
				this.isPermanentInt = value;
				if (this.isPermanentInt)
				{
					this.permanentDamageThreshold = 9999f;
					this.SetPainCategory(HealthTuning.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted cat) => cat.weight).category);
				}
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002AFE RID: 11006 RVA: 0x000E145D File Offset: 0x000DF65D
		public PainCategory PainCategory
		{
			get
			{
				return this.painCategory;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x000E1465 File Offset: 0x000DF665
		public float PainFactor
		{
			get
			{
				return (float)this.painCategory;
			}
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000E146E File Offset: 0x000DF66E
		public void SetPainCategory(PainCategory category)
		{
			this.painCategory = category;
			if (base.Pawn != null)
			{
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000E1498 File Offset: 0x000DF698
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isPermanentInt, "isPermanent", false, false);
			Scribe_Values.Look<float>(ref this.permanentDamageThreshold, "permanentDamageThreshold", 9999f, false);
			Scribe_Values.Look<PainCategory>(ref this.painCategory, "painCategory", PainCategory.Painless, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000E14E8 File Offset: 0x000DF6E8
		public void PreFinalizeInjury()
		{
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				return;
			}
			if (ModsConfig.BiotechActive && this.parent.pawn.genes != null)
			{
				if (this.parent.pawn.genes.GenesListForReading.Any((Gene x) => x.def.preventPermanentWounds))
				{
					return;
				}
			}
			float num = 0.02f * this.parent.Part.def.permanentInjuryChanceFactor * this.Props.becomePermanentChanceFactor;
			if (!this.parent.Part.def.delicate)
			{
				num *= HealthTuning.BecomePermanentChanceFactorBySeverityCurve.Evaluate(this.parent.Severity);
			}
			if (Rand.Chance(num))
			{
				if (this.parent.Part.def.delicate)
				{
					this.IsPermanent = true;
					return;
				}
				this.permanentDamageThreshold = Rand.Range(1f, this.parent.Severity / 2f);
			}
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000E160C File Offset: 0x000DF80C
		public override void CompPostInjuryHeal(float amount)
		{
			if (this.permanentDamageThreshold >= 9999f || this.IsPermanent)
			{
				return;
			}
			if (this.parent.Severity <= this.permanentDamageThreshold && this.parent.Severity >= this.permanentDamageThreshold - amount)
			{
				this.parent.Severity = this.permanentDamageThreshold;
				this.IsPermanent = true;
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000E1688 File Offset: 0x000DF888
		public override void CopyFrom(HediffComp other)
		{
			HediffComp_GetsPermanent hediffComp_GetsPermanent = other as HediffComp_GetsPermanent;
			if (hediffComp_GetsPermanent != null)
			{
				this.isPermanentInt = hediffComp_GetsPermanent.isPermanentInt;
				this.painCategory = hediffComp_GetsPermanent.painCategory;
			}
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000E16B8 File Offset: 0x000DF8B8
		public override string CompDebugString()
		{
			return string.Concat(new string[]
			{
				"isPermanent: ",
				this.isPermanentInt.ToString(),
				"\npermanentDamageThreshold: ",
				this.permanentDamageThreshold.ToString(),
				"\npainCategory: ",
				this.painCategory.ToString()
			});
		}

		// Token: 0x040022B5 RID: 8885
		public float permanentDamageThreshold = 9999f;

		// Token: 0x040022B6 RID: 8886
		public bool isPermanentInt;

		// Token: 0x040022B7 RID: 8887
		private PainCategory painCategory;

		// Token: 0x040022B8 RID: 8888
		private const float NonActivePermanentDamageThresholdValue = 9999f;
	}
}
