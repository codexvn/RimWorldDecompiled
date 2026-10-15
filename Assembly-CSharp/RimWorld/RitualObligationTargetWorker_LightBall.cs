using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualObligationTargetWorker_LightBall : RitualObligationTargetWorker_ThingDef
{
	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__3 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		string IEnumerator<string>.Current
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
		public _003CGetTargetInfos_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.LightBall.label;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.RitualSpot.label;
				_003C_003E1__state = 2;
				return true;
			case 2:
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetTargetInfos_003Ed__3(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public RitualObligationTargetWorker_LightBall()
	{
	}

	public RitualObligationTargetWorker_LightBall(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
	{
		if (!ModLister.CheckIdeology("Lightball target"))
		{
			return false;
		}
		if (!base.CanUseTargetInternal(target, obligation).canUse)
		{
			return false;
		}
		Thing thing = target.Thing;
		CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
		if (compPowerTrader != null)
		{
			if (compPowerTrader.PowerNet == null || !compPowerTrader.PowerNet.HasActivePowerSource)
			{
				return "RitualTargetLightBallIsNotPowered".Translate();
			}
			List<Thing> forCell = target.Map.listerBuldingOfDefInProximity.GetForCell(target.Cell, def.maxSpeakerDistance, ThingDefOf.Loudspeaker);
			bool flag = false;
			foreach (Thing item in forCell)
			{
				CompPowerTrader compPowerTrader2 = item.TryGetComp<CompPowerTrader>();
				if (item.GetRoom() == thing.GetRoom() && compPowerTrader2.PowerNet != null && compPowerTrader2.PowerNet.HasActivePowerSource)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return "RitualTargetNoPoweredSpeakers".Translate();
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__3))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__3(-2);
	}
}
