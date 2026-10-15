using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse;

public class ThingWithComps : Thing
{
	[CompilerGenerated]
	private sealed class _003CGetAdditionalLeavings_003Ed__37 : IEnumerable<ThingDefCountClass>, IEnumerable, IEnumerator<ThingDefCountClass>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDefCountClass _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingWithComps _003C_003E4__this;

		private DestroyMode mode;

		public DestroyMode _003C_003E3__mode;

		private int _003Ci_003E5__2;

		private IEnumerator<ThingDefCountClass> _003C_003E7__wrap2;

		ThingDefCountClass IEnumerator<ThingDefCountClass>.Current
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
		public _003CGetAdditionalLeavings_003Ed__37(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingWithComps thingWithComps = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_008c;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00b8;
				IL_008c:
				if (_003C_003E7__wrap2.MoveNext())
				{
					ThingDefCountClass current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				_003Ci_003E5__2++;
				goto IL_00b8;
				IL_00b8:
				if (_003Ci_003E5__2 < thingWithComps.AllComps.Count)
				{
					_003C_003E7__wrap2 = thingWithComps.AllComps[_003Ci_003E5__2].GetAdditionalLeavings(thingWithComps.Map, mode).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_008c;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDefCountClass> IEnumerable<ThingDefCountClass>.GetEnumerator()
		{
			_003CGetAdditionalLeavings_003Ed__37 _003CGetAdditionalLeavings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAdditionalLeavings_003Ed__ = this;
			}
			else
			{
				_003CGetAdditionalLeavings_003Ed__ = new _003CGetAdditionalLeavings_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetAdditionalLeavings_003Ed__.mode = _003C_003E3__mode;
			return _003CGetAdditionalLeavings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDefCountClass>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetComps_003Ed__19<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : class
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingWithComps _003C_003E4__this;

		private int _003Ci_003E5__2;

		T IEnumerator<T>.Current
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
		public _003CGetComps_003Ed__19(int _003C_003E1__state)
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
			ThingWithComps thingWithComps = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_006a;
			}
			_003C_003E1__state = -1;
			if (thingWithComps.comps != null)
			{
				_003Ci_003E5__2 = 0;
				goto IL_007a;
			}
			goto IL_008d;
			IL_006a:
			_003Ci_003E5__2++;
			goto IL_007a;
			IL_007a:
			if (_003Ci_003E5__2 < thingWithComps.comps.Count)
			{
				if (thingWithComps.comps[_003Ci_003E5__2] is T val)
				{
					_003C_003E2__current = val;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_006a;
			}
			goto IL_008d;
			IL_008d:
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CGetComps_003Ed__19<T> result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetComps_003Ed__19<T>(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__54 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingWithComps _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		private List<FloatMenuOption>.Enumerator _003C_003E7__wrap2;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__54(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = default(List<FloatMenuOption>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingWithComps thingWithComps = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					tmpFloatMenuOptions.Clear();
					_003C_003E7__wrap1 = ((Thing)thingWithComps).GetFloatMenuOptions(selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (thingWithComps.comps != null)
					{
						for (int i = 0; i < thingWithComps.comps.Count; i++)
						{
							try
							{
								foreach (FloatMenuOption item in thingWithComps.comps[i].CompFloatMenuOptions(selPawn))
								{
									tmpFloatMenuOptions.Add(item);
								}
							}
							catch (Exception ex)
							{
								Log.Error("Exception in CompFloatMenuOptions for " + thingWithComps.comps[i].GetType()?.ToString() + " of " + thingWithComps?.ToString() + " at " + selPawn?.ToString() + ": " + ex);
							}
						}
					}
					_003C_003E7__wrap2 = tmpFloatMenuOptions.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					FloatMenuOption current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<FloatMenuOption>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__54 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__54(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__45 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingWithComps _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__45(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				ThingWithComps thingWithComps = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Thing)thingWithComps).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00f3;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (thingWithComps.comps == null)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_011f;
					IL_00f3:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3++;
					goto IL_011f;
					IL_011f:
					if (_003Ci_003E5__3 >= thingWithComps.comps.Count)
					{
						break;
					}
					_003C_003E7__wrap1 = thingWithComps.comps[_003Ci_003E5__3].CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00f3;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__45 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__45(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetMultiSelectFloatMenuOptions_003Ed__55 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingWithComps _003C_003E4__this;

		private IEnumerable<Pawn> selPawns;

		public IEnumerable<Pawn> _003C_003E3__selPawns;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetMultiSelectFloatMenuOptions_003Ed__55(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				ThingWithComps thingWithComps = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Thing)thingWithComps).GetMultiSelectFloatMenuOptions(selPawns).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_0102;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (thingWithComps.comps == null)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_012e;
					IL_0102:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3++;
					goto IL_012e;
					IL_012e:
					if (_003Ci_003E5__3 >= thingWithComps.comps.Count)
					{
						break;
					}
					_003C_003E7__wrap1 = thingWithComps.comps[_003Ci_003E5__3].CompMultiSelectFloatMenuOptions(selPawns).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0102;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetMultiSelectFloatMenuOptions_003Ed__55 _003CGetMultiSelectFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetMultiSelectFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetMultiSelectFloatMenuOptions_003Ed__ = new _003CGetMultiSelectFloatMenuOptions_003Ed__55(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetMultiSelectFloatMenuOptions_003Ed__.selPawns = _003C_003E3__selPawns;
			return _003CGetMultiSelectFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__74 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingWithComps _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003CSpecialDisplayStats_003Ed__74(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				ThingWithComps thingWithComps = _003C_003E4__this;
				IEnumerable<StatDrawEntry> enumerable;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Thing)thingWithComps).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00fe;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (thingWithComps.comps == null)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_012a;
					IL_0118:
					_003Ci_003E5__3++;
					goto IL_012a;
					IL_00fe:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_0118;
					IL_012a:
					if (_003Ci_003E5__3 >= thingWithComps.comps.Count)
					{
						break;
					}
					enumerable = thingWithComps.comps[_003Ci_003E5__3].SpecialDisplayStats();
					if (enumerable != null)
					{
						_003C_003E7__wrap1 = enumerable.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00fe;
					}
					goto IL_0118;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__74 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__74(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private List<ThingComp> comps;

	private Dictionary<Type, ThingComp[]> compsByType;

	private int compsByTypeCount;

	private ThingComp compsByTypeLast;

	[Unsaved(false)]
	public CompStyleable compStyleable;

	[Unsaved(false)]
	public CompForbiddable compForbiddable;

	[Unsaved(false)]
	public CompQuality compQuality;

	private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();

	private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();

	public List<ThingComp> AllComps
	{
		get
		{
			if (comps == null)
			{
				return EmptyCompsList;
			}
			return comps;
		}
	}

	public override Color DrawColor
	{
		get
		{
			CompColorable comp = GetComp<CompColorable>();
			if (comp != null && comp.Active)
			{
				return comp.Color;
			}
			foreach (ThingComp allComp in AllComps)
			{
				Color? color = allComp.ForceColor();
				if (color.HasValue)
				{
					return color.Value;
				}
			}
			return base.DrawColor;
		}
		set
		{
			this.SetColor(value);
		}
	}

	public override string LabelNoCount
	{
		get
		{
			string text = base.LabelNoCount;
			if (comps != null)
			{
				int i = 0;
				for (int count = comps.Count; i < count; i++)
				{
					text = comps[i].TransformLabel(text);
				}
			}
			return text;
		}
	}

	public override string DescriptionFlavor
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DescriptionFlavor);
			if (comps != null)
			{
				for (int i = 0; i < comps.Count; i++)
				{
					string descriptionPart = comps[i].GetDescriptionPart();
					if (!descriptionPart.NullOrEmpty())
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(descriptionPart);
					}
				}
			}
			return stringBuilder.ToString();
		}
	}

	public override void PostMake()
	{
		base.PostMake();
		InitializeComps();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostPostMake();
			}
		}
	}

	public T GetComp<T>() where T : ThingComp
	{
		if (comps == null)
		{
			return null;
		}
		int count = comps.Count;
		if (count < 3)
		{
			if (comps[0] is T result)
			{
				return result;
			}
			if (count == 2 && comps[1] is T result2)
			{
				return result2;
			}
			return null;
		}
		if (compsByType != null)
		{
			if (count != compsByTypeCount || comps[count - 1] != compsByTypeLast)
			{
				RebuildCompsByType();
			}
			if (compsByType.TryGetValue(typeof(T), out var value))
			{
				return (T)value[0];
			}
			if (typeof(T).IsSealedWithCache())
			{
				return null;
			}
		}
		for (int i = 0; i < count; i++)
		{
			if (comps[i] is T result3)
			{
				return result3;
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetComps_003Ed__19<>))]
	public IEnumerable<T> GetComps<T>() where T : class
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetComps_003Ed__19<T>(-2)
		{
			_003C_003E4__this = this
		};
	}

	public ThingComp GetCompByDefType(CompProperties def)
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i].props.compClass == def.compClass)
				{
					return comps[i];
				}
			}
		}
		return null;
	}

	public void InitializeComps()
	{
		if (!def.comps.Any())
		{
			return;
		}
		comps = new List<ThingComp>();
		for (int i = 0; i < def.comps.Count; i++)
		{
			ThingComp thingComp = null;
			try
			{
				thingComp = (ThingComp)Activator.CreateInstance(def.comps[i].compClass);
				thingComp.parent = this;
				comps.Add(thingComp);
				thingComp.Initialize(def.comps[i]);
			}
			catch (Exception ex)
			{
				Log.Error("Could not instantiate or initialize a ThingComp: " + ex);
				comps.Remove(thingComp);
			}
		}
		RebuildCompsByType();
		compStyleable = GetComp<CompStyleable>();
		compForbiddable = GetComp<CompForbiddable>();
		compQuality = GetComp<CompQuality>();
	}

	private void RebuildCompsByType()
	{
		int count = comps.Count;
		Dictionary<Type, List<ThingComp>> dictionary = new Dictionary<Type, List<ThingComp>>();
		for (int i = 0; i < count; i++)
		{
			if (!dictionary.TryGetValue(comps[i].GetType(), out var value))
			{
				value = (dictionary[comps[i].GetType()] = new List<ThingComp>());
			}
			value.Add(comps[i]);
		}
		Dictionary<Type, ThingComp[]> dictionary2 = new Dictionary<Type, ThingComp[]>(dictionary.Count);
		foreach (KeyValuePair<Type, List<ThingComp>> item in dictionary)
		{
			dictionary2[item.Key] = item.Value.ToArray();
		}
		compsByType = dictionary2;
		compsByTypeCount = count;
		compsByTypeLast = ((count > 0) ? comps[count - 1] : null);
	}

	public override string GetCustomLabelNoCount(bool includeHp = true)
	{
		string text = base.GetCustomLabelNoCount(includeHp);
		if (comps != null)
		{
			int i = 0;
			for (int count = comps.Count; i < count; i++)
			{
				text = comps[i].TransformLabel(text);
			}
		}
		return text;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			InitializeComps();
		}
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostExposeData();
			}
		}
	}

	public void BroadcastCompSignal(string signal)
	{
		ReceiveCompSignal(signal);
		if (comps != null)
		{
			int i = 0;
			for (int count = comps.Count; i < count; i++)
			{
				comps[i].ReceiveCompSignal(signal);
			}
		}
	}

	protected virtual void ReceiveCompSignal(string signal)
	{
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostSpawnSetup(respawningAfterLoad);
			}
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		Map map = base.Map;
		base.DeSpawn(mode);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostDeSpawn(map, mode);
			}
		}
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		Map mapHeld = base.MapHeld;
		base.Destroy(mode);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostDestroy(mode, mapHeld);
			}
		}
	}

	public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
	{
		Map prevMap = ((!(this is Pawn pawn)) ? base.MapHeld : (pawn.prevMap ?? base.MapHeld));
		base.Kill(dinfo, exactCulprit);
		if (comps == null)
		{
			return;
		}
		foreach (ThingComp comp in comps)
		{
			comp.Notify_Killed(prevMap, dinfo);
		}
	}

	protected override void Tick()
	{
		if (comps != null)
		{
			int i = 0;
			for (int count = comps.Count; i < count; i++)
			{
				comps[i].CompTick();
			}
		}
	}

	protected override void TickInterval(int delta)
	{
		if (comps != null)
		{
			int i = 0;
			for (int count = comps.Count; i < count; i++)
			{
				comps[i].CompTickInterval(delta);
			}
		}
	}

	public override void TickRare()
	{
		if (comps != null)
		{
			int i = 0;
			for (int count = comps.Count; i < count; i++)
			{
				comps[i].CompTickRare();
			}
		}
	}

	public override void TickLong()
	{
		if (comps != null)
		{
			int i = 0;
			for (int count = comps.Count; i < count; i++)
			{
				comps[i].CompTickLong();
			}
		}
	}

	public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
	{
		base.PreApplyDamage(ref dinfo, out absorbed);
		if (absorbed || comps == null)
		{
			return;
		}
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].PostPreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				break;
			}
		}
	}

	public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		base.PostApplyDamage(dinfo, totalDamageDealt);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostPostApplyDamage(dinfo, totalDamageDealt);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetAdditionalLeavings_003Ed__37))]
	public virtual IEnumerable<ThingDefCountClass> GetAdditionalLeavings(DestroyMode mode)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAdditionalLeavings_003Ed__37(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__mode = mode
		};
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		if (!CompsPreventDrawing())
		{
			base.DrawAt(drawLoc, flip);
		}
		Comps_DrawAt(drawLoc, flip);
		Comps_PostDraw();
	}

	protected void Comps_DrawAt(Vector3 drawLoc, bool flip)
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].DrawAt(drawLoc, flip);
			}
		}
	}

	protected void Comps_PostDraw()
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostDraw();
			}
		}
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostDrawExtraSelectionOverlays();
			}
		}
	}

	public override void Print(SectionLayer layer)
	{
		if (!CompsPreventDrawing())
		{
			base.Print(layer);
		}
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostPrintOnto(layer);
			}
		}
	}

	private bool CompsPreventDrawing()
	{
		if (comps == null)
		{
			return false;
		}
		for (int i = 0; i < comps.Count; i++)
		{
			if (comps[i].DontDrawParent())
			{
				return true;
			}
		}
		return false;
	}

	public virtual void PrintForPowerGrid(SectionLayer layer)
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].CompPrintForPowerGrid(layer);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__45))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__45(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool TryAbsorbStack(Thing other, bool respectStackLimit)
	{
		if (!CanStackWith(other))
		{
			return false;
		}
		int count = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PreAbsorbStack(other, count);
			}
		}
		return base.TryAbsorbStack(other, respectStackLimit);
	}

	public override Thing SplitOff(int count)
	{
		Thing thing = base.SplitOff(count);
		if (thing != null && comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostSplitOff(thing);
			}
		}
		return thing;
	}

	public override bool CanStackWith(Thing other)
	{
		if (!base.CanStackWith(other))
		{
			return false;
		}
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				if (!comps[i].AllowStackWith(other))
				{
					return false;
				}
			}
		}
		return true;
	}

	public override TipSignal GetTooltip()
	{
		string text = base.LabelNoParenthesisCap.AsTipTitle() + GenLabel.LabelExtras(this, includeHp: true, includeQuality: true);
		text = text + "\n\n" + DescriptionDetailed;
		if (def.useHitPoints)
		{
			text = text + "\n" + HitPoints + " / " + base.MaxHitPoints;
		}
		text += "\n";
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				string text2 = comps[i].CompTipStringExtra();
				if (!text2.NullOrEmpty())
				{
					text = text + "\n" + text2;
				}
			}
		}
		return new TipSignal(text, thingIDNumber * 251235);
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		string text = InspectStringPartsFromComps();
		if (!text.NullOrEmpty())
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(text);
		}
		return stringBuilder.ToString();
	}

	protected string InspectStringPartsFromComps()
	{
		if (comps == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < comps.Count; i++)
		{
			string text = comps[i].CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
				{
					Log.ErrorOnce(comps[i].GetType()?.ToString() + " CompInspectStringExtra ended with whitespace: " + text, 25612);
					text = text.TrimEndNewlines();
				}
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(text);
			}
		}
		return stringBuilder.ToString();
	}

	public override void DrawGUIOverlay()
	{
		base.DrawGUIOverlay();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].DrawGUIOverlay();
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__54))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__54(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CGetMultiSelectFloatMenuOptions_003Ed__55))]
	public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetMultiSelectFloatMenuOptions_003Ed__55(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawns = selPawns
		};
	}

	public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PrePreTraded(action, playerNegotiator, trader);
			}
		}
		base.PreTraded(action, playerNegotiator, trader);
	}

	public override void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
	{
		base.PostGeneratedForTrader(trader, forTile, forFaction);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostPostGeneratedForTrader(trader, forTile, forFaction);
			}
		}
	}

	protected override void PrePostIngested(Pawn ingester)
	{
		base.PrePostIngested(ingester);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PrePostIngested(ingester);
			}
		}
	}

	protected override void PostIngested(Pawn ingester)
	{
		base.PostIngested(ingester);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostIngested(ingester);
			}
		}
	}

	public override void Notify_DefsHotReloaded()
	{
		base.Notify_DefsHotReloaded();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_DefsHotReloaded();
			}
		}
	}

	public override void PostMapInit()
	{
		base.PostMapInit();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostMapInit();
			}
		}
	}

	public override void Notify_SignalReceived(Signal signal)
	{
		base.Notify_SignalReceived(signal);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_SignalReceived(signal);
			}
		}
	}

	public override void Notify_RecipeProduced(Pawn pawn)
	{
		base.Notify_RecipeProduced(pawn);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_RecipeProduced(pawn);
			}
		}
	}

	public override void Notify_LordDestroyed()
	{
		base.Notify_LordDestroyed();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_LordDestroyed();
			}
		}
	}

	public override void Notify_Equipped(Pawn pawn)
	{
		base.Notify_Equipped(pawn);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_Equipped(pawn);
			}
		}
	}

	public override void Notify_Unequipped(Pawn pawn)
	{
		base.Notify_Unequipped(pawn);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_Unequipped(pawn);
			}
		}
	}

	public override void Notify_UsedVerb(Pawn pawn, Verb verb)
	{
		base.Notify_UsedVerb(pawn, verb);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_UsedVerb(pawn, verb);
			}
		}
	}

	public override void Notify_UsedWeapon(Pawn pawn)
	{
		base.Notify_UsedWeapon(pawn);
		if (ModsConfig.IdeologyActive && pawn.Ideo != null)
		{
			switch (pawn.Ideo.GetDispositionForWeapon(def))
			{
			case IdeoWeaponDisposition.Despised:
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)));
				break;
			case IdeoWeaponDisposition.Noble:
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)));
				break;
			}
		}
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_UsedWeapon(pawn);
			}
		}
	}

	public void Notify_KilledPawn(Pawn pawn)
	{
		if (ModsConfig.IdeologyActive && pawn.Ideo != null && def.IsWeapon)
		{
			switch (pawn.Ideo.GetDispositionForWeapon(def))
			{
			case IdeoWeaponDisposition.Despised:
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)));
				break;
			case IdeoWeaponDisposition.Noble:
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)));
				break;
			}
		}
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_KilledPawn(pawn);
			}
		}
	}

	public override void Notify_AbandonedAtTile(PlanetTile tile)
	{
		base.Notify_AbandonedAtTile(tile);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_AbandonedAtTile(tile);
			}
		}
	}

	public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
	{
		base.Notify_KilledLeavingsLeft(leavings);
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_KilledLeavingsLeft(leavings);
			}
		}
	}

	public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
	{
		List<ThingComp> allComps = AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			if (allComps[i] is IThingStudied thingStudied)
			{
				thingStudied.OnStudied(studier, amount, category);
			}
		}
	}

	public override void Notify_ColorChanged()
	{
		base.Notify_ColorChanged();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_ColorChanged();
			}
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__74))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__74(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_Explosion(Explosion explosion)
	{
		base.Notify_Explosion(explosion);
		CompWakeUpDormant comp = GetComp<CompWakeUpDormant>();
		if (comp != null && (explosion.Position - base.Position).LengthHorizontal <= explosion.radius)
		{
			comp.Activate(explosion.instigator);
		}
	}

	public override void Notify_MyMapRemoved()
	{
		base.Notify_MyMapRemoved();
		if (def.notifyMapRemoved && comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].Notify_MapRemoved();
			}
		}
	}

	public override void PreSwapMap()
	{
		base.PreSwapMap();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PreSwapMap();
			}
		}
	}

	public override void PostSwapMap()
	{
		base.PostSwapMap();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].PostSwapMap();
			}
		}
	}
}
