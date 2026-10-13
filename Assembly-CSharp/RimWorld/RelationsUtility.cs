using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002527 RID: 9511
	public static class RelationsUtility
	{
		// Token: 0x0600D00D RID: 53261 RVA: 0x003D2854 File Offset: 0x003D0A54
		public static bool PawnsKnowEachOther(Pawn p1, Pawn p2)
		{
			return (p1.Faction != null && p1.Faction == p2.Faction) || (p1.RaceProps.IsFlesh && p1.relations.DirectRelations.Find((DirectPawnRelation x) => x.otherPawn == p2) != null) || (p2.RaceProps.IsFlesh && p2.relations.DirectRelations.Find((DirectPawnRelation x) => x.otherPawn == p1) != null) || RelationsUtility.HasAnySocialMemoryWith(p1, p2) || RelationsUtility.HasAnySocialMemoryWith(p2, p1);
		}

		// Token: 0x0600D00E RID: 53262 RVA: 0x003D2938 File Offset: 0x003D0B38
		public static bool IsDisfigured(Pawn pawn, Pawn forPawn = null, bool ignoreSightSources = false)
		{
			bool flag = forPawn == null || forPawn.Ideo == null || forPawn.Ideo.RequiredScars == 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if ((hediffs[i].def != HediffDefOf.Scarification || flag) && hediffs[i].Part != null && hediffs[i].Part.def.beautyRelated)
				{
					Hediff_MissingPart hediff_MissingPart = hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null)
					{
						if (ignoreSightSources && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.SightSource))
						{
							goto IL_CA;
						}
						if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_MissingPart.Part))
						{
							goto IL_CA;
						}
					}
					else if (!(hediffs[i] is Hediff_Injury))
					{
						goto IL_CA;
					}
					return true;
				}
				IL_CA:;
			}
			return false;
		}

		// Token: 0x0600D00F RID: 53263 RVA: 0x003D2A20 File Offset: 0x003D0C20
		public static bool TryDevelopBondRelation(Pawn humanlike, Pawn animal, float baseChance)
		{
			if (!animal.IsAnimal)
			{
				return false;
			}
			if (animal.Faction == Faction.OfPlayer && humanlike.IsQuestLodger())
			{
				return false;
			}
			TrainabilityDef trainability = TrainableUtility.GetTrainability(animal);
			if (trainability == null || trainability.intelligenceOrder < TrainabilityDefOf.Intermediate.intelligenceOrder)
			{
				return false;
			}
			if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				return false;
			}
			if (animal.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => x.Spawned) != null)
			{
				return false;
			}
			if (humanlike.story.traits.HasTrait(TraitDefOf.Psychopath) || humanlike.Inhumanized())
			{
				return false;
			}
			if (!new HistoryEvent(HistoryEventDefOf.Bonded, humanlike.Named(HistoryEventArgsNames.Doer), animal.Named(HistoryEventArgsNames.Victim)).DoerWillingToDo())
			{
				return false;
			}
			int num = 0;
			List<DirectPawnRelation> directRelations = animal.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Bond && !directRelations[i].otherPawn.Dead)
				{
					num++;
				}
			}
			int num2 = 0;
			List<DirectPawnRelation> directRelations2 = humanlike.relations.DirectRelations;
			for (int j = 0; j < directRelations2.Count; j++)
			{
				if (directRelations2[j].def == PawnRelationDefOf.Bond && !directRelations2[j].otherPawn.Dead)
				{
					num2++;
				}
			}
			if (num > 0)
			{
				baseChance *= Mathf.Pow(0.2f, (float)num);
			}
			if (num2 > 0)
			{
				baseChance *= Mathf.Pow(0.55f, (float)num2);
			}
			baseChance *= humanlike.GetStatValue(StatDefOf.BondAnimalChanceFactor, true, -1);
			if (Rand.Value < baseChance)
			{
				humanlike.relations.AddDirectRelation(PawnRelationDefOf.Bond, animal);
				if (humanlike.Faction == Faction.OfPlayer || animal.Faction == Faction.OfPlayer)
				{
					TaleRecorder.RecordTale(TaleDefOf.BondedWithAnimal, new object[]
					{
						humanlike,
						animal
					});
				}
				bool flag = false;
				string value = null;
				if (animal.Name == null || animal.Name.Numerical)
				{
					flag = true;
					value = ((animal.Name == null) ? animal.LabelIndefinite() : animal.Name.ToStringFull);
					animal.Name = PawnBioAndNameGenerator.GeneratePawnName(animal, NameStyle.Full, null, false, null);
				}
				if (PawnUtility.ShouldSendNotificationAbout(humanlike) || PawnUtility.ShouldSendNotificationAbout(animal))
				{
					string text;
					if (flag)
					{
						text = "MessageNewBondRelationNewName".Translate(humanlike.LabelShort, value, animal.Name.ToStringFull, humanlike.Named("HUMAN"), animal.Named("ANIMAL")).AdjustedFor(animal, "PAWN", true).CapitalizeFirst();
					}
					else
					{
						text = "MessageNewBondRelation".Translate(humanlike.LabelShort, animal.LabelShort, humanlike.Named("HUMAN"), animal.Named("ANIMAL")).CapitalizeFirst();
					}
					Messages.Message(text, humanlike, MessageTypeDefOf.PositiveEvent, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600D010 RID: 53264 RVA: 0x003D2D4C File Offset: 0x003D0F4C
		public static AcceptanceReport RomanceEligiblePair(Pawn initiator, Pawn target, bool forOpinionExplanation)
		{
			if (initiator == target)
			{
				return false;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ChildcareUtility.CanSuckle(target, out breastfeedFailReason))
			{
				return false;
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingLoveRealtionshipBetween(initiator, target, false);
			if (directPawnRelation != null)
			{
				string genderSpecificLabel = directPawnRelation.def.GetGenderSpecificLabel(target);
				return "RomanceChanceExistingRelation".Translate(initiator.Named("PAWN"), genderSpecificLabel.Named("RELATION"));
			}
			if (!RelationsUtility.RomanceEligible(initiator, true, forOpinionExplanation))
			{
				return false;
			}
			if (target.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				if (!forOpinionExplanation)
				{
					return AcceptanceReport.WasRejected;
				}
				return "CantRomanceTargetYoung".Translate();
			}
			else if (RelationsUtility.Incestuous(initiator, target))
			{
				if (!forOpinionExplanation)
				{
					return AcceptanceReport.WasRejected;
				}
				return "CantRomanceTargetIncest".Translate();
			}
			else if (target.IsPrisoner)
			{
				if (!forOpinionExplanation)
				{
					return AcceptanceReport.WasRejected;
				}
				return "CantRomanceTargetPrisoner".Translate();
			}
			else if (!RelationsUtility.AttractedToGender(initiator, target.gender) || !RelationsUtility.AttractedToGender(target, initiator.gender))
			{
				if (!forOpinionExplanation)
				{
					return AcceptanceReport.WasRejected;
				}
				return "CantRomanceTargetSexuality".Translate();
			}
			else
			{
				AcceptanceReport acceptanceReport = RelationsUtility.RomanceEligible(target, false, forOpinionExplanation);
				if (!acceptanceReport)
				{
					return acceptanceReport;
				}
				if (target.relations.OpinionOf(initiator) <= 5)
				{
					return "CantRomanceTargetOpinion".Translate();
				}
				if (!forOpinionExplanation && InteractionWorker_RomanceAttempt.SuccessChance(initiator, target, 1f) <= 0f)
				{
					return "CantRomanceTargetZeroChance".Translate();
				}
				if ((!forOpinionExplanation && !initiator.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)) || target.IsForbidden(initiator))
				{
					return "CantRomanceTargetUnreachable".Translate();
				}
				if (initiator.relations.IsTryRomanceOnCooldown)
				{
					return "RomanceOnCooldown".Translate();
				}
				return true;
			}
		}

		// Token: 0x0600D011 RID: 53265 RVA: 0x003D2F18 File Offset: 0x003D1118
		public static bool AttractedToGender(Pawn pawn, Gender gender)
		{
			Pawn_StoryTracker story = pawn.story;
			bool flag;
			if (story == null)
			{
				flag = false;
			}
			else
			{
				TraitSet traits = story.traits;
				bool? flag2 = (traits != null) ? new bool?(traits.HasTrait(TraitDefOf.Asexual)) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				return false;
			}
			Pawn_StoryTracker story2 = pawn.story;
			bool flag4;
			if (story2 == null)
			{
				flag4 = false;
			}
			else
			{
				TraitSet traits2 = story2.traits;
				bool? flag2 = (traits2 != null) ? new bool?(traits2.HasTrait(TraitDefOf.Bisexual)) : null;
				bool flag3 = true;
				flag4 = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag4)
			{
				return true;
			}
			Pawn_StoryTracker story3 = pawn.story;
			bool flag5;
			if (story3 == null)
			{
				flag5 = false;
			}
			else
			{
				TraitSet traits3 = story3.traits;
				bool? flag2 = (traits3 != null) ? new bool?(traits3.HasTrait(TraitDefOf.Gay)) : null;
				bool flag3 = true;
				flag5 = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag5)
			{
				return pawn.gender == gender;
			}
			return pawn.gender != gender;
		}

		// Token: 0x0600D012 RID: 53266 RVA: 0x003D3014 File Offset: 0x003D1214
		public static AcceptanceReport RomanceEligible(Pawn pawn, bool initiator, bool forOpinionExplanation)
		{
			if (pawn.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				return false;
			}
			if (pawn.IsPrisoner)
			{
				if (!initiator || forOpinionExplanation)
				{
					return AcceptanceReport.WasRejected;
				}
				return "CantRomanceInitiateMessagePrisoner".Translate(pawn).CapitalizeFirst();
			}
			else
			{
				if (pawn.Downed && !forOpinionExplanation)
				{
					return initiator ? "CantRomanceInitiateMessageDowned".Translate(pawn).CapitalizeFirst() : "CantRomanceTargetDowned".Translate();
				}
				Pawn_StoryTracker story = pawn.story;
				bool flag;
				if (story == null)
				{
					flag = false;
				}
				else
				{
					TraitSet traits = story.traits;
					bool? flag2 = (traits != null) ? new bool?(traits.HasTrait(TraitDefOf.Asexual)) : null;
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					if (!initiator || forOpinionExplanation)
					{
						return AcceptanceReport.WasRejected;
					}
					return "CantRomanceInitiateMessageAsexual".Translate(pawn).CapitalizeFirst();
				}
				else if (initiator && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
				{
					if (!forOpinionExplanation)
					{
						return "CantRomanceInitiateMessageTalk".Translate(pawn).CapitalizeFirst();
					}
					return AcceptanceReport.WasRejected;
				}
				else
				{
					if (pawn.Drafted && !forOpinionExplanation)
					{
						return initiator ? "CantRomanceInitiateMessageDrafted".Translate(pawn).CapitalizeFirst() : "CantRomanceTargetDrafted".Translate();
					}
					if (initiator && pawn.IsSlave)
					{
						if (!forOpinionExplanation)
						{
							return "CantRomanceInitiateMessageSlave".Translate(pawn).CapitalizeFirst();
						}
						return AcceptanceReport.WasRejected;
					}
					else
					{
						if (pawn.MentalState != null)
						{
							return (initiator && !forOpinionExplanation) ? "CantRomanceInitiateMessageMentalState".Translate(pawn).CapitalizeFirst() : "CantRomanceTargetMentalState".Translate();
						}
						return true;
					}
				}
			}
		}

		// Token: 0x0600D013 RID: 53267 RVA: 0x003D3200 File Offset: 0x003D1400
		private static void GiveRomanceJob(Pawn romancer, Pawn romanceTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.TryRomance, romanceTarget);
			job.interaction = InteractionDefOf.RomanceAttempt;
			romancer.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x0600D014 RID: 53268 RVA: 0x003D3240 File Offset: 0x003D1440
		private static string GetRelationshipWarning(Pawn pawn)
		{
			int count = pawn.GetLoveRelations(false, false).Count;
			bool flag = count >= 1;
			if (flag && ModsConfig.IdeologyActive)
			{
				flag = !new HistoryEvent(pawn.GetHistoryEventForLoveRelationCountPlusOne(), pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo();
			}
			if (!flag)
			{
				return "";
			}
			if (count <= 1)
			{
				return " - " + "RomanceWarningMonogamous".Translate(pawn) + "\n";
			}
			return " - " + "RomanceWarningPolygamous".Translate(pawn, count) + "\n";
		}

		// Token: 0x0600D015 RID: 53269 RVA: 0x003D32F4 File Offset: 0x003D14F4
		private static void GiveRomanceJobWithWarning(Pawn romancer, Pawn romanceTarget)
		{
			string text = RelationsUtility.GetRelationshipWarning(romancer) + RelationsUtility.GetRelationshipWarning(romanceTarget);
			if (!text.NullOrEmpty())
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("RomanceExistingRelationshipWarning".Translate(romancer.Named("INITIATOR"), romanceTarget.Named("TARGET")) + "\n\n" + text + "\n" + "StillWishContinue".Translate(), delegate()
				{
					RelationsUtility.GiveRomanceJob(romancer, romanceTarget);
				}, true, null, WindowLayer.Dialog);
				Find.WindowStack.Add(window);
				return;
			}
			RelationsUtility.GiveRomanceJob(romancer, romanceTarget);
		}

		// Token: 0x0600D016 RID: 53270 RVA: 0x003D33C0 File Offset: 0x003D15C0
		public static bool RomanceOption(Pawn initiator, Pawn romanceTarget, out FloatMenuOption option, out float chance)
		{
			if (!RelationsUtility.AttractedToGender(initiator, romanceTarget.gender))
			{
				option = null;
				chance = 0f;
				return false;
			}
			AcceptanceReport acceptanceReport = RelationsUtility.RomanceEligiblePair(initiator, romanceTarget, false);
			if (!acceptanceReport.Accepted && acceptanceReport.Reason.NullOrEmpty())
			{
				option = null;
				chance = 0f;
				return false;
			}
			if (acceptanceReport.Accepted)
			{
				chance = InteractionWorker_RomanceAttempt.SuccessChance(initiator, romanceTarget, 1f);
				string label = string.Format("{0} ({1} {2})", romanceTarget.LabelShort, chance.ToStringPercent(), "chance".Translate());
				option = new FloatMenuOption(label, delegate()
				{
					RelationsUtility.GiveRomanceJobWithWarning(initiator, romanceTarget);
				}, MenuOptionPriority.Low, null, null, 0f, null, null, true, 0);
				return true;
			}
			chance = 0f;
			option = new FloatMenuOption(romanceTarget.LabelShort + " (" + acceptanceReport.Reason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			return false;
		}

		// Token: 0x0600D017 RID: 53271 RVA: 0x003D34E8 File Offset: 0x003D16E8
		private static bool Incestuous(Pawn one, Pawn two)
		{
			using (IEnumerator<PawnRelationDef> enumerator = one.GetRelations(two).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.romanceChanceFactor != 1f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D018 RID: 53272 RVA: 0x003D3544 File Offset: 0x003D1744
		public static string LabelWithBondInfo(Pawn humanlike, Pawn animal)
		{
			string text = humanlike.LabelShort;
			if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				text += " " + "BondBrackets".Translate();
			}
			return text;
		}

		// Token: 0x0600D019 RID: 53273 RVA: 0x003D358C File Offset: 0x003D178C
		private static bool HasAnySocialMemoryWith(Pawn p, Pawn otherPawn)
		{
			if (p.Dead)
			{
				return false;
			}
			if (!p.RaceProps.Humanlike || !otherPawn.RaceProps.Humanlike || p.needs == null || p.needs.mood == null)
			{
				return false;
			}
			List<Thought_Memory> memories = p.needs.mood.thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				Thought_MemorySocial thought_MemorySocial = memories[i] as Thought_MemorySocial;
				if (thought_MemorySocial != null && thought_MemorySocial.OtherPawn() == otherPawn)
				{
					return true;
				}
			}
			return false;
		}
	}
}
