using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Tale : IExposable, ILoadReferenceable
{
	[CompilerGenerated]
	private sealed class _003CGetTextGenerationIncludes_003Ed__28 : IEnumerable<RulePack>, IEnumerable, IEnumerator<RulePack>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RulePack _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Tale _003C_003E4__this;

		RulePack IEnumerator<RulePack>.Current
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
		public _003CGetTextGenerationIncludes_003Ed__28(int _003C_003E1__state)
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
			Tale tale = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tale.def.rulePack != null)
				{
					_003C_003E2__current = tale.def.rulePack;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
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
		IEnumerator<RulePack> IEnumerable<RulePack>.GetEnumerator()
		{
			_003CGetTextGenerationIncludes_003Ed__28 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetTextGenerationIncludes_003Ed__28(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RulePack>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTextGenerationRules_003Ed__29 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Tale _003C_003E4__this;

		private Dictionary<string, string> outConstants;

		public Dictionary<string, string> _003C_003E3__outConstants;

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
		public _003CGetTextGenerationRules_003Ed__29(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
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
				int num = _003C_003E1__state;
				Tale tale = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					Vector2 location = Vector2.zero;
					if (tale.surroundings != null && tale.surroundings.tile.Valid)
					{
						location = Find.WorldGrid.LongLatOf(tale.surroundings.tile);
					}
					_003C_003E2__current = new Rule_String("DATE", GenDate.DateFullStringAt(tale.date, location));
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					if (tale.surroundings != null)
					{
						_003C_003E7__wrap1 = tale.surroundings.GetRules().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00ee;
					}
					goto IL_0108;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00ee;
				case 3:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0108:
					_003C_003E7__wrap1 = tale.SpecialTextGenerationRules(outConstants).GetEnumerator();
					_003C_003E1__state = -4;
					break;
					IL_00ee:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					goto IL_0108;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Rule current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally2();
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
			_003CGetTextGenerationRules_003Ed__29 _003CGetTextGenerationRules_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTextGenerationRules_003Ed__ = this;
			}
			else
			{
				_003CGetTextGenerationRules_003Ed__ = new _003CGetTextGenerationRules_003Ed__29(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTextGenerationRules_003Ed__.outConstants = _003C_003E3__outConstants;
			return _003CGetTextGenerationRules_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	public TaleDef def;

	public int id;

	private int uses;

	public int date = -1;

	public TaleData_Surroundings surroundings;

	public string customLabel;

	public bool hidden;

	public int AgeTicks => Find.TickManager.TicksAbs - date;

	public int Uses => uses;

	public bool Unused => uses == 0;

	public virtual Pawn DominantPawn => null;

	public float InterestLevel
	{
		get
		{
			float baseInterest = def.baseInterest;
			baseInterest /= (float)(1 + uses * 3);
			float a = 0f;
			switch (def.type)
			{
			case TaleType.Volatile:
				a = 50f;
				break;
			case TaleType.PermanentHistorical:
				a = 50f;
				break;
			case TaleType.Expirable:
				a = def.expireDays;
				break;
			}
			float value = AgeTicks / 60000;
			baseInterest *= Mathf.InverseLerp(a, 0f, value);
			if (baseInterest < 0.01f)
			{
				baseInterest = 0.01f;
			}
			return baseInterest;
		}
	}

	public bool Expired
	{
		get
		{
			if (!Unused)
			{
				return false;
			}
			if (def.type != TaleType.Expirable)
			{
				return false;
			}
			return (float)AgeTicks > def.expireDays * 60000f;
		}
	}

	public virtual string ShortSummary
	{
		get
		{
			if (!customLabel.NullOrEmpty())
			{
				return customLabel.CapitalizeFirst();
			}
			return def.LabelCap;
		}
	}

	public virtual void GenerateTestData()
	{
		if (Find.CurrentMap == null)
		{
			Log.Error("Can't generate test data because there is no map.");
		}
		date = Rand.Range(-108000000, -7200000);
		surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
	}

	public virtual bool Concerns(Thing th)
	{
		return false;
	}

	public virtual void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_Values.Look(ref id, "id", 0);
		Scribe_Values.Look(ref uses, "uses", 0);
		Scribe_Values.Look(ref date, "date", 0);
		Scribe_Deep.Look(ref surroundings, "surroundings");
		Scribe_Values.Look(ref customLabel, "customLabel");
		Scribe_Values.Look(ref hidden, "hidden", defaultValue: false);
	}

	public void Notify_NewlyUsed()
	{
		uses++;
	}

	public void Notify_ReferenceDestroyed()
	{
		if (uses == 0)
		{
			Log.Warning("Called reference destroyed method on tale " + this?.ToString() + " but uses count is 0.");
		}
		else
		{
			uses--;
		}
	}

	public virtual void Notify_FactionRemoved(Faction faction)
	{
	}

	[IteratorStateMachine(typeof(_003CGetTextGenerationIncludes_003Ed__28))]
	public IEnumerable<RulePack> GetTextGenerationIncludes()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTextGenerationIncludes_003Ed__28(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetTextGenerationRules_003Ed__29))]
	public IEnumerable<Rule> GetTextGenerationRules(Dictionary<string, string> outConstants = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTextGenerationRules_003Ed__29(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__outConstants = outConstants
		};
	}

	protected virtual IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
	{
		return Enumerable.Empty<Rule>();
	}

	public string GetUniqueLoadID()
	{
		return "Tale_" + id;
	}

	public override int GetHashCode()
	{
		return id;
	}

	public override string ToString()
	{
		string text = "(#" + id + ": " + ShortSummary + "(age=" + ((float)AgeTicks / 60000f).ToString("F2") + " interest=" + InterestLevel;
		if (Unused && def.type == TaleType.Expirable)
		{
			text = text + ", expireDays=" + def.expireDays.ToString("F2");
		}
		return text + ")";
	}
}
