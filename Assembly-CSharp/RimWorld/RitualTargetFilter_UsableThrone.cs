using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class RitualTargetFilter_UsableThrone : RitualTargetFilter
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
				_003C_003E2__current = "AbilitySpeechTargetDescThrone".Translate();
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

	public RitualTargetFilter_UsableThrone()
	{
	}

	public RitualTargetFilter_UsableThrone(RitualTargetFilterDef def)
		: base(def)
	{
	}

	public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
	{
		Pawn pawn = initiator.Thing as Pawn;
		rejectionReason = "";
		if (pawn == null)
		{
			return false;
		}
		Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(pawn) ?? pawn.ownership.AssignedThrone;
		if (building_Throne == null)
		{
			rejectionReason = "AbilitySpeechDisabledNoThroneAssigned".Translate();
			return false;
		}
		if (!pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger()))
		{
			rejectionReason = "AbilitySpeechDisabledNoThroneIsNotAccessible".Translate();
			return false;
		}
		if (pawn.royalty.AnyUnmetThroneroomRequirements())
		{
			rejectionReason = "AbilitySpeechDisabledNoThroneUndignified".Translate();
			return false;
		}
		return true;
	}

	public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
	{
		Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone((Pawn)initiator.Thing);
		if (building_Throne == null)
		{
			return TargetInfo.Invalid;
		}
		return new TargetInfo(building_Throne.InteractionCell, building_Throne.Map);
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__4))]
	public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__4(-2);
	}
}
