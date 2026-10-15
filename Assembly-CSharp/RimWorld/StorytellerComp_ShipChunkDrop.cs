using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_ShipChunkDrop : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__3 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_ShipChunkDrop _003C_003E4__this;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		FiringIncident IEnumerator<FiringIncident>.Current
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
		public _003CMakeIntervalIncidents_003Ed__3(int _003C_003E1__state)
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
			StorytellerComp_ShipChunkDrop storytellerComp_ShipChunkDrop = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (Rand.MTBEventOccurs(storytellerComp_ShipChunkDrop.ShipChunkDropMTBDays, 60000f, 1000f))
				{
					IncidentDef shipChunkDrop = IncidentDefOf.ShipChunkDrop;
					IncidentParms parms = storytellerComp_ShipChunkDrop.GenerateParms(shipChunkDrop.category, target);
					if (shipChunkDrop.Worker.CanFireNow(parms))
					{
						_003C_003E2__current = new FiringIncident(shipChunkDrop, storytellerComp_ShipChunkDrop, parms);
						_003C_003E1__state = 1;
						return true;
					}
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
		IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
		{
			_003CMakeIntervalIncidents_003Ed__3 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CMakeIntervalIncidents_003Ed__.target = _003C_003E3__target;
			return _003CMakeIntervalIncidents_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FiringIncident>)this).GetEnumerator();
		}
	}

	private static readonly SimpleCurve ShipChunkDropMTBDaysCurve = new SimpleCurve
	{
		new CurvePoint(0f, 20f),
		new CurvePoint(1f, 40f),
		new CurvePoint(2f, 80f),
		new CurvePoint(2.75f, 135f)
	};

	private float ShipChunkDropMTBDays
	{
		get
		{
			float x = (float)Find.TickManager.TicksGame / 3600000f;
			return ShipChunkDropMTBDaysCurve.Evaluate(x);
		}
	}

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__3))]
	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__3(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}
}
