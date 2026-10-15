using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PawnRelationDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__29 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PawnRelationDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__29(int _003C_003E1__state)
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
				PawnRelationDef pawnRelationDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)pawnRelationDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						pawnRelationDef.reflexive = false;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (pawnRelationDef.implied && pawnRelationDef.reflexive)
					{
						_003C_003E2__current = pawnRelationDef.defName + ": implied relations can't use the \"reflexive\" option.";
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__29 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__29(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Type workerClass = typeof(PawnRelationWorker);

	[MustTranslate]
	public string labelFemale;

	public float importance;

	public bool implied;

	public bool reflexive;

	public int opinionOffset;

	public float generationChanceFactor;

	public float romanceChanceFactor = 1f;

	public float incestOpinionOffset;

	public bool familyByBloodRelation;

	public bool removeOnDeath;

	public bool removeOnLeftBehind;

	public float inbredChanceOnChild;

	public ThoughtDef diedThought;

	public ThoughtDef diedThoughtFemale;

	public ThoughtDef lostThought;

	public ThoughtDef lostThoughtFemale;

	public List<ThoughtDef> soldThoughts;

	public ThoughtDef killedThought;

	public ThoughtDef killedThoughtFemale;

	[Unsaved(false)]
	private PawnRelationWorker workerInt;

	public PawnRelationWorker Worker
	{
		get
		{
			if (workerInt == null)
			{
				workerInt = (PawnRelationWorker)Activator.CreateInstance(workerClass);
				workerInt.def = this;
			}
			return workerInt;
		}
	}

	public string GetGenderSpecificLabel(Pawn pawn)
	{
		if (pawn.gender == Gender.Female && !labelFemale.NullOrEmpty())
		{
			return labelFemale;
		}
		return label;
	}

	public string GetGenderSpecificLabelCap(Pawn pawn)
	{
		return GetGenderSpecificLabel(pawn).CapitalizeFirst();
	}

	public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
	{
		if (killed.gender == Gender.Female && diedThoughtFemale != null)
		{
			return diedThoughtFemale;
		}
		return diedThought;
	}

	public ThoughtDef GetGenderSpecificLostThought(Pawn killed)
	{
		if (killed.gender == Gender.Female && diedThoughtFemale != null)
		{
			return lostThoughtFemale;
		}
		return lostThought;
	}

	public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
	{
		if (killed.gender == Gender.Female && killedThoughtFemale != null)
		{
			return killedThoughtFemale;
		}
		return killedThought;
	}

	public ThoughtDef GetGenderSpecificThought(Pawn pawn, PawnDiedOrDownedThoughtsKind thoughtsKind)
	{
		if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
		{
			return GetGenderSpecificLostThought(pawn);
		}
		return GetGenderSpecificDiedThought(pawn);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__29))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__29(-2)
		{
			_003C_003E4__this = this
		};
	}
}
