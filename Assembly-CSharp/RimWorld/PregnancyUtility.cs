using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020023B5 RID: 9141
	public static class PregnancyUtility
	{
		// Token: 0x17001E1A RID: 7706
		// (get) Token: 0x0600C65F RID: 50783 RVA: 0x0039C20E File Offset: 0x0039A40E
		public static FloatRange GeneratedPawnPregnancyProgressRange
		{
			get
			{
				return new FloatRange(0.05f, 0.5f);
			}
		}

		// Token: 0x0600C660 RID: 50784 RVA: 0x0039C220 File Offset: 0x0039A420
		public static AcceptanceReport CanEverProduceChild(Pawn first, Pawn second)
		{
			if (first.Dead)
			{
				return "PawnIsDead".Translate(first.Named("PAWN"));
			}
			if (second.Dead)
			{
				return "PawnIsDead".Translate(second.Named("PAWN"));
			}
			if (first.gender == second.gender)
			{
				return "PawnsHaveSameGender".Translate(first.Named("PAWN1"), second.Named("PAWN2")).Resolve();
			}
			Pawn pawn = (first.gender == Gender.Male) ? first : second;
			Pawn pawn2 = (first.gender == Gender.Female) ? first : second;
			bool flag = pawn.GetStatValue(StatDefOf.Fertility, true, -1) <= 0f;
			bool flag2 = pawn2.GetStatValue(StatDefOf.Fertility, true, -1) <= 0f;
			if (flag && flag2)
			{
				return "PawnsAreInfertile".Translate(pawn.Named("PAWN1"), pawn2.Named("PAWN2")).Resolve();
			}
			if (flag != flag2)
			{
				return "PawnIsInfertile".Translate((flag ? pawn : pawn2).Named("PAWN")).Resolve();
			}
			bool flag3 = !pawn.ageTracker.CurLifeStage.reproductive;
			bool flag4 = !pawn2.ageTracker.CurLifeStage.reproductive;
			if (flag3 && flag4)
			{
				return "PawnsAreTooYoung".Translate(pawn.Named("PAWN1"), pawn2.Named("PAWN2")).Resolve();
			}
			if (flag3 != flag4)
			{
				return "PawnIsTooYoung".Translate((flag3 ? pawn : pawn2).Named("PAWN")).Resolve();
			}
			bool flag5 = pawn2.Sterile() && PregnancyUtility.GetPregnancyHediff(pawn2) == null;
			bool flag6 = pawn.Sterile();
			if (flag6 && flag5)
			{
				return "PawnsAreSterile".Translate(pawn.Named("PAWN1"), pawn2.Named("PAWN2")).Resolve();
			}
			if (flag6 != flag5)
			{
				return "PawnIsSterile".Translate((flag6 ? pawn : pawn2).Named("PAWN")).Resolve();
			}
			return true;
		}

		// Token: 0x0600C661 RID: 50785 RVA: 0x0039C47A File Offset: 0x0039A67A
		public static float PregnancyChanceForPawn(Pawn pawn)
		{
			if (!Find.Storyteller.difficulty.ChildrenAllowed)
			{
				return 0f;
			}
			if (pawn.Sterile())
			{
				return 0f;
			}
			return pawn.GetStatValue(StatDefOf.Fertility, true, -1);
		}

		// Token: 0x0600C662 RID: 50786 RVA: 0x0039C4AE File Offset: 0x0039A6AE
		public static float PregnancyChanceImplantEmbryo(Pawn surrogate)
		{
			return Mathf.Clamp01(PregnancyUtility.PregnancyChanceForPawn(surrogate) * 2f);
		}

		// Token: 0x0600C663 RID: 50787 RVA: 0x0039C4C4 File Offset: 0x0039A6C4
		private static float PregnancyChanceForPartnersWithoutPregnancyApproach(Pawn woman, Pawn man)
		{
			float num;
			if (man != null)
			{
				num = PregnancyUtility.PregnancyChanceForPawn(man);
			}
			else
			{
				num = 1f;
			}
			return num * PregnancyUtility.PregnancyChanceForWoman(woman);
		}

		// Token: 0x0600C664 RID: 50788 RVA: 0x0039C4EC File Offset: 0x0039A6EC
		public static float PregnancyChanceForPartners(Pawn woman, Pawn man)
		{
			float num = PregnancyUtility.PregnancyChanceForPartnersWithoutPregnancyApproach(woman, man);
			float pregnancyChanceFactor = woman.relations.GetPregnancyApproachForPartner(man).GetPregnancyChanceFactor();
			return num * pregnancyChanceFactor;
		}

		// Token: 0x0600C665 RID: 50789 RVA: 0x0039C514 File Offset: 0x0039A714
		public static float PregnancyChanceForWoman(Pawn woman)
		{
			return PregnancyUtility.PregnancyChanceForPawn(woman);
		}

		// Token: 0x0600C666 RID: 50790 RVA: 0x0039C51C File Offset: 0x0039A71C
		public static RitualRoleAssignments RitualAssignmentsForBirth(Precept_Ritual ritual, Pawn mother)
		{
			TargetInfo ritualTarget = new TargetInfo(mother.PositionHeld, mother.MapHeld, true);
			RitualRoleAssignments ritualRoleAssignments = new RitualRoleAssignments(ritual, ritualTarget);
			List<Pawn> list = new List<Pawn>
			{
				mother
			};
			Dictionary<string, Pawn> forcedRoles = new Dictionary<string, Pawn>
			{
				{
					"mother",
					mother
				}
			};
			ritualRoleAssignments.Setup(list, new List<Pawn>(), forcedRoles, list, mother);
			ritualRoleAssignments.FillPawns(null, TargetInfo.Invalid);
			return ritualRoleAssignments;
		}

		// Token: 0x0600C667 RID: 50791 RVA: 0x0039C580 File Offset: 0x0039A780
		public static float GetBirthQualityFor(Pawn mother)
		{
			Precept_Ritual ritual = (Precept_Ritual)mother.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			TargetInfo ritualTarget = new TargetInfo(mother.PositionHeld, mother.MapHeld, true);
			return RitualUtility.CalculateQualityAbstract(ritual, ritualTarget, PregnancyUtility.RitualAssignmentsForBirth(ritual, mother), null);
		}

		// Token: 0x0600C668 RID: 50792 RVA: 0x0039C5C6 File Offset: 0x0039A7C6
		public static IEnumerable<Building_Bed> BedsForBirth(Pawn p)
		{
			List<Building> things = p.MapHeld.listerBuildings.allBuildingsColonist;
			int num;
			for (int i = 0; i < things.Count; i = num + 1)
			{
				Building_Bed building_Bed = things[i] as Building_Bed;
				if (building_Bed != null && PregnancyUtility.IsUsableBedFor(p, p, building_Bed))
				{
					yield return building_Bed;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600C669 RID: 50793 RVA: 0x0039C5D6 File Offset: 0x0039A7D6
		public static bool IsUsableBedFor(Pawn mother, Pawn doctor, Building_Bed bed)
		{
			return RestUtility.IsValidBedFor(bed, mother, doctor, true, false, false, mother.GuestStatus);
		}

		// Token: 0x0600C66A RID: 50794 RVA: 0x0039C5E9 File Offset: 0x0039A7E9
		public static float ChanceMomDiesDuringBirth(float quality)
		{
			if (Find.Storyteller.difficulty.babiesAreHealthy)
			{
				return 0f;
			}
			return PregnancyUtility.DeathChanceFromBirthQuality.Evaluate(quality);
		}

		// Token: 0x0600C66B RID: 50795 RVA: 0x0039C610 File Offset: 0x0039A810
		private static string RandomLastName(Pawn geneticMother, Pawn birthingMother, Pawn father)
		{
			PregnancyUtility.tmpLastNames.Clear();
			if (geneticMother != null)
			{
				PregnancyUtility.tmpLastNames.Add(PawnNamingUtility.GetLastName(geneticMother));
			}
			if (father != null)
			{
				PregnancyUtility.tmpLastNames.Add(PawnNamingUtility.GetLastName(father));
			}
			if (birthingMother != null && birthingMother != geneticMother && birthingMother != father)
			{
				PregnancyUtility.tmpLastNames.Add(PawnNamingUtility.GetLastName(birthingMother));
			}
			if (PregnancyUtility.tmpLastNames.Count == 0)
			{
				return null;
			}
			return PregnancyUtility.tmpLastNames.RandomElement<string>();
		}

		// Token: 0x0600C66C RID: 50796 RVA: 0x0039C680 File Offset: 0x0039A880
		public static Thing ApplyBirthOutcome(RitualOutcomePossibility outcome, float quality, Precept_Ritual ritual, List<GeneDef> genes, Pawn geneticMother, Thing birtherThing, Pawn father = null, Pawn doctor = null, LordJob_Ritual lordJobRitual = null, RitualRoleAssignments assignments = null, bool preventLetter = false)
		{
			Pawn birtherPawn = birtherThing as Pawn;
			Building_GrowthVat building_GrowthVat = birtherThing as Building_GrowthVat;
			if (birtherThing.Spawned)
			{
				EffecterDefOf.Birth.Spawn(birtherThing, birtherThing.Map, 1f);
			}
			bool babiesAreHealthy = Find.Storyteller.difficulty.babiesAreHealthy;
			int positivityIndex = outcome.positivityIndex;
			bool flag = birtherPawn != null && Rand.Chance(PregnancyUtility.ChanceMomDiesDuringBirth(quality)) && (birtherPawn.genes == null || !birtherPawn.genes.HasActiveGene(GeneDefOf.Deathless));
			PawnKindDef pawnKindDef = ((geneticMother != null) ? geneticMother.kindDef : null) ?? PawnKindDefOf.Colonist;
			if (pawnKindDef is CreepJoinerFormKindDef)
			{
				pawnKindDef = PawnKindDefOf.Colonist;
			}
			PawnKindDef kind = pawnKindDef;
			Faction faction = birtherThing.Faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			string fixedLastName = PregnancyUtility.RandomLastName(geneticMother, birtherThing as Pawn, father);
			bool flag2 = positivityIndex == -1;
			XenotypeDef baseliner = XenotypeDefOf.Baseliner;
			List<GeneDef> forcedEndogenes = (genes != null) ? genes : PregnancyUtility.GetInheritedGenes(father, geneticMother);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, fixedLastName, null, null, null, true, false, false, flag2, null, forcedEndogenes, baseliner, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false)
			{
				DontGivePreArrivalPathway = true
			});
			if (GeneUtility.SameHeritableXenotype(geneticMother, father) && geneticMother.genes.UniqueXenotype)
			{
				pawn.genes.xenotypeName = geneticMother.genes.xenotypeName;
				pawn.genes.iconDef = geneticMother.genes.iconDef;
			}
			XenotypeDef xenotypeDirect;
			if (PregnancyUtility.TryGetInheritedXenotype(geneticMother, father, out xenotypeDirect))
			{
				Pawn_GeneTracker genes2 = pawn.genes;
				if (genes2 != null)
				{
					genes2.SetXenotypeDirect(xenotypeDirect);
				}
			}
			else if (PregnancyUtility.ShouldByHybrid(geneticMother, father))
			{
				pawn.genes.hybrid = true;
				pawn.genes.xenotypeName = "Hybrid".Translate();
			}
			IntVec3? intVec = null;
			Pawn birtherPawn4 = birtherPawn;
			if (birtherPawn4 != null && birtherPawn4.Spawned)
			{
				int? num;
				Building_Bed building_Bed = birtherPawn.CurrentBed(out num);
				IntVec3 intVec2 = ((building_Bed != null) ? new IntVec3?(building_Bed.GetFootSlotPos(num.Value)) : null) ?? birtherPawn.PositionHeld;
				intVec = new IntVec3?(CellFinder.RandomClosewalkCellNear(intVec2, birtherPawn.Map, 1, delegate(IntVec3 cell)
				{
					if (!(cell != birtherPawn.PositionHeld))
					{
						return false;
					}
					Building building = birtherPawn.Map.edificeGrid[cell];
					if (building == null)
					{
						return true;
					}
					ThingDef def = building.def;
					bool? flag5 = (def != null) ? new bool?(def.IsBed) : null;
					bool flag6 = true;
					return !(flag5.GetValueOrDefault() == flag6 & flag5 != null);
				}));
				PregnancyUtility.SpawnBirthFilth(birtherPawn, intVec2, ThingDefOf.Filth_AmnioticFluid, 1);
				if (flag)
				{
					PregnancyUtility.SpawnBirthFilth(birtherPawn, intVec2, ThingDefOf.Filth_Blood, 2);
				}
			}
			if (building_GrowthVat != null)
			{
				intVec = new IntVec3?(building_GrowthVat.InteractionCell);
				FilthMaker.TryMakeFilth(intVec.Value, building_GrowthVat.Map, ThingDefOf.Filth_AmnioticFluid, PregnancyUtility.BirthFilthCount.RandomInRange, FilthSourceFlags.None, true);
			}
			if (birtherPawn != null)
			{
				birtherPawn.health.AddHediff(HediffDefOf.PostpartumExhaustion, null, null, null);
				birtherPawn.health.AddHediff(HediffDefOf.Lactating, null, null, null);
			}
			if (pawn.RaceProps.IsFlesh)
			{
				if (geneticMother != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, geneticMother);
				}
				if (father != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
				}
				if (birtherPawn != null && birtherPawn != geneticMother)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ParentBirth, birtherPawn);
				}
			}
			bool flag3 = false;
			bool flag4 = positivityIndex == 0;
			if (positivityIndex >= 0)
			{
				if (pawn.playerSettings != null && ((geneticMother != null) ? geneticMother.playerSettings : null) != null)
				{
					pawn.playerSettings.AreaRestrictionInPawnCurrentMap = geneticMother.playerSettings.AreaRestrictionInPawnCurrentMap;
				}
				if (flag4)
				{
					pawn.health.AddHediff(HediffDefOf.InfantIllness, null, null, null);
				}
				if (birtherPawn != null)
				{
					TaleRecorder.RecordTale(TaleDefOf.GaveBirth, new object[]
					{
						birtherPawn,
						pawn
					});
					pawn.mindState.SetAutofeeder(birtherPawn, AutofeedMode.Urgent);
				}
				if (doctor != null && doctor.Spawned && doctor != null)
				{
					Pawn_CarryTracker carryTracker = doctor.carryTracker;
					bool? keepCarryingThingOverride = (carryTracker != null) ? new bool?(carryTracker.TryStartCarry(pawn)) : null;
					flag2 = true;
					if ((keepCarryingThingOverride.GetValueOrDefault() == flag2 & keepCarryingThingOverride != null) && birtherPawn != null && (building_GrowthVat != null || doctor.CanReach(birtherPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
					{
						Job job = JobMaker.MakeJob(JobDefOf.CarryToMomAfterBirth, pawn, birtherPawn);
						job.count = 1;
						Pawn_JobTracker jobs = doctor.jobs;
						Job newJob = job;
						JobCondition lastJobEndCondition = JobCondition.Succeeded;
						ThinkNode jobGiver = null;
						bool resumeCurJobAfterwards = false;
						bool cancelBusyStances = true;
						ThinkTreeDef thinkTree = null;
						keepCarryingThingOverride = new bool?(true);
						jobs.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, null, false, false, keepCarryingThingOverride, false, true, false);
						goto IL_568;
					}
				}
				if (!PawnUtility.TrySpawnHatchedOrBornPawn(pawn, birtherThing, intVec))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				IL_568:
				if (!flag4)
				{
					if (father != null)
					{
						Pawn_NeedsTracker needs = father.needs;
						if (needs != null)
						{
							Need_Mood mood = needs.mood;
							if (mood != null)
							{
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts != null)
								{
									MemoryThoughtHandler memories = thoughts.memories;
									if (memories != null)
									{
										memories.TryGainMemory(ThoughtDefOf.BabyBorn, pawn, null);
									}
								}
							}
						}
					}
					if (geneticMother != null)
					{
						Pawn_NeedsTracker needs2 = geneticMother.needs;
						if (needs2 != null)
						{
							Need_Mood mood2 = needs2.mood;
							if (mood2 != null)
							{
								ThoughtHandler thoughts2 = mood2.thoughts;
								if (thoughts2 != null)
								{
									MemoryThoughtHandler memories2 = thoughts2.memories;
									if (memories2 != null)
									{
										memories2.TryGainMemory(ThoughtDefOf.BabyBorn, pawn, null);
									}
								}
							}
						}
					}
					Pawn birtherPawn2 = birtherPawn;
					if (birtherPawn2 != null)
					{
						Pawn_NeedsTracker needs3 = birtherPawn2.needs;
						if (needs3 != null)
						{
							Need_Mood mood3 = needs3.mood;
							if (mood3 != null)
							{
								ThoughtHandler thoughts3 = mood3.thoughts;
								if (thoughts3 != null)
								{
									MemoryThoughtHandler memories3 = thoughts3.memories;
									if (memories3 != null)
									{
										memories3.TryGainMemory(ThoughtDefOf.BabyBorn, pawn, null);
									}
								}
							}
						}
					}
				}
			}
			else
			{
				Hediff culpritHediff = pawn.health.AddHediff(HediffDefOf.Stillborn, null, null, null);
				flag3 = true;
				Pawn birtherPawn3 = birtherPawn;
				if (birtherPawn3 != null)
				{
					Ideo ideo = birtherPawn3.Ideo;
					if (ideo != null)
					{
						ideo.Notify_MemberDied(pawn);
					}
				}
				pawn.babyNamingDeadline = Find.TickManager.TicksGame + 1;
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(pawn, pawn.RaceProps.DeathActionWorker.DeathRules, null, culpritHediff, null));
				if (birtherThing.Spawned)
				{
					GenSpawn.Spawn(pawn.Corpse, intVec ?? birtherThing.PositionHeld, birtherThing.MapHeld, WipeMode.Vanish);
				}
			}
			if (flag3)
			{
				if (geneticMother != null)
				{
					Pawn_NeedsTracker needs4 = geneticMother.needs;
					if (needs4 != null)
					{
						Need_Mood mood4 = needs4.mood;
						if (mood4 != null)
						{
							ThoughtHandler thoughts4 = mood4.thoughts;
							if (thoughts4 != null)
							{
								MemoryThoughtHandler memories4 = thoughts4.memories;
								if (memories4 != null)
								{
									memories4.TryGainMemory(ThoughtDefOf.Stillbirth, null, null);
								}
							}
						}
					}
				}
				if (father != null)
				{
					Pawn_NeedsTracker needs5 = father.needs;
					if (needs5 != null)
					{
						Need_Mood mood5 = needs5.mood;
						if (mood5 != null)
						{
							ThoughtHandler thoughts5 = mood5.thoughts;
							if (thoughts5 != null)
							{
								MemoryThoughtHandler memories5 = thoughts5.memories;
								if (memories5 != null)
								{
									memories5.TryGainMemory(ThoughtDefOf.Stillbirth, null, null);
								}
							}
						}
					}
				}
			}
			Map mapHeld = birtherThing.MapHeld;
			IntVec3 positionHeld = birtherThing.PositionHeld;
			if (flag)
			{
				birtherPawn.Kill(null, null);
			}
			Thing thing = (positivityIndex >= 0) ? pawn : pawn.Corpse;
			Find.QuestManager.Notify_PawnBorn(thing, birtherThing, geneticMother, father);
			if (ritual != null && !preventLetter)
			{
				TaggedString taggedString;
				if (birtherPawn != null)
				{
					taggedString = outcome.description.Formatted(birtherPawn.Named("MOTHER"));
				}
				else if (flag3)
				{
					if (geneticMother != null && father != null)
					{
						taggedString = "LetterVatStillborn".Translate(geneticMother, father);
					}
					else
					{
						taggedString = "LetterVatSillbornNoParents".Translate();
					}
				}
				else if (flag4)
				{
					if (geneticMother != null && father != null)
					{
						taggedString = "LetterVatInfantIllness".Translate(geneticMother, father);
					}
					else
					{
						taggedString = "LetterVatInfantIllnessNoParents".Translate();
					}
				}
				else if (geneticMother != null && father != null)
				{
					taggedString = "LetterVatHealthyBaby".Translate(geneticMother, father);
				}
				else
				{
					taggedString = "LetterVatHealthyBabyNoParents".Translate();
				}
				if (!flag3 && birtherPawn != null && geneticMother != null && birtherPawn != geneticMother && father != null)
				{
					taggedString += "\n\n" + "LetterPartSurrogacy".Translate(geneticMother, father);
				}
				if (birtherPawn != null && !babiesAreHealthy)
				{
					RitualOutcomeEffectWorker_ChildBirth ritualOutcomeEffectWorker_ChildBirth = (RitualOutcomeEffectWorker_ChildBirth)RitualOutcomeEffectDefOf.ChildBirth.GetInstance();
					if (lordJobRitual != null)
					{
						taggedString += "\n\n" + ritualOutcomeEffectWorker_ChildBirth.OutcomeQualityBreakdownDesc(quality, 1f, lordJobRitual);
					}
					else
					{
						taggedString += "\n\n" + RitualUtility.QualityBreakdownAbstract(ritual, new TargetInfo(positionHeld, mapHeld, true), assignments, null);
					}
					if (!babiesAreHealthy)
					{
						taggedString += "\n\n" + "BirthRitualHealthyBabyChance".Translate(ritualOutcomeEffectWorker_ChildBirth.GetOutcomeChanceAtQuality(lordJobRitual, ritualOutcomeEffectWorker_ChildBirth.def.BestOutcome, quality));
					}
					if (flag)
					{
						taggedString += "\n\n" + "LetterPartColonistDiedAfterChildbirth".Translate(birtherPawn);
					}
				}
				if (pawn.genes.HasActiveGene(GeneDefOf.Inbred))
				{
					taggedString += "\n\n" + "InbredBabyBorn".Translate();
				}
				taggedString += "\n\n" + "LetterPartTempBabyName".Translate(pawn) + " ";
				if (flag3)
				{
					taggedString += "LetterPartStillbirthNameDeadline".Translate();
				}
				else
				{
					taggedString += "LetterPartLiveBirthNameDeadline".Translate(60000.ToStringTicksToPeriod(true, false, true, true, false));
				}
				ChoiceLetter_BabyBirth choiceLetter_BabyBirth = (ChoiceLetter_BabyBirth)LetterMaker.MakeLetter((birtherPawn != null) ? "OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), ritual.Label.Named("RITUALLABEL")) : "LetterVatBirth".Translate(outcome.label), taggedString, LetterDefOf.BabyBirth, pawn, null, null, null);
				choiceLetter_BabyBirth.Start();
				Find.LetterStack.ReceiveLetter(choiceLetter_BabyBirth, null, 0, true);
			}
			return thing;
		}

		// Token: 0x0600C66D RID: 50797 RVA: 0x0039D168 File Offset: 0x0039B368
		public static Gizmo BirthQualityGizmo(Pawn pawn)
		{
			return new Command_Action
			{
				defaultLabel = "DEV: Output Birth Quality Now",
				action = delegate()
				{
					Messages.Message("Birth ritual quality for " + pawn.LabelShort + " is " + PregnancyUtility.GetBirthQualityFor(pawn).ToStringPercent(), pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			};
		}

		// Token: 0x0600C66E RID: 50798 RVA: 0x0039D1A4 File Offset: 0x0039B3A4
		private static void SpawnBirthFilth(Pawn mother, IntVec3 center, ThingDef filth, int radius)
		{
			int randomInRange = PregnancyUtility.BirthFilthCount.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				FilthMaker.TryMakeFilth(CellFinder.RandomClosewalkCellNear(center, mother.Map, radius, null), mother.Map, filth, mother.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
		}

		// Token: 0x0600C66F RID: 50799 RVA: 0x0039D1F0 File Offset: 0x0039B3F0
		public static GeneSet GetInheritedGeneSet(Pawn father, Pawn mother, out bool success)
		{
			GeneSet geneSet = new GeneSet();
			foreach (GeneDef gene in PregnancyUtility.GetInheritedGenes(father, mother, out success))
			{
				geneSet.AddGene(gene);
			}
			if (GeneUtility.SameHeritableXenotype(father, mother))
			{
				geneSet.SetNameDirect(father.genes.xenotypeName);
			}
			return geneSet;
		}

		// Token: 0x0600C670 RID: 50800 RVA: 0x0039D268 File Offset: 0x0039B468
		public static GeneSet GetInheritedGeneSet(Pawn father, Pawn mother)
		{
			bool flag;
			return PregnancyUtility.GetInheritedGeneSet(father, mother, out flag);
		}

		// Token: 0x0600C671 RID: 50801 RVA: 0x0039D280 File Offset: 0x0039B480
		public static List<GeneDef> GetInheritedGenes(Pawn father, Pawn mother, out bool success)
		{
			PregnancyUtility.tmpGenes.Clear();
			PregnancyUtility.tmpGenesShuffled.Clear();
			if (((father != null) ? father.genes : null) != null)
			{
				foreach (Gene gene in father.genes.Endogenes)
				{
					if (gene.def.endogeneCategory != EndogeneCategory.Melanin && gene.def.biostatArc <= 0)
					{
						PregnancyUtility.tmpGeneChances.SetOrAdd(gene.def, 0.5f);
						if (!PregnancyUtility.tmpGenesShuffled.Contains(gene.def))
						{
							PregnancyUtility.tmpGenesShuffled.Add(gene.def);
						}
					}
				}
			}
			if (((mother != null) ? mother.genes : null) != null)
			{
				foreach (Gene gene2 in mother.genes.Endogenes)
				{
					if (gene2.def.endogeneCategory != EndogeneCategory.Melanin && gene2.def.biostatArc <= 0)
					{
						if (!PregnancyUtility.tmpGenesShuffled.Contains(gene2.def))
						{
							PregnancyUtility.tmpGenesShuffled.Add(gene2.def);
						}
						if (PregnancyUtility.tmpGeneChances.ContainsKey(gene2.def))
						{
							PregnancyUtility.tmpGeneChances[gene2.def] = 1f;
						}
						else
						{
							PregnancyUtility.tmpGeneChances.Add(gene2.def, 0.5f);
						}
					}
				}
			}
			int num = 0;
			do
			{
				PregnancyUtility.tmpGenes.Clear();
				PregnancyUtility.tmpGenesShuffled.Shuffle<GeneDef>();
				foreach (GeneDef geneDef in PregnancyUtility.tmpGenesShuffled)
				{
					if (!PregnancyUtility.tmpGenes.Contains(geneDef) && Rand.Chance(PregnancyUtility.tmpGeneChances[geneDef]))
					{
						PregnancyUtility.tmpGenes.Add(geneDef);
					}
				}
				PregnancyUtility.tmpGenes.RemoveAll((GeneDef x) => x.prerequisite != null && !PregnancyUtility.tmpGenes.Contains(x.prerequisite));
				int val = PregnancyUtility.tmpGenes.NonOverriddenGenes(false).Sum((GeneDef x) => x.biostatMet);
				if (GeneTuning.BiostatRange.Includes(val))
				{
					break;
				}
				num++;
			}
			while (num < 50);
			success = (num < 50);
			GeneDef item;
			if (PawnSkinColors.SkinColorsFromParents(father, mother).TryRandomElement(out item))
			{
				PregnancyUtility.tmpGenes.Add(item);
			}
			if (!PregnancyUtility.tmpGenes.Any((GeneDef x) => x.endogeneCategory == EndogeneCategory.HairColor))
			{
				GeneDef geneDef2;
				if (father == null)
				{
					geneDef2 = null;
				}
				else
				{
					Pawn_GeneTracker genes = father.genes;
					geneDef2 = ((genes != null) ? genes.GetFirstEndogeneByCategory(EndogeneCategory.HairColor) : null);
				}
				GeneDef geneDef3 = geneDef2;
				GeneDef geneDef4;
				if (mother == null)
				{
					geneDef4 = null;
				}
				else
				{
					Pawn_GeneTracker genes2 = mother.genes;
					geneDef4 = ((genes2 != null) ? genes2.GetFirstEndogeneByCategory(EndogeneCategory.HairColor) : null);
				}
				GeneDef geneDef5 = geneDef4;
				GeneDef item2;
				if (geneDef3 != null && geneDef5 == null)
				{
					PregnancyUtility.tmpGenes.Add(geneDef3);
				}
				else if (geneDef5 != null && geneDef3 == null)
				{
					PregnancyUtility.tmpGenes.Add(geneDef5);
				}
				else if (geneDef3 != null && geneDef5 != null)
				{
					PregnancyUtility.tmpGenes.Add(Rand.Bool ? geneDef5 : geneDef3);
				}
				else if ((from x in DefDatabase<GeneDef>.AllDefs
				where x.endogeneCategory == EndogeneCategory.HairColor
				select x).TryRandomElementByWeight((GeneDef x) => x.selectionWeight, out item2))
				{
					PregnancyUtility.tmpGenes.Add(item2);
				}
			}
			PawnRelationDef pawnRelationDef;
			if (!PregnancyUtility.tmpGenes.Contains(GeneDefOf.Inbred) && Rand.Value < PregnancyUtility.InbredChanceFromParents(mother, father, out pawnRelationDef))
			{
				PregnancyUtility.tmpGenes.Add(GeneDefOf.Inbred);
			}
			PregnancyUtility.tmpGeneChances.Clear();
			PregnancyUtility.tmpGenesShuffled.Clear();
			return PregnancyUtility.tmpGenes;
		}

		// Token: 0x0600C672 RID: 50802 RVA: 0x0039D698 File Offset: 0x0039B898
		public static List<GeneDef> GetInheritedGenes(Pawn father, Pawn mother)
		{
			bool flag;
			return PregnancyUtility.GetInheritedGenes(father, mother, out flag);
		}

		// Token: 0x0600C673 RID: 50803 RVA: 0x0039D6B0 File Offset: 0x0039B8B0
		public static float InbredChanceFromParents(Pawn mother, Pawn father, out PawnRelationDef relation)
		{
			relation = null;
			if (mother != null && father != null)
			{
				float num = 0f;
				foreach (PawnRelationDef pawnRelationDef in father.GetRelations(mother))
				{
					if (pawnRelationDef.inbredChanceOnChild > num)
					{
						num = pawnRelationDef.inbredChanceOnChild;
						relation = pawnRelationDef;
					}
					num = Mathf.Max(num, pawnRelationDef.inbredChanceOnChild);
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x0600C674 RID: 50804 RVA: 0x0039D730 File Offset: 0x0039B930
		private static bool TryGetInheritedXenotype(Pawn mother, Pawn father, out XenotypeDef xenotype)
		{
			bool flag = ((mother != null) ? mother.genes : null) != null;
			bool flag2 = ((father != null) ? father.genes : null) != null;
			if (flag && flag2 && mother.genes.Xenotype.inheritable && father.genes.Xenotype.inheritable && mother.genes.Xenotype == father.genes.Xenotype)
			{
				xenotype = mother.genes.Xenotype;
				return true;
			}
			if (flag && !flag2 && mother.genes.Xenotype.inheritable)
			{
				xenotype = mother.genes.Xenotype;
				return true;
			}
			if (flag2 && !flag && father.genes.Xenotype.inheritable)
			{
				xenotype = father.genes.Xenotype;
				return true;
			}
			xenotype = null;
			return false;
		}

		// Token: 0x0600C675 RID: 50805 RVA: 0x0039D800 File Offset: 0x0039BA00
		private static bool ShouldByHybrid(Pawn mother, Pawn father)
		{
			bool flag = ((mother != null) ? mother.genes : null) != null;
			bool flag2 = ((father != null) ? father.genes : null) != null;
			if (flag && flag2)
			{
				if (mother.genes.hybrid && father.genes.hybrid)
				{
					return true;
				}
				if (mother.genes.Xenotype.inheritable && father.genes.Xenotype.inheritable)
				{
					return true;
				}
				bool flag3 = flag && (mother.genes.Xenotype.inheritable || mother.genes.hybrid);
				bool flag4 = flag2 && (father.genes.Xenotype.inheritable || father.genes.hybrid);
				if (flag3 || flag4)
				{
					return true;
				}
			}
			return (flag && !flag2 && mother.genes.hybrid) || (flag2 && !flag && father.genes.hybrid);
		}

		// Token: 0x0600C676 RID: 50806 RVA: 0x0039D8F4 File Offset: 0x0039BAF4
		public static bool TryTerminatePregnancy(Pawn pawn)
		{
			Hediff pregnancyHediff = PregnancyUtility.GetPregnancyHediff(pawn);
			if (pregnancyHediff == null)
			{
				return false;
			}
			int curStageIndex = pregnancyHediff.CurStageIndex;
			PregnancyAttitude? attitude = ((Hediff_Pregnant)pregnancyHediff).Attitude;
			pawn.health.RemoveHediff(pregnancyHediff);
			Pawn_NeedsTracker needs = pawn.needs;
			bool flag;
			if (needs == null)
			{
				flag = (null != null);
			}
			else
			{
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					flag = (null != null);
				}
				else
				{
					ThoughtHandler thoughts = mood.thoughts;
					flag = (((thoughts != null) ? thoughts.memories : null) != null);
				}
			}
			if (flag && attitude != null)
			{
				Thought_Memory thought_Memory = null;
				PregnancyAttitude value = attitude.Value;
				if (value != PregnancyAttitude.Positive)
				{
					if (value == PregnancyAttitude.Negative)
					{
						thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.PregnancyEnded);
					}
				}
				else
				{
					thought_Memory = ThoughtMaker.MakeThought(ThoughtDefOf.PregnancyTerminated, curStageIndex);
				}
				if (thought_Memory != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
				}
			}
			return true;
		}

		// Token: 0x0600C677 RID: 50807 RVA: 0x0039D9B8 File Offset: 0x0039BBB8
		public static Hediff GetPregnancyHediff(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnantHuman, true);
			}
			if (pawn.RaceProps.Animal)
			{
				return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
			}
			return null;
		}

		// Token: 0x0600C678 RID: 50808 RVA: 0x0039DA10 File Offset: 0x0039BC10
		public static void ForceEndPregnancy(Pawn pawn, bool preventLetter = false)
		{
			Hediff pregnancyHediff = PregnancyUtility.GetPregnancyHediff(pawn);
			if (pregnancyHediff != null)
			{
				pawn.health.RemoveHediff(pregnancyHediff);
			}
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Hediff hediff;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.PregnancyLabor, out hediff))
			{
				pawn.health.RemoveHediff(hediff);
			}
			Hediff hediff2;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.PregnancyLaborPushing, out hediff2))
			{
				Hediff_LaborPushing hediff_LaborPushing = hediff2 as Hediff_LaborPushing;
				if (hediff_LaborPushing == null)
				{
					return;
				}
				hediff_LaborPushing.ForceBirth(-1, preventLetter);
			}
		}

		// Token: 0x0600C679 RID: 50809 RVA: 0x0039DA8C File Offset: 0x0039BC8C
		public static string GetLabel(this PregnancyApproach mode)
		{
			switch (mode)
			{
			case PregnancyApproach.Normal:
				return "PregnancyApproach_Normal".Translate();
			case PregnancyApproach.AvoidPregnancy:
				return "PregnancyApproach_AvoidPregnancy".Translate();
			case PregnancyApproach.TryForBaby:
				return "PregnancyApproach_TryForBaby".Translate();
			default:
				Log.Error("Undefined rhythm method.");
				return string.Empty;
			}
		}

		// Token: 0x0600C67A RID: 50810 RVA: 0x0039DAEC File Offset: 0x0039BCEC
		public static float GetPregnancyChanceFactor(this PregnancyApproach mode)
		{
			if (mode == PregnancyApproach.AvoidPregnancy)
			{
				return PregnancyUtility.PregnancyChanceFactor_PregnancyApproach_AvoidPregnancy;
			}
			if (mode != PregnancyApproach.TryForBaby)
			{
				return PregnancyUtility.PregnancyChanceFactor_PregnancyApproach_Normal;
			}
			return PregnancyUtility.PregnancyChanceFactor_PregnancyApproach_TryForBaby;
		}

		// Token: 0x0600C67B RID: 50811 RVA: 0x0039DB09 File Offset: 0x0039BD09
		public static string GetDescription(this PregnancyApproach mode)
		{
			return string.Format("{0} ({1} x{2})", mode.GetLabel().CapitalizeFirst(), "PregnancyChance".Translate(), mode.GetPregnancyChanceFactor().ToStringPercent());
		}

		// Token: 0x0600C67C RID: 50812 RVA: 0x0039DB3A File Offset: 0x0039BD3A
		public static Texture2D GetIcon(this PregnancyApproach mode)
		{
			if (mode == PregnancyApproach.AvoidPregnancy)
			{
				return PregnancyUtility.PregnancyApproachIcon_AvoidPregnancy.Texture;
			}
			if (mode != PregnancyApproach.TryForBaby)
			{
				return PregnancyUtility.PregnancyApproachIcon_Normal.Texture;
			}
			return PregnancyUtility.PregnancyApproachIcon_TryForBaby.Texture;
		}

		// Token: 0x04008729 RID: 34601
		public static readonly SimpleCurve DeathChanceFromBirthQuality = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(0.2f, 0.05f),
				true
			},
			{
				new CurvePoint(0.9f, 0f),
				true
			}
		};

		// Token: 0x0400872A RID: 34602
		public static readonly Dictionary<PawnRelationDef, float> BeingFatherWeightPerRelation = new Dictionary<PawnRelationDef, float>
		{
			{
				PawnRelationDefOf.Spouse,
				1f
			},
			{
				PawnRelationDefOf.Lover,
				0.8f
			},
			{
				PawnRelationDefOf.ExSpouse,
				0.5f
			},
			{
				PawnRelationDefOf.ExLover,
				0.4f
			}
		};

		// Token: 0x0400872B RID: 34603
		public const int MaxGeneAssignmentTries = 50;

		// Token: 0x0400872C RID: 34604
		public const float MinFertileMaleAge = 14f;

		// Token: 0x0400872D RID: 34605
		public const float ChanceOfNoFatherWithCandidates = 0.2f;

		// Token: 0x0400872E RID: 34606
		public const float ChanceToInheritOneParent = 0.5f;

		// Token: 0x0400872F RID: 34607
		public const float ChanceToInheritTwoParents = 1f;

		// Token: 0x04008730 RID: 34608
		public const float IvfPregnancyFactor = 2f;

		// Token: 0x04008731 RID: 34609
		private static readonly IntRange BirthFilthCount = new IntRange(4, 7);

		// Token: 0x04008732 RID: 34610
		private static float PregnancyChanceFactor_PregnancyApproach_Normal = 1f;

		// Token: 0x04008733 RID: 34611
		private static float PregnancyChanceFactor_PregnancyApproach_AvoidPregnancy = 0.25f;

		// Token: 0x04008734 RID: 34612
		private static float PregnancyChanceFactor_PregnancyApproach_TryForBaby = 4f;

		// Token: 0x04008735 RID: 34613
		private static CachedTexture PregnancyApproachIcon_Normal = new CachedTexture("UI/Icons/PregnancyApproaches/Normal");

		// Token: 0x04008736 RID: 34614
		private static CachedTexture PregnancyApproachIcon_AvoidPregnancy = new CachedTexture("UI/Icons/PregnancyApproaches/AvoidPregnancy");

		// Token: 0x04008737 RID: 34615
		private static CachedTexture PregnancyApproachIcon_TryForBaby = new CachedTexture("UI/Icons/PregnancyApproaches/TryForBaby");

		// Token: 0x04008738 RID: 34616
		private static List<string> tmpLastNames = new List<string>(3);

		// Token: 0x04008739 RID: 34617
		private static Dictionary<GeneDef, float> tmpGeneChances = new Dictionary<GeneDef, float>();

		// Token: 0x0400873A RID: 34618
		private static List<GeneDef> tmpGenesShuffled = new List<GeneDef>();

		// Token: 0x0400873B RID: 34619
		private static List<GeneDef> tmpGenes = new List<GeneDef>();
	}
}
