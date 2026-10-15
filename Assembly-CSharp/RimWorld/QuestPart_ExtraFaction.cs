using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class QuestPart_ExtraFaction : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_InvolvedFactions_003Ed__7 : IEnumerable<Faction>, IEnumerable, IEnumerator<Faction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Faction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_ExtraFaction _003C_003E4__this;

		private IEnumerator<Faction> _003C_003E7__wrap1;

		Faction IEnumerator<Faction>.Current
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
		public _003Cget_InvolvedFactions_003Ed__7(int _003C_003E1__state)
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
				QuestPart_ExtraFaction questPart_ExtraFaction = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_ExtraFaction).InvolvedFactions.GetEnumerator();
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
						Faction current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_ExtraFaction.extraFaction?.faction != null)
					{
						_003C_003E2__current = questPart_ExtraFaction.extraFaction.faction;
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
		IEnumerator<Faction> IEnumerable<Faction>.GetEnumerator()
		{
			_003Cget_InvolvedFactions_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_InvolvedFactions_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Faction>)this).GetEnumerator();
		}
	}

	public ExtraFaction extraFaction;

	public List<Pawn> affectedPawns = new List<Pawn>();

	public bool areHelpers;

	public string inSignalRemovePawn;

	public List<string> inSignalsRemovePawn;

	private const int RelationsGainAvailableInTicks = 1800000;

	public override IEnumerable<Faction> InvolvedFactions
	{
		[IteratorStateMachine(typeof(_003Cget_InvolvedFactions_003Ed__7))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_InvolvedFactions_003Ed__7(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
	{
		base.Notify_PawnKilled(pawn, dinfo);
		if (pawn.HomeFaction != null && pawn.Faction != pawn.HomeFaction)
		{
			pawn.SetFaction(pawn.HomeFaction);
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!(signal.tag == inSignalRemovePawn))
		{
			List<string> list = inSignalsRemovePawn;
			if (list == null || !list.Contains(signal.tag))
			{
				return;
			}
		}
		if (signal.args.TryGetArg("SUBJECT", out Pawn arg) && affectedPawns.Contains(arg))
		{
			affectedPawns.Remove(arg);
			extraFaction.faction.Notify_MemberLeftExtraFaction(arg);
		}
	}

	public override bool QuestPartReserves(Faction f)
	{
		return extraFaction.faction == f;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref extraFaction, "extraFaction");
		Scribe_Collections.Look(ref affectedPawns, "affectedPawns", LookMode.Reference);
		Scribe_Values.Look(ref areHelpers, "areHelpers", defaultValue: false);
		Scribe_Values.Look(ref inSignalRemovePawn, "inSignalRemovePawn");
		Scribe_Collections.Look(ref inSignalsRemovePawn, "inSignalsRemovePawn", LookMode.Value);
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		if (extraFaction?.faction == null)
		{
			affectedPawns.Clear();
			return;
		}
		affectedPawns.RemoveAll((Pawn x) => x == null);
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		affectedPawns.Replace(replace, with);
	}

	public override void Cleanup()
	{
		base.Cleanup();
		SetRelationsGainTickForPawns();
	}

	public override void Notify_FactionRemoved(Faction faction)
	{
		if (extraFaction.faction == faction)
		{
			affectedPawns.Clear();
			extraFaction.faction = null;
		}
	}

	protected override void Disable()
	{
		base.Disable();
		SetRelationsGainTickForPawns();
	}

	private void SetRelationsGainTickForPawns()
	{
		foreach (Pawn affectedPawn in affectedPawns)
		{
			if (affectedPawn.mindState != null)
			{
				affectedPawn.mindState.SetNoAidRelationsGainUntilTick(Find.TickManager.TicksGame + 1800000);
			}
		}
	}
}
