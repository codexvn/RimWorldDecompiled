using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse.Grammar
{
	// Token: 0x02000DDD RID: 3549
	public static class GrammarUtility
	{
		// Token: 0x06005A57 RID: 23127 RVA: 0x001CF124 File Offset: 0x001CD324
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Pawn pawn, Dictionary<string, string> constants = null, bool addRelationInfoSymbol = true, bool addTags = true)
		{
			if (pawn == null)
			{
				Log.ErrorOnce("Tried to insert rule " + pawnSymbol + " for null pawn", 16015097);
				return Enumerable.Empty<Rule>();
			}
			TaggedString taggedString = "";
			if (addRelationInfoSymbol)
			{
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			}
			Name name = pawn.Name;
			Pawn_StoryTracker story = pawn.story;
			string title = (story != null) ? story.Title : null;
			PawnKindDef kindDef = pawn.kindDef;
			Gender gender = pawn.gender;
			Faction faction = pawn.Faction;
			int ageBiologicalYears = pawn.ageTracker.AgeBiologicalYears;
			int ageChronologicalYears = pawn.ageTracker.AgeChronologicalYears;
			string relationInfo = taggedString;
			bool everBeenColonistOrTameAnimal = PawnUtility.EverBeenColonistOrTameAnimal(pawn);
			bool everBeenQuestLodger = PawnUtility.EverBeenQuestLodger(pawn);
			bool isFactionLeader = pawn.Faction != null && pawn.Faction.leader == pawn;
			Pawn_RoyaltyTracker royalty = pawn.royalty;
			return GrammarUtility.RulesForPawn(pawnSymbol, name, title, kindDef, gender, faction, ageBiologicalYears, ageChronologicalYears, relationInfo, everBeenColonistOrTameAnimal, everBeenQuestLodger, isFactionLeader, (royalty != null) ? royalty.AllTitlesForReading : null, ModsConfig.AnomalyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false), pawn.LabelNoParenthesis, constants, addTags);
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x001CF218 File Offset: 0x001CD418
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Name name, string title, PawnKindDef kind, Gender gender, Faction faction, int age, int chronologicalAge, string relationInfo, bool everBeenColonistOrTameAnimal, bool everBeenQuestLodger, bool isFactionLeader, List<RoyalTitle> royalTitles, bool cubeInterest, string labelNoParenthesis, Dictionary<string, string> constants = null, bool addTags = true)
		{
			string prefix = "";
			if (!pawnSymbol.NullOrEmpty())
			{
				prefix = prefix + pawnSymbol + "_";
			}
			string kindLabel;
			if (gender == Gender.Female)
			{
				kindLabel = (kind.labelFemale.NullOrEmpty() ? kind.label : kind.labelFemale);
			}
			else if (gender == Gender.Male)
			{
				kindLabel = (kind.labelMale.NullOrEmpty() ? kind.label : kind.labelMale);
			}
			else
			{
				kindLabel = kind.label;
			}
			Gender kindGender = GrammarResolverSimple.ResolveGender(kindLabel, gender);
			string text;
			if (name != null)
			{
				text = Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringFull, gender, false, true);
			}
			else
			{
				text = Find.ActiveLanguageWorker.WithIndefiniteArticle(kindLabel, kindGender, false, false);
			}
			yield return new Rule_String(prefix + "nameFull", addTags ? text.ApplyTag(TagType.Name, null).Resolve() : text);
			string nameShort;
			if (name != null)
			{
				nameShort = name.ToStringShort;
			}
			else
			{
				nameShort = kindLabel;
			}
			yield return new Rule_String(prefix + "label", addTags ? nameShort.ApplyTag(TagType.Name, null).Resolve() : nameShort);
			yield return new Rule_String(prefix + "labelNoParenthesis", addTags ? labelNoParenthesis.ApplyTag(TagType.Name, null).Resolve() : labelNoParenthesis);
			string nameShortDef;
			if (name != null)
			{
				nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(name.ToStringShort, gender, false, true);
			}
			else
			{
				nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(kindLabel, kindGender, false, false);
			}
			yield return new Rule_String(prefix + "definite", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			yield return new Rule_String(prefix + "nameDef", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			nameShortDef = null;
			if (name != null)
			{
				nameShortDef = Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringShort, gender, false, true);
			}
			else
			{
				nameShortDef = Find.ActiveLanguageWorker.WithIndefiniteArticle(kindLabel, kindGender, false, false);
			}
			yield return new Rule_String(prefix + "indefinite", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			yield return new Rule_String(prefix + "nameIndef", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			nameShortDef = null;
			yield return new Rule_String(prefix + "pronoun", ((name != null) ? gender : kindGender).GetPronoun());
			yield return new Rule_String(prefix + "possessive", ((name != null) ? gender : kindGender).GetPossessive());
			yield return new Rule_String(prefix + "objective", ((name != null) ? gender : kindGender).GetObjective());
			if (faction != null)
			{
				yield return new Rule_String(prefix + "factionName", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
				if (constants != null)
				{
					constants[prefix + "faction"] = faction.def.defName;
				}
			}
			if (constants != null && isFactionLeader)
			{
				constants[prefix + "factionLeader"] = "True";
			}
			if (kind != null)
			{
				yield return new Rule_String(prefix + "kind", GenLabel.BestKindLabel(kind, kindGender, false, -1));
				yield return new Rule_String(prefix + "kindPlural", GenLabel.BestKindLabel(kind, kindGender, true, -1));
			}
			if (title != null)
			{
				yield return new Rule_String(prefix + "title", title);
				Gender titleGender = LanguageDatabase.activeLanguage.ResolveGender(title, null, gender);
				yield return new Rule_String(prefix + "titleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(title, titleGender, false, false));
				yield return new Rule_String(prefix + "titleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(title, titleGender, false, false));
			}
			if (royalTitles != null)
			{
				int royalTitleIndex = 0;
				RoyalTitle bestTitle = null;
				foreach (RoyalTitle royalTitle in from x in royalTitles
				orderby x.def.index
				select x)
				{
					yield return new Rule_String(prefix + "royalTitle" + royalTitleIndex.ToString(), royalTitle.def.GetLabelFor(gender));
					yield return new Rule_String(prefix + "royalTitle" + royalTitleIndex.ToString() + "Indef", Find.ActiveLanguageWorker.WithIndefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "royalTitle" + royalTitleIndex.ToString() + "Def", Find.ActiveLanguageWorker.WithDefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "royalTitleFaction" + royalTitleIndex.ToString(), royalTitle.faction.Name.ApplyTag(royalTitle.faction).Resolve());
					if (royalTitle.faction == faction)
					{
						yield return new Rule_String(prefix + "royalTitleInCurrentFaction", royalTitle.def.GetLabelFor(gender));
						yield return new Rule_String(prefix + "royalTitleInCurrentFactionIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
						yield return new Rule_String(prefix + "royalTitleInCurrentFactionDef", Find.ActiveLanguageWorker.WithDefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
						if (constants != null)
						{
							constants[prefix + "royalInCurrentFaction"] = "True";
						}
					}
					if (bestTitle == null || royalTitle.def.favorCost > bestTitle.def.favorCost)
					{
						bestTitle = royalTitle;
					}
					int num = royalTitleIndex;
					royalTitleIndex = num + 1;
					royalTitle = null;
				}
				IEnumerator<RoyalTitle> enumerator = null;
				if (bestTitle != null)
				{
					yield return new Rule_String(prefix + "bestRoyalTitle", bestTitle.def.GetLabelFor(gender));
					yield return new Rule_String(prefix + "bestRoyalTitleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(bestTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "bestRoyalTitleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(bestTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "bestRoyalTitleFaction", bestTitle.faction.Name);
				}
				bestTitle = null;
			}
			yield return new Rule_String(prefix + "age", age.ToString());
			yield return new Rule_String(prefix + "chronologicalAge", chronologicalAge.ToString());
			if (everBeenColonistOrTameAnimal)
			{
				yield return new Rule_String("formerlyColonistInfo", "PawnWasFormerlyColonist".Translate(nameShort));
				if (constants != null)
				{
					constants[prefix + "formerlyColonist"] = "True";
				}
			}
			else if (everBeenQuestLodger)
			{
				yield return new Rule_String("formerlyColonistInfo", "PawnWasFormerlyLodger".Translate(nameShort));
				if (constants != null)
				{
					constants[prefix + "formerlyColonist"] = "True";
				}
			}
			yield return new Rule_String(prefix + "relationInfo", relationInfo);
			if (constants != null && kind != null)
			{
				constants[prefix + "flesh"] = kind.race.race.FleshType.defName;
			}
			if (constants != null)
			{
				constants[prefix + "gender"] = gender.ToString();
				constants[prefix + "genderResolved"] = ((name != null) ? gender : kindGender).ToString();
				constants[prefix + "cubeInterest"] = cubeInterest.ToString();
			}
			yield break;
			yield break;
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x001CF2B0 File Offset: 0x001CD4B0
		public static IEnumerable<Rule> RulesForThing(string prefix, Thing thing)
		{
			prefix += "_";
			QualityCategory cat;
			if (thing.TryGetQuality(out cat))
			{
				yield return new Rule_String(prefix + "quality", cat.GetLabel().ToLower());
			}
			yield break;
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x001CF2C7 File Offset: 0x001CD4C7
		public static IEnumerable<Rule> RulesForDef(string prefix, Def def)
		{
			if (def == null)
			{
				Log.ErrorOnce("Tried to insert rule " + prefix + " for null def", 79641686);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", def.label);
			PawnKindDef pawnKindDef = def as PawnKindDef;
			if (pawnKindDef != null)
			{
				yield return new Rule_String(prefix + "labelPlural", pawnKindDef.GetLabelPlural(-1));
			}
			else
			{
				yield return new Rule_String(prefix + "labelPlural", Find.ActiveLanguageWorker.Pluralize(def.label, -1));
			}
			yield return new Rule_String(prefix + "description", def.description);
			yield return new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(def.label, false, false));
			yield return new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label, false, false));
			yield return new Rule_String(prefix + "possessive", "Proits".Translate());
			yield break;
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x001CF2DE File Offset: 0x001CD4DE
		public static IEnumerable<Rule> RulesForBodyPartRecord(string prefix, BodyPartRecord part)
		{
			if (part == null)
			{
				Log.ErrorOnce("Tried to insert rule " + prefix + " for null body part", 394876778);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", part.Label);
			yield return new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(part.Label, false, false));
			yield return new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(part.Label, false, false));
			yield return new Rule_String(prefix + "possessive", "Proits".Translate());
			yield break;
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x001CF2F5 File Offset: 0x001CD4F5
		public static IEnumerable<Rule> RulesForHediffDef(string prefix, HediffDef def, BodyPartRecord part)
		{
			foreach (Rule rule in GrammarUtility.RulesForDef(prefix, def))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", def.label);
			string output = (!def.labelNoun.NullOrEmpty()) ? def.labelNoun : def.label;
			yield return new Rule_String(prefix + "labelNoun", output);
			string text = def.PrettyTextForPart(part);
			if (!text.NullOrEmpty())
			{
				yield return new Rule_String(prefix + "labelNounPretty", text);
			}
			yield break;
			yield break;
		}

		// Token: 0x06005A5D RID: 23133 RVA: 0x001CF313 File Offset: 0x001CD513
		public static IEnumerable<Rule> RulesForFaction(string prefix, Faction faction, Dictionary<string, string> constants = null, bool addTags = true)
		{
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			if (faction == null)
			{
				yield return new Rule_String(prefix + "name", "FactionUnaffiliated".Translate());
				yield break;
			}
			yield return new Rule_String(prefix + "name", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
			yield return new Rule_String(prefix + "pawnSingular", faction.def.pawnSingular);
			yield return new Rule_String(prefix + "pawnSingularDef", Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnSingular, false, false));
			yield return new Rule_String(prefix + "pawnSingularIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnSingular, false, false));
			yield return new Rule_String(prefix + "pawnsPlural", faction.def.pawnsPlural);
			yield return new Rule_String(prefix + "pawnsPluralDef", Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular, Gender.Male), true, false));
			yield return new Rule_String(prefix + "pawnsPluralIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular, Gender.Male), true, false));
			yield return new Rule_String(prefix + "leaderTitle", faction.LeaderTitle);
			yield return new Rule_String(prefix + "royalFavorLabel", faction.def.royalFavorLabel);
			if (constants != null)
			{
				constants.Add(prefix + "temporary", faction.temporary.ToString());
				constants.Add(prefix + "hasLeader", (faction.leader != null) ? "True" : "False");
			}
			yield break;
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x001CF338 File Offset: 0x001CD538
		public static IEnumerable<Rule> RulesForWorldObject(string prefix, WorldObject worldObject, bool addTags = true)
		{
			GrammarUtility.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.worldObject = worldObject;
			CS$<>8__locals1.addTags = addTags;
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|7_0(CS$<>8__locals1.worldObject.Label, ref CS$<>8__locals1));
			yield return new Rule_String(prefix + "definite", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|7_0(Find.ActiveLanguageWorker.WithDefiniteArticle(CS$<>8__locals1.worldObject.Label, false, CS$<>8__locals1.worldObject.HasName), ref CS$<>8__locals1));
			yield return new Rule_String(prefix + "indefinite", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|7_0(Find.ActiveLanguageWorker.WithIndefiniteArticle(CS$<>8__locals1.worldObject.Label, false, CS$<>8__locals1.worldObject.HasName), ref CS$<>8__locals1));
			yield break;
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x001CF356 File Offset: 0x001CD556
		public static IEnumerable<Rule> RulesForIdeo(string prefix, Ideo ideo)
		{
			if (ideo == null)
			{
				Log.ErrorOnce("Tried to insert rule " + prefix + " for null ideo", 453454453);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "name", ideo.name.ApplyTag(ideo).Resolve());
			yield return new Rule_String(prefix + "memberName", ideo.memberName);
			yield return new Rule_String(prefix + "memberNamePlural", ideo.MemberNamePlural);
			yield break;
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x001CF36D File Offset: 0x001CD56D
		public static IEnumerable<Rule> RulesForPrecept(string prefix, Precept precept)
		{
			if (precept == null)
			{
				Log.ErrorOnce("Tried to insert rule " + prefix + " for null precet", 823453451);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "name", precept.Label.ApplyTag(precept.ideo).Resolve());
			yield break;
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x001CF384 File Offset: 0x001CD584
		[CompilerGenerated]
		internal static string <RulesForWorldObject>g__PossiblyWithTag|7_0(string str, ref GrammarUtility.<>c__DisplayClass7_0 A_1)
		{
			if (!(A_1.worldObject.Faction != null & A_1.addTags))
			{
				return str;
			}
			return str.ApplyTag(TagType.Settlement, A_1.worldObject.Faction.GetUniqueLoadID()).Resolve();
		}
	}
}
