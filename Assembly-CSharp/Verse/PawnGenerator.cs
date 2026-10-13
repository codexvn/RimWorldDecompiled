using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020007B1 RID: 1969
	public static class PawnGenerator
	{
		// Token: 0x0600316D RID: 12653 RVA: 0x00100F20 File Offset: 0x000FF120
		public static void Reset()
		{
			PawnGenerator.relationsGeneratableBlood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
			where rel.familyByBloodRelation && rel.generationChanceFactor > 0f
			select rel).ToArray<PawnRelationDef>();
			PawnGenerator.relationsGeneratableNonblood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
			where !rel.familyByBloodRelation && rel.generationChanceFactor > 0f
			select rel).ToArray<PawnRelationDef>();
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x00100F94 File Offset: 0x000FF194
		public static Pawn GeneratePawn(PawnKindDef kindDef, Faction faction = null, PlanetTile? tile = null)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			if (tile != null)
			{
				request.Tile = tile.Value;
			}
			if (faction != null && faction.deactivated)
			{
				request.AllowDowned = true;
			}
			if (kindDef is CreepJoinerFormKindDef)
			{
				request.IsCreepJoiner = true;
			}
			return PawnGenerator.GeneratePawn(request);
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x00101070 File Offset: 0x000FF270
		public static Pawn GeneratePawn(PawnGenerationRequest request)
		{
			request.ValidateAndFix();
			Pawn result;
			try
			{
				Pawn pawn = PawnGenerator.GenerateOrRedressPawnInternal(request);
				if (pawn != null && !request.AllowDead && !request.ForceDead && pawn.health.hediffSet.hediffs.Any<Hediff>())
				{
					bool dead = pawn.Dead;
					bool downed = pawn.Downed;
					pawn.health.hediffSet.DirtyCache();
					pawn.health.CheckForStateChange(null, null);
					if (pawn.Dead)
					{
						string[] array = new string[10];
						array[0] = "Pawn was generated dead but the pawn generation request specified the pawn must be alive. This shouldn't ever happen even if we ran out of tries because null pawn should have been returned instead in this case. Resetting health...\npawn.Dead=";
						array[1] = pawn.Dead.ToString();
						array[2] = " pawn.Downed=";
						array[3] = pawn.Downed.ToString();
						array[4] = " deadBefore=";
						array[5] = dead.ToString();
						array[6] = " downedBefore=";
						array[7] = downed.ToString();
						array[8] = "\nrequest=";
						int num = 9;
						PawnGenerationRequest pawnGenerationRequest = request;
						array[num] = pawnGenerationRequest.ToString();
						Log.Error(string.Concat(array));
						pawn.health.Reset();
					}
				}
				if (pawn.guest != null)
				{
					if (request.ForceRecruitable)
					{
						pawn.guest.Recruitable = true;
					}
					else
					{
						pawn.guest.SetupRecruitable();
					}
				}
				if (pawn.Faction == Faction.OfPlayerSilentFail && !pawn.IsQuestLodger())
				{
					Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(pawn, PopAdaptationEvent.GainedColonist);
				}
				result = pawn;
			}
			catch (Exception ex)
			{
				string str = "Error while generating pawn. Rethrowing. Exception: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				throw;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x00101240 File Offset: 0x000FF440
		private static Pawn GenerateOrRedressPawnInternal(PawnGenerationRequest request)
		{
			Pawn pawn = null;
			if (!request.AllowedDevelopmentalStages.Newborn() && !request.ForceGenerateNewPawn)
			{
				if (request.ForceRedressWorldPawnIfFormerColonist && PawnGenerator.GetValidCandidatesToRedress(request).Where(new Func<Pawn, bool>(PawnUtility.EverBeenColonistOrTameAnimal)).TryRandomElementByWeight(new Func<Pawn, float>(PawnGenerator.WorldPawnSelectionWeight), out pawn))
				{
					PawnGenerator.RedressPawn(pawn, request);
					Find.WorldPawns.RemovePawn(pawn);
				}
				if (pawn == null && request.Inhabitant && request.Tile.Valid)
				{
					Settlement settlement = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement != null && settlement.previouslyGeneratedInhabitants.Any<Pawn>() && (from x in PawnGenerator.GetValidCandidatesToRedress(request)
					where settlement.previouslyGeneratedInhabitants.Contains(x)
					select x).TryRandomElementByWeight(new Func<Pawn, float>(PawnGenerator.WorldPawnSelectionWeight), out pawn))
					{
						PawnGenerator.RedressPawn(pawn, request);
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
				if (pawn == null && Rand.Chance(PawnGenerator.ChanceToRedressAnyWorldPawn(request)) && PawnGenerator.GetValidCandidatesToRedress(request).TryRandomElementByWeight(new Func<Pawn, float>(PawnGenerator.WorldPawnSelectionWeight), out pawn))
				{
					PawnGenerator.RedressPawn(pawn, request);
					Find.WorldPawns.RemovePawn(pawn);
				}
			}
			bool redressed;
			if (pawn == null)
			{
				redressed = false;
				pawn = PawnGenerator.GenerateNewPawnInternal(ref request);
				if (pawn == null)
				{
					return null;
				}
				if (request.Inhabitant && !request.Tile.Valid)
				{
					Settlement settlement2 = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement2 != null)
					{
						settlement2.previouslyGeneratedInhabitants.Add(pawn);
					}
				}
			}
			else
			{
				redressed = true;
			}
			if (pawn.Ideo != null)
			{
				pawn.Ideo.Notify_MemberGenerated(pawn, request.AllowedDevelopmentalStages.Newborn(), request.ForceNoIdeoGear);
			}
			if (Find.Scenario != null)
			{
				Find.Scenario.Notify_PawnGenerated(pawn, request.Context, redressed);
			}
			if (request.DontGivePreArrivalPathway)
			{
				pawn.dontGivePreArrivalPathway = true;
			}
			return pawn;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x00101430 File Offset: 0x000FF630
		public static void RedressPawn(Pawn pawn, PawnGenerationRequest request)
		{
			try
			{
				if (pawn.becameWorldPawnTickAbs != -1 && pawn.health != null)
				{
					float x = (GenTicks.TicksAbs - pawn.becameWorldPawnTickAbs).TicksToDays();
					List<Hediff> list = SimplePool<List<Hediff>>.Get();
					list.Clear();
					foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
					{
						if (!hediff.def.removeOnRedressIfNotOfKind.NullOrEmpty<PawnKindDef>() && !hediff.def.removeOnRedressIfNotOfKind.Contains(request.KindDef))
						{
							list.Add(hediff);
						}
						else if (Rand.Chance(hediff.def.removeOnRedressChanceByDaysCurve.Evaluate(x)))
						{
							list.Add(hediff);
						}
					}
					foreach (Hediff hediff2 in list)
					{
						pawn.health.RemoveHediff(hediff2);
					}
					list.Clear();
					SimplePool<List<Hediff>>.Return(list);
				}
				pawn.ChangeKind(request.KindDef);
				if (pawn.royalty != null)
				{
					pawn.royalty.allowRoomRequirements = pawn.kindDef.allowRoyalRoomRequirements;
					pawn.royalty.allowApparelRequirements = pawn.kindDef.allowRoyalApparelRequirements;
				}
				if (ModsConfig.BiotechActive && pawn.genes != null)
				{
					List<Gene> genesListForReading = pawn.genes.GenesListForReading;
					for (int i = genesListForReading.Count - 1; i >= 0; i--)
					{
						if (genesListForReading[i].def.removeOnRedress)
						{
							pawn.genes.RemoveGene(genesListForReading[i]);
						}
					}
				}
				if (pawn.Faction != request.Faction)
				{
					pawn.SetFaction(request.Faction, null);
					if (request.FixedIdeo != null)
					{
						pawn.ideo.SetIdeo(request.FixedIdeo);
					}
					else if (pawn.ideo != null)
					{
						Faction faction = request.Faction;
						if (((faction != null) ? faction.ideos : null) != null && !request.Faction.ideos.Has(pawn.Ideo))
						{
							pawn.ideo.SetIdeo(request.Faction.ideos.GetRandomIdeoForNewPawn());
						}
					}
				}
				PawnGenerator.GenerateGearFor(pawn, request);
				PawnGenerator.AddRequiredScars(pawn);
				if (ModsConfig.AnomalyActive)
				{
					if (request.ForcedMutant != null)
					{
						MutantUtility.SetFreshPawnAsMutant(pawn, request.ForcedMutant);
					}
					else if (request.KindDef.mutant != null)
					{
						MutantUtility.SetFreshPawnAsMutant(pawn, request.KindDef.mutant);
					}
				}
				if (pawn.guest != null)
				{
					pawn.guest.SetGuestStatus(null, GuestStatus.Guest);
					pawn.guest.RandomizeJoinStatus();
				}
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					needs.SetInitialLevels();
				}
				Pawn_MindState mindState = pawn.mindState;
				if (mindState != null)
				{
					mindState.Notify_PawnRedressed();
				}
				Pawn_SurroundingsTracker surroundings = pawn.surroundings;
				if (surroundings != null)
				{
					surroundings.Clear();
				}
				Pawn_GeneTracker genes = pawn.genes;
				if (genes != null)
				{
					genes.Reset();
				}
				pawn.wasLeftBehindStartingPawn = false;
			}
			finally
			{
			}
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x00101774 File Offset: 0x000FF974
		public static bool IsBeingGenerated(Pawn pawn)
		{
			for (int i = 0; i < PawnGenerator.pawnsBeingGenerated.Count; i++)
			{
				if (PawnGenerator.pawnsBeingGenerated[i].Pawn == pawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x001017B0 File Offset: 0x000FF9B0
		public static bool IsPawnBeingGeneratedAndNotAllowsDead(Pawn pawn)
		{
			for (int i = 0; i < PawnGenerator.pawnsBeingGenerated.Count; i++)
			{
				if (PawnGenerator.pawnsBeingGenerated[i].Pawn == pawn && !PawnGenerator.pawnsBeingGenerated[i].AllowsDead)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x00101800 File Offset: 0x000FFA00
		private static bool IsValidCandidateToRedress(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.def != request.KindDef.race)
			{
				return false;
			}
			if (!request.WorldPawnFactionDoesntMatter && pawn.Faction != request.Faction)
			{
				return false;
			}
			if (!request.AllowDead)
			{
				if (pawn.Dead || pawn.Destroyed)
				{
					return false;
				}
				if (pawn.health.hediffSet.GetBrain() == null)
				{
					return false;
				}
			}
			if (!request.AllowDowned && pawn.Downed)
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.001f)
			{
				return false;
			}
			if (!request.CanGeneratePawnRelations && pawn.RaceProps.IsFlesh && pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return false;
			}
			if (!request.AllowGay && pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return false;
			}
			if (!request.AllowAddictions && AddictionUtility.AddictedToAnything(pawn))
			{
				return false;
			}
			if (request.ProhibitedTraits != null && request.ProhibitedTraits.Any((TraitDef t) => pawn.story.traits.HasTrait(t)))
			{
				return false;
			}
			if (request.KindDef.forcedHair != null && pawn.story.hairDef != request.KindDef.forcedHair)
			{
				return false;
			}
			if (ModsConfig.BiotechActive && !request.AllowPregnant && pawn.RaceProps.Humanlike && pawn.health.hediffSet.HasHediff(HediffDefOf.PregnantHuman, false))
			{
				return false;
			}
			if (pawn.IsMutant)
			{
				return false;
			}
			List<SkillRange> skills = request.KindDef.skills;
			if (skills != null)
			{
				for (int i = 0; i < skills.Count; i++)
				{
					SkillRecord skill = pawn.skills.GetSkill(skills[i].Skill);
					if (skill.TotallyDisabled)
					{
						return false;
					}
					if (skill.Level < skills[i].Range.min || skill.Level > skills[i].Range.max)
					{
						return false;
					}
				}
			}
			if (request.KindDef.missingParts != null)
			{
				foreach (MissingPart missingPart in request.KindDef.missingParts)
				{
					foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
					{
						Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
						if (hediff_MissingPart != null)
						{
							if (missingPart.BodyPart == hediff_MissingPart.Part.def && !PawnGenerator.tmpMissingParts.Contains(hediff_MissingPart))
							{
								PawnGenerator.tmpMissingParts.Add(hediff_MissingPart);
								break;
							}
							PawnGenerator.tmpMissingParts.Clear();
							return false;
						}
					}
				}
				PawnGenerator.tmpMissingParts.Clear();
			}
			if (request.KindDef.forcedTraits != null)
			{
				using (List<TraitRequirement>.Enumerator enumerator3 = request.KindDef.forcedTraits.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (!enumerator3.Current.HasTrait(pawn))
						{
							return false;
						}
					}
				}
			}
			if (request.ForcedTraits != null)
			{
				foreach (TraitDef tDef in request.ForcedTraits)
				{
					if (!pawn.story.traits.HasTrait(tDef))
					{
						return false;
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				if (request.ForcedXenogenes != null)
				{
					if (pawn.genes == null)
					{
						return false;
					}
					foreach (GeneDef geneDef in request.ForcedXenogenes)
					{
						if (!pawn.genes.HasXenogene(geneDef))
						{
							return false;
						}
					}
				}
				if (request.ForcedEndogenes != null)
				{
					if (pawn.genes == null)
					{
						return false;
					}
					foreach (GeneDef geneDef2 in request.ForcedEndogenes)
					{
						if (!pawn.genes.HasEndogene(geneDef2))
						{
							return false;
						}
					}
				}
				if (request.ForcedXenotype != null && (pawn.genes == null || pawn.genes.Xenotype != request.ForcedXenotype))
				{
					return false;
				}
				if (pawn.genes != null)
				{
					if (request.KindDef.useFactionXenotypes && request.Faction != null && request.Faction.def.xenotypeSet != null && !request.Faction.def.xenotypeSet.Contains(pawn.genes.Xenotype))
					{
						return false;
					}
					if (request.KindDef.xenotypeSet != null && !request.KindDef.xenotypeSet.Contains(pawn.genes.Xenotype))
					{
						return false;
					}
					if (request.MustBeCapableOfViolence && !pawn.genes.Xenotype.canGenerateAsCombatant)
					{
						return false;
					}
				}
			}
			if (!request.AllowedDevelopmentalStages.HasAny(pawn.DevelopmentalStage))
			{
				return false;
			}
			if (request.KindDef.fixedGender != null && pawn.gender != request.KindDef.fixedGender.Value)
			{
				return false;
			}
			if (request.ValidatorPreGear != null && !request.ValidatorPreGear(pawn))
			{
				return false;
			}
			if (request.ValidatorPostGear != null && !request.ValidatorPostGear(pawn))
			{
				return false;
			}
			if (request.FixedBiologicalAge != null)
			{
				float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
				float? num = request.FixedBiologicalAge;
				if (!(ageBiologicalYearsFloat == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.FixedChronologicalAge != null)
			{
				float num2 = (float)pawn.ageTracker.AgeChronologicalYears;
				float? num = request.FixedChronologicalAge;
				if (!(num2 == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.KindDef.chronologicalAgeRange != null && !request.KindDef.chronologicalAgeRange.Value.Includes((float)pawn.ageTracker.AgeChronologicalYears))
			{
				return false;
			}
			if (request.FixedGender != null)
			{
				Gender gender = pawn.gender;
				Gender? fixedGender = request.FixedGender;
				if (!(gender == fixedGender.GetValueOrDefault() & fixedGender != null))
				{
					return false;
				}
			}
			if (request.FixedLastName != null)
			{
				NameTriple nameTriple = pawn.Name as NameTriple;
				if (nameTriple == null || nameTriple.Last != request.FixedLastName)
				{
					return false;
				}
			}
			if (request.FixedTitle != null && (pawn.royalty == null || !pawn.royalty.HasTitle(request.FixedTitle)))
			{
				return false;
			}
			if (request.ForceNoIdeo && pawn.Ideo != null)
			{
				return false;
			}
			if (request.ForceNoBackstory && pawn.story != null && (pawn.story.Adulthood != null || pawn.story.Childhood != null))
			{
				return false;
			}
			if (request.KindDef.minTitleRequired != null)
			{
				if (pawn.royalty == null)
				{
					return false;
				}
				RoyalTitleDef royalTitleDef = pawn.royalty.MainTitle();
				if (royalTitleDef == null || royalTitleDef.seniority < request.KindDef.minTitleRequired.seniority)
				{
					return false;
				}
			}
			if (request.Context == PawnGenerationContext.PlayerStarter && Find.Scenario != null && !Find.Scenario.AllowPlayerStartingPawn(pawn, true, request))
			{
				return false;
			}
			if (request.MustBeCapableOfViolence)
			{
				if (pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return false;
				}
				if (pawn.RaceProps.ToolUser && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					return false;
				}
			}
			return (request.KindDef.requiredWorkTags == WorkTags.None || pawn.kindDef == request.KindDef || (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) == WorkTags.None) && PawnGenerator.HasCorrectMinBestSkillLevel(pawn, request.KindDef) && PawnGenerator.HasCorrectMinTotalSkillLevels(pawn, request.KindDef) && (pawn.royalty == null || !pawn.royalty.AllTitlesForReading.Any<RoyalTitle>() || request.KindDef.titleRequired != null || !request.KindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() || request.KindDef == pawn.kindDef) && (pawn.royalty == null || request.KindDef != pawn.kindDef || request.KindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() || pawn.royalty.AllTitlesForReading.Any<RoyalTitle>()) && (request.RedressValidator == null || request.RedressValidator(pawn)) && (request.KindDef.requiredWorkTags == WorkTags.None || pawn.kindDef == request.KindDef || (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) == WorkTags.None) && (!ModsConfig.AnomalyActive || (!pawn.IsCreepJoiner && !(pawn.kindDef is CreepJoinerFormKindDef))) && (!request.ForceDead || pawn.Dead);
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x001022C4 File Offset: 0x001004C4
		private static bool HasCorrectMinBestSkillLevel(Pawn pawn, PawnKindDef kind)
		{
			if (kind.minBestSkillLevel <= 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < pawn.skills.skills.Count; i++)
			{
				num = Mathf.Max(num, pawn.skills.skills[i].Level);
				if (num >= kind.minBestSkillLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x00102324 File Offset: 0x00100524
		private static bool HasCorrectMinTotalSkillLevels(Pawn pawn, PawnKindDef kind)
		{
			if (kind.minTotalSkillLevels <= 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < pawn.skills.skills.Count; i++)
			{
				num += pawn.skills.skills[i].Level;
				if (num >= kind.minTotalSkillLevels)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x00102380 File Offset: 0x00100580
		private static Pawn GenerateNewPawnInternal(ref PawnGenerationRequest request)
		{
			Pawn pawn = null;
			string text = null;
			bool ignoreScenarioRequirements = false;
			bool ignoreValidator = false;
			for (int i = 0; i < 120; i++)
			{
				if (i == 70)
				{
					Log.Error(string.Concat(new string[]
					{
						"Could not generate a pawn after ",
						70.ToString(),
						" tries. Last error: ",
						text,
						" Ignoring scenario requirements."
					}));
					ignoreScenarioRequirements = true;
				}
				if (i == 100)
				{
					Log.Error(string.Concat(new string[]
					{
						"Could not generate a pawn after ",
						100.ToString(),
						" tries. Last error: ",
						text,
						" Ignoring validator."
					}));
					ignoreValidator = true;
				}
				PawnGenerationRequest pawnGenerationRequest = request;
				pawn = PawnGenerator.TryGenerateNewPawnInternal(ref pawnGenerationRequest, out text, ignoreScenarioRequirements, ignoreValidator);
				if (pawn != null)
				{
					request = pawnGenerationRequest;
					break;
				}
			}
			if (pawn == null)
			{
				string[] array = new string[6];
				array[0] = "Pawn generation error: ";
				array[1] = text;
				array[2] = " Too many tries (";
				array[3] = 120.ToString();
				array[4] = "), returning null. Generation request: ";
				int num = 5;
				PawnGenerationRequest pawnGenerationRequest2 = request;
				array[num] = pawnGenerationRequest2.ToString();
				Log.Error(string.Concat(array));
				return null;
			}
			return pawn;
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x001024AC File Offset: 0x001006AC
		private static Pawn TryGenerateNewPawnInternal(ref PawnGenerationRequest request, out string error, bool ignoreScenarioRequirements, bool ignoreValidator)
		{
			error = null;
			Pawn pawn = (Pawn)ThingMaker.MakeThing(request.KindDef.race, null);
			PawnGenerator.pawnsBeingGenerated.Add(new PawnGenerator.PawnGenerationStatus(pawn, null, request.ForceDead || request.AllowDead));
			Pawn result;
			try
			{
				pawn.kindDef = request.KindDef;
				pawn.SetFactionDirect(request.Faction);
				PawnComponentsUtility.CreateInitialComponents(pawn);
				if (pawn.IsCreepJoiner && (!request.IsCreepJoiner || !ModsConfig.AnomalyActive))
				{
					error = string.Format("Attempted to generate creepjoiner kindDef ({0}) when request didn't enable creepjoiners", pawn.kindDef);
					result = null;
				}
				else
				{
					if (request.FixedGender != null)
					{
						pawn.gender = request.FixedGender.Value;
					}
					else if (request.KindDef.fixedGender != null)
					{
						pawn.gender = request.KindDef.fixedGender.Value;
					}
					else if (pawn.RaceProps.hasGenders)
					{
						if (pawn.RaceProps.forceGender != Gender.None)
						{
							pawn.gender = pawn.RaceProps.forceGender;
						}
						else if (Rand.Value < 0.5f)
						{
							pawn.gender = Gender.Male;
						}
						else
						{
							pawn.gender = Gender.Female;
						}
					}
					else
					{
						pawn.gender = Gender.None;
					}
					PawnGenerator.GenerateRandomAge(pawn, request);
					pawn.needs.SetInitialLevels();
					if (request.AllowedDevelopmentalStages.Newborn())
					{
						Pawn_NeedsTracker needs = pawn.needs;
						if (((needs != null) ? needs.food : null) != null)
						{
							pawn.needs.food.CurLevelPercentage = Mathf.Lerp(pawn.needs.food.PercentageThreshHungry, pawn.needs.food.PercentageThreshUrgentlyHungry, 0.5f);
						}
						Pawn_NeedsTracker needs2 = pawn.needs;
						if (((needs2 != null) ? needs2.rest : null) != null)
						{
							pawn.needs.rest.CurLevelPercentage = Mathf.Lerp(0.28f, 0.14f, 0.5f);
						}
					}
					if (pawn.RaceProps.Humanlike)
					{
						Faction faction;
						Faction faction2;
						if (request.Faction != null)
						{
							faction = request.Faction;
						}
						else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction2, false, true, TechLevel.Undefined, TechLevel.Undefined, false, false))
						{
							faction = faction2;
						}
						else
						{
							faction = Faction.OfAncients;
						}
						pawn.story.skinColorOverride = pawn.kindDef.skinColorOverride;
						pawn.story.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
						where x.randomChosen
						select x);
						if (ModsConfig.IdeologyActive)
						{
							PawnKindDef kindDef = request.KindDef;
							if (((kindDef != null) ? kindDef.favoriteColor : null) != null)
							{
								pawn.story.favoriteColor = request.KindDef.favoriteColor;
							}
							else
							{
								pawn.story.favoriteColor = DefDatabase<ColorDef>.AllDefs.Where(delegate(ColorDef x)
								{
									ColorType colorType = x.colorType;
									return colorType == ColorType.Ideo || colorType == ColorType.Misc;
								}).RandomElement<ColorDef>();
							}
						}
						XenotypeDef xenotypeDef = ModsConfig.BiotechActive ? PawnGenerator.GetXenotypeForGeneratedPawn(request) : null;
						if (ModsConfig.BiotechActive && request.Faction == null && xenotypeDef == XenotypeDefOf.Baseliner && request.ForcedCustomXenotype == null)
						{
							PawnGenerator.AdjustXenotypeForFactionlessPawn(pawn, ref request, ref xenotypeDef);
						}
						PawnBioAndNameGenerator.GiveAppropriateBioAndNameTo(pawn, faction.def, request, xenotypeDef);
						if (pawn.story != null)
						{
							if (request.FixedBirthName != null)
							{
								pawn.story.birthLastName = request.FixedBirthName;
							}
							else
							{
								NameTriple nameTriple = pawn.Name as NameTriple;
								if (nameTriple != null)
								{
									pawn.story.birthLastName = nameTriple.Last;
								}
							}
						}
						PawnGenerator.GenerateTraits(pawn, request);
						PawnGenerator.GenerateBodyType(pawn, request);
						PawnGenerator.GenerateGenes(pawn, xenotypeDef, request);
						PawnGenerator.GenerateSkills(pawn, request);
					}
					if (!request.AllowedDevelopmentalStages.Newborn() && request.CanGeneratePawnRelations)
					{
						PawnGenerator.GeneratePawnRelations(pawn, ref request);
					}
					if (pawn.RaceProps.Animal)
					{
						Faction faction3 = request.Faction;
						if (faction3 != null && faction3.IsPlayer)
						{
							pawn.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
							pawn.training.Train(TrainableDefOf.Tameness, null, true);
						}
					}
					if (!request.ForbidAnyTitle && pawn.Faction != null && !request.AllowedDevelopmentalStages.Newborn())
					{
						RoyalTitleDef royalTitleDef = request.FixedTitle;
						if (royalTitleDef == null)
						{
							if (request.KindDef.titleRequired != null)
							{
								royalTitleDef = request.KindDef.titleRequired;
							}
							else if (!request.KindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(request.KindDef.royalTitleChance))
							{
								royalTitleDef = request.KindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
							}
						}
						if (request.KindDef.minTitleRequired != null && (royalTitleDef == null || royalTitleDef.seniority < request.KindDef.minTitleRequired.seniority))
						{
							royalTitleDef = request.KindDef.minTitleRequired;
						}
						if (royalTitleDef != null)
						{
							Faction faction4 = (request.Faction != null && request.Faction.def.HasRoyalTitles) ? request.Faction : Find.FactionManager.RandomRoyalFaction(false, false, true, TechLevel.Undefined);
							pawn.royalty.SetTitle(faction4, royalTitleDef, false, false, true);
							if (request.Faction != null && !request.Faction.IsPlayer)
							{
								PawnGenerator.PurchasePermits(pawn, faction4);
							}
							int amount = 0;
							if (royalTitleDef.GetNextTitle(faction4) != null)
							{
								amount = Rand.Range(0, royalTitleDef.GetNextTitle(faction4).favorCost - 1);
							}
							pawn.royalty.SetFavor(faction4, amount, true);
							if (royalTitleDef.maxPsylinkLevel > 0)
							{
								Hediff_Level hediff_Level = HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, pawn.health.hediffSet.GetBrain()) as Hediff_Level;
								pawn.health.AddHediff(hediff_Level, null, null, null);
								hediff_Level.SetLevelTo(royalTitleDef.maxPsylinkLevel);
							}
						}
					}
					if (pawn.royalty != null)
					{
						pawn.royalty.allowRoomRequirements = request.KindDef.allowRoyalRoomRequirements;
						pawn.royalty.allowApparelRequirements = request.KindDef.allowRoyalApparelRequirements;
					}
					if (pawn.guest != null)
					{
						pawn.guest.RandomizeJoinStatus();
					}
					if (pawn.workSettings != null)
					{
						Faction faction5 = request.Faction;
						if (faction5 != null && faction5.IsPlayer)
						{
							pawn.workSettings.EnableAndInitialize();
						}
					}
					if (request.Faction != null && (pawn.RaceProps.Animal || (ModsConfig.BiotechActive && pawn.RaceProps.IsMechanoid)))
					{
						pawn.GenerateNecessaryName();
					}
					if (!pawn.kindDef.preventIdeo && pawn.ideo != null && !pawn.DevelopmentalStage.Baby())
					{
						if (request.FixedIdeo != null)
						{
							pawn.ideo.SetIdeo(request.FixedIdeo);
						}
						else
						{
							Faction faction6 = request.Faction;
							Ideo ideo;
							if (((faction6 != null) ? faction6.ideos : null) != null)
							{
								pawn.ideo.SetIdeo(request.Faction.ideos.GetRandomIdeoForNewPawn());
							}
							else if (Find.IdeoManager.IdeosListForReading.TryRandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(pawn, x), out ideo))
							{
								pawn.ideo.SetIdeo(ideo);
							}
						}
					}
					if (pawn.mindState != null)
					{
						pawn.mindState.SetupLastHumanMeatTick();
					}
					if (pawn.surroundings != null)
					{
						pawn.surroundings.Clear();
					}
					PawnGenerator.GenerateInitialHediffs(pawn, request);
					if (request.ForceDead && !pawn.Dead)
					{
						pawn.Kill(null, null);
					}
					if (pawn.RaceProps.Humanlike)
					{
						pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(pawn);
						if (pawn.style != null)
						{
							pawn.style.beardDef = PawnStyleItemChooser.RandomBeardFor(pawn);
							if (ModsConfig.IdeologyActive && !pawn.DevelopmentalStage.Baby())
							{
								pawn.style.FaceTattoo = PawnStyleItemChooser.RandomTattooFor(pawn, TattooType.Face);
								pawn.style.BodyTattoo = PawnStyleItemChooser.RandomTattooFor(pawn, TattooType.Body);
							}
							else
							{
								pawn.style.SetupTattoos_NoIdeology();
							}
						}
					}
					if (!request.KindDef.abilities.NullOrEmpty<AbilityDef>())
					{
						for (int i = 0; i < request.KindDef.abilities.Count; i++)
						{
							pawn.abilities.GainAbility(request.KindDef.abilities[i]);
						}
					}
					if (Find.Scenario != null)
					{
						Find.Scenario.Notify_NewPawnGenerating(pawn, request.Context);
					}
					if (!request.AllowDead && !request.ForceDead && (pawn.Dead || pawn.Destroyed))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated dead pawn.";
						result = null;
					}
					else if (!request.AllowDowned && !request.ForceDead && pawn.Downed)
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated downed pawn.";
						result = null;
					}
					else if (request.MustBeCapableOfViolence && !pawn.RaceProps.alwaysViolent && ((pawn.story != null && pawn.WorkTagIsDisabled(WorkTags.Violent)) || (!pawn.RaceProps.IsMechanoid && pawn.RaceProps.ToolUser && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn incapable of violence.";
						result = null;
					}
					else
					{
						if (request.KindDef != null && !request.KindDef.skills.NullOrEmpty<SkillRange>())
						{
							List<SkillRange> skills = request.KindDef.skills;
							for (int j = 0; j < skills.Count; j++)
							{
								if (pawn.skills.GetSkill(skills[j].Skill).TotallyDisabled)
								{
									error = "Generated pawn incapable of required skill: " + skills[j].Skill.defName;
									return null;
								}
							}
						}
						if (request.KindDef.requiredWorkTags != WorkTags.None && (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) != WorkTags.None)
						{
							PawnGenerator.DiscardGeneratedPawn(pawn);
							error = "Generated pawn with disabled requiredWorkTags.";
							result = null;
						}
						else if (!PawnGenerator.HasCorrectMinBestSkillLevel(pawn, request.KindDef))
						{
							PawnGenerator.DiscardGeneratedPawn(pawn);
							error = "Generated pawn with too low best skill level.";
							result = null;
						}
						else if (!PawnGenerator.HasCorrectMinTotalSkillLevels(pawn, request.KindDef))
						{
							PawnGenerator.DiscardGeneratedPawn(pawn);
							error = "Generated pawn with bad skills.";
							result = null;
						}
						else if (!ignoreScenarioRequirements && request.Context == PawnGenerationContext.PlayerStarter && Find.Scenario != null && !Find.Scenario.AllowPlayerStartingPawn(pawn, false, request))
						{
							PawnGenerator.DiscardGeneratedPawn(pawn);
							error = "Generated pawn doesn't meet scenario requirements.";
							result = null;
						}
						else if (!ignoreValidator && request.ValidatorPreGear != null && !request.ValidatorPreGear(pawn))
						{
							PawnGenerator.DiscardGeneratedPawn(pawn);
							error = "Generated pawn didn't pass validator check (pre-gear).";
							result = null;
						}
						else
						{
							if (!request.ForceNoGear && (!request.AllowedDevelopmentalStages.Newborn() || pawn.RaceProps.IsMechanoid))
							{
								PawnGenerator.GenerateGearFor(pawn, request);
							}
							if (request.ForceDead && pawn.Dead)
							{
								pawn.apparel.Notify_PawnKilled(null);
							}
							if (!ignoreValidator && request.ValidatorPostGear != null && !request.ValidatorPostGear(pawn))
							{
								PawnGenerator.DiscardGeneratedPawn(pawn);
								error = "Generated pawn didn't pass validator check (post-gear).";
								result = null;
							}
							else
							{
								if (ModsConfig.AnomalyActive)
								{
									if (request.ForcedMutant != null)
									{
										MutantUtility.SetFreshPawnAsMutant(pawn, request.ForcedMutant);
									}
									else if (request.KindDef.mutant != null)
									{
										MutantUtility.SetFreshPawnAsMutant(pawn, request.KindDef.mutant);
									}
								}
								for (int k = 0; k < PawnGenerator.pawnsBeingGenerated.Count - 1; k++)
								{
									if (PawnGenerator.pawnsBeingGenerated[k].PawnsGeneratedInTheMeantime == null)
									{
										PawnGenerator.pawnsBeingGenerated[k] = new PawnGenerator.PawnGenerationStatus(PawnGenerator.pawnsBeingGenerated[k].Pawn, new List<Pawn>(), PawnGenerator.pawnsBeingGenerated[k].AllowsDead);
									}
									PawnGenerator.pawnsBeingGenerated[k].PawnsGeneratedInTheMeantime.Add(pawn);
								}
								if (pawn.Faction != null)
								{
									pawn.Faction.Notify_PawnJoined(pawn);
								}
								PawnGenerator.ApplyMiscDamage(pawn, request.KindDef);
								Pawn_DrawTracker drawer = pawn.Drawer;
								if (drawer != null)
								{
									PawnRenderer renderer = drawer.renderer;
									if (renderer != null)
									{
										renderer.SetAllGraphicsDirty();
									}
								}
								result = pawn;
							}
						}
					}
				}
			}
			finally
			{
				PawnGenerator.pawnsBeingGenerated.RemoveLast<PawnGenerator.PawnGenerationStatus>();
			}
			return result;
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x00103360 File Offset: 0x00101560
		private static void ApplyMiscDamage(Pawn pawn, PawnKindDef def)
		{
			foreach (MiscDamage miscDamage in def.existingDamage)
			{
				if (miscDamage.chance == null || Rand.Chance(miscDamage.chance.Value))
				{
					foreach (BodyPartGroupDef bodyPartGroupDef in miscDamage.groups)
					{
						foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
						{
							if (bodyPartRecord.def.canScarify && (bodyPartGroupDef == null || bodyPartRecord.IsInGroup(bodyPartGroupDef)))
							{
								Hediff hediff = HediffMaker.MakeHediff(miscDamage.damage.hediff, pawn, bodyPartRecord);
								HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff.TryGetComp<HediffComp_GetsPermanent>();
								hediffComp_GetsPermanent.IsPermanent = true;
								hediffComp_GetsPermanent.SetPainCategory(PainCategory.Painless);
								pawn.health.AddHediff(hediff, null, null, null);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x001034E4 File Offset: 0x001016E4
		private static void PurchasePermits(Pawn pawn, Faction faction)
		{
			int num = 200;
			Func<RoyalTitlePermitDef, bool> <>9__0;
			do
			{
				IEnumerable<RoyalTitlePermitDef> allDefs = DefDatabase<RoyalTitlePermitDef>.AllDefs;
				Func<RoyalTitlePermitDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((RoyalTitlePermitDef x) => x.permitPointCost > 0 && x.AvailableForPawn(pawn, faction) && !x.IsPrerequisiteOfHeldPermit(pawn, faction)));
				}
				IEnumerable<RoyalTitlePermitDef> source = allDefs.Where(predicate);
				if (!source.Any<RoyalTitlePermitDef>())
				{
					return;
				}
				pawn.royalty.AddPermit(source.RandomElement<RoyalTitlePermitDef>(), faction);
				num--;
			}
			while (num > 0);
			Log.ErrorOnce("PurchasePermits exceeded max iterations.", 947492);
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x00103570 File Offset: 0x00101770
		private static void DiscardGeneratedPawn(Pawn pawn)
		{
			if (Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.RemovePawn(pawn);
			}
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			List<Pawn> pawnsGeneratedInTheMeantime = PawnGenerator.pawnsBeingGenerated.Last<PawnGenerator.PawnGenerationStatus>().PawnsGeneratedInTheMeantime;
			if (pawnsGeneratedInTheMeantime != null)
			{
				for (int i = 0; i < pawnsGeneratedInTheMeantime.Count; i++)
				{
					Pawn pawn2 = pawnsGeneratedInTheMeantime[i];
					if (Find.WorldPawns.Contains(pawn2))
					{
						Find.WorldPawns.RemovePawn(pawn2);
					}
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					for (int j = 0; j < PawnGenerator.pawnsBeingGenerated.Count; j++)
					{
						PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x00103628 File Offset: 0x00101828
		private static IEnumerable<Pawn> GetValidCandidatesToRedress(PawnGenerationRequest request)
		{
			IEnumerable<Pawn> enumerable = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Free);
			if (request.KindDef.factionLeader)
			{
				enumerable = enumerable.Concat(Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.FactionLeader));
			}
			return from x in enumerable
			where PawnGenerator.IsValidCandidateToRedress(x, request)
			select x;
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x00103688 File Offset: 0x00101888
		private static float ChanceToRedressAnyWorldPawn(PawnGenerationRequest request)
		{
			int pawnsBySituationCount = Find.WorldPawns.GetPawnsBySituationCount(WorldPawnSituation.Free);
			float num = Mathf.Min(0.02f + 0.01f * ((float)pawnsBySituationCount / 10f), 0.8f);
			if (request.MinChanceToRedressWorldPawn != null)
			{
				num = Mathf.Max(num, request.MinChanceToRedressWorldPawn.Value);
			}
			return num;
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x001036E8 File Offset: 0x001018E8
		private static float WorldPawnSelectionWeight(Pawn p)
		{
			if (p.RaceProps.IsFlesh && !p.relations.everSeenByPlayer && p.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return 0.1f;
			}
			return 1f;
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x0010371C File Offset: 0x0010191C
		private static void GenerateGearFor(Pawn pawn, PawnGenerationRequest request)
		{
			PawnApparelGenerator.GenerateStartingApparelFor(pawn, request);
			PawnInventoryGenerator.GenerateInventoryFor(pawn, request);
			if (!request.DontGiveWeapon)
			{
				PawnWeaponGenerator.TryGenerateWeaponFor(pawn, request);
			}
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x0010373C File Offset: 0x0010193C
		private static void GenerateInitialHediffs(Pawn pawn, PawnGenerationRequest request)
		{
			int num = 0;
			do
			{
				if (!request.AllowedDevelopmentalStages.Newborn())
				{
					AgeInjuryUtility.GenerateRandomOldAgeInjuries(pawn, !request.AllowDead && !request.ForceDead);
					PawnTechHediffsGenerator.GenerateTechHediffsFor(pawn);
				}
				if (!pawn.kindDef.missingParts.NullOrEmpty<MissingPart>())
				{
					using (List<MissingPart>.Enumerator enumerator = pawn.kindDef.missingParts.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MissingPart t = enumerator.Current;
							if (t.Chance == null || Rand.Chance(t.Chance.Value))
							{
								Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
								if (t.Injury != null)
								{
									hediff_MissingPart.lastInjury = t.Injury;
								}
								List<BodyPartRecord> list = (from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
								where x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryChanceFactor != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x) && x.def == t.BodyPart
								select x).ToList<BodyPartRecord>();
								if (list.Any<BodyPartRecord>())
								{
									hediff_MissingPart.Part = list.RandomElement<BodyPartRecord>();
									pawn.health.AddHediff(hediff_MissingPart, null, null, null);
								}
							}
						}
					}
				}
				if (request.AllowAddictions && pawn.DevelopmentalStage.Adult())
				{
					PawnAddictionHediffsGenerator.GenerateAddictionsAndTolerancesFor(pawn);
				}
				if (!request.AllowedDevelopmentalStages.Newborn() && !request.AllowedDevelopmentalStages.Baby())
				{
					PawnGenerator.AddRequiredScars(pawn);
					PawnGenerator.AddBlindness(pawn);
				}
				if (ModsConfig.BiotechActive && !pawn.Dead && pawn.gender == Gender.Female)
				{
					float chance = pawn.kindDef.humanPregnancyChance * PregnancyUtility.PregnancyChanceForPawn(pawn);
					if (Find.Storyteller.difficulty.ChildrenAllowed && pawn.ageTracker.AgeBiologicalYears >= 16 && request.AllowPregnant && Rand.Chance(chance))
					{
						Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.PregnantHuman, pawn, null);
						hediff_Pregnant.Severity = PregnancyUtility.GeneratedPawnPregnancyProgressRange.RandomInRange;
						Pawn father = null;
						if (!Rand.Chance(0.2f))
						{
							DirectPawnRelation directPawnRelation;
							if ((from r in pawn.relations.DirectRelations
							where PregnancyUtility.BeingFatherWeightPerRelation.ContainsKey(r.def)
							select r).TryRandomElementByWeight((DirectPawnRelation r) => PregnancyUtility.BeingFatherWeightPerRelation[r.def], out directPawnRelation))
							{
								father = directPawnRelation.otherPawn;
							}
						}
						bool flag;
						GeneSet inheritedGeneSet = PregnancyUtility.GetInheritedGeneSet(father, pawn, out flag);
						if (flag)
						{
							hediff_Pregnant.SetParents(null, father, inheritedGeneSet);
							pawn.health.AddHediff(hediff_Pregnant, null, null, null);
						}
					}
					else if (pawn.RaceProps.Humanlike && pawn.ageTracker.AgeBiologicalYears >= 20)
					{
						BackstoryDef adulthood = pawn.story.Adulthood;
						if (adulthood == null || !adulthood.spawnCategories.Contains("Tribal"))
						{
							if (Rand.Chance(0.005f))
							{
								pawn.health.AddHediff(HediffDefOf.Sterilized, null, null, null);
							}
							else if (Rand.Chance(0.005f))
							{
								pawn.health.AddHediff(HediffDefOf.ImplantedIUD, null, null, null);
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && pawn.RaceProps.Humanlike && !pawn.Dead && pawn.gender == Gender.Male && Find.Storyteller.difficulty.ChildrenAllowed && pawn.ageTracker.AgeBiologicalYears >= 20)
				{
					BackstoryDef adulthood2 = pawn.story.Adulthood;
					if (adulthood2 == null || !adulthood2.spawnCategories.Contains("Tribal"))
					{
						if (Rand.Chance(0.005f))
						{
							pawn.health.AddHediff(HediffDefOf.Sterilized, null, null, null);
						}
						else if (Rand.Chance(0.005f))
						{
							pawn.health.AddHediff(HediffDefOf.Vasectomy, null, null, null);
						}
					}
				}
				if (!pawn.kindDef.startingHediffs.NullOrEmpty<StartingHediff>() && !request.AllowedDevelopmentalStages.Newborn())
				{
					HealthUtility.AddStartingHediffs(pawn, pawn.kindDef.startingHediffs);
				}
				if (((request.AllowDead || request.ForceDead) && pawn.Dead) || request.AllowDowned || request.ForceDead || !pawn.Downed)
				{
					goto IL_62A;
				}
				pawn.health.Reset();
				num++;
			}
			while (num <= 80);
			string[] array = new string[8];
			array[0] = "Could not generate old age injuries for ";
			array[1] = pawn.ThingID;
			array[2] = " of age ";
			array[3] = pawn.ageTracker.AgeBiologicalYears.ToString();
			array[4] = " that allow pawn to move after ";
			array[5] = 80.ToString();
			array[6] = " tries. request=";
			int num2 = 7;
			PawnGenerationRequest pawnGenerationRequest = request;
			array[num2] = pawnGenerationRequest.ToString();
			Log.Warning(string.Concat(array));
			IL_62A:
			if (!pawn.Dead && (request.Faction == null || !request.Faction.IsPlayer))
			{
				int num3 = 0;
				while (pawn.health.HasHediffsNeedingTend(false))
				{
					num3++;
					if (num3 > 10000)
					{
						Log.Error("Too many iterations.");
						break;
					}
					TendUtility.DoTend(null, pawn, null);
				}
			}
			pawn.health.CheckForStateChange(null, null);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x00103E10 File Offset: 0x00102010
		private static void GenerateRandomAge(Pawn pawn, PawnGenerationRequest request)
		{
			PawnGenerator.<>c__DisplayClass37_0 CS$<>8__locals1;
			CS$<>8__locals1.request = request;
			CS$<>8__locals1.pawn = pawn;
			if (CS$<>8__locals1.request.AllowedDevelopmentalStages.Newborn())
			{
				CS$<>8__locals1.pawn.ageTracker.AgeBiologicalTicks = 0L;
				CS$<>8__locals1.pawn.babyNamingDeadline = Find.TickManager.TicksGame + 60000;
			}
			else if (CS$<>8__locals1.request.FixedBiologicalAge != null)
			{
				CS$<>8__locals1.pawn.ageTracker.AgeBiologicalTicks = (long)(CS$<>8__locals1.request.FixedBiologicalAge.Value * 3600000f);
			}
			else
			{
				PawnGenerator.<>c__DisplayClass37_1 CS$<>8__locals2;
				CS$<>8__locals2.years = 0f;
				int num = 0;
				for (;;)
				{
					if (CS$<>8__locals1.request.AllowedDevelopmentalStages == DevelopmentalStage.Baby)
					{
						CS$<>8__locals2.years = Rand.Range(0f, LifeStageUtility.GetMaxBabyAge(CS$<>8__locals1.pawn.RaceProps));
					}
					else if (CS$<>8__locals1.pawn.RaceProps.ageGenerationCurve != null)
					{
						CS$<>8__locals2.years = Rand.ByCurve(CS$<>8__locals1.pawn.RaceProps.ageGenerationCurve);
					}
					else if (CS$<>8__locals1.pawn.RaceProps.IsMechanoid)
					{
						CS$<>8__locals2.years = Rand.Range(0f, 2500f);
					}
					else
					{
						CS$<>8__locals2.years = Rand.ByCurve(PawnGenerator.DefaultAgeGenerationCurve) * CS$<>8__locals1.pawn.RaceProps.lifeExpectancy;
					}
					num++;
					if (num > 300)
					{
						break;
					}
					if (PawnGenerator.<GenerateRandomAge>g__AgeAllowed|37_0(CS$<>8__locals1.pawn, CS$<>8__locals2.years, ref CS$<>8__locals1, ref CS$<>8__locals2))
					{
						goto IL_1A0;
					}
				}
				string str = "Tried 300 times to generate age for ";
				Pawn pawn2 = CS$<>8__locals1.pawn;
				Log.Error(str + ((pawn2 != null) ? pawn2.ToString() : null));
				IL_1A0:
				CS$<>8__locals1.pawn.ageTracker.AgeBiologicalTicks = (long)(CS$<>8__locals2.years * 3600000f);
			}
			if (CS$<>8__locals1.request.AllowedDevelopmentalStages.Newborn())
			{
				CS$<>8__locals1.pawn.ageTracker.AgeChronologicalTicks = 0L;
			}
			else if (CS$<>8__locals1.request.FixedChronologicalAge != null)
			{
				CS$<>8__locals1.pawn.ageTracker.AgeChronologicalTicks = (long)(CS$<>8__locals1.request.FixedChronologicalAge.Value * 3600000f);
			}
			else if (CS$<>8__locals1.request.KindDef.chronologicalAgeRange != null)
			{
				CS$<>8__locals1.pawn.ageTracker.AgeChronologicalTicks = (long)(CS$<>8__locals1.request.KindDef.chronologicalAgeRange.Value.RandomInRange * 3600000f);
			}
			else
			{
				int num2;
				if (CS$<>8__locals1.request.CertainlyBeenInCryptosleep || Rand.Value < CS$<>8__locals1.pawn.kindDef.backstoryCryptosleepCommonality)
				{
					float value = Rand.Value;
					if (value < 0.7f)
					{
						num2 = Rand.Range(0, 100);
					}
					else if (value < 0.95f)
					{
						num2 = Rand.Range(100, 1000);
					}
					else
					{
						int maxExclusive = GenDate.Year((long)GenTicks.TicksAbs, 0f) - 2026 - CS$<>8__locals1.pawn.ageTracker.AgeBiologicalYears;
						num2 = Rand.Range(1000, maxExclusive);
					}
				}
				else
				{
					num2 = 0;
				}
				long num3 = (long)GenTicks.TicksAbs - CS$<>8__locals1.pawn.ageTracker.AgeBiologicalTicks;
				num3 -= (long)num2 * 3600000L;
				CS$<>8__locals1.pawn.ageTracker.BirthAbsTicks = num3;
			}
			if (CS$<>8__locals1.pawn.ageTracker.AgeBiologicalTicks > CS$<>8__locals1.pawn.ageTracker.AgeChronologicalTicks)
			{
				CS$<>8__locals1.pawn.ageTracker.AgeChronologicalTicks = CS$<>8__locals1.pawn.ageTracker.AgeBiologicalTicks;
			}
			CS$<>8__locals1.pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, true);
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001041C4 File Offset: 0x001023C4
		public static int RandomTraitDegree(TraitDef traitDef)
		{
			if (traitDef.degreeDatas.Count == 1)
			{
				return traitDef.degreeDatas[0].degree;
			}
			return traitDef.degreeDatas.RandomElementByWeight((TraitDegreeData dd) => dd.commonality).degree;
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x00104220 File Offset: 0x00102420
		private static void GenerateTraits(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.story == null || request.AllowedDevelopmentalStages.Newborn())
			{
				return;
			}
			if (pawn.kindDef.forcedTraits != null)
			{
				foreach (TraitRequirement traitRequirement in pawn.kindDef.forcedTraits)
				{
					pawn.story.traits.GainTrait(new Trait(traitRequirement.def, traitRequirement.degree.GetValueOrDefault(), true), false);
				}
			}
			if (request.ForcedTraits != null)
			{
				foreach (TraitDef traitDef in request.ForcedTraits)
				{
					if (traitDef != null && !pawn.story.traits.HasTrait(traitDef))
					{
						pawn.story.traits.GainTrait(new Trait(traitDef, 0, true), false);
					}
				}
			}
			BackstoryDef childhood = pawn.story.Childhood;
			if (((childhood != null) ? childhood.forcedTraits : null) != null)
			{
				List<BackstoryTrait> forcedTraits = pawn.story.Childhood.forcedTraits;
				for (int i = 0; i < forcedTraits.Count; i++)
				{
					BackstoryTrait te = forcedTraits[i];
					if (te.def == null)
					{
						string str = "Null forced trait def on ";
						BackstoryDef childhood2 = pawn.story.Childhood;
						Log.Error(str + ((childhood2 != null) ? childhood2.ToString() : null));
					}
					else if (!request.KindDef.disallowedTraits.NotNullAndContains(te.def) && (request.KindDef.disallowedTraitsWithDegree == null || !request.KindDef.disallowedTraitsWithDegree.Any((TraitRequirement t) => t.def == te.def && t.degree == null)) && !pawn.story.traits.HasTrait(te.def) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(te.def)))
					{
						pawn.story.traits.GainTrait(new Trait(te.def, te.degree, false), false);
					}
				}
			}
			if (pawn.story.Adulthood != null && pawn.story.Adulthood.forcedTraits != null)
			{
				List<BackstoryTrait> forcedTraits2 = pawn.story.Adulthood.forcedTraits;
				for (int j = 0; j < forcedTraits2.Count; j++)
				{
					BackstoryTrait te = forcedTraits2[j];
					if (te.def == null)
					{
						string str2 = "Null forced trait def on ";
						BackstoryDef adulthood = pawn.story.Adulthood;
						Log.Error(str2 + ((adulthood != null) ? adulthood.ToString() : null));
					}
					else if (!request.KindDef.disallowedTraits.NotNullAndContains(te.def) && (request.KindDef.disallowedTraitsWithDegree == null || !request.KindDef.disallowedTraitsWithDegree.Any((TraitRequirement t) => t.def == te.def && t.degree == null)) && !pawn.story.traits.HasTrait(te.def) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(te.def)))
					{
						pawn.story.traits.GainTrait(new Trait(te.def, te.degree, false), false);
					}
				}
			}
			int minimumAgeTraits = request.MinimumAgeTraits;
			int maximumAgeTraits = request.MaximumAgeTraits;
			int count = pawn.story.traits.allTraits.Count;
			int num = Mathf.Min(GrowthUtility.GrowthMomentAges.Length, PawnGenerator.TraitsCountRange.RandomInRange);
			int ageBiologicalYears = pawn.ageTracker.AgeBiologicalYears;
			if (minimumAgeTraits > 0)
			{
				num = Mathf.Max(num, count + minimumAgeTraits);
			}
			if (maximumAgeTraits >= 0)
			{
				num = Mathf.Min(num, count + maximumAgeTraits);
			}
			int num2 = 3;
			while (num2 <= ageBiologicalYears && pawn.story.traits.allTraits.Count < num)
			{
				if (GrowthUtility.IsGrowthBirthday(num2))
				{
					Trait trait = PawnGenerator.GenerateTraitsFor(pawn, 1, new PawnGenerationRequest?(request), true).FirstOrFallback(null);
					if (trait != null)
					{
						pawn.story.traits.GainTrait(trait, false);
					}
				}
				num2++;
			}
			if (request.AllowGay && (LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender(pawn) || LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender(pawn)))
			{
				Trait trait2 = new Trait(TraitDefOf.Gay, PawnGenerator.RandomTraitDegree(TraitDefOf.Gay), false);
				pawn.story.traits.GainTrait(trait2, false);
			}
			if (!ModsConfig.BiotechActive || pawn.ageTracker.AgeBiologicalYears >= 13)
			{
				PawnGenerator.TryGenerateSexualityTraitFor(pawn, request.AllowGay);
			}
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x00104724 File Offset: 0x00102924
		private static bool HasSexualityTrait(Pawn pawn)
		{
			return pawn.story.traits.HasTrait(TraitDefOf.Gay) || pawn.story.traits.HasTrait(TraitDefOf.Bisexual) || pawn.story.traits.HasTrait(TraitDefOf.Asexual);
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x00104778 File Offset: 0x00102978
		public static void TryGenerateSexualityTraitFor(Pawn pawn, bool allowGay)
		{
			if (PawnGenerator.HasSexualityTrait(pawn))
			{
				return;
			}
			PawnGenerator.tmpTraitChances.Clear();
			float second = (from x in DefDatabase<TraitDef>.AllDefsListForReading
			where !pawn.story.traits.HasTrait(x) && x != TraitDefOf.Gay && x != TraitDefOf.Asexual && x != TraitDefOf.Bisexual
			select x).Sum((TraitDef x) => x.GetGenderSpecificCommonality(pawn.gender));
			PawnGenerator.tmpTraitChances.Add(new Pair<TraitDef, float>(null, second));
			if (allowGay)
			{
				PawnGenerator.tmpTraitChances.Add(new Pair<TraitDef, float>(TraitDefOf.Gay, TraitDefOf.Gay.GetGenderSpecificCommonality(pawn.gender)));
			}
			PawnGenerator.tmpTraitChances.Add(new Pair<TraitDef, float>(TraitDefOf.Bisexual, TraitDefOf.Bisexual.GetGenderSpecificCommonality(pawn.gender)));
			PawnGenerator.tmpTraitChances.Add(new Pair<TraitDef, float>(TraitDefOf.Asexual, TraitDefOf.Asexual.GetGenderSpecificCommonality(pawn.gender)));
			Pair<TraitDef, float> pair;
			if (PawnGenerator.tmpTraitChances.TryRandomElementByWeight((Pair<TraitDef, float> x) => x.Second, out pair) && pair.First != null)
			{
				Trait trait = new Trait(pair.First, PawnGenerator.RandomTraitDegree(pair.First), false);
				pawn.story.traits.GainTrait(trait, false);
			}
			PawnGenerator.tmpTraitChances.Clear();
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x001048D4 File Offset: 0x00102AD4
		public static List<Trait> GenerateTraitsFor(Pawn pawn, int traitCount, PawnGenerationRequest? req = null, bool growthMomentTrait = false)
		{
			List<Trait> list = new List<Trait>();
			int num = 0;
			Func<TraitDef, float> <>9__0;
			Predicate<SkillDef> <>9__2;
			while (list.Count < traitCount && ++num < traitCount + 500)
			{
				PawnGenerator.<>c__DisplayClass43_1 CS$<>8__locals2 = new PawnGenerator.<>c__DisplayClass43_1();
				PawnGenerator.<>c__DisplayClass43_1 CS$<>8__locals3 = CS$<>8__locals2;
				IEnumerable<TraitDef> allDefsListForReading = DefDatabase<TraitDef>.AllDefsListForReading;
				Func<TraitDef, float> weightSelector;
				if ((weightSelector = <>9__0) == null)
				{
					weightSelector = (<>9__0 = ((TraitDef tr) => tr.GetGenderSpecificCommonality(pawn.gender)));
				}
				CS$<>8__locals3.newTraitDef = allDefsListForReading.RandomElementByWeight(weightSelector);
				if (!pawn.story.traits.HasTrait(CS$<>8__locals2.newTraitDef) && !PawnGenerator.TraitListHasDef(list, CS$<>8__locals2.newTraitDef) && (CS$<>8__locals2.newTraitDef != TraitDefOf.Gay || (!LovePartnerRelationUtility.HasAnyLovePartnerOfTheOppositeGender(pawn) && !LovePartnerRelationUtility.HasAnyExLovePartnerOfTheOppositeGender(pawn))) && (!growthMomentTrait || !ModsConfig.BiotechActive || (CS$<>8__locals2.newTraitDef != TraitDefOf.Gay && CS$<>8__locals2.newTraitDef != TraitDefOf.Bisexual && CS$<>8__locals2.newTraitDef != TraitDefOf.Asexual)))
				{
					if (req != null)
					{
						PawnGenerationRequest value = req.Value;
						if (value.KindDef.disallowedTraits.NotNullAndContains(CS$<>8__locals2.newTraitDef) || (value.KindDef.disallowedTraitsWithDegree != null && value.KindDef.disallowedTraitsWithDegree.Any((TraitRequirement t) => t.def == CS$<>8__locals2.newTraitDef && t.degree == null)) || (value.KindDef.requiredWorkTags != WorkTags.None && (CS$<>8__locals2.newTraitDef.disabledWorkTags & value.KindDef.requiredWorkTags) != WorkTags.None) || (CS$<>8__locals2.newTraitDef == TraitDefOf.Gay && !value.AllowGay) || (value.ProhibitedTraits != null && value.ProhibitedTraits.Contains(CS$<>8__locals2.newTraitDef)) || (value.Faction != null && Faction.OfPlayerSilentFail != null && value.Faction.HostileTo(Faction.OfPlayer) && !CS$<>8__locals2.newTraitDef.allowOnHostileSpawn))
						{
							continue;
						}
					}
					if (!pawn.story.traits.allTraits.Any((Trait tr) => CS$<>8__locals2.newTraitDef.ConflictsWith(tr)) && (CS$<>8__locals2.newTraitDef.requiredWorkTypes == null || !pawn.OneOfWorkTypesIsDisabled(CS$<>8__locals2.newTraitDef.requiredWorkTypes)) && !pawn.WorkTagIsDisabled(CS$<>8__locals2.newTraitDef.requiredWorkTags))
					{
						if (CS$<>8__locals2.newTraitDef.forcedPassions != null && pawn.workSettings != null)
						{
							List<SkillDef> forcedPassions = CS$<>8__locals2.newTraitDef.forcedPassions;
							Predicate<SkillDef> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((SkillDef p) => p.IsDisabled(pawn.story.DisabledWorkTagsBackstoryTraitsAndGenes, pawn.GetDisabledWorkTypes(true))));
							}
							if (forcedPassions.Any(predicate))
							{
								continue;
							}
						}
						int degree = PawnGenerator.RandomTraitDegree(CS$<>8__locals2.newTraitDef);
						if ((pawn.story.Childhood == null || !pawn.story.Childhood.DisallowsTrait(CS$<>8__locals2.newTraitDef, degree)) && (pawn.story.Adulthood == null || !pawn.story.Adulthood.DisallowsTrait(CS$<>8__locals2.newTraitDef, degree)))
						{
							CS$<>8__locals2.trait = new Trait(CS$<>8__locals2.newTraitDef, degree, false);
							if ((pawn.kindDef.disallowedTraitsWithDegree.NullOrEmpty<TraitRequirement>() || !pawn.kindDef.disallowedTraitsWithDegree.Any((TraitRequirement t) => t.Matches(CS$<>8__locals2.trait))) && (pawn.mindState == null || pawn.mindState.mentalBreaker == null || (pawn.mindState.mentalBreaker.BreakThresholdMinor + CS$<>8__locals2.trait.OffsetOfStat(StatDefOf.MentalBreakThreshold)) * CS$<>8__locals2.trait.MultiplierOfStat(StatDefOf.MentalBreakThreshold) <= 0.5f))
							{
								list.Add(CS$<>8__locals2.trait);
							}
						}
					}
				}
			}
			if (num >= traitCount + 500)
			{
				Log.Warning(string.Format("Tried to generate {0} traits for {1} over {2} extra times and failed.", traitCount, pawn, 500));
			}
			return list;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x00104CF8 File Offset: 0x00102EF8
		private static bool TraitListHasDef(List<Trait> traits, TraitDef traitDef)
		{
			if (traits.NullOrEmpty<Trait>())
			{
				return false;
			}
			using (List<Trait>.Enumerator enumerator = traits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == traitDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x00104D58 File Offset: 0x00102F58
		private static void GenerateGenes(Pawn pawn, XenotypeDef xenotype, PawnGenerationRequest request)
		{
			if (pawn.genes == null)
			{
				return;
			}
			if (ModsConfig.BiotechActive)
			{
				if (!xenotype.doubleXenotypeChances.NullOrEmpty<XenotypeChance>())
				{
					if (Rand.Value < xenotype.doubleXenotypeChances.Sum((XenotypeChance x) => x.chance))
					{
						XenotypeChance xenotypeChance;
						if (xenotype.doubleXenotypeChances.TryRandomElementByWeight((XenotypeChance x) => x.chance, out xenotypeChance))
						{
							pawn.genes.SetXenotype(xenotypeChance.xenotype);
						}
					}
				}
				pawn.genes.SetXenotype(xenotype);
				if (Rand.Value < xenotype.generateWithXenogermReplicatingHediffChance && xenotype.xenogermReplicatingDurationLeftDaysRange != FloatRange.Zero)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.XenogermReplicating, pawn, null);
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears != null)
					{
						hediffComp_Disappears.ticksToDisappear = Mathf.FloorToInt(xenotype.xenogermReplicatingDurationLeftDaysRange.RandomInRange * 60000f);
					}
					pawn.health.AddHediff(hediff, null, null, null);
				}
				if (request.ForcedCustomXenotype != null)
				{
					pawn.genes.xenotypeName = request.ForcedCustomXenotype.name;
					pawn.genes.iconDef = request.ForcedCustomXenotype.IconDef;
					foreach (GeneDef geneDef in request.ForcedCustomXenotype.genes)
					{
						pawn.genes.AddGene(geneDef, !request.ForcedCustomXenotype.inheritable);
					}
				}
				if (request.ForcedXenogenes != null)
				{
					foreach (GeneDef geneDef2 in request.ForcedXenogenes)
					{
						pawn.genes.AddGene(geneDef2, true);
					}
				}
				if (request.ForcedEndogenes != null)
				{
					foreach (GeneDef geneDef3 in request.ForcedEndogenes)
					{
						pawn.genes.AddGene(geneDef3, false);
					}
				}
			}
			if (pawn.genes.GetMelaninGene() == null)
			{
				GeneDef geneDef4 = PawnSkinColors.RandomSkinColorGene(pawn);
				if (geneDef4 != null)
				{
					pawn.genes.AddGene(geneDef4, false);
				}
			}
			if (pawn.genes.GetHairColorGene() == null)
			{
				GeneDef geneDef5 = PawnHairColors.RandomHairColorGeneFor(pawn);
				if (geneDef5 != null)
				{
					pawn.genes.AddGene(geneDef5, false);
				}
				else
				{
					pawn.story.HairColor = PawnHairColors.RandomHairColor(pawn, pawn.story.SkinColorBase, pawn.ageTracker.AgeBiologicalYears);
					Log.Error("No hair color gene for " + pawn.LabelShort + ". Getting random color as fallback.");
				}
			}
			if (pawn.kindDef.forcedHairColor != null)
			{
				pawn.story.HairColor = pawn.kindDef.forcedHairColor.Value;
				return;
			}
			if (PawnHairColors.HasGreyHair(pawn, pawn.ageTracker.AgeBiologicalYears))
			{
				pawn.story.HairColor = PawnHairColors.RandomGreyHairColor();
			}
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x00105098 File Offset: 0x00103298
		public static XenotypeDef GetXenotypeForGeneratedPawn(PawnGenerationRequest request)
		{
			if (request.ForcedXenotype != null)
			{
				return request.ForcedXenotype;
			}
			if (request.ForcedCustomXenotype != null)
			{
				return XenotypeDefOf.Baseliner;
			}
			if (Rand.Chance(request.ForceBaselinerChance))
			{
				return XenotypeDefOf.Baseliner;
			}
			XenotypeDef result;
			if (request.AllowedXenotypes != null && request.AllowedXenotypes.TryRandomElement(out result))
			{
				return result;
			}
			PawnGenerator.XenotypesAvailableFor(request.KindDef, null, request.Faction);
			if (request.MustBeCapableOfViolence)
			{
				PawnGenerator.tmpXenotypeChances.RemoveAll((KeyValuePair<XenotypeDef, float> x) => !x.Key.canGenerateAsCombatant);
			}
			KeyValuePair<XenotypeDef, float> keyValuePair;
			if (PawnGenerator.tmpXenotypeChances.TryRandomElementByWeight((KeyValuePair<XenotypeDef, float> x) => x.Value, out keyValuePair))
			{
				PawnGenerator.tmpXenotypeChances.Clear();
				return keyValuePair.Key;
			}
			PawnGenerator.tmpXenotypeChances.Clear();
			return XenotypeDefOf.Baseliner;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x0010518C File Offset: 0x0010338C
		public static void AdjustXenotypeForFactionlessPawn(Pawn pawn, ref PawnGenerationRequest request, ref XenotypeDef xenotype)
		{
			List<CustomXenotype> customXenotypes = Current.Game.customXenotypeDatabase.customXenotypes;
			float num = DefDatabase<XenotypeDef>.AllDefs.Sum((XenotypeDef x) => x.factionlessGenerationWeight);
			float num2 = (float)customXenotypes.Count * 2f;
			CustomXenotype forcedCustomXenotype;
			if (Rand.Range(0f, num + num2) <= num2 && customXenotypes.TryRandomElement(out forcedCustomXenotype))
			{
				request.ForcedCustomXenotype = forcedCustomXenotype;
				xenotype = XenotypeDefOf.Baseliner;
				return;
			}
			XenotypeDef xenotypeDef;
			if (DefDatabase<XenotypeDef>.AllDefs.TryRandomElementByWeight((XenotypeDef x) => x.factionlessGenerationWeight, out xenotypeDef))
			{
				xenotype = xenotypeDef;
			}
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x0010523C File Offset: 0x0010343C
		public static Dictionary<XenotypeDef, float> XenotypesAvailableFor(PawnKindDef kind, FactionDef factionDef = null, Faction faction = null)
		{
			PawnGenerator.tmpXenotypeChances.Clear();
			FactionDef factionDef2 = ((faction != null) ? faction.def : null) ?? factionDef;
			if (kind.useFactionXenotypes && ((factionDef2 != null) ? factionDef2.xenotypeSet : null) != null)
			{
				for (int i = 0; i < factionDef2.xenotypeSet.Count; i++)
				{
					PawnGenerator.<XenotypesAvailableFor>g__AddOrAdjust|49_0(factionDef2.xenotypeSet[i]);
				}
			}
			bool flag;
			if (faction == null)
			{
				flag = (null != null);
			}
			else
			{
				FactionIdeosTracker ideos = faction.ideos;
				if (ideos == null)
				{
					flag = (null != null);
				}
				else
				{
					Ideo primaryIdeo = ideos.PrimaryIdeo;
					flag = (((primaryIdeo != null) ? primaryIdeo.memes : null) != null);
				}
			}
			if (flag)
			{
				for (int j = 0; j < faction.ideos.PrimaryIdeo.memes.Count; j++)
				{
					MemeDef memeDef = faction.ideos.PrimaryIdeo.memes[j];
					if (memeDef.xenotypeSet != null)
					{
						for (int k = 0; k < memeDef.xenotypeSet.Count; k++)
						{
							PawnGenerator.<XenotypesAvailableFor>g__AddOrAdjust|49_0(memeDef.xenotypeSet[k]);
						}
					}
				}
			}
			if (kind.xenotypeSet != null)
			{
				for (int l = 0; l < kind.xenotypeSet.Count; l++)
				{
					PawnGenerator.<XenotypesAvailableFor>g__AddOrAdjust|49_0(kind.xenotypeSet[l]);
				}
			}
			float num = 1f - PawnGenerator.tmpXenotypeChances.Sum((KeyValuePair<XenotypeDef, float> x) => x.Value);
			if (num > 0f)
			{
				PawnGenerator.tmpXenotypeChances.Add(XenotypeDefOf.Baseliner, num);
			}
			return PawnGenerator.tmpXenotypeChances;
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x001053B8 File Offset: 0x001035B8
		private static void GenerateBodyType(Pawn pawn, PawnGenerationRequest request)
		{
			pawn.story.bodyType = (request.ForceBodyType ?? PawnGenerator.GetBodyTypeFor(pawn));
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x001053D8 File Offset: 0x001035D8
		public static BodyTypeDef GetBodyTypeFor(Pawn pawn)
		{
			PawnGenerator.tmpBodyTypes.Clear();
			if (ModsConfig.BiotechActive && pawn.DevelopmentalStage.Juvenile())
			{
				if (pawn.DevelopmentalStage == DevelopmentalStage.Baby)
				{
					return BodyTypeDefOf.Baby;
				}
				return BodyTypeDefOf.Child;
			}
			else
			{
				if (ModsConfig.BiotechActive && pawn.genes != null)
				{
					List<Gene> genesListForReading = pawn.genes.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						if (genesListForReading[i].def.bodyType != null)
						{
							PawnGenerator.tmpBodyTypes.Add(genesListForReading[i].def.bodyType.Value.ToBodyType(pawn));
						}
					}
					BodyTypeDef result;
					if (PawnGenerator.tmpBodyTypes.TryRandomElement(out result))
					{
						return result;
					}
				}
				if (pawn.story.Adulthood != null)
				{
					return pawn.story.Adulthood.BodyTypeFor(pawn.gender);
				}
				if (Rand.Value < 0.5f)
				{
					return BodyTypeDefOf.Thin;
				}
				if (pawn.gender != Gender.Female)
				{
					return BodyTypeDefOf.Male;
				}
				return BodyTypeDefOf.Female;
			}
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x001054E0 File Offset: 0x001036E0
		private static void GenerateSkills(Pawn pawn, PawnGenerationRequest request)
		{
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				int level = PawnGenerator.FinalLevelOfSkill(pawn, skillDef, request);
				pawn.skills.GetSkill(skillDef).Level = level;
			}
			PawnGenerator.<>c__DisplayClass53_0 CS$<>8__locals1;
			CS$<>8__locals1.minorPassions = 0;
			CS$<>8__locals1.majorPassions = 0;
			float num = 5f + Mathf.Clamp(Rand.Gaussian(0f, 1f), -4f, 4f);
			while (num >= 1f)
			{
				if (num >= 1.5f && Rand.Bool)
				{
					int num2 = CS$<>8__locals1.majorPassions;
					CS$<>8__locals1.majorPassions = num2 + 1;
					num -= 1.5f;
				}
				else
				{
					int num2 = CS$<>8__locals1.minorPassions;
					CS$<>8__locals1.minorPassions = num2 + 1;
					num -= 1f;
				}
			}
			foreach (SkillRecord skillRecord in pawn.skills.skills)
			{
				if (!skillRecord.TotallyDisabled)
				{
					using (List<Trait>.Enumerator enumerator2 = pawn.story.traits.allTraits.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.RequiresPassion(skillRecord.def))
							{
								PawnGenerator.<GenerateSkills>g__CreatePassion|53_0(skillRecord, true, ref CS$<>8__locals1);
							}
						}
					}
				}
			}
			int ageBiologicalYears = pawn.ageTracker.AgeBiologicalYears;
			if (ageBiologicalYears < 13)
			{
				for (int j = 3; j <= ageBiologicalYears; j++)
				{
					if (GrowthUtility.IsGrowthBirthday(j))
					{
						int num3 = Rand.RangeInclusive(0, 3);
						for (int k = 0; k < num3; k++)
						{
							SkillDef skillDef2 = ChoiceLetter_GrowthMoment.PassionOptions(pawn, 1, true).FirstOrDefault<SkillDef>();
							if (skillDef2 != null)
							{
								SkillRecord skill = pawn.skills.GetSkill(skillDef2);
								skill.passion = skill.passion.IncrementPassion();
							}
						}
					}
				}
				if (ModsConfig.BiotechActive)
				{
					pawn.ageTracker.TrySimulateGrowthPoints();
					return;
				}
			}
			else
			{
				foreach (SkillRecord skillRecord2 in from sr in pawn.skills.skills
				orderby sr.GetLevel(false) descending
				select sr)
				{
					if (!skillRecord2.TotallyDisabled)
					{
						bool flag = false;
						using (List<Trait>.Enumerator enumerator2 = pawn.story.traits.allTraits.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.def.ConflictsWithPassion(skillRecord2.def))
								{
									flag = true;
									break;
								}
							}
						}
						if (ModsConfig.BiotechActive && pawn.genes != null)
						{
							foreach (Gene gene in pawn.genes.GenesListForReading)
							{
								if (gene.Active && gene.def.passionMod != null && gene.def.passionMod.modType == PassionMod.PassionModType.DropAll && gene.def.passionMod.skill == skillRecord2.def)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							PawnGenerator.<GenerateSkills>g__CreatePassion|53_0(skillRecord2, false, ref CS$<>8__locals1);
						}
					}
				}
			}
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x001058B4 File Offset: 0x00103AB4
		private static int FinalLevelOfSkill(Pawn pawn, SkillDef sk, PawnGenerationRequest request)
		{
			if (request.AllowedDevelopmentalStages.Newborn())
			{
				return 0;
			}
			float num;
			if (sk.usuallyDefinedInBackstories)
			{
				num = (float)Rand.RangeInclusive(0, 4);
			}
			else
			{
				num = Rand.ByCurve(PawnGenerator.LevelRandomCurve);
			}
			foreach (BackstoryDef backstoryDef in from bs in pawn.story.AllBackstories
			where bs != null
			select bs)
			{
				foreach (SkillGain skillGain in backstoryDef.skillGains)
				{
					if (skillGain.skill == sk)
					{
						num += (float)skillGain.amount * Rand.Range(1f, 1.4f);
					}
				}
			}
			for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
			{
				Trait trait = pawn.story.traits.allTraits[i];
				if (!trait.Suppressed)
				{
					TraitDegreeData currentData = trait.CurrentData;
					if (!currentData.skillGains.NullOrEmpty<SkillGain>())
					{
						for (int j = 0; j < currentData.skillGains.Count; j++)
						{
							if (currentData.skillGains[j].skill == sk)
							{
								num += (float)currentData.skillGains[j].amount;
								break;
							}
						}
					}
				}
			}
			num *= Rand.Range(1f, PawnGenerator.AgeSkillMaxFactorCurve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears));
			num *= PawnGenerator.AgeSkillFactor.Evaluate((float)pawn.ageTracker.AgeBiologicalYears);
			num = PawnGenerator.LevelFinalAdjustmentCurve.Evaluate(num);
			if (num > 0f)
			{
				num += (float)pawn.kindDef.extraSkillLevels;
			}
			if (pawn.kindDef.skills != null)
			{
				foreach (SkillRange skillRange in pawn.kindDef.skills)
				{
					if (skillRange.Skill == sk)
					{
						if (num < (float)skillRange.Range.min || num > (float)skillRange.Range.max)
						{
							num = (float)skillRange.Range.RandomInRange;
							break;
						}
						break;
					}
				}
			}
			return Mathf.Clamp(Mathf.RoundToInt(num), 0, 20);
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x00105B54 File Offset: 0x00103D54
		public static void PostProcessGeneratedGear(Thing gear, Pawn pawn)
		{
			CompQuality compQuality = gear.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory qualityCategory = QualityUtility.GenerateQualityGeneratingPawn(pawn.kindDef, gear.def);
				if (pawn.royalty != null && pawn.Faction != null)
				{
					RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(pawn.Faction);
					if (currentTitle != null)
					{
						qualityCategory = (QualityCategory)Mathf.Clamp((int)qualityCategory, (int)currentTitle.requiredMinimumApparelQuality, 6);
					}
				}
				compQuality.SetQuality(qualityCategory, new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			if (gear.def.useHitPoints)
			{
				float randomInRange = pawn.kindDef.gearHealthRange.RandomInRange;
				if (randomInRange < 1f)
				{
					int num = Mathf.RoundToInt(randomInRange * (float)gear.MaxHitPoints);
					num = Mathf.Max(1, num);
					gear.HitPoints = num;
				}
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x00105C08 File Offset: 0x00103E08
		private static void GeneratePawnRelations(Pawn pawn, ref PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			Pawn[] array = (from x in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead
			where x.def == pawn.def
			select x).ToArray<Pawn>();
			if (array.Length == 0)
			{
				return;
			}
			int num = 0;
			foreach (Pawn pawn2 in array)
			{
				if (pawn2.Discarded)
				{
					string[] array3 = new string[5];
					array3[0] = "Warning during generating pawn relations for ";
					int num2 = 1;
					Pawn pawn3 = pawn;
					array3[num2] = ((pawn3 != null) ? pawn3.ToString() : null);
					array3[2] = ": Pawn ";
					int num3 = 3;
					Pawn pawn4 = pawn2;
					array3[num3] = ((pawn4 != null) ? pawn4.ToString() : null);
					array3[4] = " is discarded, yet he was yielded by PawnUtility. Discarding a pawn means that he is no longer managed by anything.";
					Log.Warning(string.Concat(array3));
				}
				else if (pawn2.Faction != null && pawn2.Faction.IsPlayer)
				{
					num++;
				}
			}
			float num4 = 45f;
			num4 += (float)num * 2.7f;
			PawnGenerationRequest localReq = request;
			Pair<Pawn, PawnRelationDef> pair = PawnGenerator.GenerateSamples(array, PawnGenerator.relationsGeneratableBlood, 40).RandomElementByWeightWithDefault((Pair<Pawn, PawnRelationDef> x) => x.Second.generationChanceFactor * x.Second.Worker.GenerationChance(pawn, x.First, localReq), num4 * 40f / (float)(array.Length * PawnGenerator.relationsGeneratableBlood.Length));
			if (pair.First != null)
			{
				pair.Second.Worker.CreateRelation(pawn, pair.First, ref request);
			}
			if (pawn.kindDef.generateInitialNonFamilyRelations)
			{
				Pair<Pawn, PawnRelationDef> pair2 = PawnGenerator.GenerateSamples(array, PawnGenerator.relationsGeneratableNonblood, 40).RandomElementByWeightWithDefault((Pair<Pawn, PawnRelationDef> x) => x.Second.generationChanceFactor * x.Second.Worker.GenerationChance(pawn, x.First, localReq), num4 * 40f / (float)(array.Length * PawnGenerator.relationsGeneratableNonblood.Length));
				if (pair2.First != null)
				{
					pair2.Second.Worker.CreateRelation(pawn, pair2.First, ref request);
				}
			}
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x00105DD8 File Offset: 0x00103FD8
		private static Pair<Pawn, PawnRelationDef>[] GenerateSamples(Pawn[] pawns, PawnRelationDef[] relations, int count)
		{
			Pair<Pawn, PawnRelationDef>[] array = new Pair<Pawn, PawnRelationDef>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new Pair<Pawn, PawnRelationDef>(pawns[Rand.Range(0, pawns.Length)], relations[Rand.Range(0, relations.Length)]);
			}
			return array;
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x00105E1C File Offset: 0x0010401C
		private static void AddRequiredScars(Pawn pawn)
		{
			if (pawn.ideo == null || pawn.ideo.Ideo == null || pawn.health == null || (pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp)))
			{
				return;
			}
			if (!Rand.Chance(PawnGenerator.ScarChanceFromAgeYearsCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat)))
			{
				return;
			}
			int num = 0;
			using (List<Hediff>.Enumerator enumerator = pawn.health.hediffSet.hediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == HediffDefOf.Scarification)
					{
						num++;
					}
				}
			}
			int num2 = pawn.ideo.Ideo.RequiredScars;
			if (pawn.Faction != null && pawn.Faction.IsPlayer && !Rand.Chance(0.5f))
			{
				num2 = Rand.RangeInclusive(0, num2 - 1);
			}
			Func<BodyPartRecord, bool> <>9__0;
			for (int i = num; i < num2; i++)
			{
				IEnumerable<BodyPartRecord> partsToApplyOn = JobDriver_Scarify.GetPartsToApplyOn(pawn);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord p) => JobDriver_Scarify.AvailableOnNow(pawn, p)));
				}
				List<BodyPartRecord> list = partsToApplyOn.Where(predicate).ToList<BodyPartRecord>();
				if (list.Count == 0)
				{
					break;
				}
				BodyPartRecord part = list.RandomElement<BodyPartRecord>();
				JobDriver_Scarify.Scarify(pawn, part);
			}
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x00105FD4 File Offset: 0x001041D4
		private static void AddBlindness(Pawn pawn)
		{
			if (pawn.ideo == null || pawn.ideo.Ideo == null || pawn.health == null)
			{
				return;
			}
			if (Rand.Chance(pawn.ideo.Ideo.BlindPawnChance))
			{
				foreach (BodyPartRecord part in pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource))
				{
					if (!pawn.health.hediffSet.PartIsMissing(part))
					{
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
						hediff_MissingPart.lastInjury = HediffDefOf.Cut;
						hediff_MissingPart.Part = part;
						hediff_MissingPart.IsFresh = false;
						pawn.health.AddHediff(hediff_MissingPart, part, null, null);
					}
				}
			}
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x001060BC File Offset: 0x001042BC
		[DebugOutput("Performance", false)]
		public static void PawnGenerationHistogram()
		{
			DebugHistogram debugHistogram = new DebugHistogram((from x in Enumerable.Range(1, 20)
			select (float)x * 10f).ToArray<float>());
			for (int i = 0; i < 100; i++)
			{
				long timestamp = Stopwatch.GetTimestamp();
				Thing thing = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Colonist, null, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				debugHistogram.Add((float)((Stopwatch.GetTimestamp() - timestamp) * 1000L / Stopwatch.Frequency));
				thing.Destroy(DestroyMode.Vanish);
			}
			debugHistogram.Display();
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x001064E4 File Offset: 0x001046E4
		[CompilerGenerated]
		internal static bool <GenerateRandomAge>g__AgeAllowed|37_0(Pawn p, float y, ref PawnGenerator.<>c__DisplayClass37_0 A_2, ref PawnGenerator.<>c__DisplayClass37_1 A_3)
		{
			return y <= (float)p.kindDef.maxGenerationAge && y >= (float)p.kindDef.minGenerationAge && A_2.request.AllowedDevelopmentalStages.Has(LifeStageUtility.CalculateDevelopmentalStage(A_2.pawn, A_3.years)) && (A_2.request.ExcludeBiologicalAgeRange == null || !A_2.request.ExcludeBiologicalAgeRange.Value.Includes(y)) && (A_2.request.BiologicalAgeRange == null || A_2.request.BiologicalAgeRange.Value.Includes(y));
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x001065A4 File Offset: 0x001047A4
		[CompilerGenerated]
		internal static void <XenotypesAvailableFor>g__AddOrAdjust|49_0(XenotypeChance xenotypeChance)
		{
			if (xenotypeChance.xenotype == XenotypeDefOf.Baseliner)
			{
				return;
			}
			if (PawnGenerator.tmpXenotypeChances.ContainsKey(xenotypeChance.xenotype))
			{
				Dictionary<XenotypeDef, float> dictionary = PawnGenerator.tmpXenotypeChances;
				XenotypeDef xenotype = xenotypeChance.xenotype;
				dictionary[xenotype] += xenotypeChance.chance;
				return;
			}
			PawnGenerator.tmpXenotypeChances.Add(xenotypeChance.xenotype, xenotypeChance.chance);
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x0010660C File Offset: 0x0010480C
		[CompilerGenerated]
		internal static void <GenerateSkills>g__CreatePassion|53_0(SkillRecord record, bool force = false, ref PawnGenerator.<>c__DisplayClass53_0 A_2)
		{
			if (A_2.majorPassions > 0)
			{
				record.passion = Passion.Major;
				int num = A_2.majorPassions;
				A_2.majorPassions = num - 1;
				return;
			}
			if (A_2.minorPassions > 0 || force)
			{
				record.passion = Passion.Minor;
				int num = A_2.minorPassions;
				A_2.minorPassions = num - 1;
			}
		}

		// Token: 0x040025CD RID: 9677
		private static List<PawnGenerator.PawnGenerationStatus> pawnsBeingGenerated = new List<PawnGenerator.PawnGenerationStatus>();

		// Token: 0x040025CE RID: 9678
		private static PawnRelationDef[] relationsGeneratableBlood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
		where rel.familyByBloodRelation && rel.generationChanceFactor > 0f
		select rel).ToArray<PawnRelationDef>();

		// Token: 0x040025CF RID: 9679
		private static PawnRelationDef[] relationsGeneratableNonblood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
		where !rel.familyByBloodRelation && rel.generationChanceFactor > 0f
		select rel).ToArray<PawnRelationDef>();

		// Token: 0x040025D0 RID: 9680
		public const float MaxStartMinorMentalBreakThreshold = 0.5f;

		// Token: 0x040025D1 RID: 9681
		public const float JoinAsSlaveChance = 0.75f;

		// Token: 0x040025D2 RID: 9682
		public const float GenerateAllRequiredScarsChance = 0.5f;

		// Token: 0x040025D3 RID: 9683
		private const int MaxTraitGenAttempts = 500;

		// Token: 0x040025D4 RID: 9684
		private const float HumanlikeSterilizationChance = 0.005f;

		// Token: 0x040025D5 RID: 9685
		private const float HumanlikeFemaleIUDChance = 0.005f;

		// Token: 0x040025D6 RID: 9686
		private const float HumanlikeMaleVasectomyChance = 0.005f;

		// Token: 0x040025D7 RID: 9687
		private const int HumanlikeFertilityHediffStartingAge = 20;

		// Token: 0x040025D8 RID: 9688
		private static readonly IntRange TraitsCountRange = new IntRange(1, 3);

		// Token: 0x040025D9 RID: 9689
		private static readonly SimpleCurve ScarChanceFromAgeYearsCurve = new SimpleCurve
		{
			{
				new CurvePoint(5f, 0f),
				true
			},
			{
				new CurvePoint(18f, 1f),
				true
			}
		};

		// Token: 0x040025DA RID: 9690
		private static List<Hediff_MissingPart> tmpMissingParts = new List<Hediff_MissingPart>();

		// Token: 0x040025DB RID: 9691
		private static SimpleCurve DefaultAgeGenerationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.05f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 100f),
				true
			},
			{
				new CurvePoint(0.675f, 100f),
				true
			},
			{
				new CurvePoint(0.75f, 30f),
				true
			},
			{
				new CurvePoint(0.875f, 18f),
				true
			},
			{
				new CurvePoint(1f, 10f),
				true
			},
			{
				new CurvePoint(1.125f, 3f),
				true
			},
			{
				new CurvePoint(1.25f, 0f),
				true
			}
		};

		// Token: 0x040025DC RID: 9692
		public const float MaxGeneratedMechanoidAge = 2500f;

		// Token: 0x040025DD RID: 9693
		private static List<Pair<TraitDef, float>> tmpTraitChances = new List<Pair<TraitDef, float>>();

		// Token: 0x040025DE RID: 9694
		private static Dictionary<XenotypeDef, float> tmpXenotypeChances = new Dictionary<XenotypeDef, float>();

		// Token: 0x040025DF RID: 9695
		private static HashSet<BodyTypeDef> tmpBodyTypes = new HashSet<BodyTypeDef>();

		// Token: 0x040025E0 RID: 9696
		private static readonly SimpleCurve AgeSkillMaxFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 0.7f),
				true
			},
			{
				new CurvePoint(35f, 1f),
				true
			},
			{
				new CurvePoint(60f, 1.6f),
				true
			}
		};

		// Token: 0x040025E1 RID: 9697
		private static readonly SimpleCurve AgeSkillFactor = new SimpleCurve
		{
			{
				new CurvePoint(3f, 0.2f),
				true
			},
			{
				new CurvePoint(18f, 1f),
				true
			}
		};

		// Token: 0x040025E2 RID: 9698
		private static readonly SimpleCurve LevelFinalAdjustmentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 10f),
				true
			},
			{
				new CurvePoint(20f, 16f),
				true
			},
			{
				new CurvePoint(27f, 20f),
				true
			}
		};

		// Token: 0x040025E3 RID: 9699
		private static readonly SimpleCurve LevelRandomCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 150f),
				true
			},
			{
				new CurvePoint(4f, 150f),
				true
			},
			{
				new CurvePoint(5f, 25f),
				true
			},
			{
				new CurvePoint(10f, 5f),
				true
			},
			{
				new CurvePoint(15f, 0f),
				true
			}
		};

		// Token: 0x020007B2 RID: 1970
		private struct PawnGenerationStatus
		{
			// Token: 0x17000935 RID: 2357
			// (get) Token: 0x0600319A RID: 12698 RVA: 0x0010665E File Offset: 0x0010485E
			// (set) Token: 0x0600319B RID: 12699 RVA: 0x00106666 File Offset: 0x00104866
			public Pawn Pawn { readonly get; private set; }

			// Token: 0x17000936 RID: 2358
			// (get) Token: 0x0600319C RID: 12700 RVA: 0x0010666F File Offset: 0x0010486F
			// (set) Token: 0x0600319D RID: 12701 RVA: 0x00106677 File Offset: 0x00104877
			public bool AllowsDead { readonly get; private set; }

			// Token: 0x17000937 RID: 2359
			// (get) Token: 0x0600319E RID: 12702 RVA: 0x00106680 File Offset: 0x00104880
			// (set) Token: 0x0600319F RID: 12703 RVA: 0x00106688 File Offset: 0x00104888
			public List<Pawn> PawnsGeneratedInTheMeantime { readonly get; private set; }

			// Token: 0x060031A0 RID: 12704 RVA: 0x00106691 File Offset: 0x00104891
			public PawnGenerationStatus(Pawn pawn, List<Pawn> pawnsGeneratedInTheMeantime, bool allowsDead)
			{
				this = default(PawnGenerator.PawnGenerationStatus);
				this.Pawn = pawn;
				this.PawnsGeneratedInTheMeantime = pawnsGeneratedInTheMeantime;
				this.AllowsDead = allowsDead;
			}
		}
	}
}
