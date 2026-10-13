using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020031F1 RID: 12785
	[StaticConstructorOnStartup]
	public static class CharacterCardUtility
	{
		// Token: 0x17002EB5 RID: 11957
		// (get) Token: 0x06011FBC RID: 73660 RVA: 0x0053C468 File Offset: 0x0053A668
		private static List<CustomXenotype> CustomXenotypes
		{
			get
			{
				if (CharacterCardUtility.cachedCustomXenotypes == null)
				{
					CharacterCardUtility.cachedCustomXenotypes = new List<CustomXenotype>();
					foreach (FileInfo fileInfo in from f in GenFilePaths.AllCustomXenotypeFiles
					orderby f.LastWriteTime
					select f)
					{
						string filePath = GenFilePaths.AbsFilePathForXenotype(Path.GetFileNameWithoutExtension(fileInfo.Name));
						PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, delegate
						{
							CustomXenotype item;
							if (GameDataSaveLoader.TryLoadXenotype(filePath, out item))
							{
								CharacterCardUtility.cachedCustomXenotypes.Add(item);
							}
						}, true);
					}
				}
				return CharacterCardUtility.cachedCustomXenotypes;
			}
		}

		// Token: 0x17002EB6 RID: 11958
		// (get) Token: 0x06011FBD RID: 73661 RVA: 0x0053C520 File Offset: 0x0053A720
		public static List<CustomXenotype> CustomXenotypesForReading
		{
			get
			{
				return CharacterCardUtility.CustomXenotypes;
			}
		}

		// Token: 0x06011FBE RID: 73662 RVA: 0x0053C528 File Offset: 0x0053A728
		public static void DrawCharacterCard(Rect rect, Pawn pawn, Action randomizeCallback = null, Rect creationRect = default(Rect), bool showName = true)
		{
			bool flag = randomizeCallback != null;
			Rect rect2 = flag ? creationRect : rect;
			Widgets.BeginGroup(rect2);
			Rect rect3 = new Rect(0f, 0f, 300f, (float)(showName ? 30 : 0));
			if (showName)
			{
				if (flag)
				{
					NameTriple nameTriple = pawn.Name as NameTriple;
					if (nameTriple != null)
					{
						Rect rect4 = new Rect(rect3);
						rect4.width *= 0.333f;
						Rect rect5 = new Rect(rect3);
						rect5.width *= 0.333f;
						rect5.x += rect5.width;
						Rect rect6 = new Rect(rect3);
						rect6.width *= 0.333f;
						rect6.x += rect5.width * 2f;
						string first = nameTriple.First;
						string nick = nameTriple.Nick;
						string last = nameTriple.Last;
						CharacterCardUtility.DoNameInputRect(rect4, ref first, 12);
						if (nameTriple.Nick == nameTriple.First || nameTriple.Nick == nameTriple.Last)
						{
							GUI.color = new Color(1f, 1f, 1f, 0.5f);
						}
						CharacterCardUtility.DoNameInputRect(rect5, ref nick, 16);
						GUI.color = Color.white;
						CharacterCardUtility.DoNameInputRect(rect6, ref last, 12);
						if (nameTriple.First != first || nameTriple.Nick != nick || nameTriple.Last != last)
						{
							pawn.Name = new NameTriple(first, string.IsNullOrEmpty(nick) ? first : nick, last);
						}
						TooltipHandler.TipRegionByKey(rect4, "FirstNameDesc");
						TooltipHandler.TipRegionByKey(rect5, "ShortIdentifierDesc");
						TooltipHandler.TipRegionByKey(rect6, "LastNameDesc");
						goto IL_292;
					}
				}
				rect3.width = 999f;
				Text.Font = GameFont.Medium;
				string text = pawn.Name.ToStringFull.CapitalizeFirst();
				Widgets.Label(rect3, text);
				if (pawn.guilt != null && pawn.guilt.IsGuilty)
				{
					float x = Text.CalcSize(text).x;
					Rect rect7 = new Rect(x + 10f, 0f, 32f, 32f);
					GUI.DrawTexture(rect7, TexUI.GuiltyTex);
					TooltipHandler.TipRegion(rect7, () => pawn.guilt.Tip, 6321623);
				}
				Text.Font = GameFont.Small;
			}
			IL_292:
			bool allowsChildSelection = ScenarioUtility.AllowsChildSelection(Find.Scenario);
			if (ModsConfig.BiotechActive && flag)
			{
				Widgets.DrawHighlight(CharacterCardUtility.highlightRect.ExpandedBy(6f));
			}
			if (flag)
			{
				Rect rect8 = new Rect(creationRect.width - 200f - 6f, 6f, 200f, rect3.height);
				if (Widgets.ButtonText(rect8, "Randomize".Translate(), true, true, true, null))
				{
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					randomizeCallback();
				}
				UIHighlighter.HighlightOpportunity(rect8, "RandomizePawn");
				if (ModsConfig.BiotechActive)
				{
					CharacterCardUtility.LifestageAndXenotypeOptions(pawn, rect8, flag, allowsChildSelection, randomizeCallback);
				}
			}
			if (flag)
			{
				Widgets.InfoCardButton(rect3.xMax + 4f, (rect3.height - 24f) / 2f, pawn);
			}
			else if (!pawn.health.Dead)
			{
				float num = CharacterCardUtility.PawnCardSize(pawn).x - 85f;
				if (pawn.IsFreeColonist && pawn.Spawned && !pawn.IsQuestLodger() && showName)
				{
					Rect rect9 = new Rect(num, 0f, 30f, 30f);
					if (Mouse.IsOver(rect9))
					{
						TooltipHandler.TipRegion(rect9, PawnBanishUtility.GetBanishButtonTip(pawn));
					}
					if (Widgets.ButtonImage(rect9, TexButton.Banish, true, null))
					{
						if (pawn.Downed)
						{
							Messages.Message("MessageCantBanishDownedPawn".Translate(pawn.LabelShort, pawn).AdjustedFor(pawn, "PAWN", true), pawn, MessageTypeDefOf.RejectInput, false);
						}
						else
						{
							PawnBanishUtility.ShowBanishPawnConfirmationDialog(pawn, null);
						}
					}
					num -= 40f;
				}
				if ((pawn.IsColonist || pawn.IsColonySubhuman || DebugSettings.ShowDevGizmos) && showName)
				{
					Rect rect10 = new Rect(num, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect10, "RenameColonist");
					if (Widgets.ButtonImage(rect10, TexButton.Rename, true, null))
					{
						Find.WindowStack.Add(pawn.NamePawnDialog(null));
					}
					num -= 40f;
				}
				if (pawn.IsFreeColonist && !pawn.IsQuestLodger() && pawn.royalty != null && pawn.royalty.AllTitlesForReading.Count > 0)
				{
					Rect rect11 = new Rect(num, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect11, "RenounceTitle");
					if (Widgets.ButtonImage(rect11, TexButton.RenounceTitle, true, null))
					{
						FloatMenuUtility.MakeMenu<RoyalTitle>(pawn.royalty.AllTitlesForReading, (RoyalTitle title) => "RenounceTitle".Translate() + ": " + "TitleOfFaction".Translate(title.def.GetLabelCapFor(pawn), title.faction.GetCallLabel()), delegate(RoyalTitle title)
						{
							Action <>9__5;
							return delegate()
							{
								List<FactionPermit> list = pawn.royalty.PermitsFromFaction(title.faction);
								List<RoyalTitlePermitDef> list2;
								List<RoyalTitlePermitDef> list3;
								RoyalTitleUtility.FindLostAndGainedPermits(title.def, null, out list2, out list3);
								StringBuilder stringBuilder = new StringBuilder();
								if (list3.Count > 0 || list.Count > 0)
								{
									stringBuilder.AppendLine("RenounceTitleWillLoosePermits".Translate(pawn.Named("PAWN")) + ":");
									foreach (RoyalTitlePermitDef royalTitlePermitDef in list3)
									{
										stringBuilder.AppendLine("- " + royalTitlePermitDef.LabelCap + " (" + base.<DrawCharacterCard>g__FirstTitleWithPermit|4(royalTitlePermitDef).GetLabelFor(pawn) + ")");
									}
									foreach (FactionPermit factionPermit in list)
									{
										stringBuilder.AppendLine("- " + factionPermit.Permit.LabelCap + " (" + factionPermit.Title.GetLabelFor(pawn) + ")");
									}
									stringBuilder.AppendLine();
								}
								int permitPoints = pawn.royalty.GetPermitPoints(title.faction);
								if (permitPoints > 0)
								{
									stringBuilder.AppendLineTagged("RenounceTitleWillLosePermitPoints".Translate(pawn.Named("PAWN"), permitPoints.Named("POINTS"), title.faction.Named("FACTION")));
								}
								if (pawn.abilities.abilities.Any<Ability>())
								{
									stringBuilder.AppendLine();
									stringBuilder.AppendLineTagged("RenounceTitleWillKeepPsylinkLevels".Translate(pawn.Named("PAWN")));
								}
								if (!title.faction.def.renounceTitleMessage.NullOrEmpty())
								{
									stringBuilder.AppendLine();
									stringBuilder.AppendLine(title.faction.def.renounceTitleMessage);
								}
								WindowStack windowStack = Find.WindowStack;
								TaggedString text2 = "RenounceTitleDescription".Translate(pawn.Named("PAWN"), "TitleOfFaction".Translate(title.def.GetLabelCapFor(pawn), title.faction.GetCallLabel()).Named("TITLE"), stringBuilder.ToString().TrimEndNewlines().Named("EFFECTS"));
								Action confirmedAct;
								if ((confirmedAct = <>9__5) == null)
								{
									confirmedAct = (<>9__5 = delegate()
									{
										pawn.royalty.SetTitle(title.faction, null, false, false, true);
										pawn.royalty.ResetPermitsAndPoints(title.faction, title.def);
									});
								}
								windowStack.Add(Dialog_MessageBox.CreateConfirmation(text2, confirmedAct, true, null, WindowLayer.Dialog));
							};
						});
					}
					num -= 40f;
				}
				if (pawn.guilt != null && pawn.guilt.IsGuilty && pawn.IsFreeColonist && !pawn.IsQuestLodger())
				{
					Rect rect12 = new Rect(num + 5f, 0f, 30f, 30f);
					TooltipHandler.TipRegionByKey(rect12, "ExecuteColonist");
					if (Widgets.ButtonImage(rect12, TexButton.ExecuteColonist, true, null))
					{
						pawn.guilt.awaitingExecution = !pawn.guilt.awaitingExecution;
						if (pawn.guilt.awaitingExecution)
						{
							Messages.Message("MessageColonistMarkedForExecution".Translate(pawn), pawn, MessageTypeDefOf.SilentInput, false);
						}
					}
					if (pawn.guilt.awaitingExecution)
					{
						Rect position = default(Rect);
						position.x += rect12.x + 22f;
						position.width = 15f;
						position.height = 15f;
						GUI.DrawTexture(position, Widgets.CheckboxOnTex);
					}
				}
			}
			float num2 = rect3.height + 10f;
			float num3 = num2;
			num2 = CharacterCardUtility.DoTopStack(pawn, rect, flag, num2);
			if (num2 - num3 < 78f)
			{
				num2 += 15f;
			}
			Rect leftRect = new Rect(0f, num2, 250f, rect2.height - num2);
			CharacterCardUtility.DoLeftSection(rect, leftRect, pawn);
			Rect rect13 = new Rect(leftRect.xMax, num2, 258f, rect2.height - num2);
			Widgets.BeginGroup(rect13);
			SkillUI.SkillDrawMode mode;
			if (Current.ProgramState == ProgramState.Playing)
			{
				mode = SkillUI.SkillDrawMode.Gameplay;
			}
			else
			{
				mode = SkillUI.SkillDrawMode.Menu;
			}
			SkillUI.DrawSkillsOf(pawn, Vector2.zero, mode, rect13);
			Widgets.EndGroup();
			Widgets.EndGroup();
		}

		// Token: 0x06011FBF RID: 73663 RVA: 0x0053CCE8 File Offset: 0x0053AEE8
		private static string GetTitleTipString(Pawn pawn, Faction faction, RoyalTitle title, int favor)
		{
			RoyalTitleDef def = title.def;
			TaggedString t = "RoyalTitleTooltipHasTitle".Translate(pawn.Named("PAWN"), faction.Named("FACTION"), def.GetLabelCapFor(pawn).Named("TITLE"));
			t += "\n\n" + faction.def.royalFavorLabel.CapitalizeFirst() + ": " + favor.ToString();
			RoyalTitleDef nextTitle = def.GetNextTitle(faction);
			if (nextTitle != null)
			{
				t += "\n" + "RoyalTitleTooltipNextTitle".Translate() + ": " + nextTitle.GetLabelCapFor(pawn) + " (" + "RoyalTitleTooltipNextTitleFavorCost".Translate(nextTitle.favorCost.ToString(), faction.Named("FACTION")) + ")";
			}
			else
			{
				t += "\n" + "RoyalTitleTooltipFinalTitle".Translate();
			}
			if (title.def.canBeInherited)
			{
				Pawn heir = pawn.royalty.GetHeir(faction);
				if (heir != null)
				{
					t += "\n\n" + "RoyalTitleTooltipInheritance".Translate(pawn.Named("PAWN"), heir.Named("HEIR"));
					if (heir.Faction == null)
					{
						t += " " + "RoyalTitleTooltipHeirNoFaction".Translate(heir.Named("HEIR"));
					}
					else if (heir.Faction != faction)
					{
						t += " " + "RoyalTitleTooltipHeirDifferentFaction".Translate(heir.Named("HEIR"), heir.Faction.Named("FACTION"));
					}
				}
				else
				{
					t += "\n\n" + "RoyalTitleTooltipNoHeir".Translate(pawn.Named("PAWN"));
				}
			}
			else
			{
				t += "\n\n" + "LetterRoyalTitleCantBeInherited".Translate(title.def.Named("TITLE")).CapitalizeFirst() + " " + "LetterRoyalTitleNoHeir".Translate(pawn.Named("PAWN"));
			}
			t += "\n\n" + (title.conceited ? "RoyalTitleTooltipConceited" : "RoyalTitleTooltipNonConceited").Translate(pawn.Named("PAWN"));
			t += "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, pawn);
			return (t + ("\n\n" + "ClickToLearnMore".Translate().Colorize(ColoredText.SubtleGrayColor))).Resolve();
		}

		// Token: 0x06011FC0 RID: 73664 RVA: 0x0053CFBC File Offset: 0x0053B1BC
		private static List<object> GetWorkTypeDisableCauses(Pawn pawn, WorkTags workTag)
		{
			List<object> list = new List<object>();
			if (pawn.story != null && pawn.story.Childhood != null && (pawn.story.Childhood.workDisables & workTag) != WorkTags.None)
			{
				list.Add(pawn.story.Childhood);
			}
			if (pawn.story != null && pawn.story.Adulthood != null && (pawn.story.Adulthood.workDisables & workTag) != WorkTags.None)
			{
				list.Add(pawn.story.Adulthood);
			}
			if (pawn.health != null && pawn.health.hediffSet != null)
			{
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					HediffStage curStage = hediff.CurStage;
					if (curStage != null && (curStage.disabledWorkTags & workTag) != WorkTags.None)
					{
						list.Add(hediff);
					}
				}
			}
			if (pawn.story.traits != null)
			{
				for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
				{
					if (!pawn.story.traits.allTraits[i].Suppressed)
					{
						Trait trait = pawn.story.traits.allTraits[i];
						if ((trait.def.disabledWorkTags & workTag) != WorkTags.None)
						{
							list.Add(trait);
						}
					}
				}
			}
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					if (royalTitle.conceited && (royalTitle.def.disabledWorkTags & workTag) != WorkTags.None)
					{
						list.Add(royalTitle);
					}
				}
			}
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null && (role.def.roleDisabledWorkTags & workTag) != WorkTags.None)
				{
					list.Add(role);
				}
			}
			if (ModsConfig.BiotechActive && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active && (gene.def.disabledWorkTags & workTag) != WorkTags.None)
					{
						list.Add(gene);
					}
				}
			}
			if (ModsConfig.AnomalyActive && pawn.IsMutant && pawn.mutant.IsPassive)
			{
				list.Add(pawn.mutant.Def);
			}
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(pawn))
			{
				if ((questPart_WorkDisabled.disabledWorkTags & workTag) != WorkTags.None && !list.Contains(questPart_WorkDisabled.quest))
				{
					list.Add(questPart_WorkDisabled.quest);
				}
			}
			return list;
		}

		// Token: 0x06011FC1 RID: 73665 RVA: 0x0053D2E4 File Offset: 0x0053B4E4
		private static Color GetDisabledWorkTagLabelColor(Pawn pawn, WorkTags workTag)
		{
			using (List<object>.Enumerator enumerator = CharacterCardUtility.GetWorkTypeDisableCauses(pawn, workTag).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is RoyalTitleDef)
					{
						return CharacterCardUtility.TitleCausedWorkTagDisableColor;
					}
				}
			}
			return Color.white;
		}

		// Token: 0x06011FC2 RID: 73666 RVA: 0x0053D348 File Offset: 0x0053B548
		private static void LifestageAndXenotypeOptions(Pawn pawn, Rect randomizeRect, bool creationMode, bool allowsChildSelection, Action randomizeCallback)
		{
			CharacterCardUtility.<>c__DisplayClass36_0 CS$<>8__locals1 = new CharacterCardUtility.<>c__DisplayClass36_0();
			CS$<>8__locals1.randomizeCallback = randomizeCallback;
			CharacterCardUtility.highlightRect = randomizeRect;
			CharacterCardUtility.highlightRect.yMax = CharacterCardUtility.highlightRect.yMax + (randomizeRect.height + Text.LineHeight + 8f);
			CS$<>8__locals1.startingPawnIndex = StartingPawnUtility.PawnIndex(pawn);
			float width = (randomizeRect.width - 4f) / 2f;
			float num = randomizeRect.x;
			Rect rect = new Rect(num, randomizeRect.y + randomizeRect.height + 4f, width, randomizeRect.height);
			num += rect.width + 4f;
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect2 = rect;
			rect2.y += rect.height + 4f;
			rect2.height = Text.LineHeight;
			Widgets.Label(rect2, pawn.DevelopmentalStage.ToString().Translate().CapitalizeFirst());
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(rect.x, rect.y, rect.width, rect2.yMax - rect.yMin);
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				if (Find.WindowStack.FloatMenu == null)
				{
					TaggedString t = CS$<>8__locals1.<LifestageAndXenotypeOptions>g__GetLabel|0().CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + "DevelopmentalAgeSelectionDesc".Translate();
					if (!allowsChildSelection)
					{
						t += "\n\n" + "MessageDevelopmentalStageSelectionDisabledByScenario".Translate().Colorize(ColorLibrary.RedReadable);
					}
					TooltipHandler.TipRegion(rect3, t.Resolve());
				}
			}
			DevelopmentalStage developmentalStage = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;
			if (ModsConfig.AnomalyActive && pawn.IsMutant)
			{
				developmentalStage = pawn.mutant.Def.allowedDevelopmentalStages;
			}
			if (Widgets.ButtonImageWithBG(rect, CS$<>8__locals1.<LifestageAndXenotypeOptions>g__GetDevelopmentalStageIcon|1(), new Vector2?(new Vector2(22f, 22f))) && TutorSystem.AllowAction("ChangeDevelopmentStage"))
			{
				if (allowsChildSelection)
				{
					int index = CS$<>8__locals1.startingPawnIndex;
					PawnGenerationRequest existing = StartingPawnUtility.GetGenerationRequest(index);
					List<FloatMenuOption> options = new List<FloatMenuOption>
					{
						new FloatMenuOption("Adult".Translate().CapitalizeFirst(), (!developmentalStage.Has(DevelopmentalStage.Adult)) ? null : new Action(delegate()
						{
							if (!existing.AllowedDevelopmentalStages.Has(DevelopmentalStage.Adult))
							{
								existing.AllowedDevelopmentalStages = DevelopmentalStage.Adult;
								existing.AllowDowned = false;
								StartingPawnUtility.SetGenerationRequest(index, existing);
								CS$<>8__locals1.randomizeCallback();
							}
						}), DevelopmentalStageExtensions.AdultTex.Texture, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false),
						new FloatMenuOption("Child".Translate().CapitalizeFirst(), (!developmentalStage.Has(DevelopmentalStage.Child)) ? null : new Action(delegate()
						{
							if (!existing.AllowedDevelopmentalStages.Has(DevelopmentalStage.Child))
							{
								existing.AllowedDevelopmentalStages = DevelopmentalStage.Child;
								existing.AllowDowned = false;
								StartingPawnUtility.SetGenerationRequest(index, existing);
								CS$<>8__locals1.randomizeCallback();
							}
						}), DevelopmentalStageExtensions.ChildTex.Texture, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false),
						new FloatMenuOption("Baby".Translate().CapitalizeFirst(), (!developmentalStage.Has(DevelopmentalStage.Baby)) ? null : new Action(delegate()
						{
							if (!existing.AllowedDevelopmentalStages.Has(DevelopmentalStage.Baby))
							{
								existing.AllowedDevelopmentalStages = DevelopmentalStage.Baby;
								existing.AllowDowned = true;
								StartingPawnUtility.SetGenerationRequest(index, existing);
								CS$<>8__locals1.randomizeCallback();
							}
						}), DevelopmentalStageExtensions.BabyTex.Texture, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
					};
					Find.WindowStack.Add(new FloatMenu(options));
				}
				else
				{
					Messages.Message("MessageDevelopmentalStageSelectionDisabledByScenario".Translate(), null, MessageTypeDefOf.RejectInput, false);
				}
			}
			Rect rect4 = new Rect(num, randomizeRect.y + randomizeRect.height + 4f, width, randomizeRect.height);
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect5 = rect4;
			rect5.y += rect4.height + 4f;
			rect5.height = Text.LineHeight;
			Widgets.Label(rect5, CharacterCardUtility.GetXenotypeLabel(CS$<>8__locals1.startingPawnIndex).Truncate(rect5.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect6 = new Rect(rect4.x, rect4.y, rect4.width, rect5.yMax - rect4.yMin);
			if (Mouse.IsOver(rect6))
			{
				Widgets.DrawHighlight(rect6);
				if (Find.WindowStack.FloatMenu == null)
				{
					TooltipHandler.TipRegion(rect6, CharacterCardUtility.GetXenotypeLabel(CS$<>8__locals1.startingPawnIndex).Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + "XenotypeSelectionDesc".Translate());
				}
			}
			if (Widgets.ButtonImageWithBG(rect4, CharacterCardUtility.GetXenotypeIcon(CS$<>8__locals1.startingPawnIndex), new Vector2?(new Vector2(22f, 22f))) && TutorSystem.AllowAction("ChangeXenotype"))
			{
				int index = CS$<>8__locals1.startingPawnIndex;
				List<FloatMenuOption> list = new List<FloatMenuOption>
				{
					new FloatMenuOption("AnyNonArchite".Translate().CapitalizeFirst(), delegate()
					{
						List<XenotypeDef> allowedXenotypes = (from x in DefDatabase<XenotypeDef>.AllDefs
						where !x.Archite && x != XenotypeDefOf.Baseliner
						select x).ToList<XenotypeDef>();
						CharacterCardUtility.SetupGenerationRequest(index, null, null, allowedXenotypes, 0.5f, (PawnGenerationRequest existing) => existing.ForcedXenotype != null || existing.ForcedCustomXenotype != null, CS$<>8__locals1.randomizeCallback, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					new FloatMenuOption("XenotypeEditor".Translate() + "...", delegate()
					{
						WindowStack windowStack = Find.WindowStack;
						int index = index;
						Action callback;
						if ((callback = CS$<>8__locals1.<>9__11) == null)
						{
							callback = (CS$<>8__locals1.<>9__11 = delegate()
							{
								CharacterCardUtility.cachedCustomXenotypes = null;
								CS$<>8__locals1.randomizeCallback();
							});
						}
						windowStack.Add(new Dialog_CreateXenotype(index, callback));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
				};
				foreach (XenotypeDef xenotype3 in from x in DefDatabase<XenotypeDef>.AllDefs
				orderby -x.displayPriority
				select x)
				{
					XenotypeDef xenotype = xenotype3;
					Func<PawnGenerationRequest, bool> <>9__17;
					list.Add(new FloatMenuOption(xenotype.LabelCap, delegate()
					{
						int index = index;
						XenotypeDef xenotype2 = xenotype;
						CustomXenotype customXenotype = null;
						List<XenotypeDef> allowedXenotypes = null;
						float forceBaselinerChance = 0f;
						Func<PawnGenerationRequest, bool> validator;
						if ((validator = <>9__17) == null)
						{
							validator = (<>9__17 = ((PawnGenerationRequest existing) => CharacterCardUtility.<LifestageAndXenotypeOptions>g__XenotypeValidator|36_5(existing, xenotype)));
						}
						CharacterCardUtility.SetupGenerationRequest(index, xenotype2, customXenotype, allowedXenotypes, forceBaselinerChance, validator, CS$<>8__locals1.randomizeCallback, true);
					}, xenotype.Icon, XenotypeDef.IconColor, MenuOptionPriority.Default, delegate(Rect r)
					{
						TooltipHandler.TipRegion(r, xenotype.descriptionShort ?? xenotype.description);
					}, null, 24f, (Rect r) => Widgets.InfoCardButton(r.x, r.y + 3f, xenotype), null, true, 0, HorizontalJustification.Left, true));
				}
				foreach (CustomXenotype customInner3 in CharacterCardUtility.CustomXenotypes)
				{
					CustomXenotype customInner = customInner3;
					Func<PawnGenerationRequest, bool> <>9__21;
					Action <>9__22;
					list.Add(new FloatMenuOption(customInner.name.CapitalizeFirst() + " (" + "Custom".Translate() + ")", delegate()
					{
						int index = index;
						XenotypeDef xenotype4 = null;
						CustomXenotype customInner2 = customInner;
						List<XenotypeDef> allowedXenotypes = null;
						float forceBaselinerChance = 0f;
						Func<PawnGenerationRequest, bool> validator;
						if ((validator = <>9__21) == null)
						{
							validator = (<>9__21 = ((PawnGenerationRequest existing) => CharacterCardUtility.<LifestageAndXenotypeOptions>g__CustomXenotypeValidator|36_6(existing, customInner)));
						}
						CharacterCardUtility.SetupGenerationRequest(index, xenotype4, customInner2, allowedXenotypes, forceBaselinerChance, validator, CS$<>8__locals1.randomizeCallback, true);
					}, customInner.IconDef.Icon, XenotypeDef.IconColor, MenuOptionPriority.Default, null, null, 24f, delegate(Rect r)
					{
						if (Widgets.ButtonImage(new Rect(r.x, r.y + (r.height - r.width) / 2f, r.width, r.width), TexButton.Delete, GUI.color, true, null))
						{
							WindowStack windowStack = Find.WindowStack;
							TaggedString text = "ConfirmDelete".Translate(customInner.name.CapitalizeFirst());
							Action confirmedAct;
							if ((confirmedAct = <>9__22) == null)
							{
								confirmedAct = (<>9__22 = delegate()
								{
									string path = GenFilePaths.AbsFilePathForXenotype(customInner.name);
									if (File.Exists(path))
									{
										File.Delete(path);
										CharacterCardUtility.cachedCustomXenotypes = null;
									}
								});
							}
							windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
							return true;
						}
						return false;
					}, null, true, 0, HorizontalJustification.Left, true));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06011FC3 RID: 73667 RVA: 0x0053DA8C File Offset: 0x0053BC8C
		private static void SetupGenerationRequest(int index, XenotypeDef xenotype, CustomXenotype customXenotype, List<XenotypeDef> allowedXenotypes, float forceBaselinerChance, Func<PawnGenerationRequest, bool> validator, Action randomizeCallback, bool randomize = true)
		{
			PawnGenerationRequest existing = StartingPawnUtility.GetGenerationRequest(index);
			if (validator(existing))
			{
				if (!CharacterCardUtility.warnedChangingXenotypeWillRandomizePawn && randomize)
				{
					Find.WindowStack.Add(new Dialog_MessageBox("WarnChangingXenotypeWillRandomizePawn".Translate(), "Yes".Translate(), delegate()
					{
						CharacterCardUtility.warnedChangingXenotypeWillRandomizePawn = true;
						existing.ForcedXenotype = xenotype;
						existing.ForcedCustomXenotype = customXenotype;
						existing.AllowedXenotypes = allowedXenotypes;
						existing.ForceBaselinerChance = forceBaselinerChance;
						StartingPawnUtility.SetGenerationRequest(index, existing);
						randomizeCallback();
					}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
					return;
				}
				existing.ForcedXenotype = xenotype;
				existing.ForcedCustomXenotype = customXenotype;
				existing.AllowedXenotypes = allowedXenotypes;
				existing.ForceBaselinerChance = forceBaselinerChance;
				StartingPawnUtility.SetGenerationRequest(index, existing);
				if (randomize)
				{
					randomizeCallback();
				}
			}
		}

		// Token: 0x06011FC4 RID: 73668 RVA: 0x0053DBAC File Offset: 0x0053BDAC
		private static string GetXenotypeLabel(int startingPawnIndex)
		{
			PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(startingPawnIndex);
			if (generationRequest.ForcedCustomXenotype != null)
			{
				return generationRequest.ForcedCustomXenotype.name.CapitalizeFirst();
			}
			if (generationRequest.ForcedXenotype != null)
			{
				return generationRequest.ForcedXenotype.LabelCap;
			}
			return "AnyLower".Translate().CapitalizeFirst();
		}

		// Token: 0x06011FC5 RID: 73669 RVA: 0x0053DC10 File Offset: 0x0053BE10
		private static Texture2D GetXenotypeIcon(int startingPawnIndex)
		{
			PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(startingPawnIndex);
			if (generationRequest.ForcedXenotype != null)
			{
				return generationRequest.ForcedXenotype.Icon;
			}
			if (generationRequest.ForcedCustomXenotype != null)
			{
				return generationRequest.ForcedCustomXenotype.IconDef.Icon;
			}
			return GeneUtility.UniqueXenotypeTex.Texture;
		}

		// Token: 0x06011FC6 RID: 73670 RVA: 0x0053DC60 File Offset: 0x0053BE60
		private static float DoTopStack(Pawn pawn, Rect rect, bool creationMode, float curY)
		{
			CharacterCardUtility.<>c__DisplayClass42_0 CS$<>8__locals1 = new CharacterCardUtility.<>c__DisplayClass42_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.creationMode = creationMode;
			CS$<>8__locals1.curY = curY;
			CharacterCardUtility.tmpStackElements.Clear();
			float num = rect.width - 10f;
			float width = CS$<>8__locals1.creationMode ? (num - 20f - Page_ConfigureStartingPawns.PawnPortraitSize.x) : num;
			Text.Font = GameFont.Small;
			bool flag = ModsConfig.BiotechActive & CS$<>8__locals1.creationMode;
			CS$<>8__locals1.mainDesc = CS$<>8__locals1.pawn.MainDesc(false, !flag);
			if (flag)
			{
				CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						GUI.DrawTexture(r, CS$<>8__locals1.pawn.gender.GetIcon());
						if (Mouse.IsOver(r))
						{
							Func<string> textGetter;
							if ((textGetter = CS$<>8__locals1.<>9__3) == null)
							{
								textGetter = (CS$<>8__locals1.<>9__3 = (() => CS$<>8__locals1.pawn.gender.GetLabel(CS$<>8__locals1.pawn.AnimalOrWildMan()).CapitalizeFirst()));
							}
							TooltipHandler.TipRegion(r, textGetter, 7594764);
						}
					},
					width = 22f
				});
			}
			CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
			{
				drawer = delegate(Rect r)
				{
					Widgets.Label(r, CS$<>8__locals1.mainDesc);
					if (Mouse.IsOver(r))
					{
						Func<string> textGetter;
						if ((textGetter = CS$<>8__locals1.<>9__5) == null)
						{
							textGetter = (CS$<>8__locals1.<>9__5 = (() => CS$<>8__locals1.pawn.ageTracker.AgeTooltipString));
						}
						TooltipHandler.TipRegion(r, textGetter, 6873641);
					}
				},
				width = Text.CalcSize(CS$<>8__locals1.mainDesc).x + 5f
			});
			if (ModsConfig.BiotechActive && CS$<>8__locals1.pawn.genes != null && CS$<>8__locals1.pawn.genes.GenesListForReading.Any<Gene>())
			{
				float num2 = 22f;
				num2 += Text.CalcSize(CS$<>8__locals1.pawn.genes.XenotypeLabelCap).x + 14f;
				CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						Rect rect2 = new Rect(r.x, r.y, r.width, r.height);
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(rect2, BaseContent.WhiteTex);
						GUI.color = Color.white;
						if (Mouse.IsOver(rect2))
						{
							Widgets.DrawHighlight(rect2);
						}
						Rect position = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
						GUI.color = XenotypeDef.IconColor;
						GUI.DrawTexture(position, CS$<>8__locals1.pawn.genes.XenotypeIcon);
						GUI.color = Color.white;
						Widgets.Label(new Rect(r.x + 22f + 5f, r.y, r.width + 22f - 1f, r.height), CS$<>8__locals1.pawn.genes.XenotypeLabelCap);
						if (Mouse.IsOver(r))
						{
							Rect rect3 = r;
							Func<string> textGetter;
							if ((textGetter = CS$<>8__locals1.<>9__9) == null)
							{
								textGetter = (CS$<>8__locals1.<>9__9 = (() => string.Concat(new string[]
								{
									("Xenotype".Translate() + ": " + CS$<>8__locals1.pawn.genes.XenotypeLabelCap).Colorize(ColoredText.TipSectionTitleColor),
									"\n\n",
									CS$<>8__locals1.pawn.genes.XenotypeDescShort,
									"\n\n",
									"ViewGenesDesc".Translate(CS$<>8__locals1.pawn.Named("PAWN")).ToString().StripTags().Colorize(ColoredText.SubtleGrayColor)
								})));
							}
							TooltipHandler.TipRegion(rect3, textGetter, 883938493);
						}
						if (Widgets.ButtonInvisible(r, true))
						{
							if (Current.ProgramState == ProgramState.Playing && Find.WindowStack.WindowOfType<Dialog_InfoCard>() == null && Find.WindowStack.WindowOfType<Dialog_GrowthMomentChoices>() == null)
							{
								InspectPaneUtility.OpenTab(typeof(ITab_Genes));
								return;
							}
							Find.WindowStack.Add(new Dialog_ViewGenes(CS$<>8__locals1.pawn));
						}
					},
					width = num2
				});
				CS$<>8__locals1.curY += GenUI.DrawElementStack<GenUI.AnonymousStackElement>(new Rect(0f, CS$<>8__locals1.curY, width, 50f), 22f, CharacterCardUtility.tmpStackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
				{
					obj.drawer(r);
				}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false).height + 4f;
				CharacterCardUtility.tmpStackElements.Clear();
			}
			if (CS$<>8__locals1.pawn.Faction != null && !CS$<>8__locals1.pawn.Faction.Hidden)
			{
				CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						Rect rect2 = new Rect(r.x, r.y, r.width, r.height);
						Color color = GUI.color;
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(rect2, BaseContent.WhiteTex);
						GUI.color = color;
						Widgets.DrawHighlightIfMouseover(rect2);
						Rect rect3 = new Rect(r.x, r.y, r.width, r.height);
						Rect position = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
						GUI.color = CS$<>8__locals1.pawn.Faction.Color;
						GUI.DrawTexture(position, CS$<>8__locals1.pawn.Faction.def.FactionIcon);
						GUI.color = color;
						Widgets.Label(new Rect(rect3.x + rect3.height + 5f, rect3.y, rect3.width - 10f, rect3.height), CS$<>8__locals1.pawn.Faction.Name);
						if (Widgets.ButtonInvisible(rect2, true))
						{
							if (CS$<>8__locals1.creationMode || Find.WindowStack.AnyWindowAbsorbingAllInput)
							{
								Find.WindowStack.Add(new Dialog_FactionDuringLanding());
							}
							else
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
							}
						}
						if (Mouse.IsOver(rect2))
						{
							string text = string.Concat(new string[]
							{
								"Faction".Translate().Colorize(ColoredText.TipSectionTitleColor),
								"\n\n",
								"FactionDesc".Translate(CS$<>8__locals1.pawn.Named("PAWN")).Resolve(),
								"\n\n",
								"ClickToViewFactions".Translate().Colorize(ColoredText.SubtleGrayColor)
							});
							TipSignal tip = new TipSignal(text, CS$<>8__locals1.pawn.Faction.loadID * 37);
							TooltipHandler.TipRegion(rect2, tip);
						}
					},
					width = Text.CalcSize(CS$<>8__locals1.pawn.Faction.Name).x + 22f + 15f
				});
			}
			CharacterCardUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(CS$<>8__locals1.pawn, CharacterCardUtility.tmpExtraFactions, null);
			GuestUtility.GetExtraFactionsFromGuestStatus(CS$<>8__locals1.pawn, CharacterCardUtility.tmpExtraFactions);
			foreach (ExtraFaction extraFaction in CharacterCardUtility.tmpExtraFactions)
			{
				if (CS$<>8__locals1.pawn.Faction != extraFaction.faction)
				{
					ExtraFaction localExtraFaction = extraFaction;
					string factionName = localExtraFaction.faction.Name;
					bool drawExtraFactionIcon = localExtraFaction.factionType == ExtraFactionType.HomeFaction || localExtraFaction.factionType == ExtraFactionType.MiniFaction;
					CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							Rect rect2 = new Rect(r.x, r.y, r.width, r.height);
							Rect rect3 = drawExtraFactionIcon ? rect2 : r;
							Color color = GUI.color;
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(rect3, BaseContent.WhiteTex);
							GUI.color = color;
							Widgets.DrawHighlightIfMouseover(rect3);
							if (drawExtraFactionIcon)
							{
								Rect rect4 = new Rect(r.x, r.y, r.width, r.height);
								Rect position = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
								GUI.color = localExtraFaction.faction.Color;
								GUI.DrawTexture(position, localExtraFaction.faction.def.FactionIcon);
								GUI.color = color;
								Widgets.Label(new Rect(rect4.x + rect4.height + 5f, rect4.y, rect4.width - 10f, rect4.height), factionName);
							}
							else
							{
								Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), factionName);
							}
							if (Widgets.ButtonInvisible(rect2, true))
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
							}
							if (Mouse.IsOver(rect3))
							{
								TipSignal tip = new TipSignal((localExtraFaction.factionType.GetLabel().CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + "ExtraFactionDesc".Translate(CS$<>8__locals1.pawn.Named("PAWN")) + "\n\n" + "ClickToViewFactions".Translate().Colorize(ColoredText.SubtleGrayColor)).Resolve(), localExtraFaction.faction.loadID ^ 1938473043);
								TooltipHandler.TipRegion(rect3, tip);
							}
						},
						width = Text.CalcSize(factionName).x + (float)(drawExtraFactionIcon ? 22 : 0) + 15f
					});
				}
			}
			if (!Find.IdeoManager.classicMode && CS$<>8__locals1.pawn.Ideo != null && ModsConfig.IdeologyActive)
			{
				float width2 = Text.CalcSize(CS$<>8__locals1.pawn.Ideo.name).x + 22f + 15f;
				CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(r, BaseContent.WhiteTex);
						GUI.color = Color.white;
						IdeoUIUtility.DrawIdeoPlate(r, CS$<>8__locals1.pawn.Ideo, CS$<>8__locals1.pawn);
					},
					width = width2
				});
			}
			if (ModsConfig.IdeologyActive)
			{
				CharacterCardUtility.<>c__DisplayClass42_2 CS$<>8__locals3 = new CharacterCardUtility.<>c__DisplayClass42_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
				CharacterCardUtility.<>c__DisplayClass42_2 CS$<>8__locals4 = CS$<>8__locals3;
				Ideo ideo = CS$<>8__locals3.CS$<>8__locals2.pawn.Ideo;
				CS$<>8__locals4.role = ((ideo != null) ? ideo.GetRole(CS$<>8__locals3.CS$<>8__locals2.pawn) : null);
				if (CS$<>8__locals3.role != null)
				{
					string roleLabel = CS$<>8__locals3.role.LabelForPawn(CS$<>8__locals3.CS$<>8__locals2.pawn);
					CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							Color color = GUI.color;
							Rect rect2 = new Rect(r.x, r.y, r.width, r.height);
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(rect2, BaseContent.WhiteTex);
							GUI.color = color;
							if (Mouse.IsOver(rect2))
							{
								Widgets.DrawHighlight(rect2);
							}
							Rect rect3 = new Rect(r.x, r.y, r.width + 22f + 9f, r.height);
							Rect position = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
							GUI.color = CS$<>8__locals3.CS$<>8__locals2.pawn.Ideo.Color;
							GUI.DrawTexture(position, CS$<>8__locals3.role.Icon);
							GUI.color = Color.white;
							Widgets.Label(new Rect(rect3.x + 22f + 5f, rect3.y, rect3.width - 10f, rect3.height), roleLabel);
							if (Widgets.ButtonInvisible(rect2, true))
							{
								InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Social));
							}
							if (Mouse.IsOver(rect2))
							{
								Func<string> textGetter;
								if ((textGetter = CS$<>8__locals3.<>9__14) == null)
								{
									textGetter = (CS$<>8__locals3.<>9__14 = (() => CS$<>8__locals3.role.GetTip()));
								}
								TipSignal tip = new TipSignal(textGetter, (int)CS$<>8__locals3.CS$<>8__locals2.curY * 39);
								TooltipHandler.TipRegion(rect2, tip);
							}
						},
						width = Text.CalcSize(roleLabel).x + 22f + 14f
					});
				}
			}
			int favor;
			if (CS$<>8__locals1.pawn.royalty != null && CS$<>8__locals1.pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				using (List<RoyalTitle>.Enumerator enumerator2 = CS$<>8__locals1.pawn.royalty.AllTitlesInEffectForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CharacterCardUtility.<>c__DisplayClass42_4 CS$<>8__locals6 = new CharacterCardUtility.<>c__DisplayClass42_4();
						CS$<>8__locals6.CS$<>8__locals4 = CS$<>8__locals1;
						CS$<>8__locals6.title = enumerator2.Current;
						CS$<>8__locals6.localTitle = CS$<>8__locals6.title;
						CharacterCardUtility.<>c__DisplayClass42_4 CS$<>8__locals7 = CS$<>8__locals6;
						string labelCapFor = CS$<>8__locals6.localTitle.def.GetLabelCapFor(CS$<>8__locals6.CS$<>8__locals4.pawn);
						string str2 = " (";
						favor = CS$<>8__locals6.CS$<>8__locals4.pawn.royalty.GetFavor(CS$<>8__locals6.localTitle.faction);
						CS$<>8__locals7.titleLabel = labelCapFor + str2 + favor.ToString() + ")";
						CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								Color color = GUI.color;
								Rect rect2 = new Rect(r.x, r.y, r.width, r.height);
								GUI.color = CharacterCardUtility.StackElementBackground;
								GUI.DrawTexture(rect2, BaseContent.WhiteTex);
								GUI.color = color;
								int favor = CS$<>8__locals6.CS$<>8__locals4.pawn.royalty.GetFavor(CS$<>8__locals6.localTitle.faction);
								if (Mouse.IsOver(rect2))
								{
									Widgets.DrawHighlight(rect2);
								}
								Rect rect3 = new Rect(r.x, r.y, r.width + 22f + 9f, r.height);
								Rect position = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
								GUI.color = CS$<>8__locals6.title.faction.Color;
								GUI.DrawTexture(position, CS$<>8__locals6.localTitle.faction.def.FactionIcon);
								GUI.color = color;
								Widgets.Label(new Rect(rect3.x + 22f + 5f, rect3.y, rect3.width - 10f, rect3.height), CS$<>8__locals6.titleLabel);
								if (Widgets.ButtonInvisible(rect2, true))
								{
									Find.WindowStack.Add(new Dialog_InfoCard(CS$<>8__locals6.localTitle.def, CS$<>8__locals6.localTitle.faction, CS$<>8__locals6.CS$<>8__locals4.pawn));
								}
								if (Mouse.IsOver(rect2))
								{
									TipSignal tip = new TipSignal(() => CharacterCardUtility.GetTitleTipString(CS$<>8__locals6.CS$<>8__locals4.pawn, CS$<>8__locals6.localTitle.faction, CS$<>8__locals6.localTitle, favor), (int)CS$<>8__locals6.CS$<>8__locals4.curY * 37);
									TooltipHandler.TipRegion(rect2, tip);
								}
							},
							width = Text.CalcSize(CS$<>8__locals6.titleLabel).x + 22f + 14f
						});
					}
				}
			}
			if (ModsConfig.IdeologyActive && !CS$<>8__locals1.pawn.DevelopmentalStage.Baby())
			{
				Pawn_StoryTracker story = CS$<>8__locals1.pawn.story;
				if (((story != null) ? story.favoriteColor : null) != null)
				{
					CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
					{
						drawer = delegate(Rect r)
						{
							string orIdeoColor = string.Empty;
							if (CS$<>8__locals1.pawn.Ideo != null && !CS$<>8__locals1.pawn.Ideo.classicMode)
							{
								orIdeoColor = "OrIdeoColor".Translate(CS$<>8__locals1.pawn.Named("PAWN"));
							}
							Widgets.DrawRectFast(r, CS$<>8__locals1.pawn.story.favoriteColor.color, null);
							GUI.color = CharacterCardUtility.FavColorBoxColor;
							Widgets.DrawBox(r, 1, null);
							GUI.color = Color.white;
							TooltipHandler.TipRegion(r, () => "FavoriteColorTooltip".Translate(CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals1.pawn.story.favoriteColor.label.Named("COLOR"), 0.6f.ToStringPercent().Named("PERCENTAGE"), orIdeoColor.Named("ORIDEO")).Resolve(), 837472764);
						},
						width = 22f
					});
				}
			}
			if (CS$<>8__locals1.pawn.guest != null && !CS$<>8__locals1.pawn.guest.Recruitable)
			{
				CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						Color color = GUI.color;
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(r, BaseContent.WhiteTex);
						GUI.color = color;
						GUI.DrawTexture(r, CharacterCardUtility.UnrecruitableIcon);
						if (Mouse.IsOver(r))
						{
							Widgets.DrawHighlight(r);
							Func<string> textGetter;
							if ((textGetter = CS$<>8__locals1.<>9__20) == null)
							{
								textGetter = (CS$<>8__locals1.<>9__20 = (() => "Unrecruitable".Translate().AsTipTitle().CapitalizeFirst() + "\n\n" + "UnrecruitableDesc".Translate(CS$<>8__locals1.pawn.Named("PAWN")).Resolve()));
							}
							TooltipHandler.TipRegion(r, textGetter, 15877733);
						}
					},
					width = 22f
				});
			}
			CS$<>8__locals1.drawMinimized = (CharacterCardUtility.tmpMaxElementStackHeight > 44f);
			QuestUtility.AppendInspectStringsFromQuestParts(delegate(string str, Quest quest)
			{
				CharacterCardUtility.tmpStackElements.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						Color color = GUI.color;
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(r, BaseContent.WhiteTex);
						GUI.color = color;
						CharacterCardUtility.DoQuestLine(r, str, quest, CS$<>8__locals1.drawMinimized);
					},
					width = CharacterCardUtility.GetQuestLineSize(str, quest, CS$<>8__locals1.drawMinimized).x
				});
			}, CS$<>8__locals1.pawn, out favor);
			float height = GenUI.DrawElementStack<GenUI.AnonymousStackElement>(new Rect(0f, CS$<>8__locals1.curY, width, 50f), 22f, CharacterCardUtility.tmpStackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
			{
				obj.drawer(r);
			}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false).height;
			CharacterCardUtility.tmpMaxElementStackHeight = Mathf.Max(height, CharacterCardUtility.tmpMaxElementStackHeight);
			CS$<>8__locals1.curY += height;
			if (CharacterCardUtility.tmpStackElements.Any<GenUI.AnonymousStackElement>())
			{
				CS$<>8__locals1.curY += 10f;
			}
			return CS$<>8__locals1.curY;
		}

		// Token: 0x06011FC7 RID: 73671 RVA: 0x0053E420 File Offset: 0x0053C620
		private static void DoLeftSection(Rect rect, Rect leftRect, Pawn pawn)
		{
			CharacterCardUtility.<>c__DisplayClass43_0 CS$<>8__locals1 = new CharacterCardUtility.<>c__DisplayClass43_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.leftRect = leftRect;
			Widgets.BeginGroup(CS$<>8__locals1.leftRect);
			Pawn pawnLocal = CS$<>8__locals1.pawn;
			List<Ability> abilities = (from a in CS$<>8__locals1.pawn.abilities.AllAbilitiesForReading
			where a.def.showOnCharacterCard
			orderby a.def.level, a.def.EntropyGain
			select a).ToList<Ability>();
			int numSections = abilities.Any<Ability>() ? 5 : 4;
			float num = (float)Enum.GetValues(typeof(BackstorySlot)).Length * 22f;
			float stackHeight = 0f;
			if (CS$<>8__locals1.pawn.story != null && CS$<>8__locals1.pawn.story.title != null)
			{
				num += 22f;
			}
			List<CharacterCardUtility.LeftRectSection> list = new List<CharacterCardUtility.LeftRectSection>();
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals1.leftRect.width, num),
				drawer = delegate(Rect sectionRect)
				{
					float num7 = sectionRect.y;
					Text.Font = GameFont.Small;
					foreach (object obj in Enum.GetValues(typeof(BackstorySlot)))
					{
						BackstorySlot backstorySlot = (BackstorySlot)obj;
						BackstoryDef backstory = CS$<>8__locals1.pawn.story.GetBackstory(backstorySlot);
						if (backstory != null)
						{
							Rect rect5 = new Rect(sectionRect.x, num7, CS$<>8__locals1.leftRect.width, 22f);
							Text.Anchor = TextAnchor.MiddleLeft;
							Widgets.Label(rect5, (backstorySlot == BackstorySlot.Adulthood) ? "Adulthood".Translate() : "Childhood".Translate());
							Text.Anchor = TextAnchor.UpperLeft;
							string text = backstory.TitleCapFor(CS$<>8__locals1.pawn.gender);
							Rect rect6 = new Rect(rect5);
							rect6.x += 90f;
							rect6.width = Text.CalcSize(text).x + 10f;
							Color color = GUI.color;
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(rect6, BaseContent.WhiteTex);
							GUI.color = color;
							Text.Anchor = TextAnchor.MiddleCenter;
							Widgets.Label(rect6, text.Truncate(rect6.width, null));
							Text.Anchor = TextAnchor.UpperLeft;
							if (Mouse.IsOver(rect6))
							{
								Widgets.DrawHighlight(rect6);
							}
							if (Mouse.IsOver(rect6))
							{
								TooltipHandler.TipRegion(rect6, backstory.FullDescriptionFor(CS$<>8__locals1.pawn).Resolve());
							}
							num7 += rect5.height + 4f;
						}
					}
					if (CS$<>8__locals1.pawn.story != null && CS$<>8__locals1.pawn.story.title != null)
					{
						Rect rect7 = new Rect(sectionRect.x, num7, CS$<>8__locals1.leftRect.width, 22f);
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect7, "BackstoryTitle".Translate() + ":");
						Text.Anchor = TextAnchor.UpperLeft;
						Rect rect8 = new Rect(rect7);
						rect8.x += 90f;
						rect8.width -= 90f;
						Widgets.Label(rect8, CS$<>8__locals1.pawn.story.title);
						num7 += rect7.height;
					}
				}
			});
			num = 30f;
			List<Trait> traits = CS$<>8__locals1.pawn.story.traits.allTraits;
			if (traits == null || traits.Count == 0)
			{
				num += 22f;
				stackHeight = 22f;
			}
			else
			{
				Rect rect2 = GenUI.DrawElementStack<Trait>(new Rect(0f, 0f, CS$<>8__locals1.leftRect.width - 5f, CS$<>8__locals1.leftRect.height), 22f, CS$<>8__locals1.pawn.story.traits.TraitsSorted, delegate(Rect r, Trait trait)
				{
				}, (Trait trait) => Text.CalcSize(trait.LabelCap).x + 10f, 4f, 5f, false);
				num += rect2.height;
				stackHeight = rect2.height;
			}
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals1.leftRect.width, num),
				drawer = delegate(Rect sectionRect)
				{
					float currentY = sectionRect.y;
					Widgets.Label(new Rect(sectionRect.x, currentY, 200f, 30f), "Traits".Translate().AsTipTitle());
					currentY += 24f;
					if (traits == null || traits.Count == 0)
					{
						Color color = GUI.color;
						GUI.color = Color.gray;
						Rect rect5 = new Rect(sectionRect.x, currentY, CS$<>8__locals1.leftRect.width, 24f);
						if (Mouse.IsOver(rect5))
						{
							Widgets.DrawHighlight(rect5);
						}
						Widgets.Label(rect5, CS$<>8__locals1.pawn.DevelopmentalStage.Baby() ? "TraitsDevelopLaterBaby".Translate() : "None".Translate());
						currentY += rect5.height + 2f;
						TooltipHandler.TipRegionByKey(rect5, "None");
						GUI.color = color;
						return;
					}
					GenUI.DrawElementStack<Trait>(new Rect(sectionRect.x, currentY, CS$<>8__locals1.leftRect.width - 5f, stackHeight), 22f, CS$<>8__locals1.pawn.story.traits.TraitsSorted, delegate(Rect r, Trait trait)
					{
						Color color2 = GUI.color;
						GUI.color = CharacterCardUtility.StackElementBackground;
						GUI.DrawTexture(r, BaseContent.WhiteTex);
						GUI.color = color2;
						if (Mouse.IsOver(r))
						{
							Widgets.DrawHighlight(r);
						}
						if (trait.Suppressed)
						{
							GUI.color = ColoredText.SubtleGrayColor;
						}
						else if (trait.sourceGene != null)
						{
							GUI.color = ColoredText.GeneColor;
						}
						Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), trait.LabelCap);
						GUI.color = Color.white;
						if (Mouse.IsOver(r))
						{
							Trait trLocal = trait;
							TipSignal tip = new TipSignal(() => trLocal.TipString(CS$<>8__locals1.pawn), (int)currentY * 37);
							TooltipHandler.TipRegion(r, tip);
						}
					}, (Trait trait) => Text.CalcSize(trait.LabelCap).x + 10f, 4f, 5f, false);
				}
			});
			num = 30f;
			WorkTags disabledTags = CS$<>8__locals1.pawn.CombinedDisabledWorkTags;
			List<WorkTags> disabledTagsList = CharacterCardUtility.WorkTagsFrom(disabledTags).ToList<WorkTags>();
			bool allowWorkTagVerticalLayout = false;
			GenUI.StackElementWidthGetter<WorkTags> workTagWidthGetter = (WorkTags tag) => Text.CalcSize(tag.LabelTranslated().CapitalizeFirst()).x + 10f;
			if (disabledTags == WorkTags.None)
			{
				num += 22f;
			}
			else
			{
				disabledTagsList.Sort(delegate(WorkTags a, WorkTags b)
				{
					int num7 = CharacterCardUtility.GetWorkTypeDisableCauses(CS$<>8__locals1.pawn, a).Any((object c) => c is RoyalTitleDef) ? 1 : -1;
					int value2 = CharacterCardUtility.GetWorkTypeDisableCauses(CS$<>8__locals1.pawn, b).Any((object c) => c is RoyalTitleDef) ? 1 : -1;
					return num7.CompareTo(value2);
				});
				Rect rect3 = GenUI.DrawElementStack<WorkTags>(new Rect(0f, 0f, CS$<>8__locals1.leftRect.width - 5f, CS$<>8__locals1.leftRect.height), 22f, disabledTagsList, delegate(Rect r, WorkTags tag)
				{
				}, workTagWidthGetter, 4f, 5f, false);
				num += rect3.height;
				stackHeight = rect3.height;
				num += 12f;
				allowWorkTagVerticalLayout = (GenUI.DrawElementStackVertical<WorkTags>(new Rect(0f, 0f, rect.width, stackHeight), 22f, disabledTagsList, delegate(Rect r, WorkTags tag)
				{
				}, workTagWidthGetter, 5f, false).width <= CS$<>8__locals1.leftRect.width);
			}
			list.Add(new CharacterCardUtility.LeftRectSection
			{
				rect = new Rect(0f, 0f, CS$<>8__locals1.leftRect.width, num),
				drawer = delegate(Rect sectionRect)
				{
					float currentY = sectionRect.y;
					Widgets.Label(new Rect(sectionRect.x, currentY, 200f, 24f), "IncapableOf".Translate(CS$<>8__locals1.pawn).AsTipTitle());
					currentY += 24f;
					if (disabledTags == WorkTags.None)
					{
						GUI.color = Color.gray;
						Rect rect5 = new Rect(sectionRect.x, currentY, CS$<>8__locals1.leftRect.width, 24f);
						if (Mouse.IsOver(rect5))
						{
							Widgets.DrawHighlight(rect5);
						}
						Widgets.Label(rect5, "None".Translate());
						TooltipHandler.TipRegionByKey(rect5, "None");
					}
					else
					{
						GenUI.StackElementDrawer<WorkTags> drawer = delegate(Rect r, WorkTags tag)
						{
							Color color = GUI.color;
							GUI.color = CharacterCardUtility.StackElementBackground;
							GUI.DrawTexture(r, BaseContent.WhiteTex);
							GUI.color = color;
							GUI.color = CharacterCardUtility.GetDisabledWorkTagLabelColor(CS$<>8__locals1.pawn, tag);
							if (Mouse.IsOver(r))
							{
								Widgets.DrawHighlight(r);
							}
							Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), tag.LabelTranslated().CapitalizeFirst());
							if (Mouse.IsOver(r))
							{
								WorkTags tagLocal = tag;
								TipSignal tip = new TipSignal(() => CharacterCardUtility.GetWorkTypeDisabledCausedBy(pawnLocal, tagLocal) + "\n" + CharacterCardUtility.GetWorkTypesDisabledByWorkTag(tagLocal), (int)currentY * 32);
								TooltipHandler.TipRegion(r, tip);
							}
						};
						if (allowWorkTagVerticalLayout)
						{
							GenUI.DrawElementStackVertical<WorkTags>(new Rect(sectionRect.x, currentY, CS$<>8__locals1.leftRect.width - 5f, CS$<>8__locals1.leftRect.height / (float)numSections), 22f, disabledTagsList, drawer, workTagWidthGetter, 5f, false);
						}
						else
						{
							GenUI.DrawElementStack<WorkTags>(new Rect(sectionRect.x, currentY, CS$<>8__locals1.leftRect.width - 5f, CS$<>8__locals1.leftRect.height / (float)numSections), 22f, disabledTagsList, drawer, workTagWidthGetter, 5f, 5f, true);
						}
					}
					GUI.color = Color.white;
				}
			});
			if (abilities.Any<Ability>())
			{
				num = 30f;
				Rect rect4 = GenUI.DrawElementStack<Ability>(new Rect(0f, 0f, CS$<>8__locals1.leftRect.width - 5f, CS$<>8__locals1.leftRect.height), 32f, abilities, delegate(Rect r, Ability abil)
				{
				}, (Ability abil) => 32f, 4f, 5f, true);
				num += rect4.height;
				stackHeight = rect4.height;
				list.Add(new CharacterCardUtility.LeftRectSection
				{
					rect = new Rect(0f, 0f, CS$<>8__locals1.leftRect.width, num),
					drawer = delegate(Rect sectionRect)
					{
						float currentY = sectionRect.y;
						Widgets.Label(new Rect(sectionRect.x, currentY, 200f, 24f), "Abilities".Translate(CS$<>8__locals1.pawn).AsTipTitle());
						currentY += 24f;
						GenUI.DrawElementStack<Ability>(new Rect(sectionRect.x, currentY, CS$<>8__locals1.leftRect.width - 5f, stackHeight), 32f, abilities, delegate(Rect r, Ability abil)
						{
							GUI.DrawTexture(r, BaseContent.ClearTex);
							if (Mouse.IsOver(r))
							{
								Widgets.DrawHighlight(r);
							}
							if (Widgets.ButtonImage(r, abil.def.uiIcon, false, null))
							{
								Find.WindowStack.Add(new Dialog_InfoCard(abil.def, null));
							}
							if (Mouse.IsOver(r))
							{
								Ability abilCapture = abil;
								TipSignal tip = new TipSignal(() => abilCapture.Tooltip + "\n\n" + "ClickToLearnMore".Translate().Colorize(ColoredText.SubtleGrayColor), (int)currentY * 37);
								TooltipHandler.TipRegion(r, tip);
							}
						}, (Ability abil) => 32f, 4f, 5f, true);
						GUI.color = Color.white;
					}
				});
			}
			else
			{
				num += 12f;
			}
			float num2 = CS$<>8__locals1.leftRect.height / (float)list.Count;
			float num3 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				CharacterCardUtility.LeftRectSection value = list[i];
				if (value.rect.height > num2)
				{
					num3 += value.rect.height - num2;
					value.calculatedSize = value.rect.height;
				}
				else
				{
					value.calculatedSize = num2;
				}
				list[i] = value;
			}
			bool flag = false;
			float num4 = 0f;
			if (num3 > 0f)
			{
				CharacterCardUtility.LeftRectSection leftRectSection = list[0];
				float num5 = leftRectSection.rect.height + 12f;
				num3 -= leftRectSection.calculatedSize - num5;
				leftRectSection.calculatedSize = num5;
				list[0] = leftRectSection;
			}
			while (num3 > 0f)
			{
				bool flag2 = true;
				for (int j = 0; j < list.Count; j++)
				{
					CharacterCardUtility.LeftRectSection leftRectSection2 = list[j];
					if (leftRectSection2.calculatedSize - leftRectSection2.rect.height > 0f)
					{
						leftRectSection2.calculatedSize -= 1f;
						num3 -= 1f;
						flag2 = false;
					}
					list[j] = leftRectSection2;
				}
				if (flag2)
				{
					for (int k = 0; k < list.Count; k++)
					{
						CharacterCardUtility.LeftRectSection leftRectSection3 = list[k];
						if (k > 0)
						{
							leftRectSection3.calculatedSize = Mathf.Max(leftRectSection3.rect.height, num2);
						}
						else
						{
							leftRectSection3.calculatedSize = leftRectSection3.rect.height + 22f;
						}
						num4 += leftRectSection3.calculatedSize;
						list[k] = leftRectSection3;
					}
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Widgets.BeginScrollView(new Rect(0f, 0f, CS$<>8__locals1.leftRect.width, CS$<>8__locals1.leftRect.height), ref CharacterCardUtility.leftRectScrollPos, new Rect(0f, 0f, CS$<>8__locals1.leftRect.width - 16f, num4), true);
			}
			float num6 = 0f;
			for (int l = 0; l < list.Count; l++)
			{
				CharacterCardUtility.LeftRectSection leftRectSection4 = list[l];
				leftRectSection4.drawer(new Rect(0f, num6, CS$<>8__locals1.leftRect.width - 5f, leftRectSection4.rect.height));
				num6 += leftRectSection4.calculatedSize;
			}
			if (flag)
			{
				Widgets.EndScrollView();
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011FC8 RID: 73672 RVA: 0x0053ECD0 File Offset: 0x0053CED0
		private static string GetWorkTypeDisabledCausedBy(Pawn pawn, WorkTags workTag)
		{
			List<object> workTypeDisableCauses = CharacterCardUtility.GetWorkTypeDisableCauses(pawn, workTag);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in workTypeDisableCauses)
			{
				BackstoryDef backstoryDef = obj as BackstoryDef;
				if (backstoryDef != null)
				{
					stringBuilder.AppendLine("IncapableOfTooltipBackstory".Translate() + ": " + backstoryDef.TitleFor(pawn.gender).CapitalizeFirst());
				}
				else
				{
					Trait trait = obj as Trait;
					if (trait != null)
					{
						stringBuilder.AppendLine("IncapableOfTooltipTrait".Translate() + ": " + trait.LabelCap);
					}
					else
					{
						Hediff hediff = obj as Hediff;
						if (hediff != null)
						{
							stringBuilder.AppendLine("IncapableOfTooltipHediff".Translate() + ": " + hediff.LabelCap);
						}
						else
						{
							RoyalTitle royalTitle = obj as RoyalTitle;
							if (royalTitle != null)
							{
								stringBuilder.AppendLine("IncapableOfTooltipTitle".Translate() + ": " + royalTitle.def.GetLabelFor(pawn));
							}
							else
							{
								Quest quest = obj as Quest;
								if (quest != null)
								{
									stringBuilder.AppendLine("IncapableOfTooltipQuest".Translate() + ": " + quest.name);
								}
								else
								{
									Precept_Role precept_Role = obj as Precept_Role;
									if (precept_Role != null)
									{
										stringBuilder.AppendLine("IncapableOfTooltipRole".Translate() + ": " + precept_Role.LabelForPawn(pawn));
									}
									else
									{
										Gene gene = obj as Gene;
										if (gene != null)
										{
											stringBuilder.AppendLine("IncapableOfTooltipGene".Translate() + ": " + gene.LabelCap);
										}
										else
										{
											MutantDef mutantDef = obj as MutantDef;
											if (mutantDef != null)
											{
												stringBuilder.AppendLine("IncapableOfTooltipMutant".Translate() + ": " + mutantDef.LabelCap);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06011FC9 RID: 73673 RVA: 0x0053EF28 File Offset: 0x0053D128
		private static string GetWorkTypesDisabledByWorkTag(WorkTags workTag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("IncapableOfTooltipWorkTypes".Translate().Colorize(ColoredText.TipSectionTitleColor));
			foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
			{
				if ((workTypeDef.workTags & workTag) > WorkTags.None)
				{
					stringBuilder.Append("- ");
					stringBuilder.AppendLine(workTypeDef.pawnLabel);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06011FCA RID: 73674 RVA: 0x0053EFB8 File Offset: 0x0053D1B8
		public static Vector2 PawnCardSize(Pawn pawn)
		{
			Vector2 basePawnCardSize = CharacterCardUtility.BasePawnCardSize;
			CharacterCardUtility.tmpInspectStrings.Length = 0;
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(CharacterCardUtility.tmpInspectStrings, pawn, out num);
			if (num >= 2)
			{
				basePawnCardSize.y += (float)((num - 1) * 20);
			}
			return basePawnCardSize;
		}

		// Token: 0x06011FCB RID: 73675 RVA: 0x0053EFFC File Offset: 0x0053D1FC
		public static void DoNameInputRect(Rect rect, ref string name, int maxLength)
		{
			string text = Widgets.TextField(rect, name);
			if (text.Length <= maxLength && CharacterCardUtility.ValidNameRegex.IsMatch(text))
			{
				name = text;
			}
		}

		// Token: 0x06011FCC RID: 73676 RVA: 0x0053F02B File Offset: 0x0053D22B
		private static IEnumerable<WorkTags> WorkTagsFrom(WorkTags tags)
		{
			foreach (WorkTags workTags in tags.GetAllSelectedItems<WorkTags>())
			{
				if (workTags != WorkTags.None)
				{
					yield return workTags;
				}
			}
			IEnumerator<WorkTags> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06011FCD RID: 73677 RVA: 0x0053F03C File Offset: 0x0053D23C
		private static Vector2 GetQuestLineSize(string line, Quest quest, bool drawMinimized)
		{
			if (drawMinimized)
			{
				return new Vector2(24f, 24f);
			}
			Vector2 vector = Text.CalcSize(line);
			return new Vector2(24f + vector.x + 10f, Mathf.Max(24f, vector.y));
		}

		// Token: 0x06011FCE RID: 73678 RVA: 0x0053F08C File Offset: 0x0053D28C
		private static void DoQuestLine(Rect rect, string line, Quest quest, bool drawMinimized)
		{
			Rect rect2 = rect;
			rect2.xMin += 29f;
			rect2.height = Text.CalcSize(line).y;
			float x = Text.CalcSize(line).x;
			Rect rect3 = new Rect(rect.x, rect.y, Mathf.Min(x, rect2.width) + 24f + 5f, rect.height);
			if (!quest.hidden)
			{
				Widgets.DrawHighlightIfMouseover(rect3);
				if (drawMinimized)
				{
					TooltipHandler.TipRegion(rect3, line + "\n\n" + "ClickToViewInQuestsTab".Translate());
				}
				else
				{
					TooltipHandler.TipRegionByKey(rect3, "ClickToViewInQuestsTab");
				}
			}
			GUI.DrawTexture(new Rect(rect.x, rect.y - 1f, 24f, 24f), CharacterCardUtility.QuestIcon);
			if (!drawMinimized)
			{
				Widgets.Label(rect2, line.Truncate(rect2.width, null));
			}
			if (!quest.hidden && Widgets.ButtonInvisible(rect3, true))
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
			}
		}

		// Token: 0x06011FD0 RID: 73680 RVA: 0x0053F298 File Offset: 0x0053D498
		[CompilerGenerated]
		internal static bool <LifestageAndXenotypeOptions>g__XenotypeValidator|36_5(PawnGenerationRequest req, XenotypeDef xenotype)
		{
			if (TutorSystem.TutorialMode && req.MustBeCapableOfViolence)
			{
				if (xenotype.AllGenes.Any((GeneDef g) => g.disabledWorkTags.HasFlag(WorkTags.Violent)))
				{
					Messages.Message("MessageStartingPawnCapableOfViolence".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return req.ForcedXenotype != xenotype;
		}

		// Token: 0x06011FD1 RID: 73681 RVA: 0x0053F30C File Offset: 0x0053D50C
		[CompilerGenerated]
		internal static bool <LifestageAndXenotypeOptions>g__CustomXenotypeValidator|36_6(PawnGenerationRequest req, CustomXenotype xenotype)
		{
			if (TutorSystem.TutorialMode && req.MustBeCapableOfViolence)
			{
				if (xenotype.genes.Any((GeneDef g) => g.disabledWorkTags.HasFlag(WorkTags.Violent)))
				{
					Messages.Message("MessageStartingPawnCapableOfViolence".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return req.ForcedCustomXenotype != xenotype;
		}

		// Token: 0x0400BA36 RID: 47670
		private static Vector2 leftRectScrollPos = Vector2.zero;

		// Token: 0x0400BA37 RID: 47671
		private static bool warnedChangingXenotypeWillRandomizePawn = false;

		// Token: 0x0400BA38 RID: 47672
		private static Rect highlightRect;

		// Token: 0x0400BA39 RID: 47673
		private const float NonArchiteBaselinerChance = 0.5f;

		// Token: 0x0400BA3A RID: 47674
		public const int MainRectsY = 100;

		// Token: 0x0400BA3B RID: 47675
		private const float MainRectsHeight = 355f;

		// Token: 0x0400BA3C RID: 47676
		private const int ConfigRectTitlesHeight = 40;

		// Token: 0x0400BA3D RID: 47677
		private const int FactionIconSize = 22;

		// Token: 0x0400BA3E RID: 47678
		private const int IdeoIconSize = 22;

		// Token: 0x0400BA3F RID: 47679
		private const int GenderIconSize = 22;

		// Token: 0x0400BA40 RID: 47680
		private const float RowHeight = 22f;

		// Token: 0x0400BA41 RID: 47681
		private const float LeftRectHeight = 250f;

		// Token: 0x0400BA42 RID: 47682
		private const float RightRectHeight = 258f;

		// Token: 0x0400BA43 RID: 47683
		public static Vector2 BasePawnCardSize = new Vector2(480f, 455f);

		// Token: 0x0400BA44 RID: 47684
		private static readonly Color FavColorBoxColor = new Color(0.25f, 0.25f, 0.25f);

		// Token: 0x0400BA45 RID: 47685
		public const int MaxNameLength = 12;

		// Token: 0x0400BA46 RID: 47686
		public const int MaxNickLength = 16;

		// Token: 0x0400BA47 RID: 47687
		public const int MaxTitleLength = 25;

		// Token: 0x0400BA48 RID: 47688
		public const int QuestLineHeight = 20;

		// Token: 0x0400BA49 RID: 47689
		public const float RandomizeButtonWidth = 200f;

		// Token: 0x0400BA4A RID: 47690
		public const float HighlightMargin = 6f;

		// Token: 0x0400BA4B RID: 47691
		private static readonly Texture2D QuestIcon = ContentFinder<Texture2D>.Get("UI/Icons/Quest", true);

		// Token: 0x0400BA4C RID: 47692
		private static readonly Texture2D UnrecruitableIcon = ContentFinder<Texture2D>.Get("UI/Icons/UnwaveringlyLoyal", true);

		// Token: 0x0400BA4D RID: 47693
		public static readonly Color StackElementBackground = new Color(1f, 1f, 1f, 0.1f);

		// Token: 0x0400BA4E RID: 47694
		public static List<CustomXenotype> cachedCustomXenotypes;

		// Token: 0x0400BA4F RID: 47695
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x0400BA50 RID: 47696
		private static readonly Color TitleCausedWorkTagDisableColor = new Color(0.67f, 0.84f, 0.9f);

		// Token: 0x0400BA51 RID: 47697
		private static List<GenUI.AnonymousStackElement> tmpStackElements = new List<GenUI.AnonymousStackElement>();

		// Token: 0x0400BA52 RID: 47698
		private static float tmpMaxElementStackHeight = 0f;

		// Token: 0x0400BA53 RID: 47699
		private static StringBuilder tmpInspectStrings = new StringBuilder();

		// Token: 0x0400BA54 RID: 47700
		public static Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-.]*$");

		// Token: 0x0400BA55 RID: 47701
		private const int QuestIconSize = 24;

		// Token: 0x020031F2 RID: 12786
		private struct LeftRectSection
		{
			// Token: 0x0400BA56 RID: 47702
			public Rect rect;

			// Token: 0x0400BA57 RID: 47703
			public Action<Rect> drawer;

			// Token: 0x0400BA58 RID: 47704
			public float calculatedSize;
		}
	}
}
