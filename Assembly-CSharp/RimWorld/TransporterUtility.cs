using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public static class TransporterUtility
{
	[CompilerGenerated]
	private sealed class _003CAllSendableItems_003Ed__5 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private List<CompTransporter> transporters;

		public List<CompTransporter> _003C_003E3__transporters;

		private List<Thing> _003Citems_003E5__2;

		private CompShuttle _003Cshuttle_003E5__3;

		private int _003Ci_003E5__4;

		Thing IEnumerator<Thing>.Current
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
		public _003CAllSendableItems_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Citems_003E5__2 = null;
			_003Cshuttle_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00f6;
			}
			_003C_003E1__state = -1;
			_003Citems_003E5__2 = CaravanFormingUtility.AllReachableColonyItems(map, !map.IsPlayerHome, transporters[0].Props.canChangeAssignedThingsAfterStarting && transporters[0].LoadingInProgressOrReadyToLaunch);
			_003Cshuttle_003E5__3 = transporters[0].parent.TryGetComp<CompShuttle>();
			_003Ci_003E5__4 = 0;
			goto IL_0106;
			IL_00f6:
			_003Ci_003E5__4++;
			goto IL_0106;
			IL_0106:
			if (_003Ci_003E5__4 < _003Citems_003E5__2.Count)
			{
				if (_003Cshuttle_003E5__3 == null || _003Cshuttle_003E5__3.IsRequired(_003Citems_003E5__2[_003Ci_003E5__4]) || _003Cshuttle_003E5__3.IsAllowed(_003Citems_003E5__2[_003Ci_003E5__4]))
				{
					_003C_003E2__current = _003Citems_003E5__2[_003Ci_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00f6;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CAllSendableItems_003Ed__5 _003CAllSendableItems_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllSendableItems_003Ed__ = this;
			}
			else
			{
				_003CAllSendableItems_003Ed__ = new _003CAllSendableItems_003Ed__5(0);
			}
			_003CAllSendableItems_003Ed__.transporters = _003C_003E3__transporters;
			_003CAllSendableItems_003Ed__.map = _003C_003E3__map;
			return _003CAllSendableItems_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllSendablePawns_003Ed__4 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<CompTransporter> transporters;

		public List<CompTransporter> _003C_003E3__transporters;

		private Map map;

		public Map _003C_003E3__map;

		private CompShuttle _003Cshuttle_003E5__2;

		private List<Pawn> _003Cpawns_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003CAllSendablePawns_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cshuttle_003E5__2 = null;
			_003Cpawns_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0109;
			}
			_003C_003E1__state = -1;
			_003Cshuttle_003E5__2 = transporters[0].parent.TryGetComp<CompShuttle>();
			_003Cpawns_003E5__3 = CaravanFormingUtility.AllSendablePawns(map, allowEvenIfDowned: true, allowEvenIfInMentalState: false, allowEvenIfPrisonerNotSecure: false, allowCapturableDownedPawns: false, allowLoadAndEnterTransportersLordForGroupID: (transporters[0].Props.canChangeAssignedThingsAfterStarting && transporters[0].LoadingInProgressOrReadyToLaunch) ? transporters[0].groupID : (-1), allowLodgers: _003Cshuttle_003E5__2 != null);
			_003Ci_003E5__4 = 0;
			goto IL_0119;
			IL_0109:
			_003Ci_003E5__4++;
			goto IL_0119;
			IL_0119:
			if (_003Ci_003E5__4 < _003Cpawns_003E5__3.Count)
			{
				if (_003Cshuttle_003E5__2 == null || _003Cshuttle_003E5__2.IsRequired(_003Cpawns_003E5__3[_003Ci_003E5__4]) || _003Cshuttle_003E5__2.IsAllowed(_003Cpawns_003E5__3[_003Ci_003E5__4]))
				{
					_003C_003E2__current = _003Cpawns_003E5__3[_003Ci_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0109;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CAllSendablePawns_003Ed__4 _003CAllSendablePawns_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllSendablePawns_003Ed__ = this;
			}
			else
			{
				_003CAllSendablePawns_003Ed__ = new _003CAllSendablePawns_003Ed__4(0);
			}
			_003CAllSendablePawns_003Ed__.transporters = _003C_003E3__transporters;
			_003CAllSendablePawns_003Ed__.map = _003C_003E3__map;
			return _003CAllSendablePawns_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CThingsBeingHauledTo_003Ed__6 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private List<CompTransporter> transporters;

		public List<CompTransporter> _003C_003E3__transporters;

		private IReadOnlyList<Pawn> _003Cpawns_003E5__2;

		private int _003Ci_003E5__3;

		Thing IEnumerator<Thing>.Current
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
		public _003CThingsBeingHauledTo_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cpawns_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00dc;
			}
			_003C_003E1__state = -1;
			_003Cpawns_003E5__2 = map.mapPawns.AllPawnsSpawned;
			_003Ci_003E5__3 = 0;
			goto IL_00ec;
			IL_00dc:
			_003Ci_003E5__3++;
			goto IL_00ec;
			IL_00ec:
			if (_003Ci_003E5__3 < _003Cpawns_003E5__2.Count)
			{
				if (_003Cpawns_003E5__2[_003Ci_003E5__3].CurJobDef == JobDefOf.HaulToTransporter && transporters.Contains(((JobDriver_HaulToTransporter)_003Cpawns_003E5__2[_003Ci_003E5__3].jobs.curDriver).Transporter) && _003Cpawns_003E5__2[_003Ci_003E5__3].carryTracker.CarriedThing != null)
				{
					_003C_003E2__current = _003Cpawns_003E5__2[_003Ci_003E5__3].carryTracker.CarriedThing;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00dc;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CThingsBeingHauledTo_003Ed__6 _003CThingsBeingHauledTo_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CThingsBeingHauledTo_003Ed__ = this;
			}
			else
			{
				_003CThingsBeingHauledTo_003Ed__ = new _003CThingsBeingHauledTo_003Ed__6(0);
			}
			_003CThingsBeingHauledTo_003Ed__.transporters = _003C_003E3__transporters;
			_003CThingsBeingHauledTo_003Ed__.map = _003C_003E3__map;
			return _003CThingsBeingHauledTo_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public static void GetTransportersInGroup(int transportersGroup, Map map, List<CompTransporter> outTransporters)
	{
		outTransporters.Clear();
		if (transportersGroup < 0)
		{
			return;
		}
		List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Transporter);
		for (int i = 0; i < list.Count; i++)
		{
			CompTransporter compTransporter = list[i].TryGetComp<CompTransporter>();
			if (compTransporter.groupID == transportersGroup)
			{
				outTransporters.Add(compTransporter);
			}
		}
	}

	public static Lord FindLord(int transportersGroup, Map map)
	{
		List<Lord> lords = map.lordManager.lords;
		for (int i = 0; i < lords.Count; i++)
		{
			if (lords[i].LordJob is LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters && lordJob_LoadAndEnterTransporters.transportersGroup == transportersGroup)
			{
				return lords[i];
			}
		}
		return null;
	}

	public static bool WasLoadingCanceled(Thing transporter)
	{
		CompTransporter compTransporter = transporter.TryGetComp<CompTransporter>();
		if (compTransporter != null && !compTransporter.LoadingInProgressOrReadyToLaunch)
		{
			return true;
		}
		return false;
	}

	public static int InitiateLoading(IEnumerable<CompTransporter> transporters)
	{
		int nextTransporterGroupID = Find.UniqueIDsManager.GetNextTransporterGroupID();
		foreach (CompTransporter transporter in transporters)
		{
			transporter.groupID = nextTransporterGroupID;
		}
		return nextTransporterGroupID;
	}

	[IteratorStateMachine(typeof(_003CAllSendablePawns_003Ed__4))]
	public static IEnumerable<Pawn> AllSendablePawns(List<CompTransporter> transporters, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllSendablePawns_003Ed__4(-2)
		{
			_003C_003E3__transporters = transporters,
			_003C_003E3__map = map
		};
	}

	[IteratorStateMachine(typeof(_003CAllSendableItems_003Ed__5))]
	public static IEnumerable<Thing> AllSendableItems(List<CompTransporter> transporters, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllSendableItems_003Ed__5(-2)
		{
			_003C_003E3__transporters = transporters,
			_003C_003E3__map = map
		};
	}

	[IteratorStateMachine(typeof(_003CThingsBeingHauledTo_003Ed__6))]
	public static IEnumerable<Thing> ThingsBeingHauledTo(List<CompTransporter> transporters, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CThingsBeingHauledTo_003Ed__6(-2)
		{
			_003C_003E3__transporters = transporters,
			_003C_003E3__map = map
		};
	}

	public static void MakeLordsAsAppropriate(List<Pawn> pawns, List<CompTransporter> transporters, Map map)
	{
		int groupID = transporters[0].groupID;
		Lord lord = null;
		IEnumerable<Pawn> enumerable = pawns.Where((Pawn x) => (x.IsColonist || x.IsColonyMechPlayerControlled) && !x.Downed && x.Spawned);
		if (enumerable.Any())
		{
			lord = map.lordManager.lords.Find((Lord x) => x.LordJob is LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters && lordJob_LoadAndEnterTransporters.transportersGroup == groupID);
			if (lord == null)
			{
				lord = LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_LoadAndEnterTransporters(groupID), map);
			}
			foreach (Pawn item in enumerable)
			{
				if (!lord.ownedPawns.Contains(item))
				{
					item.GetLord()?.Notify_PawnLost(item, PawnLostCondition.ForcedToJoinOtherLord, null);
					lord.AddPawn(item);
					item.jobs.EndCurrentJob(JobCondition.InterruptForced);
				}
			}
			for (int num = lord.ownedPawns.Count - 1; num >= 0; num--)
			{
				if (!enumerable.Contains(lord.ownedPawns[num]))
				{
					lord.Notify_PawnLost(lord.ownedPawns[num], PawnLostCondition.NoLongerEnteringTransportPods, null);
				}
			}
		}
		for (int num2 = map.lordManager.lords.Count - 1; num2 >= 0; num2--)
		{
			if (map.lordManager.lords[num2].LordJob is LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters2 && lordJob_LoadAndEnterTransporters2.transportersGroup == groupID && map.lordManager.lords[num2] != lord)
			{
				map.lordManager.RemoveLord(map.lordManager.lords[num2]);
			}
		}
	}

	public static bool IncomingTransporterPreventingMapRemoval(Map map)
	{
		if (ModsConfig.OdysseyActive)
		{
			WorldComponent_GravshipController gravshipController = Find.GravshipController;
			if (gravshipController != null && gravshipController.LandingAreaConfirmationInProgress)
			{
				return true;
			}
			if (Find.CurrentGravship != null && Find.CurrentGravship.destinationTile == map.Tile)
			{
				return true;
			}
		}
		foreach (TravellingTransporters travellingTransporter in Find.WorldObjects.TravellingTransporters)
		{
			if (travellingTransporter.destinationTile == map.Tile)
			{
				return true;
			}
		}
		return false;
	}
}
