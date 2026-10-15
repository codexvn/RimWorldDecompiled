using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class GeneResourceDrainUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public IGeneResourceDrain drain;

		internal void _003CGetResourceDrainGizmos_003Eb__0()
		{
			OffsetResource(drain, -0.1f);
		}

		internal void _003CGetResourceDrainGizmos_003Eb__1()
		{
			OffsetResource(drain, 0.1f);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetResourceDrainGizmos_003Ed__3 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IGeneResourceDrain drain;

		public IGeneResourceDrain _003C_003E3__drain;

		private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

		private Gene_Resource _003Cresource_003E5__2;

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
		public _003CGetResourceDrainGizmos_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003Cresource_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass3_0();
				_003C_003E8__1.drain = drain;
				if (DebugSettings.ShowDevGizmos && _003C_003E8__1.drain.Resource != null)
				{
					_003Cresource_003E5__2 = _003C_003E8__1.drain.Resource;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: " + _003Cresource_003E5__2.ResourceLabel + " -10",
						action = delegate
						{
							OffsetResource(_003C_003E8__1.drain, -0.1f);
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: " + _003Cresource_003E5__2.ResourceLabel + " +10",
					action = delegate
					{
						OffsetResource(_003C_003E8__1.drain, 0.1f);
					}
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Cresource_003E5__2 = null;
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
			_003CGetResourceDrainGizmos_003Ed__3 _003CGetResourceDrainGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetResourceDrainGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetResourceDrainGizmos_003Ed__ = new _003CGetResourceDrainGizmos_003Ed__3(0);
			}
			_003CGetResourceDrainGizmos_003Ed__.drain = _003C_003E3__drain;
			return _003CGetResourceDrainGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public static void TickResourceDrainInterval(IGeneResourceDrain drain, int delta)
	{
		if (drain.CanOffset && drain.Resource != null)
		{
			OffsetResource(drain, (0f - drain.ResourceLossPerDay) * (float)delta / 60000f);
		}
	}

	public static void PostResourceOffset(IGeneResourceDrain drain, float oldValue)
	{
		if (oldValue > 0f && drain.Resource.Value <= 0f)
		{
			Pawn pawn = drain.Pawn;
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.HemogenCraving))
			{
				pawn.health.AddHediff(HediffDefOf.HemogenCraving, null, null);
			}
		}
	}

	public static void OffsetResource(IGeneResourceDrain drain, float amnt)
	{
		if (drain.Resource != null)
		{
			float value = drain.Resource.Value;
			drain.Resource.Value += amnt;
			PostResourceOffset(drain, value);
		}
	}

	[IteratorStateMachine(typeof(_003CGetResourceDrainGizmos_003Ed__3))]
	public static IEnumerable<Gizmo> GetResourceDrainGizmos(IGeneResourceDrain drain)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetResourceDrainGizmos_003Ed__3(-2)
		{
			_003C_003E3__drain = drain
		};
	}
}
