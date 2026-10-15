using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class TreeNode_ThingCategory : TreeNode
{
	[CompilerGenerated]
	private sealed class _003Cget_ChildCategoryNodes_003Ed__8 : IEnumerable<TreeNode_ThingCategory>, IEnumerable, IEnumerator<TreeNode_ThingCategory>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TreeNode_ThingCategory _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TreeNode_ThingCategory _003C_003E4__this;

		private List<ThingCategoryDef>.Enumerator _003C_003E7__wrap1;

		TreeNode_ThingCategory IEnumerator<TreeNode_ThingCategory>.Current
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
		public _003Cget_ChildCategoryNodes_003Ed__8(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ThingCategoryDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TreeNode_ThingCategory treeNode_ThingCategory = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = treeNode_ThingCategory.catDef.childCategories.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingCategoryDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.treeNode;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingCategoryDef>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<TreeNode_ThingCategory> IEnumerable<TreeNode_ThingCategory>.GetEnumerator()
		{
			_003Cget_ChildCategoryNodes_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ChildCategoryNodes_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TreeNode_ThingCategory>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_ChildCategoryNodesAndThis_003Ed__6 : IEnumerable<TreeNode_ThingCategory>, IEnumerable, IEnumerator<TreeNode_ThingCategory>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TreeNode_ThingCategory _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TreeNode_ThingCategory _003C_003E4__this;

		private IEnumerator<ThingCategoryDef> _003C_003E7__wrap1;

		TreeNode_ThingCategory IEnumerator<TreeNode_ThingCategory>.Current
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
		public _003Cget_ChildCategoryNodesAndThis_003Ed__6(int _003C_003E1__state)
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
				TreeNode_ThingCategory treeNode_ThingCategory = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = treeNode_ThingCategory.catDef.ThisAndChildCategoryDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingCategoryDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.treeNode;
					_003C_003E1__state = 1;
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
		IEnumerator<TreeNode_ThingCategory> IEnumerable<TreeNode_ThingCategory>.GetEnumerator()
		{
			_003Cget_ChildCategoryNodesAndThis_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ChildCategoryNodesAndThis_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TreeNode_ThingCategory>)this).GetEnumerator();
		}
	}

	public ThingCategoryDef catDef;

	public string Label => catDef.label;

	public string LabelCap => Label.CapitalizeFirst();

	public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
	{
		[IteratorStateMachine(typeof(_003Cget_ChildCategoryNodesAndThis_003Ed__6))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ChildCategoryNodesAndThis_003Ed__6(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
	{
		[IteratorStateMachine(typeof(_003Cget_ChildCategoryNodes_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ChildCategoryNodes_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public TreeNode_ThingCategory(ThingCategoryDef def)
	{
		catDef = def;
	}

	public override string ToString()
	{
		return catDef.defName;
	}
}
