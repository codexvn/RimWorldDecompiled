using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public static class GiveItemsToPawnUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public Thing reachableThing;

		public Pawn requester;

		public Pawn current;

		internal void _003CGetFloatMenuOptionsForPawn_003Eb__0()
		{
			Job job = JobMaker.MakeJob(JobDefOf.GiveToPawn, reachableThing, requester);
			job.haulMode = HaulMode.ToContainer;
			job.lord = requester.GetLord();
			current.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptionsForPawn_003Ed__5 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn requester;

		public Pawn _003C_003E3__requester;

		private Pawn current;

		public Pawn _003C_003E3__current;

		private ThingDef requestedThingDef;

		public ThingDef _003C_003E3__requestedThingDef;

		private int countRequired;

		public int _003C_003E3__countRequired;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptionsForPawn_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass5_0 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass5_0();
				CS_0024_003C_003E8__locals13.requester = requester;
				CS_0024_003C_003E8__locals13.current = current;
				CS_0024_003C_003E8__locals13.reachableThing = FindItemToGive(CS_0024_003C_003E8__locals13.current, requestedThingDef);
				string text = $"{GetCountRemaining(CS_0024_003C_003E8__locals13.requester, requestedThingDef, countRequired)}x {requestedThingDef.label}";
				if (CS_0024_003C_003E8__locals13.reachableThing != null)
				{
					TaggedString taggedString = "GiveItemsTo".Translate(text, CS_0024_003C_003E8__locals13.requester);
					_003C_003E2__current = new FloatMenuOption(taggedString, delegate
					{
						Job job = JobMaker.MakeJob(JobDefOf.GiveToPawn, CS_0024_003C_003E8__locals13.reachableThing, CS_0024_003C_003E8__locals13.requester);
						job.haulMode = HaulMode.ToContainer;
						job.lord = CS_0024_003C_003E8__locals13.requester.GetLord();
						CS_0024_003C_003E8__locals13.current.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					}, MenuOptionPriority.High, null, CS_0024_003C_003E8__locals13.requester);
					_003C_003E1__state = 1;
					return true;
				}
				TaggedString taggedString2 = "CannotGiveItemsTo".Translate(text, CS_0024_003C_003E8__locals13.requester) + ": " + "NoItemFound".Translate();
				_003C_003E2__current = new FloatMenuOption(taggedString2, null);
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptionsForPawn_003Ed__5 _003CGetFloatMenuOptionsForPawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptionsForPawn_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptionsForPawn_003Ed__ = new _003CGetFloatMenuOptionsForPawn_003Ed__5(0);
			}
			_003CGetFloatMenuOptionsForPawn_003Ed__.requester = _003C_003E3__requester;
			_003CGetFloatMenuOptionsForPawn_003Ed__.current = _003C_003E3__current;
			_003CGetFloatMenuOptionsForPawn_003Ed__.requestedThingDef = _003C_003E3__requestedThingDef;
			_003CGetFloatMenuOptionsForPawn_003Ed__.countRequired = _003C_003E3__countRequired;
			return _003CGetFloatMenuOptionsForPawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public static Thing FindItemToGive(Pawn hauler, ThingDef thingDef)
	{
		return GenClosest.ClosestThingReachable(hauler.Position, hauler.Map, ThingRequest.ForDef(thingDef), PathEndMode.Touch, TraverseParms.For(hauler), 9999f, (Thing x) => !x.IsForbidden(hauler) && hauler.CanReserve(x));
	}

	public static bool IsWaitingForItems(Pawn pawn)
	{
		Lord lord = pawn.GetLord();
		if (lord != null)
		{
			return lord.CurLordToil is IWaitForItemsLordToil;
		}
		return false;
	}

	public static int ItemCountWaitingFor(Pawn pawn)
	{
		if (!IsWaitingForItems(pawn))
		{
			return -1;
		}
		return ((IWaitForItemsLordToil)pawn.GetLord().CurLordToil).CountRemaining;
	}

	public static int ItemCountBeingHauled(Pawn requester)
	{
		IReadOnlyList<Pawn> allPawnsSpawned = requester.Map.mapPawns.AllPawnsSpawned;
		Lord lord = requester.GetLord();
		int num = 0;
		for (int i = 0; i < allPawnsSpawned.Count; i++)
		{
			Pawn pawn = allPawnsSpawned[i];
			if (pawn.CurJob != null && pawn.jobs.curDriver is JobDriver_GiveToPawn jobDriver_GiveToPawn && jobDriver_GiveToPawn.job.lord == lord)
			{
				num += jobDriver_GiveToPawn.CountBeingHauled;
			}
		}
		return num;
	}

	public static int ItemCountLeftToCollect(Pawn requester)
	{
		return ItemCountWaitingFor(requester) - ItemCountBeingHauled(requester);
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptionsForPawn_003Ed__5))]
	public static IEnumerable<FloatMenuOption> GetFloatMenuOptionsForPawn(Pawn requester, Pawn current, ThingDef requestedThingDef, int countRequired)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptionsForPawn_003Ed__5(-2)
		{
			_003C_003E3__requester = requester,
			_003C_003E3__current = current,
			_003C_003E3__requestedThingDef = requestedThingDef,
			_003C_003E3__countRequired = countRequired
		};
	}

	public static int GetCountRemaining(Pawn pawn, ThingDef requestedThingDef, int requestedThingCount)
	{
		int num = 0;
		ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
		for (int i = 0; i < innerContainer.Count; i++)
		{
			if (innerContainer[i].def == requestedThingDef)
			{
				num += innerContainer[i].stackCount;
			}
		}
		return requestedThingCount - num;
	}
}
