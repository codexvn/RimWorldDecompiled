using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000D6 RID: 214
	public class LRUCache<K, V>
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x0001D31E File Offset: 0x0001B51E
		public LRUCache(int capacity)
		{
			this.capacity = capacity;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001D344 File Offset: 0x0001B544
		public bool TryGetValue(K key, out V result)
		{
			LinkedListNode<ValueTuple<K, V>> linkedListNode;
			if (this.cache.TryGetValue(key, out linkedListNode))
			{
				result = linkedListNode.Value.Item2;
				this.WasUsed(linkedListNode);
				return true;
			}
			result = default(V);
			return false;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001D384 File Offset: 0x0001B584
		public void Add(K key, V value)
		{
			if (this.cache.Count > this.capacity)
			{
				this.RemoveLeastUsed();
			}
			LinkedListNode<ValueTuple<K, V>> linkedListNode = new LinkedListNode<ValueTuple<K, V>>(new ValueTuple<K, V>(key, value));
			this.cache.Add(key, linkedListNode);
			this.leastRecentList.AddLast(linkedListNode);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001D3D0 File Offset: 0x0001B5D0
		public void Clear()
		{
			this.cache.Clear();
			this.leastRecentList.Clear();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001D3E8 File Offset: 0x0001B5E8
		private void WasUsed(LinkedListNode<ValueTuple<K, V>> node)
		{
			this.leastRecentList.Remove(node);
			this.leastRecentList.AddLast(node);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001D404 File Offset: 0x0001B604
		private void RemoveLeastUsed()
		{
			LinkedListNode<ValueTuple<K, V>> first = this.leastRecentList.First;
			if (first != null)
			{
				this.leastRecentList.RemoveFirst();
				this.cache.Remove(first.Value.Item1);
			}
		}

		// Token: 0x040002F8 RID: 760
		private readonly Dictionary<K, LinkedListNode<ValueTuple<K, V>>> cache = new Dictionary<K, LinkedListNode<ValueTuple<K, V>>>();

		// Token: 0x040002F9 RID: 761
		private readonly LinkedList<ValueTuple<K, V>> leastRecentList = new LinkedList<ValueTuple<K, V>>();

		// Token: 0x040002FA RID: 762
		private readonly int capacity;
	}
}
