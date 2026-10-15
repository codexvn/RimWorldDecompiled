using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_BetrayalOffer : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__13 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_BetrayalOffer _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__13(int _003C_003E1__state)
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
				QuestPart_BetrayalOffer questPart_BetrayalOffer = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_BetrayalOffer).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_00db;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3 = 0;
					goto IL_00ed;
					IL_00ed:
					if (_003Ci_003E5__3 < questPart_BetrayalOffer.pawns.Count)
					{
						if (!questPart_BetrayalOffer.pawns[_003Ci_003E5__3].Destroyed)
						{
							_003C_003E2__current = questPart_BetrayalOffer.pawns[_003Ci_003E5__3];
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_00db;
					}
					return false;
					IL_00db:
					_003Ci_003E5__3++;
					goto IL_00ed;
				}
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public ExtraFaction extraFaction;

	public Pawn asker;

	public List<Pawn> pawns = new List<Pawn>();

	public List<string> inSignals = new List<string>();

	public string outSignalEnabled;

	public string outSignalSuccess;

	public string outSignalFailure;

	public string outSignalFailureRecruited;

	public override string ExpiryInfoPart => "QuestBetrayalOffer".Translate(PawnsAliveCount, extraFaction.faction.Name);

	public override string ExpiryInfoPartTip => "QuestBetrayalOfferTip".Translate(asker.NameFullColored, extraFaction.faction.Name);

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__13))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__13(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	private int PawnsAliveCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < pawns.Count; i++)
			{
				if (!pawns[i].Destroyed)
				{
					num++;
				}
			}
			return num;
		}
	}

	private bool AnyPawnDespawnedButAlive
	{
		get
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (!pawns[i].SpawnedOrAnyParentSpawned && !pawns[i].Dead)
				{
					return true;
				}
			}
			return false;
		}
	}

	private bool AllSurvivingPawnsRecruited
	{
		get
		{
			foreach (Pawn pawn in pawns)
			{
				if (!pawn.Destroyed && (!pawn.Faction.IsPlayerSafe() || pawn.GetExtraFaction(extraFaction.factionType, quest) == extraFaction.faction))
				{
					return false;
				}
			}
			return true;
		}
	}

	protected override void Enable(SignalArgs receivedArgs)
	{
		base.Enable(receivedArgs);
		if (!AnyPawnDespawnedButAlive && PawnsAliveCount > 0)
		{
			if (!outSignalEnabled.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(outSignalEnabled, receivedArgs));
			}
		}
		else
		{
			Complete();
		}
	}

	protected override void ProcessQuestSignal(Signal signal)
	{
		base.ProcessQuestSignal(signal);
		if (inSignals.Contains(signal.tag))
		{
			if (AnyPawnDespawnedButAlive)
			{
				Find.SignalManager.SendSignal(new Signal(outSignalFailure, signal.args));
				Complete();
			}
			else if (PawnsAliveCount == 0)
			{
				Find.SignalManager.SendSignal(new Signal(outSignalSuccess, signal.args));
				Complete();
			}
			else if (AllSurvivingPawnsRecruited)
			{
				string tag = outSignalFailureRecruited ?? outSignalFailure;
				Find.SignalManager.SendSignal(new Signal(tag, signal.args));
				Complete();
			}
		}
	}

	public override void Notify_FactionRemoved(Faction f)
	{
		if (extraFaction.faction == f)
		{
			extraFaction.faction = null;
		}
	}

	public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
	{
		base.Notify_PawnKilled(pawn, dinfo);
		if (pawn == asker && base.State == QuestPartState.Enabled)
		{
			Disable();
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Deep.Look(ref extraFaction, "extraFaction");
		Scribe_References.Look(ref asker, "asker");
		Scribe_Collections.Look(ref inSignals, "inSignals", LookMode.Value);
		Scribe_Values.Look(ref outSignalSuccess, "outSignalSuccess");
		Scribe_Values.Look(ref outSignalFailure, "outSignalFailure");
		Scribe_Values.Look(ref outSignalFailureRecruited, "outSignalFailureRecruited");
		Scribe_Values.Look(ref outSignalEnabled, "outSignalEnabled");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawns.RemoveAll((Pawn x) => x == null);
			if (asker == null && base.State == QuestPartState.Enabled)
			{
				Disable();
			}
		}
	}
}
