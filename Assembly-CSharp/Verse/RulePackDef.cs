using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse.Grammar;

namespace Verse;

public class RulePackDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__17 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RulePackDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__17(int _003C_003E1__state)
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
				RulePackDef rulePackDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)rulePackDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_0111;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (rulePackDef.include == null)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_0123;
					IL_0111:
					_003Ci_003E5__3++;
					goto IL_0123;
					IL_0123:
					if (_003Ci_003E5__3 >= rulePackDef.include.Count)
					{
						break;
					}
					if (rulePackDef.include[_003Ci_003E5__3].include != null && rulePackDef.include[_003Ci_003E5__3].include.Contains(rulePackDef))
					{
						_003C_003E2__current = "includes other RulePackDef which includes it: " + rulePackDef.include[_003Ci_003E5__3].defName;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0111;
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
			_003CConfigErrors_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__17(0)
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

	public List<RulePackDef> include;

	private RulePack rulePack;

	public bool directTestable;

	[Unsaved(false)]
	private List<Rule> cachedRules;

	[Unsaved(false)]
	private List<Rule> cachedUntranslatedRules;

	public List<Rule> RulesPlusIncludes
	{
		get
		{
			if (cachedRules == null)
			{
				cachedRules = new List<Rule>();
				if (rulePack != null)
				{
					cachedRules.AddRange(rulePack.Rules);
				}
				if (include != null)
				{
					for (int i = 0; i < include.Count; i++)
					{
						cachedRules.AddRange(include[i].RulesPlusIncludes);
					}
				}
			}
			return cachedRules;
		}
	}

	public List<Rule> UntranslatedRulesPlusIncludes
	{
		get
		{
			if (cachedUntranslatedRules == null)
			{
				cachedUntranslatedRules = new List<Rule>();
				if (rulePack != null)
				{
					cachedUntranslatedRules.AddRange(rulePack.UntranslatedRules);
				}
				if (include != null)
				{
					for (int i = 0; i < include.Count; i++)
					{
						cachedUntranslatedRules.AddRange(include[i].UntranslatedRulesPlusIncludes);
					}
				}
			}
			return cachedUntranslatedRules;
		}
	}

	public List<Rule> RulesImmediate => rulePack?.Rules;

	public List<Rule> UntranslatedRulesImmediate => rulePack?.UntranslatedRules;

	public string FirstRuleKeyword
	{
		get
		{
			List<Rule> rulesPlusIncludes = RulesPlusIncludes;
			if (!rulesPlusIncludes.Any())
			{
				return "none";
			}
			return rulesPlusIncludes[0].keyword;
		}
	}

	public string FirstUntranslatedRuleKeyword
	{
		get
		{
			List<Rule> untranslatedRulesPlusIncludes = UntranslatedRulesPlusIncludes;
			if (!untranslatedRulesPlusIncludes.Any())
			{
				return "none";
			}
			return untranslatedRulesPlusIncludes[0].keyword;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__17))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static RulePackDef Named(string defName)
	{
		return DefDatabase<RulePackDef>.GetNamed(defName);
	}
}
