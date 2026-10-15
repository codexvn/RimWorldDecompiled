using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class ThingCategoryDef : Def
{
	[CompilerGenerated]
	private sealed class _003Cget_DescendantSpecialThingFilterDefs_003Ed__19 : IEnumerable<SpecialThingFilterDef>, IEnumerable, IEnumerator<SpecialThingFilterDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private SpecialThingFilterDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingCategoryDef _003C_003E4__this;

		private IEnumerator<ThingCategoryDef> _003C_003E7__wrap1;

		private List<SpecialThingFilterDef>.Enumerator _003C_003E7__wrap2;

		SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
		public _003Cget_DescendantSpecialThingFilterDefs_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<SpecialThingFilterDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingCategoryDef thingCategoryDef = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_008d;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = thingCategoryDef.ThisAndChildCategoryDefs.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ac;
				IL_008d:
				if (_003C_003E7__wrap2.MoveNext())
				{
					SpecialThingFilterDef current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<SpecialThingFilterDef>.Enumerator);
				goto IL_00ac;
				IL_00ac:
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingCategoryDef current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current2.childSpecialFilters.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_008d;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
		{
			_003Cget_DescendantSpecialThingFilterDefs_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_DescendantSpecialThingFilterDefs_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<SpecialThingFilterDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_DescendantThingDefs_003Ed__17 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingCategoryDef _003C_003E4__this;

		private IEnumerator<ThingCategoryDef> _003C_003E7__wrap1;

		private List<ThingDef>.Enumerator _003C_003E7__wrap2;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003Cget_DescendantThingDefs_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<ThingDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingCategoryDef thingCategoryDef = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_008d;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = thingCategoryDef.ThisAndChildCategoryDefs.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ac;
				IL_008d:
				if (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<ThingDef>.Enumerator);
				goto IL_00ac;
				IL_00ac:
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingCategoryDef current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current2.childThingDefs.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_008d;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003Cget_DescendantThingDefs_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_DescendantThingDefs_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Parents_003Ed__13 : IEnumerable<ThingCategoryDef>, IEnumerable, IEnumerator<ThingCategoryDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingCategoryDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingCategoryDef _003C_003E4__this;

		private IEnumerator<ThingCategoryDef> _003C_003E7__wrap1;

		ThingCategoryDef IEnumerator<ThingCategoryDef>.Current
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
		public _003Cget_Parents_003Ed__13(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				ThingCategoryDef thingCategoryDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (thingCategoryDef.parent != null)
					{
						_003C_003E2__current = thingCategoryDef.parent;
						_003C_003E1__state = 1;
						return true;
					}
					break;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = thingCategoryDef.parent.Parents.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_009a;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_009a;
					}
					IL_009a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingCategoryDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
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
		IEnumerator<ThingCategoryDef> IEnumerable<ThingCategoryDef>.GetEnumerator()
		{
			_003Cget_Parents_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Parents_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingCategoryDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_ParentsSpecialThingFilterDefs_003Ed__21 : IEnumerable<SpecialThingFilterDef>, IEnumerable, IEnumerator<SpecialThingFilterDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private SpecialThingFilterDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingCategoryDef _003C_003E4__this;

		private IEnumerator<ThingCategoryDef> _003C_003E7__wrap1;

		private List<SpecialThingFilterDef>.Enumerator _003C_003E7__wrap2;

		SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
		public _003Cget_ParentsSpecialThingFilterDefs_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<SpecialThingFilterDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingCategoryDef thingCategoryDef = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_008d;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = thingCategoryDef.Parents.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ac;
				IL_008d:
				if (_003C_003E7__wrap2.MoveNext())
				{
					SpecialThingFilterDef current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<SpecialThingFilterDef>.Enumerator);
				goto IL_00ac;
				IL_00ac:
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingCategoryDef current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current2.childSpecialFilters.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_008d;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
		{
			_003Cget_ParentsSpecialThingFilterDefs_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ParentsSpecialThingFilterDefs_003Ed__21(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<SpecialThingFilterDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_ThisAndChildCategoryDefs_003Ed__15 : IEnumerable<ThingCategoryDef>, IEnumerable, IEnumerator<ThingCategoryDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingCategoryDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingCategoryDef _003C_003E4__this;

		private List<ThingCategoryDef>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<ThingCategoryDef> _003C_003E7__wrap2;

		ThingCategoryDef IEnumerator<ThingCategoryDef>.Current
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
		public _003Cget_ThisAndChildCategoryDefs_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 2)
			{
				try
				{
					if (num == -4 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<ThingCategoryDef>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingCategoryDef thingCategoryDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = thingCategoryDef;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = thingCategoryDef.childCategories.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00ce;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00b4;
					}
					IL_00ce:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingCategoryDef current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.ThisAndChildCategoryDefs.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00b4;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<ThingCategoryDef>.Enumerator);
					return false;
					IL_00b4:
					if (_003C_003E7__wrap2.MoveNext())
					{
						ThingCategoryDef current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_00ce;
				}
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<ThingCategoryDef> IEnumerable<ThingCategoryDef>.GetEnumerator()
		{
			_003Cget_ThisAndChildCategoryDefs_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ThisAndChildCategoryDefs_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingCategoryDef>)this).GetEnumerator();
		}
	}

	public ThingCategoryDef parent;

	[NoTranslate]
	public string iconPath;

	public bool resourceReadoutRoot;

	[Unsaved(false)]
	public TreeNode_ThingCategory treeNode;

	[Unsaved(false)]
	public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

	[Unsaved(false)]
	public List<ThingDef> childThingDefs = new List<ThingDef>();

	[Unsaved(false)]
	private HashSet<ThingDef> allChildThingDefsCached;

	[Unsaved(false)]
	private List<ThingDef> sortedChildThingDefsCached;

	[Unsaved(false)]
	public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

	[Unsaved(false)]
	public Texture2D icon = BaseContent.BadTex;

	public List<ThingDef> SortedChildThingDefs => sortedChildThingDefsCached;

	public IEnumerable<ThingCategoryDef> Parents
	{
		[IteratorStateMachine(typeof(_003Cget_Parents_003Ed__13))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Parents_003Ed__13(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
	{
		[IteratorStateMachine(typeof(_003Cget_ThisAndChildCategoryDefs_003Ed__15))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ThisAndChildCategoryDefs_003Ed__15(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<ThingDef> DescendantThingDefs
	{
		[IteratorStateMachine(typeof(_003Cget_DescendantThingDefs_003Ed__17))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DescendantThingDefs_003Ed__17(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
	{
		[IteratorStateMachine(typeof(_003Cget_DescendantSpecialThingFilterDefs_003Ed__19))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DescendantSpecialThingFilterDefs_003Ed__19(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
	{
		[IteratorStateMachine(typeof(_003Cget_ParentsSpecialThingFilterDefs_003Ed__21))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ParentsSpecialThingFilterDefs_003Ed__21(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool ContainedInThisOrDescendant(ThingDef thingDef)
	{
		return allChildThingDefsCached.Contains(thingDef);
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		allChildThingDefsCached = new HashSet<ThingDef>();
		foreach (ThingCategoryDef thisAndChildCategoryDef in ThisAndChildCategoryDefs)
		{
			foreach (ThingDef childThingDef in thisAndChildCategoryDef.childThingDefs)
			{
				allChildThingDefsCached.Add(childThingDef);
			}
		}
		sortedChildThingDefsCached = childThingDefs.OrderBy((ThingDef n) => n.label).ToList();
	}

	public override void PostLoad()
	{
		treeNode = new TreeNode_ThingCategory(this);
		if (!iconPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				icon = ContentFinder<Texture2D>.Get(iconPath);
			});
		}
	}

	public static ThingCategoryDef Named(string defName)
	{
		return DefDatabase<ThingCategoryDef>.GetNamed(defName);
	}
}
