using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003238 RID: 12856
	[StaticConstructorOnStartup]
	public static class SocialCardUtility
	{
		// Token: 0x17002EBF RID: 11967
		// (get) Token: 0x06012119 RID: 74009 RVA: 0x00548A10 File Offset: 0x00546C10
		private static float IdeoligionChooseAge
		{
			get
			{
				if (SocialCardUtility.ideoChooseAge == -1f)
				{
					SocialCardUtility.ideoChooseAge = ThingDefOf.Human.race.lifeStageAges.First((LifeStageAge lsa) => lsa.def == LifeStageDefOf.HumanlikeChild).minAge;
				}
				return SocialCardUtility.ideoChooseAge;
			}
		}

		// Token: 0x0601211A RID: 74010 RVA: 0x00548A6C File Offset: 0x00546C6C
		public static void DrawSocialCard(Rect rect, Pawn pawn)
		{
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Prefs.DevMode ? 20f : 15f;
			Rect rect2 = new Rect(0f, num, rect.width, rect.height - num).ContractedBy(10f);
			Rect rect3 = new Rect(0f, 5f, rect.width, 40f);
			Rect rect4 = new Rect(0f, 40f, rect.width, 40f);
			Rect rect5 = rect2;
			Rect rect6 = rect2;
			if (ModsConfig.IdeologyActive && !pawn.Dead && pawn.Ideo != null)
			{
				if (!pawn.Ideo.classicMode)
				{
					rect5.yMin += 40f;
					SocialCardUtility.DrawPawnCertainty(pawn, rect3);
				}
				else
				{
					rect4.y = rect3.y;
				}
				rect5.yMin += 45f;
				Ideo ideo = pawn.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(pawn) : null;
				string label = (precept_Role != null) ? precept_Role.LabelForPawn(pawn) : "NoRoleAssigned".Translate();
				SocialCardUtility.DrawPawnRole(pawn, precept_Role, label, rect4, true);
				SocialCardUtility.DrawPawnRoleSelection(pawn, rect4);
			}
			rect5.height *= 0.63f;
			rect6.y = rect5.yMax + 17f;
			rect6.yMax = rect2.yMax;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(0f, (rect5.yMax + rect6.y) / 2f, rect.width);
			GUI.color = Color.white;
			if (Prefs.DevMode && !pawn.Dead)
			{
				SocialCardUtility.DrawDebugOptions(new Rect(5f, rect5.yMin - 20f, rect.width, 22f), pawn);
			}
			SocialCardUtility.DrawRelationsAndOpinions(rect5, pawn);
			InteractionCardUtility.DrawInteractionsLog(rect6, pawn, Find.PlayLog.AllEntries, 12);
			Widgets.EndGroup();
		}

		// Token: 0x0601211B RID: 74011 RVA: 0x00548C8C File Offset: 0x00546E8C
		private static void CheckRecache(Pawn selPawnForSocialInfo)
		{
			if (SocialCardUtility.cachedForPawn != selPawnForSocialInfo || Time.frameCount % 20 == 0)
			{
				SocialCardUtility.Recache(selPawnForSocialInfo);
			}
		}

		// Token: 0x0601211C RID: 74012 RVA: 0x00548CA8 File Offset: 0x00546EA8
		private static void Recache(Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.cachedForPawn = selPawnForSocialInfo;
			SocialCardUtility.tmpToCache.Clear();
			foreach (Pawn pawn in selPawnForSocialInfo.relations.RelatedPawns)
			{
				if (SocialCardUtility.ShouldShowPawnRelations(pawn, selPawnForSocialInfo))
				{
					SocialCardUtility.RecacheEntry(pawn, selPawnForSocialInfo, null, null);
					SocialCardUtility.tmpToCache.Add(pawn);
				}
			}
			List<Pawn> list = SocialCardUtility.PawnsForSocialInfo(selPawnForSocialInfo);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn2 = list[i];
				if (!SocialCardUtility.tmpToCache.Contains(pawn2))
				{
					SocialCardUtility.RecacheEntry(pawn2, selPawnForSocialInfo, null, null);
					SocialCardUtility.tmpToCache.Add(pawn2);
				}
			}
			SocialCardUtility.cachedEntries.RemoveAll((SocialCardUtility.CachedSocialTabEntry x) => !SocialCardUtility.tmpToCache.Contains(x.otherPawn));
			SocialCardUtility.cachedEntries.Sort(SocialCardUtility.CachedEntriesComparer);
			SocialCardUtility.cachedRoles.Clear();
			if (selPawnForSocialInfo.Ideo != null)
			{
				SocialCardUtility.cachedRoles.AddRange(RitualUtility.AllRolesForPawn(selPawnForSocialInfo));
				SocialCardUtility.cachedRoles.SortBy((Precept_Role x) => x.def.displayOrderInImpact);
			}
		}

		// Token: 0x0601211D RID: 74013 RVA: 0x00548E10 File Offset: 0x00547010
		private static bool ShouldShowPawnRelations(Pawn pawn, Pawn selPawnForSocialInfo)
		{
			return SocialCardUtility.showAllRelations || ((!pawn.RaceProps.Animal || !pawn.Dead || pawn.Corpse != null) && pawn.Name != null && !pawn.Name.Numerical && !pawn.relations.hidePawnRelations && !selPawnForSocialInfo.relations.hidePawnRelations && pawn.relations.everSeenByPlayer);
		}

		// Token: 0x0601211E RID: 74014 RVA: 0x00548E88 File Offset: 0x00547088
		public static List<Pawn> PawnsForSocialInfo(Pawn pawn)
		{
			SocialCardUtility.pawnsForSocialInfoTmp.Clear();
			List<Pawn> list = null;
			if (pawn.MapHeld != null)
			{
				list = pawn.MapHeld.mapPawns.AllPawns;
			}
			else if (pawn.IsCaravanMember())
			{
				list = pawn.GetCaravan().PawnsListForReading;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn2 = list[i];
					if (pawn2.RaceProps.Humanlike && pawn2 != pawn && SocialCardUtility.ShouldShowPawnRelations(pawn2, pawn) && (pawn2.relations.OpinionOf(pawn) != 0 || pawn.relations.OpinionOf(pawn2) != 0))
					{
						SocialCardUtility.pawnsForSocialInfoTmp.AddUnique(pawn2);
					}
				}
			}
			return SocialCardUtility.pawnsForSocialInfoTmp;
		}

		// Token: 0x0601211F RID: 74015 RVA: 0x00548F34 File Offset: 0x00547134
		private static void RecacheEntry(Pawn pawn, Pawn selPawnForSocialInfo, int? opinionOfMe = null, int? opinionOfOtherPawn = null)
		{
			bool flag = false;
			foreach (SocialCardUtility.CachedSocialTabEntry cachedSocialTabEntry in SocialCardUtility.cachedEntries)
			{
				if (cachedSocialTabEntry.otherPawn == pawn)
				{
					SocialCardUtility.RecacheEntryInt(cachedSocialTabEntry, selPawnForSocialInfo, opinionOfMe, opinionOfOtherPawn);
					flag = true;
				}
			}
			if (flag)
			{
				return;
			}
			SocialCardUtility.CachedSocialTabEntry cachedSocialTabEntry2 = new SocialCardUtility.CachedSocialTabEntry();
			cachedSocialTabEntry2.otherPawn = pawn;
			SocialCardUtility.RecacheEntryInt(cachedSocialTabEntry2, selPawnForSocialInfo, opinionOfMe, opinionOfOtherPawn);
			SocialCardUtility.cachedEntries.Add(cachedSocialTabEntry2);
		}

		// Token: 0x06012120 RID: 74016 RVA: 0x00548FBC File Offset: 0x005471BC
		private static void RecacheEntryInt(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo, int? opinionOfMe = null, int? opinionOfOtherPawn = null)
		{
			entry.opinionOfMe = ((opinionOfMe != null) ? opinionOfMe.Value : entry.otherPawn.relations.OpinionOf(selPawnForSocialInfo));
			entry.opinionOfOtherPawn = ((opinionOfOtherPawn != null) ? opinionOfOtherPawn.Value : selPawnForSocialInfo.relations.OpinionOf(entry.otherPawn));
			entry.relations.Clear();
			bool flag = false;
			foreach (PawnRelationDef pawnRelationDef in selPawnForSocialInfo.GetRelations(entry.otherPawn))
			{
				entry.relations.Add(pawnRelationDef);
				if (LovePartnerRelationUtility.IsLovePartnerRelation(pawnRelationDef))
				{
					flag = true;
				}
				if ((selPawnForSocialInfo.IsMutant && selPawnForSocialInfo.mutant.Def.incapableOfSocialInteractions) || (entry.otherPawn.IsMutant && entry.otherPawn.mutant.Def.incapableOfSocialInteractions))
				{
					flag = false;
				}
			}
			entry.relations.Sort((PawnRelationDef a, PawnRelationDef b) => b.importance.CompareTo(a.importance));
			if (Current.ProgramState == ProgramState.Playing && ModsConfig.BiotechActive && flag)
			{
				entry.pregnancyApproach = new PregnancyApproach?(selPawnForSocialInfo.relations.GetPregnancyApproachForPartner(entry.otherPawn));
				return;
			}
			entry.pregnancyApproach = null;
		}

		// Token: 0x06012121 RID: 74017 RVA: 0x00549124 File Offset: 0x00547324
		public static void DrawPawnCertainty(Pawn pawn, Rect rect)
		{
			SocialCardUtility.tmpAgreeableMemeTraitPairs.Clear();
			SocialCardUtility.tmpDisagreeableMemeTraitPairs.Clear();
			float num = rect.x + 17f;
			Rect rect2 = new Rect(num, rect.y + rect.height / 2f - 16f, 32f, 32f);
			pawn.Ideo.DrawIcon(rect2);
			num += 42f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect3 = new Rect(num, rect.y, rect.width / 2f - num, rect.height);
			Widgets.Label(rect3, pawn.Ideo.name.Truncate(rect3.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			num += rect3.width + 10f;
			Rect rect4 = new Rect(rect2.x, rect.y + rect.height / 2f - 16f, 0f, 32f);
			Rect rect5 = new Rect(num, rect.y + rect.height / 2f - 16f, rect.width - num - 26f, 32f);
			rect4.xMax = rect5.xMax;
			if (Mouse.IsOver(rect4))
			{
				Widgets.DrawHighlight(rect4);
				string text = pawn.ideo.CertaintyChangePerDay.ToStringPercent();
				if (pawn.ideo.CertaintyChangePerDay >= 0f)
				{
					text = "+" + text;
				}
				TaggedString tip = "CertaintyInIdeo".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO")) + ": " + pawn.ideo.Certainty.ToStringPercent() + "\n\n" + "CertaintyChangePerDay".Translate() + ": " + text + "\n\n";
				float statValue = pawn.GetStatValue(StatDefOf.CertaintyLossFactor, true, -1);
				if (statValue != 1f)
				{
					tip += StatDefOf.CertaintyLossFactor.LabelCap + ": " + statValue.ToStringPercent();
					foreach (Trait trait in pawn.story.traits.allTraits)
					{
						if (!trait.Suppressed)
						{
							float num2 = trait.MultiplierOfStat(StatDefOf.CertaintyLossFactor);
							if (num2 != 1f)
							{
								tip += "\n -  " + "AbilityIdeoConvertBreakdownTrait".Translate(trait.LabelCap.Named("TRAIT")) + ": x" + num2.ToStringPercent();
							}
						}
					}
					foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
					{
						if (precept.def.statFactors != null)
						{
							float statFactorFromList = precept.def.statFactors.GetStatFactorFromList(StatDefOf.CertaintyLossFactor);
							if (statFactorFromList != 1f)
							{
								tip += "\n -  " + "AbilityIdeoConvertBreakdownPercept".Translate(precept.LabelCap.Named("PRECEPT")) + ": x" + statFactorFromList.ToStringPercent();
							}
						}
					}
					tip += "\n\n";
				}
				string text2 = ConversionUtility.GetCertaintyReductionFactorsDescription(pawn).Resolve();
				if (!text2.NullOrEmpty())
				{
					tip += text2 + "\n\n";
				}
				if (pawn.story.traits != null)
				{
					string text3 = string.Empty;
					foreach (Trait trait2 in pawn.story.traits.allTraits)
					{
						if (!trait2.Suppressed)
						{
							List<MemeDef> affectedMemes = trait2.CurrentData.GetAffectedMemes(trait2.def, true);
							for (int i = 0; i < affectedMemes.Count; i++)
							{
								if (pawn.Ideo.HasMeme(affectedMemes[i]))
								{
									SocialCardUtility.tmpAgreeableMemeTraitPairs.Add(new Pair<Trait, MemeDef>(trait2, affectedMemes[i]));
								}
							}
							List<MemeDef> affectedMemes2 = trait2.CurrentData.GetAffectedMemes(trait2.def, false);
							for (int j = 0; j < affectedMemes2.Count; j++)
							{
								if (pawn.Ideo.HasMeme(affectedMemes2[j]))
								{
									SocialCardUtility.tmpDisagreeableMemeTraitPairs.Add(new Pair<Trait, MemeDef>(trait2, affectedMemes2[j]));
								}
							}
						}
					}
					from x in SocialCardUtility.tmpAgreeableMemeTraitPairs
					orderby x.First.Label
					select x;
					from x in SocialCardUtility.tmpDisagreeableMemeTraitPairs
					orderby x.First.Label
					select x;
					if (SocialCardUtility.tmpAgreeableMemeTraitPairs.Any<Pair<Trait, MemeDef>>())
					{
						text3 += "ConversionWeakerDueToTraitAgreements".Translate() + ":\n" + (from x in SocialCardUtility.tmpAgreeableMemeTraitPairs
						select x.First.LabelCap + "/" + x.Second.LabelCap).ToLineList("  - ", false);
					}
					if (SocialCardUtility.tmpDisagreeableMemeTraitPairs.Any<Pair<Trait, MemeDef>>())
					{
						if (!text3.NullOrEmpty())
						{
							text3 += "\n\n";
						}
						text3 += "ConversionStrongerDueToTraitAgreements".Translate() + ":\n" + (from x in SocialCardUtility.tmpDisagreeableMemeTraitPairs
						select x.First.LabelCap + "/" + x.Second.LabelCap).ToLineList("  - ", false);
					}
					if (!text3.NullOrEmpty())
					{
						tip += text3 + "\n\n";
					}
				}
				string text4 = "MoodChangeRate".Translate() + ": ";
				foreach (CurvePoint curvePoint in ConversionTuning.CertaintyPerDayByMoodCurve.Points)
				{
					string text5 = curvePoint.y.ToStringPercent();
					if (curvePoint.y >= 0f)
					{
						text5 = "+" + text5;
					}
					text4 += "\n -  " + "Mood".Translate() + " " + curvePoint.x.ToStringPercent() + ": " + "PerDay".Translate(text5);
				}
				tip += text4.Colorize(Color.grey);
				TooltipHandler.TipRegion(rect4, () => tip.Resolve(), 10218219);
			}
			if (Widgets.ButtonInvisible(rect4, true))
			{
				IdeoUIUtility.OpenIdeoInfo(pawn.ideo.Ideo);
			}
			Widgets.FillableBar(rect5.ContractedBy(4f), pawn.ideo.Certainty, SocialCardUtility.BarFullTexHor);
		}

		// Token: 0x06012122 RID: 74018 RVA: 0x00549938 File Offset: 0x00547B38
		public static void DrawPawnRole(Pawn pawn, Precept_Role role, string label, Rect rect, bool drawLine = true)
		{
			float num = rect.x + 17f;
			if (role != null)
			{
				float y = rect.y + rect.height / 2f - 16f;
				Rect outerRect = rect;
				outerRect.x = num;
				outerRect.y = y;
				outerRect.width = 32f;
				outerRect.height = 32f;
				GUI.color = role.ideo.Color;
				Widgets.DrawTextureFitted(outerRect, role.Icon, 1f, 1f);
				GUI.color = Color.white;
				num += 42f;
			}
			else
			{
				GUI.color = Color.gray;
			}
			Rect rect2 = new Rect(rect.x + 17f, rect.y + rect.height / 2f - 16f, rect.width - num - SocialCardUtility.RoleChangeButtonSize.x, 32f);
			Rect rect3 = rect;
			rect3.xMin = num;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (Mouse.IsOver(rect2))
			{
				string roleDesc = "RoleDesc".Translate().Resolve();
				if (role != null)
				{
					roleDesc = string.Concat(new string[]
					{
						roleDesc,
						"\n\n",
						role.LabelForPawn(pawn),
						": ",
						role.GetTip()
					});
				}
				Widgets.DrawHighlight(rect2);
				TipSignal tip = new TipSignal(() => roleDesc, pawn.thingIDNumber * 39);
				TooltipHandler.TipRegion(rect2, tip);
			}
			if (drawLine)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.yMax, rect.width);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06012123 RID: 74019 RVA: 0x00549B34 File Offset: 0x00547D34
		public static void DrawPawnRoleSelection(Pawn pawn, Rect rect)
		{
			SocialCardUtility.<>c__DisplayClass42_0 CS$<>8__locals1 = new SocialCardUtility.<>c__DisplayClass42_0();
			CS$<>8__locals1.pawn = pawn;
			if (CS$<>8__locals1.pawn.IsFreeNonSlaveColonist)
			{
				Ideo ideo = CS$<>8__locals1.pawn.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(CS$<>8__locals1.pawn) : null;
				Ideo primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
				SocialCardUtility.<>c__DisplayClass42_0 CS$<>8__locals2 = CS$<>8__locals1;
				Ideo ideo2 = CS$<>8__locals1.pawn.Ideo;
				CS$<>8__locals2.roleChangeRitual = (Precept_Ritual)((ideo2 != null) ? ideo2.GetPrecept(PreceptDefOf.RoleChange) : null);
				CS$<>8__locals1.ritualTarget = CS$<>8__locals1.roleChangeRitual.targetFilter.BestTarget(CS$<>8__locals1.pawn, TargetInfo.Invalid);
				bool flag = SocialCardUtility.cachedRoles.Any<Precept_Role>() && CS$<>8__locals1.pawn.Ideo != null;
				if (!flag)
				{
					GUI.color = Color.gray;
				}
				float y = rect.y + rect.height / 2f - 14f;
				if (Widgets.ButtonText(new Rect(rect.width - 150f, y, SocialCardUtility.RoleChangeButtonSize.x, SocialCardUtility.RoleChangeButtonSize.y)
				{
					xMax = rect.width - 26f - 4f
				}, "ChooseRole".Translate() + "...", true, true, flag, null))
				{
					if (CS$<>8__locals1.ritualTarget.IsValid)
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						if (precept_Role != null)
						{
							list.Add(new FloatMenuOption("RemoveCurrentRole".Translate(), delegate()
							{
								Dialog_BeginRitual dialog_BeginRitual = (Dialog_BeginRitual)CS$<>8__locals1.roleChangeRitual.GetRitualBeginWindow(CS$<>8__locals1.ritualTarget, null, null, CS$<>8__locals1.pawn, new Dictionary<string, Pawn>
								{
									{
										"role_changer",
										CS$<>8__locals1.pawn
									}
								}, null);
								dialog_BeginRitual.SetRoleToChangeTo(null);
								Find.WindowStack.Add(dialog_BeginRitual);
							}, Widgets.PlaceholderIconTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
						using (List<Precept_Role>.Enumerator enumerator = SocialCardUtility.cachedRoles.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								SocialCardUtility.<>c__DisplayClass42_1 CS$<>8__locals3 = new SocialCardUtility.<>c__DisplayClass42_1();
								CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
								CS$<>8__locals3.newRole = enumerator.Current;
								if (CS$<>8__locals3.newRole != precept_Role && CS$<>8__locals3.newRole.Active && CS$<>8__locals3.newRole.RequirementsMet(CS$<>8__locals3.CS$<>8__locals1.pawn) && (!CS$<>8__locals3.newRole.def.leaderRole || CS$<>8__locals3.CS$<>8__locals1.pawn.Ideo == primaryIdeo))
								{
									string text = CS$<>8__locals3.newRole.LabelForPawn(CS$<>8__locals3.CS$<>8__locals1.pawn).CapitalizeFirst();
									if (!CS$<>8__locals3.CS$<>8__locals1.pawn.Ideo.classicMode)
									{
										text = text + " (" + CS$<>8__locals3.newRole.def.label + ")";
									}
									list.Add(new FloatMenuOption(text, delegate()
									{
										Dialog_BeginRitual dialog_BeginRitual = (Dialog_BeginRitual)CS$<>8__locals3.CS$<>8__locals1.roleChangeRitual.GetRitualBeginWindow(CS$<>8__locals3.CS$<>8__locals1.ritualTarget, null, null, CS$<>8__locals3.CS$<>8__locals1.pawn, new Dictionary<string, Pawn>
										{
											{
												"role_changer",
												CS$<>8__locals3.CS$<>8__locals1.pawn
											}
										}, null);
										dialog_BeginRitual.SetRoleToChangeTo(CS$<>8__locals3.newRole);
										Find.WindowStack.Add(dialog_BeginRitual);
									}, CS$<>8__locals3.newRole.Icon, CS$<>8__locals3.newRole.ideo.Color, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals3.<DrawPawnRoleSelection>g__DrawTooltip|1), null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
									{
										orderInPriority = CS$<>8__locals3.newRole.def.displayOrderInImpact
									});
								}
							}
						}
						foreach (Precept_Role precept_Role2 in SocialCardUtility.cachedRoles)
						{
							if ((precept_Role2 != precept_Role && !precept_Role2.RequirementsMet(CS$<>8__locals1.pawn)) || !precept_Role2.Active)
							{
								string text2 = precept_Role2.LabelForPawn(CS$<>8__locals1.pawn) + " (" + precept_Role2.def.label + ")";
								if (precept_Role2.ChosenPawnSingle() != null)
								{
									text2 = text2 + ": " + precept_Role2.ChosenPawnSingle().LabelShort;
								}
								else if (!precept_Role2.RequirementsMet(CS$<>8__locals1.pawn))
								{
									text2 = text2 + ": " + precept_Role2.GetFirstUnmetRequirement(CS$<>8__locals1.pawn).GetLabel(precept_Role2).CapitalizeFirst();
								}
								else if (!precept_Role2.Active && precept_Role2.def.activationBelieverCount > precept_Role2.ideo.ColonistBelieverCountCached)
								{
									text2 += ": " + "InactiveRoleRequiresMoreBelievers".Translate(precept_Role2.def.activationBelieverCount, precept_Role2.ideo.memberName, precept_Role2.ideo.ColonistBelieverCountCached).CapitalizeFirst();
								}
								list.Add(new FloatMenuOption(text2, null, precept_Role2.Icon, precept_Role2.ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
								{
									orderInPriority = precept_Role2.def.displayOrderInImpact
								});
							}
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
					else
					{
						Messages.Message((Find.IdeoManager.classicMode ? "AbilityDisabledNoRitualSpot" : "AbilityDisabledNoAltarIdeogramOrRitualsSpot").Translate(), CS$<>8__locals1.pawn, MessageTypeDefOf.RejectInput, true);
					}
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x06012124 RID: 74020 RVA: 0x0054A094 File Offset: 0x00548294
		private static void DrawIdeoExposure(Pawn baby, float rectWidth, out float heightOffset)
		{
			List<Pawn_IdeoTracker.IdeoExposureWeight> babyIdeoExposureSorted = baby.ideo.BabyIdeoExposureSorted;
			float babyIdeoExposureTotal = baby.ideo.BabyIdeoExposureTotal;
			object obj = (babyIdeoExposureSorted != null) ? babyIdeoExposureSorted.Count : 0;
			float num = rectWidth - 20f - 18f;
			int num2 = Mathf.FloorToInt(num / 93f);
			object obj2 = obj;
			int num3 = Mathf.FloorToInt((obj2 - 1) / num2) + 1;
			float num4 = (obj2 == null) ? 90f : (Mathf.Min((float)num3, 2.5f) * 36f + 10f);
			Rect rect = new Rect(10f, 10f, num, 25f + num4 + 10f);
			GUI.BeginGroup(rect);
			float num5 = 0f;
			Widgets.ListSeparator(ref num5, num, "IdeoExposureSectionHeader".Translate().CapitalizeFirst());
			num5 += 4f;
			TextAnchor anchor = Text.Anchor;
			Rect rect2 = new Rect(0f, num5, num, num4);
			if (obj2 > 0)
			{
				float width = ((float)num3 > 2.5f) ? (num - 16f) : num;
				Rect viewRect = new Rect(0f, num5, width, (float)num3 * 36f + 10f);
				Widgets.BeginScrollView(rect2, ref SocialCardUtility.ideoExposureScrollPosition, viewRect, true);
				Text.Anchor = TextAnchor.MiddleCenter;
				int num6 = 0;
				foreach (Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight in babyIdeoExposureSorted)
				{
					if (num6 > 0 && num6 % num2 == 0)
					{
						num5 += 36f;
					}
					SocialCardUtility.DrawIdeoExposureItem(baby, ideoExposureWeight.ideo, rect, ideoExposureWeight.exposure, babyIdeoExposureTotal, num6 % num2, num5);
					num6++;
				}
				Widgets.EndScrollView();
			}
			else
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Small;
				Widgets.Label(rect2, "IdeoExposureNoExposure".Translate(baby.Named("BABY")));
				Text.Font = GameFont.Tiny;
				rect2.yMin += 30f;
				Widgets.Label(rect2, "IdeoExposureNoExposureDetail".Translate(baby.Named("BABY"), SocialCardUtility.IdeoligionChooseAge.ToStringDecimalIfSmall().Named("CHILDAGE")).CapitalizeFirst());
				Text.Font = font;
			}
			Text.Anchor = anchor;
			GUI.EndGroup();
			heightOffset = rect.height + rect.yMin;
		}

		// Token: 0x06012125 RID: 74021 RVA: 0x0054A2F0 File Offset: 0x005484F0
		private static void DrawIdeoExposureItem(Pawn baby, Ideo ideo, Rect rect, float ideoExposure, float totalExposure, int rowIndex, float yOff)
		{
			float num = rect.x + 93f * (float)rowIndex;
			float f = Mathf.Clamp01(ideoExposure / totalExposure);
			ideo.DrawIcon(new Rect(num, rect.y + yOff, 32f, 32f).ContractedBy(4f));
			Widgets.Label(new Rect(num + 32f + 4f, rect.y + yOff, 40f, 32f), f.ToStringPercent());
			Rect rect2 = new Rect(num, rect.y + yOff, 80f, 32f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TaggedString str = "IdeoExposurePointsTooltipJoinLikelihood".Translate(baby.Named("BABY"), f.ToStringPercent().Named("PERCENT"), ideo.Named("IDEO")) + "\n\n" + "IdeoExposurePointsTooltipJoinDescription".Translate(baby.Named("BABY"), SocialCardUtility.IdeoligionChooseAge.ToStringDecimalIfSmall().Named("CHILDAGE")) + "\n\n" + "IdeoExposurePointsTooltipExposureDescription".Translate(baby.Named("BABY")) + "\n\n" + "IdeoExposurePointsTooltipIdeoExposure".Translate(ideo.Named("IDEO")) + ": " + ideoExposure.ToStringDecimalIfSmall() + "\n" + "IdeoExposurePointsTooltipTotalExposure".Translate() + ": " + totalExposure.ToStringDecimalIfSmall();
				TooltipHandler.TipRegion(rect2, str);
				if (Widgets.ButtonInvisible(rect2, true))
				{
					IdeoUIUtility.OpenIdeoInfo(ideo);
				}
			}
		}

		// Token: 0x06012126 RID: 74022 RVA: 0x0054A4B1 File Offset: 0x005486B1
		public static bool AnyRelations(Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.CheckRecache(selPawnForSocialInfo);
			return SocialCardUtility.cachedEntries.Any<SocialCardUtility.CachedSocialTabEntry>();
		}

		// Token: 0x06012127 RID: 74023 RVA: 0x0054A4C4 File Offset: 0x005486C4
		public static void DrawRelationsAndOpinions(Rect rect, Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.CheckRecache(selPawnForSocialInfo);
			if (Current.ProgramState != ProgramState.Playing)
			{
				SocialCardUtility.showAllRelations = false;
			}
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			bool flag = SocialCardUtility.CanDrawTryRomance(selPawnForSocialInfo);
			float num = flag ? (SocialCardUtility.RoleChangeButtonSize.y + 10f) : 0f;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - num);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, SocialCardUtility.listScrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref SocialCardUtility.listScrollPosition, viewRect, true);
			float num2 = 0f;
			float y = SocialCardUtility.listScrollPosition.y;
			float num3 = SocialCardUtility.listScrollPosition.y + outRect.height;
			for (int i = 0; i < SocialCardUtility.cachedEntries.Count; i++)
			{
				float rowHeight = SocialCardUtility.GetRowHeight(SocialCardUtility.cachedEntries[i], rect2.width, selPawnForSocialInfo);
				if (num2 > y - rowHeight && num2 < num3)
				{
					SocialCardUtility.DrawPawnRow(num2, rect2.width, SocialCardUtility.cachedEntries[i], selPawnForSocialInfo);
				}
				num2 += rowHeight;
			}
			if (Event.current.type == EventType.Layout)
			{
				SocialCardUtility.listScrollViewHeight = num2 + 30f;
			}
			Widgets.EndScrollView();
			if (flag)
			{
				SocialCardUtility.DrawTryRomance(new Rect(rect.width - 150f + 10f, rect.height - SocialCardUtility.RoleChangeButtonSize.y, SocialCardUtility.RoleChangeButtonSize.x, SocialCardUtility.RoleChangeButtonSize.y), selPawnForSocialInfo);
			}
			Widgets.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x06012128 RID: 74024 RVA: 0x0054A695 File Offset: 0x00548895
		private static bool CanDrawTryRomance(Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.ageTracker.AgeBiologicalYearsFloat >= 16f && pawn.Spawned && pawn.IsFreeColonist;
		}

		// Token: 0x06012129 RID: 74025 RVA: 0x0054A6C0 File Offset: 0x005488C0
		private static void DrawTryRomance(Rect buttonRect, Pawn pawn)
		{
			Color color = GUI.color;
			bool isTryRomanceOnCooldown = pawn.relations.IsTryRomanceOnCooldown;
			AcceptanceReport acceptanceReport = RelationsUtility.RomanceEligible(pawn, true, false);
			List<FloatMenuOption> list = acceptanceReport.Accepted ? SocialCardUtility.RomanceOptions(pawn) : null;
			GUI.color = ((!acceptanceReport.Accepted || list.NullOrEmpty<FloatMenuOption>() || isTryRomanceOnCooldown) ? ColoredText.SubtleGrayColor : Color.white);
			if (Widgets.ButtonText(buttonRect, "TryRomanceButtonLabel".Translate() + "...", true, true, true, null))
			{
				if (isTryRomanceOnCooldown)
				{
					int numTicks = pawn.relations.romanceEnableTick - Find.TickManager.TicksGame;
					Messages.Message("CantRomanceInitiateMessageCooldown".Translate(pawn, numTicks.ToStringTicksToPeriod(true, false, true, true, false)), MessageTypeDefOf.RejectInput, false);
					return;
				}
				if (!acceptanceReport.Accepted)
				{
					if (!acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					}
					return;
				}
				if (list.NullOrEmpty<FloatMenuOption>())
				{
					Messages.Message("TryRomanceNoOptsMessage".Translate(pawn), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			GUI.color = color;
		}

		// Token: 0x0601212A RID: 74026 RVA: 0x0054A80C File Offset: 0x00548A0C
		private static List<FloatMenuOption> RomanceOptions(Pawn romancer)
		{
			List<ValueTuple<float, FloatMenuOption>> list = new List<ValueTuple<float, FloatMenuOption>>();
			List<FloatMenuOption> list2 = new List<FloatMenuOption>();
			foreach (Pawn romanceTarget in romancer.Map.mapPawns.FreeColonistsSpawned)
			{
				FloatMenuOption floatMenuOption;
				float item;
				if (RelationsUtility.RomanceOption(romancer, romanceTarget, out floatMenuOption, out item))
				{
					list.Add(new ValueTuple<float, FloatMenuOption>(item, floatMenuOption));
				}
				else if (floatMenuOption != null)
				{
					list2.Add(floatMenuOption);
				}
			}
			return (from pair in list
			orderby pair.Item1 descending
			select pair.Item2).Concat(from opt in list2
			orderby opt.Label
			select opt).ToList<FloatMenuOption>();
		}

		// Token: 0x0601212B RID: 74027 RVA: 0x0054A910 File Offset: 0x00548B10
		private static void DrawPawnRow(float y, float width, SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			float rowHeight = SocialCardUtility.GetRowHeight(entry, width, selPawnForSocialInfo);
			Rect rect = new Rect(0f, y, width, rowHeight);
			Pawn otherPawn = entry.otherPawn;
			if (Mouse.IsOver(rect))
			{
				GUI.color = SocialCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			Rect rect2 = rect;
			if (entry.pregnancyApproach != null)
			{
				rect2.xMax -= 28f;
			}
			if (Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, () => SocialCardUtility.GetPawnRowTooltip(entry, selPawnForSocialInfo), entry.otherPawn.thingIDNumber * 13 + selPawnForSocialInfo.thingIDNumber);
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (otherPawn.Dead)
					{
						Messages.Message("MessageCantSelectDeadPawn".Translate(otherPawn.LabelShort, otherPawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
					else if (otherPawn.SpawnedOrAnyParentSpawned || otherPawn.IsCaravanMember())
					{
						CameraJumper.TryJumpAndSelect(otherPawn, CameraJumper.MovementMode.Pan);
					}
					else
					{
						Messages.Message("MessageCantSelectOffMapPawn".Translate(otherPawn.LabelShort, otherPawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
				}
				else if (Find.GameInitData.startingAndOptionalPawns.Contains(otherPawn))
				{
					Page_ConfigureStartingPawns page_ConfigureStartingPawns = Find.WindowStack.WindowOfType<Page_ConfigureStartingPawns>();
					if (page_ConfigureStartingPawns != null)
					{
						page_ConfigureStartingPawns.SelectPawn(otherPawn);
						SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
					}
				}
			}
			float width2;
			float width3;
			float num;
			float num2;
			float num3;
			SocialCardUtility.CalculateColumnsWidths(width, out width2, out width3, out num, out num2, out num3);
			if (Current.ProgramState != ProgramState.Playing)
			{
				Rect rect3 = new Rect(5f, y + 3f, width - num - num2 - 5f, rowHeight - 3f);
				SocialCardUtility.DrawPawnAndRelationLabel(entry, otherPawn, rect3, selPawnForSocialInfo);
				Rect rect4 = new Rect(width - num - num2, y + 3f, num, rowHeight - 3f);
				SocialCardUtility.DrawMyOpinion(entry, rect4, selPawnForSocialInfo);
				Rect rect5 = new Rect(rect4.xMax, y + 3f, num2, rowHeight - 3f);
				SocialCardUtility.DrawHisOpinion(entry, rect5, selPawnForSocialInfo);
				return;
			}
			if (entry.pregnancyApproach != null)
			{
				num3 -= 28f;
			}
			Rect rect6 = new Rect(5f, y + 3f, width2, rowHeight - 3f);
			SocialCardUtility.DrawRelationLabel(entry, rect6, selPawnForSocialInfo);
			Rect rect7 = new Rect(rect6.xMax, y + 3f, width3, rowHeight - 3f);
			SocialCardUtility.DrawPawnLabel(otherPawn, rect7);
			Rect rect8 = new Rect(rect7.xMax, y + 3f, num, rowHeight - 3f);
			SocialCardUtility.DrawMyOpinion(entry, rect8, selPawnForSocialInfo);
			Rect rect9 = new Rect(rect8.xMax, y + 3f, num2, rowHeight - 3f);
			SocialCardUtility.DrawHisOpinion(entry, rect9, selPawnForSocialInfo);
			Rect rect10 = new Rect(rect9.xMax, y + 3f, num3, rowHeight - 3f);
			SocialCardUtility.DrawPawnSituationLabel(entry.otherPawn, rect10, selPawnForSocialInfo);
			if (entry.pregnancyApproach != null)
			{
				SocialCardUtility.DrawPregnancyApproach(entry, new Rect(rect10.xMax + 4f, y + 3f, 24f, 24f), selPawnForSocialInfo);
			}
		}

		// Token: 0x0601212C RID: 74028 RVA: 0x0054ACCC File Offset: 0x00548ECC
		private static float GetRowHeight(SocialCardUtility.CachedSocialTabEntry entry, float rowWidth, Pawn selPawnForSocialInfo)
		{
			float width;
			float width2;
			float num;
			float num2;
			float num3;
			SocialCardUtility.CalculateColumnsWidths(rowWidth, out width, out width2, out num, out num2, out num3);
			float num4 = 0f;
			if (Current.ProgramState != ProgramState.Playing)
			{
				float width3 = rowWidth - num2 - num - 10f;
				num4 = Text.CalcHeight(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo) + ": " + SocialCardUtility.GetPawnLabel(entry.otherPawn), width3);
			}
			else
			{
				num4 = Mathf.Max(num4, Text.CalcHeight(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo), width));
				num4 = Mathf.Max(num4, Text.CalcHeight(SocialCardUtility.GetPawnLabel(entry.otherPawn), width2));
			}
			return num4 + 3f;
		}

		// Token: 0x0601212D RID: 74029 RVA: 0x0054AD64 File Offset: 0x00548F64
		private static void CalculateColumnsWidths(float rowWidth, out float relationsWidth, out float pawnLabelWidth, out float myOpinionWidth, out float hisOpinionWidth, out float pawnSituationLabelWidth)
		{
			float num = rowWidth - 10f;
			relationsWidth = num * 0.23f;
			pawnLabelWidth = num * 0.41f;
			myOpinionWidth = num * 0.075f;
			hisOpinionWidth = num * 0.085f;
			pawnSituationLabelWidth = num * 0.2f;
			if (myOpinionWidth < 25f)
			{
				pawnLabelWidth -= 25f - myOpinionWidth;
				myOpinionWidth = 25f;
			}
			if (hisOpinionWidth < 35f)
			{
				pawnLabelWidth -= 35f - hisOpinionWidth;
				hisOpinionWidth = 35f;
			}
		}

		// Token: 0x0601212E RID: 74030 RVA: 0x0054ADE8 File Offset: 0x00548FE8
		private static void DrawRelationLabel(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			string relationsString = SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo);
			if (!relationsString.NullOrEmpty())
			{
				GUI.color = SocialCardUtility.RelationLabelColor;
				Widgets.Label(rect, relationsString);
			}
		}

		// Token: 0x0601212F RID: 74031 RVA: 0x0054AE16 File Offset: 0x00549016
		private static void DrawPawnLabel(Pawn pawn, Rect rect)
		{
			GUI.color = SocialCardUtility.PawnLabelColor;
			Widgets.Label(rect, SocialCardUtility.GetPawnLabel(pawn));
		}

		// Token: 0x06012130 RID: 74032 RVA: 0x0054AE2E File Offset: 0x0054902E
		private static void DrawPawnAndRelationLabel(SocialCardUtility.CachedSocialTabEntry entry, Pawn pawn, Rect rect, Pawn selPawnForSocialInfo)
		{
			Widgets.Label(rect, (SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo) + ": ").Colorize(SocialCardUtility.RelationLabelColor) + SocialCardUtility.GetPawnLabel(pawn).Colorize(SocialCardUtility.PawnLabelColor));
		}

		// Token: 0x06012131 RID: 74033 RVA: 0x0054AE68 File Offset: 0x00549068
		private static void DrawMyOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (!entry.otherPawn.RaceProps.Humanlike || !selPawnForSocialInfo.RaceProps.Humanlike)
			{
				return;
			}
			int opinionOfOtherPawn = entry.opinionOfOtherPawn;
			GUI.color = SocialCardUtility.OpinionLabelColor(opinionOfOtherPawn);
			Widgets.Label(rect, opinionOfOtherPawn.ToStringWithSign());
		}

		// Token: 0x06012132 RID: 74034 RVA: 0x0054AEB4 File Offset: 0x005490B4
		private static void DrawHisOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (!entry.otherPawn.RaceProps.Humanlike || !selPawnForSocialInfo.RaceProps.Humanlike)
			{
				return;
			}
			int opinionOfMe = entry.opinionOfMe;
			Color color = SocialCardUtility.OpinionLabelColor(opinionOfMe);
			GUI.color = new Color(color.r, color.g, color.b, 0.4f);
			Widgets.Label(rect, "(" + opinionOfMe.ToStringWithSign() + ")");
		}

		// Token: 0x06012133 RID: 74035 RVA: 0x0054AF2C File Offset: 0x0054912C
		private static void DrawPawnSituationLabel(Pawn pawn, Rect rect, Pawn selPawnForSocialInfo)
		{
			GUI.color = Color.gray;
			string label = SocialCardUtility.GetPawnSituationLabel(pawn, selPawnForSocialInfo).Truncate(rect.width, null);
			Widgets.Label(rect, label);
		}

		// Token: 0x06012134 RID: 74036 RVA: 0x0054AF60 File Offset: 0x00549160
		private static void DrawPregnancyApproach(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (entry.pregnancyApproach == null || !Find.Storyteller.difficulty.ChildrenAllowed)
			{
				return;
			}
			GUI.color = Color.white;
			PregnancyApproach value = entry.pregnancyApproach.Value;
			AcceptanceReport report = PregnancyUtility.CanEverProduceChild(selPawnForSocialInfo, entry.otherPawn);
			if (selPawnForSocialInfo.IsWorldPawn())
			{
				report = "PawnIsAway".Translate(selPawnForSocialInfo.Named("PAWN"));
			}
			if (!report.Accepted)
			{
				GUI.color = Color.grey;
			}
			GUI.DrawTexture(rect, value.GetIcon());
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (report.Accepted)
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (object obj in Enum.GetValues(typeof(PregnancyApproach)))
					{
						PregnancyApproach pregnancyApproach = (PregnancyApproach)obj;
						PregnancyApproach rmLocal = pregnancyApproach;
						list.Add(new FloatMenuOption(rmLocal.GetDescription(), delegate()
						{
							selPawnForSocialInfo.relations.SetPregnancyApproach(entry.otherPawn, rmLocal);
						}, pregnancyApproach.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				else
				{
					Messages.Message("PregnancyNotPossible".Translate(), null, MessageTypeDefOf.RejectInput, false);
				}
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, report ? string.Concat(new string[]
				{
					"PregnancyApproach".Translate().Colorize(ColoredText.TipSectionTitleColor),
					"\n",
					value.GetDescription(),
					"\n\n",
					"ClickToChangePregnancyApproach".Translate().Colorize(ColoredText.SubtleGrayColor)
				}) : ("PregnancyNotPossible".Translate().Resolve() + ": " + report.Reason.CapitalizeFirst()));
			}
		}

		// Token: 0x06012135 RID: 74037 RVA: 0x0054B1C0 File Offset: 0x005493C0
		private static Color OpinionLabelColor(int opinion)
		{
			if (Mathf.Abs(opinion) < 10)
			{
				return Color.gray;
			}
			if (opinion < 0)
			{
				return ColorLibrary.RedReadable;
			}
			return Color.green;
		}

		// Token: 0x06012136 RID: 74038 RVA: 0x0054B1E1 File Offset: 0x005493E1
		private static string GetPawnLabel(Pawn pawn)
		{
			if (pawn.Name != null)
			{
				return pawn.Name.ToStringFull;
			}
			return pawn.LabelCapNoCount;
		}

		// Token: 0x06012137 RID: 74039 RVA: 0x0054B200 File Offset: 0x00549400
		public static string GetPawnSituationLabel(Pawn pawn, Pawn fromPOV)
		{
			if (pawn.Dead)
			{
				return "Dead".Translate();
			}
			if (pawn.Destroyed)
			{
				return "Missing".Translate();
			}
			if (PawnUtility.IsKidnappedPawn(pawn))
			{
				return "Kidnapped".Translate();
			}
			QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = QuestUtility.GetAllQuestPartsOfType<QuestPart_LendColonistsToFaction>(true).FirstOrDefault((QuestPart_LendColonistsToFaction p) => p.LentColonistsListForReading.Contains(pawn));
			if (questPart_LendColonistsToFaction != null)
			{
				return "Lent".Translate(questPart_LendColonistsToFaction.lendColonistsToFaction.Named("FACTION"), questPart_LendColonistsToFaction.ReturnPawnsInDurationTicks.ToStringTicksToDays("0.0")).Resolve();
			}
			if (pawn.kindDef == PawnKindDefOf.Slave)
			{
				return "Slave".Translate().CapitalizeFirst();
			}
			if (PawnUtility.IsFactionLeader(pawn))
			{
				return "FactionLeader".Translate();
			}
			Faction faction = pawn.Faction;
			if (faction == fromPOV.Faction)
			{
				return "";
			}
			if (faction == null || fromPOV.Faction == null)
			{
				return "Neutral".Translate();
			}
			switch (faction.RelationKindWith(fromPOV.Faction))
			{
			case FactionRelationKind.Hostile:
				return "Hostile".Translate() + ", " + faction.Name;
			case FactionRelationKind.Neutral:
				return "Neutral".Translate() + ", " + faction.Name;
			case FactionRelationKind.Ally:
				return "Ally".Translate() + ", " + faction.Name;
			default:
				return "";
			}
		}

		// Token: 0x06012138 RID: 74040 RVA: 0x0054B3DC File Offset: 0x005495DC
		private static string GetRelationsString(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			string text = "";
			if (entry.relations.Count != 0)
			{
				for (int i = 0; i < entry.relations.Count; i++)
				{
					PawnRelationDef pawnRelationDef = entry.relations[i];
					if (!text.NullOrEmpty())
					{
						text = text + ", " + pawnRelationDef.GetGenderSpecificLabel(entry.otherPawn);
					}
					else
					{
						text = pawnRelationDef.GetGenderSpecificLabelCap(entry.otherPawn);
					}
				}
				return text;
			}
			if (entry.opinionOfOtherPawn < -20)
			{
				return "Rival".Translate();
			}
			if (entry.opinionOfOtherPawn > 20)
			{
				return "Friend".Translate();
			}
			return "Acquaintance".Translate();
		}

		// Token: 0x06012139 RID: 74041 RVA: 0x0054B494 File Offset: 0x00549694
		private static string GetPawnRowTooltip(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				stringBuilder.AppendLine(selPawnForSocialInfo.relations.OpinionExplanation(entry.otherPawn));
				stringBuilder.AppendLine();
				string text = SocialCardUtility.RomanceExplanation(selPawnForSocialInfo, entry.otherPawn);
				if (!text.NullOrEmpty())
				{
					stringBuilder.AppendLine(text);
				}
				stringBuilder.Append(("SomeonesOpinionOfMe".Translate(entry.otherPawn.LabelShort) + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.Append(entry.opinionOfMe.ToStringWithSign());
			}
			else
			{
				stringBuilder.Append(entry.otherPawn.LabelCapNoCount);
				string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(entry.otherPawn, selPawnForSocialInfo);
				if (!pawnSituationLabel.NullOrEmpty())
				{
					stringBuilder.AppendLine(" (" + pawnSituationLabel + ")");
				}
				else
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(" - " + SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo));
			}
			if (Prefs.DevMode)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("(debug) Compatibility: " + selPawnForSocialInfo.relations.CompatibilityWith(entry.otherPawn).ToString("F2"));
				stringBuilder.Append("(debug) RomanceChanceFactor: " + selPawnForSocialInfo.relations.SecondaryRomanceChanceFactor(entry.otherPawn).ToString("F2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601213A RID: 74042 RVA: 0x0054B624 File Offset: 0x00549824
		private static string RomanceExplanation(Pawn romancer, Pawn romanceTarget)
		{
			if (!SocialCardUtility.CanDrawTryRomance(romancer))
			{
				return null;
			}
			AcceptanceReport acceptanceReport = RelationsUtility.RomanceEligiblePair(romancer, romanceTarget, true);
			if (!acceptanceReport.Accepted && acceptanceReport.Reason.NullOrEmpty())
			{
				return null;
			}
			if (!acceptanceReport.Accepted)
			{
				return "RomanceChanceCant".Translate() + (" (" + acceptanceReport.Reason + ")\n");
			}
			StringBuilder stringBuilder = new StringBuilder();
			float f = InteractionWorker_RomanceAttempt.SuccessChance(romancer, romanceTarget, 1f);
			stringBuilder.AppendLine(("RomanceChance".Translate() + (": " + f.ToStringPercent())).Colorize(ColoredText.TipSectionTitleColor));
			stringBuilder.Append(InteractionWorker_RomanceAttempt.RomanceFactors(romancer, romanceTarget));
			return stringBuilder.ToString();
		}

		// Token: 0x0601213B RID: 74043 RVA: 0x0054B6E6 File Offset: 0x005498E6
		private static void DrawDebugOptions(Rect rect, Pawn pawn)
		{
			Widgets.BeginGroup(rect);
			Widgets.CheckboxLabeled(new Rect(0f, 0f, 145f, 22f), "DEV: AllRelations", ref SocialCardUtility.showAllRelations, false, null, null, false, false);
			Widgets.EndGroup();
		}

		// Token: 0x0601213C RID: 74044 RVA: 0x0054B720 File Offset: 0x00549920
		public static void ClearCaches()
		{
			SocialCardUtility.cachedForPawn = null;
			SocialCardUtility.cachedEntries.Clear();
			SocialCardUtility.cachedRoles.Clear();
			SocialCardUtility.tmpToCache.Clear();
			SocialCardUtility.tmpDisagreeableMemeTraitPairs.Clear();
			SocialCardUtility.tmpAgreeableMemeTraitPairs.Clear();
			SocialCardUtility.pawnsForSocialInfoTmp.Clear();
		}

		// Token: 0x0400BB96 RID: 48022
		private static Vector2 listScrollPosition = Vector2.zero;

		// Token: 0x0400BB97 RID: 48023
		private static Vector2 ideoExposureScrollPosition = Vector2.zero;

		// Token: 0x0400BB98 RID: 48024
		private static float listScrollViewHeight = 0f;

		// Token: 0x0400BB99 RID: 48025
		private static bool showAllRelations;

		// Token: 0x0400BB9A RID: 48026
		private static List<SocialCardUtility.CachedSocialTabEntry> cachedEntries = new List<SocialCardUtility.CachedSocialTabEntry>();

		// Token: 0x0400BB9B RID: 48027
		private static List<Precept_Role> cachedRoles = new List<Precept_Role>();

		// Token: 0x0400BB9C RID: 48028
		private static Pawn cachedForPawn;

		// Token: 0x0400BB9D RID: 48029
		private const float TopPadding = 15f;

		// Token: 0x0400BB9E RID: 48030
		private const float TopPaddingDevMode = 20f;

		// Token: 0x0400BB9F RID: 48031
		private static readonly Color RelationLabelColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x0400BBA0 RID: 48032
		private static readonly Color PawnLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x0400BBA1 RID: 48033
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x0400BBA2 RID: 48034
		private const float RowTopPadding = 3f;

		// Token: 0x0400BBA3 RID: 48035
		private const float RowLeftRightPadding = 5f;

		// Token: 0x0400BBA4 RID: 48036
		private const float IconSize = 32f;

		// Token: 0x0400BBA5 RID: 48037
		private static SocialCardUtility.CachedSocialTabEntryComparer CachedEntriesComparer = new SocialCardUtility.CachedSocialTabEntryComparer();

		// Token: 0x0400BBA6 RID: 48038
		private static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(GenUI.FillableBar_Green);

		// Token: 0x0400BBA7 RID: 48039
		private static readonly Vector2 RoleChangeButtonSize = new Vector2(115f, 28f);

		// Token: 0x0400BBA8 RID: 48040
		private const float IdeoExposurePctWidth = 40f;

		// Token: 0x0400BBA9 RID: 48041
		private const float IdeoExposureWidth = 93f;

		// Token: 0x0400BBAA RID: 48042
		private const float IdeoExposureHeight = 36f;

		// Token: 0x0400BBAB RID: 48043
		private const float IdeoExposureMaxRowsBeforeScroll = 2.5f;

		// Token: 0x0400BBAC RID: 48044
		private const float IdeoExposureHighlightMargin = 4f;

		// Token: 0x0400BBAD RID: 48045
		private const float IdeoExposureDetailSpace = 30f;

		// Token: 0x0400BBAE RID: 48046
		private static float ideoChooseAge = -1f;

		// Token: 0x0400BBAF RID: 48047
		private static HashSet<Pawn> tmpToCache = new HashSet<Pawn>();

		// Token: 0x0400BBB0 RID: 48048
		private static List<Pawn> pawnsForSocialInfoTmp = new List<Pawn>();

		// Token: 0x0400BBB1 RID: 48049
		public static List<Pair<Trait, MemeDef>> tmpAgreeableMemeTraitPairs = new List<Pair<Trait, MemeDef>>();

		// Token: 0x0400BBB2 RID: 48050
		public static List<Pair<Trait, MemeDef>> tmpDisagreeableMemeTraitPairs = new List<Pair<Trait, MemeDef>>();

		// Token: 0x02003239 RID: 12857
		private class CachedSocialTabEntry
		{
			// Token: 0x0400BBB3 RID: 48051
			public Pawn otherPawn;

			// Token: 0x0400BBB4 RID: 48052
			public int opinionOfOtherPawn;

			// Token: 0x0400BBB5 RID: 48053
			public int opinionOfMe;

			// Token: 0x0400BBB6 RID: 48054
			public List<PawnRelationDef> relations = new List<PawnRelationDef>();

			// Token: 0x0400BBB7 RID: 48055
			public PregnancyApproach? pregnancyApproach;
		}

		// Token: 0x0200323A RID: 12858
		private class CachedSocialTabEntryComparer : IComparer<SocialCardUtility.CachedSocialTabEntry>
		{
			// Token: 0x0601213F RID: 74047 RVA: 0x0054B878 File Offset: 0x00549A78
			public int Compare(SocialCardUtility.CachedSocialTabEntry a, SocialCardUtility.CachedSocialTabEntry b)
			{
				bool flag = a.relations.Any<PawnRelationDef>();
				bool flag2 = b.relations.Any<PawnRelationDef>();
				if (flag != flag2)
				{
					return flag2.CompareTo(flag);
				}
				if (flag && flag2)
				{
					float num = float.MinValue;
					for (int i = 0; i < a.relations.Count; i++)
					{
						if (a.relations[i].importance > num)
						{
							num = a.relations[i].importance;
						}
					}
					float num2 = float.MinValue;
					for (int j = 0; j < b.relations.Count; j++)
					{
						if (b.relations[j].importance > num2)
						{
							num2 = b.relations[j].importance;
						}
					}
					if (num != num2)
					{
						return num2.CompareTo(num);
					}
				}
				if (a.opinionOfOtherPawn != b.opinionOfOtherPawn)
				{
					return b.opinionOfOtherPawn.CompareTo(a.opinionOfOtherPawn);
				}
				return a.otherPawn.thingIDNumber.CompareTo(b.otherPawn.thingIDNumber);
			}
		}
	}
}
