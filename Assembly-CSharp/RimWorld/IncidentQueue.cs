using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class IncidentQueue : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public IncidentQueue _003C_003E4__this;

		private List<QueuedIncident>.Enumerator _003C_003E7__wrap1;

		object IEnumerator<object>.Current
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
		public _003CGetEnumerator_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
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
			_003C_003E7__wrap1 = default(List<QueuedIncident>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				IncidentQueue incidentQueue = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = incidentQueue.queuedIncidents.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					QueuedIncident current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<QueuedIncident>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();

	public int Count => queuedIncidents.Count;

	public string DebugQueueReadout
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (QueuedIncident queuedIncident in queuedIncidents)
			{
				stringBuilder.AppendLine(queuedIncident.ToString() + " (in " + (queuedIncident.FireTick - Find.TickManager.TicksGame) + " ticks)");
			}
			return stringBuilder.ToString();
		}
	}

	[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__5))]
	public IEnumerator GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEnumerator_003Ed__5(0)
		{
			_003C_003E4__this = this
		};
	}

	public void Clear()
	{
		queuedIncidents.Clear();
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref queuedIncidents, "queuedIncidents", LookMode.Deep);
	}

	public bool Add(QueuedIncident qi)
	{
		queuedIncidents.Add(qi);
		queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
		return true;
	}

	public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null, int retryDurationTicks = 0)
	{
		QueuedIncident qi = new QueuedIncident(new FiringIncident(def, null, parms), fireTick, retryDurationTicks);
		Add(qi);
		return true;
	}

	public void IncidentQueueTick()
	{
		for (int num = queuedIncidents.Count - 1; num >= 0; num--)
		{
			QueuedIncident queuedIncident = queuedIncidents[num];
			if (!queuedIncident.TriedToFire)
			{
				if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
				{
					bool num2 = Find.Storyteller.TryFire(queuedIncident.FiringIncident, queued: true);
					queuedIncident.Notify_TriedToFire();
					if (num2 || queuedIncident.RetryDurationTicks == 0)
					{
						queuedIncidents.Remove(queuedIncident);
					}
				}
			}
			else if (queuedIncident.FireTick + queuedIncident.RetryDurationTicks <= Find.TickManager.TicksGame)
			{
				queuedIncidents.Remove(queuedIncident);
			}
			else if (Find.TickManager.TicksGame % 833 == Rand.RangeSeeded(0, 833, queuedIncident.FireTick))
			{
				bool num3 = Find.Storyteller.TryFire(queuedIncident.FiringIncident, queued: true);
				queuedIncident.Notify_TriedToFire();
				if (num3)
				{
					queuedIncidents.Remove(queuedIncident);
				}
			}
		}
	}

	public void Notify_MapRemoved(Map map)
	{
		queuedIncidents.RemoveAll((QueuedIncident x) => x.FiringIncident.parms.target == map);
	}
}
