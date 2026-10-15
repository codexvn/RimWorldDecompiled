using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse.AI;

namespace Verse;

public class PriorityWork : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__17 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PriorityWork _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__17(int _003C_003E1__state)
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
			PriorityWork CS_0024_003C_003E8__locals11 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if ((!CS_0024_003C_003E8__locals11.IsPrioritized && (CS_0024_003C_003E8__locals11.pawn.CurJob == null || !CS_0024_003C_003E8__locals11.pawn.CurJob.playerForced || !CS_0024_003C_003E8__locals11.pawn.jobs.IsCurrentJobPlayerInterruptible()) && !CS_0024_003C_003E8__locals11.pawn.jobs.jobQueue.AnyPlayerForced) || CS_0024_003C_003E8__locals11.pawn.Drafted || CS_0024_003C_003E8__locals11.pawn.Deathresting)
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "CommandClearPrioritizedWork".Translate(),
					defaultDesc = "CommandClearPrioritizedWorkDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate
					{
						CS_0024_003C_003E8__locals11.ClearPrioritizedWorkAndJobQueue();
						if (CS_0024_003C_003E8__locals11.pawn.CurJob.playerForced && CS_0024_003C_003E8__locals11.pawn.jobs.IsCurrentJobPlayerInterruptible())
						{
							CS_0024_003C_003E8__locals11.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
						}
					},
					hotKey = KeyBindingDefOf.Designator_Cancel,
					groupKeyIgnoreContent = 6165612
				};
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private Pawn pawn;

	private IntVec3 prioritizedCell = IntVec3.Invalid;

	private WorkGiverDef prioritizedWorkGiver;

	private int prioritizeTick = Find.TickManager.TicksGame;

	private const int Timeout = 30000;

	public bool IsPrioritized
	{
		get
		{
			if (prioritizedCell.IsValid)
			{
				if (Find.TickManager.TicksGame < prioritizeTick + 30000)
				{
					return true;
				}
				Clear();
			}
			return false;
		}
	}

	public IntVec3 Cell => prioritizedCell;

	public WorkGiverDef WorkGiver => prioritizedWorkGiver;

	public PriorityWork()
	{
	}

	public PriorityWork(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref prioritizedCell, "prioritizedCell");
		Scribe_Defs.Look(ref prioritizedWorkGiver, "prioritizedWorkGiver");
		Scribe_Values.Look(ref prioritizeTick, "prioritizeTick", 0);
	}

	public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
	{
		this.prioritizedCell = prioritizedCell;
		this.prioritizedWorkGiver = prioritizedWorkGiver;
		prioritizeTick = Find.TickManager.TicksGame;
	}

	public void Clear()
	{
		prioritizedCell = IntVec3.Invalid;
		prioritizedWorkGiver = null;
		prioritizeTick = 0;
	}

	public void ClearPrioritizedWorkAndJobQueue()
	{
		Clear();
		pawn.jobs.ClearQueuedJobs();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__17))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}
}
