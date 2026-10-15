using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_Incident : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__8 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Incident _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_Incident questPart_Incident = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Incident).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_Incident.mapParent != null)
					{
						_003C_003E2__current = questPart_Incident.mapParent;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public string inSignal;

	public IncidentDef incident;

	private IncidentParms incidentParms;

	private MapParent mapParent;

	public MapParent MapParent
	{
		get
		{
			return mapParent;
		}
		set
		{
			mapParent = value;
		}
	}

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!(signal.tag == inSignal) || incidentParms == null)
		{
			return;
		}
		if (!incidentParms.forced)
		{
			Log.Error("QuestPart incident should always be forced but it's not. incident=" + incident);
			incidentParms.forced = true;
		}
		incidentParms.quest = quest;
		if (incidentParms.target == null && (mapParent == null || !mapParent.HasMap || !quest.IsParentSuitableForQuest(mapParent)))
		{
			mapParent = quest.TryFindNewSuitableMapParentForRetarget() ?? Find.AnyPlayerHomeMap?.Parent;
		}
		if (mapParent != null)
		{
			if (mapParent.HasMap)
			{
				incidentParms.target = mapParent.Map;
				if (incident.Worker.CanFireNow(incidentParms))
				{
					incident.Worker.TryExecute(incidentParms);
				}
				incidentParms.target = null;
			}
		}
		else if (incidentParms.target != null && incident.Worker.CanFireNow(incidentParms))
		{
			incident.Worker.TryExecute(incidentParms);
		}
	}

	public void SetIncidentParmsAndRemoveTarget(IncidentParms value)
	{
		incidentParms = value;
		if (incidentParms.target is Map map)
		{
			mapParent = map.Parent;
			incidentParms.target = null;
		}
		else
		{
			mapParent = null;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Defs.Look(ref incident, "incident");
		Scribe_Deep.Look(ref incidentParms, "incidentParms");
		Scribe_References.Look(ref mapParent, "mapParent");
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap != null)
		{
			incident = IncidentDefOf.RaidEnemy;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = Find.RandomPlayerHomeMap;
			incidentParms.points = 500f;
			SetIncidentParmsAndRemoveTarget(incidentParms);
		}
	}
}
