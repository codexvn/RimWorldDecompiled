using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public static class StatsReportUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass22_0
	{
		public StatRequest statRequest;

		public Func<StatDef, bool> _003C_003E9__0;

		internal bool _003CStatsToDraw_003Eb__0(StatDef st)
		{
			return st.Worker.ShouldShowFor(statRequest);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass25_0
	{
		public StatRequest statRequest;

		public Func<StatDef, bool> _003C_003E9__0;

		internal bool _003CStatsToDraw_003Eb__0(StatDef st)
		{
			return st.Worker.ShouldShowFor(statRequest);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass26_0
	{
		public Thing thing;

		public Func<StatDef, bool> _003C_003E9__0;

		internal bool _003CStatsToDraw_003Eb__0(StatDef st)
		{
			return st.Worker.ShouldShowFor(StatRequest.For(thing));
		}
	}

	[CompilerGenerated]
	private sealed class _003CStatsToDraw_003Ed__22 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Def def;

		public Def _003C_003E3__def;

		private ThingDef stuff;

		public ThingDef _003C_003E3__stuff;

		private BuildableDef _003CeDef_003E5__2;

		private IEnumerator<StatDef> _003C_003E7__wrap2;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap3;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CStatsToDraw_003Ed__22(int _003C_003E1__state)
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
			_003CeDef_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = DescriptionEntry(def);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003CeDef_003E5__2 = def as BuildableDef;
					if (_003CeDef_003E5__2 != null)
					{
						_003C_003Ec__DisplayClass22_0 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass22_0
						{
							statRequest = StatRequest.For(_003CeDef_003E5__2, stuff)
						};
						_003C_003E7__wrap2 = DefDatabase<StatDef>.AllDefs.Where((StatDef st) => st.Worker.ShouldShowFor(CS_0024_003C_003E8__locals1.statRequest)).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0133;
					}
					goto IL_014d;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0133;
				case 3:
					{
						_003C_003E1__state = -4;
						goto IL_01a7;
					}
					IL_01a7:
					if (_003C_003E7__wrap3.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					break;
					IL_014d:
					if (!(def is ThingDef { IsStuff: not false } thingDef))
					{
						break;
					}
					_003C_003E7__wrap3 = StuffStats(thingDef).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01a7;
					IL_0133:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDef current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = new StatDrawEntry(current2.category, current2, _003CeDef_003E5__2.GetStatValueAbstract(current2, stuff), StatRequest.For(_003CeDef_003E5__2, stuff), ToStringNumberSense.Undefined, null);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					goto IL_014d;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CStatsToDraw_003Ed__22 _003CStatsToDraw_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStatsToDraw_003Ed__ = this;
			}
			else
			{
				_003CStatsToDraw_003Ed__ = new _003CStatsToDraw_003Ed__22(0);
			}
			_003CStatsToDraw_003Ed__.def = _003C_003E3__def;
			_003CStatsToDraw_003Ed__.stuff = _003C_003E3__stuff;
			return _003CStatsToDraw_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStatsToDraw_003Ed__23 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RoyalTitleDef title;

		public RoyalTitleDef _003C_003E3__title;

		private Faction faction;

		public Faction _003C_003E3__faction;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CStatsToDraw_003Ed__23(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = DescriptionEntry(title, faction);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CStatsToDraw_003Ed__23 _003CStatsToDraw_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStatsToDraw_003Ed__ = this;
			}
			else
			{
				_003CStatsToDraw_003Ed__ = new _003CStatsToDraw_003Ed__23(0);
			}
			_003CStatsToDraw_003Ed__.title = _003C_003E3__title;
			_003CStatsToDraw_003Ed__.faction = _003C_003E3__faction;
			return _003CStatsToDraw_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStatsToDraw_003Ed__24 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Faction faction;

		public Faction _003C_003E3__faction;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CStatsToDraw_003Ed__24(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = DescriptionEntry(faction);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CStatsToDraw_003Ed__24 _003CStatsToDraw_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStatsToDraw_003Ed__ = this;
			}
			else
			{
				_003CStatsToDraw_003Ed__ = new _003CStatsToDraw_003Ed__24(0);
			}
			_003CStatsToDraw_003Ed__.faction = _003C_003E3__faction;
			return _003CStatsToDraw_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStatsToDraw_003Ed__25 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private AbilityDef def;

		public AbilityDef _003C_003E3__def;

		private _003C_003Ec__DisplayClass25_0 _003C_003E8__1;

		private IEnumerator<StatDef> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CStatsToDraw_003Ed__25(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass25_0();
					_003C_003E2__current = DescriptionEntry(def);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E8__1.statRequest = StatRequest.For(def);
					_003C_003E7__wrap1 = DefDatabase<StatDef>.AllDefs.Where((StatDef st) => st.Worker.ShouldShowFor(_003C_003E8__1.statRequest)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					StatDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = new StatDrawEntry(current.category, current, def.GetStatValueAbstract(current), StatRequest.For(def), ToStringNumberSense.Undefined, null);
					_003C_003E1__state = 2;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CStatsToDraw_003Ed__25 _003CStatsToDraw_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStatsToDraw_003Ed__ = this;
			}
			else
			{
				_003CStatsToDraw_003Ed__ = new _003CStatsToDraw_003Ed__25(0);
			}
			_003CStatsToDraw_003Ed__.def = _003C_003E3__def;
			return _003CStatsToDraw_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStatsToDraw_003Ed__26 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing thing;

		public Thing _003C_003E3__thing;

		private _003C_003Ec__DisplayClass26_0 _003C_003E8__1;

		private IEnumerator<StatDef> _003C_003E7__wrap1;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap2;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CStatsToDraw_003Ed__26(int _003C_003E1__state)
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
			case 3:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 7:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass26_0();
					_003C_003E8__1.thing = thing;
					_003C_003E2__current = DescriptionEntry(_003C_003E8__1.thing);
					_003C_003E1__state = 1;
					return true;
				case 1:
				{
					_003C_003E1__state = -1;
					StatDrawEntry statDrawEntry = QualityEntry(_003C_003E8__1.thing);
					if (statDrawEntry != null)
					{
						_003C_003E2__current = statDrawEntry;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b2;
				}
				case 2:
					_003C_003E1__state = -1;
					goto IL_00b2;
				case 3:
					_003C_003E1__state = -3;
					goto IL_01c6;
				case 4:
					_003C_003E1__state = -3;
					goto IL_01c6;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0292;
				case 6:
					_003C_003E1__state = -4;
					goto IL_02e2;
				case 7:
					{
						_003C_003E1__state = -5;
						goto IL_0365;
					}
					IL_0292:
					_003C_003E7__wrap2 = _003C_003E8__1.thing.SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_02e2;
					IL_00b2:
					_003C_003E7__wrap1 = DefDatabase<StatDef>.AllDefs.Where((StatDef st) => st.Worker.ShouldShowFor(StatRequest.For(_003C_003E8__1.thing))).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01c6;
					IL_02e2:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 6;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					if (!_003C_003E8__1.thing.def.IsStuff)
					{
						break;
					}
					_003C_003E7__wrap2 = StuffStats(_003C_003E8__1.thing.def).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0365;
					IL_01c6:
					while (_003C_003E7__wrap1.MoveNext())
					{
						StatDef current2 = _003C_003E7__wrap1.Current;
						if (!current2.Worker.IsDisabledFor(_003C_003E8__1.thing))
						{
							float statValue = _003C_003E8__1.thing.GetStatValue(current2);
							if (current2.showOnDefaultValue || statValue != current2.defaultBaseValue)
							{
								_003C_003E2__current = new StatDrawEntry(current2.category, current2, statValue, StatRequest.For(_003C_003E8__1.thing), ToStringNumberSense.Undefined, null);
								_003C_003E1__state = 3;
								return true;
							}
							continue;
						}
						_003C_003E2__current = new StatDrawEntry(current2.category, current2);
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (_003C_003E8__1.thing.def.useHitPoints)
					{
						StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "HitPointsBasic".Translate().CapitalizeFirst(), _003C_003E8__1.thing.HitPoints + " / " + _003C_003E8__1.thing.MaxHitPoints, "Stat_HitPoints_Desc".Translate(), 99998);
						_003C_003E2__current = statDrawEntry2;
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0292;
					IL_0365:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current3 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = null;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CStatsToDraw_003Ed__26 _003CStatsToDraw_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStatsToDraw_003Ed__ = this;
			}
			else
			{
				_003CStatsToDraw_003Ed__ = new _003CStatsToDraw_003Ed__26(0);
			}
			_003CStatsToDraw_003Ed__.thing = _003C_003E3__thing;
			return _003CStatsToDraw_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStatsToDraw_003Ed__27 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private WorldObject worldObject;

		public WorldObject _003C_003E3__worldObject;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CStatsToDraw_003Ed__27(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = DescriptionEntry(worldObject);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = worldObject.SpecialDisplayStats.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					StatDrawEntry current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 2;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CStatsToDraw_003Ed__27 _003CStatsToDraw_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStatsToDraw_003Ed__ = this;
			}
			else
			{
				_003CStatsToDraw_003Ed__ = new _003CStatsToDraw_003Ed__27(0);
			}
			_003CStatsToDraw_003Ed__.worldObject = _003C_003E3__worldObject;
			return _003CStatsToDraw_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStuffStats_003Ed__28 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef stuffDef;

		public ThingDef _003C_003E3__stuffDef;

		private int _003Ci_003E5__2;

		private List<StatModifierQuality>.Enumerator _003C_003E7__wrap2;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CStuffStats_003Ed__28(int _003C_003E1__state)
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
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap2 = default(List<StatModifierQuality>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!stuffDef.stuffProps.statFactors.NullOrEmpty())
					{
						_003Ci_003E5__2 = 0;
						goto IL_00da;
					}
					goto IL_00fa;
				case 1:
					_003C_003E1__state = -1;
					_003Ci_003E5__2++;
					goto IL_00da;
				case 2:
					_003C_003E1__state = -3;
					goto IL_017a;
				case 3:
					_003C_003E1__state = -1;
					_003Ci_003E5__2++;
					goto IL_0244;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_02e1;
					}
					IL_0199:
					if (!stuffDef.stuffProps.statOffsets.NullOrEmpty())
					{
						_003Ci_003E5__2 = 0;
						goto IL_0244;
					}
					goto IL_0264;
					IL_00da:
					if (_003Ci_003E5__2 < stuffDef.stuffProps.statFactors.Count)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, stuffDef.stuffProps.statFactors[_003Ci_003E5__2].stat, stuffDef.stuffProps.statFactors[_003Ci_003E5__2].value, StatRequest.ForEmpty(), ToStringNumberSense.Factor, null);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00fa;
					IL_00fa:
					if (!stuffDef.stuffProps.statFactorsQuality.NullOrEmpty())
					{
						_003C_003E7__wrap2 = stuffDef.stuffProps.statFactorsQuality.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_017a;
					}
					goto IL_0199;
					IL_0244:
					if (_003Ci_003E5__2 < stuffDef.stuffProps.statOffsets.Count)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, stuffDef.stuffProps.statOffsets[_003Ci_003E5__2].stat, stuffDef.stuffProps.statOffsets[_003Ci_003E5__2].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0264;
					IL_017a:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatModifierQuality current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, current.stat, current.ToStringAsFactorRange);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = default(List<StatModifierQuality>.Enumerator);
					goto IL_0199;
					IL_02e1:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatModifierQuality current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, current2.stat, current2.ToStringAsOffsetRange);
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<StatModifierQuality>.Enumerator);
					break;
					IL_0264:
					if (stuffDef.stuffProps.statOffsetsQuality.NullOrEmpty())
					{
						break;
					}
					_003C_003E7__wrap2 = stuffDef.stuffProps.statOffsetsQuality.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_02e1;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CStuffStats_003Ed__28 _003CStuffStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStuffStats_003Ed__ = this;
			}
			else
			{
				_003CStuffStats_003Ed__ = new _003CStuffStats_003Ed__28(0);
			}
			_003CStuffStats_003Ed__.stuffDef = _003C_003E3__stuffDef;
			return _003CStuffStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private static StatDrawEntry selectedEntry;

	private static StatDrawEntry mousedOverEntry;

	private static Vector2 scrollPosition;

	private static ScrollPositioner scrollPositioner = new ScrollPositioner();

	private static Vector2 scrollPositionRightPanel;

	private static QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

	private static float listHeight;

	private static float rightPanelHeight;

	private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();

	private static List<string> cachedEntryValues = new List<string>();

	public static int SelectedStatIndex
	{
		get
		{
			if (cachedDrawEntries.NullOrEmpty() || selectedEntry == null)
			{
				return -1;
			}
			return cachedDrawEntries.IndexOf(selectedEntry);
		}
	}

	public static QuickSearchWidget QuickSearchWidget => quickSearchWidget;

	public static void Reset()
	{
		scrollPosition = default(Vector2);
		scrollPositionRightPanel = default(Vector2);
		selectedEntry = null;
		scrollPositioner.Arm(armed: false);
		mousedOverEntry = null;
		cachedDrawEntries.Clear();
		cachedEntryValues.Clear();
		quickSearchWidget.Reset();
		PermitsCardUtility.selectedPermit = null;
		PermitsCardUtility.selectedFaction = ((ModLister.RoyaltyInstalled && Current.ProgramState == ProgramState.Playing) ? Faction.OfEmpire : null);
	}

	public static void DrawStatsReport(Rect rect, Def def, ThingDef stuff)
	{
		if (cachedDrawEntries.NullOrEmpty())
		{
			StatRequest req = ((def is BuildableDef def2) ? StatRequest.For(def2, stuff) : StatRequest.ForEmpty());
			cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
			cachedDrawEntries.AddRange(from r in StatsToDraw(def, stuff)
				where r.ShouldDisplay()
				select r);
			FinalizeCachedDrawEntries(cachedDrawEntries);
		}
		DrawStatsWorker(rect, null, null);
	}

	public static void DrawStatsReport(Rect rect, AbilityDef def)
	{
		if (cachedDrawEntries.NullOrEmpty())
		{
			StatRequest req = StatRequest.ForEmpty();
			cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
			cachedDrawEntries.AddRange(from r in StatsToDraw(def)
				where r.ShouldDisplay()
				select r);
			FinalizeCachedDrawEntries(cachedDrawEntries);
		}
		DrawStatsWorker(rect, null, null);
	}

	public static void DrawStatsReport(Rect rect, Thing thing)
	{
		if (cachedDrawEntries.NullOrEmpty())
		{
			cachedDrawEntries.AddRange(thing.def.SpecialDisplayStats(StatRequest.For(thing)));
			cachedDrawEntries.AddRange(StatsToDraw(thing));
			cachedDrawEntries.RemoveAll((StatDrawEntry de) => (de.stat != null && !de.stat.showNonAbstract) || !de.ShouldDisplay(thing));
			FinalizeCachedDrawEntries(cachedDrawEntries);
		}
		DrawStatsWorker(rect, thing, null);
	}

	public static void DrawStatsReport(Rect rect, Hediff hediff)
	{
		if (cachedDrawEntries.NullOrEmpty())
		{
			cachedDrawEntries.AddRange(hediff.SpecialDisplayStats(StatRequest.ForEmpty()));
			cachedDrawEntries.AddRange(from r in StatsToDraw(hediff.def, null)
				where r.ShouldDisplay()
				select r);
			FinalizeCachedDrawEntries(cachedDrawEntries);
		}
		DrawStatsWorker(rect, null, null);
	}

	public static void DrawStatsReport(Rect rect, WorldObject worldObject)
	{
		if (cachedDrawEntries.NullOrEmpty())
		{
			cachedDrawEntries.AddRange(worldObject.def.SpecialDisplayStats(StatRequest.ForEmpty()));
			cachedDrawEntries.AddRange(from r in StatsToDraw(worldObject)
				where r.ShouldDisplay()
				select r);
			cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
			FinalizeCachedDrawEntries(cachedDrawEntries);
		}
		DrawStatsWorker(rect, null, worldObject);
	}

	public static void DrawStatsReport(Rect rect, RoyalTitleDef title, Faction faction, Pawn pawn = null)
	{
		if (cachedDrawEntries.NullOrEmpty())
		{
			cachedDrawEntries.AddRange(title.SpecialDisplayStats(StatRequest.For(title, faction, pawn)));
			cachedDrawEntries.AddRange(from r in StatsToDraw(title, faction)
				where r.ShouldDisplay(pawn)
				select r);
			FinalizeCachedDrawEntries(cachedDrawEntries);
		}
		DrawStatsWorker(rect, null, null);
	}

	public static void DrawStatsReport(Rect rect, Faction faction)
	{
		if (cachedDrawEntries.NullOrEmpty())
		{
			StatRequest req = StatRequest.ForEmpty();
			cachedDrawEntries.AddRange(faction.def.SpecialDisplayStats(req));
			cachedDrawEntries.AddRange(from r in StatsToDraw(faction)
				where r.ShouldDisplay()
				select r);
			FinalizeCachedDrawEntries(cachedDrawEntries);
		}
		DrawStatsWorker(rect, null, null);
	}

	[IteratorStateMachine(typeof(_003CStatsToDraw_003Ed__22))]
	private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStatsToDraw_003Ed__22(-2)
		{
			_003C_003E3__def = def,
			_003C_003E3__stuff = stuff
		};
	}

	[IteratorStateMachine(typeof(_003CStatsToDraw_003Ed__23))]
	private static IEnumerable<StatDrawEntry> StatsToDraw(RoyalTitleDef title, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStatsToDraw_003Ed__23(-2)
		{
			_003C_003E3__title = title,
			_003C_003E3__faction = faction
		};
	}

	[IteratorStateMachine(typeof(_003CStatsToDraw_003Ed__24))]
	private static IEnumerable<StatDrawEntry> StatsToDraw(Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStatsToDraw_003Ed__24(-2)
		{
			_003C_003E3__faction = faction
		};
	}

	[IteratorStateMachine(typeof(_003CStatsToDraw_003Ed__25))]
	private static IEnumerable<StatDrawEntry> StatsToDraw(AbilityDef def)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStatsToDraw_003Ed__25(-2)
		{
			_003C_003E3__def = def
		};
	}

	[IteratorStateMachine(typeof(_003CStatsToDraw_003Ed__26))]
	private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStatsToDraw_003Ed__26(-2)
		{
			_003C_003E3__thing = thing
		};
	}

	[IteratorStateMachine(typeof(_003CStatsToDraw_003Ed__27))]
	private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStatsToDraw_003Ed__27(-2)
		{
			_003C_003E3__worldObject = worldObject
		};
	}

	[IteratorStateMachine(typeof(_003CStuffStats_003Ed__28))]
	private static IEnumerable<StatDrawEntry> StuffStats(ThingDef stuffDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStuffStats_003Ed__28(-2)
		{
			_003C_003E3__stuffDef = stuffDef
		};
	}

	private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
	{
		cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList();
		quickSearchWidget.noResultsMatched = !cachedDrawEntries.Any();
		foreach (StatDrawEntry cachedDrawEntry in cachedDrawEntries)
		{
			cachedEntryValues.Add(cachedDrawEntry.ValueString);
		}
		if (selectedEntry != null)
		{
			selectedEntry = cachedDrawEntries.FirstOrDefault((StatDrawEntry e) => e.Same(selectedEntry));
		}
		if (!quickSearchWidget.filter.Active)
		{
			return;
		}
		foreach (StatDrawEntry cachedDrawEntry2 in cachedDrawEntries)
		{
			if (Matches(cachedDrawEntry2))
			{
				selectedEntry = cachedDrawEntry2;
				scrollPositioner.Arm();
				break;
			}
		}
	}

	private static bool Matches(StatDrawEntry sd)
	{
		return quickSearchWidget.filter.Matches(sd.LabelCap);
	}

	private static StatDrawEntry DescriptionEntry(Def def)
	{
		return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", def.description, 99999, null, Dialog_InfoCard.DefsToHyperlinks(def.descriptionHyperlinks));
	}

	private static StatDrawEntry DescriptionEntry(Faction faction)
	{
		return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", faction.GetReportText, 99999, null, Dialog_InfoCard.DefsToHyperlinks(faction.def.descriptionHyperlinks));
	}

	private static StatDrawEntry DescriptionEntry(RoyalTitleDef title, Faction faction)
	{
		return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", title.GetReportText(faction), 99999, null, Dialog_InfoCard.TitleDefsToHyperlinks(title.GetHyperlinks(faction)));
	}

	private static StatDrawEntry DescriptionEntry(Thing thing)
	{
		return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", thing.DescriptionFlavor, 99999, null, Dialog_InfoCard.DefsToHyperlinks(thing.DescriptionHyperlinks), forceUnfinalizedMode: false, overridesHideStats: true);
	}

	private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
	{
		return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", worldObject.GetDescription(), 99999);
	}

	private static StatDrawEntry QualityEntry(Thing t)
	{
		if (!t.TryGetQuality(out var qc))
		{
			return null;
		}
		return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Quality".Translate(), qc.GetLabel().CapitalizeFirst(), "QualityDescription".Translate(), 99999);
	}

	public static void SelectEntry(int index)
	{
		if (index >= 0 && index <= cachedDrawEntries.Count)
		{
			SelectEntry(cachedDrawEntries[index]);
		}
	}

	public static void SelectEntry(StatDef stat, bool playSound = false)
	{
		foreach (StatDrawEntry cachedDrawEntry in cachedDrawEntries)
		{
			if (cachedDrawEntry.stat == stat)
			{
				SelectEntry(cachedDrawEntry, playSound);
				return;
			}
		}
		Messages.Message("MessageCannotSelectInvisibleStat".Translate(stat), MessageTypeDefOf.RejectInput, historical: false);
	}

	private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
	{
		selectedEntry = rec;
		if (playSound)
		{
			SoundDefOf.Tick_High.PlayOneShotOnCamera();
		}
	}

	private static void DrawStatsWorker(Rect rect, Thing optionalThing, WorldObject optionalWorldObject)
	{
		Rect rect2 = new Rect(rect);
		rect2.width *= 0.5f;
		Rect rect3 = new Rect(rect);
		rect3.x = rect2.xMax;
		rect3.width = rect.xMax - rect3.x;
		scrollPositioner.ClearInterestRects();
		Text.Font = GameFont.Small;
		Rect viewRect = new Rect(0f, 0f, rect2.width - 16f, listHeight);
		Widgets.BeginScrollView(rect2, ref scrollPosition, viewRect);
		float curY = 0f;
		string text = null;
		mousedOverEntry = null;
		for (int i = 0; i < cachedDrawEntries.Count; i++)
		{
			StatDrawEntry ent = cachedDrawEntries[i];
			if (ent.category.LabelCap != text)
			{
				Widgets.ListSeparator(ref curY, viewRect.width, ent.category.LabelCap);
				text = ent.category.LabelCap;
			}
			bool highlightLabel = false;
			bool lowlightLabel = false;
			bool flag = selectedEntry == ent;
			bool flag2 = false;
			GUI.color = Color.white;
			if (quickSearchWidget.filter.Active)
			{
				if (Matches(ent))
				{
					highlightLabel = true;
					flag2 = true;
				}
				else
				{
					lowlightLabel = true;
				}
			}
			Rect rect4 = new Rect(8f, curY, viewRect.width - 8f, 30f);
			curY = (rect4.yMax = curY + ent.Draw(rect4.x, rect4.y, rect4.width, flag, highlightLabel, lowlightLabel, delegate
			{
				SelectEntry(ent);
			}, delegate
			{
				mousedOverEntry = ent;
			}, scrollPosition, rect2, cachedEntryValues[i]));
			if (flag || flag2)
			{
				scrollPositioner.RegisterInterestRect(rect4);
			}
		}
		listHeight = curY + 100f;
		Widgets.EndScrollView();
		scrollPositioner.ScrollVertically(ref scrollPosition, rect2.size);
		Rect outRect = rect3.ContractedBy(10f);
		StatDrawEntry statDrawEntry = selectedEntry ?? mousedOverEntry ?? cachedDrawEntries.FirstOrDefault();
		if (statDrawEntry == null)
		{
			return;
		}
		Rect rect5 = new Rect(0f, 0f, outRect.width - 16f, rightPanelHeight);
		StatRequest statRequest = (statDrawEntry.hasOptionalReq ? statDrawEntry.optionalReq : ((optionalThing == null) ? StatRequest.ForEmpty() : StatRequest.For(optionalThing)));
		string explanationText = statDrawEntry.GetExplanationText(statRequest);
		float num2 = 0f;
		Widgets.BeginScrollView(outRect, ref scrollPositionRightPanel, rect5);
		Rect rect6 = rect5;
		rect6.width -= 4f;
		Widgets.Label(rect6, explanationText);
		float num3 = Text.CalcHeight(explanationText, rect6.width) + 10f;
		num2 += num3;
		IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = statDrawEntry.GetHyperlinks(statRequest);
		if (hyperlinks != null)
		{
			Rect rect7 = new Rect(rect6.x, rect6.y + num3, rect6.width, rect6.height - num3);
			Color color = GUI.color;
			GUI.color = Widgets.NormalOptionColor;
			foreach (Dialog_InfoCard.Hyperlink item in hyperlinks)
			{
				float num4 = Text.CalcHeight(item.Label, rect7.width);
				Widgets.HyperlinkWithIcon(new Rect(rect7.x, rect7.y, rect7.width, num4), text: item.HasGeneOwnerThing ? item.Label : "ViewHyperlink".Translate(item.Label).ToString(), hyperlink: item, iconMargin: 2f, textOffsetX: 6f, color: null);
				rect7.y += num4;
				rect7.height -= num4;
				num2 += num4;
			}
			GUI.color = color;
		}
		rightPanelHeight = num2;
		Widgets.EndScrollView();
	}

	public static void Notify_QuickSearchChanged()
	{
		cachedDrawEntries.Clear();
		cachedEntryValues.Clear();
	}
}
