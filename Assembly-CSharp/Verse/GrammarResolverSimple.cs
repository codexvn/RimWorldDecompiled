using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000362 RID: 866
	public static class GrammarResolverSimple
	{
		// Token: 0x0600162A RID: 5674 RVA: 0x000689D0 File Offset: 0x00066BD0
		public static TaggedString Formatted(TaggedString str, List<string> argsLabelsArg, List<object> argsObjectsArg)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			bool flag;
			StringBuilder stringBuilder;
			List<string> list;
			List<object> list2;
			if (GrammarResolverSimple.formatterWorking)
			{
				flag = false;
				stringBuilder = new StringBuilder();
				list = argsLabelsArg.ToList<string>();
				list2 = argsObjectsArg.ToList<object>();
			}
			else
			{
				flag = true;
				stringBuilder = GrammarResolverSimple.tmpResultBuffer;
				list = GrammarResolverSimple.tmpArgsLabels;
				list.Clear();
				list.AddRange(argsLabelsArg);
				list2 = GrammarResolverSimple.tmpArgsObjects;
				list2.Clear();
				list2.AddRange(argsObjectsArg);
			}
			if (flag)
			{
				GrammarResolverSimple.formatterWorking = true;
			}
			TaggedString result;
			try
			{
				stringBuilder.Length = 0;
				GrammarResolverSimple.TryResolveInner(str, 0, stringBuilder, list, list2, false);
				string str2 = GenText.CapitalizeSentences(stringBuilder.ToString(), false);
				str2 = Find.ActiveLanguageWorker.PostProcessed(str2);
				result = str2;
			}
			finally
			{
				if (flag)
				{
					GrammarResolverSimple.formatterWorking = false;
				}
			}
			return result;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00068A98 File Offset: 0x00066C98
		private static int TryResolveInner(TaggedString str, int strOffset, StringBuilder resultBuffer, List<string> argsLabels, List<object> argsObjects, bool recursive = false)
		{
			bool flag = false;
			StringBuilder stringBuilder;
			StringBuilder stringBuilder2;
			StringBuilder stringBuilder3;
			StringBuilder stringBuilder4;
			StringBuilder stringBuilder5;
			StringBuilder stringBuilder6;
			if (GrammarResolverSimple.symbolParserWorking)
			{
				stringBuilder = new StringBuilder();
				stringBuilder2 = new StringBuilder();
				stringBuilder3 = new StringBuilder();
				stringBuilder4 = new StringBuilder();
				stringBuilder5 = new StringBuilder();
				stringBuilder6 = new StringBuilder();
			}
			else
			{
				flag = true;
				GrammarResolverSimple.symbolParserWorking = true;
				stringBuilder = GrammarResolverSimple.tmpSymbolBuffer;
				stringBuilder2 = GrammarResolverSimple.tmpSymbolBuffer_objectLabel;
				stringBuilder3 = GrammarResolverSimple.tmpSymbolBuffer_subSymbol;
				stringBuilder4 = GrammarResolverSimple.tmpSymbolBuffer_function;
				stringBuilder5 = GrammarResolverSimple.tmpSymbolBuffer_args;
				stringBuilder6 = GrammarResolverSimple.tmpSymbolBuffer_functionArgs;
			}
			int num = 0;
			int i = strOffset;
			while (i < str.Length)
			{
				char c = str[i];
				if (c == '{')
				{
					stringBuilder.Length = 0;
					stringBuilder4.Length = 0;
					stringBuilder2.Length = 0;
					stringBuilder3.Length = 0;
					stringBuilder5.Length = 0;
					stringBuilder6.Length = 0;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					i++;
					bool flag6 = i < str.Length && str[i] == '{';
					if (!flag6)
					{
						bool flag7 = false;
						int j = i;
						int num2 = 0;
						while (j < str.Length)
						{
							char c2 = str[j];
							if (c2 == '{' || c2 == '}' || c2 == '?' || (c2 == ' ' && flag7))
							{
								break;
							}
							if (c2 != ' ')
							{
								flag7 = true;
							}
							if (c2 == ':')
							{
								flag5 = true;
								break;
							}
							stringBuilder4.Append(c2);
							j++;
							num2++;
						}
						if (flag5)
						{
							i = j + 1;
							num += num2 + 1;
						}
					}
					while (i < str.Length)
					{
						char c3 = str[i];
						if (c3 == '}')
						{
							flag2 = true;
							num++;
							break;
						}
						if (c3 == '{' && !flag6)
						{
							if (flag4 || flag5)
							{
								int num3 = GrammarResolverSimple.TryResolveInner(str, i, flag5 ? stringBuilder6 : stringBuilder5, argsLabels, argsObjects, true);
								i += num3;
								num += num3;
							}
							else
							{
								Log.ErrorOnce("Tried to use nested symbol for something but a symbol argument, this is not supported. For string: " + str, str.GetHashCode() ^ 194857266);
							}
						}
						else
						{
							stringBuilder.Append(c3);
							if (!flag5)
							{
								if (c3 == '_' && !flag3)
								{
									flag3 = true;
								}
								else if (c3 == '?' && !flag4)
								{
									flag4 = true;
								}
								else if (flag4)
								{
									stringBuilder5.Append(c3);
								}
								else if (flag3)
								{
									stringBuilder3.Append(c3);
								}
								else
								{
									stringBuilder2.Append(c3);
								}
							}
							else
							{
								stringBuilder6.Append(c3);
							}
						}
						i++;
						num++;
					}
					if (!flag2)
					{
						Log.ErrorOnce("Could not find matching '}' in \"" + str + "\".", str.GetHashCode() ^ 194857261);
					}
					else if (flag6)
					{
						resultBuffer.Append(stringBuilder);
					}
					else if (flag5)
					{
						resultBuffer.Append(GrammarResolverSimple.ResolveFunction(stringBuilder4.ToString(), stringBuilder6.ToString(), str));
					}
					else
					{
						if (flag4)
						{
							while (stringBuilder3.Length != 0 && stringBuilder3[stringBuilder3.Length - 1] == ' ')
							{
								StringBuilder stringBuilder7 = stringBuilder3;
								int length = stringBuilder7.Length;
								stringBuilder7.Length = length - 1;
							}
						}
						string text = stringBuilder2.ToString();
						bool flag8 = false;
						int num4 = -1;
						if (int.TryParse(text, out num4))
						{
							TaggedString taggedString;
							if (num4 >= 0 && num4 < argsObjects.Count && GrammarResolverSimple.TryResolveSymbol(argsObjects[num4], stringBuilder3.ToString(), stringBuilder5.ToString(), out taggedString, str))
							{
								flag8 = true;
								resultBuffer.Append(taggedString.RawText);
							}
						}
						else
						{
							int k = 0;
							while (k < argsLabels.Count)
							{
								if (argsLabels[k] == text)
								{
									TaggedString taggedString2;
									if (GrammarResolverSimple.TryResolveSymbol(argsObjects[k], stringBuilder3.ToString(), stringBuilder5.ToString(), out taggedString2, str))
									{
										flag8 = true;
										resultBuffer.Append(taggedString2.RawText);
										break;
									}
									break;
								}
								else
								{
									k++;
								}
							}
							for (int l = 0; l < argsLabels.Count; l++)
							{
								string a = argsLabels[l];
								string str2 = text;
								string str3 = "_";
								StringBuilder stringBuilder8 = stringBuilder3;
								if (a == str2 + str3 + ((stringBuilder8 != null) ? stringBuilder8.ToString() : null))
								{
									object obj = argsObjects[l];
									if (obj is Gender)
									{
										Gender gender = (Gender)obj;
										flag8 = true;
										resultBuffer.Append(GrammarResolverSimple.ResolveGenderSymbol(gender, false, stringBuilder5.ToString(), str));
										break;
									}
								}
							}
						}
						if (!flag8)
						{
							string str4 = "Could not resolve symbol \"";
							StringBuilder stringBuilder9 = stringBuilder;
							Log.ErrorOnce(str4 + ((stringBuilder9 != null) ? stringBuilder9.ToString() : null) + "\" for string \"" + str + "\".", str.GetHashCode() ^ stringBuilder.ToString().GetHashCode() ^ 879654654);
						}
					}
					if (recursive)
					{
						break;
					}
				}
				else
				{
					resultBuffer.Append(c);
				}
				i++;
				num++;
			}
			if (flag)
			{
				GrammarResolverSimple.symbolParserWorking = false;
			}
			return num;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00068F84 File Offset: 0x00067184
		private static bool TryResolveSymbol(object obj, string subSymbol, string symbolArgs, out TaggedString resolvedStr, string fullStringForReference)
		{
			Pawn pawn = obj as Pawn;
			if (pawn != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
				if (num <= 2306218066U)
				{
					if (num <= 861101311U)
					{
						if (num <= 356082287U)
						{
							if (num <= 202251908U)
							{
								if (num != 111078584U)
								{
									if (num != 176126825U)
									{
										if (num == 202251908U)
										{
											if (subSymbol == "parentage")
											{
												resolvedStr = pawn.GetParentage();
												GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
												return true;
											}
										}
									}
									else if (subSymbol == "kindPlural")
									{
										resolvedStr = pawn.GetKindLabelPlural(-1);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
								else if (subSymbol == "genderResolved")
								{
									resolvedStr = GrammarResolverSimple.ResolveGenderSymbol((pawn.Name != null) ? pawn.gender : GrammarResolverSimple.ResolveGender(pawn.KindLabel, pawn.gender), pawn.RaceProps.Animal, symbolArgs, fullStringForReference);
									return true;
								}
							}
							else if (num <= 238594642U)
							{
								if (num != 225333296U)
								{
									if (num == 238594642U)
									{
										if (subSymbol == "factionPawnSingularIndef")
										{
											resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Faction.def.pawnSingular, false, false) : "");
											GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
											return true;
										}
									}
								}
								else if (subSymbol == "genderNoun")
								{
									resolvedStr = pawn.gender.GetGenderNoun();
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
							else if (num != 267723693U)
							{
								if (num == 356082287U)
								{
									if (subSymbol == "nameFull")
									{
										resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringFull, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite());
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "nameDef")
							{
								resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelDefinite());
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num <= 619309234U)
						{
							if (num <= 543181407U)
							{
								if (num != 418492385U)
								{
									if (num == 543181407U)
									{
										if (subSymbol == "lifeStageIndef")
										{
											resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.ageTracker.CurLifeStage.label, pawn.gender, false, false);
											GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
											return true;
										}
									}
								}
								else if (subSymbol == "definite")
								{
									resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelDefinite());
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
							else if (num != 575730602U)
							{
								if (num == 619309234U)
								{
									if (subSymbol == "labelNoParenthesis")
									{
										resolvedStr = pawn.LabelNoParenthesis;
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "lifeStageDef")
							{
								resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.ageTracker.CurLifeStage.label, pawn.gender, false, false);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num <= 667530978U)
						{
							if (num != 658875246U)
							{
								if (num == 667530978U)
								{
									if (subSymbol == "lifeStageAdjective")
									{
										resolvedStr = pawn.ageTracker.CurLifeStage.Adjective;
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "relationInfoInParentheses")
							{
								resolvedStr = "";
								PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref resolvedStr, pawn);
								if (!resolvedStr.NullOrEmpty())
								{
									resolvedStr = "(" + resolvedStr + ")";
								}
								return true;
							}
						}
						else if (num != 742476188U)
						{
							if (num == 861101311U)
							{
								if (subSymbol == "factionPawnsPluralDef")
								{
									resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(pawn.Faction.def.pawnsPlural, pawn.Faction.def.pawnSingular, Gender.Male), true, false) : "");
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "age")
						{
							resolvedStr = pawn.ageTracker.AgeBiologicalYears.ToString();
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 1587320192U)
					{
						if (num <= 1167748615U)
						{
							if (num <= 1147977518U)
							{
								if (num != 998961680U)
								{
									if (num == 1147977518U)
									{
										if (subSymbol == "nameFullDef")
										{
											resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Name.ToStringFull, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelDefinite());
											GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
											return true;
										}
									}
								}
								else if (subSymbol == "nameIndef")
								{
									resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite());
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
							else if (num != 1162320608U)
							{
								if (num == 1167748615U)
								{
									if (subSymbol == "kindIndef")
									{
										resolvedStr = pawn.KindLabelIndefinite();
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "factionName")
							{
								resolvedStr = ((pawn.Faction != null) ? pawn.Faction.Name.ApplyTag(pawn.Faction) : "");
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num <= 1365350650U)
						{
							if (num != 1277025515U)
							{
								if (num == 1365350650U)
								{
									if (subSymbol == "royalTitleInCurrentFaction")
									{
										resolvedStr = GrammarResolverSimple.PawnResolveRoyalTitleInCurrentFaction(pawn);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "possessive")
							{
								resolvedStr = pawn.gender.GetPossessive();
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num != 1387836843U)
						{
							if (num == 1587320192U)
							{
								if (subSymbol == "gender")
								{
									resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(pawn.gender, pawn.RaceProps.Animal, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "lifeStage")
						{
							resolvedStr = pawn.ageTracker.CurLifeStage.label;
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 1911534845U)
					{
						if (num <= 1670603679U)
						{
							if (num != 1653343472U)
							{
								if (num == 1670603679U)
								{
									if (subSymbol == "raceDef")
									{
										resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.def.label, false, false);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "kindPluralDef")
							{
								resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.GetKindLabelPlural(-1), pawn.gender, true, false);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num != 1691639576U)
						{
							if (num == 1911534845U)
							{
								if (subSymbol == "labelShort")
								{
									resolvedStr = ((pawn.Name != null) ? pawn.Name.ToStringShort.ApplyTag(TagType.Name, null) : pawn.KindLabel);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "factionRoyalFavorLabel")
						{
							resolvedStr = ((pawn.Faction != null) ? pawn.Faction.def.royalFavorLabel : "");
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 2166136261U)
					{
						if (num != 1998225958U)
						{
							if (num == 2166136261U)
							{
								if (subSymbol != null)
								{
									if (subSymbol.Length == 0)
									{
										resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite());
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
						}
						else if (subSymbol == "factionPawnsPluralIndef")
						{
							resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(pawn.Faction.def.pawnsPlural, pawn.Faction.def.pawnSingular, Gender.Male), true, false) : "");
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 2279990553U)
					{
						if (num == 2306218066U)
						{
							if (subSymbol == "indefinite")
							{
								resolvedStr = ((pawn.Name != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.Name.ToStringShort, pawn.gender, false, true).ApplyTag(TagType.Name, null) : pawn.KindLabelIndefinite());
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "relationInfo")
					{
						resolvedStr = "";
						TaggedString taggedString = resolvedStr;
						PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
						resolvedStr = taggedString.RawText;
						return true;
					}
				}
				else if (num <= 3246736155U)
				{
					if (num <= 2618666040U)
					{
						if (num <= 2528592613U)
						{
							if (num != 2360586432U)
							{
								if (num != 2394669720U)
								{
									if (num == 2528592613U)
									{
										if (subSymbol == "kindBaseDef")
										{
											resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.kindDef.label, false, false);
											GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
											return true;
										}
									}
								}
								else if (subSymbol == "race")
								{
									resolvedStr = pawn.def.label;
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
							else if (subSymbol == "kindBasePlural")
							{
								resolvedStr = GenLabel.BestKindLabel(pawn.kindDef, pawn.gender, true, 2);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num <= 2602800180U)
						{
							if (num != 2556802313U)
							{
								if (num == 2602800180U)
								{
									if (subSymbol == "legalStatus")
									{
										resolvedStr = pawn.LegalStatus;
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "title")
							{
								resolvedStr = ((pawn.story != null) ? pawn.story.Title : "");
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num != 2605899927U)
						{
							if (num == 2618666040U)
							{
								if (subSymbol == "objective")
								{
									resolvedStr = pawn.gender.GetObjective();
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "kindBasePluralDef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawn.kindDef.GetLabelPlural(-1), pawn.kindDef.label, Gender.Male), true, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 2892888801U)
					{
						if (num <= 2829057629U)
						{
							if (num != 2740648940U)
							{
								if (num == 2829057629U)
								{
									if (subSymbol == "xenotype")
									{
										resolvedStr = pawn.genes.XenotypeLabel;
										return true;
									}
								}
							}
							else if (subSymbol == "factionPawnSingular")
							{
								resolvedStr = ((pawn.Faction != null) ? pawn.Faction.def.pawnSingular : "");
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num != 2835508048U)
						{
							if (num == 2892888801U)
							{
								if (subSymbol == "kindPluralIndef")
								{
									resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.GetKindLabelPlural(-1), pawn.gender, true, false);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "titleDef")
						{
							resolvedStr = ((pawn.story != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.story.Title, GrammarResolverSimple.ResolveGender(pawn.story.Title, pawn.gender), false, false) : "");
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 3109671438U)
					{
						if (num != 2994657680U)
						{
							if (num == 3109671438U)
							{
								if (subSymbol == "bestRoyalTitleIndef")
								{
									resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(GrammarResolverSimple.PawnResolveBestRoyalTitle(pawn), false, false);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "bestRoyalTitle")
						{
							resolvedStr = GrammarResolverSimple.PawnResolveBestRoyalTitle(pawn);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 3124331847U)
					{
						if (num == 3246736155U)
						{
							if (subSymbol == "age_numCase")
							{
								resolvedStr = GrammarResolverSimple.ResolveNumCase(pawn.ageTracker.AgeBiologicalYears.ToString(), symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "bestRoyalTitleDef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(GrammarResolverSimple.PawnResolveBestRoyalTitle(pawn), false, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 3866324033U)
				{
					if (num <= 3638871208U)
					{
						if (num <= 3317904369U)
						{
							if (num != 3276274232U)
							{
								if (num == 3317904369U)
								{
									if (subSymbol == "humanlike")
									{
										resolvedStr = GrammarResolverSimple.ResolveHumanlikeSymbol(pawn.RaceProps.Humanlike, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "chronologicalAge")
							{
								resolvedStr = pawn.ageTracker.AgeChronologicalYears.ToString();
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num != 3444987233U)
						{
							if (num == 3638871208U)
							{
								if (subSymbol == "kindBaseIndef")
								{
									resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.kindDef.label, false, false);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "ageFull")
						{
							resolvedStr = pawn.ageTracker.AgeNumberString;
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 3651983315U)
					{
						if (num != 3641958979U)
						{
							if (num == 3651983315U)
							{
								if (subSymbol == "factionPawnSingularDef")
								{
									resolvedStr = ((pawn.Faction != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.Faction.def.pawnSingular, false, false) : "");
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "kind")
						{
							resolvedStr = pawn.KindLabel;
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 3802171214U)
					{
						if (num == 3866324033U)
						{
							if (subSymbol == "titleIndef")
							{
								resolvedStr = ((pawn.story != null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.story.Title, GrammarResolverSimple.ResolveGender(pawn.story.Title, pawn.gender), false, false) : "");
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "kindBase")
					{
						resolvedStr = GenLabel.BestKindLabel(pawn.kindDef, pawn.gender, false, -1);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 4044507857U)
				{
					if (num <= 3976846386U)
					{
						if (num != 3868512966U)
						{
							if (num == 3976846386U)
							{
								if (subSymbol == "kindDef")
								{
									resolvedStr = pawn.KindLabelDefinite();
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "raceIndef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.def.label, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 3996112312U)
					{
						if (num == 4044507857U)
						{
							if (subSymbol == "royalTitleInCurrentFactionDef")
							{
								resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(GrammarResolverSimple.PawnResolveRoyalTitleInCurrentFaction(pawn), false, false);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "factionPawnsPlural")
					{
						resolvedStr = ((pawn.Faction != null) ? pawn.Faction.def.pawnsPlural : "");
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 4062297208U)
				{
					if (num != 4059209310U)
					{
						if (num == 4062297208U)
						{
							if (subSymbol == "pronoun")
							{
								resolvedStr = pawn.gender.GetPronoun();
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "kindBasePluralIndef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawn.kindDef.GetLabelPlural(-1), pawn.kindDef.label, Gender.Male), true, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num != 4137097213U)
				{
					if (num == 4201427756U)
					{
						if (subSymbol == "royalTitleInCurrentFactionIndef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(GrammarResolverSimple.PawnResolveRoyalTitleInCurrentFaction(pawn), false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
				}
				else if (subSymbol == "label")
				{
					resolvedStr = pawn.LabelNoCountColored;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				resolvedStr = "";
				return false;
			}
			Thing thing = obj as Thing;
			if (thing != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
				if (num <= 2166136261U)
				{
					if (num <= 1291906365U)
					{
						if (num <= 619309234U)
						{
							if (num != 418492385U)
							{
								if (num == 619309234U)
								{
									if (subSymbol == "labelNoParenthesis")
									{
										resolvedStr = thing.LabelNoParenthesis;
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "definite")
							{
								resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(thing.Label, false, false);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num != 1162320608U)
						{
							if (num != 1277025515U)
							{
								if (num == 1291906365U)
								{
									if (subSymbol == "labelShortIndef")
									{
										resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.LabelShort, false, false);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "possessive")
							{
								resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null, Gender.Male).GetPossessive();
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (subSymbol == "factionName")
						{
							resolvedStr = ((thing.Faction != null) ? thing.Faction.Name : "");
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 1587320192U)
					{
						if (num != 1339988243U)
						{
							if (num == 1587320192U)
							{
								if (subSymbol == "gender")
								{
									resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null, Gender.Male), false, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "labelPluralIndef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), thing.LabelNoCount, Gender.Male), true, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 1911534845U)
					{
						if (num != 2084067798U)
						{
							if (num == 2166136261U)
							{
								if (subSymbol != null)
								{
									if (subSymbol.Length == 0)
									{
										resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.Label, false, false);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
						}
						else if (subSymbol == "labelPluralDef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), thing.LabelNoCount, Gender.Male), true, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol == "labelShort")
					{
						resolvedStr = thing.LabelShort;
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 3677077177U)
				{
					if (num <= 2597670950U)
					{
						if (num != 2306218066U)
						{
							if (num == 2597670950U)
							{
								if (subSymbol == "quality")
								{
									QualityCategory cat;
									resolvedStr = (thing.TryGetQuality(out cat) ? cat.GetLabel() : "");
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "indefinite")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.Label, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 2618666040U)
					{
						if (num != 3548544003U)
						{
							if (num == 3677077177U)
							{
								if (subSymbol == "labelNoParenthesisDef")
								{
									resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(thing.LabelNoParenthesis, false, false);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "labelCap")
						{
							resolvedStr = thing.LabelCap;
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol == "objective")
					{
						resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null, Gender.Male).GetObjective();
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 4137097213U)
				{
					if (num != 4062297208U)
					{
						if (num == 4137097213U)
						{
							if (subSymbol == "label")
							{
								resolvedStr = thing.Label;
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "pronoun")
					{
						resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(thing.LabelNoCount, null, Gender.Male).GetPronoun();
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num != 4246959508U)
				{
					if (num != 4252169255U)
					{
						if (num == 4284439316U)
						{
							if (subSymbol == "labelNoParenthesisIndef")
							{
								resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.LabelNoParenthesis, false, false);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "labelPlural")
					{
						resolvedStr = Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (subSymbol == "labelShortDef")
				{
					resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(thing.LabelShort, false, false);
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				resolvedStr = "";
				return false;
			}
			Hediff hediff = obj as Hediff;
			if (hediff != null)
			{
				if (subSymbol != null && subSymbol.Length == 0)
				{
					resolvedStr = hediff.Label;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				if (subSymbol == "label")
				{
					resolvedStr = hediff.Label;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				if (subSymbol == "labelNoun")
				{
					resolvedStr = ((!hediff.def.labelNoun.NullOrEmpty()) ? hediff.def.labelNoun : hediff.Label);
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
			}
			WorldObject worldObject = obj as WorldObject;
			if (worldObject != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
				if (num <= 2084067798U)
				{
					if (num <= 1277025515U)
					{
						if (num != 418492385U)
						{
							if (num != 1162320608U)
							{
								if (num == 1277025515U)
								{
									if (subSymbol == "possessive")
									{
										resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null, Gender.Male).GetPossessive();
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "factionName")
							{
								resolvedStr = ((worldObject.Faction != null) ? worldObject.Faction.Name.ApplyTag(worldObject.Faction) : "");
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (subSymbol == "definite")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(worldObject.Label, false, worldObject.HasName);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 1339988243U)
					{
						if (num != 1587320192U)
						{
							if (num == 2084067798U)
							{
								if (subSymbol == "labelPluralDef")
								{
									resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), worldObject.Label, Gender.Male), true, worldObject.HasName);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "gender")
						{
							resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null, Gender.Male), false, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol == "labelPluralIndef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1), worldObject.Label, Gender.Male), true, worldObject.HasName);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 2618666040U)
				{
					if (num != 2166136261U)
					{
						if (num != 2306218066U)
						{
							if (num == 2618666040U)
							{
								if (subSymbol == "objective")
								{
									resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null, Gender.Male).GetObjective();
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "indefinite")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(worldObject.Label, false, worldObject.HasName);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol != null)
					{
						if (subSymbol.Length == 0)
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(worldObject.Label, false, worldObject.HasName);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
				}
				else if (num <= 4062297208U)
				{
					if (num != 3548544003U)
					{
						if (num == 4062297208U)
						{
							if (subSymbol == "pronoun")
							{
								resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(worldObject.Label, null, Gender.Male).GetPronoun();
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "labelCap")
					{
						resolvedStr = worldObject.LabelCap;
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num != 4137097213U)
				{
					if (num == 4252169255U)
					{
						if (subSymbol == "labelPlural")
						{
							resolvedStr = Find.ActiveLanguageWorker.Pluralize(worldObject.Label, -1);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
				}
				else if (subSymbol == "label")
				{
					resolvedStr = worldObject.Label;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				resolvedStr = "";
				return false;
			}
			Faction faction = obj as Faction;
			if (faction != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
				if (num <= 2307658270U)
				{
					if (num <= 2028987726U)
					{
						if (num != 493124349U)
						{
							if (num != 1812998298U)
							{
								if (num == 2028987726U)
								{
									if (subSymbol == "pawnSingular")
									{
										resolvedStr = faction.def.pawnSingular;
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "royalFavorLabel")
							{
								resolvedStr = faction.def.royalFavorLabel;
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (subSymbol == "pawnsPluralDef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular, Gender.Male), true, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 2082817202U)
					{
						if (num != 2166136261U)
						{
							if (num == 2307658270U)
							{
								if (subSymbol == "leaderNameDef")
								{
									resolvedStr = ((faction.leader != null && faction.leader.Name != null) ? Find.ActiveLanguageWorker.WithDefiniteArticle(faction.leader.Name.ToStringShort, faction.leader.gender, false, true).ApplyTag(TagType.Name, null) : "");
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol != null)
						{
							if (subSymbol.Length == 0)
							{
								resolvedStr = faction.Name.ApplyTag(faction);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "leaderPossessive")
					{
						resolvedStr = ((faction.leader != null) ? faction.leader.gender.GetPossessive() : "");
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 2654955287U)
				{
					if (num != 2369371622U)
					{
						if (num != 2461125861U)
						{
							if (num == 2654955287U)
							{
								if (subSymbol == "leaderPronoun")
								{
									resolvedStr = ((faction.leader != null) ? faction.leader.gender.GetPronoun() : "");
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "pawnSingularDef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnSingular, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol == "name")
					{
						resolvedStr = faction.Name.ApplyTag(faction);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 2892717736U)
				{
					if (num != 2873559712U)
					{
						if (num == 2892717736U)
						{
							if (subSymbol == "pawnSingularIndef")
							{
								resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnSingular, false, false);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "pawnsPluralIndef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular, Gender.Male), true, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num != 2965909334U)
				{
					if (num == 3562162903U)
					{
						if (subSymbol == "leaderObjective")
						{
							resolvedStr = ((faction.leader != null) ? faction.leader.gender.GetObjective() : "");
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
				}
				else if (subSymbol == "pawnsPlural")
				{
					resolvedStr = faction.def.pawnsPlural;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				resolvedStr = "";
				return false;
			}
			Ideo ideo = obj as Ideo;
			if (ideo != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
				if (num <= 2166136261U)
				{
					if (num != 316778831U)
					{
						if (num != 2003836726U)
						{
							if (num == 2166136261U)
							{
								if (subSymbol != null)
								{
									if (subSymbol.Length == 0)
									{
										resolvedStr = ideo.name.ApplyTag(ideo);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
						}
						else if (subSymbol == "memberNameIndef")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(ideo.memberName, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol == "memberNameDef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(ideo.memberName, false, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 2766658070U)
				{
					if (num != 2369371622U)
					{
						if (num == 2766658070U)
						{
							if (subSymbol == "memberNamePlural")
							{
								resolvedStr = ideo.MemberNamePlural;
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "name")
					{
						resolvedStr = ideo.name.ApplyTag(ideo);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num != 2821535572U)
				{
					if (num == 4094101544U)
					{
						if (subSymbol == "memberName")
						{
							resolvedStr = ideo.memberName;
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
				}
				else if (subSymbol == "adjective")
				{
					resolvedStr = ideo.adjective;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				resolvedStr = "";
				return false;
			}
			Precept precept = obj as Precept;
			if (precept != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
				if (num <= 2369371622U)
				{
					if (num <= 1509316157U)
					{
						if (num != 898486002U)
						{
							if (num != 1509316157U)
							{
								goto IL_2A81;
							}
							if (!(subSymbol == "labelIndef"))
							{
								goto IL_2A81;
							}
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(precept.Label, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
						else
						{
							if (!(subSymbol == "labelCapDef"))
							{
								goto IL_2A81;
							}
							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(precept.Label, false, !precept.usesDefiniteArticle).CapitalizeFirst();
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 2166136261U)
					{
						if (num != 2369371622U)
						{
							goto IL_2A81;
						}
						if (!(subSymbol == "name"))
						{
							goto IL_2A81;
						}
					}
					else
					{
						if (subSymbol == null)
						{
							goto IL_2A81;
						}
						if (subSymbol.Length != 0)
						{
							goto IL_2A81;
						}
					}
				}
				else if (num <= 3548544003U)
				{
					if (num != 3000616903U)
					{
						if (num != 3548544003U)
						{
							goto IL_2A81;
						}
						if (!(subSymbol == "labelCap"))
						{
							goto IL_2A81;
						}
						resolvedStr = precept.LabelCap;
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
					else
					{
						if (!(subSymbol == "labelCapIndef"))
						{
							goto IL_2A81;
						}
						resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(precept.Label, false, false).CapitalizeFirst();
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num != 3597039252U)
				{
					if (num != 4137097213U)
					{
						goto IL_2A81;
					}
					if (!(subSymbol == "label"))
					{
						goto IL_2A81;
					}
				}
				else
				{
					if (!(subSymbol == "labelDef"))
					{
						goto IL_2A81;
					}
					resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(precept.Label, false, !precept.usesDefiniteArticle);
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				resolvedStr = precept.Label;
				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
				return true;
				IL_2A81:
				resolvedStr = "";
				return false;
			}
			Def def = obj as Def;
			if (def != null)
			{
				PawnKindDef pawnKindDef = def as PawnKindDef;
				if (pawnKindDef != null)
				{
					if (subSymbol == "labelPlural")
					{
						resolvedStr = pawnKindDef.GetLabelPlural(-1);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
					if (subSymbol == "labelPluralDef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(pawnKindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawnKindDef.GetLabelPlural(-1), pawnKindDef.label, Gender.Male), true, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
					if (subSymbol == "labelPluralIndef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawnKindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawnKindDef.GetLabelPlural(-1), pawnKindDef.label, Gender.Male), true, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
				if (num <= 2084067798U)
				{
					if (num <= 1277025515U)
					{
						if (num != 418492385U)
						{
							if (num == 1277025515U)
							{
								if (subSymbol == "possessive")
								{
									resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(def.label, null, Gender.Male).GetPossessive();
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "definite")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(def.label, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 1339988243U)
					{
						if (num != 1587320192U)
						{
							if (num == 2084067798U)
							{
								if (subSymbol == "labelPluralDef")
								{
									resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(def.label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(def.label, -1), def.label, Gender.Male), true, false);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "gender")
						{
							resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(def.label, null, Gender.Male), false, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol == "labelPluralIndef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(def.label, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(def.label, -1), def.label, Gender.Male), true, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (num <= 2618666040U)
				{
					if (num != 2166136261U)
					{
						if (num != 2306218066U)
						{
							if (num == 2618666040U)
							{
								if (subSymbol == "objective")
								{
									resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(def.label, null, Gender.Male).GetObjective();
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "indefinite")
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol != null)
					{
						if (subSymbol.Length == 0)
						{
							resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label, false, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
				}
				else if (num != 4062297208U)
				{
					if (num != 4137097213U)
					{
						if (num == 4252169255U)
						{
							if (subSymbol == "labelPlural")
							{
								resolvedStr = Find.ActiveLanguageWorker.Pluralize(def.label, -1);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "label")
					{
						resolvedStr = def.label;
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
				}
				else if (subSymbol == "pronoun")
				{
					resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(def.label, null, Gender.Male).GetPronoun();
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				resolvedStr = "";
				return false;
			}
			RoyalTitle royalTitle = obj as RoyalTitle;
			if (royalTitle != null)
			{
				if (subSymbol != null && subSymbol.Length == 0)
				{
					resolvedStr = royalTitle.Label;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				if (subSymbol == "label")
				{
					resolvedStr = royalTitle.Label;
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					return true;
				}
				if (!(subSymbol == "indefinite"))
				{
					resolvedStr = "";
					return false;
				}
				resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(royalTitle.Label, false, false);
				GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
				return true;
			}
			else
			{
				string text = obj as string;
				if (text != null)
				{
					uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
					if (num <= 2306218066U)
					{
						if (num <= 1277025515U)
						{
							if (num != 418492385U)
							{
								if (num != 686961615U)
								{
									if (num == 1277025515U)
									{
										if (subSymbol == "possessive")
										{
											resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(text, null, Gender.Male).GetPossessive();
											GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
											return true;
										}
									}
								}
								else if (subSymbol == "plural")
								{
									resolvedStr = Find.ActiveLanguageWorker.Pluralize(text, -1);
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
							else if (subSymbol == "definite")
							{
								resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(text, false, false);
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (num != 1587320192U)
						{
							if (num != 2166136261U)
							{
								if (num == 2306218066U)
								{
									if (subSymbol == "indefinite")
									{
										resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(text, false, false);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol != null)
							{
								if (subSymbol.Length == 0)
								{
									resolvedStr = text;
									GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "gender")
						{
							resolvedStr = GrammarResolverSimple.ResolveGenderSymbol(LanguageDatabase.activeLanguage.ResolveGender(text, null, Gender.Male), false, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 3774422699U)
					{
						if (num != 2618666040U)
						{
							if (num != 2704835779U)
							{
								if (num == 3774422699U)
								{
									if (subSymbol == "pluralIndef")
									{
										resolvedStr = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(text, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(text, -1), text, Gender.Male), true, false);
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "replace")
							{
								resolvedStr = GrammarResolverSimple.ResolveReplace(text, symbolArgs);
								return true;
							}
						}
						else if (subSymbol == "objective")
						{
							resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(text, null, Gender.Male).GetObjective();
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 3784914766U)
					{
						if (num != 4062297208U)
						{
							if (num == 4118121487U)
							{
								if (subSymbol == "numCase")
								{
									resolvedStr = GrammarResolverSimple.ResolveNumCase(text, symbolArgs, fullStringForReference);
									return true;
								}
							}
						}
						else if (subSymbol == "pronoun")
						{
							resolvedStr = LanguageDatabase.activeLanguage.ResolveGender(text, null, Gender.Male).GetPronoun();
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (subSymbol == "pluralDef")
					{
						resolvedStr = Find.ActiveLanguageWorker.WithDefiniteArticle(Find.ActiveLanguageWorker.Pluralize(text, -1), LanguageDatabase.activeLanguage.ResolveGender(Find.ActiveLanguageWorker.Pluralize(text, -1), text, Gender.Male), true, false);
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
					resolvedStr = "";
					return false;
				}
				if (obj is int || obj is long || obj is float)
				{
					int num2 = (obj is int) ? ((int)obj) : ((obj is float) ? ((int)((float)obj)) : ((int)((long)obj)));
					float f = (obj as float?) ?? ((float)num2);
					uint num = <PrivateImplementationDetails>.ComputeStringHash(subSymbol);
					if (num <= 1146255554U)
					{
						if (num != 328483777U)
						{
							if (num != 437054748U)
							{
								if (num == 1146255554U)
								{
									if (subSymbol == "ordinal")
									{
										resolvedStr = Find.ActiveLanguageWorker.OrdinalNumber(num2, Gender.None).ToString();
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
							else if (subSymbol == "percentageEmptyZero")
							{
								resolvedStr = f.ToStringPercentEmptyZero("F0");
								GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
								return true;
							}
						}
						else if (subSymbol == "multiple")
						{
							resolvedStr = GrammarResolverSimple.ResolveMultipleSymbol(num2, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num <= 2166136261U)
					{
						if (num != 1564253156U)
						{
							if (num == 2166136261U)
							{
								if (subSymbol != null)
								{
									if (subSymbol.Length == 0)
									{
										resolvedStr = num2.ToString();
										GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
										return true;
									}
								}
							}
						}
						else if (subSymbol == "time")
						{
							resolvedStr = num2.ToStringTicksToPeriod(true, false, true, true, false);
							GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
							return true;
						}
					}
					else if (num != 2538831285U)
					{
						if (num == 4118121487U)
						{
							if (subSymbol == "numCase")
							{
								resolvedStr = GrammarResolverSimple.ResolveNumCase(num2.ToString(), symbolArgs, fullStringForReference);
								return true;
							}
						}
					}
					else if (subSymbol == "percentage")
					{
						resolvedStr = f.ToStringPercent();
						GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
						return true;
					}
					resolvedStr = "";
					return false;
				}
				if (obj is TaggedString)
				{
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					resolvedStr = ((TaggedString)obj).RawText;
				}
				if (subSymbol.NullOrEmpty())
				{
					GrammarResolverSimple.EnsureNoArgs(subSymbol, symbolArgs, fullStringForReference);
					if (obj == null)
					{
						resolvedStr = "";
					}
					else
					{
						resolvedStr = obj.ToString();
					}
					return true;
				}
				resolvedStr = "";
				return false;
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0006C618 File Offset: 0x0006A818
		private static void EnsureNoArgs(string subSymbol, string symbolArgs, string fullStringForReference)
		{
			if (!symbolArgs.NullOrEmpty())
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Symbol \"",
					subSymbol,
					"\" doesn't expect any args but \"",
					symbolArgs,
					"\" args were provided. Full string: \"",
					fullStringForReference,
					"\"."
				}), subSymbol.GetHashCode() ^ symbolArgs.GetHashCode() ^ fullStringForReference.GetHashCode() ^ 958090126);
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0006C684 File Offset: 0x0006A884
		public static string ResolveGenderSymbol(Gender gender, bool animal, string args, string fullStringForReference)
		{
			if (args.NullOrEmpty())
			{
				return gender.GetLabel(animal);
			}
			int argsCount = GrammarResolverSimple.GetArgsCount(args, ':');
			if (argsCount == 2)
			{
				switch (gender)
				{
				case Gender.None:
					return GrammarResolverSimple.GetArg(args, 0, ':');
				case Gender.Male:
					return GrammarResolverSimple.GetArg(args, 0, ':');
				case Gender.Female:
					return GrammarResolverSimple.GetArg(args, 1, ':');
				default:
					return "";
				}
			}
			else
			{
				if (argsCount != 3)
				{
					Log.ErrorOnce("Invalid args count in \"" + fullStringForReference + "\" for symbol \"gender\".", args.GetHashCode() ^ fullStringForReference.GetHashCode() ^ 787618371);
					return "";
				}
				switch (gender)
				{
				case Gender.None:
					return GrammarResolverSimple.GetArg(args, 2, ':');
				case Gender.Male:
					return GrammarResolverSimple.GetArg(args, 0, ':');
				case Gender.Female:
					return GrammarResolverSimple.GetArg(args, 1, ':');
				default:
					return "";
				}
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0006C750 File Offset: 0x0006A950
		private static string ResolveHumanlikeSymbol(bool humanlike, string args, string fullStringForReference)
		{
			if (GrammarResolverSimple.GetArgsCount(args, ':') != 2)
			{
				Log.ErrorOnce("Invalid args count in \"" + fullStringForReference + "\" for symbol \"humanlike\".", args.GetHashCode() ^ fullStringForReference.GetHashCode() ^ 895109845);
				return "";
			}
			if (humanlike)
			{
				return GrammarResolverSimple.GetArg(args, 0, ':');
			}
			return GrammarResolverSimple.GetArg(args, 1, ':');
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0006C7AC File Offset: 0x0006A9AC
		private static string ResolveMultipleSymbol(int count, string args, string fullStringForReference)
		{
			if (GrammarResolverSimple.GetArgsCount(args, ':') != 2)
			{
				Log.ErrorOnce("Invalid args count in \"" + fullStringForReference + "\" for symbol \"multiple\".", args.GetHashCode() ^ fullStringForReference.GetHashCode() ^ 231251341);
				return "";
			}
			if (count > 1)
			{
				return GrammarResolverSimple.GetArg(args, 0, ':');
			}
			return GrammarResolverSimple.GetArg(args, 1, ':');
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0006C809 File Offset: 0x0006AA09
		public static Gender ResolveGender(string word, Gender defaultGender)
		{
			return LanguageDatabase.activeLanguage.ResolveGender(word, null, defaultGender);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0006C818 File Offset: 0x0006AA18
		public static int GetArgsCount(string args, char delimiter = ':')
		{
			int num = 1;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == delimiter)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0006C848 File Offset: 0x0006AA48
		public static string GetArg(string args, int argIndex, char delimiter = ':')
		{
			GrammarResolverSimple.tmpArg.Length = 0;
			int num = 0;
			foreach (char c in args)
			{
				if (c == delimiter)
				{
					num++;
				}
				else if (num == argIndex)
				{
					GrammarResolverSimple.tmpArg.Append(c);
				}
				else if (num > argIndex)
				{
					IL_55:
					while (GrammarResolverSimple.tmpArg.Length != 0)
					{
						if (GrammarResolverSimple.tmpArg[0] != ' ')
						{
							break;
						}
						GrammarResolverSimple.tmpArg.Remove(0, 1);
					}
					while (GrammarResolverSimple.tmpArg.Length != 0 && GrammarResolverSimple.tmpArg[GrammarResolverSimple.tmpArg.Length - 1] == ' ')
					{
						StringBuilder stringBuilder = GrammarResolverSimple.tmpArg;
						int length = stringBuilder.Length;
						stringBuilder.Length = length - 1;
					}
					return GrammarResolverSimple.tmpArg.ToString();
				}
			}
			goto IL_55;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0006C90C File Offset: 0x0006AB0C
		public static string PawnResolveBestRoyalTitle(Pawn pawn)
		{
			if (pawn.royalty == null)
			{
				return "";
			}
			RoyalTitle royalTitle = null;
			foreach (RoyalTitle royalTitle2 in from x in pawn.royalty.AllTitlesForReading
			orderby x.def.index
			select x)
			{
				if (royalTitle == null || royalTitle2.def.favorCost > royalTitle.def.favorCost)
				{
					royalTitle = royalTitle2;
				}
			}
			if (royalTitle == null)
			{
				return "";
			}
			return royalTitle.def.GetLabelFor(pawn.gender);
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0006C9C4 File Offset: 0x0006ABC4
		public static string PawnResolveRoyalTitleInCurrentFaction(Pawn pawn)
		{
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in from x in pawn.royalty.AllTitlesForReading
				orderby x.def.index
				select x)
				{
					if (royalTitle.faction == pawn.Faction)
					{
						return royalTitle.def.GetLabelFor(pawn.gender);
					}
				}
			}
			return "";
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0006CA64 File Offset: 0x0006AC64
		public static string ResolveNumCase(string number, string args, string fullStringForReference)
		{
			LanguageWorker activeLanguageWorker = Find.ActiveLanguageWorker;
			int num = LanguageDatabase.activeLanguage.info.totalNumCaseCount ?? activeLanguageWorker.TotalNumCaseCount;
			if (GrammarResolverSimple.GetArgsCount(args, ':') != num)
			{
				Log.Error("Invalid argument count for _numCase, expected " + num.ToString() + " arguments. Full string: " + fullStringForReference);
				return "";
			}
			GrammarResolverSimple.numCaseArgs.Clear();
			for (int i = 0; i < num; i++)
			{
				GrammarResolverSimple.numCaseArgs.Add(GrammarResolverSimple.GetArg(args, i, ':'));
			}
			float number2;
			if (float.TryParse(number, out number2))
			{
				return activeLanguageWorker.ResolveNumCase(number2, GrammarResolverSimple.numCaseArgs);
			}
			return "";
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0006CB18 File Offset: 0x0006AD18
		public static string ResolveReplace(string symbol, string args)
		{
			LanguageWorker activeLanguageWorker = Find.ActiveLanguageWorker;
			int argsCount = GrammarResolverSimple.GetArgsCount(args, ':');
			GrammarResolverSimple.replaceArgs.Clear();
			GrammarResolverSimple.replaceArgs.Add(symbol);
			for (int i = 0; i < argsCount; i++)
			{
				GrammarResolverSimple.replaceArgs.Add(GrammarResolverSimple.GetArg(args, i, ':'));
			}
			return activeLanguageWorker.ResolveReplace(GrammarResolverSimple.replaceArgs);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0006CB74 File Offset: 0x0006AD74
		public static string ResolveFunction(string name, string args, string fullStringForReference)
		{
			GrammarResolverSimple.functionArgs.Clear();
			int argsCount = GrammarResolverSimple.GetArgsCount(args, ';');
			for (int i = 0; i < argsCount; i++)
			{
				GrammarResolverSimple.functionArgs.Add(GrammarResolverSimple.GetArg(args, i, ';'));
			}
			return Find.ActiveLanguageWorker.ResolveFunction(name, GrammarResolverSimple.functionArgs, fullStringForReference);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0006CBC4 File Offset: 0x0006ADC4
		public static List<string> TryParseNumCase(string str)
		{
			int num = str.IndexOf("{0_numCase", StringComparison.Ordinal);
			if (num != -1)
			{
				int i = num + 10;
				bool flag = false;
				bool flag2 = false;
				string text = "";
				while (i < str.Length)
				{
					if (str[i] == '?')
					{
						flag = true;
					}
					else
					{
						if (str[i] == '}')
						{
							flag2 = true;
							break;
						}
						if (flag)
						{
							text += str[i].ToString();
						}
					}
					i++;
				}
				if (!flag2)
				{
					return null;
				}
				int argsCount = GrammarResolverSimple.GetArgsCount(text, ':');
				if (argsCount > 0)
				{
					List<string> list = new List<string>();
					for (int j = 0; j < argsCount; j++)
					{
						list.Add(GrammarResolverSimple.GetArg(text, j, ':'));
					}
					return list;
				}
			}
			return null;
		}

		// Token: 0x04001464 RID: 5220
		private static bool formatterWorking;

		// Token: 0x04001465 RID: 5221
		private static bool symbolParserWorking;

		// Token: 0x04001466 RID: 5222
		private static StringBuilder tmpResultBuffer = new StringBuilder();

		// Token: 0x04001467 RID: 5223
		private static StringBuilder tmpSymbolBuffer = new StringBuilder();

		// Token: 0x04001468 RID: 5224
		private static StringBuilder tmpSymbolBuffer_objectLabel = new StringBuilder();

		// Token: 0x04001469 RID: 5225
		private static StringBuilder tmpSymbolBuffer_subSymbol = new StringBuilder();

		// Token: 0x0400146A RID: 5226
		private static StringBuilder tmpSymbolBuffer_function = new StringBuilder();

		// Token: 0x0400146B RID: 5227
		private static StringBuilder tmpSymbolBuffer_args = new StringBuilder();

		// Token: 0x0400146C RID: 5228
		private static StringBuilder tmpSymbolBuffer_functionArgs = new StringBuilder();

		// Token: 0x0400146D RID: 5229
		private static List<string> tmpArgsLabels = new List<string>();

		// Token: 0x0400146E RID: 5230
		private static List<object> tmpArgsObjects = new List<object>();

		// Token: 0x0400146F RID: 5231
		private static StringBuilder tmpArg = new StringBuilder();

		// Token: 0x04001470 RID: 5232
		private static List<string> numCaseArgs = new List<string>();

		// Token: 0x04001471 RID: 5233
		private static List<string> replaceArgs = new List<string>();

		// Token: 0x04001472 RID: 5234
		private static List<string> functionArgs = new List<string>();
	}
}
