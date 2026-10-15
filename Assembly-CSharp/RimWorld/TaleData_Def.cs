using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class TaleData_Def : TaleData
{
	[CompilerGenerated]
	private sealed class _003CGetRules_003Ed__4 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TaleData_Def _003C_003E4__this;

		private string prefix;

		public string _003C_003E3__prefix;

		Rule IEnumerator<Rule>.Current
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
		public _003CGetRules_003Ed__4(int _003C_003E1__state)
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
			TaleData_Def taleData_Def = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (taleData_Def.def != null)
				{
					_003C_003E2__current = new Rule_String(prefix + "_label", taleData_Def.def.label);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(taleData_Def.def.label));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(taleData_Def.def.label));
				_003C_003E1__state = 3;
				return true;
			case 3:
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CGetRules_003Ed__4 _003CGetRules_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRules_003Ed__ = this;
			}
			else
			{
				_003CGetRules_003Ed__ = new _003CGetRules_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetRules_003Ed__.prefix = _003C_003E3__prefix;
			return _003CGetRules_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	public Def def;

	private string tmpDefName;

	private Type tmpDefType;

	public override void ExposeData()
	{
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			tmpDefName = ((def != null) ? def.defName : null);
			tmpDefType = ((def != null) ? def.GetType() : null);
		}
		Scribe_Values.Look(ref tmpDefName, "defName");
		Scribe_Values.Look(ref tmpDefType, "defType");
		if (Scribe.mode == LoadSaveMode.LoadingVars && tmpDefName != null)
		{
			def = GenDefDatabase.GetDef(tmpDefType, BackCompatibility.BackCompatibleDefName(tmpDefType, tmpDefName));
		}
	}

	[IteratorStateMachine(typeof(_003CGetRules_003Ed__4))]
	public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRules_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__prefix = prefix
		};
	}

	public static TaleData_Def GenerateFrom(Def def)
	{
		return new TaleData_Def
		{
			def = def
		};
	}
}
