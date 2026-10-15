using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StatPart_Glow : StatPart
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__5 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StatPart_Glow _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__5(int _003C_003E1__state)
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
			StatPart_Glow statPart_Glow = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (statPart_Glow.factorFromGlowCurve == null)
				{
					_003C_003E2__current = "factorFromLightCurve is null.";
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
			_003CConfigErrors_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__5(0)
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

	private bool pawnOnly;

	private bool humanlikeOnly;

	private SimpleCurve factorFromGlowCurve;

	private bool ignoreIfPrefersDarkness;

	private bool ignoreIfIncapableOfSight;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__5))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void TransformValue(StatRequest req, ref float val)
	{
		if (req.HasThing && ActiveFor(req.Thing))
		{
			val *= FactorFromGlow(req.Thing);
		}
	}

	public override string ExplanationPart(StatRequest req)
	{
		if (req.HasThing && ActiveFor(req.Thing))
		{
			return "StatsReport_LightMultiplier".Translate(GlowLevel(req.Thing).ToStringPercent()) + ": x" + FactorFromGlow(req.Thing).ToStringPercent();
		}
		return null;
	}

	private bool ActiveFor(Thing t)
	{
		if (t is Pawn pawn)
		{
			if (humanlikeOnly && !pawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (pawn.IsShambler)
			{
				return false;
			}
			if (ignoreIfIncapableOfSight && PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
			{
				return false;
			}
			if (ignoreIfPrefersDarkness)
			{
				if (pawn.Ideo != null && pawn.Ideo.IdeoPrefersDarkness())
				{
					return false;
				}
				if (pawn.genes != null && !pawn.genes.AffectedByDarkness)
				{
					return false;
				}
			}
		}
		else if (pawnOnly)
		{
			return false;
		}
		return t.Spawned;
	}

	private float GlowLevel(Thing t)
	{
		return t.Map.glowGrid.GroundGlowAt(t.Position);
	}

	private float FactorFromGlow(Thing t)
	{
		return factorFromGlowCurve.Evaluate(GlowLevel(t));
	}
}
