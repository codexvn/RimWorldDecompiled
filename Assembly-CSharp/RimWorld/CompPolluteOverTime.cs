using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompPolluteOverTime : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPolluteOverTime _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__7(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			CompPolluteOverTime @object = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Pollute",
						action = @object.Pollute
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private CompProperties_PolluteOverTime Props => (CompProperties_PolluteOverTime)props;

	private int TicksToPolluteCell => 60000 / Props.cellsToPollutePerDay;

	public override void CompTick()
	{
		if (parent.Spawned && parent.IsHashIntervalTick(TicksToPolluteCell))
		{
			Pollute();
		}
	}

	private void Pollute()
	{
		if (!ModsConfig.BiotechActive)
		{
			return;
		}
		int num = GenRadial.NumCellsInRadius(GenRadial.MaxRadialPatternRadius - 1f);
		for (int i = 0; i < num; i++)
		{
			IntVec3 intVec = parent.Position + GenRadial.RadialPattern[i];
			if (!intVec.IsPolluted(parent.Map) && intVec.CanPollute(parent.Map))
			{
				intVec.Pollute(parent.Map);
				parent.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(intVec, parent.Map, Vector3.zero), intVec, 45);
				break;
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		return "TilePollution".Translate() + ": " + "CellsPerDay".Translate(Props.cellsToPollutePerDay);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__7))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
