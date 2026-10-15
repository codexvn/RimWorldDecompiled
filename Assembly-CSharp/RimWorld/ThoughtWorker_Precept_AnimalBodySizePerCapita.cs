using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThoughtWorker_Precept_AnimalBodySizePerCapita : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptionArgs_003Ed__10 : IEnumerable<NamedArgument>, IEnumerable, IEnumerator<NamedArgument>, IEnumerator, IDisposable
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
		public _003CGetDescriptionArgs_003Ed__10(int _003C_003E1__state)
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
				_003C_003E2__current = string.Concat("(" + "AnimalsBodySizePerColonist".Translate() + ": ", 1f.ToString(), ")").Named("STAGE1");
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = string.Concat("(" + "AnimalsBodySizePerColonist".Translate() + ": ", 2f.ToString(), ")").Named("STAGE2");
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = string.Concat("(" + "AnimalsBodySizePerColonist".Translate() + ": ", 6f.ToString(), ")").Named("STAGE4");
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = string.Concat("(" + "AnimalsBodySizePerColonist".Translate() + ": ", 8f.ToString(), ")").Named("STAGE5");
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
			return new _003CGetDescriptionArgs_003Ed__10(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedArgument>)this).GetEnumerator();
		}
	}

	private const float NoAnimals = 0f;

	private const float ScarceAnimals = 1f;

	private const float FewAnimals = 2f;

	private const float NoThought = 4f;

	private const float SomeAnimals = 6f;

	private const float LotsOfAnimals = 8f;

	private const int MinimumTicksBeforeFewAnimals = 900000;

	protected override ThoughtState ShouldHaveThought(Pawn p)
	{
		if (ThoughtUtility.ThoughtNullified(p, def))
		{
			return false;
		}
		if (p.IsSlave)
		{
			return false;
		}
		float num = PawnUtility.PlayerAnimalBodySizePerCapita();
		if (num <= 2f && GenTicks.TicksAbs < 900000)
		{
			return false;
		}
		if (num < 4f && def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < def.minExpectationForNegativeThought.order)
		{
			return false;
		}
		if (ThoughtStageFromAnimalDensity(num) < 0)
		{
			return false;
		}
		return ThoughtState.ActiveAtStage(ThoughtStageFromAnimalDensity(PawnUtility.PlayerAnimalBodySizePerCapita()));
	}

	public override string PostProcessDescription(Pawn p, string description)
	{
		return base.PostProcessDescription(p, description) + "\n\n" + "CurrentTotalAnimalBodySizePerColonist".Translate() + ": " + PawnUtility.PlayerAnimalBodySizePerCapita().ToString("F1") + "\n" + "MinAnimalBodySizePerColonist".Translate(4f.ToString("F1"));
	}

	private int ThoughtStageFromAnimalDensity(float density)
	{
		if (density < 0f)
		{
			return 0;
		}
		if (density < 1f)
		{
			return 1;
		}
		if (density < 2f)
		{
			return 2;
		}
		if (density < 4f)
		{
			return -1;
		}
		if (density < 6f)
		{
			return 3;
		}
		if (density < 8f)
		{
			return 4;
		}
		return 5;
	}

	[IteratorStateMachine(typeof(_003CGetDescriptionArgs_003Ed__10))]
	public IEnumerable<NamedArgument> GetDescriptionArgs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptionArgs_003Ed__10(-2);
	}
}
