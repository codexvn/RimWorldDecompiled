using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_DeepDrillInfestation : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__5 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		public StorytellerComp_DeepDrillInfestation _003C_003E4__this;

		private float _003Cmtb_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CMakeIntervalIncidents_003Ed__5(int _003C_003E1__state)
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
			StorytellerComp_DeepDrillInfestation storytellerComp_DeepDrillInfestation = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00ce;
			}
			_003C_003E1__state = -1;
			Map map = (Map)target;
			tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, tmpDrills);
			if (!tmpDrills.Any())
			{
				return false;
			}
			_003Cmtb_003E5__2 = storytellerComp_DeepDrillInfestation.DeepDrillInfestationMTBDaysPerDrill;
			if (_003Cmtb_003E5__2 < 0f)
			{
				return false;
			}
			_003Ci_003E5__3 = 0;
			goto IL_00e0;
			IL_00e0:
			if (_003Ci_003E5__3 < tmpDrills.Count)
			{
				if (Rand.MTBEventOccurs(_003Cmtb_003E5__2, 60000f, 1000f))
				{
					IncidentParms parms = storytellerComp_DeepDrillInfestation.GenerateParms(IncidentCategoryDefOf.DeepDrillInfestation, target);
					if (storytellerComp_DeepDrillInfestation.UsableIncidentsInCategory(IncidentCategoryDefOf.DeepDrillInfestation, parms).TryRandomElement(out var result))
					{
						_003C_003E2__current = new FiringIncident(result, storytellerComp_DeepDrillInfestation, parms);
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_00ce;
			}
			return false;
			IL_00ce:
			_003Ci_003E5__3++;
			goto IL_00e0;
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
			_003CMakeIntervalIncidents_003Ed__5 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__5(0)
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

	private static List<Thing> tmpDrills = new List<Thing>();

	protected StorytellerCompProperties_DeepDrillInfestation Props => (StorytellerCompProperties_DeepDrillInfestation)props;

	private float DeepDrillInfestationMTBDaysPerDrill
	{
		get
		{
			Difficulty difficulty = Find.Storyteller.difficulty;
			if (difficulty.deepDrillInfestationChanceFactor <= 0f)
			{
				return -1f;
			}
			return Props.baseMtbDaysPerDrill / difficulty.deepDrillInfestationChanceFactor;
		}
	}

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__5))]
	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}
}
