using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Steam;

namespace RimWorld;

public class InstructionDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__24 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public InstructionDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__24(int _003C_003E1__state)
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
				InstructionDef instructionDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)instructionDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ca;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00f7;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0124;
				case 5:
					_003C_003E1__state = -1;
					_003Ci_003E5__3++;
					goto IL_0190;
				case 6:
					{
						_003C_003E1__state = -1;
						_003Ci_003E5__3++;
						goto IL_0218;
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
					if (instructionDef.instructionClass == null)
					{
						_003C_003E2__current = "no instruction class";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ca;
					IL_0124:
					tmpParseErrors.Clear();
					instructionDef.text.AdjustedForKeys(tmpParseErrors, resolveKeys: false);
					_003Ci_003E5__3 = 0;
					goto IL_0190;
					IL_00ca:
					if (instructionDef.text.NullOrEmpty())
					{
						_003C_003E2__current = "no text";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00f7;
					IL_0190:
					if (_003Ci_003E5__3 < tmpParseErrors.Count)
					{
						_003C_003E2__current = "text error: " + tmpParseErrors[_003Ci_003E5__3];
						_003C_003E1__state = 5;
						return true;
					}
					if (instructionDef.textController.NullOrEmpty())
					{
						break;
					}
					tmpParseErrors.Clear();
					instructionDef.textController.AdjustedForKeys(tmpParseErrors, resolveKeys: false);
					_003Ci_003E5__3 = 0;
					goto IL_0218;
					IL_00f7:
					if (instructionDef.eventTagInitiate.NullOrEmpty())
					{
						_003C_003E2__current = "no eventTagInitiate";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0124;
					IL_0218:
					if (_003Ci_003E5__3 < tmpParseErrors.Count)
					{
						_003C_003E2__current = "text error (controller): " + tmpParseErrors[_003Ci_003E5__3];
						_003C_003E1__state = 6;
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
			_003CConfigErrors_003Ed__24 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__24(0)
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

	public Type instructionClass = typeof(Instruction_Basic);

	[MustTranslate]
	public string text;

	[MustTranslate]
	public string textController;

	public bool startCentered;

	public bool tutorialModeOnly = true;

	[NoTranslate]
	public string eventTagInitiate;

	public InstructionDef eventTagInitiateSource;

	[NoTranslate]
	public List<string> eventTagsEnd;

	[NoTranslate]
	public List<string> actionTagsAllowed;

	[MustTranslate]
	public string rejectInputMessage;

	public ConceptDef concept;

	[NoTranslate]
	public List<string> highlightTags;

	[MustTranslate]
	public string onMapInstruction;

	public int targetCount;

	public ThingDef thingDef;

	public RecipeDef recipeDef;

	public int recipeTargetCount = 1;

	public ThingDef giveOnActivateDef;

	public int giveOnActivateCount;

	public bool endTutorial;

	public bool resetBuildDesignatorStuffs;

	private static List<string> tmpParseErrors = new List<string>();

	public string Text
	{
		get
		{
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode || textController.NullOrEmpty())
			{
				return text;
			}
			return textController;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__24))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__24(-2)
		{
			_003C_003E4__this = this
		};
	}
}
