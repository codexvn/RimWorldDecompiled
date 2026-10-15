using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StatPart_AgeOffset : StatPart
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__7 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StatPart_AgeOffset _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__7(int _003C_003E1__state)
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
			StatPart_AgeOffset statPart_AgeOffset = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (statPart_AgeOffset.curve == null)
				{
					_003C_003E2__current = "curve is null.";
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__7(0)
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

	private SimpleCurve curve;

	private bool useBiologicalYears;

	private bool humanlikeOnly;

	private bool ActiveFor(Pawn pawn)
	{
		if (pawn.ageTracker == null)
		{
			return false;
		}
		if (humanlikeOnly && !pawn.RaceProps.Humanlike)
		{
			return false;
		}
		return true;
	}

	public override void TransformValue(StatRequest req, ref float val)
	{
		if (req.HasThing && req.Thing is Pawn pawn && ActiveFor(pawn))
		{
			val += AgeOffset(pawn);
		}
	}

	public override string ExplanationPart(StatRequest req)
	{
		if (req.HasThing && req.Thing is Pawn pawn && ActiveFor(pawn))
		{
			return "StatsReport_AgeOffset".Translate(pawn.ageTracker.AgeBiologicalYears) + ": " + AgeOffset(pawn).ToStringByStyle(parentStat.toStringStyle, parentStat.toStringNumberSense);
		}
		return null;
	}

	private float AgeOffset(Pawn pawn)
	{
		if (!useBiologicalYears)
		{
			return curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}
		return curve.Evaluate(pawn.ageTracker.AgeBiologicalYears);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__7))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
