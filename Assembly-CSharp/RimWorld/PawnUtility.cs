using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020023AB RID: 9131
	public static class PawnUtility
	{
		// Token: 0x0600C5CF RID: 50639 RVA: 0x00399460 File Offset: 0x00397660
		public static Faction GetFactionLeaderFaction(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].leader == pawn)
				{
					return allFactionsListForReading[i];
				}
			}
			return null;
		}

		// Token: 0x0600C5D0 RID: 50640 RVA: 0x003994A1 File Offset: 0x003976A1
		public static bool IsFactionLeader(Pawn pawn)
		{
			return PawnUtility.GetFactionLeaderFaction(pawn) != null;
		}

		// Token: 0x0600C5D1 RID: 50641 RVA: 0x003994AC File Offset: 0x003976AC
		public static bool IsInteractionBlocked(this Pawn pawn, InteractionDef interaction, bool isInitiator, bool isRandom)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			if (mentalStateDef == null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].def.blocksSocialInteraction)
					{
						return true;
					}
				}
				Lord lord = pawn.GetLord();
				return lord != null && lord.BlocksSocialInteraction(pawn);
			}
			if (isRandom)
			{
				return mentalStateDef.blockRandomInteraction;
			}
			if (interaction == null)
			{
				return false;
			}
			List<InteractionDef> list = isInitiator ? mentalStateDef.blockInteractionInitiationExcept : mentalStateDef.blockInteractionRecipientExcept;
			return list != null && !list.Contains(interaction);
		}

		// Token: 0x0600C5D2 RID: 50642 RVA: 0x00399544 File Offset: 0x00397744
		public static bool IsKidnappedPawn(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C5D3 RID: 50643 RVA: 0x00399589 File Offset: 0x00397789
		public static bool IsTravelingInTransportPodWorldObject(Pawn pawn)
		{
			return (pawn.IsWorldPawn() && ThingOwnerUtility.AnyParentIs<ActiveTransporterInfo>(pawn)) || ThingOwnerUtility.AnyParentIs<TravellingTransporters>(pawn);
		}

		// Token: 0x0600C5D4 RID: 50644 RVA: 0x003995A3 File Offset: 0x003977A3
		public static bool ForSaleBySettlement(Pawn pawn)
		{
			return pawn.ParentHolder is Settlement_TraderTracker;
		}

		// Token: 0x0600C5D5 RID: 50645 RVA: 0x003995B3 File Offset: 0x003977B3
		public static bool IsCarrying(this Pawn pawn)
		{
			return !pawn.Destroyed && pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null;
		}

		// Token: 0x0600C5D6 RID: 50646 RVA: 0x003995D5 File Offset: 0x003977D5
		public static bool IsCarryingPawn(this Pawn pawn, Pawn carryPawn = null)
		{
			return pawn.carryTracker != null && pawn.carryTracker.CarriedThing is Pawn && (carryPawn == null || pawn.carryTracker.CarriedThing == carryPawn);
		}

		// Token: 0x0600C5D7 RID: 50647 RVA: 0x00399606 File Offset: 0x00397806
		public static bool IsCarryingThing(this Pawn pawn, Thing carriedThing)
		{
			return pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing == carriedThing;
		}

		// Token: 0x0600C5D8 RID: 50648 RVA: 0x0039962D File Offset: 0x0039782D
		public static void TryDestroyStartingColonistFamily(Pawn pawn)
		{
			if (!pawn.relations.RelatedPawns.Any((Pawn x) => Find.GameInitData.startingAndOptionalPawns.Contains(x)))
			{
				PawnUtility.DestroyStartingColonistFamily(pawn);
			}
		}

		// Token: 0x0600C5D9 RID: 50649 RVA: 0x00399668 File Offset: 0x00397868
		public static void DestroyStartingColonistFamily(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns.ToList<Pawn>())
			{
				if (!Find.GameInitData.startingAndOptionalPawns.Contains(pawn2))
				{
					WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn2);
					if (situation == WorldPawnSituation.Free || situation == WorldPawnSituation.Dead)
					{
						Find.WorldPawns.RemovePawn(pawn2);
						Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					}
				}
			}
		}

		// Token: 0x0600C5DA RID: 50650 RVA: 0x003996FC File Offset: 0x003978FC
		public static bool EnemiesAreNearby(Pawn pawn, int regionsToScan = 9, bool passDoors = false, float maxDistance = -1f, int maxCount = 1, bool invisible = false)
		{
			TraverseParms tp = passDoors ? TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false) : TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			int count = 0;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				int count;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if ((maxDistance <= 0f || thing.Position.InHorDistOf(pawn.Position, maxDistance)) && thing.HostileTo(pawn))
					{
						if (!invisible)
						{
							Pawn pawn2 = thing as Pawn;
							if (pawn2 != null && pawn2.IsPsychologicallyInvisible())
							{
								goto IL_83;
							}
						}
						count = count;
						count++;
					}
					IL_83:;
				}
				return count >= maxCount;
			}, regionsToScan, RegionType.Set_Passable);
			return count >= maxCount;
		}

		// Token: 0x0600C5DB RID: 50651 RVA: 0x003997A4 File Offset: 0x003979A4
		public static bool WillSoonHaveBasicNeed(Pawn p, float thresholdOffset = 0.05f)
		{
			return p.needs != null && ((p.needs.rest != null && p.needs.rest.CurLevel < 0.28f + thresholdOffset) || (p.needs.food != null && p.needs.food.CurLevelPercentage < p.needs.food.PercentageThreshHungry + thresholdOffset));
		}

		// Token: 0x0600C5DC RID: 50652 RVA: 0x00399818 File Offset: 0x00397A18
		public static bool CanCasuallyInteractNow(this Pawn p, bool twoWayInteraction = false, bool canInteractWhileSleeping = false, bool canInteractWhileRoaming = false, bool canInteractWhileDrafted = false)
		{
			if (p.Drafted && !canInteractWhileDrafted)
			{
				return false;
			}
			if (p.IsPsychologicallyInvisible())
			{
				return false;
			}
			if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p))
			{
				return false;
			}
			if (p.InAggroMentalState)
			{
				return false;
			}
			if (p.InMentalState && p.MentalStateDef == MentalStateDefOf.Roaming && !canInteractWhileRoaming)
			{
				return false;
			}
			if (!p.Awake() && !canInteractWhileSleeping)
			{
				return false;
			}
			if (p.IsFormingCaravan())
			{
				return false;
			}
			Job curJob = p.CurJob;
			return curJob == null || !twoWayInteraction || (curJob.def.casualInterruptible && curJob.playerForced);
		}

		// Token: 0x0600C5DD RID: 50653 RVA: 0x003998A7 File Offset: 0x00397AA7
		public static IEnumerable<Pawn> SpawnedMasteredPawns(Pawn master)
		{
			if (Current.ProgramState != ProgramState.Playing || master.Faction == null || !master.RaceProps.Humanlike)
			{
				yield break;
			}
			if (!master.Spawned)
			{
				yield break;
			}
			List<Pawn> pawns = master.Map.mapPawns.SpawnedPawnsInFaction(master.Faction);
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].playerSettings != null && pawns[i].playerSettings.Master == master)
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600C5DE RID: 50654 RVA: 0x003998B7 File Offset: 0x00397AB7
		public static bool InValidState(Pawn p)
		{
			return p.health != null && (p.Dead || (p.stances != null && p.mindState != null && p.needs != null && p.ageTracker != null));
		}

		// Token: 0x0600C5DF RID: 50655 RVA: 0x003998F0 File Offset: 0x00397AF0
		public static PawnPosture GetPosture(this Pawn p)
		{
			PawnPosture pawnPosture;
			if (p.Dead)
			{
				pawnPosture = PawnPosture.LayingOnGroundNormal;
			}
			else if (ModsConfig.BiotechActive && p.IsCharging())
			{
				pawnPosture = PawnPosture.Standing;
			}
			else
			{
				IThingHolderWithDrawnPawn thingHolderWithDrawnPawn = p.ParentHolder as IThingHolderWithDrawnPawn;
				if (thingHolderWithDrawnPawn != null)
				{
					pawnPosture = thingHolderWithDrawnPawn.HeldPawnPosture;
				}
				else if (p.Downed)
				{
					if (p.jobs != null && p.jobs.posture.Laying())
					{
						pawnPosture = p.jobs.posture;
					}
					else
					{
						pawnPosture = PawnPosture.LayingOnGroundNormal;
					}
				}
				else if (p.jobs == null)
				{
					pawnPosture = PawnPosture.Standing;
				}
				else
				{
					pawnPosture = p.jobs.posture;
				}
			}
			Pawn_MindState mindState = p.mindState;
			bool flag;
			if (mindState == null)
			{
				flag = false;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				bool? flag2;
				if (duty == null)
				{
					flag2 = null;
				}
				else
				{
					DutyDef def = duty.def;
					flag2 = ((def != null) ? new bool?(def.forceFaceUpPosture) : null);
				}
				bool? flag3 = flag2;
				bool flag4 = true;
				flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
			}
			if (flag && pawnPosture != PawnPosture.Standing)
			{
				pawnPosture |= PawnPosture.FaceUpMask;
			}
			return pawnPosture;
		}

		// Token: 0x0600C5E0 RID: 50656 RVA: 0x003999E0 File Offset: 0x00397BE0
		public static void ForceWait(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false, bool maintainSleep = false)
		{
			if (ticks <= 0)
			{
				Log.ErrorOnce("Forcing a wait for zero ticks", 47045639);
			}
			JobDef def = maintainPosture ? JobDefOf.Wait_MaintainPosture : JobDefOf.Wait;
			if (pawn.IsDeactivated())
			{
				def = JobDefOf.Deactivated;
			}
			if (pawn.IsSelfShutdown())
			{
				def = JobDefOf.SelfShutdown;
			}
			else if (pawn.InBed())
			{
				def = (pawn.Awake() ? JobDefOf.LayDownAwake : JobDefOf.LayDown);
			}
			else if (!pawn.health.capacities.CanBeAwake)
			{
				def = JobDefOf.Wait_Downed;
			}
			else if (maintainSleep && !pawn.Awake())
			{
				def = JobDefOf.Wait_Asleep;
			}
			Job job = JobMaker.MakeJob(def, faceTarget);
			if (maintainSleep && !pawn.Awake())
			{
				job.forceSleep = true;
				job.targetA = pawn.Position;
			}
			if (pawn.InBed())
			{
				job.targetA = pawn.CurrentBed();
			}
			job.expiryInterval = ticks;
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x0600C5E1 RID: 50657 RVA: 0x00399AF4 File Offset: 0x00397CF4
		public static void GainComfortFromCellIfPossible(this Pawn p, int delta, bool chairsOnly = false)
		{
			if (!p.Spawned)
			{
				return;
			}
			if (p.IsHashIntervalTick(15, delta))
			{
				Building edifice = p.Position.GetEdifice(p.Map);
				if (edifice != null && (!chairsOnly || (edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable)))
				{
					PawnUtility.GainComfortFromThingIfPossible(p, edifice, delta);
				}
			}
		}

		// Token: 0x0600C5E2 RID: 50658 RVA: 0x00399B58 File Offset: 0x00397D58
		public static void GainComfortFromThingIfPossible(Pawn p, Thing from, int delta)
		{
			if (p.IsHashIntervalTick(15, delta))
			{
				float statValue = from.GetStatValue(StatDefOf.Comfort, true, 100);
				if (statValue >= 0f && p.needs != null && p.needs.comfort != null)
				{
					p.needs.comfort.ComfortUsed(statValue);
				}
			}
		}

		// Token: 0x0600C5E3 RID: 50659 RVA: 0x00399BB0 File Offset: 0x00397DB0
		public static float BodyResourceGrowthSpeed(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			float? num;
			if (needs == null)
			{
				num = null;
			}
			else
			{
				Need_Food food = needs.food;
				num = ((food != null) ? new float?(food.CurCategory.HungerMultiplier()) : null);
			}
			float? num2 = num;
			if (num2 == null)
			{
				return 1f;
			}
			return num2.GetValueOrDefault();
		}

		// Token: 0x0600C5E4 RID: 50660 RVA: 0x00399C0C File Offset: 0x00397E0C
		public static bool FertileMateTarget(Pawn male, Pawn female)
		{
			if (female.gender != Gender.Female || female.Sterile())
			{
				return false;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			return compEggLayer == null || !compEggLayer.FullyFertilized;
		}

		// Token: 0x0600C5E5 RID: 50661 RVA: 0x00399C44 File Offset: 0x00397E44
		public static void Mated(Pawn male, Pawn female)
		{
			if (female.Sterile() || male.Sterile())
			{
				return;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			if (compEggLayer != null)
			{
				compEggLayer.Fertilize(male);
				return;
			}
			if (Rand.Value < 0.5f)
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, female, null);
				hediff_Pregnant.SetParents(null, male, null);
				female.health.AddHediff(hediff_Pregnant, null, null, null);
			}
		}

		// Token: 0x0600C5E6 RID: 50662 RVA: 0x00399CB4 File Offset: 0x00397EB4
		public static bool PlayerForcedJobNowOrSoon(Pawn pawn)
		{
			if (pawn.jobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob != null)
			{
				return curJob.playerForced;
			}
			return pawn.jobs.jobQueue.Count > 0 && pawn.jobs.jobQueue.Peek().job.playerForced;
		}

		// Token: 0x0600C5E7 RID: 50663 RVA: 0x00399D0C File Offset: 0x00397F0C
		public static bool TrySpawnHatchedOrBornPawn(Pawn pawn, Thing motherOrEgg, IntVec3? positionOverride = null)
		{
			if (motherOrEgg.SpawnedOrAnyParentSpawned)
			{
				return GenSpawn.Spawn(pawn, positionOverride ?? motherOrEgg.PositionHeld, motherOrEgg.MapHeld, WipeMode.Vanish) != null;
			}
			Pawn pawn2 = motherOrEgg as Pawn;
			if (pawn2 != null)
			{
				if (pawn2.IsCaravanMember())
				{
					pawn2.GetCaravan().AddPawn(pawn, true);
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
				if (pawn2.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
			}
			else if (motherOrEgg.ParentHolder != null)
			{
				Pawn_InventoryTracker pawn_InventoryTracker = motherOrEgg.ParentHolder as Pawn_InventoryTracker;
				if (pawn_InventoryTracker != null)
				{
					if (pawn_InventoryTracker.pawn.IsCaravanMember())
					{
						pawn_InventoryTracker.pawn.GetCaravan().AddPawn(pawn, true);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
					if (pawn_InventoryTracker.pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C5E8 RID: 50664 RVA: 0x00399DF0 File Offset: 0x00397FF0
		public static bool TryGetAvoidGrid(this Pawn p, out AvoidGrid grid, bool onlyIfLordAllows = true)
		{
			grid = null;
			if (!p.Spawned)
			{
				return false;
			}
			if (p.Faction == null)
			{
				return false;
			}
			if (!p.Faction.def.canUseAvoidGrid)
			{
				return false;
			}
			if (p.Faction == Faction.OfPlayer || !p.Faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (p.kindDef.canUseAvoidGrid)
			{
				grid = p.Map.avoidGrid;
				return true;
			}
			if (!onlyIfLordAllows)
			{
				grid = p.Map.avoidGrid;
				return true;
			}
			Lord lord = p.GetLord();
			LordToil lordToil = (lord != null) ? lord.CurLordToil : null;
			if (lordToil != null && lordToil.useAvoidGrid)
			{
				grid = lord.Map.avoidGrid;
				return true;
			}
			return false;
		}

		// Token: 0x0600C5E9 RID: 50665 RVA: 0x00399EA8 File Offset: 0x003980A8
		public static bool ShouldCollideWithPawns(Pawn p)
		{
			return p != null && !p.Downed && !p.Dead && (p.IsShambler || (p.mindState.anyCloseHostilesRecently && !p.IsPsychologicallyInvisible() && p.kindDef.collidesWithPawns));
		}

		// Token: 0x0600C5EA RID: 50666 RVA: 0x00399EFD File Offset: 0x003980FD
		public static bool AnyPawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false, bool useId = false)
		{
			return PawnUtility.PawnBlockingPathAt(c, forPawn, actAsIfHadCollideWithPawnsJob, collideOnlyWithStandingPawns, forPathFinder, useId) != null;
		}

		// Token: 0x0600C5EB RID: 50667 RVA: 0x00399F10 File Offset: 0x00398110
		public static Pawn PawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false, bool useId = false)
		{
			List<Thing> thingList = c.GetThingList(forPawn.Map);
			if (thingList.Count == 0)
			{
				return null;
			}
			bool collideWithNonHostile = false;
			if (actAsIfHadCollideWithPawnsJob)
			{
				collideWithNonHostile = true;
			}
			else
			{
				Job curJob = forPawn.CurJob;
				if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns || forPawn.jobs.curDriver.collideWithPawns))
				{
					collideWithNonHostile = true;
				}
			}
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (PawnUtility.PawnBlockedBy(forPawn, pawn, collideOnlyWithStandingPawns, collideWithNonHostile, forPathFinder, useId))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x0600C5EC RID: 50668 RVA: 0x00399FA4 File Offset: 0x003981A4
		public static bool PawnBlockedBy(Pawn forPawn, Pawn other, bool collideOnlyWithStandingPawns = false, bool collideWithNonHostile = false, bool forPathFinder = false, bool useId = false)
		{
			if (!PawnUtility.ShouldCollideWithPawns(other))
			{
				return false;
			}
			if (useId && forPawn.thingIDNumber < other.thingIDNumber && !other.stances.FullBodyBusyRecently)
			{
				return false;
			}
			if (other == forPawn)
			{
				return false;
			}
			if (collideOnlyWithStandingPawns)
			{
				if (other.pather.MovingNow)
				{
					return false;
				}
				if (other.pather.Moving && other.pather.MovedRecently(60))
				{
					return false;
				}
			}
			if (PawnUtility.PawnsCanShareCellBecauseOfBodySize(other, forPawn))
			{
				return false;
			}
			if (other.IsPsychologicallyInvisible())
			{
				return false;
			}
			if (!other.kindDef.collidesWithPawns)
			{
				return false;
			}
			if (other.HostileTo(forPawn))
			{
				return true;
			}
			if (forPawn.IsShambler && !MutantUtility.ShamblerShouldCollideWith(forPawn, other))
			{
				return false;
			}
			if (collideWithNonHostile)
			{
				if (!forPathFinder && forPawn.Drafted && other.RaceProps.Animal)
				{
					return false;
				}
				Job curJob = other.CurJob;
				if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns || other.jobs.curDriver.collideWithPawns))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C5ED RID: 50669 RVA: 0x0039A0A8 File Offset: 0x003982A8
		private static bool PawnsCanShareCellBecauseOfBodySize(Pawn p1, Pawn p2)
		{
			float bodySize = p1.BodySize;
			float bodySize2 = p2.BodySize;
			if (bodySize >= 1.5f || bodySize2 >= 1.5f)
			{
				return false;
			}
			float num = bodySize / bodySize2;
			if (num < 1f)
			{
				num = 1f / num;
			}
			return num > 3.57f;
		}

		// Token: 0x0600C5EE RID: 50670 RVA: 0x0039A0F4 File Offset: 0x003982F4
		public static bool KnownDangerAt(IntVec3 c, Map map, Pawn forPawn)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.IsDangerousFor(forPawn);
		}

		// Token: 0x0600C5EF RID: 50671 RVA: 0x0039A118 File Offset: 0x00398318
		[Obsolete("Lord and job report display validation is now checked separately.")]
		public static bool ShouldDisplayActionInInspectString(Pawn p)
		{
			return (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer) && !p.InMentalState && (!p.IsMutant || !p.mutant.Def.overrideInspectString);
		}

		// Token: 0x0600C5F0 RID: 50672 RVA: 0x0039A168 File Offset: 0x00398368
		public static bool ShouldDisplayLordReport(Pawn pawn)
		{
			return PawnUtility.ShouldShowCultistLordReport(pawn) || PawnUtility.ShouldShowActionReportToPlayer(pawn);
		}

		// Token: 0x0600C5F1 RID: 50673 RVA: 0x0039A17C File Offset: 0x0039837C
		public static bool ShouldDisplayJobReport(Pawn pawn)
		{
			return (!pawn.IsMutant || !pawn.mutant.Def.overrideInspectString) && ((pawn.CurJobDef != null && pawn.CurJobDef.alwaysShowReport) || (ModsConfig.AnomalyActive && pawn.Faction == Faction.OfHoraxCult && pawn.CurJobDef == JobDefOf.HateChanting) || PawnUtility.ShouldShowCultistLordReport(pawn) || PawnUtility.ShouldShowActionReportToPlayer(pawn));
		}

		// Token: 0x0600C5F2 RID: 50674 RVA: 0x0039A1F0 File Offset: 0x003983F0
		private static bool ShouldShowActionReportToPlayer(Pawn p)
		{
			return (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer) && !p.InMentalState && (!p.IsMutant || !p.mutant.Def.overrideInspectString);
		}

		// Token: 0x0600C5F3 RID: 50675 RVA: 0x0039A240 File Offset: 0x00398440
		public static bool ShouldDisplayFactionInInspectString(Pawn p)
		{
			return !p.IsMutant || !p.mutant.Def.overrideInspectString;
		}

		// Token: 0x0600C5F4 RID: 50676 RVA: 0x0039A25F File Offset: 0x0039845F
		private static bool ShouldShowCultistLordReport(Pawn pawn)
		{
			if (ModsConfig.AnomalyActive && pawn.Faction == Faction.OfHoraxCult)
			{
				PawnDuty duty = pawn.mindState.duty;
				if (((duty != null) ? duty.def : null) == DutyDefOf.Invoke)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C5F5 RID: 50677 RVA: 0x0039A298 File Offset: 0x00398498
		public static bool ShouldSendNotificationAbout(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (p == null)
			{
				return false;
			}
			if (PawnGenerator.IsBeingGenerated(p))
			{
				return false;
			}
			if (p.IsWorldPawn() && (!p.IsCaravanMember() || !p.GetCaravan().IsPlayerControlled) && !PawnUtility.IsTravelingInTransportPodWorldObject(p) && !p.IsBorrowedByAnyFaction() && p.Corpse.DestroyedOrNull())
			{
				return false;
			}
			if (p.IsSubhuman)
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && p.Corpse is UnnaturalCorpse)
			{
				return false;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				if (p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (p.RaceProps.Humanlike && p.guest.Released && !p.Downed && !p.InBed())
				{
					return false;
				}
				if (p.CurJob != null && p.CurJob.exitMapOnArrival && !PrisonBreakUtility.IsPrisonBreaking(p))
				{
					return false;
				}
				if (PawnUtility.IsExitingMap(p))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C5F6 RID: 50678 RVA: 0x0039A38C File Offset: 0x0039858C
		public static bool ShouldGetThoughtAbout(Pawn pawn, Pawn subject)
		{
			return !pawn.IsSubhuman && !subject.IsSubhuman && (pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn()));
		}

		// Token: 0x0600C5F7 RID: 50679 RVA: 0x0039A3C4 File Offset: 0x003985C4
		public static bool IsTeetotaler(this Pawn pawn)
		{
			return !new HistoryEvent(HistoryEventDefOf.IngestedDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo() || !new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo() || (pawn.story != null && pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) < 0);
		}

		// Token: 0x0600C5F8 RID: 50680 RVA: 0x0039A430 File Offset: 0x00398630
		public static bool CanTakeDrug(this Pawn pawn, ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			if (compProperties == null)
			{
				return true;
			}
			if (PawnUtility.CanTakeDrugForDependency(pawn, drug))
			{
				return true;
			}
			if (!compProperties.teetotalerCanConsume && pawn.IsTeetotaler())
			{
				return false;
			}
			if (ModsConfig.IdeologyActive)
			{
				if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.IngestedDrug, pawn))
				{
					return false;
				}
				if (drug.IsNonMedicalDrug && !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.IngestedRecreationalDrug, pawn))
				{
					return false;
				}
				if (drug.ingestible != null && drug.ingestible.drugCategory == DrugCategory.Hard && !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.IngestedHardDrug, pawn))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C5F9 RID: 50681 RVA: 0x0039A4BC File Offset: 0x003986BC
		public static bool CanTakeDrugForDependency(Pawn pawn, ThingDef drug)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			if (compProperties == null)
			{
				return true;
			}
			foreach (Gene gene in pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null && gene.Active && gene_ChemicalDependency.def.chemical == compProperties.chemical)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C5FA RID: 50682 RVA: 0x0039A558 File Offset: 0x00398758
		public static bool TryGetChemicalDependencyGene(Pawn pawn, out Gene_ChemicalDependency gene)
		{
			gene = null;
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			foreach (Gene gene2 in pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene2 as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null)
				{
					gene = gene_ChemicalDependency;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C5FB RID: 50683 RVA: 0x0039A5D0 File Offset: 0x003987D0
		public static bool PawnWouldBeUnhappyTakingDrug(this Pawn pawn, ThingDef drug)
		{
			return pawn.IsTeetotaler() && drug.GetCompProperties<CompProperties_Drug>() != null;
		}

		// Token: 0x0600C5FC RID: 50684 RVA: 0x0039A5E5 File Offset: 0x003987E5
		public static bool IsProsthophobe(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.BodyPurist);
		}

		// Token: 0x0600C5FD RID: 50685 RVA: 0x0039A606 File Offset: 0x00398806
		public static bool IsPrisonerInPrisonCell(this Pawn pawn)
		{
			return pawn.IsPrisoner && pawn.Spawned && pawn.Position.IsInPrisonCell(pawn.Map);
		}

		// Token: 0x0600C5FE RID: 50686 RVA: 0x0039A62C File Offset: 0x0039882C
		public static bool IsBeingArrested(Pawn pawn)
		{
			if (pawn.Map == null)
			{
				return false;
			}
			foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (pawn2 != pawn && pawn2.CurJobDef == JobDefOf.Arrest && pawn2.CurJob.AnyTargetIs(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C5FF RID: 50687 RVA: 0x0039A6B4 File Offset: 0x003988B4
		public static string PawnKindsToCommaList(IEnumerable<Pawn> pawns, bool useAnd = false)
		{
			PawnUtility.tmpPawns.Clear();
			PawnUtility.tmpPawns.AddRange(pawns);
			if (PawnUtility.tmpPawns.Count >= 2)
			{
				PawnUtility.tmpPawns.SortBy((Pawn x) => !x.RaceProps.Humanlike, (Pawn x) => x.GetKindLabelPlural(-1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawns.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawns[i].kindDef))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawns[i].kindDef);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawns.Count; j++)
					{
						if (PawnUtility.tmpPawns[j].kindDef == PawnUtility.tmpPawns[i].kindDef)
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + PawnUtility.tmpPawns[i].KindLabel);
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num.ToString() + " " + PawnUtility.tmpPawns[i].GetKindLabelPlural(num));
					}
				}
			}
			PawnUtility.tmpPawns.Clear();
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd, false);
		}

		// Token: 0x0600C600 RID: 50688 RVA: 0x0039A840 File Offset: 0x00398A40
		public static List<string> PawnKindsToList(IEnumerable<PawnKindDef> pawnKinds)
		{
			PawnUtility.tmpPawnKinds.Clear();
			PawnUtility.tmpPawnKinds.AddRange(pawnKinds);
			if (PawnUtility.tmpPawnKinds.Count >= 2)
			{
				PawnUtility.tmpPawnKinds.SortBy((PawnKindDef x) => !x.RaceProps.Humanlike, (PawnKindDef x) => GenLabel.BestKindLabel(x, Gender.None, true, -1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawnKinds.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawnKinds[i]))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawnKinds[i]);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawnKinds.Count; j++)
					{
						if (PawnUtility.tmpPawnKinds[j] == PawnUtility.tmpPawnKinds[i])
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, false, -1));
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num.ToString() + " " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, true, num));
					}
				}
			}
			return PawnUtility.tmpPawnKindsStr;
		}

		// Token: 0x0600C601 RID: 50689 RVA: 0x0039A9A9 File Offset: 0x00398BA9
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x0600C602 RID: 50690 RVA: 0x0039A9C0 File Offset: 0x00398BC0
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix, Color color)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			for (int i = 0; i < PawnUtility.tmpPawnKindsStr.Count; i++)
			{
				PawnUtility.tmpPawnKindsStr[i] = PawnUtility.tmpPawnKindsStr[i].Colorize(color);
			}
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x0600C603 RID: 50691 RVA: 0x0039AA10 File Offset: 0x00398C10
		public static string PawnKindsToCommaList(IEnumerable<PawnKindDef> pawnKinds, bool useAnd = false)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd, false);
		}

		// Token: 0x0600C604 RID: 50692 RVA: 0x0039AA25 File Offset: 0x00398C25
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.locomotion != LocomotionUrgency.None)
			{
				return pawn.mindState.duty.locomotion;
			}
			return secondPriority;
		}

		// Token: 0x0600C605 RID: 50693 RVA: 0x0039AA60 File Offset: 0x00398C60
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority, LocomotionUrgency thirdPriority)
		{
			LocomotionUrgency locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, secondPriority);
			if (locomotionUrgency != LocomotionUrgency.None)
			{
				return locomotionUrgency;
			}
			return thirdPriority;
		}

		// Token: 0x0600C606 RID: 50694 RVA: 0x0039AA7B File Offset: 0x00398C7B
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.maxDanger != Danger.Unspecified)
			{
				return pawn.mindState.duty.maxDanger;
			}
			return secondPriority;
		}

		// Token: 0x0600C607 RID: 50695 RVA: 0x0039AAB8 File Offset: 0x00398CB8
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority, Danger thirdPriority)
		{
			Danger danger = PawnUtility.ResolveMaxDanger(pawn, secondPriority);
			if (danger != Danger.Unspecified)
			{
				return danger;
			}
			return thirdPriority;
		}

		// Token: 0x0600C608 RID: 50696 RVA: 0x0039AAD3 File Offset: 0x00398CD3
		public static bool IsPermanentCombatant(this Pawn pawn)
		{
			return ((pawn != null) ? pawn.mindState : null) == null || ((!pawn.IsAnimal || pawn.Faction == null) && !pawn.DevelopmentalStage.Juvenile());
		}

		// Token: 0x0600C609 RID: 50697 RVA: 0x0039AB07 File Offset: 0x00398D07
		public static bool IsCombatant(this Pawn pawn)
		{
			return pawn.IsPermanentCombatant() || pawn.mindState.CombatantRecently;
		}

		// Token: 0x0600C60A RID: 50698 RVA: 0x0039AB20 File Offset: 0x00398D20
		public static bool IsFighting(this Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic || pawn.CurJob.def == JobDefOf.Wait_Combat || pawn.CurJob.def == JobDefOf.PredatorHunt || pawn.CurJob.def == JobDefOf.ManTurret);
		}

		// Token: 0x0600C60B RID: 50699 RVA: 0x0039AB94 File Offset: 0x00398D94
		public static bool IsAttacking(this Pawn pawn)
		{
			if (pawn.CurJobDef == JobDefOf.AttackMelee || pawn.CurJobDef == JobDefOf.AttackStatic)
			{
				return true;
			}
			if (pawn.CurJobDef == JobDefOf.Wait_Combat)
			{
				Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
				if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C60C RID: 50700 RVA: 0x0039ABEE File Offset: 0x00398DEE
		public static Hediff_Psylink GetMainPsylinkSource(this Pawn pawn)
		{
			return (Hediff_Psylink)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
		}

		// Token: 0x0600C60D RID: 50701 RVA: 0x0039AC0C File Offset: 0x00398E0C
		public static int GetPsylinkLevel(this Pawn pawn)
		{
			int num = 0;
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				Hediff_Psylink hediff_Psylink = hediff as Hediff_Psylink;
				if (hediff_Psylink != null)
				{
					num += hediff_Psylink.level;
				}
			}
			return num;
		}

		// Token: 0x0600C60E RID: 50702 RVA: 0x0039AC78 File Offset: 0x00398E78
		public static int GetMaxPsylinkLevel(this Pawn pawn)
		{
			return (int)HediffDefOf.PsychicAmplifier.maxSeverity;
		}

		// Token: 0x0600C60F RID: 50703 RVA: 0x0039AC88 File Offset: 0x00398E88
		public static RoyalTitle GetMaxPsylinkLevelTitle(this Pawn pawn)
		{
			if (pawn.royalty == null)
			{
				return null;
			}
			int num = 0;
			RoyalTitle result = null;
			foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
			{
				if (num < royalTitle.def.maxPsylinkLevel)
				{
					num = royalTitle.def.maxPsylinkLevel;
					result = royalTitle;
				}
			}
			return result;
		}

		// Token: 0x0600C610 RID: 50704 RVA: 0x0039AD04 File Offset: 0x00398F04
		public static int GetMaxPsylinkLevelByTitle(this Pawn pawn)
		{
			RoyalTitle maxPsylinkLevelTitle = pawn.GetMaxPsylinkLevelTitle();
			if (maxPsylinkLevelTitle == null)
			{
				return 0;
			}
			return maxPsylinkLevelTitle.def.maxPsylinkLevel;
		}

		// Token: 0x0600C611 RID: 50705 RVA: 0x0039AD28 File Offset: 0x00398F28
		public static void ChangePsylinkLevel(this Pawn pawn, int levelOffset, bool sendLetter = true)
		{
			Hediff_Psylink hediff_Psylink = pawn.GetMainPsylinkSource();
			if (hediff_Psylink == null)
			{
				hediff_Psylink = (Hediff_Psylink)HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, null);
				try
				{
					hediff_Psylink.suppressPostAddLetter = !sendLetter;
					pawn.health.AddHediff(hediff_Psylink, pawn.health.hediffSet.GetBrain(), null, null);
					return;
				}
				finally
				{
					hediff_Psylink.suppressPostAddLetter = false;
				}
			}
			hediff_Psylink.ChangeLevel(levelOffset, sendLetter);
		}

		// Token: 0x0600C612 RID: 50706 RVA: 0x0039ADA4 File Offset: 0x00398FA4
		public static void GiveAllStartingPlayerPawnsThought(ThoughtDef thought)
		{
			foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
			{
				if (pawn.needs.mood != null)
				{
					if (thought.IsSocial)
					{
						using (List<Pawn>.Enumerator enumerator2 = Find.GameInitData.startingAndOptionalPawns.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Pawn pawn2 = enumerator2.Current;
								if (pawn2 != pawn)
								{
									pawn.needs.mood.thoughts.memories.TryGainMemory(thought, pawn2, null);
								}
							}
							continue;
						}
					}
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought, null, null);
				}
			}
		}

		// Token: 0x0600C613 RID: 50707 RVA: 0x0039AE8C File Offset: 0x0039908C
		public static IntVec3 DutyLocation(this Pawn pawn)
		{
			Pawn_RopeTracker roping = pawn.roping;
			if (roping != null && roping.IsRopedToSpot)
			{
				return pawn.roping.RopedToSpot;
			}
			if (pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid)
			{
				return pawn.mindState.duty.focus.Cell;
			}
			return pawn.Position;
		}

		// Token: 0x0600C614 RID: 50708 RVA: 0x0039AEF9 File Offset: 0x003990F9
		public static bool EverBeenColonistOrTameAnimal(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsColonistOrColonyAnimal) > 0;
		}

		// Token: 0x0600C615 RID: 50709 RVA: 0x0039AF0E File Offset: 0x0039910E
		public static bool EverBeenPrisoner(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsPrisoner) > 0;
		}

		// Token: 0x0600C616 RID: 50710 RVA: 0x0039AF23 File Offset: 0x00399123
		public static bool EverBeenQuestLodger(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsQuestLodger) > 0;
		}

		// Token: 0x0600C617 RID: 50711 RVA: 0x0039AF38 File Offset: 0x00399138
		public static void RecoverFromUnwalkablePositionOrKill(IntVec3 c, Map map)
		{
			if (!c.InBounds(map) || c.Walkable(map))
			{
				return;
			}
			PawnUtility.tmpThings.Clear();
			PawnUtility.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < PawnUtility.tmpThings.Count; i++)
			{
				Pawn pawn = PawnUtility.tmpThings[i] as Pawn;
				if (pawn != null)
				{
					IntVec3 position;
					if (CellFinder.TryFindBestPawnStandCell(pawn, out position, false))
					{
						pawn.Position = position;
						pawn.Notify_Teleported(true, false);
					}
					else
					{
						DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, pawn.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.Collapse, null, true, true, QualityCategory.Normal, true, false);
						damageInfo.SetIgnoreInstantKillProtection(true);
						pawn.TakeDamage(damageInfo);
						if (!pawn.Dead)
						{
							pawn.Kill(new DamageInfo?(damageInfo), null);
						}
					}
				}
			}
		}

		// Token: 0x0600C618 RID: 50712 RVA: 0x0039B018 File Offset: 0x00399218
		public static float GetManhunterOnDamageChance(Pawn pawn, Thing instigator = null, float distance = -1f)
		{
			float num = PawnUtility.GetManhunterOnDamageChance(pawn.def);
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				num += 0.5f;
			}
			if (instigator != null)
			{
				num *= GenMath.LerpDoubleClamped(1f, 30f, 3f, 1f, distance);
				num *= 1f - instigator.GetStatValue(StatDefOf.HuntingStealth, true, -1);
				Pawn pawn2 = instigator as Pawn;
				if (pawn2 != null)
				{
					num *= PawnUtility.GetManhunterChanceFactorForInstigator(pawn2);
				}
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x0600C619 RID: 50713 RVA: 0x0039B0A0 File Offset: 0x003992A0
		public static float GetManhunterOnDamageChance(ThingDef def)
		{
			return def.race.manhunterOnDamageChance * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		// Token: 0x0600C61A RID: 50714 RVA: 0x0039B0C0 File Offset: 0x003992C0
		public static string GetManhunterOnDamageChanceExplanation(ThingDef def, Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("HarmedRevengeChanceExplanation".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(def.LabelCap + ": " + def.race.manhunterOnDamageChance.ToStringPercent());
			stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.manhunterChanceOnDamageFactor.ToStringPercent());
			if (pawn != null && pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				stringBuilder.AppendLine(HediffDefOf.Scaria.LabelCap + ": " + 0.5f.ToStringPercentSigned());
			}
			stringBuilder.AppendLine();
			float f = (pawn == null) ? PawnUtility.GetManhunterOnDamageChance(def) : PawnUtility.GetManhunterOnDamageChance(pawn, null, -1f);
			stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + f.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x0600C61B RID: 50715 RVA: 0x0039B208 File Offset: 0x00399408
		public static float GetManhunterChanceFactorForInstigator(Pawn instigator)
		{
			if (ModsConfig.AnomalyActive && ((instigator != null) ? instigator.Faction : null) == Faction.OfEntities)
			{
				return 0f;
			}
			float num = 1f;
			if (ModsConfig.IdeologyActive && ((instigator != null) ? instigator.Ideo : null) != null)
			{
				Precept_Role role = instigator.Ideo.GetRole(instigator);
				RoleEffect roleEffect;
				if (role == null)
				{
					roleEffect = null;
				}
				else
				{
					List<RoleEffect> roleEffects = role.def.roleEffects;
					if (roleEffects == null)
					{
						roleEffect = null;
					}
					else
					{
						roleEffect = roleEffects.FirstOrDefault((RoleEffect eff) => eff is RoleEffect_HuntingRevengeChanceFactor);
					}
				}
				RoleEffect roleEffect2 = roleEffect;
				if (roleEffect2 != null)
				{
					num *= ((RoleEffect_HuntingRevengeChanceFactor)roleEffect2).factor;
				}
			}
			return num;
		}

		// Token: 0x0600C61C RID: 50716 RVA: 0x0039B2AB File Offset: 0x003994AB
		public static float GetManhunterOnTameFailChance(Pawn pawn)
		{
			return Mathf.Clamp01(PawnUtility.GetManhunterOnTameFailChance(pawn.def));
		}

		// Token: 0x0600C61D RID: 50717 RVA: 0x0039B2BD File Offset: 0x003994BD
		public static float GetManhunterOnTameFailChance(ThingDef def)
		{
			return def.race.manhunterOnTameFailChance;
		}

		// Token: 0x0600C61E RID: 50718 RVA: 0x0039B2CC File Offset: 0x003994CC
		public static string GetManhunterOnTameFailChanceExplanation(ThingDef def, Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Stat_Race_Animal_TameFailedRevengeChance_Desc".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(def.LabelCap + ": " + def.race.manhunterOnTameFailChance.ToStringPercent());
			stringBuilder.AppendLine();
			float f = (pawn == null) ? PawnUtility.GetManhunterOnTameFailChance(def) : PawnUtility.GetManhunterOnTameFailChance(pawn);
			stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + f.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x0600C61F RID: 50719 RVA: 0x0039B375 File Offset: 0x00399575
		public static MentalStateDef ManhunterStateFor(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				return MentalStateDefOf.ManhunterPermanent;
			}
			return MentalStateDefOf.Manhunter;
		}

		// Token: 0x0600C620 RID: 50720 RVA: 0x0039B39C File Offset: 0x0039959C
		public static bool PlayerHasReproductivePair(PawnKindDef pawnKindDef)
		{
			if (!pawnKindDef.RaceProps.Animal)
			{
				return false;
			}
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.Count; i++)
			{
				Pawn pawn = allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i];
				if (pawn.kindDef == pawnKindDef && pawn.ageTracker.CurLifeStage.reproductive)
				{
					if (pawn.gender == Gender.Male)
					{
						flag = true;
					}
					else if (pawn.gender == Gender.Female)
					{
						flag2 = true;
					}
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C621 RID: 50721 RVA: 0x0039B41C File Offset: 0x0039961C
		public static float PlayerAnimalBodySizePerCapita()
		{
			float num = 0f;
			int num2 = 0;
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.Count; i++)
			{
				Pawn pawn = allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i];
				if (pawn.IsFreeColonist && !pawn.IsQuestLodger())
				{
					num2++;
				}
				if (pawn.IsAnimal)
				{
					num += pawn.BodySize;
				}
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x0600C622 RID: 50722 RVA: 0x0039B488 File Offset: 0x00399688
		private static List<Pawn> PawnsOfFactionOnMapOrInCaravan(Pawn pawn)
		{
			List<Pawn> result;
			if (pawn.Spawned)
			{
				result = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			}
			else
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan == null)
				{
					return null;
				}
				result = caravan.PawnsListForReading;
			}
			return result;
		}

		// Token: 0x0600C623 RID: 50723 RVA: 0x0039B4CC File Offset: 0x003996CC
		public static float PlayerVeneratedAnimalBodySizePerCapitaOnMapOrCaravan(Pawn pawn)
		{
			if (pawn.Ideo == null || pawn.Faction == null)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = 0;
			List<Pawn> list = PawnUtility.PawnsOfFactionOnMapOrInCaravan(pawn);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == pawn.Faction && !pawn.IsQuestLodger())
				{
					if (list[i].IsAnimal && pawn.Ideo.IsVeneratedAnimal(list[i]))
					{
						num += list[i].BodySize;
					}
					else if (list[i].RaceProps.Humanlike)
					{
						num2++;
					}
				}
			}
			return Mathf.Round(((num2 > 0) ? (num / (float)num2) : 0f) * 100f) / 100f;
		}

		// Token: 0x0600C624 RID: 50724 RVA: 0x0039B598 File Offset: 0x00399798
		public static Pawn FirstVeneratedAnimalOnMapOrCaravan(Pawn pawn)
		{
			if (pawn.Ideo == null || pawn.Faction == null)
			{
				return null;
			}
			List<Pawn> list = PawnUtility.PawnsOfFactionOnMapOrInCaravan(pawn);
			for (int i = 0; i < list.Count; i++)
			{
				if (pawn.Faction == list[i].Faction && pawn.Ideo.IsVeneratedAnimal(list[i]))
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600C625 RID: 50725 RVA: 0x0039B600 File Offset: 0x00399800
		public static bool HasClothingNotRequiredByKind(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			if (wornApparel.Count > 0 && p.kindDef.apparelRequired.NullOrEmpty<ThingDef>())
			{
				return true;
			}
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if (apparel.def.apparel.countsAsClothingForNudity && !p.kindDef.apparelRequired.Contains(apparel.def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C626 RID: 50726 RVA: 0x0039B686 File Offset: 0x00399886
		public static IEnumerable<PawnKindDef> GetCombatPawnKindsForPoints(Func<PawnKindDef, bool> selector, float points, Func<PawnKindDef, float> selectionWeight = null)
		{
			IEnumerable<PawnKindDef> allKinds = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(selector);
			if (selectionWeight == null)
			{
				selectionWeight = ((PawnKindDef _) => 1f);
			}
			Func<PawnKindDef, bool> <>9__1;
			while (points > 0f)
			{
				IEnumerable<PawnKindDef> source = allKinds;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PawnKindDef def) => def.combatPower > 0f && def.combatPower <= points && def.appearsRandomlyInCombatGroups));
				}
				PawnKindDef pawnKindDef;
				if (!source.Where(predicate).TryRandomElementByWeight(selectionWeight, out pawnKindDef))
				{
					break;
				}
				points -= pawnKindDef.combatPower;
				yield return pawnKindDef;
			}
			yield break;
		}

		// Token: 0x0600C627 RID: 50727 RVA: 0x0039B6A4 File Offset: 0x003998A4
		public static int GetMaxAllowedToPickUp(Pawn pawn, ThingDef thingDef)
		{
			int maxAllowedToPickUp = PawnUtility.GetMaxAllowedToPickUp(thingDef, pawn.Map);
			if (maxAllowedToPickUp <= 0)
			{
				return 0;
			}
			int num = pawn.inventory.Count((Thing t) => t.def.orderedTakeGroup == thingDef.orderedTakeGroup);
			return Math.Max(maxAllowedToPickUp - num, 0);
		}

		// Token: 0x0600C628 RID: 50728 RVA: 0x0039B6F7 File Offset: 0x003998F7
		public static int GetMaxAllowedToPickUp(ThingDef thingDef, Map map = null)
		{
			if (map != null && !map.IsPlayerHome)
			{
				return int.MaxValue;
			}
			if (thingDef.orderedTakeGroup == null)
			{
				return 0;
			}
			return thingDef.orderedTakeGroup.max;
		}

		// Token: 0x0600C629 RID: 50729 RVA: 0x0039B71F File Offset: 0x0039991F
		public static bool CanPickUp(Pawn pawn, ThingDef thingDef)
		{
			return !pawn.Map.IsPlayerHome || (pawn.inventory != null && thingDef.orderedTakeGroup != null && thingDef.orderedTakeGroup.max > 0);
		}

		// Token: 0x0600C62A RID: 50730 RVA: 0x0039B750 File Offset: 0x00399950
		public static bool ShouldBeSlaughtered(this Pawn pawn)
		{
			return pawn.Spawned && pawn.IsAnimal && (pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) != null || pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Contains(pawn)) && pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.ReleaseAnimalToWild) == null;
		}

		// Token: 0x0600C62B RID: 50731 RVA: 0x0039B7BC File Offset: 0x003999BC
		public static bool CanBeBuried(this Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse == null || corpse.MapHeld == null || corpse.MapHeld.designationManager.DesignationOn(corpse, DesignationDefOf.ExtractSkull) == null;
		}

		// Token: 0x0600C62C RID: 50732 RVA: 0x0039B7F8 File Offset: 0x003999F8
		public static bool PawnHadFuneral(Pawn pawn)
		{
			Pawn_IdeoTracker ideo = pawn.ideo;
			object obj;
			if (ideo == null)
			{
				obj = null;
			}
			else
			{
				Ideo ideo2 = ideo.Ideo;
				obj = ((ideo2 != null) ? ideo2.GetPrecept(PreceptDefOf.Funeral) : null);
			}
			Precept_Ritual precept_Ritual = (Precept_Ritual)obj;
			return precept_Ritual != null && !precept_Ritual.completedObligations.NullOrEmpty<RitualObligation>() && precept_Ritual.completedObligations.Any((RitualObligation o) => o.FirstValidTarget.Thing == pawn);
		}

		// Token: 0x0600C62D RID: 50733 RVA: 0x0039B869 File Offset: 0x00399A69
		public static bool IsBiologicallyOrArtificiallyBlind(Pawn pawn)
		{
			return PawnUtility.IsBiologicallyBlind(pawn) || PawnUtility.IsArtificiallyBlind(pawn);
		}

		// Token: 0x0600C62E RID: 50734 RVA: 0x0039B87B File Offset: 0x00399A7B
		public static bool IsBiologicallyBlind(Pawn pawn)
		{
			return !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}

		// Token: 0x0600C62F RID: 50735 RVA: 0x0039B898 File Offset: 0x00399A98
		public static bool IsArtificiallyBlind(Pawn p)
		{
			if (PawnUtility.IsBiologicallyBlind(p))
			{
				return false;
			}
			if (p.apparel != null)
			{
				using (List<Apparel>.Enumerator enumerator = p.apparel.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.blocksVision)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600C630 RID: 50736 RVA: 0x0039B914 File Offset: 0x00399B14
		public static bool IsWorkTypeDisabledByAge(this Pawn pawn, WorkTypeDef workType, out int minAgeRequired)
		{
			for (int i = 0; i < pawn.RaceProps.lifeStageWorkSettings.Count; i++)
			{
				LifeStageWorkSettings lifeStageWorkSettings = pawn.RaceProps.lifeStageWorkSettings[i];
				if (lifeStageWorkSettings.workType == workType && lifeStageWorkSettings.IsDisabled(pawn))
				{
					minAgeRequired = lifeStageWorkSettings.minAge;
					return true;
				}
			}
			minAgeRequired = 0;
			return false;
		}

		// Token: 0x0600C631 RID: 50737 RVA: 0x0039B970 File Offset: 0x00399B70
		public static bool DutyActiveWhenDown(this Pawn pawn, bool onlyInBed = false)
		{
			if (onlyInBed && !pawn.InBed())
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			bool? flag;
			if (lord == null)
			{
				flag = null;
			}
			else
			{
				LordJob lordJob = lord.LordJob;
				flag = ((lordJob != null) ? new bool?(lordJob.DutyActiveWhenDown(pawn)) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x0600C632 RID: 50738 RVA: 0x0039B9C6 File Offset: 0x00399BC6
		public static bool IsPotentiallyPredator(this Pawn pawn)
		{
			return pawn.RaceProps.predator || (pawn.IsWildMan() && !pawn.IsPrisoner && !pawn.WorkTypeIsDisabled(WorkTypeDefOf.Hunting));
		}

		// Token: 0x0600C633 RID: 50739 RVA: 0x0039B9F8 File Offset: 0x00399BF8
		public static bool IsExitingMap(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && (lord.LordJob is LordJob_ExitMapBest || lord.LordJob is LordJob_ExitMapNear || lord.LordJob is LordJob_ExitOnShuttle || lord.LordJob is LordJob_TravelAndExit);
		}

		// Token: 0x0600C634 RID: 50740 RVA: 0x0039BA48 File Offset: 0x00399C48
		public static void ForceEjectFromContainer(Pawn pawn)
		{
			Pawn_CarryTracker pawn_CarryTracker = pawn.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Thing thing;
				pawn_CarryTracker.TryDropCarriedThing(pawn_CarryTracker.pawn.Position, ThingPlaceMode.Near, out thing, null);
				pawn_CarryTracker.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			Building_Enterable building_Enterable = pawn.ParentHolder as Building_Enterable;
			if (building_Enterable != null)
			{
				Thing thing;
				building_Enterable.innerContainer.TryDrop(pawn, building_Enterable.InteractionCell, building_Enterable.Map, ThingPlaceMode.Near, out thing, null, null, true);
			}
			Building_Casket building_Casket = pawn.ParentHolder as Building_Casket;
			if (building_Casket != null)
			{
				building_Casket.EjectContents();
			}
			Building_HoldingPlatform building_HoldingPlatform = pawn.ParentHolder as Building_HoldingPlatform;
			if (building_HoldingPlatform != null)
			{
				building_HoldingPlatform.EjectContents();
			}
			CompBiosculpterPod compBiosculpterPod = pawn.ParentHolder as CompBiosculpterPod;
			if (compBiosculpterPod != null)
			{
				compBiosculpterPod.EjectContents(true, true, null);
			}
		}

		// Token: 0x040086F3 RID: 34547
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040086F4 RID: 34548
		private static List<string> tmpPawnKindsStr = new List<string>();

		// Token: 0x040086F5 RID: 34549
		private static HashSet<PawnKindDef> tmpAddedPawnKinds = new HashSet<PawnKindDef>();

		// Token: 0x040086F6 RID: 34550
		private static List<PawnKindDef> tmpPawnKinds = new List<PawnKindDef>();

		// Token: 0x040086F7 RID: 34551
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
