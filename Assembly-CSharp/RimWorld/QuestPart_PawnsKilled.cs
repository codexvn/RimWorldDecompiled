using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_PawnsKilled : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_InvolvedFactions_003Ed__9 : IEnumerable<Faction>, IEnumerable, IEnumerator<Faction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Faction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_PawnsKilled _003C_003E4__this;

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
		public _003Cget_InvolvedFactions_003Ed__9(int _003C_003E1__state)
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
				QuestPart_PawnsKilled questPart_PawnsKilled = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_PawnsKilled).InvolvedFactions.GetEnumerator();
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
					if (questPart_PawnsKilled.requiredInstigatorFaction != null)
					{
						_003C_003E2__current = questPart_PawnsKilled.requiredInstigatorFaction;
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
			_003Cget_InvolvedFactions_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_InvolvedFactions_003Ed__9(0)
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

	public ThingDef race;

	public Faction requiredInstigatorFaction;

	public int count;

	public MapParent mapParent;

	public string outSignalPawnKilled;

	private int killed;

	public override string DescriptionPart => string.Concat("PawnsKilled".Translate(GenLabel.BestKindLabel(race.race.AnyPawnKind, Gender.None, plural: true)).CapitalizeFirst() + ": ", killed.ToString(), " / ", count.ToString());

	public override IEnumerable<Faction> InvolvedFactions
	{
		[IteratorStateMachine(typeof(_003Cget_InvolvedFactions_003Ed__9))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_InvolvedFactions_003Ed__9(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	protected override void Enable(SignalArgs receivedArgs)
	{
		base.Enable(receivedArgs);
		killed = 0;
	}

	public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
	{
		base.Notify_PawnKilled(pawn, dinfo);
		if (base.State == QuestPartState.Enabled && pawn.def == race && (requiredInstigatorFaction == null || (dinfo.HasValue && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == requiredInstigatorFaction))))
		{
			killed++;
			Find.SignalManager.SendSignal(new Signal(outSignalPawnKilled));
			if (killed >= count)
			{
				Complete();
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref race, "race");
		Scribe_References.Look(ref requiredInstigatorFaction, "requiredInstigatorFaction");
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_Values.Look(ref count, "count", 0);
		Scribe_Values.Look(ref killed, "killed", 0);
		Scribe_Values.Look(ref outSignalPawnKilled, "outSignalPawnKilled");
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		race = ThingDefOf.Muffalo;
		requiredInstigatorFaction = Faction.OfPlayer;
		count = 10;
	}
}
