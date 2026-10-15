using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_MechanitorComplexQuest : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__4 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_MechanitorComplexQuest _003C_003E4__this;

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
			StorytellerComp_MechanitorComplexQuest storytellerComp_MechanitorComplexQuest = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (Find.History.mechanoidDatacoreOpportunityAvailable && !Find.History.mechanoidDatacoreReadOrLost)
				{
					return false;
				}
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				if (!storytellerComp_MechanitorComplexQuest.Props.blockedByQueuedOrActiveQuests.NullOrEmpty())
				{
					for (int i = 0; i < questsListForReading.Count; i++)
					{
						if (storytellerComp_MechanitorComplexQuest.Props.blockedByQueuedOrActiveQuests.Contains(questsListForReading[i].root) && (questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing))
						{
							return false;
						}
					}
				}
				int num2 = -1;
				for (int j = 0; j < questsListForReading.Count; j++)
				{
					if (questsListForReading[j].root == storytellerComp_MechanitorComplexQuest.Props.incident.questScriptDef && questsListForReading[j].cleanupTick > num2)
					{
						num2 = questsListForReading[j].cleanupTick;
					}
				}
				if (num2 > 0 && (Find.TickManager.TicksGame - num2).TicksToDays() < storytellerComp_MechanitorComplexQuest.Props.minSpacingDays)
				{
					return false;
				}
				float num3 = storytellerComp_MechanitorComplexQuest.Props.mtbDays;
				if (storytellerComp_MechanitorComplexQuest.Props.existingMechanitorOrMechlinkMTBFactor != 1f && storytellerComp_MechanitorComplexQuest.ExistingMechanitorOrMechlink)
				{
					num3 *= storytellerComp_MechanitorComplexQuest.Props.existingMechanitorOrMechlinkMTBFactor;
				}
				if (Rand.MTBEventOccurs(num3, 60000f, 1000f))
				{
					IncidentParms parms = storytellerComp_MechanitorComplexQuest.GenerateParms(storytellerComp_MechanitorComplexQuest.Props.incident.category, target);
					if (storytellerComp_MechanitorComplexQuest.Props.incident.Worker.CanFireNow(parms))
					{
						_003C_003E2__current = new FiringIncident(storytellerComp_MechanitorComplexQuest.Props.incident, storytellerComp_MechanitorComplexQuest, parms);
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

	private StorytellerCompProperties_MechanitorComplexQuest Props => (StorytellerCompProperties_MechanitorComplexQuest)props;

	private bool ExistingMechanitorOrMechlink
	{
		get
		{
			if (!MechanitorUtility.AnyMechanitorInPlayerFaction())
			{
				return MechanitorUtility.AnyMechlinkInMap();
			}
			return true;
		}
	}

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
