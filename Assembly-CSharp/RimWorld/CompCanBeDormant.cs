using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class CompCanBeDormant : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass20_0
	{
		public int delayTicks;

		public CompCanBeDormant _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__1()
		{
			_003C_003E4__this.wakeUpOnTick = Find.TickManager.TicksGame + delayTicks;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompCanBeDormant _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__20(int _003C_003E1__state)
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
			CompCanBeDormant CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!Prefs.DevMode || !DebugSettings.godMode || CS_0024_003C_003E8__locals10.Props.dontShowDevGizmos)
				{
					return false;
				}
				if (!CS_0024_003C_003E8__locals10.Awake)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Wake Up",
						action = CS_0024_003C_003E8__locals10.WakeUp
					};
					_003C_003E1__state = 1;
					return true;
				}
				if (CS_0024_003C_003E8__locals10.Props.jobDormancy)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: To Sleep",
						action = CS_0024_003C_003E8__locals10.ToSleep
					};
					_003C_003E1__state = 3;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						for (int i = 0; i < 10; i++)
						{
							_003C_003Ec__DisplayClass20_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass20_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals10
							};
							float num2 = (float)i * 0.5f;
							CS_0024_003C_003E8__locals5.delayTicks = Mathf.RoundToInt(num2 * 60f);
							string label = num2.ToString("F1") + "s";
							list.Add(new FloatMenuOption(label, delegate
							{
								CS_0024_003C_003E8__locals5._003C_003E4__this.wakeUpOnTick = Find.TickManager.TicksGame + CS_0024_003C_003E8__locals5.delayTicks;
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					defaultLabel = "DEV: Wake up after...",
					defaultDesc = "DEV: Wake up after a delay"
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
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
			_003CCompGetGizmosExtra_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__20(0)
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

	private int makeTick;

	public int wokeUpTick = int.MinValue;

	public int wakeUpOnTick = int.MinValue;

	private int repeatWakeUpSignalOnTick = int.MaxValue;

	private Effecter wakeUpEffect;

	public string wakeUpSignalTag;

	public List<string> wakeUpSignalTags;

	public const string DefaultWakeUpSignal = "CompCanBeDormant.WakeUp";

	public CompProperties_CanBeDormant Props => (CompProperties_CanBeDormant)props;

	private bool WaitingToWakeUp => wakeUpOnTick != int.MinValue;

	protected virtual JobDef SleepJob => JobDefOf.Wait_AsleepDormancy;

	public virtual bool Awake
	{
		get
		{
			if (wokeUpTick == int.MinValue || wokeUpTick > Find.TickManager.TicksGame)
			{
				if (parent.Spawned && Props.jobDormancy && parent is Pawn pawn)
				{
					return pawn.CurJobDef != SleepJob;
				}
				return false;
			}
			return true;
		}
	}

	protected virtual bool ShowZs
	{
		get
		{
			if (parent is Pawn)
			{
				return false;
			}
			if (!Props.showSleepingZs)
			{
				return false;
			}
			if (!Props.delayedWakeUpDoesZs)
			{
				return wakeUpOnTick == int.MinValue;
			}
			return true;
		}
	}

	public override void PostPostMake()
	{
		base.PostPostMake();
		makeTick = GenTicks.TicksGame;
		if (!Props.startsDormant)
		{
			WakeUp();
		}
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		wakeUpSignalTag = Props.wakeUpSignalTag;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__20))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		if (parent is Pawn p && (p.IsSelfShutdown() || p.IsDeactivated() || p.Awake()))
		{
			return null;
		}
		if (Awake)
		{
			if (parent.Faction != Faction.OfPlayer && makeTick != wokeUpTick && !(parent is Pawn { Downed: not false }))
			{
				return Props.awakeStateLabelKey.Translate((GenTicks.TicksGame - wokeUpTick).TicksToDays().ToString("0.#"));
			}
			return null;
		}
		if (!Props.wakeUpDelayStateLabelKey.NullOrEmpty() && wakeUpOnTick != int.MinValue)
		{
			return Props.wakeUpDelayStateLabelKey.Translate();
		}
		return Props.dormantStateLabelKey.Translate();
	}

	public void WakeUpWithDelay()
	{
		if (!Awake)
		{
			wakeUpOnTick = Find.TickManager.TicksGame + Props.wakeUpDelayRange.RandomInRange;
			repeatWakeUpSignalOnTick = (Props.wakeUpRepeatSignalDelayRange.HasValue ? (Find.TickManager.TicksGame + Props.wakeUpRepeatSignalDelayRange.Value.RandomInRange) : int.MinValue);
		}
	}

	public virtual void WakeUp()
	{
		if (Awake)
		{
			return;
		}
		wokeUpTick = GenTicks.TicksGame;
		wakeUpOnTick = int.MinValue;
		Pawn obj = parent as Pawn;
		Building building = parent as Building;
		(obj?.GetLord() ?? building?.GetLord())?.Notify_DormancyWakeup();
		if (parent.Spawned)
		{
			if (parent is IAttackTarget t)
			{
				parent.Map.attackTargetsCache.UpdateTarget(t);
			}
			if (Props.jobDormancy && parent is Pawn pawn && pawn.CurJobDef == SleepJob)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
		}
	}

	public virtual void ToSleep()
	{
		if (!Awake)
		{
			return;
		}
		wokeUpTick = int.MinValue;
		if (parent.Spawned)
		{
			if (parent is IAttackTarget t)
			{
				parent.Map.attackTargetsCache.UpdateTarget(t);
			}
			if (Props.jobDormancy && parent is Pawn pawn)
			{
				Job job = JobMaker.MakeJob(SleepJob, pawn.Position);
				job.forceSleep = true;
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
			}
		}
	}

	public override void CompTickRare()
	{
		base.CompTickRare();
		if (wakeUpOnTick != int.MinValue && Find.TickManager.TicksGame >= wakeUpOnTick)
		{
			WakeUp();
		}
		TickRareWorker();
	}

	public override void CompTick()
	{
		base.CompTick();
		if (wakeUpOnTick != int.MinValue)
		{
			if (Find.TickManager.TicksGame >= wakeUpOnTick)
			{
				WakeUp();
				wakeUpEffect?.Cleanup();
				wakeUpEffect = null;
			}
			else
			{
				if (Props.wakeUpEffect != null && wakeUpEffect == null)
				{
					wakeUpEffect = Props.wakeUpEffect.SpawnAttached(parent, parent.Map);
				}
				wakeUpEffect?.EffectTick(parent, parent);
				if (repeatWakeUpSignalOnTick != int.MinValue && Find.TickManager.TicksGame == repeatWakeUpSignalOnTick)
				{
					Find.SignalManager.SendSignal(new Signal(wakeUpSignalTag, parent.Named("SUBJECT")));
				}
			}
		}
		if (parent.IsHashIntervalTick(250))
		{
			TickRareWorker();
		}
	}

	public void TickRareWorker()
	{
		if (parent.Spawned && !Awake && ShowZs && !parent.Position.Fogged(parent.Map))
		{
			FleckMaker.ThrowMetaIcon(parent.Position, parent.Map, FleckDefOf.SleepZ);
		}
	}

	public override void Notify_SignalReceived(Signal signal)
	{
		if (!string.IsNullOrEmpty(wakeUpSignalTag) && !Awake && (signal.tag == wakeUpSignalTag || (wakeUpSignalTags != null && wakeUpSignalTags.Contains(signal.tag))) && signal.args.TryGetArg("SUBJECT", out Thing arg) && arg != parent && arg != null && arg.Map == parent.Map && parent.Position.DistanceTo(arg.Position) <= Props.maxDistAwakenByOther && (!signal.args.TryGetArg("FACTION", out Faction arg2) || arg2 == null || arg2 == parent.Faction) && (Props.canWakeUpFogged || !parent.Fogged()) && !WaitingToWakeUp)
		{
			WakeUpWithDelay();
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref wokeUpTick, "wokeUpTick", int.MinValue);
		Scribe_Values.Look(ref wakeUpOnTick, "wakeUpOnTick", int.MinValue);
		Scribe_Values.Look(ref wakeUpSignalTag, "wakeUpSignalTag");
		Scribe_Collections.Look(ref wakeUpSignalTags, "wakeUpSignalTags", LookMode.Value);
		Scribe_Values.Look(ref makeTick, "makeTick", 0);
		Scribe_Values.Look(ref repeatWakeUpSignalOnTick, "repeatWakeUpSignalOnTick", int.MinValue);
	}
}
