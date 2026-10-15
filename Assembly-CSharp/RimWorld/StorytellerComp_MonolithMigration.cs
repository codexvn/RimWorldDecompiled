using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_MonolithMigration : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__2 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_MonolithMigration _003C_003E4__this;

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
			StorytellerComp_MonolithMigration storytellerComp_MonolithMigration = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				if (Find.Anomaly.monolith != null)
				{
					return false;
				}
				if (Find.Anomaly.LevelDef == MonolithLevelDefOf.Disrupted)
				{
					return false;
				}
				if (Find.QuestManager.QuestsListForReading.Any((Quest q) => q.root == QuestScriptDefOf.MonolithMigration && (q.State == QuestState.Ongoing || q.State == QuestState.NotYetAccepted)))
				{
					return false;
				}
				if (Rand.MTBEventOccurs(DebugSettings.fastMonolithRespawn ? 0.1f : storytellerComp_MonolithMigration.Props.mtbDays, 60000f, 1000f))
				{
					_003C_003E2__current = new FiringIncident(storytellerComp_MonolithMigration.Props.incident, storytellerComp_MonolithMigration, storytellerComp_MonolithMigration.GenerateParms(IncidentCategoryDefOf.Misc, target));
					_003C_003E1__state = 1;
					return true;
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

	protected StorytellerCompProperties_MonolithMigration Props => (StorytellerCompProperties_MonolithMigration)props;

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
