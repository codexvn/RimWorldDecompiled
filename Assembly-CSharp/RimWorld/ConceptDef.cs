using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Steam;

namespace RimWorld;

public class ConceptDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__13 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ConceptDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003CConfigErrors_003Ed__13(int _003C_003E1__state)
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
				ConceptDef conceptDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)conceptDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c9;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00f6;
				case 4:
					_003C_003E1__state = -1;
					goto IL_012b;
				case 5:
					_003C_003E1__state = -1;
					_003Ci_003E5__3++;
					goto IL_0197;
				case 6:
					{
						_003C_003E1__state = -1;
						_003Ci_003E5__3++;
						goto IL_021f;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (conceptDef.priority > 9999999f)
					{
						_003C_003E2__current = "priority isn't set";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c9;
					IL_021f:
					if (_003Ci_003E5__3 < tmpParseErrors.Count)
					{
						_003C_003E2__current = "helpText error (controller): " + tmpParseErrors[_003Ci_003E5__3];
						_003C_003E1__state = 6;
						return true;
					}
					break;
					IL_0197:
					if (_003Ci_003E5__3 < tmpParseErrors.Count)
					{
						_003C_003E2__current = "helpText error: " + tmpParseErrors[_003Ci_003E5__3];
						_003C_003E1__state = 5;
						return true;
					}
					if (conceptDef.helpTextController.NullOrEmpty())
					{
						break;
					}
					tmpParseErrors.Clear();
					conceptDef.helpTextController.AdjustedForKeys(tmpParseErrors, resolveKeys: false);
					_003Ci_003E5__3 = 0;
					goto IL_021f;
					IL_00c9:
					if (conceptDef.helpText.NullOrEmpty())
					{
						_003C_003E2__current = "no help text";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00f6;
					IL_00f6:
					if (conceptDef.TriggeredDirect && conceptDef.label.NullOrEmpty())
					{
						_003C_003E2__current = "no label";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_012b;
					IL_012b:
					tmpParseErrors.Clear();
					conceptDef.helpText.AdjustedForKeys(tmpParseErrors, resolveKeys: false);
					_003Ci_003E5__3 = 0;
					goto IL_0197;
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
			_003CConfigErrors_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__13(0)
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

	public float priority = float.MaxValue;

	public bool noteTeaches;

	public bool needsOpportunity;

	public bool opportunityDecays = true;

	public ProgramState gameMode = ProgramState.Playing;

	[MustTranslate]
	private string helpText;

	[MustTranslate]
	private string helpTextController;

	[NoTranslate]
	public List<string> highlightTags;

	private static List<string> tmpParseErrors = new List<string>();

	public bool TriggeredDirect => priority <= 0f;

	public string HelpTextAdjusted
	{
		get
		{
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode || helpTextController.NullOrEmpty())
			{
				return helpText.AdjustedForKeys();
			}
			return helpTextController.AdjustedForKeys();
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__13))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static ConceptDef Named(string defName)
	{
		return DefDatabase<ConceptDef>.GetNamed(defName);
	}

	public void HighlightAllTags()
	{
		if (highlightTags != null)
		{
			for (int i = 0; i < highlightTags.Count; i++)
			{
				UIHighlighter.HighlightTag(highlightTags[i]);
			}
		}
	}
}
