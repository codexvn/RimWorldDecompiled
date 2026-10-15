using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class LordToil_BestowingCeremony_Wait : LordToil_Wait
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public LordJob_BestowingCeremony lordJob;

		public LordToil_BestowingCeremony_Wait _003C_003E4__this;

		public Func<Pawn, bool> _003C_003E9__3;

		public Dialog_BeginRitual.ActionCallback _003C_003E9__1;

		internal void _003CExtraFloatMenuOptions_003Eb__0()
		{
			Find.WindowStack.Add(new Dialog_BeginRitual("RitualBestowingCeremony".Translate(), null, lordJob.targetSpot.ToTargetInfo(_003C_003E4__this.bestower.Map), _003C_003E4__this.bestower.Map, delegate(RitualRoleAssignments assignments)
			{
				_003C_003E4__this.StartRitual(assignments.Participants.Where((Pawn p) => p != _003C_003E4__this.bestower).ToList());
				return true;
			}, _003C_003E4__this.bestower, null, delegate(Pawn pawn, bool voluntary, bool allowOtherIdeos)
			{
				if (pawn.GetLord()?.LordJob is LordJob_Ritual)
				{
					return false;
				}
				if (pawn.IsSubhuman)
				{
					return false;
				}
				return !pawn.IsPrisonerOfColony && !pawn.RaceProps.Animal;
			}, "Begin".Translate(), new List<Pawn> { _003C_003E4__this.bestower, _003C_003E4__this.target }, null, RitualOutcomeEffectDefOf.BestowingCeremony));
		}

		internal bool _003CExtraFloatMenuOptions_003Eb__1(RitualRoleAssignments assignments)
		{
			_003C_003E4__this.StartRitual(assignments.Participants.Where((Pawn p) => p != _003C_003E4__this.bestower).ToList());
			return true;
		}

		internal bool _003CExtraFloatMenuOptions_003Eb__3(Pawn p)
		{
			return p != _003C_003E4__this.bestower;
		}
	}

	[CompilerGenerated]
	private sealed class _003CExtraFloatMenuOptions_003Ed__7 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LordToil_BestowingCeremony_Wait _003C_003E4__this;

		private Pawn target;

		public Pawn _003C_003E3__target;

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
		public _003CExtraFloatMenuOptions_003Ed__7(int _003C_003E1__state)
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
			LordToil_BestowingCeremony_Wait lordToil_BestowingCeremony_Wait = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass7_0
				{
					_003C_003E4__this = _003C_003E4__this,
					lordJob = (LordJob_BestowingCeremony)lordToil_BestowingCeremony_Wait.lord.LordJob
				};
				if (target != lordToil_BestowingCeremony_Wait.bestower)
				{
					break;
				}
				FloatMenuOption floatMenuOption = new FloatMenuOption("BeginRitual".Translate("RitualBestowingCeremony".Translate()), delegate
				{
					Find.WindowStack.Add(new Dialog_BeginRitual("RitualBestowingCeremony".Translate(), null, CS_0024_003C_003E8__locals8.lordJob.targetSpot.ToTargetInfo(CS_0024_003C_003E8__locals8._003C_003E4__this.bestower.Map), CS_0024_003C_003E8__locals8._003C_003E4__this.bestower.Map, delegate(RitualRoleAssignments assignments)
					{
						CS_0024_003C_003E8__locals8._003C_003E4__this.StartRitual(assignments.Participants.Where((Pawn p) => p != CS_0024_003C_003E8__locals8._003C_003E4__this.bestower).ToList());
						return true;
					}, CS_0024_003C_003E8__locals8._003C_003E4__this.bestower, null, delegate(Pawn pawn, bool voluntary, bool allowOtherIdeos)
					{
						if (pawn.GetLord()?.LordJob is LordJob_Ritual)
						{
							return false;
						}
						if (pawn.IsSubhuman)
						{
							return false;
						}
						return !pawn.IsPrisonerOfColony && !pawn.RaceProps.Animal;
					}, "Begin".Translate(), new List<Pawn>
					{
						CS_0024_003C_003E8__locals8._003C_003E4__this.bestower,
						CS_0024_003C_003E8__locals8._003C_003E4__this.target
					}, null, RitualOutcomeEffectDefOf.BestowingCeremony));
				});
				if (!LordJob_BestowingCeremony.TryGetUsableSpotAdjacentToBestower(lordToil_BestowingCeremony_Wait.bestower).IsValid)
				{
					floatMenuOption.Disabled = true;
					floatMenuOption.Label += " (" + "BestowingSpotUnavailable".Translate() + ")";
				}
				_003C_003E2__current = floatMenuOption;
				_003C_003E1__state = 1;
				return true;
			}
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CExtraFloatMenuOptions_003Ed__7 _003CExtraFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CExtraFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CExtraFloatMenuOptions_003Ed__ = new _003CExtraFloatMenuOptions_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CExtraFloatMenuOptions_003Ed__.target = _003C_003E3__target;
			return _003CExtraFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPawnGizmos_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn p;

		public Pawn _003C_003E3__p;

		public LordToil_BestowingCeremony_Wait _003C_003E4__this;

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
		public _003CGetPawnGizmos_003Ed__6(int _003C_003E1__state)
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
			LordToil_BestowingCeremony_Wait lordToil_BestowingCeremony_Wait = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (p == lordToil_BestowingCeremony_Wait.bestower)
				{
					LordJob_BestowingCeremony job = (LordJob_BestowingCeremony)lordToil_BestowingCeremony_Wait.lord.LordJob;
					_003C_003E2__current = new Command_BestowerCeremony(job, lordToil_BestowingCeremony_Wait.bestower, lordToil_BestowingCeremony_Wait.target, lordToil_BestowingCeremony_Wait.StartRitual);
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetPawnGizmos_003Ed__6 _003CGetPawnGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPawnGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetPawnGizmos_003Ed__ = new _003CGetPawnGizmos_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetPawnGizmos_003Ed__.p = _003C_003E3__p;
			return _003CGetPawnGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public Pawn target;

	public Pawn bestower;

	public LordToil_BestowingCeremony_Wait(Pawn target, Pawn bestower)
	{
		this.target = target;
		this.bestower = bestower;
	}

	public override void Init()
	{
		Messages.Message("MessageBestowerWaiting".Translate(target.Named("TARGET"), lord.ownedPawns[0].Named("BESTOWER")), new LookTargets(new Pawn[2]
		{
			target,
			lord.ownedPawns[0]
		}), MessageTypeDefOf.NeutralEvent);
	}

	protected override void DecoratePawnDuty(PawnDuty duty)
	{
		duty.focus = target;
	}

	public override void DrawPawnGUIOverlay(Pawn pawn)
	{
		pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
	}

	[IteratorStateMachine(typeof(_003CGetPawnGizmos_003Ed__6))]
	public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPawnGizmos_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__p = p
		};
	}

	[IteratorStateMachine(typeof(_003CExtraFloatMenuOptions_003Ed__7))]
	public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn target, Pawn forPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CExtraFloatMenuOptions_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}

	private void StartRitual(List<Pawn> pawns)
	{
		foreach (Pawn pawn in pawns)
		{
			if (pawn.GetLord()?.LordJob is LordJob_VoluntarilyJoinable)
			{
				pawn.GetLord().Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}
		lord.AddPawns(pawns);
		((LordJob_BestowingCeremony)lord.LordJob).colonistParticipants.AddRange(pawns);
		lord.ReceiveMemo(LordJob_BestowingCeremony.MemoCeremonyStarted);
		foreach (Pawn pawn2 in pawns)
		{
			if (pawn2.drafter != null)
			{
				pawn2.drafter.Drafted = false;
			}
			if (!pawn2.Awake())
			{
				RestUtility.WakeUp(pawn2);
			}
		}
	}
}
