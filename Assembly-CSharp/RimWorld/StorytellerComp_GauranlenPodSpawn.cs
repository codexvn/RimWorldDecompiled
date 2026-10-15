using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_GauranlenPodSpawn : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__2 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_GauranlenPodSpawn _003C_003E4__this;

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
			StorytellerComp_GauranlenPodSpawn storytellerComp_GauranlenPodSpawn = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ModsConfig.IdeologyActive || storytellerComp_GauranlenPodSpawn.Props.daysBetweenPodSpawns == 0f || Faction.OfPlayer.ideos == null)
				{
					return false;
				}
				if ((float)GenDate.DaysPassed < storytellerComp_GauranlenPodSpawn.Props.minDaysPassed)
				{
					return false;
				}
				int num2 = 1;
				if (storytellerComp_GauranlenPodSpawn.Props.countdownFactorAnyConnectors > 1)
				{
					foreach (Ideo allIdeo in Faction.OfPlayer.ideos.AllIdeos)
					{
						if (allIdeo.HasMeme(MemeDefOf.TreeConnection))
						{
							num2 = storytellerComp_GauranlenPodSpawn.Props.countdownFactorAnyConnectors;
							break;
						}
					}
				}
				Find.IdeoManager.ticksToNextGauranlenSpawn -= 1000 * num2;
				if (Find.IdeoManager.ticksToNextGauranlenSpawn <= 0)
				{
					Find.IdeoManager.ticksToNextGauranlenSpawn = (int)(storytellerComp_GauranlenPodSpawn.Props.daysBetweenPodSpawns * 60000f);
					_003C_003E2__current = new FiringIncident(storytellerComp_GauranlenPodSpawn.Props.incident, storytellerComp_GauranlenPodSpawn, storytellerComp_GauranlenPodSpawn.GenerateParms(storytellerComp_GauranlenPodSpawn.Props.incident.category, target));
					_003C_003E1__state = 1;
					return true;
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

	protected StorytellerCompProperties_GauranlenPodSpawn Props => (StorytellerCompProperties_GauranlenPodSpawn)props;

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
}
