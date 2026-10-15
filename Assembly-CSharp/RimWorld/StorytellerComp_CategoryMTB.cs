using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_CategoryMTB : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__2 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_CategoryMTB _003C_003E4__this;

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
		public _003CMakeIntervalIncidents_003Ed__2(int _003C_003E1__state)
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
			StorytellerComp_CategoryMTB storytellerComp_CategoryMTB = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				float num2 = storytellerComp_CategoryMTB.Props.mtbDays;
				if (storytellerComp_CategoryMTB.Props.mtbDaysFactorByDaysPassedCurve != null)
				{
					num2 *= storytellerComp_CategoryMTB.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedSinceSettleFloat);
				}
				if (Rand.MTBEventOccurs(num2, 60000f, 1000f))
				{
					IncidentParms parms = storytellerComp_CategoryMTB.GenerateParms(storytellerComp_CategoryMTB.Props.category, target);
					if (storytellerComp_CategoryMTB.TrySelectRandomIncident(storytellerComp_CategoryMTB.UsableIncidentsInCategory(storytellerComp_CategoryMTB.Props.category, parms), out var foundDef, target))
					{
						_003C_003E2__current = new FiringIncident(foundDef, storytellerComp_CategoryMTB, parms);
						_003C_003E1__state = 1;
						return true;
					}
				}
				break;
			}
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
			_003CMakeIntervalIncidents_003Ed__2 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__2(0)
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

	protected StorytellerCompProperties_CategoryMTB Props => (StorytellerCompProperties_CategoryMTB)props;

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__2))]
	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}

	public override string ToString()
	{
		return $"{base.ToString()} {Props.category}";
	}
}
