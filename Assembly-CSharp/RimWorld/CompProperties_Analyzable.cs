using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class CompProperties_Analyzable : CompProperties_Interactable
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__16 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Analyzable _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

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
		public _003CConfigErrors_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 4)
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
				CompProperties_Analyzable compProperties_Analyzable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!string.IsNullOrEmpty(compProperties_Analyzable.progressedLetterLabel) && !compProperties_Analyzable.progressedLetters.NullOrEmpty() && compProperties_Analyzable.progressedLetterDef == null)
					{
						_003C_003E2__current = "Letter def is null (progressedLetterDef) for analyzable component";
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0078;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ba;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00f9;
				case 4:
					{
						_003C_003E1__state = -3;
						break;
					}
					IL_00f9:
					_003C_003E7__wrap1 = ((CompProperties_Interactable)compProperties_Analyzable).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					break;
					IL_00ba:
					if ((!string.IsNullOrEmpty(compProperties_Analyzable.repeatCompletedLetterLabel) || !string.IsNullOrEmpty(compProperties_Analyzable.repeatCompletedLetter)) && compProperties_Analyzable.repeatCompletedLetterDef == null)
					{
						_003C_003E2__current = "Letter def is null (repeatCompletedLetterDef) for analyzable component";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00f9;
					IL_0078:
					if ((!string.IsNullOrEmpty(compProperties_Analyzable.completedLetterLabel) || !string.IsNullOrEmpty(compProperties_Analyzable.completedLetter)) && compProperties_Analyzable.completedLetterDef == null)
					{
						_003C_003E2__current = "Letter def is null (completedLetterDef) for analyzable component";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ba;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 4;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			_003CConfigErrors_003Ed__16 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__16(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public IntRange analysisRequiredRange = new IntRange(1, 1);

	public bool destroyedOnAnalyzed;

	public float analysisDurationHours = 1.5f;

	public bool showProgress;

	public bool allowRepeatAnalysis;

	[MustTranslate]
	public string progressedLetterLabel;

	[MustTranslate]
	public List<string> progressedLetters = new List<string>();

	public LetterDef progressedLetterDef;

	[MustTranslate]
	public string completedLetterLabel;

	[MustTranslate]
	public string completedLetter;

	public LetterDef completedLetterDef;

	[MustTranslate]
	public string repeatCompletedLetterLabel;

	[MustTranslate]
	public string repeatCompletedLetter;

	public LetterDef repeatCompletedLetterDef;

	public bool canStudyInPlace;

	public CompProperties_Analyzable()
	{
		compClass = typeof(CompAnalyzable);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__16))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__16(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
