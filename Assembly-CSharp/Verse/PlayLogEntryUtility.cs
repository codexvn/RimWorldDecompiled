using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse.Grammar;

namespace Verse;

public static class PlayLogEntryUtility
{
	[CompilerGenerated]
	private sealed class _003CRulesForDamagedParts_003Ed__1 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<BodyPartRecord> bodyParts;

		public List<BodyPartRecord> _003C_003E3__bodyParts;

		private string prefix;

		public string _003C_003E3__prefix;

		private Dictionary<string, string> constants;

		public Dictionary<string, string> _003C_003E3__constants;

		private List<bool> bodyPartsDestroyed;

		public List<bool> _003C_003E3__bodyPartsDestroyed;

		private int _003CdestroyedIndex_003E5__2;

		private int _003CdamagedIndex_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003CRulesForDamagedParts_003Ed__1(int _003C_003E1__state)
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
			int num;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (bodyParts != null)
				{
					_003CdestroyedIndex_003E5__2 = 0;
					_003CdamagedIndex_003E5__3 = 0;
					_003Ci_003E5__4 = 0;
					goto IL_05a9;
				}
				constants[prefix + "_count"] = "0";
				constants[prefix + "_destroyed_count"] = "0";
				constants[prefix + "_damaged_count"] = "0";
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(string.Format(prefix + "{0}_definite", _003Ci_003E5__4), Find.ActiveLanguageWorker.WithDefiniteArticle(bodyParts[_003Ci_003E5__4].Label));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(string.Format(prefix + "{0}_indefinite", _003Ci_003E5__4), Find.ActiveLanguageWorker.WithIndefiniteArticle(bodyParts[_003Ci_003E5__4].Label));
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				constants[string.Format(prefix + "{0}_destroyed", _003Ci_003E5__4)] = bodyPartsDestroyed[_003Ci_003E5__4].ToString();
				constants[string.Format(prefix + "{0}_gender", _003Ci_003E5__4)] = LanguageDatabase.activeLanguage.ResolveGender(bodyParts[_003Ci_003E5__4].Label).ToString();
				if (bodyPartsDestroyed[_003Ci_003E5__4])
				{
					_003C_003E2__current = new Rule_String(string.Format(prefix + "_destroyed{0}_label", _003CdestroyedIndex_003E5__2), bodyParts[_003Ci_003E5__4].Label);
					_003C_003E1__state = 4;
					return true;
				}
				_003C_003E2__current = new Rule_String(string.Format(prefix + "_damaged{0}_label", _003CdamagedIndex_003E5__3), bodyParts[_003Ci_003E5__4].Label);
				_003C_003E1__state = 7;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(string.Format(prefix + "_destroyed{0}_definite", _003CdestroyedIndex_003E5__2), Find.ActiveLanguageWorker.WithDefiniteArticle(bodyParts[_003Ci_003E5__4].Label));
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(string.Format(prefix + "_destroyed{0}_indefinite", _003CdestroyedIndex_003E5__2), Find.ActiveLanguageWorker.WithIndefiniteArticle(bodyParts[_003Ci_003E5__4].Label));
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				constants[$"{prefix}_destroyed{_003CdestroyedIndex_003E5__2}_outside"] = (bodyParts[_003Ci_003E5__4].depth == BodyPartDepth.Outside).ToString();
				constants[$"{prefix}_destroyed{_003CdestroyedIndex_003E5__2}_gender"] = LanguageDatabase.activeLanguage.ResolveGender(bodyParts[_003Ci_003E5__4].Label).ToString();
				_003CdestroyedIndex_003E5__2++;
				goto IL_0599;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(string.Format(prefix + "_damaged{0}_definite", _003CdamagedIndex_003E5__3), Find.ActiveLanguageWorker.WithDefiniteArticle(bodyParts[_003Ci_003E5__4].Label));
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(string.Format(prefix + "_damaged{0}_indefinite", _003CdamagedIndex_003E5__3), Find.ActiveLanguageWorker.WithIndefiniteArticle(bodyParts[_003Ci_003E5__4].Label));
				_003C_003E1__state = 9;
				return true;
			case 9:
				{
					_003C_003E1__state = -1;
					constants[$"{prefix}_damaged{_003CdamagedIndex_003E5__3}_outside"] = (bodyParts[_003Ci_003E5__4].depth == BodyPartDepth.Outside).ToString();
					constants[$"{prefix}_damaged{_003CdamagedIndex_003E5__3}_gender"] = LanguageDatabase.activeLanguage.ResolveGender(bodyParts[_003Ci_003E5__4].Label).ToString();
					_003CdamagedIndex_003E5__3++;
					goto IL_0599;
				}
				IL_0599:
				num = _003Ci_003E5__4 + 1;
				_003Ci_003E5__4 = num;
				goto IL_05a9;
				IL_05a9:
				if (_003Ci_003E5__4 < bodyParts.Count)
				{
					_003C_003E2__current = new Rule_String(string.Format(prefix + "{0}_label", _003Ci_003E5__4), bodyParts[_003Ci_003E5__4].Label);
					_003C_003E1__state = 1;
					return true;
				}
				constants[prefix + "_count"] = bodyParts.Count.ToString();
				constants[prefix + "_destroyed_count"] = _003CdestroyedIndex_003E5__2.ToString();
				constants[prefix + "_damaged_count"] = _003CdamagedIndex_003E5__3.ToString();
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
			_003CRulesForDamagedParts_003Ed__1 _003CRulesForDamagedParts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForDamagedParts_003Ed__ = this;
			}
			else
			{
				_003CRulesForDamagedParts_003Ed__ = new _003CRulesForDamagedParts_003Ed__1(0);
			}
			_003CRulesForDamagedParts_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForDamagedParts_003Ed__.bodyParts = _003C_003E3__bodyParts;
			_003CRulesForDamagedParts_003Ed__.bodyPartsDestroyed = _003C_003E3__bodyPartsDestroyed;
			_003CRulesForDamagedParts_003Ed__.constants = _003C_003E3__constants;
			return _003CRulesForDamagedParts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForOptionalWeapon_003Ed__0 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef weaponDef;

		public ThingDef _003C_003E3__weaponDef;

		private string prefix;

		public string _003C_003E3__prefix;

		private ThingDef projectileDef;

		public ThingDef _003C_003E3__projectileDef;

		private IEnumerator<Rule> _003C_003E7__wrap1;

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
		public _003CRulesForOptionalWeapon_003Ed__0(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				ThingDef thingDef;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (weaponDef == null)
					{
						break;
					}
					_003C_003E7__wrap1 = GrammarUtility.RulesForDef(prefix, weaponDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0081;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0081;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_0124;
					}
					IL_0124:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					break;
					IL_0081:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					thingDef = projectileDef;
					if (thingDef == null && !weaponDef.Verbs.NullOrEmpty())
					{
						thingDef = weaponDef.Verbs[0].defaultProjectile;
					}
					if (thingDef == null)
					{
						break;
					}
					_003C_003E7__wrap1 = GrammarUtility.RulesForDef(prefix + "_projectile", thingDef).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0124;
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

		private void _003C_003Em__Finally2()
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForOptionalWeapon_003Ed__0 _003CRulesForOptionalWeapon_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForOptionalWeapon_003Ed__ = this;
			}
			else
			{
				_003CRulesForOptionalWeapon_003Ed__ = new _003CRulesForOptionalWeapon_003Ed__0(0);
			}
			_003CRulesForOptionalWeapon_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForOptionalWeapon_003Ed__.weaponDef = _003C_003E3__weaponDef;
			_003CRulesForOptionalWeapon_003Ed__.projectileDef = _003C_003E3__projectileDef;
			return _003CRulesForOptionalWeapon_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CRulesForOptionalWeapon_003Ed__0))]
	public static IEnumerable<Rule> RulesForOptionalWeapon(string prefix, ThingDef weaponDef, ThingDef projectileDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForOptionalWeapon_003Ed__0(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__weaponDef = weaponDef,
			_003C_003E3__projectileDef = projectileDef
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForDamagedParts_003Ed__1))]
	public static IEnumerable<Rule> RulesForDamagedParts(string prefix, BodyDef body, List<BodyPartRecord> bodyParts, List<bool> bodyPartsDestroyed, Dictionary<string, string> constants)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForDamagedParts_003Ed__1(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__bodyParts = bodyParts,
			_003C_003E3__bodyPartsDestroyed = bodyPartsDestroyed,
			_003C_003E3__constants = constants
		};
	}
}
