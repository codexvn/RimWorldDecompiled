using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024BC RID: 9404
	public class Pawn_GeneTracker : IExposable
	{
		// Token: 0x17001F29 RID: 7977
		// (get) Token: 0x0600CC60 RID: 52320 RVA: 0x003BE369 File Offset: 0x003BC569
		public XenotypeDef Xenotype
		{
			get
			{
				return this.xenotype ?? XenotypeDefOf.Baseliner;
			}
		}

		// Token: 0x17001F2A RID: 7978
		// (get) Token: 0x0600CC61 RID: 52321 RVA: 0x003BE37A File Offset: 0x003BC57A
		public bool UniqueXenotype
		{
			get
			{
				return !this.xenotypeName.NullOrEmpty();
			}
		}

		// Token: 0x17001F2B RID: 7979
		// (get) Token: 0x0600CC62 RID: 52322 RVA: 0x003BE38A File Offset: 0x003BC58A
		public string XenotypeLabel
		{
			get
			{
				if (!this.UniqueXenotype)
				{
					return this.Xenotype.label;
				}
				return this.xenotypeName ?? "Unique".Translate();
			}
		}

		// Token: 0x17001F2C RID: 7980
		// (get) Token: 0x0600CC63 RID: 52323 RVA: 0x003BE3B9 File Offset: 0x003BC5B9
		public string XenotypeLabelCap
		{
			get
			{
				return this.XenotypeLabel.CapitalizeFirst();
			}
		}

		// Token: 0x17001F2D RID: 7981
		// (get) Token: 0x0600CC64 RID: 52324 RVA: 0x003BE3C8 File Offset: 0x003BC5C8
		public CustomXenotype CustomXenotype
		{
			get
			{
				if (this.Xenotype != XenotypeDefOf.Baseliner || this.hybrid || Current.ProgramState != ProgramState.Playing)
				{
					return null;
				}
				if (this.cachedHasCustomXenotype == null)
				{
					this.cachedHasCustomXenotype = new bool?(false);
					foreach (CustomXenotype custom in Current.Game.customXenotypeDatabase.customXenotypes)
					{
						if (GeneUtility.PawnIsCustomXenotype(this.pawn, custom))
						{
							this.cachedHasCustomXenotype = new bool?(true);
							this.cachedCustomXenotype = custom;
							break;
						}
					}
				}
				return this.cachedCustomXenotype;
			}
		}

		// Token: 0x17001F2E RID: 7982
		// (get) Token: 0x0600CC65 RID: 52325 RVA: 0x003BE480 File Offset: 0x003BC680
		public string XenotypeDescShort
		{
			get
			{
				if (this.UniqueXenotype)
				{
					return "UniqueXenotypeDesc".Translate();
				}
				if (!this.Xenotype.descriptionShort.NullOrEmpty())
				{
					return this.Xenotype.descriptionShort + "\n\n" + "MoreInfoInInfoScreen".Translate().Colorize(ColoredText.SubtleGrayColor);
				}
				return this.Xenotype.description;
			}
		}

		// Token: 0x17001F2F RID: 7983
		// (get) Token: 0x0600CC66 RID: 52326 RVA: 0x003BE4EC File Offset: 0x003BC6EC
		public Texture2D XenotypeIcon
		{
			get
			{
				if (!ModsConfig.BiotechActive)
				{
					return null;
				}
				if (this.iconDef != null)
				{
					return this.iconDef.Icon;
				}
				if (this.UniqueXenotype)
				{
					return XenotypeIconDefOf.Basic.Icon;
				}
				return this.Xenotype.Icon;
			}
		}

		// Token: 0x17001F30 RID: 7984
		// (get) Token: 0x0600CC67 RID: 52327 RVA: 0x003BE529 File Offset: 0x003BC729
		public List<Gene> GenesListForReading
		{
			get
			{
				if (this.cachedGenes == null)
				{
					this.cachedGenes = new List<Gene>();
					this.cachedGenes.AddRange(this.xenogenes);
					this.cachedGenes.AddRange(this.endogenes);
				}
				return this.cachedGenes;
			}
		}

		// Token: 0x17001F31 RID: 7985
		// (get) Token: 0x0600CC68 RID: 52328 RVA: 0x003BE566 File Offset: 0x003BC766
		public List<Gene> Xenogenes
		{
			get
			{
				return this.xenogenes;
			}
		}

		// Token: 0x17001F32 RID: 7986
		// (get) Token: 0x0600CC69 RID: 52329 RVA: 0x003BE56E File Offset: 0x003BC76E
		public List<Gene> Endogenes
		{
			get
			{
				return this.endogenes;
			}
		}

		// Token: 0x17001F33 RID: 7987
		// (get) Token: 0x0600CC6A RID: 52330 RVA: 0x003BE578 File Offset: 0x003BC778
		public float PainOffset
		{
			get
			{
				float num = 0f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num += genesListForReading[i].def.painOffset;
					}
				}
				return num;
			}
		}

		// Token: 0x17001F34 RID: 7988
		// (get) Token: 0x0600CC6B RID: 52331 RVA: 0x003BE5D0 File Offset: 0x003BC7D0
		public float PainFactor
		{
			get
			{
				float num = 1f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.painFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x17001F35 RID: 7989
		// (get) Token: 0x0600CC6C RID: 52332 RVA: 0x003BE628 File Offset: 0x003BC828
		public float SocialFightChanceFactor
		{
			get
			{
				float num = 1f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.socialFightChanceFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x17001F36 RID: 7990
		// (get) Token: 0x0600CC6D RID: 52333 RVA: 0x003BE680 File Offset: 0x003BC880
		public float AggroMentalBreakSelectionChanceFactor
		{
			get
			{
				float num = 1f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.aggroMentalBreakSelectionChanceFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x17001F37 RID: 7991
		// (get) Token: 0x0600CC6E RID: 52334 RVA: 0x003BE6D8 File Offset: 0x003BC8D8
		public bool AffectedByDarkness
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return true;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.ignoreDarkness)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001F38 RID: 7992
		// (get) Token: 0x0600CC6F RID: 52335 RVA: 0x003BE72C File Offset: 0x003BC92C
		public float BiologicalAgeTickFactor
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return 1f;
				}
				if (this.cachedGenesAffectAge != null && !this.cachedGenesAffectAge.Value)
				{
					return 1f;
				}
				bool value = this.cachedGenesAffectAge.GetValueOrDefault();
				if (this.cachedGenesAffectAge == null)
				{
					value = false;
					this.cachedGenesAffectAge = new bool?(value);
				}
				float num = 1f;
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.biologicalAgeTickFactorFromAgeCurve != null)
					{
						num *= genesListForReading[i].def.biologicalAgeTickFactorFromAgeCurve.Evaluate(this.pawn.ageTracker.AgeBiologicalYearsFloat);
						this.cachedGenesAffectAge = new bool?(true);
					}
				}
				return num;
			}
		}

		// Token: 0x17001F39 RID: 7993
		// (get) Token: 0x0600CC70 RID: 52336 RVA: 0x003BE804 File Offset: 0x003BCA04
		public bool EnjoysSunlight
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return true;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].def.dislikesSunlight && genesListForReading[i].Active)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001F3A RID: 7994
		// (get) Token: 0x0600CC71 RID: 52337 RVA: 0x003BE858 File Offset: 0x003BCA58
		public bool ShouldHaveCallTracker
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return false;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.soundCall != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001F3B RID: 7995
		// (get) Token: 0x0600CC72 RID: 52338 RVA: 0x003BE8AC File Offset: 0x003BCAAC
		public bool DontMindRawFood
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return false;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.dontMindRawFood)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001F3C RID: 7996
		// (get) Token: 0x0600CC73 RID: 52339 RVA: 0x003BE900 File Offset: 0x003BCB00
		public bool CanHaveBeard
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return this.pawn.gender != Gender.Female;
				}
				if (this.pawn.gender == Gender.Female)
				{
					List<Gene> genesListForReading = this.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						if (genesListForReading[i].Active && genesListForReading[i].def.womenCanHaveBeards)
						{
							return true;
						}
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x17001F3D RID: 7997
		// (get) Token: 0x0600CC74 RID: 52340 RVA: 0x003BE974 File Offset: 0x003BCB74
		public float PrisonBreakIntervalFactor
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return 1f;
				}
				float num = 1f;
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.prisonBreakMTBFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x17001F3E RID: 7998
		// (get) Token: 0x0600CC75 RID: 52341 RVA: 0x003BE9D0 File Offset: 0x003BCBD0
		public WorkTags DisabledWorkTags
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return WorkTags.None;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				WorkTags workTags = WorkTags.None;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						workTags |= genesListForReading[i].def.disabledWorkTags;
					}
				}
				return workTags;
			}
		}

		// Token: 0x17001F3F RID: 7999
		// (get) Token: 0x0600CC76 RID: 52342 RVA: 0x003BEA24 File Offset: 0x003BCC24
		public bool TattoosVisible
		{
			get
			{
				if (this.cachedTattoosVisible != null)
				{
					return this.cachedTattoosVisible.Value;
				}
				this.cachedTattoosVisible = new bool?(true);
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && !genesListForReading[i].def.tattoosVisible)
					{
						this.cachedTattoosVisible = new bool?(false);
						break;
					}
				}
				return this.cachedTattoosVisible.Value;
			}
		}

		// Token: 0x17001F40 RID: 8000
		// (get) Token: 0x0600CC77 RID: 52343 RVA: 0x003BEAA8 File Offset: 0x003BCCA8
		public int? WaterCellCost
		{
			get
			{
				if (this.hasCachedWaterCost)
				{
					return this.cachedWaterCellCost;
				}
				this.hasCachedWaterCost = true;
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.waterCellCost != null)
					{
						int? waterCellCost = genesListForReading[i].def.waterCellCost;
						if (this.cachedWaterCellCost == null || waterCellCost.Value < this.cachedWaterCellCost.Value)
						{
							this.cachedWaterCellCost = waterCellCost;
						}
					}
				}
				return this.cachedWaterCellCost;
			}
		}

		// Token: 0x0600CC78 RID: 52344 RVA: 0x003BEB4C File Offset: 0x003BCD4C
		public Pawn_GeneTracker()
		{
		}

		// Token: 0x0600CC79 RID: 52345 RVA: 0x003BEBA4 File Offset: 0x003BCDA4
		public Pawn_GeneTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CC7A RID: 52346 RVA: 0x003BEC00 File Offset: 0x003BCE00
		public void SetXenotype(XenotypeDef xenotype)
		{
			if (!ModLister.CheckBiotech("Xenotypes"))
			{
				return;
			}
			this.xenotype = xenotype;
			this.xenotypeName = null;
			this.iconDef = null;
			this.cachedHasCustomXenotype = null;
			this.cachedCustomXenotype = null;
			this.ClearXenogenes();
			for (int i = 0; i < xenotype.genes.Count; i++)
			{
				this.AddGene(xenotype.genes[i], !xenotype.inheritable);
			}
		}

		// Token: 0x0600CC7B RID: 52347 RVA: 0x003BEC7A File Offset: 0x003BCE7A
		public void SetXenotypeDirect(XenotypeDef xenotype)
		{
			if (!ModLister.CheckBiotech("Xenotypes"))
			{
				return;
			}
			this.xenotype = xenotype;
			this.xenotypeName = null;
			this.cachedHasCustomXenotype = null;
			this.cachedCustomXenotype = null;
		}

		// Token: 0x0600CC7C RID: 52348 RVA: 0x003BECAC File Offset: 0x003BCEAC
		[Obsolete("Use HasActiveGene() instead")]
		public bool HasGene(GeneDef geneDef)
		{
			if (geneDef == null)
			{
				return false;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].def == geneDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CC7D RID: 52349 RVA: 0x003BECE8 File Offset: 0x003BCEE8
		public bool HasActiveGene(GeneDef geneDef)
		{
			if (geneDef == null)
			{
				return false;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].def == geneDef && genesListForReading[i].Active)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CC7E RID: 52350 RVA: 0x003BED32 File Offset: 0x003BCF32
		public bool TryGetNeedEnablingGene(NeedDef def, out Gene gene)
		{
			if (this.cachedEnabledNeeds == null)
			{
				gene = null;
				return false;
			}
			return this.cachedEnabledNeeds.TryGetValue(def, out gene);
		}

		// Token: 0x0600CC7F RID: 52351 RVA: 0x003BED4E File Offset: 0x003BCF4E
		public bool TryGetNeedDisablingGene(NeedDef def, out Gene gene)
		{
			if (this.cachedDisabledNeeds == null)
			{
				gene = null;
				return false;
			}
			return this.cachedDisabledNeeds.TryGetValue(def, out gene);
		}

		// Token: 0x0600CC80 RID: 52352 RVA: 0x003BED6A File Offset: 0x003BCF6A
		public bool EnablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Gene> dictionary = this.cachedEnabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x0600CC81 RID: 52353 RVA: 0x003BED7E File Offset: 0x003BCF7E
		public bool DisablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Gene> dictionary = this.cachedDisabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x0600CC82 RID: 52354 RVA: 0x003BED92 File Offset: 0x003BCF92
		public bool IsXenogene(Gene gene)
		{
			return this.xenogenes.Contains(gene);
		}

		// Token: 0x0600CC83 RID: 52355 RVA: 0x003BEDA0 File Offset: 0x003BCFA0
		public bool HasXenogene(GeneDef geneDef)
		{
			for (int i = 0; i < this.xenogenes.Count; i++)
			{
				if (this.xenogenes[i].def == geneDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CC84 RID: 52356 RVA: 0x003BEDDC File Offset: 0x003BCFDC
		public bool HasEndogene(GeneDef geneDef)
		{
			for (int i = 0; i < this.endogenes.Count; i++)
			{
				if (this.endogenes[i].def == geneDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CC85 RID: 52357 RVA: 0x003BEE18 File Offset: 0x003BD018
		public Gene GetGene(GeneDef geneDef)
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].def == geneDef)
				{
					return genesListForReading[i];
				}
			}
			return null;
		}

		// Token: 0x0600CC86 RID: 52358 RVA: 0x003BEE58 File Offset: 0x003BD058
		public T GetFirstGeneOfType<T>() where T : Gene
		{
			if (!ModLister.BiotechInstalled)
			{
				return default(T);
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					T t = genesListForReading[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x0600CC87 RID: 52359 RVA: 0x003BEEC4 File Offset: 0x003BD0C4
		public GeneDef GetMelaninGene()
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.endogeneCategory == EndogeneCategory.Melanin)
				{
					return genesListForReading[i].def;
				}
			}
			return null;
		}

		// Token: 0x0600CC88 RID: 52360 RVA: 0x003BEF1C File Offset: 0x003BD11C
		public GeneDef GetHairColorGene()
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.endogeneCategory == EndogeneCategory.HairColor)
				{
					return genesListForReading[i].def;
				}
			}
			return null;
		}

		// Token: 0x0600CC89 RID: 52361 RVA: 0x003BEF74 File Offset: 0x003BD174
		public GeneDef GetFirstEndogeneByCategory(EndogeneCategory cat)
		{
			List<Gene> list = this.Endogenes;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Active && list[i].def.endogeneCategory == cat)
				{
					return list[i].def;
				}
			}
			return null;
		}

		// Token: 0x0600CC8A RID: 52362 RVA: 0x003BEFC9 File Offset: 0x003BD1C9
		public Gene AddGene(GeneDef geneDef, bool xenogene)
		{
			if (xenogene && !ModLister.BiotechInstalled)
			{
				return null;
			}
			if (!xenogene && this.HasEndogene(geneDef))
			{
				return null;
			}
			return this.AddGene(GeneMaker.MakeGene(geneDef, this.pawn), xenogene);
		}

		// Token: 0x0600CC8B RID: 52363 RVA: 0x003BEFF8 File Offset: 0x003BD1F8
		private Gene AddGene(Gene gene, bool addAsXenogene)
		{
			if (addAsXenogene && !ModLister.BiotechInstalled)
			{
				return null;
			}
			if (addAsXenogene)
			{
				this.xenogenes.Add(gene);
			}
			else
			{
				this.endogenes.Add(gene);
			}
			this.cachedGenes = null;
			if (ModLister.BiotechInstalled)
			{
				this.CheckForOverrides();
				if (!gene.def.abilities.NullOrEmpty<AbilityDef>() && this.pawn.abilities != null)
				{
					for (int i = 0; i < gene.def.abilities.Count; i++)
					{
						this.pawn.abilities.GainAbility(gene.def.abilities[i]);
					}
				}
				if (!gene.def.forcedTraits.NullOrEmpty<GeneticTraitData>() && this.pawn.story != null)
				{
					for (int j = 0; j < gene.def.forcedTraits.Count; j++)
					{
						Trait trait = new Trait(gene.def.forcedTraits[j].def, gene.def.forcedTraits[j].degree, false);
						trait.sourceGene = gene;
						this.pawn.story.traits.GainTrait(trait, true);
					}
				}
				if (gene.def.passionMod != null)
				{
					SkillRecord skill = this.pawn.skills.GetSkill(gene.def.passionMod.skill);
					gene.passionPreAdd = new Passion?(skill.passion);
					skill.passion = gene.def.passionMod.NewPassionFor(skill);
				}
				Pawn_StoryTracker story = this.pawn.story;
				if (story != null)
				{
					TraitSet traits = story.traits;
					if (traits != null)
					{
						traits.RecalculateSuppression();
					}
				}
				if (gene.def.disabledWorkTags.HasFlag(WorkTags.Violent))
				{
					Pawn_EquipmentTracker equipment = this.pawn.equipment;
					if (((equipment != null) ? equipment.Primary : null) != null)
					{
						if (this.pawn.PositionHeld.IsValid && this.pawn.MapHeld != null)
						{
							ThingWithComps thingWithComps;
							this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, false);
						}
						else
						{
							this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
						}
					}
				}
			}
			this.Notify_GenesChanged(gene.def);
			if (ModLister.BiotechInstalled)
			{
				gene.PostAdd();
			}
			return gene;
		}

		// Token: 0x0600CC8C RID: 52364 RVA: 0x003BF26C File Offset: 0x003BD46C
		public void RemoveGene(Gene gene)
		{
			if (this.xenogenes.Remove(gene) || this.endogenes.Remove(gene))
			{
				this.cachedGenes = null;
				if (ModLister.BiotechInstalled)
				{
					this.CheckForOverrides();
					if (this.pawn.abilities != null && !gene.def.abilities.NullOrEmpty<AbilityDef>())
					{
						for (int i = 0; i < gene.def.abilities.Count; i++)
						{
							this.pawn.abilities.RemoveAbility(gene.def.abilities[i]);
						}
					}
					if (!gene.def.forcedTraits.NullOrEmpty<GeneticTraitData>() || !gene.def.suppressedTraits.NullOrEmpty<GeneticTraitData>())
					{
						Pawn_StoryTracker story = this.pawn.story;
						if (story != null)
						{
							story.traits.Notify_GeneRemoved(gene);
						}
					}
					if (gene.def.passionMod != null)
					{
						SkillRecord skill = this.pawn.skills.GetSkill(gene.def.passionMod.skill);
						skill.passion = gene.NewPassionForOnRemoval(skill);
					}
				}
				this.Notify_GenesChanged(gene.def);
				if (ModLister.BiotechInstalled)
				{
					gene.PostRemove();
				}
			}
		}

		// Token: 0x0600CC8D RID: 52365 RVA: 0x003BF3A0 File Offset: 0x003BD5A0
		private void CheckForOverrides()
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			foreach (Gene gene in genesListForReading)
			{
				if (!gene.def.RandomChosen)
				{
					gene.OverrideBy(null);
				}
			}
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (!genesListForReading[i].def.RandomChosen)
				{
					for (int j = i + 1; j < genesListForReading.Count; j++)
					{
						if (!genesListForReading[j].def.RandomChosen && genesListForReading[i].def.ConflictsWith(genesListForReading[j].def))
						{
							if (genesListForReading[i].def.Overrides(genesListForReading[j].def, this.IsXenogene(genesListForReading[i]), this.IsXenogene(genesListForReading[j])))
							{
								genesListForReading[j].OverrideBy(genesListForReading[i]);
							}
							else
							{
								genesListForReading[i].OverrideBy(genesListForReading[j]);
							}
						}
					}
				}
			}
			Pawn_SkillTracker skills = this.pawn.skills;
			if (skills != null)
			{
				skills.DirtyAptitudes();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
		}

		// Token: 0x0600CC8E RID: 52366 RVA: 0x003BF50C File Offset: 0x003BD70C
		public void ClearXenogenes()
		{
			for (int i = this.xenogenes.Count - 1; i >= 0; i--)
			{
				this.RemoveGene(this.xenogenes[i]);
			}
		}

		// Token: 0x0600CC8F RID: 52367 RVA: 0x003BF544 File Offset: 0x003BD744
		public void GeneTrackerTick()
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			for (int i = this.xenogenes.Count - 1; i >= 0; i--)
			{
				if (this.xenogenes[i].Active)
				{
					this.xenogenes[i].Tick();
				}
			}
			for (int j = this.endogenes.Count - 1; j >= 0; j--)
			{
				if (this.endogenes[j].Active)
				{
					this.endogenes[j].Tick();
				}
			}
		}

		// Token: 0x0600CC90 RID: 52368 RVA: 0x003BF5D4 File Offset: 0x003BD7D4
		public void GeneTrackerTickInterval(int delta)
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			for (int i = this.xenogenes.Count - 1; i >= 0; i--)
			{
				if (this.xenogenes[i].Active)
				{
					this.xenogenes[i].TickInterval(delta);
				}
			}
			for (int j = this.endogenes.Count - 1; j >= 0; j--)
			{
				if (this.endogenes[j].Active)
				{
					this.endogenes[j].TickInterval(delta);
				}
			}
			if (this.pawn.Spawned && this.Xenotype != XenotypeDefOf.Baseliner && this.pawn.IsHashIntervalTick(300, delta))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.GenesAndXenotypes, OpportunityType.Important);
			}
		}

		// Token: 0x0600CC91 RID: 52369 RVA: 0x003BF69C File Offset: 0x003BD89C
		public void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					genesListForReading[i].Notify_IngestedThing(thing, numTaken);
				}
			}
		}

		// Token: 0x0600CC92 RID: 52370 RVA: 0x003BF6E8 File Offset: 0x003BD8E8
		public void Reset()
		{
			for (int i = 0; i < this.GenesListForReading.Count; i++)
			{
				this.GenesListForReading[i].Reset();
			}
		}

		// Token: 0x0600CC93 RID: 52371 RVA: 0x003BF71C File Offset: 0x003BD91C
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.BiotechInstalled)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.GenesListForReading.Count; i = num + 1)
			{
				IEnumerable<Gizmo> gizmos = this.GenesListForReading[i].GetGizmos();
				if (gizmos != null)
				{
					foreach (Gizmo gizmo in gizmos)
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600CC94 RID: 52372 RVA: 0x003BF72C File Offset: 0x003BD92C
		private void Notify_GenesChanged(GeneDef addedOrRemovedGene)
		{
			Pawn_GeneTracker.<>c__DisplayClass97_0 CS$<>8__locals1 = new Pawn_GeneTracker.<>c__DisplayClass97_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.addedOrRemovedGene = addedOrRemovedGene;
			bool flag = false;
			this.cachedGenes = null;
			CS$<>8__locals1.genes = this.GenesListForReading;
			if (ModLister.BiotechInstalled && CS$<>8__locals1.addedOrRemovedGene.skinIsHairColor)
			{
				Gene gene;
				if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene x) => x.def.skinIsHairColor, out gene))
				{
					this.pawn.story.skinColorOverride = new Color?(this.pawn.story.HairColor);
					this.OverrideAllConflicting(gene);
				}
				else
				{
					this.pawn.story.skinColorOverride = null;
				}
				this.EnsureCorrectSkinColorOverride();
				flag = true;
			}
			if (CS$<>8__locals1.addedOrRemovedGene.hairColorOverride != null)
			{
				Gene gene2;
				if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.hairColorOverride != null, out gene2))
				{
					Color color = gene2.def.hairColorOverride.Value;
					if (gene2.def.randomBrightnessFactor != 0f)
					{
						color *= 1f + Rand.Range(-gene2.def.randomBrightnessFactor, gene2.def.randomBrightnessFactor);
					}
					this.pawn.story.HairColor = color.ClampToValueRange(GeneTuning.HairColorValueRange);
					this.OverrideAllConflicting(gene2);
					this.EnsureCorrectSkinColorOverride();
					flag = true;
				}
			}
			if (CS$<>8__locals1.addedOrRemovedGene.skinColorBase != null)
			{
				Gene gene3;
				if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.skinColorBase != null, out gene3))
				{
					this.OverrideAllConflicting(gene3);
					this.pawn.story.SkinColorBase = gene3.def.skinColorBase.Value;
					flag = true;
				}
			}
			if (ModLister.BiotechInstalled)
			{
				if (CS$<>8__locals1.addedOrRemovedGene.skinColorOverride != null)
				{
					Gene gene4;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.skinColorOverride != null, out gene4))
					{
						Color color2 = gene4.def.skinColorOverride.Value;
						if (gene4.def.randomBrightnessFactor != 0f)
						{
							color2 *= 1f + Rand.Range(-gene4.def.randomBrightnessFactor, gene4.def.randomBrightnessFactor);
						}
						this.pawn.story.skinColorOverride = new Color?(color2.ClampToValueRange(GeneTuning.SkinColorValueRange));
						this.OverrideAllConflicting(gene4);
					}
					else
					{
						this.pawn.story.skinColorOverride = null;
					}
					this.EnsureCorrectSkinColorOverride();
					flag = true;
				}
				if (CS$<>8__locals1.addedOrRemovedGene.bodyType != null && !this.pawn.DevelopmentalStage.Juvenile())
				{
					Gene gene5;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.bodyType != null, out gene5))
					{
						this.OverrideAllConflicting(gene5);
						this.pawn.story.bodyType = gene5.def.bodyType.Value.ToBodyType(this.pawn);
					}
					else
					{
						this.pawn.story.bodyType = PawnGenerator.GetBodyTypeFor(this.pawn);
					}
					flag = true;
				}
				if (!CS$<>8__locals1.addedOrRemovedGene.forcedHeadTypes.NullOrEmpty<HeadTypeDef>())
				{
					Gene gene6;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => !g.def.forcedHeadTypes.NullOrEmpty<HeadTypeDef>(), out gene6))
					{
						this.OverrideAllConflicting(gene6);
						if (!this.pawn.story.TryGetRandomHeadFromSet(gene6.def.forcedHeadTypes))
						{
							this.pawn.story.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
							where x.randomChosen
							select x);
						}
					}
					else
					{
						this.pawn.story.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
						where x.randomChosen
						select x);
					}
					flag = true;
				}
				if ((CS$<>8__locals1.addedOrRemovedGene.forcedHair != null || CS$<>8__locals1.addedOrRemovedGene.hairTagFilter != null) && !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.pawn.story.hairDef, null))
				{
					this.pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(this.pawn);
					flag = true;
				}
				if (CS$<>8__locals1.addedOrRemovedGene.beardTagFilter != null && this.pawn.style != null && !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.pawn.style.beardDef, null))
				{
					this.pawn.style.beardDef = PawnStyleItemChooser.RandomBeardFor(this.pawn);
					flag = true;
				}
				if (CS$<>8__locals1.addedOrRemovedGene.fur != null)
				{
					Gene gene7;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.fur != null, out gene7))
					{
						this.pawn.story.furDef = gene7.def.fur;
						this.OverrideAllConflicting(gene7);
					}
					else
					{
						this.pawn.story.furDef = null;
					}
					flag = true;
				}
				Gene gene8;
				if (CS$<>8__locals1.addedOrRemovedGene.RandomChosen && CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.Active && g.def.ConflictsWith(CS$<>8__locals1.addedOrRemovedGene), out gene8))
				{
					this.OverrideAllConflicting(gene8);
					flag = true;
				}
				this.RecacheNeeds();
				if (CS$<>8__locals1.addedOrRemovedGene.soundCall != null)
				{
					PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
				}
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
				this.pawn.health.hediffSet.DirtyCache();
				this.cachedDamageFactors.Clear();
				this.cachedAddictionChanceFactors.Clear();
				this.cachedGenesAffectAge = null;
				this.cachedHasCustomXenotype = null;
				this.cachedCustomXenotype = null;
				this.cachedTattoosVisible = null;
				this.cachedWaterCellCost = null;
				this.hasCachedWaterCost = false;
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills != null)
				{
					skills.DirtyAptitudes();
				}
				this.pawn.Notify_DisabledWorkTypesChanged();
			}
			if (flag)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x0600CC95 RID: 52373 RVA: 0x003BFDA0 File Offset: 0x003BDFA0
		private void EnsureCorrectSkinColorOverride()
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			bool flag = false;
			Color? skinColorOverride = null;
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				Gene gene = genesListForReading[i];
				if (gene.Active)
				{
					if (gene.def.skinIsHairColor)
					{
						flag = true;
					}
					else if (gene.def.skinColorOverride != null)
					{
						skinColorOverride = gene.def.skinColorOverride;
					}
				}
			}
			if (flag)
			{
				this.pawn.story.skinColorOverride = new Color?(this.pawn.story.HairColor);
				return;
			}
			this.pawn.story.skinColorOverride = skinColorOverride;
		}

		// Token: 0x0600CC96 RID: 52374 RVA: 0x003BFE58 File Offset: 0x003BE058
		private void OverrideAllConflicting(Gene gene)
		{
			if (!ModLister.BiotechInstalled || !gene.def.RandomChosen)
			{
				return;
			}
			gene.OverrideBy(null);
			foreach (Gene gene2 in this.GenesListForReading)
			{
				if (gene2 != gene && gene2.def.ConflictsWith(gene.def))
				{
					gene2.OverrideBy(gene);
				}
			}
		}

		// Token: 0x0600CC97 RID: 52375 RVA: 0x003BFEE0 File Offset: 0x003BE0E0
		public void Notify_NewColony()
		{
			foreach (Gene gene in this.GenesListForReading)
			{
				gene.Notify_NewColony();
			}
		}

		// Token: 0x0600CC98 RID: 52376 RVA: 0x003BFF30 File Offset: 0x003BE130
		public void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = genesListForReading.Count - 1; i >= 0; i--)
			{
				genesListForReading[i].Notify_PawnDied(dinfo, culprit);
			}
		}

		// Token: 0x0600CC99 RID: 52377 RVA: 0x003BFF68 File Offset: 0x003BE168
		public bool StyleItemAllowed(StyleItemDef styleItem)
		{
			if (!ModLister.BiotechInstalled)
			{
				return true;
			}
			bool flag = styleItem is HairDef;
			bool flag2 = styleItem is BeardDef;
			if (!flag && !flag2)
			{
				return true;
			}
			if (flag2 && !this.pawn.style.CanWantBeard)
			{
				return styleItem == BeardDefOf.NoBeard;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					if (flag)
					{
						if (genesListForReading[i].def.forcedHair != null && genesListForReading[i].def.forcedHair != styleItem)
						{
							return false;
						}
						if (genesListForReading[i].def.hairTagFilter != null && !genesListForReading[i].def.hairTagFilter.Allows(styleItem.styleTags))
						{
							return false;
						}
					}
					else if (flag2 && genesListForReading[i].def.beardTagFilter != null && !genesListForReading[i].def.beardTagFilter.Allows(styleItem.styleTags))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600CC9A RID: 52378 RVA: 0x003C0080 File Offset: 0x003BE280
		public float FactorForDamage(DamageInfo dinfo)
		{
			if (!ModLister.BiotechInstalled || dinfo.Def == null || this.GenesListForReading.NullOrEmpty<Gene>())
			{
				return 1f;
			}
			float result;
			if (this.cachedDamageFactors.TryGetValue(dinfo.Def, out result))
			{
				return result;
			}
			float num = 1f;
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				Gene gene = genesListForReading[i];
				if (gene.Active && !gene.def.damageFactors.NullOrEmpty<DamageFactor>())
				{
					for (int j = 0; j < gene.def.damageFactors.Count; j++)
					{
						if (gene.def.damageFactors[j].damageDef == dinfo.Def)
						{
							num *= gene.def.damageFactors[j].factor;
						}
					}
				}
			}
			this.cachedDamageFactors.Add(dinfo.Def, num);
			return num;
		}

		// Token: 0x0600CC9B RID: 52379 RVA: 0x003C0184 File Offset: 0x003BE384
		public float AddictionChanceFactor(ChemicalDef chemical)
		{
			if (!ModLister.BiotechInstalled || this.GenesListForReading.NullOrEmpty<Gene>())
			{
				return 1f;
			}
			float result;
			if (this.cachedAddictionChanceFactors.TryGetValue(chemical, out result))
			{
				return result;
			}
			float num = 1f;
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.chemical == chemical)
				{
					num *= genesListForReading[i].def.addictionChanceFactor;
				}
			}
			this.cachedAddictionChanceFactors.Add(chemical, num);
			return num;
		}

		// Token: 0x0600CC9C RID: 52380 RVA: 0x003C0220 File Offset: 0x003BE420
		public bool HediffGiversCanGive(HediffDef hediff)
		{
			if (!ModLister.BiotechInstalled)
			{
				return true;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.hediffGiversCannotGive != null && genesListForReading[i].def.hediffGiversCannotGive.Contains(hediff))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CC9D RID: 52381 RVA: 0x003C028C File Offset: 0x003BE48C
		public SoundDef GetSoundOverrideFromGenes(Func<GeneDef, SoundDef> getter, SoundDef oldSound)
		{
			if (!ModLister.BiotechInstalled)
			{
				return oldSound;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					SoundDef soundDef = getter(genesListForReading[i].def);
					if (soundDef != null)
					{
						return soundDef;
					}
				}
			}
			return oldSound;
		}

		// Token: 0x0600CC9E RID: 52382 RVA: 0x003C02E4 File Offset: 0x003BE4E4
		public void InitializeGenesFromOldSave(float melanin)
		{
			if (this.GetMelaninGene() == null)
			{
				this.AddGene(PawnSkinColors.GetSkinColorGene(melanin), false);
			}
			if (this.GetHairColorGene() == null)
			{
				GeneDef geneDef = PawnHairColors.ClosestHairColorGene(this.pawn.story.HairColor, this.pawn.story.SkinColorBase);
				if (geneDef != null)
				{
					this.AddGene(geneDef, false);
				}
			}
		}

		// Token: 0x0600CC9F RID: 52383 RVA: 0x003C0341 File Offset: 0x003BE541
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (!ModLister.BiotechInstalled)
			{
				yield break;
			}
			foreach (Gene gene in this.GenesListForReading)
			{
				if (gene.Active)
				{
					IEnumerable<StatDrawEntry> enumerable = gene.SpecialDisplayStats();
					if (enumerable != null)
					{
						foreach (StatDrawEntry statDrawEntry in enumerable)
						{
							yield return statDrawEntry;
						}
						IEnumerator<StatDrawEntry> enumerator2 = null;
					}
				}
			}
			List<Gene>.Enumerator enumerator = default(List<Gene>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CCA0 RID: 52384 RVA: 0x003C0354 File Offset: 0x003BE554
		public void ExposeData()
		{
			Scribe_Collections.Look<Gene>(ref this.xenogenes, "xenogenes", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Gene>(ref this.endogenes, "endogenes", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Values.Look<string>(ref this.xenotypeName, "xenotypeName", null, false);
			Scribe_Values.Look<bool>(ref this.hybrid, "hybrid", false, false);
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.xenogenes != null)
				{
					if (this.xenogenes.RemoveAll((Gene x) => x == null || x.def == null) > 0)
					{
						goto IL_E2;
					}
				}
				if (this.endogenes == null)
				{
					goto IL_EC;
				}
				if (this.endogenes.RemoveAll((Gene x) => x == null || x.def == null) <= 0)
				{
					goto IL_EC;
				}
				IL_E2:
				Log.Error("Removed null gene(s)");
			}
			IL_EC:
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.xenotype == null && ModsConfig.BiotechActive)
				{
					this.xenotype = XenotypeDefOf.Baseliner;
				}
				if (this.xenogenes == null)
				{
					this.xenogenes = new List<Gene>();
				}
				if (this.endogenes == null)
				{
					this.endogenes = new List<Gene>();
				}
				this.RecacheNeeds();
			}
		}

		// Token: 0x0600CCA1 RID: 52385 RVA: 0x003C049C File Offset: 0x003BE69C
		private void RecacheNeeds()
		{
			Dictionary<NeedDef, Gene> dictionary = this.cachedDisabledNeeds;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			Dictionary<NeedDef, Gene> dictionary2 = this.cachedEnabledNeeds;
			if (dictionary2 != null)
			{
				dictionary2.Clear();
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				Gene gene = genesListForReading[i];
				if (gene.Active && !gene.def.enablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int j = 0; j < gene.def.enablesNeeds.Count; j++)
					{
						NeedDef key = gene.def.enablesNeeds[j];
						if (this.cachedEnabledNeeds == null)
						{
							this.cachedEnabledNeeds = new Dictionary<NeedDef, Gene>();
						}
						this.cachedEnabledNeeds[key] = gene;
					}
				}
				if (gene.Active && !gene.def.disablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int k = 0; k < gene.def.disablesNeeds.Count; k++)
					{
						NeedDef key2 = gene.def.disablesNeeds[k];
						if (this.cachedDisabledNeeds == null)
						{
							this.cachedDisabledNeeds = new Dictionary<NeedDef, Gene>();
						}
						this.cachedDisabledNeeds[key2] = gene;
					}
				}
			}
		}

		// Token: 0x0600CCA2 RID: 52386 RVA: 0x003C05C8 File Offset: 0x003BE7C8
		public void Debug_AddAllGenes(bool xenogene)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			this.ClearXenogenes();
			this.cachedGenes = null;
			foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefs)
			{
				if (geneDef.endogeneCategory != EndogeneCategory.Melanin && geneDef.endogeneCategory != EndogeneCategory.HairColor)
				{
					Gene gene = GeneMaker.MakeGene(geneDef, this.pawn);
					if (xenogene)
					{
						this.xenogenes.Add(gene);
					}
					else
					{
						this.endogenes.Add(gene);
					}
					this.Notify_GenesChanged(geneDef);
					gene.PostAdd();
				}
			}
			this.CheckForOverrides();
		}

		// Token: 0x04008B4E RID: 35662
		public Pawn pawn;

		// Token: 0x04008B4F RID: 35663
		public string xenotypeName;

		// Token: 0x04008B50 RID: 35664
		public XenotypeIconDef iconDef;

		// Token: 0x04008B51 RID: 35665
		public bool hybrid;

		// Token: 0x04008B52 RID: 35666
		private XenotypeDef xenotype;

		// Token: 0x04008B53 RID: 35667
		private List<Gene> xenogenes = new List<Gene>();

		// Token: 0x04008B54 RID: 35668
		private List<Gene> endogenes = new List<Gene>();

		// Token: 0x04008B55 RID: 35669
		[Unsaved(false)]
		private Dictionary<DamageDef, float> cachedDamageFactors = new Dictionary<DamageDef, float>();

		// Token: 0x04008B56 RID: 35670
		[Unsaved(false)]
		private Dictionary<ChemicalDef, float> cachedAddictionChanceFactors = new Dictionary<ChemicalDef, float>();

		// Token: 0x04008B57 RID: 35671
		[Unsaved(false)]
		private Dictionary<NeedDef, Gene> cachedEnabledNeeds;

		// Token: 0x04008B58 RID: 35672
		[Unsaved(false)]
		private Dictionary<NeedDef, Gene> cachedDisabledNeeds;

		// Token: 0x04008B59 RID: 35673
		[Unsaved(false)]
		private List<Gene> cachedGenes;

		// Token: 0x04008B5A RID: 35674
		[Unsaved(false)]
		private bool? cachedGenesAffectAge;

		// Token: 0x04008B5B RID: 35675
		[Unsaved(false)]
		private CustomXenotype cachedCustomXenotype;

		// Token: 0x04008B5C RID: 35676
		[Unsaved(false)]
		private bool? cachedHasCustomXenotype;

		// Token: 0x04008B5D RID: 35677
		[Unsaved(false)]
		private bool? cachedTattoosVisible;

		// Token: 0x04008B5E RID: 35678
		[Unsaved(false)]
		private int? cachedWaterCellCost;

		// Token: 0x04008B5F RID: 35679
		[Unsaved(false)]
		private bool hasCachedWaterCost;

		// Token: 0x04008B60 RID: 35680
		private const int LearningOpportunityCheckInterval = 300;

		// Token: 0x04008B61 RID: 35681
		private List<GeneDefWithType> tmpGeneDefWithTypes = new List<GeneDefWithType>();

		// Token: 0x04008B62 RID: 35682
		private readonly List<Gene> tmpGenes = new List<Gene>();
	}
}
