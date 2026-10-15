using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse.AI;

namespace Verse;

public class ThinkTreeDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThinkTreeDef _003C_003E4__this;

		private HashSet<int> _003CusedKeys_003E5__2;

		private HashSet<ThinkNode> _003Cinstances_003E5__3;

		private IEnumerator<string> _003C_003E7__wrap3;

		private IEnumerator<ThinkNode> _003C_003E7__wrap4;

		private ThinkNode _003Cnode_003E5__6;

		private int _003Ckey_003E5__7;

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
		public _003CConfigErrors_003Ed__4(int _003C_003E1__state)
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
			case 3:
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
			_003CusedKeys_003E5__2 = null;
			_003Cinstances_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E7__wrap4 = null;
			_003Cnode_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThinkTreeDef thinkTreeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap3 = ((Def)thinkTreeDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0218;
				case 3:
					_003C_003E1__state = -4;
					goto IL_0218;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_0218;
					}
					IL_007a:
					if (_003C_003E7__wrap3.MoveNext())
					{
						string current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					_003CusedKeys_003E5__2 = new HashSet<int>();
					_003Cinstances_003E5__3 = new HashSet<ThinkNode>();
					_003C_003E7__wrap4 = thinkTreeDef.thinkRoot.ThisAndChildrenRecursive.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_024c;
					IL_0218:
					if (_003Ckey_003E5__7 != -1)
					{
						_003CusedKeys_003E5__2.Add(_003Ckey_003E5__7);
					}
					_003Cinstances_003E5__3.Add(_003Cnode_003E5__6);
					_003Cnode_003E5__6 = null;
					goto IL_024c;
					IL_024c:
					if (_003C_003E7__wrap4.MoveNext())
					{
						_003Cnode_003E5__6 = _003C_003E7__wrap4.Current;
						_003Ckey_003E5__7 = _003Cnode_003E5__6.UniqueSaveKey;
						if (_003Ckey_003E5__7 == -1)
						{
							_003C_003E2__current = "Thinknode " + _003Cnode_003E5__6.GetType()?.ToString() + " has invalid save key " + _003Ckey_003E5__7;
							_003C_003E1__state = 2;
							return true;
						}
						if (_003Cinstances_003E5__3.Contains(_003Cnode_003E5__6))
						{
							_003C_003E2__current = "There are two same ThinkNode instances in one think tree (their type is " + _003Cnode_003E5__6.GetType()?.ToString() + ")";
							_003C_003E1__state = 3;
							return true;
						}
						if (_003CusedKeys_003E5__2.Contains(_003Ckey_003E5__7))
						{
							_003C_003E2__current = "Two ThinkNodes have the same unique save key " + _003Ckey_003E5__7 + " (one of the nodes is " + _003Cnode_003E5__6.GetType()?.ToString() + ")";
							_003C_003E1__state = 4;
							return true;
						}
						goto IL_0218;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					return false;
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
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
			_003CConfigErrors_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__4(0)
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

	public ThinkNode thinkRoot;

	[NoTranslate]
	public string insertTag;

	public float insertPriority;

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		thinkRoot.ResolveSubnodesAndRecur();
		foreach (ThinkNode item in thinkRoot.ThisAndChildrenRecursive)
		{
			item.ResolveReferences();
		}
		ThinkTreeKeyAssigner.AssignKeys(thinkRoot, GenText.StableStringHash(defName));
		ResolveParentNodes(thinkRoot);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__4))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool TryGetThinkNodeWithSaveKey(int key, out ThinkNode outNode)
	{
		outNode = null;
		if (key == -1)
		{
			return false;
		}
		if (key == thinkRoot.UniqueSaveKey)
		{
			outNode = thinkRoot;
			return true;
		}
		foreach (ThinkNode item in thinkRoot.ChildrenRecursive)
		{
			if (item.UniqueSaveKey == key)
			{
				outNode = item;
				return true;
			}
		}
		return false;
	}

	private void ResolveParentNodes(ThinkNode node)
	{
		for (int i = 0; i < node.subNodes.Count; i++)
		{
			if (node.subNodes[i].parent != null)
			{
				Log.Warning("Think node " + node.subNodes[i]?.ToString() + " from think tree " + defName + " already has a parent node (" + node.subNodes[i].parent?.ToString() + "). This means that it's referenced by more than one think tree (should have been copied instead).");
			}
			else
			{
				node.subNodes[i].parent = node;
				ResolveParentNodes(node.subNodes[i]);
			}
		}
	}
}
