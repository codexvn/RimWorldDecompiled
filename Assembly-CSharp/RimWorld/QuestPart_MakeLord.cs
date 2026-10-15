using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public abstract class QuestPart_MakeLord : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__8 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_MakeLord _003C_003E4__this;

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
		public _003Cget_QuestLookTargets_003Ed__8(int _003C_003E1__state)
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
				QuestPart_MakeLord questPart_MakeLord = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_MakeLord).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						_003Ci_003E5__3++;
						break;
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
					if (questPart_MakeLord.excludeFromLookTargets)
					{
						return false;
					}
					_003Ci_003E5__3 = 0;
					break;
				}
				if (_003Ci_003E5__3 < questPart_MakeLord.pawns.Count)
				{
					_003C_003E2__current = questPart_MakeLord.pawns[_003Ci_003E5__3];
					_003C_003E1__state = 2;
					return true;
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__8(0)
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

	public List<Pawn> pawns = new List<Pawn>();

	public string inSignal;

	public Faction faction;

	public MapParent mapParent;

	public string inSignalRemovePawn;

	public Pawn mapOfPawn;

	public bool excludeFromLookTargets;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Map Map
	{
		get
		{
			if (mapOfPawn != null)
			{
				return mapOfPawn.MapHeld;
			}
			if (mapParent != null)
			{
				return mapParent.Map;
			}
			return null;
		}
	}

	protected abstract Lord MakeLord();

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (signal.tag == inSignal)
		{
			if (Map == null)
			{
				mapOfPawn = null;
				mapParent = quest.TryFindNewSuitableMapParentForRetarget();
			}
			if (Map != null)
			{
				pawns.RemoveAll((Pawn x) => x.MapHeld != Map);
				for (int i = 0; i < pawns.Count; i++)
				{
					pawns[i].GetLord()?.Notify_PawnLost(pawns[i], PawnLostCondition.ForcedByQuest, null);
				}
				Lord lord = MakeLord();
				for (int j = 0; j < pawns.Count; j++)
				{
					if (!pawns[j].Dead)
					{
						lord.AddPawn(pawns[j]);
					}
				}
			}
		}
		if (signal.tag == inSignalRemovePawn && signal.args.TryGetArg("SUBJECT", out Pawn arg) && pawns.Contains(arg))
		{
			pawns.Remove(arg);
		}
	}

	public override void Notify_FactionRemoved(Faction f)
	{
		if (faction == f)
		{
			faction = null;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref inSignalRemovePawn, "inSignalRemovePawn");
		Scribe_References.Look(ref faction, "faction");
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_References.Look(ref mapOfPawn, "mapOfPawn");
		Scribe_Values.Look(ref excludeFromLookTargets, "excludeFromLookTargets", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault());
		mapParent = pawns[0].Map.Parent;
	}
}
