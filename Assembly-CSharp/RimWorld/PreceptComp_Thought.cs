using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class PreceptComp_Thought : PreceptComp
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptions_003Ed__6 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptComp_Thought _003C_003E4__this;

		private int _003Cstage_003E5__2;

		private List<string>.Enumerator _003C_003E7__wrap2;

		private IEnumerator<string> _003C_003E7__wrap3;

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
		public _003CGetDescriptions_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap2 = default(List<string>.Enumerator);
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PreceptComp_Thought preceptComp_Thought = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!preceptComp_Thought.thoughtStageDescriptions.NullOrEmpty())
					{
						_003Cstage_003E5__2 = 0;
						_003C_003E7__wrap2 = preceptComp_Thought.thoughtStageDescriptions.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00a7;
					}
					_003C_003E7__wrap3 = ((PreceptComp)preceptComp_Thought).GetDescriptions().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0112;
				case 1:
					_003C_003E1__state = -3;
					_003Cstage_003E5__2++;
					goto IL_00a7;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_0112;
					}
					IL_00a7:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = preceptComp_Thought.ParseDescription(current, _003Cstage_003E5__2);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = default(List<string>.Enumerator);
					break;
					IL_0112:
					if (_003C_003E7__wrap3.MoveNext())
					{
						string current2 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = preceptComp_Thought.ParseDescription(current2);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
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
			_003CGetDescriptions_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDescriptions_003Ed__6(0)
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

	public ThoughtDef thought;

	[MustTranslate]
	public List<string> thoughtStageDescriptions;

	public bool tooltipShowMoodRange;

	public bool AffectsMood
	{
		get
		{
			for (int i = 0; i < thought.stages.Count; i++)
			{
				if (thought.stages[i].baseMoodEffect != 0f)
				{
					return true;
				}
			}
			return false;
		}
	}

	private string ParseDescription(string description, int thoughtStage = -1)
	{
		bool flag = thoughtStage != -1;
		if (!flag)
		{
			thoughtStage = 0;
		}
		if (!description.NullOrEmpty() && thought.Worker is IPreceptCompDescriptionArgs preceptCompDescriptionArgs)
		{
			description = description.Formatted(preceptCompDescriptionArgs.GetDescriptionArgs());
		}
		if (description.NullOrEmpty())
		{
			description = ((!thought.stages[thoughtStage].LabelAbstractCap.NullOrEmpty()) ? thought.stages[thoughtStage].LabelAbstractCap : thought.LabelCap.Resolve());
			description = description.Formatted(Gender.Male.Named("PAWN_gender"));
		}
		if (AffectsMood)
		{
			ThoughtStage thoughtStage2 = thought.stages[thoughtStage];
			if (thought.minExpectation != null)
			{
				description += " (" + "MinExpectation".Translate() + ": " + thought.minExpectation.LabelCap + ")";
			}
			else if (thought.minExpectationForNegativeThought != null && thoughtStage2.baseMoodEffect < 0f)
			{
				description += " (" + "MinExpectation".Translate() + ": " + thought.minExpectationForNegativeThought.LabelCap + ")";
			}
			string text = "";
			if (tooltipShowMoodRange && !flag)
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				for (int i = 0; i < thought.stages.Count; i++)
				{
					num = Mathf.Min(num, thought.stages[i].baseMoodEffect);
					num2 = Mathf.Max(num2, thought.stages[i].baseMoodEffect);
				}
				text = "PreceptThoughtMoodRange".Translate(num.ToStringWithSign("F0"), num2.ToStringWithSign("F0"));
			}
			else
			{
				text = thoughtStage2.baseMoodEffect.ToStringWithSign("F0");
			}
			return description + ": " + text;
		}
		return description + ": " + thought.stages[thoughtStage].baseOpinionOffset.ToStringWithSign("F0");
	}

	[IteratorStateMachine(typeof(_003CGetDescriptions_003Ed__6))]
	public override IEnumerable<string> GetDescriptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptions_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
