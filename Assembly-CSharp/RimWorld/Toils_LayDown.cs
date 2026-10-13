using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013B4 RID: 5044
	public static class Toils_LayDown
	{
		// Token: 0x06007B80 RID: 31616 RVA: 0x00253324 File Offset: 0x00251524
		public static Toil LayDown(TargetIndex bedOrRestSpotIndex, bool hasBed, bool lookForOtherJobs, bool canSleep = true, bool gainRestAndHealth = true, PawnPosture noBedLayingPosture = PawnPosture.LayingOnGroundNormal, bool deathrest = false)
		{
			Toil layDown = ToilMaker.MakeToil("LayDown");
			layDown.initAction = delegate()
			{
				Pawn actor = layDown.actor;
				Pawn_PathFollower pather = actor.pather;
				if (pather != null)
				{
					pather.StopDead();
				}
				if (hasBed)
				{
					Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedOrRestSpotIndex).Thing;
					if (!building_Bed.OccupiedRect().Contains(actor.Position))
					{
						string str = "Can't start LayDown toil because pawn is not in the bed. pawn=";
						Pawn pawn = actor;
						Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
						actor.jobs.EndCurrentJob(JobCondition.Errored, true, true);
						return;
					}
					actor.jobs.posture = PawnPosture.LayingInBed;
					actor.mindState.lastBedDefSleptIn = building_Bed.def;
					PortraitsCache.SetDirty(actor);
				}
				else
				{
					actor.jobs.posture = noBedLayingPosture;
					actor.mindState.lastBedDefSleptIn = null;
				}
				if (actor.mindState.applyBedThoughtsTick == 0)
				{
					actor.mindState.applyBedThoughtsTick = Find.TickManager.TicksGame + Rand.Range(2500, 10000);
					actor.mindState.applyBedThoughtsOnLeave = false;
				}
				if (actor.ownership != null && actor.CurrentBed() != actor.ownership.OwnedBed && !deathrest)
				{
					ThoughtUtility.RemovePositiveBedroomThoughts(actor);
				}
			};
			layDown.tickAction = delegate()
			{
				Pawn actor = layDown.actor;
				Job curJob = actor.CurJob;
				JobDriver curDriver = actor.jobs.curDriver;
				Building_Bed bed = curJob.GetTarget(bedOrRestSpotIndex).Thing as Building_Bed;
				if (!curDriver.asleep)
				{
					if (canSleep && (RestUtility.CanFallAsleep(actor) || curJob.forceSleep) && (actor.ageTracker.CurLifeStage.canVoluntarilySleep || curJob.startInvoluntarySleep))
					{
						curDriver.asleep = true;
						curJob.startInvoluntarySleep = false;
					}
				}
				else if (!canSleep || (RestUtility.ShouldWakeUp(actor) && !curJob.forceSleep))
				{
					curDriver.asleep = false;
				}
				Toils_LayDown.ApplyBedRelatedEffects(actor, bed, curDriver.asleep, gainRestAndHealth, 1);
				if (lookForOtherJobs && actor.IsHashIntervalTick(211))
				{
					actor.jobs.CheckForJobOverride(0f, true);
				}
			};
			layDown.defaultCompleteMode = ToilCompleteMode.Never;
			if (hasBed)
			{
				layDown.FailOnBedNoLongerUsable(bedOrRestSpotIndex);
			}
			layDown.AddFinishAction(delegate
			{
				Toils_LayDown.FinalizeLayingJob(layDown.actor, hasBed ? ((Building_Bed)layDown.actor.CurJob.GetTarget(bedOrRestSpotIndex).Thing) : null, deathrest);
			});
			return layDown;
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x002533EC File Offset: 0x002515EC
		private static void ApplyBedRelatedEffects(Pawn p, Building_Bed bed, bool asleep, bool gainRest, int delta)
		{
			p.GainComfortFromCellIfPossible(delta, false);
			if (asleep && gainRest && p.needs.rest != null)
			{
				float restEffectiveness;
				if (bed != null && bed.def.statBases.StatListContains(StatDefOf.BedRestEffectiveness))
				{
					restEffectiveness = bed.GetStatValue(StatDefOf.BedRestEffectiveness, true, 15);
				}
				else
				{
					restEffectiveness = StatDefOf.BedRestEffectiveness.valueIfMissing;
				}
				p.needs.rest.TickResting(restEffectiveness);
			}
			if (p.IsHashIntervalTick(100, delta))
			{
				Thing spawnedParentOrMe = p.SpawnedParentOrMe;
				if (spawnedParentOrMe != null && !spawnedParentOrMe.Position.Fogged(spawnedParentOrMe.Map))
				{
					if (asleep && !p.RaceProps.IsMechanoid)
					{
						FleckDef fleckDef = FleckDefOf.SleepZ;
						float velocitySpeed = 0.42f;
						if (p.ageTracker.CurLifeStage.developmentalStage == DevelopmentalStage.Baby || p.ageTracker.CurLifeStage.developmentalStage == DevelopmentalStage.Newborn)
						{
							fleckDef = FleckDefOf.SleepZ_Tiny;
							velocitySpeed = 0.25f;
						}
						else if (p.ageTracker.CurLifeStage.developmentalStage == DevelopmentalStage.Child)
						{
							fleckDef = FleckDefOf.SleepZ_Small;
							velocitySpeed = 0.33f;
						}
						FleckMaker.ThrowMetaIcon(spawnedParentOrMe.Position, spawnedParentOrMe.Map, fleckDef, velocitySpeed);
					}
					if (gainRest && p.health.hediffSet.GetNaturallyHealingInjuredParts().Any<BodyPartRecord>())
					{
						FleckMaker.ThrowMetaIcon(spawnedParentOrMe.Position, spawnedParentOrMe.Map, FleckDefOf.HealingCross, 0.42f);
					}
				}
			}
			if (p.mindState.applyBedThoughtsTick != 0 && p.mindState.applyBedThoughtsTick <= Find.TickManager.TicksGame)
			{
				Toils_LayDown.ApplyBedThoughts(p, bed);
				p.mindState.applyBedThoughtsTick += 60000;
				p.mindState.applyBedThoughtsOnLeave = true;
			}
			if (ModsConfig.IdeologyActive && bed != null && p.IsHashIntervalTick(2500, delta) && !p.Awake() && (p.IsFreeColonist || p.IsPrisonerOfColony) && !p.IsSlaveOfColony)
			{
				Room room = bed.GetRoom(RegionType.Set_All);
				if (!room.PsychologicallyOutdoors)
				{
					bool flag = false;
					foreach (Building_Bed building_Bed in room.ContainedBeds)
					{
						foreach (Pawn pawn in building_Bed.CurOccupants)
						{
							if (pawn != p && !pawn.Awake() && pawn.IsSlave && !LovePartnerRelationUtility.LovePartnerRelationExists(p, pawn))
							{
								p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInRoomWithSlave, null, null);
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06007B82 RID: 31618 RVA: 0x002536C4 File Offset: 0x002518C4
		private static void FinalizeLayingJob(Pawn pawn, Building_Bed bed, bool deathrest)
		{
			if (pawn.mindState.applyBedThoughtsOnLeave)
			{
				Toils_LayDown.ApplyBedThoughts(pawn, bed);
			}
			if (deathrest)
			{
				Toils_LayDown.UpdateDeathrestThoughtIndex(pawn);
			}
		}

		// Token: 0x06007B83 RID: 31619 RVA: 0x002536E4 File Offset: 0x002518E4
		private static void ApplyBedThoughts(Pawn actor, Building_Bed bed)
		{
			if (actor.needs.mood == null)
			{
				return;
			}
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBedroom);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBarracks);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOutside);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOnGround);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInCold);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInHeat);
			float ambientTemperature = actor.AmbientTemperature;
			float num = actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null);
			float num2 = actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
			if (ModsConfig.BiotechActive && actor.genes != null)
			{
				foreach (Gene gene in actor.genes.GenesListForReading)
				{
					if (gene.Active)
					{
						num += gene.def.statOffsets.GetStatOffsetFromList(StatDefOf.ComfyTemperatureMin);
						num2 += gene.def.statOffsets.GetStatOffsetFromList(StatDefOf.ComfyTemperatureMax);
					}
				}
			}
			if (ambientTemperature < num)
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInCold, null, null);
			}
			if (ambientTemperature > num2)
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInHeat, null, null);
			}
			if (!actor.IsWildMan())
			{
				if (actor.GetRoom(RegionType.Set_All).PsychologicallyOutdoors)
				{
					actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOutside, null, null);
				}
				if (bed == null || bed.CostListAdjusted().Count == 0)
				{
					actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOnGround, null, null);
				}
			}
			if (bed != null && bed == actor.ownership.OwnedBed && !bed.ForPrisoners && !actor.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				Room room = bed.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					ThoughtDef thoughtDef = null;
					if (room.Role == RoomRoleDefOf.Bedroom)
					{
						thoughtDef = ThoughtDefOf.SleptInBedroom;
					}
					else if (room.Role == RoomRoleDefOf.Barracks)
					{
						thoughtDef = ThoughtDefOf.SleptInBarracks;
					}
					if (thoughtDef != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(bed.GetRoom(RegionType.Set_All).GetStat(RoomStatDefOf.Impressiveness));
						if (thoughtDef.stages[scoreStageIndex] != null)
						{
							actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex), null);
						}
					}
				}
			}
			actor.Notify_AddBedThoughts();
		}

		// Token: 0x06007B84 RID: 31620 RVA: 0x00253A00 File Offset: 0x00251C00
		private static void UpdateDeathrestThoughtIndex(Pawn actor)
		{
			if (actor.needs.mood == null || !ModsConfig.BiotechActive)
			{
				return;
			}
			Pawn_GeneTracker genes = actor.genes;
			Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
			if (gene_Deathrest == null)
			{
				return;
			}
			Room room = actor.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				gene_Deathrest.chamberThoughtIndex = -1;
				return;
			}
			if (!actor.IsWildMan() && room.PsychologicallyOutdoors)
			{
				gene_Deathrest.chamberThoughtIndex = 0;
				return;
			}
			if (actor.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				gene_Deathrest.chamberThoughtIndex = -1;
				return;
			}
			gene_Deathrest.chamberThoughtIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness)) + 1;
		}

		// Token: 0x06007B85 RID: 31621 RVA: 0x00253AA4 File Offset: 0x00251CA4
		public static Toil SelfShutdown()
		{
			Toil layDown = ToilMaker.MakeToil("SelfShutdown");
			layDown.initAction = delegate()
			{
				Pawn actor = layDown.actor;
				Pawn_PathFollower pather = actor.pather;
				if (pather != null)
				{
					pather.StopDead();
				}
				JobDriver curDriver = actor.jobs.curDriver;
				actor.jobs.posture = PawnPosture.LayingOnGroundNormal;
				actor.mindState.lastBedDefSleptIn = null;
				curDriver.asleep = true;
			};
			layDown.defaultCompleteMode = ToilCompleteMode.Never;
			layDown.AddFinishAction(delegate
			{
				layDown.actor.jobs.curDriver.asleep = false;
			});
			return layDown;
		}

		// Token: 0x06007B86 RID: 31622 RVA: 0x00253B08 File Offset: 0x00251D08
		public static Toil ActivityDormant()
		{
			Toil toil = ToilMaker.MakeToil("ActivityDormant");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn_PathFollower pather = actor.pather;
				if (pather != null)
				{
					pather.StopDead();
				}
				JobDriver curDriver = actor.jobs.curDriver;
				actor.jobs.posture = PawnPosture.Standing;
				CompCanBeDormant comp = actor.GetComp<CompCanBeDormant>();
				if (comp != null)
				{
					comp.ToSleep();
				}
				curDriver.asleep = true;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.AddFinishAction(delegate
			{
				toil.actor.jobs.curDriver.asleep = false;
			});
			toil.AddEndCondition(delegate
			{
				if (toil.actor.GetComp<CompActivity>().IsActive)
				{
					return JobCondition.InterruptForced;
				}
				return JobCondition.Ongoing;
			});
			return toil;
		}

		// Token: 0x0400537E RID: 21374
		private const int TicksBetweenSleepZs = 100;

		// Token: 0x0400537F RID: 21375
		private const int GetUpOrStartJobWhileInBedCheckInterval = 211;

		// Token: 0x04005380 RID: 21376
		private const int SlavesInSleepingRoomCheckInterval = 2500;

		// Token: 0x04005381 RID: 21377
		private const float VelocityForAdults = 0.42f;

		// Token: 0x04005382 RID: 21378
		private const float VelocityForBabies = 0.25f;

		// Token: 0x04005383 RID: 21379
		private const float VelocityForChildren = 0.33f;
	}
}
