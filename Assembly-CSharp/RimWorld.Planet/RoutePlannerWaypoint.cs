using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld.Planet;

public class RoutePlannerWaypoint : WorldObject
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__3 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoutePlannerWaypoint _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__3(int _003C_003E1__state)
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
				RoutePlannerWaypoint CS_0024_003C_003E8__locals2 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldObject)CS_0024_003C_003E8__locals2).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandRemoveWaypointLabel".Translate(),
						defaultDesc = "CommandRemoveWaypointDesc".Translate(),
						icon = TexCommand.RemoveRoutePlannerWaypoint,
						action = delegate
						{
							Find.WorldRoutePlanner.TryRemoveWaypoint(CS_0024_003C_003E8__locals2);
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__3(0)
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

	public override string Label
	{
		get
		{
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			if (worldRoutePlanner.Active)
			{
				int num = worldRoutePlanner.waypoints.IndexOf(this);
				if (num >= 0)
				{
					return base.Label + " " + (num + 1);
				}
			}
			return base.Label;
		}
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
		if (worldRoutePlanner.Active)
		{
			int num = worldRoutePlanner.waypoints.IndexOf(this);
			if (num >= 1)
			{
				int ticksToWaypoint = worldRoutePlanner.GetTicksToWaypoint(num);
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("EstimatedTimeToWaypoint".Translate(ticksToWaypoint.ToStringTicksToDays("0.#")));
				if (num >= 2)
				{
					int ticksToWaypoint2 = worldRoutePlanner.GetTicksToWaypoint(num - 1);
					stringBuilder.AppendLine();
					stringBuilder.Append("EstimatedTimeToWaypointFromPrevious".Translate((ticksToWaypoint - ticksToWaypoint2).ToStringTicksToDays("0.#")));
				}
			}
		}
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__3))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}
}
