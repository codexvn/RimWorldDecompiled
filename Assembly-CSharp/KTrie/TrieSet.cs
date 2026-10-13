using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KTrie
{
	// Token: 0x02003EA4 RID: 16036
	public class TrieSet<T> : ICollection<IEnumerable<T>>, IEnumerable<IEnumerable<T>>, IEnumerable
	{
		// Token: 0x06015F7B RID: 89979 RVA: 0x0069049D File Offset: 0x0068E69D
		public TrieSet() : this(EqualityComparer<T>.Default)
		{
		}

		// Token: 0x06015F7C RID: 89980 RVA: 0x006904AC File Offset: 0x0068E6AC
		public TrieSet(IEqualityComparer<T> comparer)
		{
			this._comparer = comparer;
			this._root = new TrieSet<T>.TrieNode(default(T), comparer);
		}

		// Token: 0x17003701 RID: 14081
		// (get) Token: 0x06015F7D RID: 89981 RVA: 0x006904DB File Offset: 0x0068E6DB
		// (set) Token: 0x06015F7E RID: 89982 RVA: 0x006904E3 File Offset: 0x0068E6E3
		public int Count { get; private set; }

		// Token: 0x17003702 RID: 14082
		// (get) Token: 0x06015F7F RID: 89983 RVA: 0x00002501 File Offset: 0x00000701
		bool ICollection<IEnumerable<!0>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06015F80 RID: 89984 RVA: 0x006904EC File Offset: 0x0068E6EC
		public IEnumerator<IEnumerable<T>> GetEnumerator()
		{
			return TrieSet<T>.GetAllNodes(this._root).Select(new Func<TrieSet<T>.TrieNode, IEnumerable<T>>(TrieSet<T>.GetFullKey)).GetEnumerator();
		}

		// Token: 0x06015F81 RID: 89985 RVA: 0x0069050F File Offset: 0x0068E70F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06015F82 RID: 89986 RVA: 0x00690518 File Offset: 0x0068E718
		public void Add(IEnumerable<T> key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TrieSet<T>.TrieNode trieNode = this._root;
			foreach (T key2 in key)
			{
				trieNode = this.AddItem(trieNode, key2);
			}
			if (trieNode.IsTerminal)
			{
				throw new ArgumentException(string.Format("An element with the same key already exists: '{0}'", key), "key");
			}
			trieNode.IsTerminal = true;
			trieNode.Item = key;
			int count = this.Count;
			this.Count = count + 1;
		}

		// Token: 0x06015F83 RID: 89987 RVA: 0x006905B4 File Offset: 0x0068E7B4
		public void AddRange(IEnumerable<IEnumerable<T>> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (IEnumerable<T> key in collection)
			{
				this.Add(key);
			}
		}

		// Token: 0x06015F84 RID: 89988 RVA: 0x0069060C File Offset: 0x0068E80C
		public void Clear()
		{
			this._root.Children.Clear();
			this.Count = 0;
		}

		// Token: 0x06015F85 RID: 89989 RVA: 0x00690628 File Offset: 0x0068E828
		public bool Contains(IEnumerable<T> item)
		{
			TrieSet<T>.TrieNode node = this.GetNode(item);
			return node != null && node.IsTerminal;
		}

		// Token: 0x06015F86 RID: 89990 RVA: 0x00690648 File Offset: 0x0068E848
		public void CopyTo(IEnumerable<T>[] array, int arrayIndex)
		{
			Array.Copy(TrieSet<T>.GetAllNodes(this._root).Select(new Func<TrieSet<T>.TrieNode, IEnumerable<T>>(TrieSet<T>.GetFullKey)).ToArray<IEnumerable<T>>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x06015F87 RID: 89991 RVA: 0x0069067C File Offset: 0x0068E87C
		public bool Remove(IEnumerable<T> key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TrieSet<T>.TrieNode node = this.GetNode(key);
			if (node == null)
			{
				return false;
			}
			if (!node.IsTerminal)
			{
				return false;
			}
			this.RemoveNode(node);
			return true;
		}

		// Token: 0x06015F88 RID: 89992 RVA: 0x006906B8 File Offset: 0x0068E8B8
		public bool TryGetItem(IEnumerable<T> key, out IEnumerable<T> item)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TrieSet<T>.TrieNode node = this.GetNode(key);
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

		// Token: 0x06015F89 RID: 89993 RVA: 0x006906F6 File Offset: 0x0068E8F6
		internal bool TryGetNode(IEnumerable<T> key, out TrieSet<T>.TrieNode node)
		{
			node = this.GetNode(key);
			return node != null && node.IsTerminal;
		}

		// Token: 0x06015F8A RID: 89994 RVA: 0x00690714 File Offset: 0x0068E914
		public IEnumerable<IEnumerable<T>> GetByPrefix(IEnumerable<T> prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			TrieSet<T>.TrieNode root = this._root;
			foreach (T key in prefix)
			{
				if (!root.Children.TryGetValue(key, out root))
				{
					return Enumerable.Empty<IEnumerable<T>>();
				}
			}
			return TrieSet<T>.GetByPrefix(root);
		}

		// Token: 0x06015F8B RID: 89995 RVA: 0x0069078C File Offset: 0x0068E98C
		private static IEnumerable<TrieSet<T>.TrieNode> GetAllNodes(TrieSet<T>.TrieNode node)
		{
			foreach (KeyValuePair<T, TrieSet<T>.TrieNode> child in node.Children)
			{
				if (child.Value.IsTerminal)
				{
					yield return child.Value;
				}
				foreach (TrieSet<T>.TrieNode trieNode in TrieSet<T>.GetAllNodes(child.Value))
				{
					if (trieNode.IsTerminal)
					{
						yield return trieNode;
					}
				}
				IEnumerator<TrieSet<T>.TrieNode> enumerator2 = null;
				child = default(KeyValuePair<T, TrieSet<T>.TrieNode>);
			}
			IEnumerator<KeyValuePair<T, TrieSet<T>.TrieNode>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06015F8C RID: 89996 RVA: 0x0069079C File Offset: 0x0068E99C
		private static IEnumerable<IEnumerable<T>> GetByPrefix(TrieSet<T>.TrieNode node)
		{
			Stack<TrieSet<T>.TrieNode> stack = new Stack<TrieSet<T>.TrieNode>();
			TrieSet<T>.TrieNode current = node;
			while (stack.Count > 0 || current != null)
			{
				if (current != null)
				{
					if (current.IsTerminal)
					{
						yield return TrieSet<T>.GetFullKey(current);
					}
					using (IEnumerator<KeyValuePair<T, TrieSet<T>.TrieNode>> enumerator = current.Children.GetEnumerator())
					{
						TrieSet<T>.TrieNode trieNode;
						if (!enumerator.MoveNext())
						{
							trieNode = null;
						}
						else
						{
							KeyValuePair<T, TrieSet<T>.TrieNode> keyValuePair = enumerator.Current;
							trieNode = keyValuePair.Value;
						}
						current = trieNode;
						while (enumerator.MoveNext())
						{
							Stack<TrieSet<T>.TrieNode> stack2 = stack;
							KeyValuePair<T, TrieSet<T>.TrieNode> keyValuePair = enumerator.Current;
							stack2.Push(keyValuePair.Value);
						}
						continue;
					}
				}
				current = stack.Pop();
			}
			yield break;
		}

		// Token: 0x06015F8D RID: 89997 RVA: 0x006907AC File Offset: 0x0068E9AC
		private static IEnumerable<T> GetFullKey(TrieSet<T>.TrieNode node)
		{
			return node.Item;
		}

		// Token: 0x06015F8E RID: 89998 RVA: 0x006907B4 File Offset: 0x0068E9B4
		private TrieSet<T>.TrieNode GetNode(IEnumerable<T> key)
		{
			TrieSet<T>.TrieNode root = this._root;
			foreach (T key2 in key)
			{
				if (!root.Children.TryGetValue(key2, out root))
				{
					return null;
				}
			}
			return root;
		}

		// Token: 0x06015F8F RID: 89999 RVA: 0x00690814 File Offset: 0x0068EA14
		private void RemoveNode(TrieSet<T>.TrieNode node)
		{
			this.Remove(node);
			int count = this.Count;
			this.Count = count - 1;
		}

		// Token: 0x06015F90 RID: 90000 RVA: 0x00690838 File Offset: 0x0068EA38
		private TrieSet<T>.TrieNode AddItem(TrieSet<T>.TrieNode node, T key)
		{
			TrieSet<T>.TrieNode trieNode;
			if (!node.Children.TryGetValue(key, out trieNode))
			{
				trieNode = new TrieSet<T>.TrieNode(key, this._comparer)
				{
					Parent = node
				};
				node.Children.Add(key, trieNode);
			}
			return trieNode;
		}

		// Token: 0x06015F91 RID: 90001 RVA: 0x00690878 File Offset: 0x0068EA78
		private void Remove(TrieSet<T>.TrieNode node, T key)
		{
			foreach (KeyValuePair<T, TrieSet<T>.TrieNode> item in node.Children)
			{
				if (this._comparer.Equals(key, item.Key))
				{
					node.Children.Remove(item);
					break;
				}
			}
		}

		// Token: 0x06015F92 RID: 90002 RVA: 0x006908E4 File Offset: 0x0068EAE4
		private void Remove(TrieSet<T>.TrieNode node)
		{
			for (;;)
			{
				node.IsTerminal = false;
				if (node.Children.Count != 0 || node.Parent == null)
				{
					break;
				}
				this.Remove(node.Parent, node.Key);
				if (node.Parent.IsTerminal)
				{
					break;
				}
				node = node.Parent;
			}
		}

		// Token: 0x0400F364 RID: 62308
		private readonly IEqualityComparer<T> _comparer;

		// Token: 0x0400F365 RID: 62309
		private readonly TrieSet<T>.TrieNode _root;

		// Token: 0x02003EA5 RID: 16037
		internal sealed class TrieNode
		{
			// Token: 0x06015F93 RID: 90003 RVA: 0x00690936 File Offset: 0x0068EB36
			public TrieNode(T key, IEqualityComparer<T> comparer)
			{
				this.Key = key;
				this.Children = new Dictionary<T, TrieSet<T>.TrieNode>(comparer);
			}

			// Token: 0x17003703 RID: 14083
			// (get) Token: 0x06015F94 RID: 90004 RVA: 0x00690951 File Offset: 0x0068EB51
			// (set) Token: 0x06015F95 RID: 90005 RVA: 0x00690959 File Offset: 0x0068EB59
			public bool IsTerminal { get; set; }

			// Token: 0x17003704 RID: 14084
			// (get) Token: 0x06015F96 RID: 90006 RVA: 0x00690962 File Offset: 0x0068EB62
			public T Key { get; }

			// Token: 0x17003705 RID: 14085
			// (get) Token: 0x06015F97 RID: 90007 RVA: 0x0069096A File Offset: 0x0068EB6A
			// (set) Token: 0x06015F98 RID: 90008 RVA: 0x00690972 File Offset: 0x0068EB72
			public IEnumerable<T> Item { get; set; }

			// Token: 0x17003706 RID: 14086
			// (get) Token: 0x06015F99 RID: 90009 RVA: 0x0069097B File Offset: 0x0068EB7B
			public IDictionary<T, TrieSet<T>.TrieNode> Children { get; }

			// Token: 0x17003707 RID: 14087
			// (get) Token: 0x06015F9A RID: 90010 RVA: 0x00690983 File Offset: 0x0068EB83
			// (set) Token: 0x06015F9B RID: 90011 RVA: 0x0069098B File Offset: 0x0068EB8B
			public TrieSet<T>.TrieNode Parent { get; set; }
		}
	}
}
