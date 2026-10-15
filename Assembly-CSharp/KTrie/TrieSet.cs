using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KTrie;

public class TrieSet<T> : ICollection<IEnumerable<T>>, IEnumerable<IEnumerable<T>>, IEnumerable
{
	internal sealed class TrieNode
	{
		public bool IsTerminal { get; set; }

		public T Key { get; }

		public IEnumerable<T> Item { get; set; }

		public IDictionary<T, TrieNode> Children { get; }

		public TrieNode Parent { get; set; }

		public TrieNode(T key, IEqualityComparer<T> comparer)
		{
			Key = key;
			Children = new Dictionary<T, TrieNode>(comparer);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetAllNodes_003Ed__21 : IEnumerable<TrieNode>, IEnumerable, IEnumerator<TrieNode>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TrieNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TrieNode node;

		public TrieNode _003C_003E3__node;

		private IEnumerator<KeyValuePair<T, TrieNode>> _003C_003E7__wrap1;

		private KeyValuePair<T, TrieNode> _003Cchild_003E5__3;

		private IEnumerator<TrieNode> _003C_003E7__wrap3;

		TrieNode IEnumerator<TrieNode>.Current
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
		public _003CGetAllNodes_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
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
			_003C_003E7__wrap1 = null;
			_003Cchild_003E5__3 = default(KeyValuePair<T, TrieNode>);
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
					_003C_003E7__wrap1 = node.Children.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_010d;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0094;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00e7;
					}
					IL_010d:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cchild_003E5__3 = _003C_003E7__wrap1.Current;
						if (_003Cchild_003E5__3.Value.IsTerminal)
						{
							_003C_003E2__current = _003Cchild_003E5__3.Value;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_0094;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
					IL_0094:
					_003C_003E7__wrap3 = TrieSet<T>.GetAllNodes(_003Cchild_003E5__3.Value).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00e7;
					IL_00e7:
					while (_003C_003E7__wrap3.MoveNext())
					{
						TrieNode current = _003C_003E7__wrap3.Current;
						if (current.IsTerminal)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					_003Cchild_003E5__3 = default(KeyValuePair<T, TrieNode>);
					goto IL_010d;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<TrieNode> IEnumerable<TrieNode>.GetEnumerator()
		{
			_003CGetAllNodes_003Ed__21 _003CGetAllNodes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllNodes_003Ed__ = this;
			}
			else
			{
				_003CGetAllNodes_003Ed__ = new _003CGetAllNodes_003Ed__21(0);
			}
			_003CGetAllNodes_003Ed__.node = _003C_003E3__node;
			return _003CGetAllNodes_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TrieNode>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetByPrefix_003Ed__22 : IEnumerable<IEnumerable<T>>, IEnumerable, IEnumerator<IEnumerable<T>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IEnumerable<T> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TrieNode node;

		public TrieNode _003C_003E3__node;

		private Stack<TrieNode> _003Cstack_003E5__2;

		private TrieNode _003Ccurrent_003E5__3;

		IEnumerable<T> IEnumerator<IEnumerable<T>>.Current
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
		public _003CGetByPrefix_003Ed__22(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cstack_003E5__2 = null;
			_003Ccurrent_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_006c;
			}
			_003C_003E1__state = -1;
			_003Cstack_003E5__2 = new Stack<TrieNode>();
			_003Ccurrent_003E5__3 = node;
			goto IL_00dc;
			IL_006c:
			using (IEnumerator<KeyValuePair<T, TrieNode>> enumerator = _003Ccurrent_003E5__3.Children.GetEnumerator())
			{
				_003Ccurrent_003E5__3 = (enumerator.MoveNext() ? enumerator.Current.Value : null);
				while (enumerator.MoveNext())
				{
					_003Cstack_003E5__2.Push(enumerator.Current.Value);
				}
			}
			goto IL_00dc;
			IL_00dc:
			while (true)
			{
				if (_003Cstack_003E5__2.Count > 0 || _003Ccurrent_003E5__3 != null)
				{
					if (_003Ccurrent_003E5__3 != null)
					{
						break;
					}
					_003Ccurrent_003E5__3 = _003Cstack_003E5__2.Pop();
					continue;
				}
				return false;
			}
			if (_003Ccurrent_003E5__3.IsTerminal)
			{
				_003C_003E2__current = TrieSet<T>.GetFullKey(_003Ccurrent_003E5__3);
				_003C_003E1__state = 1;
				return true;
			}
			goto IL_006c;
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
		IEnumerator<IEnumerable<T>> IEnumerable<IEnumerable<T>>.GetEnumerator()
		{
			_003CGetByPrefix_003Ed__22 _003CGetByPrefix_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetByPrefix_003Ed__ = this;
			}
			else
			{
				_003CGetByPrefix_003Ed__ = new _003CGetByPrefix_003Ed__22(0);
			}
			_003CGetByPrefix_003Ed__.node = _003C_003E3__node;
			return _003CGetByPrefix_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IEnumerable<T>>)this).GetEnumerator();
		}
	}

	private readonly IEqualityComparer<T> _comparer;

	private readonly TrieNode _root;

	public int Count { get; private set; }

	bool ICollection<IEnumerable<T>>.IsReadOnly => false;

	public TrieSet()
		: this((IEqualityComparer<T>)EqualityComparer<T>.Default)
	{
	}

	public TrieSet(IEqualityComparer<T> comparer)
	{
		_comparer = comparer;
		_root = new TrieNode(default(T), comparer);
	}

	public IEnumerator<IEnumerable<T>> GetEnumerator()
	{
		return GetAllNodes(_root).Select(GetFullKey).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(IEnumerable<T> key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		TrieNode trieNode = _root;
		foreach (T item in key)
		{
			trieNode = AddItem(trieNode, item);
		}
		if (trieNode.IsTerminal)
		{
			throw new ArgumentException($"An element with the same key already exists: '{key}'", "key");
		}
		trieNode.IsTerminal = true;
		trieNode.Item = key;
		Count++;
	}

	public void AddRange(IEnumerable<IEnumerable<T>> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		foreach (IEnumerable<T> item in collection)
		{
			Add(item);
		}
	}

	public void Clear()
	{
		_root.Children.Clear();
		Count = 0;
	}

	public bool Contains(IEnumerable<T> item)
	{
		return GetNode(item)?.IsTerminal ?? false;
	}

	public void CopyTo(IEnumerable<T>[] array, int arrayIndex)
	{
		Array.Copy(GetAllNodes(_root).Select(GetFullKey).ToArray(), 0, array, arrayIndex, Count);
	}

	public bool Remove(IEnumerable<T> key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		TrieNode node = GetNode(key);
		if (node == null)
		{
			return false;
		}
		if (!node.IsTerminal)
		{
			return false;
		}
		RemoveNode(node);
		return true;
	}

	public bool TryGetItem(IEnumerable<T> key, out IEnumerable<T> item)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		TrieNode node = GetNode(key);
		item = null;
		if (node == null)
		{
			return false;
		}
		if (!node.IsTerminal)
		{
			return false;
		}
		item = node.Item;
		return true;
	}

	internal bool TryGetNode(IEnumerable<T> key, out TrieNode node)
	{
		node = GetNode(key);
		if (node == null)
		{
			return false;
		}
		if (!node.IsTerminal)
		{
			return false;
		}
		return true;
	}

	public IEnumerable<IEnumerable<T>> GetByPrefix(IEnumerable<T> prefix)
	{
		if (prefix == null)
		{
			throw new ArgumentNullException("prefix");
		}
		TrieNode value = _root;
		foreach (T item in prefix)
		{
			if (!value.Children.TryGetValue(item, out value))
			{
				return Enumerable.Empty<IEnumerable<T>>();
			}
		}
		return GetByPrefix(value);
	}

	[IteratorStateMachine(typeof(TrieSet<>._003CGetAllNodes_003Ed__21))]
	private static IEnumerable<TrieNode> GetAllNodes(TrieNode node)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllNodes_003Ed__21(-2)
		{
			_003C_003E3__node = node
		};
	}

	[IteratorStateMachine(typeof(TrieSet<>._003CGetByPrefix_003Ed__22))]
	private static IEnumerable<IEnumerable<T>> GetByPrefix(TrieNode node)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetByPrefix_003Ed__22(-2)
		{
			_003C_003E3__node = node
		};
	}

	private static IEnumerable<T> GetFullKey(TrieNode node)
	{
		return node.Item;
	}

	private TrieNode GetNode(IEnumerable<T> key)
	{
		TrieNode value = _root;
		foreach (T item in key)
		{
			if (!value.Children.TryGetValue(item, out value))
			{
				return null;
			}
		}
		return value;
	}

	private void RemoveNode(TrieNode node)
	{
		Remove(node);
		Count--;
	}

	private TrieNode AddItem(TrieNode node, T key)
	{
		if (!node.Children.TryGetValue(key, out var value))
		{
			value = new TrieNode(key, _comparer)
			{
				Parent = node
			};
			node.Children.Add(key, value);
		}
		return value;
	}

	private void Remove(TrieNode node, T key)
	{
		foreach (KeyValuePair<T, TrieNode> child in node.Children)
		{
			if (_comparer.Equals(key, child.Key))
			{
				node.Children.Remove(child);
				break;
			}
		}
	}

	private void Remove(TrieNode node)
	{
		while (true)
		{
			node.IsTerminal = false;
			if (node.Children.Count == 0 && node.Parent != null)
			{
				Remove(node.Parent, node.Key);
				if (!node.Parent.IsTerminal)
				{
					node = node.Parent;
					continue;
				}
				break;
			}
			break;
		}
	}
}
