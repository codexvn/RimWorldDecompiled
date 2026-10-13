using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020025F4 RID: 9716
	public class IdeoStyleTracker : IExposable
	{
		// Token: 0x170020B5 RID: 8373
		// (get) Token: 0x0600D525 RID: 54565 RVA: 0x003EF2F4 File Offset: 0x003ED4F4
		public int NumHairAndBeardStylesAvailable
		{
			get
			{
				if (this.hairAndBeardStylesAvailable < 0)
				{
					this.RecacheStyleItemCounts();
				}
				return this.hairAndBeardStylesAvailable;
			}
		}

		// Token: 0x170020B6 RID: 8374
		// (get) Token: 0x0600D526 RID: 54566 RVA: 0x003EF30B File Offset: 0x003ED50B
		public int NumTattooStylesAvailable
		{
			get
			{
				if (this.tattooStylesAvailable < 0)
				{
					this.RecacheStyleItemCounts();
				}
				return this.tattooStylesAvailable;
			}
		}

		// Token: 0x170020B7 RID: 8375
		// (get) Token: 0x0600D527 RID: 54567 RVA: 0x003EF324 File Offset: 0x003ED524
		public HairDef DisplayedHairDef
		{
			get
			{
				if (this.cachedDisplayHairDef == null)
				{
					if (this.hairFrequencies == null)
					{
						return null;
					}
					Rand.PushState(this.ideo.id);
					this.cachedDisplayHairDef = this.hairFrequencies.RandomElementByWeight((KeyValuePair<HairDef, StyleItemSpawningProperties> x) => (float)x.Value.frequency).Key;
					Rand.PopState();
				}
				return this.cachedDisplayHairDef;
			}
		}

		// Token: 0x170020B8 RID: 8376
		// (get) Token: 0x0600D528 RID: 54568 RVA: 0x003EF398 File Offset: 0x003ED598
		public TattooDef DisplayedTattooDef
		{
			get
			{
				if (this.cachedDisplayTattooDef == null)
				{
					if (this.tattooFrequencies == null)
					{
						return null;
					}
					Rand.PushState(this.ideo.id);
					KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair;
					if (!this.tattooFrequencies.TryRandomElementByWeight((KeyValuePair<TattooDef, StyleItemSpawningProperties> x) => (float)IdeoStyleTracker.<get_DisplayedTattooDef>g__GetWeight|16_0(x, false), out keyValuePair))
					{
						this.tattooFrequencies.TryRandomElementByWeight((KeyValuePair<TattooDef, StyleItemSpawningProperties> x) => (float)IdeoStyleTracker.<get_DisplayedTattooDef>g__GetWeight|16_0(x, true), out keyValuePair);
					}
					Rand.PopState();
					this.cachedDisplayTattooDef = keyValuePair.Key;
				}
				return this.cachedDisplayTattooDef;
			}
		}

		// Token: 0x0600D529 RID: 54569 RVA: 0x003EF43D File Offset: 0x003ED63D
		public IdeoStyleTracker()
		{
		}

		// Token: 0x0600D52A RID: 54570 RVA: 0x003EF469 File Offset: 0x003ED669
		public IdeoStyleTracker(Ideo ideo)
		{
			this.ideo = ideo;
		}

		// Token: 0x0600D52B RID: 54571 RVA: 0x003EF49C File Offset: 0x003ED69C
		private void InitializeDefMaps()
		{
			this.hairFrequencies = new DefMap<HairDef, StyleItemSpawningProperties>();
			this.beardFrequencies = new DefMap<BeardDef, StyleItemSpawningProperties>();
			this.tattooFrequencies = new DefMap<TattooDef, StyleItemSpawningProperties>();
		}

		// Token: 0x0600D52C RID: 54572 RVA: 0x003EF4C0 File Offset: 0x003ED6C0
		public StyleItemFrequency GetFrequency(StyleItemDef def)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				return this.hairFrequencies[hairDef].frequency;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				return this.beardFrequencies[beardDef].frequency;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				return this.tattooFrequencies[tattooDef].frequency;
			}
			return StyleItemFrequency.Never;
		}

		// Token: 0x0600D52D RID: 54573 RVA: 0x003EF540 File Offset: 0x003ED740
		public StyleGender GetGender(StyleItemDef def)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				return this.hairFrequencies[hairDef].gender;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				return this.beardFrequencies[beardDef].gender;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				return this.tattooFrequencies[tattooDef].gender;
			}
			return StyleGender.Any;
		}

		// Token: 0x0600D52E RID: 54574 RVA: 0x003EF5C0 File Offset: 0x003ED7C0
		public void SetFrequency(StyleItemDef def, StyleItemFrequency freq)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].frequency = freq;
			}
			else
			{
				BeardDef beardDef = def as BeardDef;
				if (beardDef != null)
				{
					this.beardFrequencies[beardDef].frequency = freq;
				}
				else
				{
					TattooDef tattooDef = def as TattooDef;
					if (tattooDef != null)
					{
						this.tattooFrequencies[tattooDef].frequency = freq;
					}
				}
			}
			this.hairAndBeardStylesAvailable = -1;
			this.tattooStylesAvailable = -1;
			this.cachedDisplayHairDef = null;
			this.cachedDisplayTattooDef = null;
		}

		// Token: 0x0600D52F RID: 54575 RVA: 0x003EF660 File Offset: 0x003ED860
		public void SetGender(StyleItemDef def, StyleGender gender)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].gender = gender;
				return;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				this.beardFrequencies[beardDef].gender = gender;
				return;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				this.tattooFrequencies[tattooDef].gender = gender;
			}
		}

		// Token: 0x0600D530 RID: 54576 RVA: 0x003EF6E4 File Offset: 0x003ED8E4
		public void EnsureAtLeastOneStyleItemAvailable()
		{
			bool flag = false;
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				if (keyValuePair.Value.frequency > StyleItemFrequency.Never)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.hairFrequencies[HairDefOf.Bald].frequency = StyleItemFrequency.Normal;
				this.hairFrequencies[HairDefOf.Bald].gender = StyleGender.Any;
			}
			flag = false;
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				if (keyValuePair2.Value.frequency != StyleItemFrequency.Never)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.beardFrequencies[BeardDefOf.NoBeard].frequency = StyleItemFrequency.Normal;
				this.beardFrequencies[BeardDefOf.NoBeard].gender = StyleGender.Any;
			}
			bool flag2 = false;
			bool flag3 = false;
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				if (keyValuePair3.Value.frequency != StyleItemFrequency.Never)
				{
					if (keyValuePair3.Key.tattooType == TattooType.Face)
					{
						flag2 = true;
					}
					else if (keyValuePair3.Key.tattooType == TattooType.Body)
					{
						flag3 = true;
					}
				}
				if (flag2 && flag3)
				{
					break;
				}
			}
			if (!flag2)
			{
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].frequency = StyleItemFrequency.Normal;
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].gender = StyleGender.Any;
			}
			if (!flag3)
			{
				this.tattooFrequencies[TattooDefOf.NoTattoo_Body].frequency = StyleItemFrequency.Normal;
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].gender = StyleGender.Any;
			}
			this.hairAndBeardStylesAvailable = -1;
			this.tattooStylesAvailable = -1;
		}

		// Token: 0x0600D531 RID: 54577 RVA: 0x003EF8D0 File Offset: 0x003EDAD0
		private void RecacheStyleItemCounts()
		{
			if (this.ideo.culture == null)
			{
				return;
			}
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			this.hairAndBeardStylesAvailable = 0;
			foreach (HairDef def in DefDatabase<HairDef>.AllDefs)
			{
				if (this.hairFrequencies[def].frequency != StyleItemFrequency.Never)
				{
					this.hairAndBeardStylesAvailable++;
				}
			}
			foreach (BeardDef def2 in DefDatabase<BeardDef>.AllDefs)
			{
				if (this.beardFrequencies[def2].frequency != StyleItemFrequency.Never)
				{
					this.hairAndBeardStylesAvailable++;
				}
			}
			this.tattooStylesAvailable = 0;
			foreach (TattooDef tattooDef in DefDatabase<TattooDef>.AllDefs)
			{
				if (tattooDef != TattooDefOf.NoTattoo_Face && tattooDef != TattooDefOf.NoTattoo_Body && this.tattooFrequencies[tattooDef].frequency != StyleItemFrequency.Never)
				{
					this.tattooStylesAvailable++;
				}
			}
		}

		// Token: 0x0600D532 RID: 54578 RVA: 0x003EFA34 File Offset: 0x003EDC34
		public void RecalculateAvailableStyleItems()
		{
			if (!this.ideo.culture.styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
			{
				this.memberStyleTags.AddRange(this.ideo.culture.styleItemTags);
			}
			int i;
			int num;
			for (i = 0; i < this.ideo.memes.Count; i = num + 1)
			{
				if (!this.ideo.memes[i].styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
				{
					int j;
					for (j = 0; j < this.ideo.memes[i].styleItemTags.Count; j = num + 1)
					{
						StyleItemTagWeighted styleItemTagWeighted = this.memberStyleTags.Find((StyleItemTagWeighted x) => x.Tag == this.ideo.memes[i].styleItemTags[j].Tag);
						if (styleItemTagWeighted == null)
						{
							this.memberStyleTags.Add(this.ideo.memes[i].styleItemTags[j]);
						}
						else
						{
							styleItemTagWeighted.Add(this.ideo.memes[i].styleItemTags[j]);
						}
						num = j;
					}
				}
				num = i;
			}
			this.InitializeDefMaps();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|27_0<HairDef>();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|27_0<BeardDef>();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|27_0<TattooDef>();
			this.EnsureAtLeastOneStyleItemAvailable();
			this.memberStyleTags.Clear();
		}

		// Token: 0x0600D533 RID: 54579 RVA: 0x003EFBE4 File Offset: 0x003EDDE4
		private bool NeedToRecacheStyleItems()
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				return true;
			}
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				if (keyValuePair.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				if (keyValuePair2.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				if (keyValuePair3.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D534 RID: 54580 RVA: 0x003EFCF4 File Offset: 0x003EDEF4
		public StyleCategoryPair StyleForThingDef(ThingDef thing, Precept precept = null)
		{
			if (thing == null)
			{
				return null;
			}
			if (this.styleForThingDef.ContainsKey(thing))
			{
				return this.styleForThingDef[thing];
			}
			if (Find.IdeoManager.classicMode)
			{
				foreach (StyleCategoryDef styleCategoryDef in Find.IdeoManager.selectedStyleCategories)
				{
					ThingStyleDef thingStyleDef = styleCategoryDef.GetStyleForThingDef(thing, precept);
					if (thingStyleDef != null)
					{
						StyleCategoryPair styleCategoryPair = new StyleCategoryPair
						{
							styleDef = thingStyleDef,
							category = styleCategoryDef
						};
						this.styleForThingDef.Add(thing, styleCategoryPair);
						return styleCategoryPair;
					}
				}
			}
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.ideo.thingStyleCategories)
			{
				ThingStyleDef thingStyleDef2 = thingStyleCategoryWithPriority.category.GetStyleForThingDef(thing, precept);
				if (thingStyleDef2 != null)
				{
					StyleCategoryPair styleCategoryPair2 = new StyleCategoryPair
					{
						styleDef = thingStyleDef2,
						category = thingStyleCategoryWithPriority.category
					};
					this.styleForThingDef.Add(thing, styleCategoryPair2);
					return styleCategoryPair2;
				}
			}
			return null;
		}

		// Token: 0x0600D535 RID: 54581 RVA: 0x003EFE30 File Offset: 0x003EE030
		public void SetStyleForThingDef(ThingDef thing, StyleCategoryPair styleAndCat)
		{
			this.styleForThingDef.SetOrAdd(thing, styleAndCat);
		}

		// Token: 0x0600D536 RID: 54582 RVA: 0x003EFE3F File Offset: 0x003EE03F
		public void ResetStylesForThingDef()
		{
			this.styleForThingDef.RemoveAll((KeyValuePair<ThingDef, StyleCategoryPair> kvp) => !this.ideo.PreceptsListForReading.Any(delegate(Precept p)
			{
				Precept_ThingDef precept_ThingDef = p as Precept_ThingDef;
				return precept_ThingDef != null && precept_ThingDef.ThingDef == kvp.Key;
			}) || !kvp.Key.canEditAnyStyle);
		}

		// Token: 0x0600D537 RID: 54583 RVA: 0x003EFE59 File Offset: 0x003EE059
		public void ResetStyleForThing(ThingDef thingDef)
		{
			this.styleForThingDef.Remove(thingDef);
		}

		// Token: 0x0600D538 RID: 54584 RVA: 0x003EFE68 File Offset: 0x003EE068
		public virtual void CopyTo(IdeoStyleTracker other)
		{
			other.InitializeDefMaps();
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				other.hairFrequencies[keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				other.beardFrequencies[keyValuePair2.Key] = keyValuePair2.Value;
			}
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				other.tattooFrequencies[keyValuePair3.Key] = keyValuePair3.Value;
			}
			other.styleForThingDef.Clear();
			other.styleForThingDef.AddRange(this.styleForThingDef);
			other.cachedDisplayHairDef = this.cachedDisplayHairDef;
			other.cachedDisplayTattooDef = this.cachedDisplayTattooDef;
			other.RecacheStyleItemCounts();
		}

		// Token: 0x0600D539 RID: 54585 RVA: 0x003EFFAC File Offset: 0x003EE1AC
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<HairDef, StyleItemSpawningProperties>>(ref this.hairFrequencies, "hairFrequencies", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<BeardDef, StyleItemSpawningProperties>>(ref this.beardFrequencies, "beardFrequencies", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TattooDef, StyleItemSpawningProperties>>(ref this.tattooFrequencies, "tattooFrequencies", Array.Empty<object>());
			Scribe_Collections.Look<ThingDef, StyleCategoryPair>(ref this.styleForThingDef, "styleForThingDef", LookMode.Def, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.NeedToRecacheStyleItems())
			{
				this.RecalculateAvailableStyleItems();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.styleForThingDef == null)
				{
					this.styleForThingDef = new Dictionary<ThingDef, StyleCategoryPair>();
					return;
				}
				this.styleForThingDef.RemoveAll((KeyValuePair<ThingDef, StyleCategoryPair> x) => x.Key == null || x.Value == null);
			}
		}

		// Token: 0x0600D53A RID: 54586 RVA: 0x003F0067 File Offset: 0x003EE267
		[CompilerGenerated]
		internal static int <get_DisplayedTattooDef>g__GetWeight|16_0(KeyValuePair<TattooDef, StyleItemSpawningProperties> pair, bool ignoreFrequency)
		{
			if (pair.Key == TattooDefOf.NoTattoo_Body || pair.Key == TattooDefOf.NoTattoo_Face)
			{
				return 0;
			}
			if (ignoreFrequency)
			{
				return 1;
			}
			return (int)pair.Value.frequency;
		}

		// Token: 0x0600D53B RID: 54587 RVA: 0x003F0098 File Offset: 0x003EE298
		[CompilerGenerated]
		private void <RecalculateAvailableStyleItems>g__SetupFrequency|27_0<T>() where T : StyleItemDef
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				float freq = PawnStyleItemChooser.StyleItemChoiceLikelihoodFromTags(t, this.memberStyleTags);
				bool flag = t is TattooDef && this.ideo.classicMode;
				this.SetFrequency(t, flag ? StyleItemFrequency.Never : PawnStyleItemChooser.GetStyleItemFrequency(freq));
				this.SetGender(t, t.styleGender);
			}
		}

		// Token: 0x040090C6 RID: 37062
		public Ideo ideo;

		// Token: 0x040090C7 RID: 37063
		private DefMap<HairDef, StyleItemSpawningProperties> hairFrequencies;

		// Token: 0x040090C8 RID: 37064
		private DefMap<BeardDef, StyleItemSpawningProperties> beardFrequencies;

		// Token: 0x040090C9 RID: 37065
		private DefMap<TattooDef, StyleItemSpawningProperties> tattooFrequencies;

		// Token: 0x040090CA RID: 37066
		private Dictionary<ThingDef, StyleCategoryPair> styleForThingDef = new Dictionary<ThingDef, StyleCategoryPair>();

		// Token: 0x040090CB RID: 37067
		private int hairAndBeardStylesAvailable = -1;

		// Token: 0x040090CC RID: 37068
		private int tattooStylesAvailable = -1;

		// Token: 0x040090CD RID: 37069
		private HairDef cachedDisplayHairDef;

		// Token: 0x040090CE RID: 37070
		private TattooDef cachedDisplayTattooDef;

		// Token: 0x040090CF RID: 37071
		private List<StyleItemTagWeighted> memberStyleTags = new List<StyleItemTagWeighted>();
	}
}
