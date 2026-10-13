using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B31 RID: 6961
	public class XenotypeDef : Def
	{
		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x06009B77 RID: 39799 RVA: 0x002D1CA4 File Offset: 0x002CFEA4
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x06009B78 RID: 39800 RVA: 0x002D1CF1 File Offset: 0x002CFEF1
		public List<GeneDef> AllGenes
		{
			get
			{
				return this.genes;
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x06009B79 RID: 39801 RVA: 0x002D1CF9 File Offset: 0x002CFEF9
		public bool Archite
		{
			get
			{
				return this.AllGenes.Any((GeneDef g) => g.displayCategory == GeneCategoryDefOf.Archite);
			}
		}

		// Token: 0x06009B7A RID: 39802 RVA: 0x002D1D28 File Offset: 0x002CFF28
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (!this.genes.NullOrEmpty<GeneDef>())
			{
				if (this.descriptionHyperlinks == null)
				{
					this.descriptionHyperlinks = new List<DefHyperlink>();
				}
				foreach (GeneDef def in this.genes)
				{
					this.descriptionHyperlinks.Add(new DefHyperlink(def));
				}
			}
		}

		// Token: 0x06009B7B RID: 39803 RVA: 0x002D1DAC File Offset: 0x002CFFAC
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.nameMakerFemale != null)
			{
				return this.nameMakerFemale;
			}
			return this.nameMaker;
		}

		// Token: 0x06009B7C RID: 39804 RVA: 0x002D1DC7 File Offset: 0x002CFFC7
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Genes".Translate().CapitalizeFirst(), (from x in this.genes
			select x.label).ToCommaList(false, false).CapitalizeFirst(), "GenesDesc".Translate() + "\n\n" + (from x in this.genes
			select x.label).ToLineList("  - ", true), 1000, null, null, false, false);
			if (!this.genes.NullOrEmpty<GeneDef>())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "GenesAreInheritable".Translate(), this.inheritable.ToStringYesNo(), "GenesAreInheritableXenotypeDef".Translate(), 990, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06009B7D RID: 39805 RVA: 0x002D1DDE File Offset: 0x002CFFDE
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.iconPath.NullOrEmpty())
			{
				yield return "iconPath is empty.";
			}
			if (this.doubleXenotypeChances != null)
			{
				if (this.doubleXenotypeChances.Sum((XenotypeChance x) => x.chance) > 1f)
				{
					yield return "doubleXenotypeChance sum is > 1";
				}
				foreach (XenotypeChance xenotypeChance in this.doubleXenotypeChances)
				{
					if (!xenotypeChance.xenotype.inheritable)
					{
						yield return "doubleXenotypeChance contains non-inheritable xenotype " + xenotypeChance.xenotype.label;
					}
				}
				List<XenotypeChance>.Enumerator enumerator2 = default(List<XenotypeChance>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04006BEC RID: 27628
		public List<GeneDef> genes = new List<GeneDef>();

		// Token: 0x04006BED RID: 27629
		public bool inheritable;

		// Token: 0x04006BEE RID: 27630
		[NoTranslate]
		public string iconPath;

		// Token: 0x04006BEF RID: 27631
		[MustTranslate]
		public string descriptionShort;

		// Token: 0x04006BF0 RID: 27632
		public float combatPowerFactor = 1f;

		// Token: 0x04006BF1 RID: 27633
		public bool canGenerateAsCombatant = true;

		// Token: 0x04006BF2 RID: 27634
		public float factionlessGenerationWeight = 1f;

		// Token: 0x04006BF3 RID: 27635
		public RulePackDef nameMaker;

		// Token: 0x04006BF4 RID: 27636
		public RulePackDef nameMakerFemale;

		// Token: 0x04006BF5 RID: 27637
		public float chanceToUseNameMaker;

		// Token: 0x04006BF6 RID: 27638
		public List<WeaponClassDef> forbiddenWeaponClasses;

		// Token: 0x04006BF7 RID: 27639
		public float generateWithXenogermReplicatingHediffChance;

		// Token: 0x04006BF8 RID: 27640
		public FloatRange xenogermReplicatingDurationLeftDaysRange = FloatRange.Zero;

		// Token: 0x04006BF9 RID: 27641
		public SoundDef soundDefOnImplant;

		// Token: 0x04006BFA RID: 27642
		public List<XenotypeChance> doubleXenotypeChances;

		// Token: 0x04006BFB RID: 27643
		public float displayPriority;

		// Token: 0x04006BFC RID: 27644
		[Unsaved(false)]
		private Texture2D cachedIcon;

		// Token: 0x04006BFD RID: 27645
		public static readonly Color IconColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
