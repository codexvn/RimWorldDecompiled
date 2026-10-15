using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_PaintBuilding : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkThingsGlobal_003Ed__3 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<Designation> _003C_003E7__wrap1;

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
		public _003CPotentialWorkThingsGlobal_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
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
					_003C_003E7__wrap1 = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.PaintBuilding).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Designation current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.target.Thing;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPotentialWorkThingsGlobal_003Ed__3 _003CPotentialWorkThingsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkThingsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkThingsGlobal_003Ed__ = new _003CPotentialWorkThingsGlobal_003Ed__3(0);
			}
			_003CPotentialWorkThingsGlobal_003Ed__.pawn = _003C_003E3__pawn;
			return _003CPotentialWorkThingsGlobal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private static List<Thing> tmpDye = new List<Thing>();

	public override PathEndMode PathEndMode => PathEndMode.Touch;

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.PaintBuilding);
	}

	[IteratorStateMachine(typeof(_003CPotentialWorkThingsGlobal_003Ed__3))]
	public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkThingsGlobal_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		AcceptanceReport acceptanceReport = ShouldPaintThing(pawn, t, forced, checkDye: true);
		if (!acceptanceReport)
		{
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				JobFailReason.Is(acceptanceReport.Reason);
			}
			return false;
		}
		return true;
	}

	public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		tmpDye.Clear();
		tmpDye = PaintUtility.FindNearbyDyes(pawn, forced);
		int stackCountFromThingList = ThingUtility.GetStackCountFromThingList(tmpDye);
		if (!tmpDye.Any())
		{
			return null;
		}
		tmpDye.SortBy((Thing x) => x.Position.DistanceToSquared(t.Position));
		int num = 0;
		Job job = JobMaker.MakeJob(JobDefOf.PaintBuilding);
		job.AddQueuedTarget(TargetIndex.A, t);
		job.AddQueuedTarget(TargetIndex.B, tmpDye[num]);
		job.countQueue = new List<int> { 1 };
		int num2 = Mathf.Min(10, stackCountFromThingList);
		for (int i = 0; i < 100; i++)
		{
			IntVec3 intVec = t.Position + GenRadial.RadialPattern[i];
			if (!intVec.InBounds(t.Map) || intVec.Fogged(t.Map) || !pawn.CanReach(intVec, PathEndMode.Touch, Danger.Deadly))
			{
				continue;
			}
			List<Thing> thingList = intVec.GetThingList(t.Map);
			for (int j = 0; j < thingList.Count; j++)
			{
				Thing thing = thingList[j];
				if (thing == t || !ShouldPaintThing(pawn, thing, forced, checkDye: false) || job.targetQueueA.Contains(thing))
				{
					continue;
				}
				job.AddQueuedTarget(TargetIndex.A, thing);
				job.countQueue[0]++;
				if (job.countQueue[0] >= tmpDye[num].stackCount)
				{
					num++;
					if (num >= tmpDye.Count)
					{
						break;
					}
					job.AddQueuedTarget(TargetIndex.B, tmpDye[num]);
				}
			}
			if (job.GetTargetQueue(TargetIndex.A).Count >= num2 || num >= tmpDye.Count)
			{
				break;
			}
		}
		if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
		{
			job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
		}
		return job;
	}

	private AcceptanceReport ShouldPaintThing(Pawn pawn, Thing t, bool forced, bool checkDye)
	{
		if (t.def.building == null || !t.def.building.paintable)
		{
			return false;
		}
		Designation designation = pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.PaintBuilding);
		if (designation?.colorDef == null)
		{
			return false;
		}
		if (t is Building building && building.PaintColorDef == designation.colorDef)
		{
			return false;
		}
		if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.PaintBuilding) == null)
		{
			return false;
		}
		if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
		{
			return false;
		}
		if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RemovePaintBuilding) != null)
		{
			return false;
		}
		if (t.IsBurning())
		{
			return false;
		}
		if (!pawn.CanReserve(t, 1, -1, null, forced))
		{
			return false;
		}
		if (checkDye)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].IsForbidden(pawn) && pawn.CanReserveAndReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, 1, 1, null, forced))
				{
					return true;
				}
			}
			return "NoIngredient".Translate(ThingDefOf.Dye);
		}
		return true;
	}
}
