using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_PaintFloor : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkCellsGlobal_003Ed__3 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<Designation> _003C_003E7__wrap1;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CPotentialWorkCellsGlobal_003Ed__3(int _003C_003E1__state)
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
					_003C_003E7__wrap1 = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.PaintFloor).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Designation current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.target.Cell;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CPotentialWorkCellsGlobal_003Ed__3 _003CPotentialWorkCellsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkCellsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkCellsGlobal_003Ed__ = new _003CPotentialWorkCellsGlobal_003Ed__3(0);
			}
			_003CPotentialWorkCellsGlobal_003Ed__.pawn = _003C_003E3__pawn;
			return _003CPotentialWorkCellsGlobal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private static List<Thing> tmpDye = new List<Thing>();

	public override PathEndMode PathEndMode => PathEndMode.Touch;

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.PaintFloor);
	}

	[IteratorStateMachine(typeof(_003CPotentialWorkCellsGlobal_003Ed__3))]
	public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkCellsGlobal_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
	{
		AcceptanceReport acceptanceReport = ShouldPaintCell(pawn, c, forced, checkDye: true);
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

	public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
	{
		tmpDye.Clear();
		tmpDye = PaintUtility.FindNearbyDyes(pawn, forced);
		int stackCountFromThingList = ThingUtility.GetStackCountFromThingList(tmpDye);
		if (!tmpDye.Any())
		{
			return null;
		}
		tmpDye.SortBy((Thing x) => x.Position.DistanceToSquared(cell));
		int num = 0;
		Job job = JobMaker.MakeJob(JobDefOf.PaintFloor);
		job.AddQueuedTarget(TargetIndex.A, cell);
		job.AddQueuedTarget(TargetIndex.B, tmpDye[num]);
		job.countQueue = new List<int> { 1 };
		int num2 = Mathf.Min(10, stackCountFromThingList);
		for (int i = 0; i < 100; i++)
		{
			IntVec3 intVec = cell + GenRadial.RadialPattern[i];
			if (!intVec.InBounds(pawn.Map) || intVec.Fogged(pawn.Map) || !pawn.CanReach(intVec, PathEndMode.Touch, Danger.Deadly))
			{
				continue;
			}
			if ((bool)ShouldPaintCell(pawn, intVec, forced, checkDye: false))
			{
				if (job.targetQueueA.Contains(intVec))
				{
					continue;
				}
				job.AddQueuedTarget(TargetIndex.A, intVec);
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
			if (job.GetTargetQueue(TargetIndex.A).Count >= num2)
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

	private AcceptanceReport ShouldPaintCell(Pawn pawn, IntVec3 c, bool forced, bool checkDye)
	{
		if (!pawn.Map.terrainGrid.TerrainAt(c).isPaintable)
		{
			return false;
		}
		Designation designation = pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor);
		if (designation?.colorDef == null)
		{
			return false;
		}
		if (pawn.Map.terrainGrid.ColorAt(c) == designation.colorDef)
		{
			return false;
		}
		if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor) == null)
		{
			return false;
		}
		if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
		{
			return false;
		}
		if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemovePaintFloor) != null)
		{
			return false;
		}
		if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced))
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
