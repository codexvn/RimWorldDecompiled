using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualObligationTargetWorker_AnimaTree : RitualObligationTargetFilter
{
	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
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
		public _003CGetTargetInfos_003Ed__4(int _003C_003E1__state)
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
				_003C_003E2__current = "RitualTargetAnimaTreeInfo".Translate();
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetTargetInfos_003Ed__4(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public RitualObligationTargetWorker_AnimaTree()
	{
	}

	public RitualObligationTargetWorker_AnimaTree(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
	{
		return Enumerable.Empty<TargetInfo>();
	}

	protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
	{
		CompPsylinkable compPsylinkable = target.Thing.TryGetComp<CompPsylinkable>();
		if (compPsylinkable == null)
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		foreach (Pawn item in target.Map.mapPawns.FreeColonistsSpawned)
		{
			if ((bool)compPsylinkable.CanPsylink(item, null, checkSpot: false))
			{
				flag2 = true;
			}
			if (compPsylinkable.Props.requiredFocus.CanPawnUse(item))
			{
				flag = true;
			}
		}
		if (compPsylinkable.CompSubplant.SubplantsForReading.Count < compPsylinkable.Props.requiredSubplantCountPerPsylinkLevel[0])
		{
			return "RitualTargetAnimaTreeNotEnoughAnimaGrass".Translate(compPsylinkable.Props.requiredSubplantCountPerPsylinkLevel[0]);
		}
		if (!flag)
		{
			return "RitualTargetAnimaTreeNoPawnsWithNatureFocus".Translate();
		}
		if (!flag2)
		{
			return "RitualTargetAnimaTreeNoPawnsToLink".Translate();
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__4))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__4(-2);
	}
}
