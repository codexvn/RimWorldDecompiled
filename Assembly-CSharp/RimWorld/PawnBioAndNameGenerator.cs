using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200235C RID: 9052
	public static class PawnBioAndNameGenerator
	{
		// Token: 0x0600C458 RID: 50264 RVA: 0x003911E0 File Offset: 0x0038F3E0
		public static void GiveAppropriateBioAndNameTo(Pawn pawn, FactionDef factionType, PawnGenerationRequest request, XenotypeDef xenotype = null)
		{
			List<BackstoryCategoryFilter> backstoryCategoryFiltersFor = PawnBioAndNameGenerator.GetBackstoryCategoryFiltersFor(pawn, factionType);
			bool flag = pawn.DevelopmentalStage.Baby();
			if (!request.ForceNoBackstory && !request.OnlyUseForcedBackstories && !flag && (Rand.Value < 0.25f || pawn.kindDef.factionLeader) && PawnBioAndNameGenerator.TryGiveSolidBioTo(pawn, request.FixedLastName, backstoryCategoryFiltersFor))
			{
				return;
			}
			PawnBioAndNameGenerator.GiveShuffledBioTo(pawn, factionType, request.FixedLastName, backstoryCategoryFiltersFor, request.ForceNoBackstory, false, xenotype, request.OnlyUseForcedBackstories);
			if (flag)
			{
				NameTriple nameTriple = pawn.Name as NameTriple;
				if (nameTriple != null)
				{
					if (pawn.Faction.IsPlayerSafe())
					{
						pawn.Name = new NameTriple("Baby".Translate().CapitalizeFirst(), null, nameTriple.Last);
						return;
					}
					pawn.Name = new NameTriple(nameTriple.First, null, nameTriple.Last);
				}
			}
		}

		// Token: 0x0600C459 RID: 50265 RVA: 0x003912C0 File Offset: 0x0038F4C0
		private static void GiveShuffledBioTo(Pawn pawn, FactionDef factionType, string requiredLastName, List<BackstoryCategoryFilter> backstoryCategories, bool forceNoBackstory = false, bool forceNoNick = false, XenotypeDef xenotype = null, bool onlyForcedBackstories = false)
		{
			bool flag = pawn.kindDef.fixedChildBackstories.Any<BackstoryDef>();
			bool flag2 = pawn.kindDef.fixedAdultBackstories.Any<BackstoryDef>();
			bool flag3 = pawn.ageTracker.AgeBiologicalYearsFloat >= 20f;
			if (!forceNoBackstory)
			{
				if (flag)
				{
					pawn.story.Childhood = pawn.kindDef.fixedChildBackstories.RandomElement<BackstoryDef>();
				}
				else if (!onlyForcedBackstories)
				{
					PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Childhood, backstoryCategories, factionType, flag3 ? new BackstorySlot?(BackstorySlot.Adulthood) : null);
				}
				if (flag3 && flag2)
				{
					pawn.story.Adulthood = pawn.kindDef.fixedAdultBackstories.RandomElement<BackstoryDef>();
				}
				else if (flag3 && !onlyForcedBackstories)
				{
					PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Adulthood, backstoryCategories, factionType, null);
				}
			}
			pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, requiredLastName, forceNoNick, xenotype);
		}

		// Token: 0x0600C45A RID: 50266 RVA: 0x00391398 File Offset: 0x0038F598
		public static void FillBackstorySlotShuffled(Pawn pawn, BackstorySlot slot, List<BackstoryCategoryFilter> backstoryCategories, FactionDef factionType, BackstorySlot? mustBeCompatibleTo = null)
		{
			BackstoryCategoryFilter categoryFilter = backstoryCategories.RandomElementByWeight((BackstoryCategoryFilter c) => c.commonality) ?? PawnBioAndNameGenerator.FallbackCategoryGroup;
			IEnumerable<BackstoryDef> source = from bs in DefDatabase<BackstoryDef>.AllDefs
			where bs.shuffleable && categoryFilter.Matches(bs)
			select bs;
			PawnBioAndNameGenerator.tmpBackstories.Clear();
			if (mustBeCompatibleTo == null)
			{
				PawnBioAndNameGenerator.tmpBackstories.AddRange(from bs in source
				where bs.slot == slot
				select bs);
			}
			else
			{
				IEnumerable<BackstoryDef> compatibleBackstories = from bs in source
				where bs.slot == mustBeCompatibleTo.Value
				select bs;
				PawnBioAndNameGenerator.tmpBackstories.AddRange(from bs in source
				where bs.slot == slot && compatibleBackstories.Any((BackstoryDef b) => !b.requiredWorkTags.OverlapsWithOnAnyWorkType(bs.workDisables))
				select bs);
			}
			BackstoryDef backstoryDef;
			if (!(from bs in PawnBioAndNameGenerator.tmpBackstories.TakeRandom(20)
			where slot != BackstorySlot.Adulthood || bs.requiredWorkTags == WorkTags.None || !bs.requiredWorkTags.OverlapsWithOnAnyWorkType(pawn.story.Childhood.workDisables)
			select bs).TryRandomElementByWeight(new Func<BackstoryDef, float>(PawnBioAndNameGenerator.BackstorySelectionWeight), out backstoryDef))
			{
				Log.Error(string.Concat(new string[]
				{
					"No shuffled ",
					slot.ToString(),
					" found for ",
					pawn.ToStringSafe<Pawn>(),
					" of ",
					factionType.ToStringSafe<FactionDef>(),
					". Choosing random."
				}));
				backstoryDef = (from bs in DefDatabase<BackstoryDef>.AllDefs
				where bs.slot == slot
				select bs).RandomElement<BackstoryDef>();
			}
			if (slot == BackstorySlot.Adulthood)
			{
				pawn.story.Adulthood = backstoryDef;
			}
			else
			{
				pawn.story.Childhood = backstoryDef;
			}
			PawnBioAndNameGenerator.tmpBackstories.Clear();
		}

		// Token: 0x0600C45B RID: 50267 RVA: 0x00391568 File Offset: 0x0038F768
		private static bool TryGiveSolidBioTo(Pawn pawn, string requiredLastName, List<BackstoryCategoryFilter> backstoryCategories)
		{
			PawnBio pawnBio;
			if (!PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(backstoryCategories, pawn.kindDef, pawn.gender, requiredLastName, out pawnBio))
			{
				return false;
			}
			if (pawnBio.rare && Rand.Value < 0.5f && !PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(backstoryCategories, pawn.kindDef, pawn.gender, requiredLastName, out pawnBio))
			{
				return false;
			}
			pawn.Name = pawnBio.name;
			pawn.story.Childhood = pawnBio.childhood;
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20f)
			{
				pawn.story.Adulthood = pawnBio.adulthood;
			}
			return true;
		}

		// Token: 0x0600C45C RID: 50268 RVA: 0x003915FC File Offset: 0x0038F7FC
		private static bool IsBioUseable(PawnBio bio, BackstoryCategoryFilter categoryFilter, PawnKindDef kind, Gender gender, string requiredLastName)
		{
			if (bio.gender != GenderPossibility.Either)
			{
				if (gender == Gender.Male && bio.gender != GenderPossibility.Male)
				{
					return false;
				}
				if (gender == Gender.Female && bio.gender != GenderPossibility.Female)
				{
					return false;
				}
			}
			if (!requiredLastName.NullOrEmpty() && bio.name.Last != requiredLastName)
			{
				return false;
			}
			if (kind.factionLeader && !bio.pirateKing)
			{
				return false;
			}
			if (!categoryFilter.Matches(bio))
			{
				return false;
			}
			if (bio.name.UsedThisGame)
			{
				return false;
			}
			if (kind.requiredWorkTags != WorkTags.None)
			{
				if (bio.childhood != null && (bio.childhood.workDisables & kind.requiredWorkTags) != WorkTags.None)
				{
					return false;
				}
				if (bio.adulthood != null && (bio.adulthood.workDisables & kind.requiredWorkTags) != WorkTags.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C45D RID: 50269 RVA: 0x003916C0 File Offset: 0x0038F8C0
		private static bool TryGetRandomUnusedSolidBioFor(List<BackstoryCategoryFilter> backstoryCategories, PawnKindDef kind, Gender gender, string requiredLastName, out PawnBio result)
		{
			BackstoryCategoryFilter categoryFilter = backstoryCategories.RandomElementByWeight((BackstoryCategoryFilter c) => c.commonality) ?? PawnBioAndNameGenerator.FallbackCategoryGroup;
			if (Rand.Value < 0.5f)
			{
				PawnBioAndNameGenerator.tmpNames.Clear();
				PawnBioAndNameGenerator.tmpNames.AddRange(Prefs.PreferredNames);
				PawnBioAndNameGenerator.tmpNames.Shuffle<string>();
				foreach (string a in PawnBioAndNameGenerator.tmpNames)
				{
					foreach (PawnBio pawnBio in SolidBioDatabase.allBios)
					{
						if (a == pawnBio.name.ToString() && PawnBioAndNameGenerator.IsBioUseable(pawnBio, categoryFilter, kind, gender, requiredLastName))
						{
							result = pawnBio;
							return true;
						}
					}
				}
			}
			return (from bio in SolidBioDatabase.allBios.TakeRandom(20)
			where PawnBioAndNameGenerator.IsBioUseable(bio, categoryFilter, kind, gender, requiredLastName)
			select bio).TryRandomElementByWeight(new Func<PawnBio, float>(PawnBioAndNameGenerator.BioSelectionWeight), out result);
		}

		// Token: 0x0600C45E RID: 50270 RVA: 0x0039183C File Offset: 0x0038FA3C
		public static NameTriple TryGetRandomUnusedSolidName(Gender gender, string requiredLastName = null, bool forceNoNick = false)
		{
			List<NameTriple> listForGender = PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Either);
			List<NameTriple> list = (gender == Gender.Male) ? PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Male) : PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Female);
			float num = ((float)listForGender.Count + 0.1f) / ((float)(listForGender.Count + list.Count) + 0.1f);
			List<NameTriple> list2;
			if (Rand.Value < num)
			{
				list2 = listForGender;
			}
			else
			{
				list2 = list;
			}
			if (list2.Count == 0)
			{
				Log.Error("Empty solid pawn name list for gender: " + gender.ToString() + ".");
				return null;
			}
			if (Rand.Value < 0.5f)
			{
				PawnBioAndNameGenerator.tmpNames.Clear();
				PawnBioAndNameGenerator.tmpNames.AddRange(Prefs.PreferredNames);
				PawnBioAndNameGenerator.tmpNames.Shuffle<string>();
				foreach (string rawName in PawnBioAndNameGenerator.tmpNames)
				{
					NameTriple nameTriple = NameTriple.FromString(rawName, forceNoNick);
					if (list2.Contains(nameTriple) && !nameTriple.UsedThisGame && (requiredLastName == null || !(nameTriple.Last != requiredLastName)))
					{
						return nameTriple;
					}
				}
			}
			list2.Shuffle<NameTriple>();
			return (from name in list2
			where (requiredLastName == null || !(name.Last != requiredLastName)) && !name.UsedThisGame
			select name).FirstOrDefault<NameTriple>();
		}

		// Token: 0x0600C45F RID: 50271 RVA: 0x0039199C File Offset: 0x0038FB9C
		private static List<BackstoryCategoryFilter> GetBackstoryCategoryFiltersFor(Pawn pawn, FactionDef faction)
		{
			if (pawn.DevelopmentalStage.Baby())
			{
				return new List<BackstoryCategoryFilter>
				{
					PawnBioAndNameGenerator.NewbornCategoryGroup
				};
			}
			if (pawn.DevelopmentalStage.Child())
			{
				if (faction == FactionDefOf.PlayerTribe)
				{
					return LifeStageWorker_HumanlikeChild.ChildTribalBackstoryFilters;
				}
				return new List<BackstoryCategoryFilter>
				{
					PawnBioAndNameGenerator.ChildCategoryGroup
				};
			}
			else
			{
				if (!pawn.kindDef.backstoryFiltersOverride.NullOrEmpty<BackstoryCategoryFilter>())
				{
					return pawn.kindDef.backstoryFiltersOverride;
				}
				List<BackstoryCategoryFilter> list = new List<BackstoryCategoryFilter>();
				if (pawn.kindDef.backstoryFilters != null)
				{
					list.AddRange(pawn.kindDef.backstoryFilters);
				}
				if (faction != null && !faction.backstoryFilters.NullOrEmpty<BackstoryCategoryFilter>())
				{
					for (int i = 0; i < faction.backstoryFilters.Count; i++)
					{
						BackstoryCategoryFilter item = faction.backstoryFilters[i];
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
				if (!list.NullOrEmpty<BackstoryCategoryFilter>())
				{
					return list;
				}
				string[] array = new string[5];
				array[0] = "PawnKind ";
				int num = 1;
				PawnKindDef kindDef = pawn.kindDef;
				array[num] = ((kindDef != null) ? kindDef.ToString() : null);
				array[2] = " generating with factionDef ";
				array[3] = ((faction != null) ? faction.ToString() : null);
				array[4] = ": no backstoryCategories in either.";
				Log.ErrorOnce(string.Concat(array), 1871521);
				return new List<BackstoryCategoryFilter>
				{
					PawnBioAndNameGenerator.FallbackCategoryGroup
				};
			}
		}

		// Token: 0x0600C460 RID: 50272 RVA: 0x00391AE4 File Offset: 0x0038FCE4
		public static Name GeneratePawnName(Pawn pawn, NameStyle style = NameStyle.Full, string forcedLastName = null, bool forceNoNick = false, XenotypeDef xenotype = null)
		{
			if (style == NameStyle.Full)
			{
				Faction faction = pawn.Faction;
				CultureDef cultureDef;
				if (faction == null)
				{
					cultureDef = null;
				}
				else
				{
					FactionIdeosTracker ideos = faction.ideos;
					cultureDef = ((ideos != null) ? ideos.PrimaryCulture : null);
				}
				CultureDef cultureDef2 = cultureDef;
				if (Find.IdeoManager.classicMode && cultureDef2 != null && !pawn.Faction.def.allowedCultures.NullOrEmpty<CultureDef>())
				{
					cultureDef2 = pawn.Faction.def.allowedCultures.RandomElement<CultureDef>();
				}
				return PawnBioAndNameGenerator.GenerateFullPawnName(pawn.def, pawn.kindDef.GetNameMaker(pawn.gender), pawn.story, xenotype, pawn.RaceProps.GetNameGenerator(pawn.gender), cultureDef2, pawn.IsCreepJoiner, pawn.gender, pawn.RaceProps.nameCategory, forcedLastName, forceNoNick);
			}
			if (style == NameStyle.Numeric)
			{
				try
				{
					foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
					{
						NameSingle nameSingle = pawn2.Name as NameSingle;
						if (nameSingle != null)
						{
							PawnBioAndNameGenerator.usedNamesTmp.Add(nameSingle.Name);
						}
					}
					int num = 1;
					string text;
					for (;;)
					{
						text = pawn.KindLabel.CapitalizeFirst() + " " + num.ToString();
						if (!PawnBioAndNameGenerator.usedNamesTmp.Contains(text))
						{
							break;
						}
						num++;
					}
					return new NameSingle(text, true);
				}
				finally
				{
					PawnBioAndNameGenerator.usedNamesTmp.Clear();
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600C461 RID: 50273 RVA: 0x00391C64 File Offset: 0x0038FE64
		public static Name GenerateFullPawnName(ThingDef genFor, RulePackDef pawnKindNameMaker = null, Pawn_StoryTracker story = null, XenotypeDef xenotype = null, RulePackDef nameGenner = null, CultureDef primaryCulture = null, bool creepjoiner = false, Gender gender = Gender.None, PawnNameCategory nameCategory = PawnNameCategory.HumanStandard, string forcedLastName = null, bool forceNoNick = false)
		{
			if (ModsConfig.AnomalyActive && creepjoiner)
			{
				string name = PawnNameDatabaseShuffled.BankOf(nameCategory).GetName(PawnNameSlot.First, gender, true);
				List<Rule> extraRules = new List<Rule>
				{
					new Rule_String("creepjoinerFirstname", name)
				};
				return PawnBioAndNameGenerator.NameResolvedFrom(RulePackDefOf.NamerPersonCreepjoiner, forceNoNick, extraRules);
			}
			if (ModsConfig.BiotechActive && xenotype != null)
			{
				RulePackDef nameMaker = xenotype.GetNameMaker(gender);
				if (nameMaker != null && Rand.Value < xenotype.chanceToUseNameMaker)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(nameMaker, forceNoNick, null);
				}
			}
			if (pawnKindNameMaker != null)
			{
				return PawnBioAndNameGenerator.NameResolvedFrom(pawnKindNameMaker, forceNoNick, null);
			}
			if (story != null)
			{
				BackstoryDef childhood = story.Childhood;
				if (((childhood != null) ? childhood.nameMaker : null) != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(story.Childhood.nameMaker, forceNoNick, null);
				}
				BackstoryDef adulthood = story.Adulthood;
				if (((adulthood != null) ? adulthood.nameMaker : null) != null)
				{
					return PawnBioAndNameGenerator.NameResolvedFrom(story.Adulthood.nameMaker, forceNoNick, null);
				}
			}
			if (nameGenner != null)
			{
				return new NameSingle(NameGenerator.GenerateName(nameGenner, (string x) => !new NameSingle(x, false).UsedThisGame, false, null, null, null), false);
			}
			RulePackDef rulePackDef = (primaryCulture != null) ? primaryCulture.GetPawnNameMaker(gender) : null;
			if (rulePackDef != null)
			{
				return PawnBioAndNameGenerator.NameResolvedFrom(rulePackDef, forceNoNick, null);
			}
			if (nameCategory != PawnNameCategory.NoName)
			{
				if (Rand.Value < 0.5f)
				{
					NameTriple nameTriple = PawnBioAndNameGenerator.TryGetRandomUnusedSolidName(gender, forcedLastName, forceNoNick);
					if (nameTriple != null)
					{
						return nameTriple;
					}
				}
				return PawnBioAndNameGenerator.GeneratePawnName_Shuffled(nameCategory, gender, forcedLastName, false);
			}
			Log.Error("No name making method for " + ((genFor != null) ? genFor.ToString() : null));
			return NameTriple.FromString(genFor.label, false);
		}

		// Token: 0x0600C462 RID: 50274 RVA: 0x00391DEA File Offset: 0x0038FFEA
		private static Name NameResolvedFrom(RulePackDef nameMaker, bool forceNoNick = false, List<Rule> extraRules = null)
		{
			return NameTriple.FromString(NameGenerator.GenerateName(nameMaker, (string x) => !NameTriple.FromString(x, false).UsedThisGame, false, null, null, extraRules), forceNoNick);
		}

		// Token: 0x0600C463 RID: 50275 RVA: 0x00391E1C File Offset: 0x0039001C
		private static NameTriple GeneratePawnName_Shuffled(PawnNameCategory nType, Gender gender, string forcedLastName = null, bool forceNoNick = false)
		{
			if (nType == PawnNameCategory.NoName)
			{
				Log.Message("Can't create a name of type NoName. Defaulting to HumanStandard.");
				nType = PawnNameCategory.HumanStandard;
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(nType);
			string name = nameBank.GetName(PawnNameSlot.First, gender, true);
			string text = forcedLastName ?? nameBank.GetName(PawnNameSlot.Last, Gender.None, true);
			string nick;
			if (!forceNoNick)
			{
				int num = 0;
				Func<Name, bool> <>9__0;
				IEnumerable<Name> allPawnsNamesEverUsed;
				Func<Name, bool> predicate;
				do
				{
					num++;
					if (Rand.Value < 0.15f)
					{
						Gender gender2 = gender;
						if (Rand.Value < 0.8f)
						{
							gender2 = Gender.None;
						}
						nick = nameBank.GetName(PawnNameSlot.Nick, gender2, true);
					}
					else if (Rand.Value < 0.5f)
					{
						nick = name;
					}
					else
					{
						nick = text;
					}
					if (num >= 50)
					{
						break;
					}
					allPawnsNamesEverUsed = NameUseChecker.AllPawnsNamesEverUsed;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = delegate(Name x)
						{
							NameTriple nameTriple = x as NameTriple;
							return nameTriple != null && nameTriple.Nick == nick;
						});
					}
				}
				while (allPawnsNamesEverUsed.Any(predicate));
			}
			else
			{
				nick = null;
			}
			return new NameTriple(name, nick, text);
		}

		// Token: 0x0600C464 RID: 50276 RVA: 0x00391F06 File Offset: 0x00390106
		private static float BackstorySelectionWeight(BackstoryDef bs)
		{
			return PawnBioAndNameGenerator.SelectionWeightFactorFromWorkTagsDisabled(bs.workDisables);
		}

		// Token: 0x0600C465 RID: 50277 RVA: 0x00391F13 File Offset: 0x00390113
		private static float BioSelectionWeight(PawnBio bio)
		{
			return PawnBioAndNameGenerator.SelectionWeightFactorFromWorkTagsDisabled(bio.adulthood.workDisables | bio.childhood.workDisables);
		}

		// Token: 0x0600C466 RID: 50278 RVA: 0x00391F34 File Offset: 0x00390134
		private static float SelectionWeightFactorFromWorkTagsDisabled(WorkTags wt)
		{
			float num = 1f;
			if ((wt & WorkTags.ManualDumb) != WorkTags.None)
			{
				num *= 0.5f;
			}
			if ((wt & WorkTags.ManualSkilled) != WorkTags.None)
			{
				num *= 1f;
			}
			if ((wt & WorkTags.Violent) != WorkTags.None)
			{
				num *= 0.6f;
			}
			if ((wt & WorkTags.Social) != WorkTags.None)
			{
				num *= 0.7f;
			}
			if ((wt & WorkTags.Intellectual) != WorkTags.None)
			{
				num *= 0.4f;
			}
			if ((wt & WorkTags.Firefighting) != WorkTags.None)
			{
				num *= 0.8f;
			}
			return num;
		}

		// Token: 0x040085EC RID: 34284
		private const float MinAgeForAdulthood = 20f;

		// Token: 0x040085ED RID: 34285
		private const float SolidBioChance = 0.25f;

		// Token: 0x040085EE RID: 34286
		private const float SolidNameChance = 0.5f;

		// Token: 0x040085EF RID: 34287
		private const float TryPreferredNameChance_Bio = 0.5f;

		// Token: 0x040085F0 RID: 34288
		private const float TryPreferredNameChance_Name = 0.5f;

		// Token: 0x040085F1 RID: 34289
		private const float ShuffledNicknameChance = 0.15f;

		// Token: 0x040085F2 RID: 34290
		private const float ShuffledNicknameChanceImperial = 0.05f;

		// Token: 0x040085F3 RID: 34291
		private const float ShuffledNicknameChanceUnisex = 0.8f;

		// Token: 0x040085F4 RID: 34292
		public static readonly BackstoryCategoryFilter ChildCategoryGroup = new BackstoryCategoryFilter
		{
			categories = new List<string>
			{
				"Child"
			},
			commonality = 1f
		};

		// Token: 0x040085F5 RID: 34293
		private static readonly BackstoryCategoryFilter NewbornCategoryGroup = new BackstoryCategoryFilter
		{
			categories = new List<string>
			{
				"Newborn"
			},
			commonality = 1f
		};

		// Token: 0x040085F6 RID: 34294
		private static readonly BackstoryCategoryFilter FallbackCategoryGroup = new BackstoryCategoryFilter
		{
			categories = new List<string>
			{
				"Civil"
			},
			commonality = 1f
		};

		// Token: 0x040085F7 RID: 34295
		private static List<BackstoryDef> tmpBackstories = new List<BackstoryDef>();

		// Token: 0x040085F8 RID: 34296
		private static List<string> tmpNames = new List<string>();

		// Token: 0x040085F9 RID: 34297
		private static HashSet<string> usedNamesTmp = new HashSet<string>();
	}
}
