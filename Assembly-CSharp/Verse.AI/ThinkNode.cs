using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse.AI;

public abstract class ThinkNode
{
	[CompilerGenerated]
	private sealed class _003Cget_ChildrenRecursive_003Ed__14 : IEnumerable<ThinkNode>, IEnumerable, IEnumerator<ThinkNode>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThinkNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThinkNode _003C_003E4__this;

		private Stack<ThinkNode> _003Cstack_003E5__2;

		private ThinkNode _003Celem_003E5__3;

		ThinkNode IEnumerator<ThinkNode>.Current
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
		public _003Cget_ChildrenRecursive_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cstack_003E5__2 = null;
			_003Celem_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ThinkNode thinkNode = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (thinkNode.subNodes.Count == 0)
				{
					return false;
				}
				_003Cstack_003E5__2 = new Stack<ThinkNode>();
				for (int num3 = thinkNode.subNodes.Count - 1; num3 >= 0; num3--)
				{
					_003Cstack_003E5__2.Push(thinkNode.subNodes[num3]);
				}
				break;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				for (int num2 = _003Celem_003E5__3.subNodes.Count - 1; num2 >= 0; num2--)
				{
					_003Cstack_003E5__2.Push(_003Celem_003E5__3.subNodes[num2]);
				}
				_003Celem_003E5__3 = null;
				break;
			}
			}
			if (_003Cstack_003E5__2.Count != 0)
			{
				_003Celem_003E5__3 = _003Cstack_003E5__2.Pop();
				_003C_003E2__current = _003Celem_003E5__3;
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<ThinkNode> IEnumerable<ThinkNode>.GetEnumerator()
		{
			_003Cget_ChildrenRecursive_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ChildrenRecursive_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThinkNode>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_ThisAndChildrenRecursive_003Ed__12 : IEnumerable<ThinkNode>, IEnumerable, IEnumerator<ThinkNode>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThinkNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThinkNode _003C_003E4__this;

		private IEnumerator<ThinkNode> _003C_003E7__wrap1;

		ThinkNode IEnumerator<ThinkNode>.Current
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
		public _003Cget_ThisAndChildrenRecursive_003Ed__12(int _003C_003E1__state)
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
				ThinkNode thinkNode = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = thinkNode;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = thinkNode.ChildrenRecursive.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThinkNode current = _003C_003E7__wrap1.Current;
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
		IEnumerator<ThinkNode> IEnumerable<ThinkNode>.GetEnumerator()
		{
			_003Cget_ThisAndChildrenRecursive_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ThisAndChildrenRecursive_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThinkNode>)this).GetEnumerator();
		}
	}

	public List<ThinkNode> subNodes = new List<ThinkNode>();

	public bool leaveJoinableLordIfIssuesJob;

	protected float priority = -1f;

	[Unsaved(false)]
	private int uniqueSaveKeyInt = -2;

	[NoTranslate]
	public string tag;

	[Unsaved(false)]
	public ThinkNode parent;

	[Unsaved(false)]
	private bool hasPriority;

	public const int InvalidSaveKey = -1;

	protected const int UnresolvedSaveKey = -2;

	public int UniqueSaveKey => uniqueSaveKeyInt;

	public IEnumerable<ThinkNode> ThisAndChildrenRecursive
	{
		[IteratorStateMachine(typeof(_003Cget_ThisAndChildrenRecursive_003Ed__12))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ThisAndChildrenRecursive_003Ed__12(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<ThinkNode> ChildrenRecursive
	{
		[IteratorStateMachine(typeof(_003Cget_ChildrenRecursive_003Ed__14))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ChildrenRecursive_003Ed__14(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	protected ThinkNode()
	{
		hasPriority = priority >= 0f || GetType().IsMethodOverriden("GetPriority");
	}

	public T FirstNodeOfType<T>(bool includeThis = false) where T : ThinkNode
	{
		if (includeThis && this is T result)
		{
			return result;
		}
		foreach (ThinkNode subNode in subNodes)
		{
			T val = subNode.FirstNodeOfType<T>(includeThis: true);
			if (val != null)
			{
				return val;
			}
		}
		return null;
	}

	public bool TryGetPriority(Pawn pawn, out float value)
	{
		if (!hasPriority)
		{
			value = 0f;
			return false;
		}
		value = GetPriority(pawn);
		return true;
	}

	public virtual float GetPriority(Pawn pawn)
	{
		if (priority < 0f)
		{
			Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, GetHashCode());
			return 0f;
		}
		return priority;
	}

	public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

	protected virtual void ResolveSubnodes()
	{
	}

	public void ResolveSubnodesAndRecur()
	{
		if (uniqueSaveKeyInt == -2)
		{
			ResolveSubnodes();
			for (int i = 0; i < subNodes.Count; i++)
			{
				subNodes[i].ResolveSubnodesAndRecur();
			}
		}
	}

	public virtual void ResolveReferences()
	{
	}

	public virtual ThinkNode DeepCopy(bool resolve = true)
	{
		ThinkNode thinkNode = (ThinkNode)Activator.CreateInstance(GetType());
		for (int i = 0; i < subNodes.Count; i++)
		{
			thinkNode.subNodes.Add(subNodes[i].DeepCopy(resolve));
		}
		thinkNode.priority = priority;
		thinkNode.leaveJoinableLordIfIssuesJob = leaveJoinableLordIfIssuesJob;
		thinkNode.uniqueSaveKeyInt = uniqueSaveKeyInt;
		thinkNode.tag = tag;
		if (resolve)
		{
			thinkNode.ResolveSubnodesAndRecur();
		}
		ThinkTreeKeyAssigner.AssignSingleKey(thinkNode, 0);
		return thinkNode;
	}

	internal void SetUniqueSaveKey(int key)
	{
		uniqueSaveKeyInt = key;
	}

	public override int GetHashCode()
	{
		return Gen.HashCombineInt(uniqueSaveKeyInt, 1157295731);
	}
}
