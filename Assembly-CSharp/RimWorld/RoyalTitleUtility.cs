using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld.QuestGen;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020036AC RID: 13996
	public static class RoyalTitleUtility
	{
		// Token: 0x060136F9 RID: 79609 RVA: 0x005C605C File Offset: 0x005C425C
		public static void FindLostAndGainedPermits(RoyalTitleDef currentTitle, RoyalTitleDef newTitle, out List<RoyalTitlePermitDef> gainedPermits, out List<RoyalTitlePermitDef> lostPermits)
		{
			gainedPermits = new List<RoyalTitlePermitDef>();
			lostPermits = new List<RoyalTitlePermitDef>();
			if (newTitle != null && newTitle.permits != null)
			{
				foreach (RoyalTitlePermitDef item in newTitle.permits)
				{
					if (currentTitle == null || currentTitle.permits == null || !currentTitle.permits.Contains(item))
					{
						gainedPermits.Add(item);
					}
				}
			}
			if (currentTitle != null && currentTitle.permits != null)
			{
				foreach (RoyalTitlePermitDef item2 in currentTitle.permits)
				{
					if (newTitle == null || newTitle.permits == null || !newTitle.permits.Contains(item2))
					{
						lostPermits.Add(item2);
					}
				}
			}
		}

		// Token: 0x060136FA RID: 79610 RVA: 0x005C614C File Offset: 0x005C434C
		public static string BuildDifferenceExplanationText(RoyalTitleDef currentTitle, RoyalTitleDef newTitle, Faction faction, Pawn pawn)
		{
			RoyalTitleUtility.<>c__DisplayClass1_0 CS$<>8__locals1 = new RoyalTitleUtility.<>c__DisplayClass1_0();
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.newTitle = newTitle;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(CS$<>8__locals1.pawn);
			List<WorkTags> list = CS$<>8__locals1.pawn.story.DisabledWorkTagsBackstoryTraitsAndGenes.GetAllSelectedItems<WorkTags>().ToList<WorkTags>();
			List<WorkTags> list2 = (CS$<>8__locals1.newTitle == null) ? new List<WorkTags>() : CS$<>8__locals1.newTitle.disabledWorkTags.GetAllSelectedItems<WorkTags>().ToList<WorkTags>();
			List<WorkTags> list3 = new List<WorkTags>();
			foreach (WorkTags item in list2)
			{
				if (!list.Contains(item))
				{
					list3.Add(item);
				}
			}
			int num = (CS$<>8__locals1.newTitle != null) ? CS$<>8__locals1.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(CS$<>8__locals1.newTitle) : -1;
			if (CS$<>8__locals1.newTitle != null && flag)
			{
				stringBuilder.AppendLineTagged("LetterRoyalTitleConceitedTrait".Translate(CS$<>8__locals1.pawn.Named("PAWN"), (from t in RoyalTitleUtility.GetConceitedTraits(CS$<>8__locals1.pawn)
				select t.Label).ToCommaList(true, false)));
				stringBuilder.AppendLine();
				if (CS$<>8__locals1.newTitle.minExpectation != null)
				{
					stringBuilder.AppendLineTagged("LetterRoyalTitleExpectation".Translate(CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals1.newTitle.minExpectation.label).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
			}
			if (CS$<>8__locals1.newTitle != null)
			{
				if (CS$<>8__locals1.newTitle.canBeInherited)
				{
					Pawn heir = CS$<>8__locals1.pawn.royalty.GetHeir(CS$<>8__locals1.faction);
					TaggedString taggedString = (heir != null) ? "LetterRoyalTitleHeir".Translate(CS$<>8__locals1.pawn.Named("PAWN"), heir.Named("HEIR")) : "LetterRoyalTitleNoHeir".Translate(CS$<>8__locals1.pawn.Named("PAWN"));
					stringBuilder.AppendTagged(taggedString);
					if (heir != null && heir.Faction != Faction.OfPlayer)
					{
						stringBuilder.AppendTagged(" " + "LetterRoyalTitleHeirFactionWarning".Translate(heir.Named("PAWN"), CS$<>8__locals1.faction.Named("FACTION")));
					}
					stringBuilder.AppendLineTagged(" " + "LetterRoyalTitleChangingHeir".Translate(CS$<>8__locals1.faction.Named("FACTION")));
				}
				else
				{
					stringBuilder.AppendTagged("LetterRoyalTitleCantBeInherited".Translate(CS$<>8__locals1.newTitle.Named("TITLE")).CapitalizeFirst());
					stringBuilder.AppendTagged(" " + "LetterRoyalTitleNoHeir".Translate(CS$<>8__locals1.pawn.Named("PAWN")));
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine();
				if (CS$<>8__locals1.newTitle.permitPointsAwarded > 0 && CS$<>8__locals1.pawn.royalty.NewHighestTitle(CS$<>8__locals1.faction, CS$<>8__locals1.newTitle))
				{
					stringBuilder.AppendLine("PermitPointsAwarded".Translate(CS$<>8__locals1.newTitle.permitPointsAwarded));
					stringBuilder.AppendLine();
				}
			}
			if (flag && list3.Count > 0)
			{
				stringBuilder.AppendLine("LetterRoyalTitleDisabledWorkTag".Translate(CS$<>8__locals1.pawn.Named("PAWN"), (from t in list3
				orderby base.<BuildDifferenceExplanationText>g__FirstTitleDisablingWorkTags|1(t).seniority
				select t.LabelTranslated() + " (" + base.<BuildDifferenceExplanationText>g__FirstTitleDisablingWorkTags|1(t).GetLabelFor(CS$<>8__locals1.pawn) + ")").ToLineList("- ", false)).CapitalizeFirst());
				stringBuilder.AppendLine();
			}
			if (CS$<>8__locals1.newTitle != null)
			{
				if (CS$<>8__locals1.newTitle.requiredMinimumApparelQuality > QualityCategory.Awful)
				{
					stringBuilder.AppendLine("LetterRoyalTitleApparelQualityRequirement".Translate(CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals1.newTitle.requiredMinimumApparelQuality.GetLabel().ToLower()).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				if (CS$<>8__locals1.newTitle.requiredApparel != null && CS$<>8__locals1.newTitle.requiredApparel.Count > 0)
				{
					bool flag2 = false;
					stringBuilder.AppendLine("LetterRoyalTitleApparelRequirement".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					foreach (ApparelRequirement apparelRequirement in CS$<>8__locals1.newTitle.requiredApparel)
					{
						int i = 0;
						stringBuilder.Append("- ");
						stringBuilder.Append(string.Join(", ", apparelRequirement.AllRequiredApparelForPawn(CS$<>8__locals1.pawn, false, true).Select(delegate(ThingDef a)
						{
							int i;
							string result = (i == 0) ? a.LabelCap.Resolve() : a.label;
							i = i;
							i++;
							return result;
						}).ToArray<string>()));
						string t2;
						if (!ApparelUtility.IsRequirementActive(apparelRequirement, ApparelRequirementSource.Title, CS$<>8__locals1.pawn, out t2))
						{
							stringBuilder.Append(" [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t2 + "]");
						}
						else
						{
							flag2 = true;
						}
						stringBuilder.AppendLine();
					}
					if (flag2)
					{
						stringBuilder.AppendLine("- " + "ApparelRequirementAnyPrestigeArmor".Translate());
						stringBuilder.AppendLine("- " + "ApparelRequirementAnyPsycasterApparel".Translate());
						if (ModsConfig.BiotechActive)
						{
							stringBuilder.AppendLine("- " + "ApparelRequirementAnyMechlordApparel".Translate());
						}
					}
					stringBuilder.AppendLine();
				}
				if (!CS$<>8__locals1.newTitle.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleThroneroomRequirements".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from r in CS$<>8__locals1.newTitle.throneRoomRequirements
					select r.LabelCap(null)).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				if (!CS$<>8__locals1.newTitle.GetBedroomRequirements(CS$<>8__locals1.pawn).EnumerableNullOrEmpty<RoomRequirement>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleBedroomRequirements".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from r in CS$<>8__locals1.newTitle.GetBedroomRequirements(CS$<>8__locals1.pawn)
					select r.LabelCap(null)).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				if (flag && CS$<>8__locals1.newTitle.foodRequirement.Defined && CS$<>8__locals1.newTitle.SatisfyingMeals(true).Any<ThingDef>() && (CS$<>8__locals1.pawn.story == null || !CS$<>8__locals1.pawn.story.traits.HasTrait(TraitDefOf.Ascetic)))
				{
					stringBuilder.AppendLine("LetterRoyalTitleFoodRequirements".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from m in CS$<>8__locals1.newTitle.SatisfyingMeals(false)
					select m.LabelCap.Resolve()).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
			}
			List<RoyalTitlePermitDef> list4;
			List<RoyalTitlePermitDef> list5;
			RoyalTitleUtility.FindLostAndGainedPermits(currentTitle, CS$<>8__locals1.newTitle, out list4, out list5);
			if (CS$<>8__locals1.newTitle != null && CS$<>8__locals1.newTitle.permits != null)
			{
				stringBuilder.AppendLine("LetterRoyalTitlePermits".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
				IEnumerable<RoyalTitlePermitDef> permits = CS$<>8__locals1.newTitle.permits;
				Func<RoyalTitlePermitDef, int?> keySelector;
				if ((keySelector = CS$<>8__locals1.<>9__11) == null)
				{
					keySelector = (CS$<>8__locals1.<>9__11 = delegate(RoyalTitlePermitDef p)
					{
						RoyalTitleDef royalTitleDef2 = base.<BuildDifferenceExplanationText>g__FirstTitleWithPermit|7(p);
						if (royalTitleDef2 == null)
						{
							return null;
						}
						return new int?(royalTitleDef2.seniority);
					});
				}
				foreach (RoyalTitlePermitDef royalTitlePermitDef in permits.OrderBy(keySelector))
				{
					RoyalTitleDef royalTitleDef = CS$<>8__locals1.<BuildDifferenceExplanationText>g__FirstTitleWithPermit|7(royalTitlePermitDef);
					if (royalTitleDef != null)
					{
						stringBuilder.AppendLine("- " + royalTitlePermitDef.LabelCap + " (" + royalTitleDef.GetLabelFor(CS$<>8__locals1.pawn) + ")");
					}
				}
				stringBuilder.AppendLine();
			}
			if (list5.Count > 0)
			{
				stringBuilder.AppendLine("LetterRoyalTitleLostPermits".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
				foreach (RoyalTitlePermitDef royalTitlePermitDef2 in list5)
				{
					stringBuilder.AppendLine("- " + royalTitlePermitDef2.LabelCap);
				}
				stringBuilder.AppendLine();
			}
			if (CS$<>8__locals1.newTitle != null)
			{
				if (CS$<>8__locals1.newTitle.grantedAbilities.Contains(AbilityDefOf.Speech) && (currentTitle == null || !currentTitle.grantedAbilities.Contains(AbilityDefOf.Speech)))
				{
					stringBuilder.AppendLine("LetterRoyalTitleSpeechAbilityGained".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
				List<JoyKindDef> list6 = (from def in DefDatabase<JoyKindDef>.AllDefsListForReading
				where def.titleRequiredAny != null && def.titleRequiredAny.Contains(CS$<>8__locals1.newTitle)
				select def).ToList<JoyKindDef>();
				if (list6.Count > 0)
				{
					stringBuilder.AppendLine("LetterRoyalTitleEnabledJoyKind".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					foreach (JoyKindDef joyKindDef in list6)
					{
						stringBuilder.AppendLine("- " + joyKindDef.LabelCap);
					}
					stringBuilder.AppendLine();
				}
				if (flag && !CS$<>8__locals1.newTitle.disabledJoyKinds.NullOrEmpty<JoyKindDef>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleDisabledJoyKind".Translate(CS$<>8__locals1.pawn.Named("PAWN")).CapitalizeFirst());
					foreach (JoyKindDef joyKindDef2 in CS$<>8__locals1.newTitle.disabledJoyKinds)
					{
						stringBuilder.AppendLine("- " + joyKindDef2.LabelCap);
					}
					stringBuilder.AppendLine();
				}
				if (CS$<>8__locals1.faction.def.royalImplantRules != null)
				{
					List<RoyalImplantRule> list7 = new List<RoyalImplantRule>();
					foreach (RoyalImplantRule royalImplantRule in CS$<>8__locals1.faction.def.royalImplantRules)
					{
						RoyalTitleDef minTitleForImplant = CS$<>8__locals1.faction.GetMinTitleForImplant(royalImplantRule.implantHediff, 0);
						int num2 = CS$<>8__locals1.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant);
						if (num >= num2)
						{
							if (royalImplantRule.maxLevel == 0)
							{
								list7.Add(royalImplantRule);
							}
							else
							{
								list7.AddUnique(CS$<>8__locals1.faction.GetMaxAllowedImplantLevel(royalImplantRule.implantHediff, CS$<>8__locals1.newTitle));
							}
						}
					}
					if (list7.Count > 0)
					{
						stringBuilder.AppendLine("LetterRoyalTitleAllowedImplants".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + list7.Select(delegate(RoyalImplantRule i)
						{
							if (i.maxLevel == 0)
							{
								return string.Format("{0} ({1})", i.implantHediff.LabelCap, CS$<>8__locals1.faction.GetMinTitleForImplant(i.implantHediff, 0).GetLabelFor(CS$<>8__locals1.pawn));
							}
							return string.Format("{0}({1}x) ({2})", i.implantHediff.LabelCap, i.maxLevel, i.minTitle.GetLabelFor(CS$<>8__locals1.pawn));
						}).ToLineList("- ", false)).CapitalizeFirst());
						stringBuilder.AppendLine();
					}
				}
				if (currentTitle != null && CS$<>8__locals1.newTitle.seniority < currentTitle.seniority)
				{
					List<Hediff> list8 = new List<Hediff>();
					if (CS$<>8__locals1.pawn.health != null && CS$<>8__locals1.pawn.health.hediffSet != null)
					{
						foreach (Hediff hediff in CS$<>8__locals1.pawn.health.hediffSet.hediffs)
						{
							if (hediff.def.HasComp(typeof(HediffComp_RoyalImplant)))
							{
								RoyalTitleDef minTitleForImplant2 = CS$<>8__locals1.faction.GetMinTitleForImplant(hediff.def, HediffComp_RoyalImplant.GetImplantLevel(hediff));
								if (CS$<>8__locals1.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant2) > num)
								{
									list8.Add(hediff);
								}
							}
						}
					}
					if (list8.Count > 0)
					{
						stringBuilder.AppendLine("LetterRoyalTitleImplantsMustBeRemoved".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from i in list8
						select i.LabelCap).ToLineList("- ", false)).Resolve());
						stringBuilder.AppendLine("LetterRoyalTitleImplantGracePeriod".Translate());
						stringBuilder.AppendLine();
					}
				}
				if (CS$<>8__locals1.pawn.royalty.NewHighestTitle(CS$<>8__locals1.faction, CS$<>8__locals1.newTitle) && !CS$<>8__locals1.newTitle.rewards.NullOrEmpty<ThingDefCountClass>())
				{
					stringBuilder.AppendLine("LetterRoyalTitleRewardGranted".Translate(CS$<>8__locals1.pawn.Named("PAWN"), "\n" + (from r in CS$<>8__locals1.newTitle.rewards
					select r.Label).ToLineList("- ", false)).CapitalizeFirst());
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060136FB RID: 79611 RVA: 0x005C706C File Offset: 0x005C526C
		public static RoyalTitleDef GetCurrentTitleIn(this Pawn p, Faction faction)
		{
			if (p.royalty != null)
			{
				return p.royalty.GetCurrentTitle(faction);
			}
			return null;
		}

		// Token: 0x060136FC RID: 79612 RVA: 0x005C7084 File Offset: 0x005C5284
		public static int GetCurrentTitleSeniorityIn(this Pawn p, Faction faction)
		{
			RoyalTitleDef currentTitleIn = p.GetCurrentTitleIn(faction);
			if (currentTitleIn != null)
			{
				return currentTitleIn.seniority;
			}
			return 0;
		}

		// Token: 0x060136FD RID: 79613 RVA: 0x005C70A4 File Offset: 0x005C52A4
		public static string GetTitleProgressionInfo(Faction faction, Pawn pawn = null)
		{
			TaggedString t = "RoyalTitleTooltipTitlesEarnable".Translate(faction.Named("FACTION")) + ":";
			int num = 0;
			foreach (RoyalTitleDef royalTitleDef in faction.def.RoyalTitlesAwardableInSeniorityOrderForReading)
			{
				num += royalTitleDef.favorCost;
				t += "\n  - " + ((pawn != null) ? royalTitleDef.GetLabelCapFor(pawn) : royalTitleDef.GetLabelCapForBothGenders()) + ": " + "RoyalTitleTooltipRoyalFavorAmount".Translate(royalTitleDef.favorCost, faction.def.royalFavorLabel) + " (" + "RoyalTitleTooltipRoyalFavorTotal".Translate(num.ToString()) + ")";
			}
			t += "\n\n" + "RoyalTitleTooltipTitlesNonEarnable".Translate(faction.Named("FACTION")) + ":";
			foreach (RoyalTitleDef royalTitleDef2 in from tit in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where !tit.Awardable
			select tit)
			{
				t += "\n  - " + royalTitleDef2.GetLabelCapForBothGenders();
			}
			return t.Resolve();
		}

		// Token: 0x060136FE RID: 79614 RVA: 0x005C725C File Offset: 0x005C545C
		public static Building_Throne FindBestUnassignedThrone(Pawn pawn)
		{
			float num = float.PositiveInfinity;
			Building_Throne result = null;
			foreach (Thing thing in pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Throne))
			{
				Building_Throne building_Throne = thing as Building_Throne;
				if (building_Throne != null && building_Throne.CompAssignableToPawn.HasFreeSlot && building_Throne.Spawned && !building_Throne.IsForbidden(pawn) && pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false) && RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_All)) == null)
				{
					PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, building_Throne, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, PathEndMode.InteractionCell, null);
					float num2 = pawnPath.Found ? pawnPath.TotalCost : float.PositiveInfinity;
					pawnPath.ReleaseToPool();
					if (num > num2)
					{
						num = num2;
						result = building_Throne;
					}
				}
			}
			if (num >= float.PositiveInfinity)
			{
				return null;
			}
			return result;
		}

		// Token: 0x060136FF RID: 79615 RVA: 0x005C738C File Offset: 0x005C558C
		public static Building_Throne FindBestUsableThrone(Pawn pawn)
		{
			Building_Throne building_Throne = pawn.ownership.AssignedThrone;
			if (building_Throne != null)
			{
				if (!building_Throne.Spawned || building_Throne.IsForbidden(pawn) || !pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false))
				{
					return null;
				}
				if (RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_All)) != null)
				{
					return null;
				}
			}
			else
			{
				building_Throne = RoyalTitleUtility.FindBestUnassignedThrone(pawn);
				if (building_Throne == null)
				{
					return null;
				}
			}
			return building_Throne;
		}

		// Token: 0x06013700 RID: 79616 RVA: 0x005C73F4 File Offset: 0x005C55F4
		public static bool BedroomSatisfiesRequirements(Room room, RoyalTitle title)
		{
			using (List<RoomRequirement>.Enumerator enumerator = title.def.bedroomRequirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.MetOrDisabled(room, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06013701 RID: 79617 RVA: 0x005C7454 File Offset: 0x005C5654
		public static bool IsPawnConceited(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traitSet = (story != null) ? story.traits : null;
			return (traitSet == null || !traitSet.HasTrait(TraitDefOf.Ascetic)) && (!p.Faction.IsPlayer || p.IsQuestLodger() || (traitSet != null && (traitSet.HasTrait(TraitDefOf.Abrasive) || traitSet.HasTrait(TraitDefOf.Greedy) || traitSet.HasTrait(TraitDefOf.Jealous))));
		}

		// Token: 0x06013702 RID: 79618 RVA: 0x005C74C9 File Offset: 0x005C56C9
		public static IEnumerable<Trait> GetConceitedTraits(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traits = (story != null) ? story.traits : null;
			if (traits != null)
			{
				int num;
				for (int i = 0; i < RoyalTitleUtility.ConceitedTraits.Count; i = num + 1)
				{
					Trait trait = traits.GetTrait(RoyalTitleUtility.ConceitedTraits[i]);
					if (trait != null)
					{
						yield return trait;
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06013703 RID: 79619 RVA: 0x005C74D9 File Offset: 0x005C56D9
		public static IEnumerable<Trait> GetTraitsAffectingPsylinkNegatively(Pawn p)
		{
			if (p.story == null || p.story.traits == null || p.story.traits.allTraits.NullOrEmpty<Trait>())
			{
				yield break;
			}
			foreach (Trait trait in p.story.traits.allTraits)
			{
				if (!trait.Suppressed)
				{
					TraitDegreeData traitDegreeData = trait.def.DataAtDegree(trait.Degree);
					if (traitDegreeData.statFactors != null)
					{
						if (traitDegreeData.statFactors.Any((StatModifier f) => f.stat == StatDefOf.PsychicSensitivity && f.value < 1f))
						{
							goto IL_11F;
						}
					}
					if (traitDegreeData.statOffsets == null)
					{
						continue;
					}
					if (!traitDegreeData.statOffsets.Any((StatModifier f) => f.stat == StatDefOf.PsychicSensitivity && f.value < 0f))
					{
						continue;
					}
					IL_11F:
					yield return trait;
				}
			}
			List<Trait>.Enumerator enumerator = default(List<Trait>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06013704 RID: 79620 RVA: 0x005C74EC File Offset: 0x005C56EC
		public static TaggedString GetPsylinkAffectedByTraitsNegativelyWarning(Pawn p)
		{
			if (p.HasPsylink || !RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(p).Any<Trait>())
			{
				return null;
			}
			return "RoyalWithTraitAffectingPsylinkNegatively".Translate(p.Named("PAWN"), p.Faction.Named("FACTION"), (from t in RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(p)
			select t.Label).ToCommaList(true, false));
		}

		// Token: 0x06013705 RID: 79621 RVA: 0x005C7570 File Offset: 0x005C5770
		public static bool ShouldBecomeConceitedOnNewTitle(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traitSet = (story != null) ? story.traits : null;
			return (traitSet == null || !traitSet.HasTrait(TraitDefOf.Ascetic)) && (p.Faction == null || !p.Faction.IsPlayer || p.IsQuestLodger() || RoyalTitleUtility.GetConceitedTraits(p).Any<Trait>());
		}

		// Token: 0x06013706 RID: 79622 RVA: 0x005C75CC File Offset: 0x005C57CC
		public static Quest GetCurrentBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (!quest.Historical)
				{
					QuestPart_BestowingCeremony questPart_BestowingCeremony = (QuestPart_BestowingCeremony)quest.PartsListForReading.FirstOrDefault((QuestPart p) => p is QuestPart_BestowingCeremony);
					if (questPart_BestowingCeremony != null && questPart_BestowingCeremony.target == pawn && questPart_BestowingCeremony.bestower != null && questPart_BestowingCeremony.bestower.Faction == faction)
					{
						return quest;
					}
				}
			}
			return null;
		}

		// Token: 0x06013707 RID: 79623 RVA: 0x005C7680 File Offset: 0x005C5880
		public static bool ShouldGetBestowingCeremonyQuest(Pawn pawn, out Faction faction)
		{
			faction = null;
			return (!pawn.IsMutant || !pawn.mutant.Def.disableTitles) && (pawn.Faction != null && pawn.Faction.IsPlayer && pawn.royalty != null && pawn.royalty.CanUpdateTitleOfAnyFaction(out faction)) && RoyalTitleUtility.GetCurrentBestowingCeremonyQuest(pawn, faction) == null;
		}

		// Token: 0x06013708 RID: 79624 RVA: 0x005C76E4 File Offset: 0x005C58E4
		public static bool ShouldGetBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			return (!pawn.IsMutant || !pawn.mutant.Def.disableTitles) && (pawn.Faction != null && pawn.Faction.IsPlayer && pawn.royalty != null && pawn.royalty.CanUpdateTitle(faction)) && RoyalTitleUtility.GetCurrentBestowingCeremonyQuest(pawn, faction) == null;
		}

		// Token: 0x06013709 RID: 79625 RVA: 0x005C7744 File Offset: 0x005C5944
		public static void EndExistingBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (!quest.Historical && quest.State != QuestState.Ongoing)
				{
					QuestPart_BestowingCeremony questPart_BestowingCeremony = (QuestPart_BestowingCeremony)quest.PartsListForReading.FirstOrDefault((QuestPart p) => p is QuestPart_BestowingCeremony);
					if (questPart_BestowingCeremony != null && questPart_BestowingCeremony.target == pawn && questPart_BestowingCeremony.bestower.Faction == faction)
					{
						quest.End(QuestEndOutcome.InvalidPreAcceptance, false, true);
					}
				}
			}
		}

		// Token: 0x0601370A RID: 79626 RVA: 0x005C77F8 File Offset: 0x005C59F8
		public static void GenerateBestowingCeremonyQuest(Pawn pawn, Faction faction)
		{
			if (pawn == null || pawn.Dead)
			{
				return;
			}
			Slate slate = new Slate();
			slate.Set<Pawn>("titleHolder", pawn, false);
			slate.Set<Faction>("bestowingFaction", faction, false);
			if (QuestScriptDefOf.BestowingCeremony.CanRun(slate, pawn.MapHeld))
			{
				Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.BestowingCeremony, slate);
				if (quest.root.sendAvailableLetter)
				{
					QuestUtility.SendLetterQuestAvailable(quest, null);
				}
			}
		}

		// Token: 0x0601370B RID: 79627 RVA: 0x005C7864 File Offset: 0x005C5A64
		public static void ResetStaticData()
		{
			RoyalTitleUtility.ConceitedTraits = new List<TraitDef>
			{
				TraitDefOf.Abrasive,
				TraitDefOf.Greedy,
				TraitDefOf.Jealous
			};
		}

		// Token: 0x0601370C RID: 79628 RVA: 0x005C7894 File Offset: 0x005C5A94
		public static void DoTable_IngestibleMaxSatisfiedTitle()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("name", (ThingDef f) => f.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("max satisfied title", delegate(ThingDef t)
			{
				RoyalTitleDef royalTitleDef = t.ingestible.MaxSatisfiedTitle();
				if (royalTitleDef == null)
				{
					return "-";
				}
				return royalTitleDef.LabelCap;
			}));
			DebugTables.MakeTablesDialog<ThingDef>(from t in DefDatabase<ThingDef>.AllDefsListForReading
			where t.ingestible != null && !t.IsCorpse && t.ingestible.HumanEdible
			select t, list.ToArray());
		}

		// Token: 0x0400D77B RID: 55163
		private static List<TraitDef> ConceitedTraits;
	}
}
