using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020025B9 RID: 9657
	public abstract class IdeoFoundation : IExposable
	{
		// Token: 0x0600D38B RID: 54155
		public abstract void Init(IdeoGenerationParms parms);

		// Token: 0x0600D38C RID: 54156
		public abstract void DoInfo(ref float curY, float width, IdeoEditMode editMode);

		// Token: 0x0600D38D RID: 54157
		public abstract void GenerateTextSymbols();

		// Token: 0x0600D38E RID: 54158 RVA: 0x003E58DC File Offset: 0x003E3ADC
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<IdeoFoundationDef>(ref this.def, "def");
			Scribe_Defs.Look<PlaceDef>(ref this.place, "place");
		}

		// Token: 0x0600D38F RID: 54159 RVA: 0x003E5900 File Offset: 0x003E3B00
		public static IdeoIconDef GetRandomIconDef(Ideo ideo)
		{
			IEnumerable<IdeoIconDef> enumerable = null;
			if (Find.World != null)
			{
				enumerable = from x in DefDatabase<IdeoIconDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo) && !Find.IdeoManager.IdeosListForReading.Any((Ideo y) => y.iconDef == x)
				select x;
			}
			if (enumerable.EnumerableNullOrEmpty<IdeoIconDef>())
			{
				enumerable = from x in DefDatabase<IdeoIconDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo)
				select x;
			}
			return enumerable.RandomElement<IdeoIconDef>();
		}

		// Token: 0x0600D390 RID: 54160 RVA: 0x003E5960 File Offset: 0x003E3B60
		public static ColorDef GetRandomColorDef(Ideo ideo)
		{
			IEnumerable<ColorDef> enumerable = null;
			if (Find.World != null)
			{
				enumerable = from x in DefDatabase<IdeoColorDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo) && x.colorDef.colorType == ColorType.Ideo && !Find.IdeoManager.IdeosListForReading.Any((Ideo y) => y.colorDef == x.colorDef)
				select x.colorDef;
			}
			if (enumerable.EnumerableNullOrEmpty<ColorDef>())
			{
				enumerable = from x in DefDatabase<IdeoColorDef>.AllDefs
				where x.CanBeChosenForIdeo(ideo) && x.colorDef.colorType == ColorType.Ideo
				select x.colorDef;
			}
			return enumerable.RandomElement<ColorDef>();
		}

		// Token: 0x0600D391 RID: 54161 RVA: 0x003E5A08 File Offset: 0x003E3C08
		public virtual void RandomizeCulture(IdeoGenerationParms parms)
		{
			if (parms.forFaction != null && parms.forFaction.allowedCultures != null)
			{
				this.ideo.culture = parms.forFaction.allowedCultures.RandomElement<CultureDef>();
				return;
			}
			this.ideo.culture = DefDatabase<CultureDef>.AllDefsListForReading.RandomElement<CultureDef>();
		}

		// Token: 0x0600D392 RID: 54162 RVA: 0x003E5A5B File Offset: 0x003E3C5B
		public virtual void RandomizePlace()
		{
			this.place = (from p in DefDatabase<PlaceDef>.AllDefsListForReading
			where p.tags.SharesElementWith(this.ideo.culture.allowedPlaceTags)
			select p).RandomElement<PlaceDef>();
		}

		// Token: 0x0600D393 RID: 54163 RVA: 0x003E5A7E File Offset: 0x003E3C7E
		protected virtual void RandomizeMemes(IdeoGenerationParms parms)
		{
			this.ideo.memes.Clear();
			this.ideo.memes.AddRange(IdeoUtility.GenerateRandomMemes(parms));
			this.ideo.SortMemesInDisplayOrder();
		}

		// Token: 0x0600D394 RID: 54164 RVA: 0x003E5AB4 File Offset: 0x003E3CB4
		public virtual void RandomizeStyles()
		{
			if (this.ideo == null)
			{
				return;
			}
			List<ThingStyleCategoryWithPriority> list = new List<ThingStyleCategoryWithPriority>();
			if (!this.ideo.culture.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				list.AddRange(this.ideo.culture.thingStyleCategories);
			}
			List<MemeDef> memes = this.ideo.memes;
			for (int i = 0; i < memes.Count; i++)
			{
				if (!memes[i].thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					list.AddRange(memes[i].thingStyleCategories);
				}
			}
			list.Sort(delegate(ThingStyleCategoryWithPriority first, ThingStyleCategoryWithPriority second)
			{
				if (first.priority != second.priority)
				{
					return -first.priority.CompareTo(second.priority);
				}
				if (Rand.Value >= 0.5f)
				{
					return 1;
				}
				return -1;
			});
			this.ideo.thingStyleCategories = new List<ThingStyleCategoryWithPriority>();
			while (this.ideo.thingStyleCategories.Count < 3 && list.Any<ThingStyleCategoryWithPriority>())
			{
				ThingStyleCategoryWithPriority thingStyleCategoryWithPriority = list.First<ThingStyleCategoryWithPriority>();
				list.Remove(thingStyleCategoryWithPriority);
				if (this.<RandomizeStyles>g__CanUseStyleCategory|22_1(thingStyleCategoryWithPriority.category))
				{
					this.ideo.thingStyleCategories.Add(thingStyleCategoryWithPriority);
					this.ideo.SortStyleCategories();
				}
			}
			this.ideo.style.ResetStylesForThingDef();
		}

		// Token: 0x0600D395 RID: 54165 RVA: 0x003E5BDC File Offset: 0x003E3DDC
		public bool CanAddForFaction(PreceptDef precept, FactionDef forFaction, List<PreceptDef> disallowedPrecepts, bool checkDuplicates, bool ignoreMemeRequirements = false, bool ignoreConflictingMemes = false, bool classic = false)
		{
			if (!this.CanAdd(precept, checkDuplicates).Accepted)
			{
				return false;
			}
			if (disallowedPrecepts != null && disallowedPrecepts.Contains(precept))
			{
				return false;
			}
			if (precept.classicModeOnly)
			{
				return false;
			}
			if (classic)
			{
				return precept.classicExtra || precept.classic;
			}
			if (forFaction != null)
			{
				if (forFaction.disallowedPrecepts != null && forFaction.disallowedPrecepts.Contains(precept))
				{
					return false;
				}
				if (forFaction.classicIdeo)
				{
					if (precept.classic)
					{
						return true;
					}
					if (precept.impact == PreceptImpact.High)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600D396 RID: 54166 RVA: 0x003E5C64 File Offset: 0x003E3E64
		public virtual void RandomizePrecepts(bool init, IdeoGenerationParms parms)
		{
			IdeoFoundation.<>c__DisplayClass28_0 CS$<>8__locals1 = new IdeoFoundation.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.parms = parms;
			this.ideo.ClearPrecepts();
			IdeoFoundation.tmpWeaponClassPairs.Clear();
			IdeoFoundation.tmpInitializedPrecepts.Clear();
			CS$<>8__locals1.high = IdeoFoundation.PreceptCountRange_HighImpact.RandomInRange;
			CS$<>8__locals1.medium = IdeoFoundation.PreceptCountRange_MediumImpact.RandomInRange;
			CS$<>8__locals1.low = IdeoFoundation.PreceptCountRange_LowImpact.RandomInRange;
			bool flag = !this.ideo.Fluid && !CS$<>8__locals1.parms.forceNoWeaponPreference && Rand.Bool;
			this.AddRequiredPreceptsForMemes(this.ideo.memes, CS$<>8__locals1.parms, flag, IdeoFoundation.tmpWeaponClassPairs, delegate(PreceptDef p)
			{
				int num2;
				if (p.impact == PreceptImpact.High)
				{
					num2 = CS$<>8__locals1.high;
					CS$<>8__locals1.high = num2 - 1;
					return;
				}
				if (p.impact == PreceptImpact.Medium)
				{
					num2 = CS$<>8__locals1.medium;
					CS$<>8__locals1.medium = num2 - 1;
					return;
				}
				num2 = CS$<>8__locals1.low;
				CS$<>8__locals1.low = num2 - 1;
			});
			if (CS$<>8__locals1.parms.requiredPreceptsOnly)
			{
				this.AddSpecialPrecepts(CS$<>8__locals1.parms, DefDatabase<PreceptDef>.AllDefsListForReading);
				this.FinalizeIdeo(this.ideo);
				if (init)
				{
					this.InitPrecepts(CS$<>8__locals1.parms, IdeoFoundation.tmpInitializedPrecepts);
					this.ideo.RecachePrecepts();
				}
				IdeoFoundation.tmpInitializedPrecepts.Clear();
				return;
			}
			List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
			if (CS$<>8__locals1.parms.classicExtra)
			{
				for (int m = 0; m < allDefsListForReading.Count; m++)
				{
					if (allDefsListForReading[m].classic)
					{
						this.ideo.AddPrecept(PreceptMaker.MakePrecept(allDefsListForReading[m]), false, null, null);
					}
				}
			}
			CS$<>8__locals1.<RandomizePrecepts>g__AddPreceptsOfImpact|1(PreceptImpact.High, CS$<>8__locals1.high);
			CS$<>8__locals1.<RandomizePrecepts>g__AddPreceptsOfImpact|1(PreceptImpact.Medium, CS$<>8__locals1.medium);
			CS$<>8__locals1.<RandomizePrecepts>g__AddPreceptsOfImpact|1(PreceptImpact.Low, CS$<>8__locals1.low);
			CS$<>8__locals1.allIssueDefs = DefDatabase<IssueDef>.AllDefsListForReading;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.allIssueDefs.Count; i = i2 + 1)
			{
				PreceptDef preceptDef;
				PreceptDef preceptDef2;
				if (allDefsListForReading.Where(delegate(PreceptDef x)
				{
					if (x.issue == CS$<>8__locals1.allIssueDefs[i])
					{
						List<MemeDef> associatedMemes = x.associatedMemes;
						Predicate<MemeDef> predicate;
						if ((predicate = CS$<>8__locals1.<>9__7) == null)
						{
							predicate = (CS$<>8__locals1.<>9__7 = ((MemeDef y) => CS$<>8__locals1.<>4__this.ideo.memes.Contains(y)));
						}
						if (associatedMemes.Any(predicate))
						{
							return CS$<>8__locals1.<>4__this.CanAddForFaction(x, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false, CS$<>8__locals1.parms.classicExtra);
						}
					}
					return false;
				}).TryRandomElementByWeight((PreceptDef x) => x.defaultSelectionWeight, out preceptDef))
				{
					this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef), false, null, null);
				}
				else if ((from x in allDefsListForReading
				where x.issue == CS$<>8__locals1.allIssueDefs[i] && CS$<>8__locals1.<>4__this.CanAddForFaction(x, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false, CS$<>8__locals1.parms.classicExtra)
				select x).TryRandomElementByWeight((PreceptDef x) => x.defaultSelectionWeight, out preceptDef2))
				{
					this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef2), false, null, null);
				}
				i2 = i;
			}
			this.AddSpecialPrecepts(CS$<>8__locals1.parms, allDefsListForReading);
			IEnumerable<PreceptDef> enumerable = from x in DefDatabase<PreceptDef>.AllDefs
			where x.preceptClass == typeof(Precept_RoleMulti) && CS$<>8__locals1.<>4__this.CanAddForFaction(x, CS$<>8__locals1.parms.forFaction, CS$<>8__locals1.parms.disallowedPrecepts, true, false, false, CS$<>8__locals1.parms.classicExtra)
			select x;
			if (enumerable.Any<PreceptDef>())
			{
				for (int j = 0; j < 2; j++)
				{
					IEnumerable<PreceptDef> source = enumerable;
					Func<PreceptDef, float> weightSelector;
					if ((weightSelector = CS$<>8__locals1.<>9__10) == null)
					{
						weightSelector = (CS$<>8__locals1.<>9__10 = delegate(PreceptDef x)
						{
							if (!CS$<>8__locals1.<>4__this.ideo.HasPrecept(x))
							{
								return 1f;
							}
							return 0f;
						});
					}
					PreceptDef preceptDef3;
					if (source.TryRandomElementByWeight(weightSelector, out preceptDef3))
					{
						this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef3), false, null, null);
					}
				}
			}
			if (!CS$<>8__locals1.parms.classicExtra)
			{
				int num = IdeoFoundation.InitialVeneratedAnimalsCountRange.RandomInRange;
				for (int k = 0; k < this.ideo.memes.Count; k++)
				{
					if (this.ideo.memes[k].veneratedAnimalsCountOverride >= 0)
					{
						num = this.ideo.memes[k].veneratedAnimalsCountOverride;
						break;
					}
					num += this.ideo.memes[k].veneratedAnimalsCountOffset;
				}
				for (int l = 0; l < Mathf.Min(num, PreceptDefOf.AnimalVenerated.maxCount); l++)
				{
					Precept_Animal precept = (Precept_Animal)PreceptMaker.MakePrecept(PreceptDefOf.AnimalVenerated);
					this.ideo.AddPrecept(precept, false, null, null);
				}
				if (flag)
				{
					if (this.ideo.culture.preferredWeaponClasses != null)
					{
						IdeoFoundation.tmpWeaponClassPairs.Add(this.ideo.culture.preferredWeaponClasses);
					}
					Precept_Weapon precept_Weapon = (Precept_Weapon)PreceptMaker.MakePrecept(PreceptDefOf.NobleDespisedWeapons);
					if (!IdeoFoundation.tmpWeaponClassPairs.NullOrEmpty<IdeoWeaponClassPair>())
					{
						IdeoWeaponClassPair ideoWeaponClassPair = IdeoFoundation.tmpWeaponClassPairs.RandomElement<IdeoWeaponClassPair>();
						precept_Weapon.noble = ideoWeaponClassPair.noble;
						precept_Weapon.despised = ideoWeaponClassPair.despised;
					}
					else
					{
						WeaponClassPairDef weaponClassPairDef = DefDatabase<WeaponClassPairDef>.AllDefs.RandomElement<WeaponClassPairDef>();
						bool @bool = Rand.Bool;
						precept_Weapon.noble = (@bool ? weaponClassPairDef.second : weaponClassPairDef.first);
						precept_Weapon.despised = (@bool ? weaponClassPairDef.first : weaponClassPairDef.second);
					}
					this.ideo.AddPrecept(precept_Weapon, false, null, null);
				}
				if (ModsConfig.BiotechActive && CS$<>8__locals1.parms.forFaction != null)
				{
					XenotypeSet xenotypeSet = CS$<>8__locals1.parms.forFaction.xenotypeSet;
					if (xenotypeSet != null && xenotypeSet.Count == 1 && Rand.Bool)
					{
						Precept_Xenotype precept_Xenotype = (Precept_Xenotype)PreceptMaker.MakePrecept(PreceptDefOf.PreferredXenotype);
						precept_Xenotype.xenotype = xenotypeSet[0].xenotype;
						this.ideo.AddPrecept(precept_Xenotype, false, null, null);
					}
				}
			}
			this.FinalizeIdeo(this.ideo);
			if (init)
			{
				this.InitPrecepts(CS$<>8__locals1.parms, IdeoFoundation.tmpInitializedPrecepts);
				this.ideo.RecachePrecepts();
			}
			IdeoFoundation.tmpInitializedPrecepts.Clear();
		}

		// Token: 0x0600D397 RID: 54167 RVA: 0x003E61A8 File Offset: 0x003E43A8
		private void AddSpecialPrecepts(IdeoGenerationParms parms, List<PreceptDef> allPreceptDefs)
		{
			for (int i = 0; i < allPreceptDefs.Count; i++)
			{
				PreceptDef preceptDef = allPreceptDefs[i];
				if (!preceptDef.countsTowardsPreceptLimit && preceptDef.canGenerateAsSpecialPrecept && this.CanAddForFaction(preceptDef, parms.forFaction, parms.disallowedPrecepts, true, false, false, parms.classicExtra))
				{
					if (preceptDef.preceptInstanceCountCurve != null)
					{
						int num = Mathf.CeilToInt(preceptDef.preceptInstanceCountCurve.Evaluate(Rand.Value));
						for (int j = 0; j < num; j++)
						{
							this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef), false, null, null);
						}
					}
					else
					{
						this.ideo.AddPrecept(PreceptMaker.MakePrecept(preceptDef), false, null, null);
					}
				}
			}
		}

		// Token: 0x0600D398 RID: 54168 RVA: 0x003E6258 File Offset: 0x003E4458
		private void FinalizeIdeo(Ideo ideo)
		{
			if (ideo.IdeoPrefersNudity())
			{
				List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
				for (int i = preceptsListForReading.Count - 1; i >= 0; i--)
				{
					if (preceptsListForReading[i] is Precept_Apparel)
					{
						ideo.RemovePrecept(preceptsListForReading[i], false);
					}
				}
			}
		}

		// Token: 0x0600D399 RID: 54169 RVA: 0x003E62A4 File Offset: 0x003E44A4
		private void AddRequiredPreceptsForMemes(List<MemeDef> memes, IdeoGenerationParms parms, bool generateWeaponPrecept, List<IdeoWeaponClassPair> weaponClassPairsOut = null, Action<PreceptDef> preceptGenerated = null)
		{
			Func<PreceptThingPair, bool> <>9__0;
			Func<PreceptDef, bool> <>9__2;
			for (int i = 0; i < memes.Count; i++)
			{
				if (memes[i].selectOneOrNone != null && Rand.Value >= memes[i].selectOneOrNone.noneChance)
				{
					IEnumerable<PreceptThingPair> preceptThingPairs = memes[i].selectOneOrNone.preceptThingPairs;
					Func<PreceptThingPair, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((PreceptThingPair x) => this.CanAddForFaction(x.precept, parms.forFaction, parms.disallowedPrecepts, true, false, false, parms.classicExtra)));
					}
					PreceptThingPair preceptThingPair;
					if (preceptThingPairs.Where(predicate).TryRandomElementByWeight((PreceptThingPair x) => x.precept.selectionWeight, out preceptThingPair))
					{
						Precept precept = PreceptMaker.MakePrecept(preceptThingPair.precept);
						Precept_Apparel precept_Apparel = precept as Precept_Apparel;
						if (precept_Apparel != null)
						{
							precept_Apparel.apparelDef = preceptThingPair.thing;
						}
						else
						{
							Precept_ThingDef precept_ThingDef = precept as Precept_ThingDef;
							if (precept_ThingDef != null)
							{
								precept_ThingDef.ThingDef = preceptThingPair.thing;
							}
						}
						this.ideo.AddPrecept(precept, true, null, null);
					}
				}
				if (memes[i].requiredRituals != null && !parms.classicExtra)
				{
					for (int j = 0; j < memes[i].requiredRituals.Count; j++)
					{
						RequiredRitualAndBuilding requiredRitualAndBuilding = memes[i].requiredRituals[j];
						Precept_Ritual precept_Ritual = (Precept_Ritual)PreceptMaker.MakePrecept(requiredRitualAndBuilding.precept);
						this.ideo.AddPrecept(precept_Ritual, true, parms.forFaction, requiredRitualAndBuilding.pattern);
						precept_Ritual.RegenerateName();
						IdeoFoundation.tmpInitializedPrecepts.Add(precept_Ritual);
						if (requiredRitualAndBuilding.building != null)
						{
							Precept_Building precept_Building = (Precept_Building)PreceptMaker.MakePrecept(PreceptDefOf.IdeoBuilding);
							this.ideo.AddPrecept(precept_Building, true, parms.forFaction, null);
							precept_Building.ThingDef = requiredRitualAndBuilding.building;
							IdeoFoundation.tmpInitializedPrecepts.Add(precept_Building);
						}
					}
				}
				if (memes[i].requireOne != null)
				{
					for (int k = 0; k < memes[i].requireOne.Count; k++)
					{
						PreceptDef preceptDef = memes[i].requireOne[k].FirstOrDefault<PreceptDef>();
						if (preceptDef != null && !preceptDef.allowDuplicates)
						{
							IssueDef issue = memes[i].requireOne[k].First<PreceptDef>().issue;
							Precept precept2 = this.ideo.PreceptsListForReading.FirstOrDefault((Precept x) => x.def.issue == issue && !this.ideo.PreceptIsRequired(x.def));
							if (precept2 != null)
							{
								this.ideo.RemovePrecept(precept2, false);
							}
						}
						IEnumerable<PreceptDef> source = memes[i].requireOne[k];
						Func<PreceptDef, bool> predicate2;
						if ((predicate2 = <>9__2) == null)
						{
							predicate2 = (<>9__2 = ((PreceptDef x) => this.CanAddForFaction(x, parms.forFaction, parms.disallowedPrecepts, true, false, false, parms.classicExtra)));
						}
						PreceptDef obj;
						if (source.Where(predicate2).TryRandomElementByWeight((PreceptDef x) => x.selectionWeight, out obj))
						{
							this.ideo.AddPrecept(PreceptMaker.MakePrecept(obj), true, null, null);
							if (preceptGenerated != null)
							{
								preceptGenerated(obj);
							}
						}
					}
				}
				if (generateWeaponPrecept && memes[i].preferredWeaponClasses != null && weaponClassPairsOut != null)
				{
					weaponClassPairsOut.Add(memes[i].preferredWeaponClasses);
				}
			}
		}

		// Token: 0x0600D39A RID: 54170 RVA: 0x003E6618 File Offset: 0x003E4818
		public void EnsurePreceptsCompatibleWithMemes(List<MemeDef> oldMemes, List<MemeDef> newMemes, IdeoGenerationParms parms)
		{
			this.tmpPreceptsToRemove.Clear();
			this.tmpPreceptsToRemove.AddRange(this.GetPreceptsToRemoveFromMemeChanges(oldMemes, newMemes));
			for (int i = 0; i < this.tmpPreceptsToRemove.Count; i++)
			{
				this.ideo.RemovePrecept(this.tmpPreceptsToRemove[i], false);
			}
			this.tmpPreceptsToRemove.Clear();
			this.tmpAddedMemes.Clear();
			this.tmpAddedMemes.AddRange(from m in newMemes
			where !oldMemes.Contains(m)
			select m);
			IdeoFoundation.tmpInitializedPrecepts.Clear();
			this.AddRequiredPreceptsForMemes(this.tmpAddedMemes, parms, false, null, null);
			IdeoFoundation.tmpInitializedPrecepts.Clear();
			this.AddRitualsForMemes(this.tmpAddedMemes, null);
			this.FinalizeIdeo(this.ideo);
			IdeoFoundation.tmpInitializedPrecepts.Clear();
			this.tmpAddedMemes.Clear();
		}

		// Token: 0x0600D39B RID: 54171 RVA: 0x003E6708 File Offset: 0x003E4908
		public IEnumerable<Precept> GetPreceptsToRemoveFromMemeChanges(List<MemeDef> prevMemes, List<MemeDef> futureMemes)
		{
			List<Precept> existingPrecepts = this.ideo.PreceptsListForReading;
			List<MemeDef> newMemes = (from m in futureMemes
			where !prevMemes.Contains(m)
			select m).ToList<MemeDef>();
			int num;
			for (int i = 0; i < existingPrecepts.Count; i = num + 1)
			{
				if (!this.HasRequiredMemes(existingPrecepts[i].def, futureMemes))
				{
					yield return existingPrecepts[i];
				}
				else if (this.ConflictsWithNewMemes(existingPrecepts[i], newMemes))
				{
					yield return existingPrecepts[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600D39C RID: 54172 RVA: 0x003E6728 File Offset: 0x003E4928
		private bool ConflictsWithNewMemes(Precept precept, List<MemeDef> newMemes)
		{
			for (int i = 0; i < newMemes.Count; i++)
			{
				if (precept.def.conflictingMemes.Contains(newMemes[i]))
				{
					return true;
				}
				if (!precept.def.allowDuplicates && !newMemes[i].requireOne.NullOrEmpty<List<PreceptDef>>())
				{
					for (int j = 0; j < newMemes[i].requireOne.Count; j++)
					{
						List<PreceptDef> list = newMemes[i].requireOne[j];
						if (list.Count != 0)
						{
							int num = 0;
							for (int k = 0; k < list.Count; k++)
							{
								if (list[k] != precept.def && list[k].issue == precept.def.issue)
								{
									num++;
								}
							}
							if (num == list.Count)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D39D RID: 54173 RVA: 0x003E6818 File Offset: 0x003E4A18
		private bool HasRequiredMemes(PreceptDef preceptDef, List<MemeDef> memes)
		{
			return preceptDef.requiredMemes.NullOrEmpty<MemeDef>() || memes.Any((MemeDef x) => preceptDef.requiredMemes.Contains(x));
		}

		// Token: 0x0600D39E RID: 54174 RVA: 0x003E6858 File Offset: 0x003E4A58
		public void InitPrecepts(IdeoGenerationParms parms, List<Precept> initializedPrecepts = null)
		{
			IdeoFoundation.<>c__DisplayClass38_0 CS$<>8__locals1 = new IdeoFoundation.<>c__DisplayClass38_0();
			CS$<>8__locals1.parms = parms;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.initializedPrecepts = initializedPrecepts;
			Dictionary<MemeDef, int> dictionary = new Dictionary<MemeDef, int>();
			CS$<>8__locals1.extraPrecepts = new List<Precept>();
			CS$<>8__locals1.filledPrecepts = new List<Precept>();
			CS$<>8__locals1.initedPrecepts = new List<Precept>();
			foreach (Precept_Ritual precept_Ritual in this.ideo.PreceptsListForReading.OfType<Precept_Ritual>())
			{
				IdeoFoundation.<>c__DisplayClass38_1 CS$<>8__locals2 = new IdeoFoundation.<>c__DisplayClass38_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				RitualPatternDef ritualPatternDef = null;
				IdeoFoundation.<>c__DisplayClass38_1 CS$<>8__locals3 = CS$<>8__locals2;
				string groupTag;
				if ((groupTag = precept_Ritual.patternGroupTag) == null)
				{
					RitualPatternDef ritualPatternBase = precept_Ritual.def.ritualPatternBase;
					groupTag = ((ritualPatternBase != null) ? ritualPatternBase.patternGroupTag : null);
				}
				CS$<>8__locals3.groupTag = groupTag;
				if (!CS$<>8__locals2.groupTag.NullOrEmpty() && (CS$<>8__locals2.CS$<>8__locals1.initializedPrecepts == null || !CS$<>8__locals2.CS$<>8__locals1.initializedPrecepts.Contains(precept_Ritual)))
				{
					ritualPatternDef = (from d in DefDatabase<RitualPatternDef>.AllDefs
					where d.patternGroupTag == CS$<>8__locals2.groupTag && d.CanFactionUse(CS$<>8__locals2.CS$<>8__locals1.parms.forFaction) && !CS$<>8__locals2.CS$<>8__locals1.<>4__this.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual p) => p.behavior != null && p.sourcePattern == d)
					select d).RandomElementWithFallback(null);
				}
				if (ritualPatternDef != null)
				{
					ritualPatternDef.Fill(precept_Ritual);
					CS$<>8__locals2.CS$<>8__locals1.filledPrecepts.Add(precept_Ritual);
				}
			}
			CS$<>8__locals1.usedMemePatternDefs = new HashSet<RitualPatternDef>();
			foreach (MemeDef memeDef in IdeoFoundation.tmpMemes)
			{
				if (IdeoFoundation.tmpMemesNumRitualsToMake.ContainsKey(memeDef) && (!dictionary.ContainsKey(memeDef) || dictionary[memeDef] < IdeoFoundation.tmpMemesNumRitualsToMake[memeDef]) && !memeDef.replacementPatterns.NullOrEmpty<RitualPatternDef>())
				{
					foreach (Precept precept in from p in this.ideo.PreceptsListForReading
					orderby p is Precept_Ritual descending
					select p)
					{
						RitualPatternDef ritualPatternBase2 = precept.def.ritualPatternBase;
						if (ritualPatternBase2 != null && !ritualPatternBase2.tags.NullOrEmpty<string>() && memeDef.replaceRitualsWithTags.Any(new Predicate<string>(ritualPatternBase2.tags.Contains)))
						{
							int num = 0;
							int num2;
							if (dictionary.TryGetValue(memeDef, out num2))
							{
								num = num2;
							}
							IEnumerable<RitualPatternDef> replacementPatterns = memeDef.replacementPatterns;
							Func<RitualPatternDef, bool> predicate;
							if ((predicate = CS$<>8__locals1.<>9__4) == null)
							{
								predicate = (CS$<>8__locals1.<>9__4 = ((RitualPatternDef p) => p.CanFactionUse(CS$<>8__locals1.parms.forFaction) && !CS$<>8__locals1.usedMemePatternDefs.Contains(p)));
							}
							RitualPatternDef ritualPatternDef2 = replacementPatterns.Where(predicate).RandomElementWithFallback(null);
							if (ritualPatternDef2 != null)
							{
								ritualPatternDef2.Fill((Precept_Ritual)precept);
								dictionary.SetOrAdd(memeDef, num + 1);
								CS$<>8__locals1.filledPrecepts.Add(precept);
								CS$<>8__locals1.usedMemePatternDefs.Add(ritualPatternDef2);
							}
						}
					}
					if (!dictionary.ContainsKey(memeDef) || dictionary[memeDef] < IdeoFoundation.tmpMemesNumRitualsToMake[memeDef])
					{
						foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefsListForReading)
						{
							RitualPatternDef pattern = preceptDef.ritualPatternBase;
							if (pattern != null && !pattern.tags.NullOrEmpty<string>() && memeDef.replaceRitualsWithTags.Any(new Predicate<string>(pattern.tags.Contains)) && !CS$<>8__locals1.usedMemePatternDefs.Contains(pattern) && !this.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual r) => r.behavior != null && r.behavior.def == pattern.ritualBehavior))
							{
								Precept precept2 = PreceptMaker.MakePrecept(preceptDef);
								this.ideo.AddPrecept(precept2, false, null, null);
								RitualPatternDef pattern2 = pattern;
								if (pattern2 != null)
								{
									pattern2.Fill((Precept_Ritual)precept2);
								}
								CS$<>8__locals1.filledPrecepts.Add(precept2);
								CS$<>8__locals1.usedMemePatternDefs.Add(pattern);
							}
						}
					}
				}
			}
			this.AddRitualsForMemes(IdeoFoundation.tmpMemes, CS$<>8__locals1.filledPrecepts);
			CS$<>8__locals1.<InitPrecepts>g__AddAndInitPrecepts|0();
			foreach (Precept_Ritual precept_Ritual2 in this.ideo.PreceptsListForReading.OfType<Precept_Ritual>())
			{
				if (precept_Ritual2.behavior != null && !precept_Ritual2.behavior.def.preceptRequirements.NullOrEmpty<PreceptRequirement>())
				{
					foreach (PreceptRequirement preceptRequirement in precept_Ritual2.behavior.def.preceptRequirements)
					{
						if (!preceptRequirement.Met(this.ideo.PreceptsListForReading) && !preceptRequirement.Met(CS$<>8__locals1.extraPrecepts))
						{
							CS$<>8__locals1.extraPrecepts.Add(preceptRequirement.MakePrecept(this.ideo));
						}
					}
				}
			}
			CS$<>8__locals1.<InitPrecepts>g__AddAndInitPrecepts|0();
		}

		// Token: 0x0600D39F RID: 54175 RVA: 0x003E6E00 File Offset: 0x003E5000
		private void AddRitualsForMemes(List<MemeDef> memes, List<Precept> outPrecepts = null)
		{
			using (List<PreceptDef>.Enumerator enumerator = DefDatabase<PreceptDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptDef p = enumerator.Current;
					if (p.ritualPatternBase != null && !p.requiredMemes.NullOrEmpty<MemeDef>() && p.requiredMemes.Any(new Predicate<MemeDef>(memes.Contains)) && !this.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual r) => r.sourcePattern == p.ritualPatternBase))
					{
						Precept_Ritual precept_Ritual = (Precept_Ritual)PreceptMaker.MakePrecept(p);
						this.ideo.AddPrecept(precept_Ritual, false, null, null);
						p.ritualPatternBase.Fill(precept_Ritual);
						precept_Ritual.RegenerateName();
						if (outPrecepts != null)
						{
							outPrecepts.Add(precept_Ritual);
						}
					}
				}
			}
		}

		// Token: 0x0600D3A0 RID: 54176 RVA: 0x003E6F04 File Offset: 0x003E5104
		public virtual void RandomizeIcon()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ideo.SetIcon(IdeoFoundation.GetRandomIconDef(this.ideo), IdeoFoundation.GetRandomColorDef(this.ideo), false);
			});
		}

		// Token: 0x0600D3A1 RID: 54177 RVA: 0x003E6F18 File Offset: 0x003E5118
		public virtual void GenerateLeaderTitle()
		{
			if (this.ideo.classicMode)
			{
				this.ideo.leaderTitleMale = PreceptDefOf.IdeoRole_Leader.label;
				this.ideo.leaderTitleFemale = this.ideo.leaderTitleMale;
				return;
			}
			if (this.ideo.culture.leaderTitleMaker == null)
			{
				this.ideo.leaderTitleMale = null;
				this.ideo.leaderTitleFemale = null;
				return;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.ideo.culture.leaderTitleMaker);
			for (int i = 0; i < this.ideo.memes.Count; i++)
			{
				if (this.ideo.memes[i].generalRules != null)
				{
					request.IncludesBare.Add(this.ideo.memes[i].generalRules);
				}
			}
			this.ideo.leaderTitleMale = NameGenerator.GenerateName(request, null, false, "r_leaderTitle", null);
			this.ideo.leaderTitleFemale = this.ideo.leaderTitleMale;
		}

		// Token: 0x0600D3A2 RID: 54178 RVA: 0x003E7034 File Offset: 0x003E5234
		public AcceptanceReport CanAdd(PreceptDef precept, bool checkDuplicates = true)
		{
			List<Precept> preceptsListForReading = this.ideo.PreceptsListForReading;
			if (precept.takeNameFrom != null)
			{
				bool flag = false;
				using (List<Precept>.Enumerator enumerator = this.ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def == precept.takeNameFrom)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (precept.preceptClass == typeof(Precept_RoleMulti))
			{
				if (this.ideo.PreceptsListForReading.Count((Precept p) => p is Precept_RoleMulti && p.def.visible) >= 2)
				{
					return "MaxMultiRolesCount".Translate(2);
				}
			}
			if (!precept.requiredMemes.NullOrEmpty<MemeDef>())
			{
				bool flag2 = false;
				foreach (MemeDef item in precept.requiredMemes)
				{
					if (this.ideo.memes.Contains(item))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (precept.requiredMemes.Count == 1)
					{
						return new AcceptanceReport("RequiresMeme".Translate() + ": " + precept.requiredMemes[0].LabelCap);
					}
					return new AcceptanceReport("RequiresOneOfMemes".Translate() + ": " + precept.RequiredMemeLabels.ToCommaList(false, false).CapitalizeFirst());
				}
			}
			int i = 0;
			while (i < precept.conflictingMemes.Count)
			{
				if (this.ideo.memes.Contains(precept.conflictingMemes[i]))
				{
					if (precept.conflictingMemes.Count == 1)
					{
						return new AcceptanceReport("ConflictsWithMeme".Translate() + ": " + precept.conflictingMemes[0].LabelCap);
					}
					return new AcceptanceReport("ConflictsWithMemes".Translate() + ": " + (from m in precept.conflictingMemes
					select m.label).ToCommaList(false, false).CapitalizeFirst());
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < preceptsListForReading.Count; j++)
			{
				if (checkDuplicates)
				{
					if (precept == preceptsListForReading[j].def)
					{
						if (!precept.allowDuplicates)
						{
							return false;
						}
					}
					else if (!precept.issue.allowMultiplePrecepts && precept.issue == preceptsListForReading[j].def.issue)
					{
						return false;
					}
				}
				else if (precept.issue == preceptsListForReading[j].def.issue && this.ideo.PreceptIsRequired(preceptsListForReading[j].def))
				{
					return this.ideo.PreceptIsRequired(precept);
				}
				for (int k = 0; k < precept.exclusionTags.Count; k++)
				{
					if (preceptsListForReading[j].def.exclusionTags.Contains(precept.exclusionTags[k]) && (preceptsListForReading[j].def.issue != precept.issue || checkDuplicates))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600D3A3 RID: 54179 RVA: 0x003E73F8 File Offset: 0x003E55F8
		public void AddPlaceRules(ref GrammarRequest request)
		{
			PlaceDef placeDef = this.place;
			if (((placeDef != null) ? placeDef.placeRules : null) != null)
			{
				request.IncludesBare.Add(this.place.placeRules);
			}
		}

		// Token: 0x0600D3A4 RID: 54180 RVA: 0x003E7424 File Offset: 0x003E5624
		public virtual void CopyTo(IdeoFoundation other)
		{
			other.place = this.place;
			other.def = this.def;
		}

		// Token: 0x0600D3A8 RID: 54184 RVA: 0x003E7504 File Offset: 0x003E5704
		[CompilerGenerated]
		private bool <RandomizeStyles>g__CanUseStyleCategory|22_1(StyleCategoryDef cat)
		{
			if (this.ideo.thingStyleCategories.Any((ThingStyleCategoryWithPriority x) => x.category == cat))
			{
				return false;
			}
			Func<ThingDefStyle, bool> <>9__3;
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.ideo.thingStyleCategories)
			{
				IEnumerable<ThingDefStyle> thingDefStyles = thingStyleCategoryWithPriority.category.thingDefStyles;
				Func<ThingDefStyle, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((ThingDefStyle x) => cat.thingDefStyles.Contains(x)));
				}
				if (thingDefStyles.All(predicate))
				{
					return false;
				}
			}
			return !cat.fixedIdeoOnly;
		}

		// Token: 0x04008FAF RID: 36783
		public Ideo ideo;

		// Token: 0x04008FB0 RID: 36784
		public IdeoFoundationDef def;

		// Token: 0x04008FB1 RID: 36785
		public PlaceDef place;

		// Token: 0x04008FB2 RID: 36786
		public static readonly IntRange MemeCountRangeAbsolute = new IntRange(1, 4);

		// Token: 0x04008FB3 RID: 36787
		public static readonly IntRange MemeCountRangeNPCInitial = new IntRange(1, 3);

		// Token: 0x04008FB4 RID: 36788
		public static readonly IntRange MemeCountRangeFluidAbsolute = new IntRange(1, 1);

		// Token: 0x04008FB5 RID: 36789
		private static IntRange PreceptCountRange_HighImpact = new IntRange(0, 2);

		// Token: 0x04008FB6 RID: 36790
		private static IntRange PreceptCountRange_MediumImpact = new IntRange(5, 5);

		// Token: 0x04008FB7 RID: 36791
		private static IntRange PreceptCountRange_LowImpact = new IntRange(5, 5);

		// Token: 0x04008FB8 RID: 36792
		private static IntRange InitialVeneratedAnimalsCountRange = new IntRange(0, 1);

		// Token: 0x04008FB9 RID: 36793
		public const int MaxStyleCategories = 3;

		// Token: 0x04008FBA RID: 36794
		public const int MaxRituals = 6;

		// Token: 0x04008FBB RID: 36795
		public const int MaxMultiRoles = 2;

		// Token: 0x04008FBC RID: 36796
		private static List<MemeDef> tmpMemes = new List<MemeDef>();

		// Token: 0x04008FBD RID: 36797
		private static Dictionary<MemeDef, int> tmpMemesNumRitualsToMake = new Dictionary<MemeDef, int>();

		// Token: 0x04008FBE RID: 36798
		private static List<IdeoWeaponClassPair> tmpWeaponClassPairs = new List<IdeoWeaponClassPair>();

		// Token: 0x04008FBF RID: 36799
		private static List<Precept> tmpInitializedPrecepts = new List<Precept>();

		// Token: 0x04008FC0 RID: 36800
		private List<Precept> tmpPreceptsToRemove = new List<Precept>();

		// Token: 0x04008FC1 RID: 36801
		private List<MemeDef> tmpAddedMemes = new List<MemeDef>();
	}
}
