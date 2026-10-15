using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RimWorld;

public class ScenPart_DisableIncident : ScenPart_IncidentBase
{
	[CompilerGenerated]
	private sealed class _003CRandomizableIncidents_003Ed__2 : IEnumerable<IncidentDef>, IEnumerable, IEnumerator<IncidentDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IncidentDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		IncidentDef IEnumerator<IncidentDef>.Current
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
		public _003CRandomizableIncidents_003Ed__2(int _003C_003E1__state)
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
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.TraderCaravanArrival;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.OrbitalTraderArrival;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.WandererJoin;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.Eclipse;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.ToxicFallout;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = IncidentDefOf.SolarFlare;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<IncidentDef> IEnumerable<IncidentDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CRandomizableIncidents_003Ed__2(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IncidentDef>)this).GetEnumerator();
		}
	}

	protected override string IncidentTag => "DisableIncident";

	[IteratorStateMachine(typeof(_003CRandomizableIncidents_003Ed__2))]
	protected override IEnumerable<IncidentDef> RandomizableIncidents()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRandomizableIncidents_003Ed__2(-2);
	}
}
