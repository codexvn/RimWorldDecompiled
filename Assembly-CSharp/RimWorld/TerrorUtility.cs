using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class TerrorUtility
{
	[CompilerGenerated]
	private sealed class _003CGetTerrorThoughts_003Ed__3 : IEnumerable<Thought_MemoryObservationTerror>, IEnumerable, IEnumerator<Thought_MemoryObservationTerror>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thought_MemoryObservationTerror _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Thought_Memory> _003Cmemories_003E5__2;

		private int _003Ci_003E5__3;

		Thought_MemoryObservationTerror IEnumerator<Thought_MemoryObservationTerror>.Current
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
		public _003CGetTerrorThoughts_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cmemories_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0076;
			}
			_003C_003E1__state = -1;
			_003Cmemories_003E5__2 = pawn.needs.mood.thoughts.memories.Memories;
			_003Ci_003E5__3 = 0;
			goto IL_0086;
			IL_0076:
			_003Ci_003E5__3++;
			goto IL_0086;
			IL_0086:
			if (_003Ci_003E5__3 < _003Cmemories_003E5__2.Count)
			{
				if (_003Cmemories_003E5__2[_003Ci_003E5__3] is Thought_MemoryObservationTerror thought_MemoryObservationTerror)
				{
					_003C_003E2__current = thought_MemoryObservationTerror;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0076;
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
		IEnumerator<Thought_MemoryObservationTerror> IEnumerable<Thought_MemoryObservationTerror>.GetEnumerator()
		{
			_003CGetTerrorThoughts_003Ed__3 _003CGetTerrorThoughts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTerrorThoughts_003Ed__ = this;
			}
			else
			{
				_003CGetTerrorThoughts_003Ed__ = new _003CGetTerrorThoughts_003Ed__3(0);
			}
			_003CGetTerrorThoughts_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetTerrorThoughts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thought_MemoryObservationTerror>)this).GetEnumerator();
		}
	}

	private const int MaxTerror = 100;

	private const int TerrorThoughtsToTake = 3;

	public static SimpleCurve SuppressionFallRateOverTerror = new SimpleCurve
	{
		new CurvePoint(0f, 0f),
		new CurvePoint(25f, -15f),
		new CurvePoint(50f, -25f),
		new CurvePoint(100f, -45f)
	};

	[IteratorStateMachine(typeof(_003CGetTerrorThoughts_003Ed__3))]
	public static IEnumerable<Thought_MemoryObservationTerror> GetTerrorThoughts(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTerrorThoughts_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public static IEnumerable<Thought_MemoryObservationTerror> TakeTopTerrorThoughts(IEnumerable<Thought_MemoryObservationTerror> thoughts)
	{
		return thoughts.OrderByDescending((Thought_MemoryObservationTerror t) => t.intensity).Take(3);
	}

	public static float GetTerrorLevel(this Pawn pawn)
	{
		int num = 0;
		if (!pawn.IsSlave)
		{
			return num;
		}
		foreach (Thought_MemoryObservationTerror terrorThought in GetTerrorThoughts(pawn))
		{
			num += terrorThought.intensity;
		}
		return (float)Mathf.Min(num, 100) / 100f;
	}

	public static void RemoveAllTerrorThoughts(Pawn pawn)
	{
		foreach (Thought_MemoryObservationTerror terrorThought in GetTerrorThoughts(pawn))
		{
			pawn.needs.mood.thoughts.memories.RemoveMemory(terrorThought);
		}
	}

	public static bool TryCreateTerrorThought(Thing thing, out Thought_MemoryObservationTerror thought)
	{
		thought = null;
		if (!ModsConfig.IdeologyActive)
		{
			return false;
		}
		float statValue = thing.GetStatValue(StatDefOf.TerrorSource);
		if (statValue <= 0f)
		{
			return false;
		}
		thought = (Thought_MemoryObservationTerror)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedTerror);
		thought.Target = thing;
		thought.intensity = (int)statValue;
		return true;
	}
}
