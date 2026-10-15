using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_RefiringUniqueQuest : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__5 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_RefiringUniqueQuest _003C_003E4__this;

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
			StorytellerComp_RefiringUniqueQuest storytellerComp_RefiringUniqueQuest = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!storytellerComp_RefiringUniqueQuest.Props.incident.TargetAllowed(target))
				{
					return false;
				}
				Quest quest = null;
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (questsListForReading[i].root == storytellerComp_RefiringUniqueQuest.Props.incident.questScriptDef && (quest == null || questsListForReading[i].appearanceTick > quest.appearanceTick))
					{
						quest = questsListForReading[i];
						break;
					}
				}
				if ((quest == null) ? ((storytellerComp_RefiringUniqueQuest.Props.minColonyWealth <= 0 || !(WealthUtility.PlayerWealth < (float)storytellerComp_RefiringUniqueQuest.Props.minColonyWealth)) && ((!storytellerComp_RefiringUniqueQuest.generateSkipped) ? (storytellerComp_RefiringUniqueQuest.IntervalsPassed == (int)(storytellerComp_RefiringUniqueQuest.Props.minDaysPassed * 60f) + 1) : ((float)GenTicks.TicksGame >= storytellerComp_RefiringUniqueQuest.Props.minDaysPassed * 60000f))) : (storytellerComp_RefiringUniqueQuest.Props.refireEveryDays >= 0f && ((quest.State != QuestState.EndedSuccess && quest.State != QuestState.Ongoing && quest.State != 0 && quest.cleanupTick >= 0 && storytellerComp_RefiringUniqueQuest.IntervalsPassed == (int)((float)quest.cleanupTick + storytellerComp_RefiringUniqueQuest.Props.refireEveryDays * 60000f) / 1000) ? true : false)))
				{
					IncidentParms parms = storytellerComp_RefiringUniqueQuest.GenerateParms(storytellerComp_RefiringUniqueQuest.Props.incident.category, target);
					if (storytellerComp_RefiringUniqueQuest.Props.incident.Worker.CanFireNow(parms))
					{
						_003C_003E2__current = new FiringIncident(storytellerComp_RefiringUniqueQuest.Props.incident, storytellerComp_RefiringUniqueQuest, parms);
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

	private bool generateSkipped;

	private int IntervalsPassed => Find.TickManager.TicksGame / 1000;

	private StorytellerCompProperties_RefiringUniqueQuest Props => (StorytellerCompProperties_RefiringUniqueQuest)props;

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

	public override void Initialize()
	{
		base.Initialize();
		if ((float)GenTicks.TicksGame >= Props.minDaysPassed * 60000f)
		{
			generateSkipped = true;
		}
	}

	public override string ToString()
	{
		return base.ToString() + " " + Props.incident;
	}
}
