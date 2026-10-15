using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class LordToil_ReimplantXenogerm : LordToil
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0
	{
		public Pawn current;

		public Pawn requester;

		internal void _003CExtraFloatMenuOptions_003Eb__0()
		{
			current.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.GetReimplanted, requester), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CExtraFloatMenuOptions_003Ed__11 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn current;

		public Pawn _003C_003E3__current;

		private Pawn requester;

		public Pawn _003C_003E3__requester;

		public LordToil_ReimplantXenogerm _003C_003E4__this;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CExtraFloatMenuOptions_003Ed__11(int _003C_003E1__state)
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
			LordToil_ReimplantXenogerm lordToil_ReimplantXenogerm = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass11_0 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass11_0
				{
					current = current,
					requester = requester
				};
				LordToilData_ReimplantXenogerm data = lordToil_ReimplantXenogerm.Data;
				if (data.target == CS_0024_003C_003E8__locals10.requester)
				{
					if (CS_0024_003C_003E8__locals10.requester.genes == null || CS_0024_003C_003E8__locals10.current.genes == null)
					{
						return false;
					}
					string text = "AcceptXenogerm".Translate(CS_0024_003C_003E8__locals10.requester);
					if (!CS_0024_003C_003E8__locals10.current.CanReach(data.target, PathEndMode.ClosestTouch, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption(text + ": " + "CannotReach".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 1;
						return true;
					}
					if (CS_0024_003C_003E8__locals10.current.IsQuestLodger())
					{
						_003C_003E2__current = new FloatMenuOption(text + ": " + "MessageCannotImplantInTempFactionMembers".Translate(), null);
						_003C_003E1__state = 2;
						return true;
					}
					if (GeneUtility.SameXenotype(CS_0024_003C_003E8__locals10.current, data.target))
					{
						_003C_003E2__current = new FloatMenuOption(text + ": " + "MessageCannotUseOnSameXenotype".Translate(CS_0024_003C_003E8__locals10.current), null);
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003E2__current = new FloatMenuOption(text, delegate
					{
						CS_0024_003C_003E8__locals10.current.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.GetReimplanted, CS_0024_003C_003E8__locals10.requester), JobTag.Misc);
					});
					_003C_003E1__state = 4;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
				_003C_003E1__state = -1;
				break;
			case 4:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CExtraFloatMenuOptions_003Ed__11 _003CExtraFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CExtraFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CExtraFloatMenuOptions_003Ed__ = new _003CExtraFloatMenuOptions_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CExtraFloatMenuOptions_003Ed__.requester = _003C_003E3__requester;
			_003CExtraFloatMenuOptions_003Ed__.current = _003C_003E3__current;
			return _003CExtraFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private const float WanderRadius = 10f;

	public override bool ShouldFail => Data.target == null;

	public LordToilData_ReimplantXenogerm Data => (LordToilData_ReimplantXenogerm)data;

	public LordToil_ReimplantXenogerm(IntVec3 spot)
	{
		data = new LordToilData_ReimplantXenogerm
		{
			waitSpot = spot
		};
	}

	public override void Init()
	{
		SetTarget();
	}

	private void SetTarget()
	{
		LordToilData_ReimplantXenogerm lordToilData_ReimplantXenogerm = Data;
		if (!lord.ownedPawns.TryRandomElementByWeight((Pawn x) => (!x.Destroyed && !x.Downed && !x.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating)) ? 1 : 0, out lordToilData_ReimplantXenogerm.target))
		{
			lord.ownedPawns.TryRandomElementByWeight((Pawn x) => (!x.Destroyed && !x.Downed) ? 1 : 0, out lordToilData_ReimplantXenogerm.target);
		}
		if (lordToilData_ReimplantXenogerm.target != null)
		{
			Hediff firstHediffOfDef = lordToilData_ReimplantXenogerm.target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.XenogermReplicating);
			if (firstHediffOfDef != null)
			{
				lordToilData_ReimplantXenogerm.target.health.RemoveHediff(firstHediffOfDef);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelSanguophageWaitingToReimplant".Translate(), "LetterTextSanguophageWaitingToReimplant".Translate(lordToilData_ReimplantXenogerm.target.Named("PAWN"), 15000.ToStringTicksToPeriod().Named("DURATION")), LetterDefOf.PositiveEvent, lordToilData_ReimplantXenogerm.target);
		}
	}

	public override void UpdateAllDuties()
	{
		LordToilData_ReimplantXenogerm lordToilData_ReimplantXenogerm = Data;
		for (int i = 0; i < lord.ownedPawns.Count; i++)
		{
			if (lord.ownedPawns[i] == lordToilData_ReimplantXenogerm.target)
			{
				lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Idle, lordToilData_ReimplantXenogerm.waitSpot);
			}
			else
			{
				lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.WanderClose_NoNeeds, lordToilData_ReimplantXenogerm.waitSpot, 10f);
			}
		}
	}

	public override void DrawPawnGUIOverlay(Pawn pawn)
	{
		if (pawn == Data.target)
		{
			pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
		}
	}

	public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
	{
		if (p == Data.target)
		{
			SetTarget();
		}
	}

	[IteratorStateMachine(typeof(_003CExtraFloatMenuOptions_003Ed__11))]
	public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CExtraFloatMenuOptions_003Ed__11(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__requester = requester,
			_003C_003E3__current = current
		};
	}
}
