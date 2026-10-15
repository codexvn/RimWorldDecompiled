using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class TaleDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__15 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TaleDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__15(int _003C_003E1__state)
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
				TaleDef taleDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)taleDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d5;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0136;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0136;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0168;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
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
					if (taleDef.taleClass == null)
					{
						_003C_003E2__current = taleDef.defName + " taleClass is null.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d5;
					IL_0168:
					if (taleDef.firstPawnSymbol == "pawn" || taleDef.secondPawnSymbol == "pawn")
					{
						_003C_003E2__current = "pawn symbols should not be 'pawn', this is the default and only choice for SinglePawn tales so using it here is confusing.";
						_003C_003E1__state = 6;
						return true;
					}
					break;
					IL_00d5:
					if (taleDef.expireDays < 0f)
					{
						if (taleDef.type == TaleType.Expirable)
						{
							_003C_003E2__current = "Expirable tale type is used but expireDays<0";
							_003C_003E1__state = 3;
							return true;
						}
					}
					else if (taleDef.type != TaleType.Expirable)
					{
						_003C_003E2__current = "Non expirable tale type is used but expireDays>=0";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0136;
					IL_0136:
					if (taleDef.baseInterest > 1E-06f && !taleDef.usableForArt)
					{
						_003C_003E2__current = "Non-zero baseInterest but not usable for art";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0168;
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
			_003CConfigErrors_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__15(0)
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

	public TaleType type;

	public Type taleClass;

	public bool usableForArt = true;

	public bool colonistOnly = true;

	public bool usableWithChildren = true;

	public int maxPerPawn = -1;

	public float ignoreChance;

	public float expireDays = -1f;

	public RulePack rulePack;

	[NoTranslate]
	public string firstPawnSymbol;

	[NoTranslate]
	public string secondPawnSymbol;

	[NoTranslate]
	public string defSymbol;

	public Type defType = typeof(ThingDef);

	public float baseInterest;

	public Color historyGraphColor = Color.white;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__15))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static TaleDef Named(string str)
	{
		return DefDatabase<TaleDef>.GetNamed(str);
	}
}
