using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200176D RID: 5997
	public static class RestUtility
	{
		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x06008CB7 RID: 36023 RVA: 0x0029D89C File Offset: 0x0029BA9C
		public static List<ThingDef> AllBedDefBestToWorst
		{
			get
			{
				return RestUtility.bedDefsBestToWorst_RestEffectiveness;
			}
		}

		// Token: 0x06008CB8 RID: 36024 RVA: 0x0029D8A4 File Offset: 0x0029BAA4
		public static void Reset()
		{
			RestUtility.bedDefsBestToWorst_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_SlabBed_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			select d).OrderBy(delegate(ThingDef d)
			{
				if (!d.building.bed_slabBed)
				{
					return 1;
				}
				return 0;
			}).ThenBy((ThingDef d) => d.building.bed_maxBodySize).ThenByDescending((ThingDef d) => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null)).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_SlabBed_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			select d).OrderBy(delegate(ThingDef d)
			{
				if (!d.building.bed_slabBed)
				{
					return 1;
				}
				return 0;
			}).ThenBy((ThingDef d) => d.building.bed_maxBodySize).ThenByDescending((ThingDef d) => d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null)).ThenByDescending((ThingDef d) => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null)).ToList<ThingDef>();
		}

		// Token: 0x06008CB9 RID: 36025 RVA: 0x0029DB30 File Offset: 0x0029BD30
		public static bool BedOwnerWillShare(Building_Bed bed, Pawn sleeper, GuestStatus? guestStatus)
		{
			if (!bed.OwnersForReading.Any<Pawn>())
			{
				return true;
			}
			if (!sleeper.IsPrisoner)
			{
				GuestStatus? guestStatus2 = guestStatus;
				GuestStatus guestStatus3 = GuestStatus.Prisoner;
				if (!(guestStatus2.GetValueOrDefault() == guestStatus3 & guestStatus2 != null) && !sleeper.IsSlave)
				{
					guestStatus2 = guestStatus;
					guestStatus3 = GuestStatus.Slave;
					if (!(guestStatus2.GetValueOrDefault() == guestStatus3 & guestStatus2 != null))
					{
						if (!bed.AnyUnownedSleepingSlot)
						{
							return false;
						}
						if (!RestUtility.IsAnyOwnerLovePartnerOf(bed, sleeper))
						{
							return false;
						}
						return true;
					}
				}
			}
			if (!bed.AnyUnownedSleepingSlot)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06008CBA RID: 36026 RVA: 0x0029DBAC File Offset: 0x0029BDAC
		public static bool CanUseBedNow(Thing bedThing, Pawn sleeper, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, GuestStatus? guestStatusOverride = null)
		{
			Building_Bed building_Bed = bedThing as Building_Bed;
			if (building_Bed == null)
			{
				return false;
			}
			if (!building_Bed.Spawned)
			{
				return false;
			}
			if (building_Bed.Map != sleeper.MapHeld)
			{
				return false;
			}
			if (building_Bed.IsBurning())
			{
				return false;
			}
			if (sleeper.HarmedByVacuum && building_Bed.Position.GetVacuum(bedThing.Map) >= 0.5f)
			{
				return false;
			}
			if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
			{
				return false;
			}
			if (building_Bed.CompAssignableToPawn.IdeoligionForbids(sleeper))
			{
				return false;
			}
			int? num;
			bool flag = building_Bed.IsOwner(sleeper, out num);
			int? num2;
			bool flag2 = sleeper.CurrentBed(out num2) == building_Bed;
			if (!building_Bed.AnyUnoccupiedSleepingSlot && !flag && !flag2)
			{
				return false;
			}
			GuestStatus? guestStatus = guestStatusOverride;
			GuestStatus? guestStatus2 = guestStatus = ((guestStatus != null) ? guestStatus : sleeper.GuestStatus);
			GuestStatus guestStatus3 = GuestStatus.Prisoner;
			bool flag3 = guestStatus.GetValueOrDefault() == guestStatus3 & guestStatus != null;
			guestStatus = guestStatus2;
			guestStatus3 = GuestStatus.Slave;
			bool flag4 = guestStatus.GetValueOrDefault() == guestStatus3 & guestStatus != null;
			if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, flag3, false))
			{
				return false;
			}
			if (building_Bed.ForPrisoners != flag3)
			{
				return false;
			}
			if (building_Bed.ForSlaves != flag4)
			{
				return false;
			}
			if (building_Bed.ForPrisoners && !building_Bed.Position.IsInPrisonCell(building_Bed.Map))
			{
				return false;
			}
			if (building_Bed.Medical)
			{
				if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(sleeper))
				{
					return false;
				}
				if (!HealthAIUtility.ShouldSeekMedicalRest(sleeper))
				{
					return false;
				}
			}
			else
			{
				if (!flag && !RestUtility.BedOwnerWillShare(building_Bed, sleeper, guestStatusOverride))
				{
					return false;
				}
				if (flag2)
				{
					int? num3 = num2;
					int? num4 = num;
					if (!(num3.GetValueOrDefault() == num4.GetValueOrDefault() & num3 != null == (num4 != null)))
					{
						return false;
					}
				}
			}
			if (sleeper.IsColonist && !flag3)
			{
				Job curJob = sleeper.CurJob;
				if ((curJob == null || !curJob.ignoreForbidden) && !sleeper.Downed && building_Bed.IsForbidden(sleeper))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008CBB RID: 36027 RVA: 0x0029DD78 File Offset: 0x0029BF78
		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, bool ignoreOtherReservations = false, GuestStatus? guestStatus = null)
		{
			if (!RestUtility.CanUseBedNow(bedThing, sleeper, checkSocialProperness, allowMedBedEvenIfSetToNoCare, guestStatus))
			{
				return false;
			}
			Building_Bed building_Bed = (Building_Bed)bedThing;
			if (!traveler.CanReach(building_Bed, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			if (!sleeper.HasReserved(building_Bed, null) && !traveler.CanReserve(building_Bed, building_Bed.SleepingSlotsCount, 0, null, ignoreOtherReservations))
			{
				return false;
			}
			if (traveler.HasReserved(building_Bed, new LocalTargetInfo?(sleeper), null, null))
			{
				return false;
			}
			if (building_Bed.IsForbidden(traveler))
			{
				return false;
			}
			GuestStatus? guestStatus2 = guestStatus;
			GuestStatus guestStatus3 = GuestStatus.Prisoner;
			bool flag = guestStatus2.GetValueOrDefault() == guestStatus3 & guestStatus2 != null;
			guestStatus2 = guestStatus;
			guestStatus3 = GuestStatus.Slave;
			bool flag2 = guestStatus2.GetValueOrDefault() == guestStatus3 & guestStatus2 != null;
			return (flag || flag2 || building_Bed.Faction == traveler.Faction || (traveler.HostFaction != null && building_Bed.Faction == traveler.HostFaction)) && (!ModsConfig.AnomalyActive || !sleeper.IsMutant || sleeper.needs.rest != null || !sleeper.mutant.Def.entitledToMedicalCare || building_Bed.Medical);
		}

		// Token: 0x06008CBC RID: 36028 RVA: 0x0029DEAC File Offset: 0x0029C0AC
		public static void TuckIntoBed(Building_Bed bed, Pawn taker, Pawn takee, bool rescued)
		{
			IntVec3 position = bed.Position;
			if (taker != takee)
			{
				Thing thing;
				taker.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, null);
			}
			if (RestUtility.CanUseBedNow(bed, takee, false, false, null))
			{
				takee.jobs.Notify_TuckedIntoBed(bed);
				if (taker != takee && rescued)
				{
					takee.relations.Notify_RescuedBy(taker);
				}
				takee.mindState.Notify_TuckedIntoBed();
			}
			if (takee.IsPrisonerOfColony)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, takee, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06008CBD RID: 36029 RVA: 0x0029DF2C File Offset: 0x0029C12C
		private static bool IsAnyOwnerLovePartnerOf(Building_Bed bed, Pawn sleeper)
		{
			for (int i = 0; i < bed.OwnersForReading.Count; i++)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(sleeper, bed.OwnersForReading[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008CBE RID: 36030 RVA: 0x0029DF66 File Offset: 0x0029C166
		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, true, false, p.GuestStatus);
		}

		// Token: 0x06008CBF RID: 36031 RVA: 0x0029DF78 File Offset: 0x0029C178
		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool checkSocialProperness, bool ignoreOtherReservations = false, GuestStatus? guestStatus = null)
		{
			RestUtility.<>c__DisplayClass14_0 CS$<>8__locals1 = new RestUtility.<>c__DisplayClass14_0();
			CS$<>8__locals1.sleeper = sleeper;
			CS$<>8__locals1.traveler = traveler;
			CS$<>8__locals1.checkSocialProperness = checkSocialProperness;
			CS$<>8__locals1.ignoreOtherReservations = ignoreOtherReservations;
			CS$<>8__locals1.guestStatus = guestStatus;
			if (CS$<>8__locals1.sleeper.RaceProps.IsMechanoid)
			{
				return null;
			}
			if (ModsConfig.BiotechActive && CS$<>8__locals1.sleeper.Deathresting)
			{
				Building_Bed assignedDeathrestCasket = CS$<>8__locals1.sleeper.ownership.AssignedDeathrestCasket;
				if (assignedDeathrestCasket != null && RestUtility.IsValidBedFor(assignedDeathrestCasket, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, true, false, false, null))
				{
					CompDeathrestBindable compDeathrestBindable = assignedDeathrestCasket.TryGetComp<CompDeathrestBindable>();
					if (compDeathrestBindable != null && (compDeathrestBindable.BoundPawn == CS$<>8__locals1.sleeper || compDeathrestBindable.BoundPawn == null))
					{
						return assignedDeathrestCasket;
					}
				}
			}
			bool flag = false;
			if (CS$<>8__locals1.sleeper.Ideo != null)
			{
				using (List<Precept>.Enumerator enumerator = CS$<>8__locals1.sleeper.Ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.prefersSlabBed)
						{
							flag = true;
							break;
						}
					}
				}
			}
			List<ThingDef> list = flag ? RestUtility.bedDefsBestToWorst_SlabBed_Medical : RestUtility.bedDefsBestToWorst_Medical;
			List<ThingDef> list2 = flag ? RestUtility.bedDefsBestToWorst_SlabBed_RestEffectiveness : RestUtility.bedDefsBestToWorst_RestEffectiveness;
			if (HealthAIUtility.ShouldSeekMedicalRest(CS$<>8__locals1.sleeper))
			{
				if (CS$<>8__locals1.sleeper.InBed() && CS$<>8__locals1.sleeper.CurrentBed().Medical && RestUtility.IsValidBedFor(CS$<>8__locals1.sleeper.CurrentBed(), CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
				{
					return CS$<>8__locals1.sleeper.CurrentBed();
				}
				for (int i = 0; i < list.Count; i++)
				{
					ThingDef thingDef = list[i];
					if (RestUtility.CanUseBedEver(CS$<>8__locals1.sleeper, thingDef))
					{
						for (int j = 0; j < 2; j++)
						{
							Danger maxDanger = (j == 0) ? Danger.None : Danger.Deadly;
							Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(CS$<>8__locals1.sleeper.Position, CS$<>8__locals1.sleeper.MapHeld, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing b) => ((Building_Bed)b).Medical && b.Position.GetDangerFor(CS$<>8__locals1.sleeper, CS$<>8__locals1.sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus), null, 0, -1, false, RegionType.Set_Passable, false, false);
							if (building_Bed != null)
							{
								return building_Bed;
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.sleeper.RaceProps.Dryad)
			{
				return null;
			}
			if (CS$<>8__locals1.sleeper.ownership != null && CS$<>8__locals1.sleeper.ownership.OwnedBed != null && RestUtility.IsValidBedFor(CS$<>8__locals1.sleeper.ownership.OwnedBed, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
			{
				return CS$<>8__locals1.sleeper.ownership.OwnedBed;
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(CS$<>8__locals1.sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null && RestUtility.IsValidBedFor(ownedBed, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
				{
					return ownedBed;
				}
			}
			int dg;
			int dg2;
			for (dg = 0; dg < 3; dg = dg2 + 1)
			{
				Danger maxDanger = (dg <= 1) ? Danger.None : Danger.Deadly;
				Predicate<Thing> <>9__1;
				for (int k = 0; k < list2.Count; k++)
				{
					ThingDef thingDef2 = list2[k];
					if (RestUtility.CanUseBedEver(CS$<>8__locals1.sleeper, thingDef2))
					{
						IntVec3 positionHeld = CS$<>8__locals1.sleeper.PositionHeld;
						Map mapHeld = CS$<>8__locals1.sleeper.MapHeld;
						ThingRequest thingReq = ThingRequest.ForDef(thingDef2);
						PathEndMode peMode = PathEndMode.OnCell;
						TraverseParms traverseParams = TraverseParms.For(CS$<>8__locals1.traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
						float maxDistance = 9999f;
						Predicate<Thing> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = delegate(Thing b)
							{
								if (((Building_Bed)b).Medical || b.Position.GetDangerFor(CS$<>8__locals1.sleeper, CS$<>8__locals1.sleeper.MapHeld) > maxDanger || !RestUtility.IsValidBedFor(b, CS$<>8__locals1.sleeper, CS$<>8__locals1.traveler, CS$<>8__locals1.checkSocialProperness, false, CS$<>8__locals1.ignoreOtherReservations, CS$<>8__locals1.guestStatus))
								{
									return false;
								}
								if (dg <= 0)
								{
									return !b.Position.GetItems(b.Map).Any((Thing thing) => thing.def.IsCorpse);
								}
								return true;
							});
						}
						Building_Bed building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(positionHeld, mapHeld, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
						if (building_Bed2 != null)
						{
							return building_Bed2;
						}
					}
				}
				dg2 = dg;
			}
			return null;
		}

		// Token: 0x06008CC0 RID: 36032 RVA: 0x0029E410 File Offset: 0x0029C610
		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> medBedValidator = delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return building_Bed2 != null && building_Bed2.Medical && RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, false, true, false, pawn.GuestStatus);
			};
			if (pawn.InBed() && medBedValidator(pawn.CurrentBed()))
			{
				return pawn.CurrentBed();
			}
			for (int i = 0; i < 2; i++)
			{
				Danger maxDanger = (i == 0) ? Danger.None : Danger.Deadly;
				Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Bed), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing b) => b.Position.GetDangerFor(pawn, pawn.Map) <= maxDanger && medBedValidator(b), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (building_Bed != null)
				{
					return building_Bed;
				}
			}
			return RestUtility.FindBedFor(pawn);
		}

		// Token: 0x06008CC1 RID: 36033 RVA: 0x0029E50C File Offset: 0x0029C70C
		public static IntVec3 GetBedSleepingSlotPosFor(Pawn pawn, Building_Bed bed)
		{
			int? num;
			if (bed.IsOwner(pawn, out num))
			{
				return bed.GetSleepingSlotPos(num.Value);
			}
			for (int i = 0; i < bed.SleepingSlotsCount; i++)
			{
				Pawn curOccupant = bed.GetCurOccupant(i);
				if ((i >= bed.OwnersForReading.Count || bed.OwnersForReading[i] == null) && curOccupant == pawn)
				{
					return bed.GetSleepingSlotPos(i);
				}
			}
			for (int j = 0; j < bed.SleepingSlotsCount; j++)
			{
				Pawn curOccupant2 = bed.GetCurOccupant(j);
				if ((j >= bed.OwnersForReading.Count || bed.OwnersForReading[j] == null) && curOccupant2 == null)
				{
					return bed.GetSleepingSlotPos(j);
				}
			}
			Log.Error("Could not find good sleeping slot position for " + ((pawn != null) ? pawn.ToString() : null) + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.");
			return bed.GetSleepingSlotPos(0);
		}

		// Token: 0x06008CC2 RID: 36034 RVA: 0x0029E5E0 File Offset: 0x0029C7E0
		public static void KickOutOfBed(Pawn pawn, Building_Bed bed)
		{
			if (pawn == null)
			{
				return;
			}
			if (!pawn.Spawned)
			{
				Log.Error("Tried to kick unspawned pawn " + pawn.ToStringSafe<Pawn>() + " out of bed.");
			}
			if (!pawn.Dead && !pawn.GetPosture().InBed())
			{
				Log.Error("Tried to kick pawn " + pawn.ToStringSafe<Pawn>() + " out of bed when they weren't in bed.");
			}
			int? num;
			Building_Bed building_Bed = pawn.CurrentBed(out num);
			if (building_Bed != bed)
			{
				if (building_Bed == null)
				{
					bed = null;
				}
				else
				{
					Log.Error("Tried to kick pawn " + pawn.ToStringSafe<Pawn>() + " out of a bed they're not currently in.");
				}
			}
			pawn.jobs.posture &= ~PawnPosture.InBedMask;
			if (bed != null && (pawn.Downed || pawn.Deathresting))
			{
				pawn.Position = bed.GetFootSlotPos(num.Value);
			}
		}

		// Token: 0x06008CC3 RID: 36035 RVA: 0x0029E6B0 File Offset: 0x0029C8B0
		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			return !p.RaceProps.IsMechanoid && p.BodySize <= bedDef.building.bed_maxBodySize && p.RaceProps.Humanlike == bedDef.building.bed_humanlike && (!ModsConfig.BiotechActive || bedDef != ThingDefOf.DeathrestCasket || p.CanDeathrest());
		}

		// Token: 0x06008CC4 RID: 36036 RVA: 0x0029E718 File Offset: 0x0029C918
		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			Pawn_TimetableTracker timetable = pawn.timetable;
			if (((timetable != null) ? timetable.CurrentAssignment : null) != null && !pawn.timetable.CurrentAssignment.allowRest)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (((needs != null) ? needs.rest : null) != null && pawn.needs.rest.CurLevel >= 0.2f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008CC5 RID: 36037 RVA: 0x0029E779 File Offset: 0x0029C979
		public static bool DisturbancePreventsLyingDown(Pawn pawn)
		{
			return !pawn.Downed && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400;
		}

		// Token: 0x06008CC6 RID: 36038 RVA: 0x0029E7A2 File Offset: 0x0029C9A2
		public static bool Awake(this Pawn p)
		{
			return p.health.capacities.CanBeAwake && (p.CurJob == null || p.jobs.curDriver == null || !p.jobs.curDriver.asleep);
		}

		// Token: 0x06008CC7 RID: 36039 RVA: 0x0029E7E2 File Offset: 0x0029C9E2
		public static bool IsSelfShutdown(this Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			return ((needs != null) ? needs.energy : null) != null && p.needs.energy.IsSelfShutdown;
		}

		// Token: 0x06008CC8 RID: 36040 RVA: 0x0029E80C File Offset: 0x0029CA0C
		public static bool IsDeactivated(this Pawn p)
		{
			CompMechanoid compMechanoid = p.TryGetComp<CompMechanoid>();
			return compMechanoid != null && compMechanoid.Deactivated;
		}

		// Token: 0x06008CC9 RID: 36041 RVA: 0x0029E82C File Offset: 0x0029CA2C
		public static bool IsActivityDormant(this Pawn p)
		{
			CompActivity activity = p.activity;
			return activity != null && activity.IsActive;
		}

		// Token: 0x06008CCA RID: 36042 RVA: 0x0029E84B File Offset: 0x0029CA4B
		public static bool IsCharging(this Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			object obj;
			if (needs == null)
			{
				obj = null;
			}
			else
			{
				Need_MechEnergy energy = needs.energy;
				obj = ((energy != null) ? energy.currentCharger : null);
			}
			return obj != null;
		}

		// Token: 0x06008CCB RID: 36043 RVA: 0x0029E870 File Offset: 0x0029CA70
		public static Building_Bed CurrentBed(this Pawn p)
		{
			int? num;
			return p.CurrentBed(out num);
		}

		// Token: 0x06008CCC RID: 36044 RVA: 0x0029E888 File Offset: 0x0029CA88
		public static Building_Bed CurrentBed(this Pawn p, out int? sleepingSlot)
		{
			sleepingSlot = null;
			if (!p.Spawned || p.CurJob == null || !p.GetPosture().InBed())
			{
				return null;
			}
			Building_Bed building_Bed = null;
			List<Thing> thingList = p.Position.GetThingList(p.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				building_Bed = (thingList[i] as Building_Bed);
				if (building_Bed != null)
				{
					break;
				}
			}
			if (building_Bed == null)
			{
				return null;
			}
			for (int j = 0; j < building_Bed.SleepingSlotsCount; j++)
			{
				if (building_Bed.GetCurOccupant(j) == p)
				{
					sleepingSlot = new int?(j);
					return building_Bed;
				}
			}
			return null;
		}

		// Token: 0x06008CCD RID: 36045 RVA: 0x0029E91F File Offset: 0x0029CB1F
		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		// Token: 0x06008CCE RID: 36046 RVA: 0x0029E92A File Offset: 0x0029CB2A
		public static bool IsLayingForJobCleanup(Pawn p)
		{
			return p.InBed() || (p.CurJob != null && p.CurJob.def == JobDefOf.LayDown && p.GetPosture().Laying());
		}

		// Token: 0x06008CCF RID: 36047 RVA: 0x0029E960 File Offset: 0x0029CB60
		public static void WakeUp(Pawn p, bool startNewJob = true)
		{
			if (p.CurJob != null && (p.GetPosture().Laying() || p.CurJobDef == JobDefOf.LayDown) && !p.Downed)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, startNewJob, true);
			}
			CompCanBeDormant comp = p.GetComp<CompCanBeDormant>();
			if (comp != null)
			{
				comp.WakeUp();
			}
			if (p.mindState != null)
			{
				p.mindState.hibernationEndedTick = GenTicks.TicksGame;
			}
		}

		// Token: 0x06008CD0 RID: 36048 RVA: 0x0029E9D0 File Offset: 0x0029CBD0
		public static bool ShouldWakeUp(Pawn pawn)
		{
			if (pawn.Deathresting)
			{
				return false;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			return ((needs != null) ? needs.rest : null) == null || pawn.needs.rest.CurLevel >= RestUtility.WakeThreshold(pawn) || pawn.health.hediffSet.HasHediffBlocksSleeping();
		}

		// Token: 0x06008CD1 RID: 36049 RVA: 0x0029EA28 File Offset: 0x0029CC28
		public static bool CanFallAsleep(Pawn pawn)
		{
			if (pawn.Deathresting)
			{
				return true;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			bool flag;
			if (needs == null)
			{
				flag = false;
			}
			else
			{
				Need_Food food = needs.food;
				bool? flag2 = (food != null) ? new bool?(food.Starving) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag && !pawn.ageTracker.CurLifeStage.canSleepWhenStarving)
			{
				return false;
			}
			if (pawn.mindState != null && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400)
			{
				return false;
			}
			Pawn_NeedsTracker needs2 = pawn.needs;
			float? num;
			if (needs2 == null)
			{
				num = null;
			}
			else
			{
				Need_Rest rest = needs2.rest;
				num = ((rest != null) ? new float?(rest.CurLevel) : null);
			}
			float? num2 = num;
			float num3 = RestUtility.FallAsleepMaxLevel(pawn);
			if ((num2.GetValueOrDefault() < num3 & num2 != null) && !pawn.health.hediffSet.HasHediffBlocksSleeping())
			{
				JobDef curJobDef = pawn.CurJobDef;
				return curJobDef == null || curJobDef.sleepCanInterrupt;
			}
			return false;
		}

		// Token: 0x06008CD2 RID: 36050 RVA: 0x0029EB34 File Offset: 0x0029CD34
		private static float WakeThreshold(Pawn p)
		{
			Lord lord = p.GetLord();
			if (lord != null && lord.CurLordToil != null && lord.CurLordToil.CustomWakeThreshold != null)
			{
				return lord.CurLordToil.CustomWakeThreshold.Value;
			}
			LifeStageDef curLifeStage = p.ageTracker.CurLifeStage;
			float? num = (curLifeStage != null) ? curLifeStage.naturalWakeThresholdOverride : null;
			if (num == null)
			{
				return 1f;
			}
			return num.GetValueOrDefault();
		}

		// Token: 0x06008CD3 RID: 36051 RVA: 0x0029EBB4 File Offset: 0x0029CDB4
		private static float FallAsleepMaxLevel(Pawn p)
		{
			LifeStageDef curLifeStage = p.ageTracker.CurLifeStage;
			return Mathf.Min(((curLifeStage != null) ? curLifeStage.fallAsleepMaxThresholdOverride : null) ?? 0.75f, RestUtility.WakeThreshold(p) - 0.01f);
		}

		// Token: 0x04005B70 RID: 23408
		public const int NoSleepingDurationAfterBeingDisturbed = 400;

		// Token: 0x04005B71 RID: 23409
		private static List<ThingDef> bedDefsBestToWorst_RestEffectiveness;

		// Token: 0x04005B72 RID: 23410
		private static List<ThingDef> bedDefsBestToWorst_Medical;

		// Token: 0x04005B73 RID: 23411
		private static List<ThingDef> bedDefsBestToWorst_SlabBed_RestEffectiveness;

		// Token: 0x04005B74 RID: 23412
		private static List<ThingDef> bedDefsBestToWorst_SlabBed_Medical;
	}
}
