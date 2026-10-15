using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public static class ChildcareUtility
{
	public enum BreastfeedFailReason
	{
		MomNotEnoughMilk,
		MomInMentalState,
		BabyTooFull,
		BabyForbiddenToMom,
		BabyForbiddenToHauler,
		MomForbiddenToHauler,
		MomIsMobile,
		HaulerCannotReachBaby,
		HaulerCannotReachMom,
		NoMomsAssignedToBaby,
		MomNotAssignedToChildcare,
		MomInsideContainer,
		MomBeingCarried,
		NoBiotechMod,
		MomNull,
		BabyNull,
		HaulerNull,
		BabyNotHumanlike,
		MomNotHumanLike,
		BabyDead,
		MomDead,
		BabyTooOld,
		HaulerDowned,
		HaulerDead,
		MomNotLactating,
		BabyNotHungry,
		BabyNotOnMap,
		MomNotOnMap,
		HaulerNotOnMap,
		HaulerNotOnBabyMap,
		HaulerNotOnMomMap,
		MomNotAssignedToBaby,
		BabyInIncompatibleFactionToMom,
		BabyInIncompatibleFactionToHauler,
		MomInIncompatibleFactionToHauler,
		HaulerCannotReserveBaby,
		HaulerCannotReserveMom,
		HaulerIncapableOfManipulation,
		BabyShambler
	}

	[CompilerGenerated]
	private sealed class _003CCanBreastfeedMothers_003Ed__34 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn baby;

		public Pawn _003C_003E3__baby;

		private bool includeAutoBreastfeedOff;

		public bool _003C_003E3__includeAutoBreastfeedOff;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCanBreastfeedMothers_003Ed__34(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!baby.SpawnedOrAnyParentSpawned)
					{
						return false;
					}
					tmpAutoBreastfeedOpportunisticMoms.Clear();
					tmpAutoBreastfeedOffMoms.Clear();
					_003C_003E7__wrap1 = baby.MapHeld.mapPawns.FreeColonistsAndPrisoners.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00f8;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00f8;
				case 2:
					_003C_003E1__state = -4;
					goto IL_015f;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_01c8;
					}
					IL_015f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					if (!includeAutoBreastfeedOff)
					{
						break;
					}
					_003C_003E7__wrap1 = tmpAutoBreastfeedOffMoms.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_01c8;
					IL_01c8:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					break;
					IL_00f8:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Pawn current3 = _003C_003E7__wrap1.Current;
						if (CanMomBreastfeedBaby(current3, baby, out var _))
						{
							switch (baby.mindState.AutofeedSetting(current3))
							{
							case AutofeedMode.Urgent:
								_003C_003E2__current = current3;
								_003C_003E1__state = 1;
								return true;
							case AutofeedMode.Childcare:
								tmpAutoBreastfeedOpportunisticMoms.Add(current3);
								break;
							case AutofeedMode.Never:
								tmpAutoBreastfeedOffMoms.Add(current3);
								break;
							}
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					_003C_003E7__wrap1 = tmpAutoBreastfeedOpportunisticMoms.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_015f;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CCanBreastfeedMothers_003Ed__34 _003CCanBreastfeedMothers_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCanBreastfeedMothers_003Ed__ = this;
			}
			else
			{
				_003CCanBreastfeedMothers_003Ed__ = new _003CCanBreastfeedMothers_003Ed__34(0);
			}
			_003CCanBreastfeedMothers_003Ed__.baby = _003C_003E3__baby;
			_003CCanBreastfeedMothers_003Ed__.includeAutoBreastfeedOff = _003C_003E3__includeAutoBreastfeedOff;
			return _003CCanBreastfeedMothers_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public static readonly IntRange NextCachedDownedMotherTickCheck = new IntRange(200, 300);

	private static Dictionary<Pawn, Dictionary<Pawn, Pawn>> cachedHaulerBabyMotherImmobile = new Dictionary<Pawn, Dictionary<Pawn, Pawn>>();

	private static int canBreastfeedCacheGameTicks = -1;

	private static List<Pawn> canBreastfeedPlayerPawns = new List<Pawn>();

	private const float BreastfeedHungerPercentage = 0.66f;

	private const int FullLactatingSeverityTicks = 10000;

	public const int FullFeedSessionTicks = 5000;

	public const int BabyFullFeedsPerDay = 2;

	private const float PreferBedUnsafeTemperatureEpsilon = 5f;

	private const int BabyTemperatureMoveRepeatTicksDelay = 2500;

	private static List<Pawn> childcarers = new List<Pawn>();

	private static List<Pawn> tmpAutoBreastfeedOpportunisticMoms = new List<Pawn>();

	private static List<Pawn> tmpAutoBreastfeedOffMoms = new List<Pawn>();

	private static readonly List<Pawn> tmpBabyList = new List<Pawn>();

	private static HashSet<Pawn> tmpReserversOut = new HashSet<Pawn>();

	public static List<Pawn> CanBreastfeedPlayerPawns
	{
		get
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (canBreastfeedCacheGameTicks < ticksGame)
			{
				canBreastfeedPlayerPawns.Clear();
				foreach (Pawn item in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
				{
					if (CanBreastfeed(item, out var _))
					{
						canBreastfeedPlayerPawns.Add(item);
					}
				}
				canBreastfeedCacheGameTicks = ticksGame;
			}
			return canBreastfeedPlayerPawns;
		}
	}

	public static List<Pawn> SpawnedColonistChildcarers(Map map)
	{
		childcarers.Clear();
		foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
		{
			if (!item.WorkTypeIsDisabled(WorkTypeDefOf.Childcare))
			{
				childcarers.Add(item);
			}
		}
		return childcarers;
	}

	public static TaggedString Translate(this BreastfeedFailReason reason, Pawn hauler = null, Pawn mom = null, Pawn baby = null)
	{
		NamedArgument arg = hauler.Named("HAULER");
		NamedArgument arg2 = mom.Named("MOM");
		NamedArgument arg3 = baby.Named("BABY");
		string key;
		switch (reason)
		{
		case BreastfeedFailReason.MomNotEnoughMilk:
			key = "BreastfeedFailReason_MomNotEnoughMilk";
			break;
		case BreastfeedFailReason.MomInMentalState:
			key = "BreastfeedFailReason_MomInMentalState";
			break;
		case BreastfeedFailReason.BabyTooFull:
			key = "BreastfeedFailReason_BabyTooFull";
			break;
		case BreastfeedFailReason.BabyForbiddenToMom:
			key = "BreastfeedFailReason_BabyForbiddenToMom";
			break;
		case BreastfeedFailReason.BabyForbiddenToHauler:
			key = "BreastfeedFailReason_BabyForbiddenToHauler";
			break;
		case BreastfeedFailReason.MomForbiddenToHauler:
			key = "BreastfeedFailReason_MomForbiddenToHauler";
			break;
		case BreastfeedFailReason.MomIsMobile:
			key = "BreastfeedFailReason_MomIsMobile";
			break;
		case BreastfeedFailReason.HaulerCannotReachBaby:
			key = "BreastfeedFailReason_HaulerCannotReachBaby";
			break;
		case BreastfeedFailReason.HaulerCannotReachMom:
			key = "BreastfeedFailReason_HaulerCannotReachMom";
			break;
		case BreastfeedFailReason.NoMomsAssignedToBaby:
			key = "BreastfeedFailReason_NoMomsAssignedToBaby";
			break;
		case BreastfeedFailReason.MomNotAssignedToChildcare:
			key = "BreastfeedFailReason_MomNotAssignedToChildcare";
			break;
		case BreastfeedFailReason.MomInsideContainer:
			key = "BreastfeedFailReason_MomInsideContainer";
			break;
		case BreastfeedFailReason.MomBeingCarried:
			key = "BreastfeedFailReason_MomBeingCarried";
			break;
		default:
			Log.ErrorOnce($"BreastfeedFailReason {reason} should not be player facing.", (int)(784668349 + reason));
			return reason.ToString();
		}
		return key.Translate(arg, arg2, arg3);
	}

	public static bool CanFeed(Pawn mom, out BreastfeedFailReason? reason)
	{
		reason = null;
		if (mom == null)
		{
			reason = BreastfeedFailReason.MomNull;
		}
		else if (mom.Dead)
		{
			reason = BreastfeedFailReason.MomDead;
		}
		else if (!mom.RaceProps.Humanlike)
		{
			reason = BreastfeedFailReason.MomNotHumanLike;
		}
		return !reason.HasValue;
	}

	public static bool CanBreastfeed(Pawn mom, out BreastfeedFailReason? reason)
	{
		reason = null;
		if (!CanFeed(mom, out reason))
		{
			return false;
		}
		if (!ModsConfig.BiotechActive)
		{
			reason = BreastfeedFailReason.NoBiotechMod;
		}
		else if (!mom.health.hediffSet.HasHediff(HediffDefOf.Lactating))
		{
			reason = BreastfeedFailReason.MomNotLactating;
		}
		return !reason.HasValue;
	}

	public static bool CanBreastfeedNow(Pawn mom, out BreastfeedFailReason? reason)
	{
		if (!CanBreastfeed(mom, out reason))
		{
			return false;
		}
		Hediff firstHediffOfDef = mom.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);
		if (firstHediffOfDef == null || firstHediffOfDef.TryGetComp<HediffComp_Chargeable>()?.CanActivate != true)
		{
			reason = BreastfeedFailReason.MomNotEnoughMilk;
		}
		else if (mom.InMentalState && !mom.Downed)
		{
			reason = BreastfeedFailReason.MomInMentalState;
		}
		return !reason.HasValue;
	}

	public static bool CanSuckle(Pawn baby, out BreastfeedFailReason? reason)
	{
		reason = null;
		if (!ModsConfig.BiotechActive)
		{
			reason = BreastfeedFailReason.NoBiotechMod;
		}
		else if (baby == null)
		{
			reason = BreastfeedFailReason.BabyNull;
		}
		else if (baby.Dead)
		{
			reason = BreastfeedFailReason.BabyDead;
		}
		else if (!baby.RaceProps.Humanlike)
		{
			reason = BreastfeedFailReason.BabyNotHumanlike;
		}
		else if (!baby.DevelopmentalStage.Baby())
		{
			reason = BreastfeedFailReason.BabyTooOld;
		}
		else if (baby.IsShambler)
		{
			reason = BreastfeedFailReason.BabyShambler;
		}
		return !reason.HasValue;
	}

	public static bool WantsSuckle(Pawn baby, out BreastfeedFailReason? reason)
	{
		if (!CanSuckle(baby, out reason))
		{
			return false;
		}
		if (!FeedPatientUtility.IsHungry(baby))
		{
			reason = BreastfeedFailReason.BabyNotHungry;
		}
		return !reason.HasValue;
	}

	public static bool CanSuckleNow(Pawn baby, out BreastfeedFailReason? reason)
	{
		if (!CanSuckle(baby, out reason))
		{
			return false;
		}
		if (baby.needs?.food != null && baby.needs.food.CurLevelPercentage >= 0.66f)
		{
			reason = BreastfeedFailReason.BabyTooFull;
		}
		return !reason.HasValue;
	}

	public static bool CanFeedBaby(Pawn feeder, Pawn baby, out BreastfeedFailReason? reason)
	{
		if (!CanFeed(feeder, out reason))
		{
			return false;
		}
		if (!CanSuckle(baby, out reason))
		{
			return false;
		}
		if (feeder.MapHeld != null && baby.IsForbidden(feeder))
		{
			reason = BreastfeedFailReason.BabyForbiddenToMom;
		}
		else if (!HasBreastfeedCompatibleFactions(feeder, baby))
		{
			reason = BreastfeedFailReason.BabyInIncompatibleFactionToMom;
		}
		else
		{
			reason = null;
		}
		return !reason.HasValue;
	}

	public static bool CanMomBreastfeedBaby(Pawn mom, Pawn baby, out BreastfeedFailReason? reason)
	{
		if (!CanBreastfeed(mom, out reason))
		{
			return false;
		}
		if (!CanSuckle(baby, out reason))
		{
			return false;
		}
		if (!CanFeedBaby(mom, baby, out reason))
		{
			return false;
		}
		return !reason.HasValue;
	}

	public static bool CanMomBreastfeedBabyNow(Pawn mom, Pawn baby, out BreastfeedFailReason? reason)
	{
		if (!CanBreastfeedNow(mom, out reason))
		{
			return false;
		}
		if (!CanSuckleNow(baby, out reason))
		{
			return false;
		}
		if (!CanMomBreastfeedBaby(mom, baby, out reason))
		{
			return false;
		}
		return !reason.HasValue;
	}

	public static bool CanHaulBaby(Pawn hauler, Pawn baby, out BreastfeedFailReason? reason)
	{
		reason = null;
		if (hauler == null)
		{
			reason = BreastfeedFailReason.HaulerNull;
		}
		else if (baby == null)
		{
			reason = BreastfeedFailReason.BabyNull;
		}
		else if (hauler.Dead)
		{
			reason = BreastfeedFailReason.HaulerDead;
		}
		else if (hauler.Downed)
		{
			reason = BreastfeedFailReason.HaulerDowned;
		}
		else if (hauler.Map == null)
		{
			reason = BreastfeedFailReason.HaulerNotOnMap;
		}
		else if (baby.MapHeld == null)
		{
			reason = BreastfeedFailReason.BabyNotOnMap;
		}
		else if (hauler.Map != baby.MapHeld)
		{
			reason = BreastfeedFailReason.HaulerNotOnBabyMap;
		}
		else if (baby.IsForbidden(hauler))
		{
			reason = BreastfeedFailReason.BabyForbiddenToHauler;
		}
		else if (!HasBreastfeedCompatibleFactions(hauler, baby))
		{
			if (!BabyHasFeederInCompatibleFaction(hauler.Faction, baby))
			{
				reason = BreastfeedFailReason.BabyInIncompatibleFactionToHauler;
			}
		}
		else if (!hauler.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			reason = BreastfeedFailReason.HaulerIncapableOfManipulation;
		}
		return !reason.HasValue;
	}

	public static bool CanHaulBabyNow(Pawn hauler, Pawn baby, bool ignoreOtherReservations, out BreastfeedFailReason? reason)
	{
		if (!CanHaulBaby(hauler, baby, out reason))
		{
			return false;
		}
		if (!hauler.CanReserve(baby, 1, -1, null, ignoreOtherReservations))
		{
			reason = BreastfeedFailReason.HaulerCannotReserveBaby;
		}
		else if (!hauler.CanReach(baby, PathEndMode.Touch, Danger.Deadly))
		{
			reason = BreastfeedFailReason.HaulerCannotReachBaby;
		}
		return !reason.HasValue;
	}

	public static bool CanHaulToMom(Pawn hauler, Pawn mom, out BreastfeedFailReason? reason)
	{
		reason = null;
		if (hauler == null)
		{
			reason = BreastfeedFailReason.HaulerNull;
		}
		else if (mom == null)
		{
			reason = BreastfeedFailReason.MomNull;
		}
		else if (hauler.Dead)
		{
			reason = BreastfeedFailReason.HaulerDead;
		}
		else if (hauler.Downed)
		{
			reason = BreastfeedFailReason.HaulerDowned;
		}
		else if (mom.MapHeld == null)
		{
			reason = BreastfeedFailReason.MomNotOnMap;
		}
		else if (hauler.Map == null)
		{
			reason = BreastfeedFailReason.HaulerNotOnMap;
		}
		else if (hauler.Map != mom.MapHeld)
		{
			reason = BreastfeedFailReason.HaulerNotOnMomMap;
		}
		else if (hauler != mom)
		{
			if (mom.IsForbidden(hauler))
			{
				reason = BreastfeedFailReason.MomForbiddenToHauler;
			}
			else if (!HasBreastfeedCompatibleFactions(hauler, mom))
			{
				reason = BreastfeedFailReason.MomInIncompatibleFactionToHauler;
			}
		}
		return !reason.HasValue;
	}

	public static bool CanHaulBabyToMomNow(Pawn hauler, Pawn mom, Pawn baby, bool ignoreOtherReservations, out BreastfeedFailReason? reason)
	{
		if (!CanHaulBabyNow(hauler, baby, ignoreOtherReservations, out reason))
		{
			return false;
		}
		if (!CanHaulToMom(hauler, mom, out reason))
		{
			return false;
		}
		if (hauler != mom)
		{
			if (!mom.Downed && !mom.IsPrisoner && CanHaulBabyToMomNow(mom, mom, baby, ignoreOtherReservations, out var _))
			{
				reason = BreastfeedFailReason.MomIsMobile;
			}
			else if (!hauler.CanReserve(mom, 1, -1, null, ignoreOtherReservations))
			{
				reason = BreastfeedFailReason.HaulerCannotReserveMom;
			}
			else if (!mom.Spawned)
			{
				if (mom.SpawnedParentOrMe is Pawn)
				{
					reason = BreastfeedFailReason.MomBeingCarried;
				}
				else
				{
					reason = BreastfeedFailReason.MomInsideContainer;
				}
			}
			else if (!hauler.Map.reachability.CanReach(baby.Position, mom.Position, PathEndMode.Touch, TraverseParms.For(hauler)))
			{
				reason = BreastfeedFailReason.HaulerCannotReachMom;
			}
		}
		return !reason.HasValue;
	}

	public static bool CanAutoBreastfeed(Pawn mom, Pawn baby, bool forced, out BreastfeedFailReason? reason)
	{
		if (!CanMomBreastfeedBaby(mom, baby, out reason))
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
			reason = BreastfeedFailReason.MomNotAssignedToChildcare;
		}
		else if (!forced && autofeedMode == AutofeedMode.Never)
		{
			reason = BreastfeedFailReason.MomNotAssignedToBaby;
		}
		return !reason.HasValue;
	}

	public static bool CanMomAutoBreastfeedBabyNow(Pawn mother, Pawn baby, bool forced, out BreastfeedFailReason? reason)
	{
		if (!CanMomBreastfeedBabyNow(mother, baby, out reason))
		{
			return false;
		}
		if (!CanAutoBreastfeed(mother, baby, forced, out reason))
		{
			return false;
		}
		if (!CanHaulBabyToMomNow(mother, mother, baby, forced, out reason))
		{
			return false;
		}
		return true;
	}

	public static bool CanHaulBabyToDownedMomToBreastfeedNow(Pawn hauler, Pawn mother, Pawn baby, bool forced, out BreastfeedFailReason? reason)
	{
		if (!CanMomBreastfeedBabyNow(mother, baby, out reason))
		{
			return false;
		}
		if (!CanAutoBreastfeed(mother, baby, forced, out reason))
		{
			return false;
		}
		if (!CanHaulBabyToMomNow(hauler, mother, baby, forced, out reason))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CCanBreastfeedMothers_003Ed__34))]
	public static IEnumerable<Pawn> CanBreastfeedMothers(Pawn baby, bool includeAutoBreastfeedOff = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCanBreastfeedMothers_003Ed__34(-2)
		{
			_003C_003E3__baby = baby,
			_003C_003E3__includeAutoBreastfeedOff = includeAutoBreastfeedOff
		};
	}

	public static Job MakeBringBabyToSafetyJob(Pawn hauler, Pawn baby)
	{
		Job job = JobMaker.MakeJob(JobDefOf.BringBabyToSafety, baby);
		job.count = 1;
		return job;
	}

	public static Job MakeBreastfeedCarryToMomJob(Pawn baby, Pawn mom)
	{
		Job job = JobMaker.MakeJob(JobDefOf.BreastfeedCarryToMom, baby, mom);
		job.count = 1;
		return job;
	}

	public static Job MakeBreastfeedJob(Pawn baby, Building_Bed bedWithDownedMom = null)
	{
		Job obj = ((bedWithDownedMom == null) ? JobMaker.MakeJob(JobDefOf.Breastfeed, baby) : JobMaker.MakeJob(JobDefOf.Breastfeed, baby, bedWithDownedMom));
		obj.count = 1;
		return obj;
	}

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

	public static Job MakeAutofeedBabyJob(Pawn feeder, Pawn baby, Thing foodSource)
	{
		if (foodSource == null)
		{
			Log.Error("Cannot MakeFeedBabyJob with null foodSource.");
			return null;
		}
		if (foodSource is Pawn pawn)
		{
			if (feeder == pawn)
			{
				return MakeBreastfeedJob(baby);
			}
			return MakeBreastfeedCarryToMomJob(baby, pawn);
		}
		return MakeBottlefeedJob(baby, foodSource);
	}

	public static Job MakeBabySuckleJob(Pawn feeder)
	{
		Job job = JobMaker.MakeJob(JobDefOf.BabySuckle, feeder);
		job.count = 1;
		return job;
	}

	public static Job MakeBabyPlayJob(Pawn feeder)
	{
		Job job = JobMaker.MakeJob(JobDefOf.BabyPlay, feeder);
		job.count = 1;
		return job;
	}

	public static Toil MakeBabyPlayAsLongAsToilIsActive(Toil toil, TargetIndex babyIndex)
	{
		toil.AddPreInitAction(delegate
		{
			((Pawn)toil.actor.jobs.curJob.GetTarget(babyIndex).Thing).jobs.StartJob(MakeBabyPlayJob(toil.actor), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
		});
		toil.AddFinishAction(delegate
		{
			Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(babyIndex).Thing;
			if (pawn.CurJobDef == JobDefOf.BabyPlay)
			{
				pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
			}
		});
		return toil;
	}

	public static bool HasBreastfeedCompatibleFactions(Faction faction, Pawn baby)
	{
		if (baby.Faction == faction)
		{
			return true;
		}
		if (faction != null && baby.HostFaction == faction)
		{
			return true;
		}
		return false;
	}

	public static bool BabyHasFeederInCompatibleFaction(Faction faction, Pawn baby)
	{
		foreach (Pawn item in CanBreastfeedMothers(baby))
		{
			if (item.Faction == faction)
			{
				return true;
			}
			if (faction != null && item.HostFaction == faction)
			{
				return true;
			}
		}
		return false;
	}

	public static bool HasBreastfeedCompatibleFactions(Pawn mom, Pawn baby)
	{
		return HasBreastfeedCompatibleFactions(mom.Faction, baby);
	}

	public static bool ImmobileBreastfeederAvailable(Pawn hauler, Pawn baby, bool forced, out Pawn feeder, out BreastfeedFailReason? reason)
	{
		if (!cachedHaulerBabyMotherImmobile.ContainsKey(hauler))
		{
			cachedHaulerBabyMotherImmobile[hauler] = new Dictionary<Pawn, Pawn>();
		}
		if (cachedHaulerBabyMotherImmobile[hauler].TryGetValue(baby, out var value) && CanHaulBabyToDownedMomToBreastfeedNow(hauler, value, baby, forced, out var _))
		{
			feeder = value;
			reason = null;
			return true;
		}
		reason = BreastfeedFailReason.NoMomsAssignedToBaby;
		feeder = null;
		foreach (Pawn item in CanBreastfeedMothers(baby, forced))
		{
			if (item == hauler)
			{
				if (feeder == null)
				{
					reason = null;
					feeder = item;
				}
				continue;
			}
			if (!CanHaulBabyToDownedMomToBreastfeedNow(hauler, item, baby, forced, out reason))
			{
				feeder = item;
				continue;
			}
			cachedHaulerBabyMotherImmobile[hauler][baby] = item;
			return true;
		}
		return false;
	}

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
		if (!CanFeed(feeder, out var reason))
		{
			return false;
		}
		Thing food;
		Pawn pawn = FindAutofeedBaby(feeder, AutofeedMode.Urgent, out food);
		if (pawn == null)
		{
			return false;
		}
		if (food == null && !ImmobileBreastfeederAvailable(feeder, pawn, forced: false, out var _, out reason))
		{
			return false;
		}
		return true;
	}

	public static Thing FindBabyFoodForBaby(Pawn feeder, Pawn baby)
	{
		if (!FoodUtility.TryFindBestFoodSourceFor(feeder, baby, baby.needs.food.CurCategory == HungerCategory.Starving, out var foodSource, out var _, canRefillDispenser: false, canUseInventory: true, canUsePackAnimalInventory: false, allowForbidden: false, allowCorpse: true, allowSociallyImproper: false, allowHarvest: false, forceScanWholeMap: false, ignoreReservations: false, calculateWantedStackCount: false, allowVenerated: false, FoodPreferability.RawBad))
		{
			return null;
		}
		return foodSource;
	}

	public static Pawn FindAutofeedBaby(Pawn mom, AutofeedMode priorityLevel, out Thing food)
	{
		if (priorityLevel == AutofeedMode.Never)
		{
			Log.Warning("Will never auto breastfeed a baby if priority level is OFF");
			food = null;
			return null;
		}
		BreastfeedFailReason? reason;
		bool flag = CanBreastfeedNow(mom, out reason);
		foreach (Pawn item in mom.MapHeld.mapPawns.FreeHumanlikesOfFaction(mom.Faction))
		{
			if (!item.Suspended && WantsSuckle(item, out reason) && item.mindState.AutofeedSetting(mom) == priorityLevel && CanFeedBaby(mom, item, out reason) && CanHaulBabyToMomNow(mom, mom, item, ignoreOtherReservations: false, out reason))
			{
				if (flag)
				{
					food = mom;
				}
				else
				{
					food = FindBabyFoodForBaby(mom, item);
				}
				return item;
			}
		}
		food = null;
		return null;
	}

	public static bool SuckleFromLactatingPawn(Pawn baby, Pawn feeder, int delta)
	{
		Hediff firstHediffOfDef = feeder.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);
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
		baby.ideo?.IncreaseIdeoExposureIfBabyTick(feeder.Ideo);
		if (Mathf.Approximately(num2, nutritionWanted))
		{
			return false;
		}
		if (num2 < num)
		{
			return false;
		}
		return true;
	}

	public static void ClearCache()
	{
		canBreastfeedCacheGameTicks = -1;
		canBreastfeedPlayerPawns.Clear();
		cachedHaulerBabyMotherImmobile.Clear();
	}

	public static bool SwaddleBaby(this Pawn baby)
	{
		if (ModsConfig.BiotechActive && baby.RaceProps.Humanlike && baby.DevelopmentalStage.Baby() && !baby.Dead)
		{
			return !(baby.ParentHolder is Building_GrowthVat);
		}
		return false;
	}

	public static bool BabyNeedsMovingForTemperatureReasons(Pawn baby, Pawn hauler, out Region preferredRegion, IntVec3? positionOverride = null)
	{
		if (!CanSuckle(baby, out var reason))
		{
			preferredRegion = null;
			return false;
		}
		if (!CanHaulBabyNow(hauler, baby, ignoreOtherReservations: false, out reason))
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
		float f = ((!positionOverride.HasValue) ? baby.AmbientTemperature : GenTemperature.GetTemperatureForCell(positionOverride.Value, baby.MapHeld));
		if (tempRange.Includes(f))
		{
			preferredRegion = null;
			return false;
		}
		Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(root, baby.MapHeld, baby, tempRange, TraverseParms.For(hauler, Danger.Some));
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
		Region region2 = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(root, baby.MapHeld, baby, tempRange2, TraverseParms.For(hauler, Danger.Some));
		if (region2 != null)
		{
			preferredRegion = region2;
			return true;
		}
		preferredRegion = null;
		return false;
	}

	public static LocalTargetInfo SafePlaceForBaby(Pawn baby, Pawn hauler, bool ignoreOtherReservations = false)
	{
		if (!CanSuckle(baby, out var reason))
		{
			return LocalTargetInfo.Invalid;
		}
		if (!CanHaulBabyNow(hauler, baby, ignoreOtherReservations, out reason))
		{
			return LocalTargetInfo.Invalid;
		}
		Building_Bed building_Bed = baby.CurrentBed() ?? RestUtility.FindBedFor(baby, hauler, checkSocialProperness: true, ignoreOtherReservations: false, baby.GuestStatus);
		float temperatureForCell = GenTemperature.GetTemperatureForCell(building_Bed?.Position ?? IntVec3.Invalid, baby.MapHeld);
		if (building_Bed != null && building_Bed.Medical && HealthAIUtility.ShouldSeekMedicalRest(baby))
		{
			return building_Bed;
		}
		if (building_Bed != null && baby.ComfortableTemperatureRange().Includes(temperatureForCell))
		{
			return building_Bed;
		}
		LocalTargetInfo invalid = LocalTargetInfo.Invalid;
		invalid = (BabyNeedsMovingForTemperatureReasons(baby, hauler, out var preferredRegion, null) ? ((LocalTargetInfo)RCellFinder.SpotToStandDuringJob(hauler, null, preferredRegion)) : ((!baby.Spawned) ? ((LocalTargetInfo)RCellFinder.SpotToStandDuringJob(hauler, null, baby.GetRegionHeld())) : ((LocalTargetInfo)baby.Position)));
		if (invalid.IsValid && baby.ComfortableTemperatureAtCell(invalid.Cell, baby.MapHeld))
		{
			return invalid;
		}
		if (building_Bed != null && baby.SafeTemperatureRange().Includes(temperatureForCell))
		{
			return building_Bed;
		}
		if (invalid.IsValid && baby.SafeTemperatureAtCell(invalid.Cell, baby.MapHeld))
		{
			return invalid;
		}
		LocalTargetInfo invalid2 = LocalTargetInfo.Invalid;
		invalid2 = ((!baby.Spawned) ? ((LocalTargetInfo)RCellFinder.SpotToStandDuringJob(hauler)) : ((LocalTargetInfo)baby.Position));
		if (building_Bed != null && GenTemperature.GetTemperatureForCell(invalid2.Cell, baby.MapHeld) < temperatureForCell + 5f)
		{
			return building_Bed;
		}
		return invalid2;
	}

	public static Pawn FindUnsafeBaby(Pawn mom, AutofeedMode priorityLevel)
	{
		if (priorityLevel == AutofeedMode.Never)
		{
			return null;
		}
		tmpBabyList.Clear();
		tmpBabyList.AddRange(mom.MapHeld.mapPawns.FreeHumanlikesSpawnedOfFaction(mom.Faction));
		foreach (Pawn tmpBaby in tmpBabyList)
		{
			if (!CanSuckle(tmpBaby, out var _) || tmpBaby.mindState.AutofeedSetting(mom) != priorityLevel || CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(tmpBaby))
			{
				continue;
			}
			LocalTargetInfo localTargetInfo = SafePlaceForBaby(tmpBaby, mom);
			if (!localTargetInfo.IsValid)
			{
				continue;
			}
			if (localTargetInfo.Thing is Building_Bed building_Bed)
			{
				if (tmpBaby.CurrentBed() == building_Bed)
				{
					continue;
				}
			}
			else if (tmpBaby.Spawned && tmpBaby.Position == localTargetInfo.Cell)
			{
				continue;
			}
			tmpBabyList.Clear();
			return tmpBaby;
		}
		tmpBabyList.Clear();
		return null;
	}

	public static bool BabyBeingPlayedWith(Pawn baby)
	{
		if (!CanSuckle(baby, out var _))
		{
			return false;
		}
		tmpReserversOut.Clear();
		baby.MapHeld.reservationManager.ReserversOf(baby, tmpReserversOut);
		foreach (Pawn item in tmpReserversOut)
		{
			if (baby.MapHeld.reservationManager.ReservedBy(baby, item, item.CurJob) && item.jobs.curDriver is JobDriver_BabyPlay)
			{
				return true;
			}
		}
		return false;
	}
}
