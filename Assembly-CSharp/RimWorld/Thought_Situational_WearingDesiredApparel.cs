using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Thought_Situational_WearingDesiredApparel : Thought_Situational
{
	[CompilerGenerated]
	private sealed class _003CMetApparelPrecepts_003Ed__8 : IEnumerable<Precept_Apparel>, IEnumerable, IEnumerator<Precept_Apparel>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Precept_Apparel _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Thought_Situational_WearingDesiredApparel _003C_003E4__this;

		private List<Precept>.Enumerator _003C_003E7__wrap1;

		Precept_Apparel IEnumerator<Precept_Apparel>.Current
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
		public _003CMetApparelPrecepts_003Ed__8(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Precept>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Thought_Situational_WearingDesiredApparel thought_Situational_WearingDesiredApparel = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (!ModsConfig.IdeologyActive || thought_Situational_WearingDesiredApparel.pawn.Ideo == null)
					{
						return false;
					}
					List<Precept> allPreceptsAllowingSituationalThought = thought_Situational_WearingDesiredApparel.pawn.Ideo.GetAllPreceptsAllowingSituationalThought(thought_Situational_WearingDesiredApparel.def);
					_003C_003E7__wrap1 = allPreceptsAllowingSituationalThought.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					if (_003C_003E7__wrap1.Current is Precept_Apparel precept_Apparel && (precept_Apparel.TargetGender == Gender.None || precept_Apparel.TargetGender == thought_Situational_WearingDesiredApparel.pawn.gender) && thought_Situational_WearingDesiredApparel.HasApparel(precept_Apparel.apparelDef))
					{
						_003C_003E2__current = precept_Apparel;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Precept>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Precept_Apparel> IEnumerable<Precept_Apparel>.GetEnumerator()
		{
			_003CMetApparelPrecepts_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMetApparelPrecepts_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Precept_Apparel>)this).GetEnumerator();
		}
	}

	public bool multipleApparelItems;

	public override string LabelCap => base.CurStage.label.Formatted(multipleApparelItems ? "WearingDesiredApparelMulti".Translate().Named("APPAREL") : MetApparelPrecepts().FirstOrFallback(SourceApparelPrecept).apparelDef.label.Named("APPAREL")).CapitalizeFirst();

	public override string Description => base.CurStage.description.Formatted(multipleApparelItems ? "WearingDesiredApparelMulti".Translate().Named("APPAREL") : Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(MetApparelPrecepts().FirstOrFallback(SourceApparelPrecept).apparelDef.label).Named("APPAREL"), pawn.Named("PAWN")).CapitalizeFirst() + base.CausedByBeliefInPrecept;

	private Precept_Apparel SourceApparelPrecept => (Precept_Apparel)sourcePrecept;

	protected override ThoughtState CurrentStateInternal()
	{
		if (!ModsConfig.IdeologyActive || pawn.Ideo == null)
		{
			return ThoughtState.Inactive;
		}
		List<Precept> allPreceptsAllowingSituationalThought = pawn.Ideo.GetAllPreceptsAllowingSituationalThought(def);
		if (allPreceptsAllowingSituationalThought.Count == 0)
		{
			return ThoughtState.Inactive;
		}
		int num = 0;
		foreach (Precept item in allPreceptsAllowingSituationalThought)
		{
			if (item is Precept_Apparel precept_Apparel && (precept_Apparel.TargetGender == Gender.None || precept_Apparel.TargetGender == pawn.gender))
			{
				if (!HasApparel(precept_Apparel.apparelDef))
				{
					return ThoughtState.Inactive;
				}
				num++;
			}
		}
		multipleApparelItems = num > 1;
		if (num != 0)
		{
			return base.CurrentStateInternal();
		}
		return ThoughtState.Inactive;
	}

	[IteratorStateMachine(typeof(_003CMetApparelPrecepts_003Ed__8))]
	private IEnumerable<Precept_Apparel> MetApparelPrecepts()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMetApparelPrecepts_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	private bool HasApparel(ThingDef thingDef)
	{
		foreach (Apparel item in pawn.apparel.WornApparel)
		{
			if (item.def == thingDef)
			{
				return true;
			}
		}
		return false;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref multipleApparelItems, "multipleApparelItems", defaultValue: false);
	}
}
