using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002437 RID: 9271
	public class Pawn_StoryTracker : IExposable
	{
		// Token: 0x17001E3E RID: 7742
		// (get) Token: 0x0600C89C RID: 51356 RVA: 0x003A9AB5 File Offset: 0x003A7CB5
		// (set) Token: 0x0600C89D RID: 51357 RVA: 0x003A9AC0 File Offset: 0x003A7CC0
		public BackstoryDef Childhood
		{
			get
			{
				return this.childhood;
			}
			set
			{
				if (this.pawn.DevelopmentalStage.Adult() && value.spawnCategories.Contains("Child"))
				{
					Log.Warning(string.Format("Assigning active child backstory to adult pawn {0}.", this.pawn));
				}
				this.backstoriesCache = null;
				this.childhood = value;
			}
		}

		// Token: 0x17001E3F RID: 7743
		// (get) Token: 0x0600C89E RID: 51358 RVA: 0x003A9B14 File Offset: 0x003A7D14
		// (set) Token: 0x0600C89F RID: 51359 RVA: 0x003A9B1C File Offset: 0x003A7D1C
		public BackstoryDef Adulthood
		{
			get
			{
				return this.adulthood;
			}
			set
			{
				this.backstoriesCache = null;
				this.adulthood = value;
			}
		}

		// Token: 0x17001E40 RID: 7744
		// (get) Token: 0x0600C8A0 RID: 51360 RVA: 0x003A9B2C File Offset: 0x003A7D2C
		// (set) Token: 0x0600C8A1 RID: 51361 RVA: 0x003A9B43 File Offset: 0x003A7D43
		public string Title
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				return this.TitleDefault;
			}
			set
			{
				this.title = null;
				if (value != this.Title && !value.NullOrEmpty())
				{
					this.title = value;
				}
			}
		}

		// Token: 0x17001E41 RID: 7745
		// (get) Token: 0x0600C8A2 RID: 51362 RVA: 0x003A9B69 File Offset: 0x003A7D69
		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

		// Token: 0x17001E42 RID: 7746
		// (get) Token: 0x0600C8A3 RID: 51363 RVA: 0x003A9B78 File Offset: 0x003A7D78
		public string TitleDefault
		{
			get
			{
				if (this.adulthood != null)
				{
					return this.adulthood.TitleFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleFor(this.pawn.gender);
				}
				return "";
			}
		}

		// Token: 0x17001E43 RID: 7747
		// (get) Token: 0x0600C8A4 RID: 51364 RVA: 0x003A9BC8 File Offset: 0x003A7DC8
		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

		// Token: 0x17001E44 RID: 7748
		// (get) Token: 0x0600C8A5 RID: 51365 RVA: 0x003A9BD8 File Offset: 0x003A7DD8
		public string TitleShort
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				if (this.adulthood != null)
				{
					return this.adulthood.TitleShortFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleShortFor(this.pawn.gender);
				}
				return "";
			}
		}

		// Token: 0x17001E45 RID: 7749
		// (get) Token: 0x0600C8A6 RID: 51366 RVA: 0x003A9C37 File Offset: 0x003A7E37
		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		// Token: 0x17001E46 RID: 7750
		// (get) Token: 0x0600C8A7 RID: 51367 RVA: 0x003A9C44 File Offset: 0x003A7E44
		// (set) Token: 0x0600C8A8 RID: 51368 RVA: 0x003A9CBA File Offset: 0x003A7EBA
		public Color SkinColorBase
		{
			get
			{
				if (this.skinColorBase == null && this.pawn.genes != null)
				{
					GeneDef melaninGene = this.pawn.genes.GetMelaninGene();
					if (melaninGene != null)
					{
						this.skinColorBase = melaninGene.skinColorBase;
					}
					else
					{
						GeneDef geneDef = PawnSkinColors.RandomSkinColorGene(this.pawn);
						if (geneDef != null)
						{
							this.pawn.genes.AddGene(geneDef, false);
						}
					}
				}
				return this.skinColorBase.Value;
			}
			set
			{
				this.skinColorBase = new Color?(value);
			}
		}

		// Token: 0x17001E47 RID: 7751
		// (get) Token: 0x0600C8A9 RID: 51369 RVA: 0x003A9CC8 File Offset: 0x003A7EC8
		public Color SkinColor
		{
			get
			{
				Color baseColor = this.skinColorOverride ?? this.SkinColorBase;
				if (this.pawn.IsMutant)
				{
					baseColor = MutantUtility.GetMutantSkinColor(this.pawn, baseColor);
				}
				return this.pawn.health.hediffSet.GetSkinColor(baseColor);
			}
		}

		// Token: 0x17001E48 RID: 7752
		// (get) Token: 0x0600C8AA RID: 51370 RVA: 0x003A9D27 File Offset: 0x003A7F27
		public bool SkinColorOverriden
		{
			get
			{
				return this.skinColorOverride != null || (this.pawn.IsMutant && this.pawn.mutant.Def.skinColorOverride != null);
			}
		}

		// Token: 0x17001E49 RID: 7753
		// (get) Token: 0x0600C8AB RID: 51371 RVA: 0x003A9D61 File Offset: 0x003A7F61
		public bool CaresAboutOthersAppearance
		{
			get
			{
				return !this.pawn.Inhumanized() && !this.traits.HasTrait(TraitDefOf.Kind) && !this.traits.HasTrait(TraitDefOf.Ascetic);
			}
		}

		// Token: 0x17001E4A RID: 7754
		// (get) Token: 0x0600C8AC RID: 51372 RVA: 0x003A9D98 File Offset: 0x003A7F98
		// (set) Token: 0x0600C8AD RID: 51373 RVA: 0x003A9E44 File Offset: 0x003A8044
		public Color HairColor
		{
			get
			{
				if (this.pawn.Corpse != null && this.pawn.Corpse.CurRotDrawMode == RotDrawMode.Rotting)
				{
					return PawnRenderUtility.GetRottenColor(this.hairColor);
				}
				if (ModsConfig.AnomalyActive && this.pawn.IsMutant && this.pawn.mutant.Def.useCorpseGraphics && this.pawn.mutant.rotStage == RotStage.Rotting)
				{
					return PawnRenderUtility.GetRottenColor(this.hairColor);
				}
				if (ModsConfig.AnomalyActive && this.pawn.IsShambler)
				{
					return MutantUtility.GetShamblerColor(this.hairColor);
				}
				return this.hairColor;
			}
			set
			{
				this.hairColor = value;
				if (ModsConfig.BiotechActive && this.pawn.genes != null)
				{
					foreach (Gene gene in this.pawn.genes.GenesListForReading)
					{
						if (gene.def.skinIsHairColor && gene.Active)
						{
							this.skinColorOverride = new Color?(this.hairColor);
							break;
						}
					}
				}
			}
		}

		// Token: 0x17001E4B RID: 7755
		// (get) Token: 0x0600C8AE RID: 51374 RVA: 0x003A9EE0 File Offset: 0x003A80E0
		public List<BackstoryDef> AllBackstories
		{
			get
			{
				if (this.backstoriesCache == null)
				{
					this.backstoriesCache = new List<BackstoryDef>();
					if (this.childhood != null)
					{
						this.backstoriesCache.Add(this.childhood);
					}
					if (this.adulthood != null)
					{
						this.backstoriesCache.Add(this.adulthood);
					}
				}
				return this.backstoriesCache;
			}
		}

		// Token: 0x17001E4C RID: 7756
		// (get) Token: 0x0600C8AF RID: 51375 RVA: 0x003A9F38 File Offset: 0x003A8138
		public WorkTags DisabledWorkTagsBackstoryAndTraits
		{
			get
			{
				WorkTags workTags = WorkTags.None;
				if (this.childhood != null)
				{
					workTags |= this.childhood.workDisables;
				}
				if (this.adulthood != null)
				{
					workTags |= this.adulthood.workDisables;
				}
				for (int i = 0; i < this.traits.allTraits.Count; i++)
				{
					if (!this.traits.allTraits[i].Suppressed)
					{
						workTags |= this.traits.allTraits[i].def.disabledWorkTags;
					}
				}
				return workTags;
			}
		}

		// Token: 0x17001E4D RID: 7757
		// (get) Token: 0x0600C8B0 RID: 51376 RVA: 0x003A9FC8 File Offset: 0x003A81C8
		public WorkTags DisabledWorkTagsBackstoryTraitsAndGenes
		{
			get
			{
				WorkTags workTags = this.DisabledWorkTagsBackstoryAndTraits;
				if (this.pawn.genes != null)
				{
					workTags |= this.pawn.genes.DisabledWorkTags;
				}
				return workTags;
			}
		}

		// Token: 0x17001E4E RID: 7758
		// (get) Token: 0x0600C8B1 RID: 51377 RVA: 0x003AA000 File Offset: 0x003A8200
		public float VoicePitchFactor
		{
			get
			{
				return Pawn_StoryTracker.VoicePitchFactorRange.RandomInRangeSeeded(this.pawn.thingIDNumber);
			}
		}

		// Token: 0x17001E4F RID: 7759
		// (get) Token: 0x0600C8B2 RID: 51378 RVA: 0x003AA025 File Offset: 0x003A8225
		public bool IsDisturbing
		{
			get
			{
				return (ModsConfig.AnomalyActive && this.traits.HasTrait(TraitDefOf.Disturbing)) || this.pawn.health.hediffSet.HasHediff(HediffDefOf.VoidTouched, false);
			}
		}

		// Token: 0x0600C8B3 RID: 51379 RVA: 0x003AA05D File Offset: 0x003A825D
		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

		// Token: 0x0600C8B4 RID: 51380 RVA: 0x003AA090 File Offset: 0x003A8290
		public void ExposeData()
		{
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<Color?>(ref this.skinColorOverride, "skinColorOverride", null, false);
			Scribe_Deep.Look<TraitSet>(ref this.traits, "traits", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.birthLastName, "birthLastName", null, false);
			Scribe_Defs.Look<ColorDef>(ref this.favoriteColor, "favoriteColorDef");
			Scribe_Defs.Look<FurDef>(ref this.furDef, "furDef");
			Scribe_Defs.Look<HeadTypeDef>(ref this.headType, "headType");
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Values.Look<float>(ref this.melanin, "melanin", -1f, false);
				if (Scribe.mode == LoadSaveMode.PostLoadInit && this.melanin >= 0f)
				{
					this.pawn.genes.InitializeGenesFromOldSave(this.melanin);
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				string headGraphicPath = null;
				Scribe_Values.Look<string>(ref headGraphicPath, "headGraphicPath", null, false);
				if (!headGraphicPath.NullOrEmpty())
				{
					this.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
					where x.graphicPath == headGraphicPath
					select x);
				}
				if (ModsConfig.IdeologyActive && this.favoriteColor == null)
				{
					Color? color2 = null;
					Scribe_Values.Look<Color?>(ref color2, "favoriteColor", null, false);
					if (color2 != null)
					{
						Color color = color2.Value;
						this.favoriteColor = DefDatabase<ColorDef>.AllDefsListForReading.FirstOrDefault((ColorDef cd) => cd.color.IndistinguishableFrom(color));
						if (this.favoriteColor == null)
						{
							this.favoriteColor = DefDatabase<ColorDef>.AllDefs.Where(delegate(ColorDef x)
							{
								ColorType colorType = x.colorType;
								return colorType == ColorType.Ideo || colorType == ColorType.Misc;
							}).RandomElement<ColorDef>();
						}
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.birthLastName == null)
				{
					NameTriple nameTriple = this.pawn.Name as NameTriple;
					if (nameTriple != null)
					{
						this.birthLastName = nameTriple.Last;
					}
				}
				if (this.hairDef == null)
				{
					this.hairDef = DefDatabase<HairDef>.AllDefs.RandomElement<HairDef>();
				}
				if (this.headType == null)
				{
					this.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
					where x.randomChosen
					select x);
				}
				if (ModsConfig.BiotechActive && this.pawn.DevelopmentalStage.Child() && this.bodyType != BodyTypeDefOf.Child)
				{
					Pawn pawn = this.pawn;
					string str = (pawn != null) ? pawn.ToString() : null;
					string str2 = " had body type set to ";
					BodyTypeDef bodyTypeDef = this.bodyType;
					Log.Warning(str + str2 + ((bodyTypeDef != null) ? bodyTypeDef.label : null) + " as a child.");
					this.bodyType = BodyTypeDefOf.Child;
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				string childDefName = null;
				Scribe_Values.Look<string>(ref childDefName, "childhood", null, false);
				if (!childDefName.NullOrEmpty())
				{
					this.childhood = DefDatabase<BackstoryDef>.AllDefs.FirstOrDefault((BackstoryDef b) => b.defName == childDefName);
					if (this.childhood == null)
					{
						Log.Error(string.Format("Couldn't load child backstory {0} for {1}. Giving random.", childDefName, this.pawn.Name));
						this.childhood = (from bs in DefDatabase<BackstoryDef>.AllDefs
						where bs.slot == BackstorySlot.Childhood
						select bs).RandomElement<BackstoryDef>();
					}
				}
				string adultDefName = null;
				Scribe_Values.Look<string>(ref adultDefName, "adulthood", null, false);
				if (!adultDefName.NullOrEmpty())
				{
					this.adulthood = DefDatabase<BackstoryDef>.AllDefs.FirstOrDefault((BackstoryDef b) => b.defName == adultDefName);
					if (this.adulthood == null)
					{
						Log.Error(string.Format("Couldn't load adult backstory {0} for {1}. Giving random.", adultDefName, this.pawn.Name));
						this.adulthood = (from bs in DefDatabase<BackstoryDef>.AllDefs
						where bs.slot == BackstorySlot.Adulthood
						select bs).RandomElement<BackstoryDef>();
						return;
					}
				}
			}
			else if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe_Defs.Look<BackstoryDef>(ref this.childhood, "childhood");
				Scribe_Defs.Look<BackstoryDef>(ref this.adulthood, "adulthood");
			}
		}

		// Token: 0x0600C8B5 RID: 51381 RVA: 0x003AA51E File Offset: 0x003A871E
		public BackstoryDef GetBackstory(BackstorySlot slot)
		{
			if (slot == BackstorySlot.Childhood)
			{
				return this.childhood;
			}
			return this.adulthood;
		}

		// Token: 0x0600C8B6 RID: 51382 RVA: 0x003AA530 File Offset: 0x003A8730
		public bool TryGetRandomHeadFromSet(IEnumerable<HeadTypeDef> options)
		{
			Rand.PushState(this.pawn.thingIDNumber);
			bool result = options.Where(new Func<HeadTypeDef, bool>(this.<TryGetRandomHeadFromSet>g__CanUseHeadType|61_0)).TryRandomElementByWeight((HeadTypeDef x) => x.selectionWeight, out this.headType);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600C8B8 RID: 51384 RVA: 0x003AA5A4 File Offset: 0x003A87A4
		[CompilerGenerated]
		private bool <TryGetRandomHeadFromSet>g__CanUseHeadType|61_0(HeadTypeDef head)
		{
			if (!ModsConfig.BiotechActive || head.requiredGenes.NullOrEmpty<GeneDef>())
			{
				return head.gender == Gender.None || head.gender == this.pawn.gender;
			}
			if (this.pawn.genes == null)
			{
				return false;
			}
			foreach (GeneDef geneDef in head.requiredGenes)
			{
				if (!this.pawn.genes.HasActiveGene(geneDef))
				{
					return false;
				}
			}
			return head.gender == Gender.None || head.gender == this.pawn.gender;
		}

		// Token: 0x04008883 RID: 34947
		private Pawn pawn;

		// Token: 0x04008884 RID: 34948
		private BackstoryDef childhood;

		// Token: 0x04008885 RID: 34949
		private BackstoryDef adulthood;

		// Token: 0x04008886 RID: 34950
		private Color hairColor = Color.white;

		// Token: 0x04008887 RID: 34951
		public Color? skinColorOverride;

		// Token: 0x04008888 RID: 34952
		public HeadTypeDef headType;

		// Token: 0x04008889 RID: 34953
		public BodyTypeDef bodyType;

		// Token: 0x0400888A RID: 34954
		public HairDef hairDef;

		// Token: 0x0400888B RID: 34955
		public TraitSet traits;

		// Token: 0x0400888C RID: 34956
		public string title;

		// Token: 0x0400888D RID: 34957
		public string birthLastName;

		// Token: 0x0400888E RID: 34958
		public ColorDef favoriteColor;

		// Token: 0x0400888F RID: 34959
		public FurDef furDef;

		// Token: 0x04008890 RID: 34960
		[Unsaved(false)]
		private Color? skinColorBase;

		// Token: 0x04008891 RID: 34961
		[Unsaved(false)]
		private float melanin = -1f;

		// Token: 0x04008892 RID: 34962
		[Unsaved(false)]
		private List<BackstoryDef> backstoriesCache;

		// Token: 0x04008893 RID: 34963
		private static readonly FloatRange VoicePitchFactorRange = new FloatRange(0.85f, 1.15f);
	}
}
