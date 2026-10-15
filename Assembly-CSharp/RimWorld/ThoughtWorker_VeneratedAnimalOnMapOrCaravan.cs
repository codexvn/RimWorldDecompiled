using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThoughtWorker_VeneratedAnimalOnMapOrCaravan : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptionArgs_003Ed__6 : IEnumerable<NamedArgument>, IEnumerable, IEnumerator<NamedArgument>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private NamedArgument _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		NamedArgument IEnumerator<NamedArgument>.Current
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
		public _003CGetDescriptionArgs_003Ed__6(int _003C_003E1__state)
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
				_003C_003E2__current = string.Concat("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": ", 1f.ToString(), ")").Named("STAGE1");
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = string.Concat("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": ", 2f.ToString(), ")").Named("STAGE2");
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = string.Concat("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": ", 4f.ToString(), ")").Named("STAGE3");
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = string.Concat("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": ", 6f.ToString(), ")").Named("STAGE4");
				_003C_003E1__state = 4;
				return true;
			case 4:
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
		IEnumerator<NamedArgument> IEnumerable<NamedArgument>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetDescriptionArgs_003Ed__6(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedArgument>)this).GetEnumerator();
		}
	}

	private const float FewAnimals = 1f;

	private const float SomeAnimals = 2f;

	private const float ManyAnimals = 4f;

	private const float LotsOfAnimals = 6f;

	public override string PostProcessLabel(Pawn p, string label)
	{
		Pawn pawn = PawnUtility.FirstVeneratedAnimalOnMapOrCaravan(p);
		if (pawn == null)
		{
			return label;
		}
		return label.Formatted(NamedArgumentUtility.Named(pawn.kindDef, "ANIMAL"));
	}

	protected override ThoughtState ShouldHaveThought(Pawn p)
	{
		if (p.IsSlave)
		{
			return false;
		}
		int num = ThoughtStageFromAnimalDensity(PawnUtility.PlayerVeneratedAnimalBodySizePerCapitaOnMapOrCaravan(p));
		if (num >= 0)
		{
			return ThoughtState.ActiveAtStage(num);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetDescriptionArgs_003Ed__6))]
	public IEnumerable<NamedArgument> GetDescriptionArgs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptionArgs_003Ed__6(-2);
	}

	private int ThoughtStageFromAnimalDensity(float density)
	{
		if (density <= 0f)
		{
			return -1;
		}
		if (density < 1f)
		{
			return 0;
		}
		if (density < 2f)
		{
			return 1;
		}
		if (density < 4f)
		{
			return 2;
		}
		if (density < 6f)
		{
			return 3;
		}
		return 4;
	}
}
