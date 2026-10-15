using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompSendSignalOnCountdown : ThingComp, IThingGlower
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompSendSignalOnCountdown _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__7(int _003C_003E1__state)
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
			CompSendSignalOnCountdown CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!Prefs.DevMode || !DebugSettings.godMode)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Activate",
					action = delegate
					{
						Find.SignalManager.SendSignal(new Signal(CS_0024_003C_003E8__locals5.signalTag, CS_0024_003C_003E8__locals5.parent.Named("SUBJECT")));
						SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(CS_0024_003C_003E8__locals5.parent.Position, CS_0024_003C_003E8__locals5.parent.Map));
						CS_0024_003C_003E8__locals5.ticksLeft = 0;
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
			_003CCompGetGizmosExtra_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__7(0)
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

	public string signalTag;

	public int ticksLeft;

	private const float MaxDistActivationByOther = 40f;

	private CompProperties_SendSignalOnCountdown Props => (CompProperties_SendSignalOnCountdown)props;

	public bool ShouldBeLitNow()
	{
		return ticksLeft > 0;
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		signalTag = Props.signalTag;
		ticksLeft = Mathf.CeilToInt(Rand.ByCurve(Props.countdownCurveTicks));
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__7))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void CompTick()
	{
		if (parent.IsHashIntervalTick(250))
		{
			TickRareWorker();
		}
	}

	public override void CompTickRare()
	{
		base.CompTickRare();
		TickRareWorker();
	}

	public void TickRareWorker()
	{
		if (ticksLeft > 0 && parent.Spawned)
		{
			ticksLeft -= 250;
			if (ticksLeft <= 0)
			{
				Find.SignalManager.SendSignal(new Signal(signalTag, parent.Named("SUBJECT")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		if (!parent.Spawned)
		{
			return null;
		}
		if (ticksLeft <= 0)
		{
			return "expired".Translate().CapitalizeFirst();
		}
		return "SendSignalOnCountdownCompTime".Translate(ticksLeft.ToStringTicksToPeriod()).Resolve();
	}

	public override void Notify_SignalReceived(Signal signal)
	{
		if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg("SUBJECT", out Thing arg) && arg != parent && arg != null && arg.Map == parent.Map && parent.Position.DistanceTo(arg.Position) <= 40f)
		{
			ticksLeft = 0;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref ticksLeft, "ticksLeft", 0);
		Scribe_Values.Look(ref signalTag, "signalTag");
	}
}
