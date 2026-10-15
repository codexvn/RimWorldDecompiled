using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_BuildRoof : JobDriver_AffectRoof
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BuildRoof _003C_003E4__this;

		private IEnumerator<Toil> _003C_003E7__wrap1;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__2(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				JobDriver_BuildRoof CS_0024_003C_003E8__locals10 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals10.FailOn(() => !CS_0024_003C_003E8__locals10.Map.areaManager.BuildRoof[CS_0024_003C_003E8__locals10.Cell]);
					CS_0024_003C_003E8__locals10.FailOn(() => !RoofCollapseUtility.WithinRangeOfRoofHolder(CS_0024_003C_003E8__locals10.Cell, CS_0024_003C_003E8__locals10.Map));
					CS_0024_003C_003E8__locals10.FailOn(() => !RoofCollapseUtility.ConnectedToRoofHolder(CS_0024_003C_003E8__locals10.Cell, CS_0024_003C_003E8__locals10.Map, assumeRoofAtRoot: true));
					_003C_003E7__wrap1 = ((JobDriver_AffectRoof)CS_0024_003C_003E8__locals10).MakeNewToils().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Toil current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	protected override PathEndMode PathEndMode => PathEndMode.Touch;

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__2))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected override void DoEffect()
	{
		for (int i = 0; i < 9; i++)
		{
			IntVec3 intVec = base.Cell + GenAdj.AdjacentCellsAndInside[i];
			if (!intVec.InBounds(base.Map) || !base.Map.areaManager.BuildRoof[intVec] || intVec.Roofed(base.Map) || !RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, base.Map) || RoofUtility.FirstBlockingThing(intVec, base.Map) != null)
			{
				continue;
			}
			base.Map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
			MoteMaker.PlaceTempRoof(intVec, base.Map);
			List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
			for (int j = 0; j < list.Count; j++)
			{
				Thing thing = list[j];
				if (thing.def.building != null && thing.def.building.IsMortar && thing.TryGetComp(out CompWakeUpDormant comp))
				{
					comp.Activate(pawn);
				}
			}
		}
	}

	protected override bool DoWorkFailOn()
	{
		return base.Cell.Roofed(base.Map);
	}
}
