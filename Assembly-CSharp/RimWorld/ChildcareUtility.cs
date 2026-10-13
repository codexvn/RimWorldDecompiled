using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001148 RID: 4424
	public static class ChildcareUtility
	{
		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06006C01 RID: 27649 RVA: 0x0021EFF8 File Offset: 0x0021D1F8
		public static List<Pawn> CanBreastfeedPlayerPawns
		{
			get
			{
				int ticksGame = Find.TickManager.TicksGame;
				if (ChildcareUtility.canBreastfeedCacheGameTicks < ticksGame)
				{
					ChildcareUtility.canBreastfeedPlayerPawns.Clear();
					foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
					{
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanBreastfeed(pawn, out breastfeedFailReason))
						{
							ChildcareUtility.canBreastfeedPlayerPawns.Add(pawn);
						}
					}
					ChildcareUtility.canBreastfeedCacheGameTicks = ticksGame;
				}
				return ChildcareUtility.canBreastfeedPlayerPawns;
			}
		}

		// Token: 0x06006C02 RID: 27650 RVA: 0x0021F080 File Offset: 0x0021D280
		public static List<Pawn> SpawnedColonistChildcarers(Map map)
		{
			ChildcareUtility.childcarers.Clear();
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn.WorkTypeIsDisabled(WorkTypeDefOf.Childcare))
				{
					ChildcareUtility.childcarers.Add(pawn);
				}
			}
			return ChildcareUtility.childcarers;
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x0021F0F8 File Offset: 0x0021D2F8
		public static TaggedString Translate(this ChildcareUtility.BreastfeedFailReason reason, Pawn hauler = null, Pawn mom = null, Pawn baby = null)
		{
			NamedArgument arg = hauler.Named("HAULER");
			NamedArgument arg2 = mom.Named("MOM");
			NamedArgument arg3 = baby.Named("BABY");
			string key;
			switch (reason)
			{
			case ChildcareUtility.BreastfeedFailReason.MomNotEnoughMilk:
				key = "BreastfeedFailReason_MomNotEnoughMilk";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomInMentalState:
				key = "BreastfeedFailReason_MomInMentalState";
				break;
			case ChildcareUtility.BreastfeedFailReason.BabyTooFull:
				key = "BreastfeedFailReason_BabyTooFull";
				break;
			case ChildcareUtility.BreastfeedFailReason.BabyForbiddenToMom:
				key = "BreastfeedFailReason_BabyForbiddenToMom";
				break;
			case ChildcareUtility.BreastfeedFailReason.BabyForbiddenToHauler:
				key = "BreastfeedFailReason_BabyForbiddenToHauler";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomForbiddenToHauler:
				key = "BreastfeedFailReason_MomForbiddenToHauler";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomIsMobile:
				key = "BreastfeedFailReason_MomIsMobile";
				break;
			case ChildcareUtility.BreastfeedFailReason.HaulerCannotReachBaby:
				key = "BreastfeedFailReason_HaulerCannotReachBaby";
				break;
			case ChildcareUtility.BreastfeedFailReason.HaulerCannotReachMom:
				key = "BreastfeedFailReason_HaulerCannotReachMom";
				break;
			case ChildcareUtility.BreastfeedFailReason.NoMomsAssignedToBaby:
				key = "BreastfeedFailReason_NoMomsAssignedToBaby";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomNotAssignedToChildcare:
				key = "BreastfeedFailReason_MomNotAssignedToChildcare";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomInsideContainer:
				key = "BreastfeedFailReason_MomInsideContainer";
				break;
			case ChildcareUtility.BreastfeedFailReason.MomBeingCarried:
				key = "BreastfeedFailReason_MomBeingCarried";
				break;
			default:
				Log.ErrorOnce(string.Format("BreastfeedFailReason {0} should not be player facing.", reason), (int)(784668349 + reason));
				return reason.ToString();
			}
			return key.Translate(arg, arg2, arg3);
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x0021F20C File Offset: 0x0021D40C
		public static bool CanFeed(Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (mom == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNull);
			}
			else if (mom.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomDead);
			}
			else if (!mom.RaceProps.Humanlike)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotHumanLike);
			}
			return reason == null;
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x0021F26C File Offset: 0x0021D46C
		public static bool CanBreastfeed(Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (!ChildcareUtility.CanFeed(mom, out reason))
			{
				return false;
			}
			if (!ModsConfig.BiotechActive)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.NoBiotechMod);
			}
			else if (!mom.health.hediffSet.HasHediff(HediffDefOf.Lactating, false))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotLactating);
			}
			return reason == null;
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x0021F2D0 File Offset: 0x0021D4D0
		public static bool CanBreastfeedNow(Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanBreastfeed(mom, out reason))
			{
				return false;
			}
			Hediff firstHediffOfDef = mom.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
			bool flag;
			if (firstHediffOfDef == null)
			{
				flag = true;
			}
			else
			{
				HediffComp_Chargeable hediffComp_Chargeable = firstHediffOfDef.TryGetComp<HediffComp_Chargeable>();
				bool? flag2 = (hediffComp_Chargeable != null) ? new bool?(hediffComp_Chargeable.CanActivate) : null;
				bool flag3 = true;
				flag = !(flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotEnoughMilk);
			}
			else if (mom.InMentalState && !mom.Downed)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomInMentalState);
			}
			return reason == null;
		}

		// Token: 0x06006C07 RID: 27655 RVA: 0x0021F370 File Offset: 0x0021D570
		public static bool CanSuckle(Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (!ModsConfig.BiotechActive)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.NoBiotechMod);
			}
			else if (baby == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNull);
			}
			else if (baby.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyDead);
			}
			else if (!baby.RaceProps.Humanlike)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNotHumanlike);
			}
			else if (!baby.DevelopmentalStage.Baby())
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyTooOld);
			}
			else if (baby.IsShambler)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyShambler);
			}
			return reason == null;
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x0021F419 File Offset: 0x0021D619
		public static bool WantsSuckle(Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanSuckle(baby, out reason))
			{
				return false;
			}
			if (!FeedPatientUtility.IsHungry(baby))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNotHungry);
			}
			return reason == null;
		}

		// Token: 0x06006C09 RID: 27657 RVA: 0x0021F444 File Offset: 0x0021D644
		public static bool CanSuckleNow(Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanSuckle(baby, out reason))
			{
				return false;
			}
			Pawn_NeedsTracker needs = baby.needs;
			if (((needs != null) ? needs.food : null) != null && baby.needs.food.CurLevelPercentage >= 0.66f)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyTooFull);
			}
			return reason == null;
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x0021F49C File Offset: 0x0021D69C
		public static bool CanFeedBaby(Pawn feeder, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanFeed(feeder, out reason))
			{
				return false;
			}
			if (!ChildcareUtility.CanSuckle(baby, out reason))
			{
				return false;
			}
			if (feeder.MapHeld != null && baby.IsForbidden(feeder))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyForbiddenToMom);
			}
			else if (!ChildcareUtility.HasBreastfeedCompatibleFactions(feeder, baby))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyInIncompatibleFactionToMom);
			}
			else
			{
				reason = null;
			}
			return reason == null;
		}

		// Token: 0x06006C0B RID: 27659 RVA: 0x0021F506 File Offset: 0x0021D706
		public static bool CanMomBreastfeedBaby(Pawn mom, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanBreastfeed(mom, out reason) && ChildcareUtility.CanSuckle(baby, out reason) && ChildcareUtility.CanFeedBaby(mom, baby, out reason) && reason == null;
		}

		// Token: 0x06006C0C RID: 27660 RVA: 0x0021F533 File Offset: 0x0021D733
		public static bool CanMomBreastfeedBabyNow(Pawn mom, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanBreastfeedNow(mom, out reason) && ChildcareUtility.CanSuckleNow(baby, out reason) && ChildcareUtility.CanMomBreastfeedBaby(mom, baby, out reason) && reason == null;
		}

		// Token: 0x06006C0D RID: 27661 RVA: 0x0021F560 File Offset: 0x0021D760
		public static bool CanHaulBaby(Pawn hauler, Pawn baby, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (hauler == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNull);
			}
			else if (baby == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNull);
			}
			else if (hauler.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDead);
			}
			else if (hauler.Downed)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDowned);
			}
			else if (hauler.Map == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnMap);
			}
			else if (baby.MapHeld == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyNotOnMap);
			}
			else if (hauler.Map != baby.MapHeld)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnBabyMap);
			}
			else if (baby.IsForbidden(hauler))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyForbiddenToHauler);
			}
			else if (!ChildcareUtility.HasBreastfeedCompatibleFactions(hauler, baby))
			{
				if (!ChildcareUtility.BabyHasFeederInCompatibleFaction(hauler.Faction, baby))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.BabyInIncompatibleFactionToHauler);
				}
			}
			else if (!hauler.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerIncapableOfManipulation);
			}
			return reason == null;
		}

		// Token: 0x06006C0E RID: 27662 RVA: 0x0021F68C File Offset: 0x0021D88C
		public static bool CanHaulBabyNow(Pawn hauler, Pawn baby, bool ignoreOtherReservations, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanHaulBaby(hauler, baby, out reason))
			{
				return false;
			}
			if (!hauler.CanReserve(baby, 1, -1, null, ignoreOtherReservations))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReserveBaby);
			}
			else if (!hauler.CanReach(baby, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReachBaby);
			}
			return reason == null;
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x0021F6F0 File Offset: 0x0021D8F0
		public static bool CanHaulToMom(Pawn hauler, Pawn mom, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			reason = null;
			if (hauler == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNull);
			}
			else if (mom == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNull);
			}
			else if (hauler.Dead)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDead);
			}
			else if (hauler.Downed)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerDowned);
			}
			else if (mom.MapHeld == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotOnMap);
			}
			else if (hauler.Map == null)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnMap);
			}
			else if (hauler.Map != mom.MapHeld)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerNotOnMomMap);
			}
			else if (hauler != mom)
			{
				if (mom.IsForbidden(hauler))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomForbiddenToHauler);
				}
				else if (!ChildcareUtility.HasBreastfeedCompatibleFactions(hauler, mom))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomInIncompatibleFactionToHauler);
				}
			}
			return reason == null;
		}

		// Token: 0x06006C10 RID: 27664 RVA: 0x0021F7E4 File Offset: 0x0021D9E4
		public static bool CanHaulBabyToMomNow(Pawn hauler, Pawn mom, Pawn baby, bool ignoreOtherReservations, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanHaulBabyNow(hauler, baby, ignoreOtherReservations, out reason))
			{
				return false;
			}
			if (!ChildcareUtility.CanHaulToMom(hauler, mom, out reason))
			{
				return false;
			}
			if (hauler != mom)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (!mom.Downed && !mom.IsPrisoner && ChildcareUtility.CanHaulBabyToMomNow(mom, mom, baby, ignoreOtherReservations, out breastfeedFailReason))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomIsMobile);
				}
				else if (!hauler.CanReserve(mom, 1, -1, null, ignoreOtherReservations))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReserveMom);
				}
				else if (!mom.Spawned)
				{
					if (mom.SpawnedParentOrMe is Pawn)
					{
						reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomBeingCarried);
					}
					else
					{
						reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomInsideContainer);
					}
				}
				else if (!hauler.Map.reachability.CanReach(baby.Position, mom.Position, PathEndMode.Touch, TraverseParms.For(hauler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true)))
				{
					reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.HaulerCannotReachMom);
				}
			}
			return reason == null;
		}

		// Token: 0x06006C11 RID: 27665 RVA: 0x0021F8E0 File Offset: 0x0021DAE0
		public static bool CanAutoBreastfeed(Pawn mom, Pawn baby, bool forced, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.CanMomBreastfeedBaby(mom, baby, out reason))
			{
				return false;
			}
			AutofeedMode autofeedMode = baby.mindState.AutofeedSetting(mom);
			if (autofeedMode == AutofeedMode.Urgent)
			{
				reason = null;
			}
			else if (!mom.workSettings.WorkIsActive(WorkTypeDefOf.Childcare))
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotAssignedToChildcare);
			}
			else if (!forced && autofeedMode == AutofeedMode.Never)
			{
				reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.MomNotAssignedToBaby);
			}
			return reason == null;
		}

		// Token: 0x06006C12 RID: 27666 RVA: 0x0021F950 File Offset: 0x0021DB50
		public static bool CanMomAutoBreastfeedBabyNow(Pawn mother, Pawn baby, bool forced, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanMomBreastfeedBabyNow(mother, baby, out reason) && ChildcareUtility.CanAutoBreastfeed(mother, baby, forced, out reason) && ChildcareUtility.CanHaulBabyToMomNow(mother, mother, baby, forced, out reason);
		}

		// Token: 0x06006C13 RID: 27667 RVA: 0x0021F97A File Offset: 0x0021DB7A
		public static bool CanHaulBabyToDownedMomToBreastfeedNow(Pawn hauler, Pawn mother, Pawn baby, bool forced, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			return ChildcareUtility.CanMomBreastfeedBabyNow(mother, baby, out reason) && ChildcareUtility.CanAutoBreastfeed(mother, baby, forced, out reason) && ChildcareUtility.CanHaulBabyToMomNow(hauler, mother, baby, forced, out reason);
		}

		// Token: 0x06006C14 RID: 27668 RVA: 0x0021F9A7 File Offset: 0x0021DBA7
		public static IEnumerable<Pawn> CanBreastfeedMothers(Pawn baby, bool includeAutoBreastfeedOff = false)
		{
			if (!baby.SpawnedOrAnyParentSpawned)
			{
				yield break;
			}
			ChildcareUtility.tmpAutoBreastfeedOpportunisticMoms.Clear();
			ChildcareUtility.tmpAutoBreastfeedOffMoms.Clear();
			foreach (Pawn pawn in baby.MapHeld.mapPawns.FreeColonistsAndPrisoners)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (ChildcareUtility.CanMomBreastfeedBaby(pawn, baby, out breastfeedFailReason))
				{
					switch (baby.mindState.AutofeedSetting(pawn))
					{
					case AutofeedMode.Never:
						ChildcareUtility.tmpAutoBreastfeedOffMoms.Add(pawn);
						break;
					case AutofeedMode.Childcare:
						ChildcareUtility.tmpAutoBreastfeedOpportunisticMoms.Add(pawn);
						break;
					case AutofeedMode.Urgent:
						yield return pawn;
						break;
					}
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			foreach (Pawn pawn2 in ChildcareUtility.tmpAutoBreastfeedOpportunisticMoms)
			{
				yield return pawn2;
			}
			enumerator = default(List<Pawn>.Enumerator);
			if (includeAutoBreastfeedOff)
			{
				foreach (Pawn pawn3 in ChildcareUtility.tmpAutoBreastfeedOffMoms)
				{
					yield return pawn3;
				}
				enumerator = default(List<Pawn>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06006C15 RID: 27669 RVA: 0x0021F9BE File Offset: 0x0021DBBE
		public static Job MakeBringBabyToSafetyJob(Pawn hauler, Pawn baby)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BringBabyToSafety, baby);
			job.count = 1;
			return job;
		}

		// Token: 0x06006C16 RID: 27670 RVA: 0x0021F9D7 File Offset: 0x0021DBD7
		public static Job MakeBreastfeedCarryToMomJob(Pawn baby, Pawn mom)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BreastfeedCarryToMom, baby, mom);
			job.count = 1;
			return job;
		}

		// Token: 0x06006C17 RID: 27671 RVA: 0x0021F9F6 File Offset: 0x0021DBF6
		public static Job MakeBreastfeedJob(Pawn baby, Building_Bed bedWithDownedMom = null)
		{
			Job job = (bedWithDownedMom == null) ? JobMaker.MakeJob(JobDefOf.Breastfeed, baby) : JobMaker.MakeJob(JobDefOf.Breastfeed, baby, bedWithDownedMom);
			job.count = 1;
			return job;
		}

		// Token: 0x06006C18 RID: 27672 RVA: 0x0021FA2A File Offset: 0x0021DC2A
		public static Job MakeBottlefeedJob(Pawn baby, Thing foodSource)
		{
			if (foodSource is Pawn)
			{
				Log.Error("Cannot use a pawn as a food source for bottlefeeding a baby.  Use MakeBreastfeedJob instead.");
			}
			Job job = JobMaker.MakeJob(JobDefOf.BottleFeedBaby);
			job.targetA = baby;
			job.targetB = foodSource;
			job.count = 1;
			return job;
		}

		// Token: 0x06006C19 RID: 27673 RVA: 0x0021FA68 File Offset: 0x0021DC68
		public static Job MakeAutofeedBabyJob(Pawn feeder, Pawn baby, Thing foodSource)
		{
			if (foodSource == null)
			{
				Log.Error("Cannot MakeFeedBabyJob with null foodSource.");
				return null;
			}
			Pawn pawn = foodSource as Pawn;
			if (pawn == null)
			{
				return ChildcareUtility.MakeBottlefeedJob(baby, foodSource);
			}
			if (feeder == pawn)
			{
				return ChildcareUtility.MakeBreastfeedJob(baby, null);
			}
			return ChildcareUtility.MakeBreastfeedCarryToMomJob(baby, pawn);
		}

		// Token: 0x06006C1A RID: 27674 RVA: 0x0021FAA9 File Offset: 0x0021DCA9
		public static Job MakeBabySuckleJob(Pawn feeder)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BabySuckle, feeder);
			job.count = 1;
			return job;
		}

		// Token: 0x06006C1B RID: 27675 RVA: 0x0021FAC2 File Offset: 0x0021DCC2
		public static Job MakeBabyPlayJob(Pawn feeder)
		{
			Job job = JobMaker.MakeJob(JobDefOf.BabyPlay, feeder);
			job.count = 1;
			return job;
		}

		// Token: 0x06006C1C RID: 27676 RVA: 0x0021FADC File Offset: 0x0021DCDC
		public static Toil MakeBabyPlayAsLongAsToilIsActive(Toil toil, TargetIndex babyIndex)
		{
			toil.AddPreInitAction(delegate
			{
				((Pawn)toil.actor.jobs.curJob.GetTarget(babyIndex).Thing).jobs.StartJob(ChildcareUtility.MakeBabyPlayJob(toil.actor), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			});
			toil.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(babyIndex).Thing;
				if (pawn.CurJobDef == JobDefOf.BabyPlay)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			});
			return toil;
		}

		// Token: 0x06006C1D RID: 27677 RVA: 0x0021FB31 File Offset: 0x0021DD31
		public static bool HasBreastfeedCompatibleFactions(Faction faction, Pawn baby)
		{
			return baby.Faction == faction || (faction != null && baby.HostFaction == faction);
		}

		// Token: 0x06006C1E RID: 27678 RVA: 0x0021FB50 File Offset: 0x0021DD50
		public static bool BabyHasFeederInCompatibleFaction(Faction faction, Pawn baby)
		{
			foreach (Pawn pawn in ChildcareUtility.CanBreastfeedMothers(baby, false))
			{
				if (pawn.Faction == faction)
				{
					return true;
				}
				if (faction != null && pawn.HostFaction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006C1F RID: 27679 RVA: 0x0021FBB8 File Offset: 0x0021DDB8
		public static bool HasBreastfeedCompatibleFactions(Pawn mom, Pawn baby)
		{
			return ChildcareUtility.HasBreastfeedCompatibleFactions(mom.Faction, baby);
		}

		// Token: 0x06006C20 RID: 27680 RVA: 0x0021FBC8 File Offset: 0x0021DDC8
		public static bool ImmobileBreastfeederAvailable(Pawn hauler, Pawn baby, bool forced, out Pawn feeder, out ChildcareUtility.BreastfeedFailReason? reason)
		{
			if (!ChildcareUtility.cachedHaulerBabyMotherImmobile.ContainsKey(hauler))
			{
				ChildcareUtility.cachedHaulerBabyMotherImmobile[hauler] = new Dictionary<Pawn, Pawn>();
			}
			Pawn pawn;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ChildcareUtility.cachedHaulerBabyMotherImmobile[hauler].TryGetValue(baby, out pawn) && ChildcareUtility.CanHaulBabyToDownedMomToBreastfeedNow(hauler, pawn, baby, forced, out breastfeedFailReason))
			{
				feeder = pawn;
				reason = null;
				return true;
			}
			reason = new ChildcareUtility.BreastfeedFailReason?(ChildcareUtility.BreastfeedFailReason.NoMomsAssignedToBaby);
			feeder = null;
			foreach (Pawn pawn2 in ChildcareUtility.CanBreastfeedMothers(baby, forced))
			{
				if (pawn2 == hauler)
				{
					if (feeder == null)
					{
						reason = null;
						feeder = pawn2;
					}
				}
				else
				{
					if (ChildcareUtility.CanHaulBabyToDownedMomToBreastfeedNow(hauler, pawn2, baby, forced, out reason))
					{
						ChildcareUtility.cachedHaulerBabyMotherImmobile[hauler][baby] = pawn2;
						return true;
					}
					feeder = pawn2;
				}
			}
			return false;
		}

		// Token: 0x06006C21 RID: 27681 RVA: 0x0021FCB0 File Offset: 0x0021DEB0
		public static bool ShouldWakeUpToAutofeedUrgent(Pawn feeder)
		{
			if (feeder.Awake())
			{
				Log.Warning("Should not try check if pawn should wake up to autofeed if pawn already awake.");
				return false;
			}
			if (feeder.mindState.nextSleepingBreastfeedStrictTick > Find.TickManager.TicksGame)
			{
				return false;
			}
			feeder.mindState.nextSleepingBreastfeedStrictTick = Find.TickManager.TicksGame + Pawn_MindState.NextSleepingBreastfeedTickCheck.RandomInRange;
			if (feeder.Downed)
			{
				return false;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanFeed(feeder, out breastfeedFailReason))
			{
				return false;
			}
			Thing thing;
			Pawn pawn = ChildcareUtility.FindAutofeedBaby(feeder, AutofeedMode.Urgent, out thing);
			Pawn pawn2;
			return pawn != null && (thing != null || ChildcareUtility.ImmobileBreastfeederAvailable(feeder, pawn, false, out pawn2, out breastfeedFailReason));
		}

		// Token: 0x06006C22 RID: 27682 RVA: 0x0021FD48 File Offset: 0x0021DF48
		public static Thing FindBabyFoodForBaby(Pawn feeder, Pawn baby)
		{
			Thing result;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(feeder, baby, baby.needs.food.CurCategory == HungerCategory.Starving, out result, out thingDef, false, true, false, false, true, false, false, false, false, false, false, FoodPreferability.RawBad))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06006C23 RID: 27683 RVA: 0x0021FD84 File Offset: 0x0021DF84
		public static Pawn FindAutofeedBaby(Pawn mom, AutofeedMode priorityLevel, out Thing food)
		{
			if (priorityLevel == AutofeedMode.Never)
			{
				Log.Warning("Will never auto breastfeed a baby if priority level is OFF");
				food = null;
				return null;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			bool flag = ChildcareUtility.CanBreastfeedNow(mom, out breastfeedFailReason);
			foreach (Pawn pawn in mom.MapHeld.mapPawns.FreeHumanlikesOfFaction(mom.Faction))
			{
				if (!pawn.Suspended && ChildcareUtility.WantsSuckle(pawn, out breastfeedFailReason) && pawn.mindState.AutofeedSetting(mom) == priorityLevel && ChildcareUtility.CanFeedBaby(mom, pawn, out breastfeedFailReason) && ChildcareUtility.CanHaulBabyToMomNow(mom, mom, pawn, false, out breastfeedFailReason))
				{
					if (flag)
					{
						food = mom;
					}
					else
					{
						food = ChildcareUtility.FindBabyFoodForBaby(mom, pawn);
					}
					return pawn;
				}
			}
			food = null;
			return null;
		}

		// Token: 0x06006C24 RID: 27684 RVA: 0x0021FE54 File Offset: 0x0021E054
		public static bool SuckleFromLactatingPawn(Pawn baby, Pawn feeder, int delta)
		{
			Hediff firstHediffOfDef = feeder.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
			HediffComp_Chargeable hediffComp_Chargeable = firstHediffOfDef.TryGetComp<HediffComp_Chargeable>();
			firstHediffOfDef.Severity = Mathf.Min(1f, firstHediffOfDef.Severity + 0.0001f * (float)delta);
			float nutritionWanted = baby.needs.food.NutritionWanted;
			float num = Mathf.Min(baby.needs.food.MaxLevel / 5000f * (float)delta, nutritionWanted);
			float num2 = hediffComp_Chargeable.GreedyConsume(num);
			baby.needs.food.CurLevel += num2;
			Caravan caravan = baby.GetCaravan();
			if (caravan != null && feeder.GetCaravan() == caravan)
			{
				feeder.mindState.BreastfeedCaravan(baby, num2 / baby.needs.food.MaxLevel);
			}
			Pawn_IdeoTracker ideo = baby.ideo;
			if (ideo != null)
			{
				ideo.IncreaseIdeoExposureIfBabyTick(feeder.Ideo, 1);
			}
			return !Mathf.Approximately(num2, nutritionWanted) && num2 >= num;
		}

		// Token: 0x06006C25 RID: 27685 RVA: 0x0021FF4B File Offset: 0x0021E14B
		public static void ClearCache()
		{
			ChildcareUtility.canBreastfeedCacheGameTicks = -1;
			ChildcareUtility.canBreastfeedPlayerPawns.Clear();
			ChildcareUtility.cachedHaulerBabyMotherImmobile.Clear();
		}

		// Token: 0x06006C26 RID: 27686 RVA: 0x0021FF67 File Offset: 0x0021E167
		public static bool SwaddleBaby(this Pawn baby)
		{
			return ModsConfig.BiotechActive && baby.RaceProps.Humanlike && baby.DevelopmentalStage.Baby() && !baby.Dead && !(baby.ParentHolder is Building_GrowthVat);
		}

		// Token: 0x06006C27 RID: 27687 RVA: 0x0021FFA8 File Offset: 0x0021E1A8
		public static bool BabyNeedsMovingForTemperatureReasons(Pawn baby, Pawn hauler, out Region preferredRegion, IntVec3? positionOverride = null)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(baby, out breastfeedFailReason))
			{
				preferredRegion = null;
				return false;
			}
			if (!ChildcareUtility.CanHaulBabyNow(hauler, baby, false, out breastfeedFailReason))
			{
				preferredRegion = null;
				return false;
			}
			if (Find.TickManager.TicksGame < baby.mindState.lastBroughtToSafeTemperatureTick + 2500)
			{
				preferredRegion = null;
				return false;
			}
			FloatRange tempRange = baby.ComfortableTemperatureRange();
			FloatRange tempRange2 = baby.SafeTemperatureRange();
			IntVec3 root = positionOverride ?? baby.PositionHeld;
			float f;
			if (positionOverride != null)
			{
				f = GenTemperature.GetTemperatureForCell(positionOverride.Value, baby.MapHeld);
			}
			else
			{
				f = baby.AmbientTemperature;
			}
			if (tempRange.Includes(f))
			{
				preferredRegion = null;
				return false;
			}
			Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(root, baby.MapHeld, baby, tempRange, TraverseParms.For(hauler, Danger.Some, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable, -1f);
			if (region != null)
			{
				preferredRegion = region;
				return true;
			}
			if (tempRange2.Includes(f))
			{
				preferredRegion = null;
				return false;
			}
			Region region2 = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(root, baby.MapHeld, baby, tempRange2, TraverseParms.For(hauler, Danger.Some, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable, -1f);
			if (region2 != null)
			{
				preferredRegion = region2;
				return true;
			}
			preferredRegion = null;
			return false;
		}

		// Token: 0x06006C28 RID: 27688 RVA: 0x002200C4 File Offset: 0x0021E2C4
		public static LocalTargetInfo SafePlaceForBaby(Pawn baby, Pawn hauler, bool ignoreOtherReservations = false)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(baby, out breastfeedFailReason))
			{
				return LocalTargetInfo.Invalid;
			}
			if (!ChildcareUtility.CanHaulBabyNow(hauler, baby, ignoreOtherReservations, out breastfeedFailReason))
			{
				return LocalTargetInfo.Invalid;
			}
			Building_Bed building_Bed = baby.CurrentBed() ?? RestUtility.FindBedFor(baby, hauler, true, false, baby.GuestStatus);
			float temperatureForCell = GenTemperature.GetTemperatureForCell((building_Bed != null) ? building_Bed.Position : IntVec3.Invalid, baby.MapHeld);
			if (building_Bed != null && building_Bed.Medical && HealthAIUtility.ShouldSeekMedicalRest(baby))
			{
				return building_Bed;
			}
			if (building_Bed != null && baby.ComfortableTemperatureRange().Includes(temperatureForCell))
			{
				return building_Bed;
			}
			LocalTargetInfo result = LocalTargetInfo.Invalid;
			Region targetRegion;
			if (ChildcareUtility.BabyNeedsMovingForTemperatureReasons(baby, hauler, out targetRegion, null))
			{
				result = RCellFinder.SpotToStandDuringJob(hauler, null, targetRegion);
			}
			else if (baby.Spawned)
			{
				result = baby.Position;
			}
			else
			{
				result = RCellFinder.SpotToStandDuringJob(hauler, null, baby.GetRegionHeld(RegionType.Set_Passable));
			}
			if (result.IsValid && baby.ComfortableTemperatureAtCell(result.Cell, baby.MapHeld))
			{
				return result;
			}
			if (building_Bed != null && baby.SafeTemperatureRange().Includes(temperatureForCell))
			{
				return building_Bed;
			}
			if (result.IsValid && baby.SafeTemperatureAtCell(result.Cell, baby.MapHeld))
			{
				return result;
			}
			LocalTargetInfo result2 = LocalTargetInfo.Invalid;
			if (baby.Spawned)
			{
				result2 = baby.Position;
			}
			else
			{
				result2 = RCellFinder.SpotToStandDuringJob(hauler, null, null);
			}
			if (building_Bed != null && GenTemperature.GetTemperatureForCell(result2.Cell, baby.MapHeld) < temperatureForCell + 5f)
			{
				return building_Bed;
			}
			return result2;
		}

		// Token: 0x06006C29 RID: 27689 RVA: 0x00220268 File Offset: 0x0021E468
		public static Pawn FindUnsafeBaby(Pawn mom, AutofeedMode priorityLevel)
		{
			if (priorityLevel == AutofeedMode.Never)
			{
				return null;
			}
			ChildcareUtility.tmpBabyList.Clear();
			ChildcareUtility.tmpBabyList.AddRange(mom.MapHeld.mapPawns.FreeHumanlikesSpawnedOfFaction(mom.Faction));
			foreach (Pawn pawn in ChildcareUtility.tmpBabyList)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason) && pawn.mindState.AutofeedSetting(mom) == priorityLevel && !CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(pawn))
				{
					LocalTargetInfo localTargetInfo = ChildcareUtility.SafePlaceForBaby(pawn, mom, false);
					if (localTargetInfo.IsValid)
					{
						Building_Bed building_Bed = localTargetInfo.Thing as Building_Bed;
						if (building_Bed != null)
						{
							if (pawn.CurrentBed() == building_Bed)
							{
								continue;
							}
						}
						else if (pawn.Spawned && pawn.Position == localTargetInfo.Cell)
						{
							continue;
						}
						ChildcareUtility.tmpBabyList.Clear();
						return pawn;
					}
				}
			}
			ChildcareUtility.tmpBabyList.Clear();
			return null;
		}

		// Token: 0x06006C2A RID: 27690 RVA: 0x00220370 File Offset: 0x0021E570
		public static bool BabyBeingPlayedWith(Pawn baby)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(baby, out breastfeedFailReason))
			{
				return false;
			}
			ChildcareUtility.tmpReserversOut.Clear();
			baby.MapHeld.reservationManager.ReserversOf(baby, ChildcareUtility.tmpReserversOut);
			foreach (Pawn pawn in ChildcareUtility.tmpReserversOut)
			{
				if (baby.MapHeld.reservationManager.ReservedBy(baby, pawn, pawn.CurJob) && pawn.jobs.curDriver is JobDriver_BabyPlay)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004BDD RID: 19421
		public static readonly IntRange NextCachedDownedMotherTickCheck = new IntRange(200, 300);

		// Token: 0x04004BDE RID: 19422
		private static Dictionary<Pawn, Dictionary<Pawn, Pawn>> cachedHaulerBabyMotherImmobile = new Dictionary<Pawn, Dictionary<Pawn, Pawn>>();

		// Token: 0x04004BDF RID: 19423
		private static int canBreastfeedCacheGameTicks = -1;

		// Token: 0x04004BE0 RID: 19424
		private static List<Pawn> canBreastfeedPlayerPawns = new List<Pawn>();

		// Token: 0x04004BE1 RID: 19425
		private const float BreastfeedHungerPercentage = 0.66f;

		// Token: 0x04004BE2 RID: 19426
		private const int FullLactatingSeverityTicks = 10000;

		// Token: 0x04004BE3 RID: 19427
		public const int FullFeedSessionTicks = 5000;

		// Token: 0x04004BE4 RID: 19428
		public const int BabyFullFeedsPerDay = 2;

		// Token: 0x04004BE5 RID: 19429
		private const float PreferBedUnsafeTemperatureEpsilon = 5f;

		// Token: 0x04004BE6 RID: 19430
		private const int BabyTemperatureMoveRepeatTicksDelay = 2500;

		// Token: 0x04004BE7 RID: 19431
		private static List<Pawn> childcarers = new List<Pawn>();

		// Token: 0x04004BE8 RID: 19432
		private static List<Pawn> tmpAutoBreastfeedOpportunisticMoms = new List<Pawn>();

		// Token: 0x04004BE9 RID: 19433
		private static List<Pawn> tmpAutoBreastfeedOffMoms = new List<Pawn>();

		// Token: 0x04004BEA RID: 19434
		private static readonly List<Pawn> tmpBabyList = new List<Pawn>();

		// Token: 0x04004BEB RID: 19435
		private static HashSet<Pawn> tmpReserversOut = new HashSet<Pawn>();

		// Token: 0x02001149 RID: 4425
		public enum BreastfeedFailReason
		{
			// Token: 0x04004BED RID: 19437
			MomNotEnoughMilk,
			// Token: 0x04004BEE RID: 19438
			MomInMentalState,
			// Token: 0x04004BEF RID: 19439
			BabyTooFull,
			// Token: 0x04004BF0 RID: 19440
			BabyForbiddenToMom,
			// Token: 0x04004BF1 RID: 19441
			BabyForbiddenToHauler,
			// Token: 0x04004BF2 RID: 19442
			MomForbiddenToHauler,
			// Token: 0x04004BF3 RID: 19443
			MomIsMobile,
			// Token: 0x04004BF4 RID: 19444
			HaulerCannotReachBaby,
			// Token: 0x04004BF5 RID: 19445
			HaulerCannotReachMom,
			// Token: 0x04004BF6 RID: 19446
			NoMomsAssignedToBaby,
			// Token: 0x04004BF7 RID: 19447
			MomNotAssignedToChildcare,
			// Token: 0x04004BF8 RID: 19448
			MomInsideContainer,
			// Token: 0x04004BF9 RID: 19449
			MomBeingCarried,
			// Token: 0x04004BFA RID: 19450
			NoBiotechMod,
			// Token: 0x04004BFB RID: 19451
			MomNull,
			// Token: 0x04004BFC RID: 19452
			BabyNull,
			// Token: 0x04004BFD RID: 19453
			HaulerNull,
			// Token: 0x04004BFE RID: 19454
			BabyNotHumanlike,
			// Token: 0x04004BFF RID: 19455
			MomNotHumanLike,
			// Token: 0x04004C00 RID: 19456
			BabyDead,
			// Token: 0x04004C01 RID: 19457
			MomDead,
			// Token: 0x04004C02 RID: 19458
			BabyTooOld,
			// Token: 0x04004C03 RID: 19459
			HaulerDowned,
			// Token: 0x04004C04 RID: 19460
			HaulerDead,
			// Token: 0x04004C05 RID: 19461
			MomNotLactating,
			// Token: 0x04004C06 RID: 19462
			BabyNotHungry,
			// Token: 0x04004C07 RID: 19463
			BabyNotOnMap,
			// Token: 0x04004C08 RID: 19464
			MomNotOnMap,
			// Token: 0x04004C09 RID: 19465
			HaulerNotOnMap,
			// Token: 0x04004C0A RID: 19466
			HaulerNotOnBabyMap,
			// Token: 0x04004C0B RID: 19467
			HaulerNotOnMomMap,
			// Token: 0x04004C0C RID: 19468
			MomNotAssignedToBaby,
			// Token: 0x04004C0D RID: 19469
			BabyInIncompatibleFactionToMom,
			// Token: 0x04004C0E RID: 19470
			BabyInIncompatibleFactionToHauler,
			// Token: 0x04004C0F RID: 19471
			MomInIncompatibleFactionToHauler,
			// Token: 0x04004C10 RID: 19472
			HaulerCannotReserveBaby,
			// Token: 0x04004C11 RID: 19473
			HaulerCannotReserveMom,
			// Token: 0x04004C12 RID: 19474
			HaulerIncapableOfManipulation,
			// Token: 0x04004C13 RID: 19475
			BabyShambler
		}
	}
}
