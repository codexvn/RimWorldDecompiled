using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_SingleOnceFixed : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__4 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_SingleOnceFixed _003C_003E4__this;

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
		public _003CMakeIntervalIncidents_003Ed__4(int _003C_003E1__state)
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
			StorytellerComp_SingleOnceFixed storytellerComp_SingleOnceFixed = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				int num2 = storytellerComp_SingleOnceFixed.IntervalsPassed;
				if (storytellerComp_SingleOnceFixed.Props.minColonistCount > 0)
				{
					if (target.StoryState.lastFireTicks.ContainsKey(storytellerComp_SingleOnceFixed.Props.incident))
					{
						return false;
					}
					if (PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count < storytellerComp_SingleOnceFixed.Props.minColonistCount)
					{
						return false;
					}
					num2 -= target.StoryState.GetTicksFromColonistCount(storytellerComp_SingleOnceFixed.Props.minColonistCount) / 1000;
				}
				if (num2 != storytellerComp_SingleOnceFixed.Props.fireAfterDaysPassed * 60)
				{
					break;
				}
				if (storytellerComp_SingleOnceFixed.Props.skipIfColonistHasMinTitle != null)
				{
					List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
					for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count; i++)
					{
						if (allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].royalty != null && allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].royalty.AllTitlesForReading.Any() && allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[i].royalty.MainTitle().seniority >= storytellerComp_SingleOnceFixed.Props.skipIfColonistHasMinTitle.seniority)
						{
							return false;
						}
					}
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (!storytellerComp_SingleOnceFixed.Props.skipIfOnExtremeBiome || (anyPlayerHomeMap != null && !anyPlayerHomeMap.Biome.isExtremeBiome))
				{
					IncidentDef incident = storytellerComp_SingleOnceFixed.Props.incident;
					if (incident.TargetAllowed(target))
					{
						FiringIncident firingIncident = new FiringIncident(incident, storytellerComp_SingleOnceFixed, storytellerComp_SingleOnceFixed.GenerateParms(incident.category, target));
						_003C_003E2__current = firingIncident;
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
			_003CMakeIntervalIncidents_003Ed__4 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__4(0)
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

	protected int IntervalsPassed => Find.TickManager.TicksGame / 1000;

	private StorytellerCompProperties_SingleOnceFixed Props => (StorytellerCompProperties_SingleOnceFixed)props;

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__4))]
	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}
}
