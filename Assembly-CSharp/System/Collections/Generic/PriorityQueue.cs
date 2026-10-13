using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02003EA8 RID: 16040
	[DebuggerDisplay("Count = {Count}")]
	public class PriorityQueue<TElement, TPriority>
	{
		// Token: 0x06015FAE RID: 90030 RVA: 0x00690D83 File Offset: 0x0068EF83
		public PriorityQueue()
		{
			this._nodes = Array.Empty<ValueTuple<TElement, TPriority>>();
			this._comparer = PriorityQueue<TElement, TPriority>.InitializeComparer(null);
		}

		// Token: 0x06015FAF RID: 90031 RVA: 0x00690DA2 File Offset: 0x0068EFA2
		public PriorityQueue(int initialCapacity) : this(initialCapacity, null)
		{
		}

		// Token: 0x06015FB0 RID: 90032 RVA: 0x00690DAC File Offset: 0x0068EFAC
		public PriorityQueue(IComparer<TPriority> comparer)
		{
			this._nodes = Array.Empty<ValueTuple<TElement, TPriority>>();
			this._comparer = PriorityQueue<TElement, TPriority>.InitializeComparer(comparer);
		}

		// Token: 0x06015FB1 RID: 90033 RVA: 0x00690DCB File Offset: 0x0068EFCB
		public PriorityQueue(int initialCapacity, IComparer<TPriority> comparer)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity");
			}
			this._nodes = new ValueTuple<TElement, TPriority>[initialCapacity];
			this._comparer = PriorityQueue<TElement, TPriority>.InitializeComparer(comparer);
		}

		// Token: 0x06015FB2 RID: 90034 RVA: 0x00690DFA File Offset: 0x0068EFFA
		public PriorityQueue([TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})] IEnumerable<ValueTuple<TElement, TPriority>> items) : this(items, null)
		{
		}

		// Token: 0x06015FB3 RID: 90035 RVA: 0x00690E04 File Offset: 0x0068F004
		public PriorityQueue([TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})] IEnumerable<ValueTuple<TElement, TPriority>> items, IComparer<TPriority> comparer)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this._nodes = items.ToArray<ValueTuple<TElement, TPriority>>();
			this._size = this._nodes.Length;
			this._comparer = PriorityQueue<TElement, TPriority>.InitializeComparer(comparer);
			if (this._size > 1)
			{
				this.Heapify();
			}
		}

		// Token: 0x1700370C RID: 14092
		// (get) Token: 0x06015FB4 RID: 90036 RVA: 0x00690E5A File Offset: 0x0068F05A
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x1700370D RID: 14093
		// (get) Token: 0x06015FB5 RID: 90037 RVA: 0x00690E62 File Offset: 0x0068F062
		public IComparer<TPriority> Comparer
		{
			get
			{
				return this._comparer ?? Comparer<TPriority>.Default;
			}
		}

		// Token: 0x06015FB6 RID: 90038 RVA: 0x00690E74 File Offset: 0x0068F074
		public void Enqueue(TElement element, TPriority priority)
		{
			int size = this._size;
			this._size = size + 1;
			int num = size;
			this._version++;
			if (this._nodes.Length == num)
			{
				this.Grow(num + 1);
			}
			if (this._comparer == null)
			{
				this.MoveUpDefaultComparer(new ValueTuple<TElement, TPriority>(element, priority), num);
				return;
			}
			this.MoveUpCustomComparer(new ValueTuple<TElement, TPriority>(element, priority), num);
		}

		// Token: 0x06015FB7 RID: 90039 RVA: 0x00690EDA File Offset: 0x0068F0DA
		public TElement Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException();
			}
			return this._nodes[0].Item1;
		}

		// Token: 0x06015FB8 RID: 90040 RVA: 0x00690EFB File Offset: 0x0068F0FB
		public TElement Dequeue()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException();
			}
			TElement item = this._nodes[0].Item1;
			this.RemoveRootNode();
			return item;
		}

		// Token: 0x06015FB9 RID: 90041 RVA: 0x00690F24 File Offset: 0x0068F124
		public bool TryDequeue(out TElement element, out TPriority priority)
		{
			if (this._size != 0)
			{
				ValueTuple<TElement, TPriority> valueTuple = this._nodes[0];
				element = valueTuple.Item1;
				priority = valueTuple.Item2;
				this.RemoveRootNode();
				return true;
			}
			element = default(TElement);
			priority = default(TPriority);
			return false;
		}

		// Token: 0x06015FBA RID: 90042 RVA: 0x00690F78 File Offset: 0x0068F178
		public bool TryPeek(out TElement element, out TPriority priority)
		{
			if (this._size != 0)
			{
				ValueTuple<TElement, TPriority> valueTuple = this._nodes[0];
				element = valueTuple.Item1;
				priority = valueTuple.Item2;
				return true;
			}
			element = default(TElement);
			priority = default(TPriority);
			return false;
		}

		// Token: 0x06015FBB RID: 90043 RVA: 0x00690FC4 File Offset: 0x0068F1C4
		public TElement EnqueueDequeue(TElement element, TPriority priority)
		{
			if (this._size != 0)
			{
				ValueTuple<TElement, TPriority> valueTuple = this._nodes[0];
				if (this._comparer == null)
				{
					if (Comparer<TPriority>.Default.Compare(priority, valueTuple.Item2) > 0)
					{
						this.MoveDownDefaultComparer(new ValueTuple<TElement, TPriority>(element, priority), 0);
						this._version++;
						return valueTuple.Item1;
					}
				}
				else if (this._comparer.Compare(priority, valueTuple.Item2) > 0)
				{
					this.MoveDownCustomComparer(new ValueTuple<TElement, TPriority>(element, priority), 0);
					this._version++;
					return valueTuple.Item1;
				}
			}
			return element;
		}

		// Token: 0x06015FBC RID: 90044 RVA: 0x00691064 File Offset: 0x0068F264
		public void EnqueueRange([TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})] IEnumerable<ValueTuple<TElement, TPriority>> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			int num = 0;
			ICollection<ValueTuple<TElement, TPriority>> collection = items as ICollection<ValueTuple<TElement, TPriority>>;
			if (collection != null && (num = collection.Count) > this._nodes.Length - this._size)
			{
				this.Grow(this._size + num);
			}
			if (this._size == 0)
			{
				if (collection != null)
				{
					collection.CopyTo(this._nodes, 0);
					this._size = num;
				}
				else
				{
					int num2 = 0;
					ValueTuple<TElement, TPriority>[] nodes = this._nodes;
					foreach (ValueTuple<TElement, TPriority> valueTuple in items)
					{
						TElement item = valueTuple.Item1;
						TPriority item2 = valueTuple.Item2;
						if (nodes.Length == num2)
						{
							this.Grow(num2 + 1);
							nodes = this._nodes;
						}
						nodes[num2++] = new ValueTuple<TElement, TPriority>(item, item2);
					}
					this._size = num2;
				}
				this._version++;
				if (this._size > 1)
				{
					this.Heapify();
					return;
				}
			}
			else
			{
				foreach (ValueTuple<TElement, TPriority> valueTuple2 in items)
				{
					TElement item3 = valueTuple2.Item1;
					TPriority item4 = valueTuple2.Item2;
					this.Enqueue(item3, item4);
				}
			}
		}

		// Token: 0x06015FBD RID: 90045 RVA: 0x006911C4 File Offset: 0x0068F3C4
		public void EnqueueRange(IEnumerable<TElement> elements, TPriority priority)
		{
			if (elements == null)
			{
				throw new ArgumentNullException("elements");
			}
			ICollection<ValueTuple<TElement, TPriority>> collection = elements as ICollection<ValueTuple<TElement, TPriority>>;
			int count;
			if (collection != null && (count = collection.Count) > this._nodes.Length - this._size)
			{
				this.Grow(this._size + count);
			}
			if (this._size == 0)
			{
				int num = 0;
				ValueTuple<TElement, TPriority>[] nodes = this._nodes;
				foreach (TElement item in elements)
				{
					if (nodes.Length == num)
					{
						this.Grow(num + 1);
						nodes = this._nodes;
					}
					nodes[num++] = new ValueTuple<TElement, TPriority>(item, priority);
				}
				this._size = num;
				this._version++;
				if (num > 1)
				{
					this.Heapify();
					return;
				}
			}
			else
			{
				foreach (TElement element in elements)
				{
					this.Enqueue(element, priority);
				}
			}
		}

		// Token: 0x06015FBE RID: 90046 RVA: 0x006912E4 File Offset: 0x0068F4E4
		public void Clear()
		{
			Array.Clear(this._nodes, 0, this._size);
			this._size = 0;
			this._version++;
		}

		// Token: 0x06015FBF RID: 90047 RVA: 0x0069130D File Offset: 0x0068F50D
		public int EnsureCapacity(int capacity)
		{
			if (this._nodes.Length < capacity)
			{
				this.Grow(capacity);
				this._version++;
			}
			return this._nodes.Length;
		}

		// Token: 0x06015FC0 RID: 90048 RVA: 0x00691338 File Offset: 0x0068F538
		public void TrimExcess()
		{
			int num = (int)((double)this._nodes.Length * 0.9);
			if (this._size < num)
			{
				Array.Resize<ValueTuple<TElement, TPriority>>(ref this._nodes, this._size);
				this._version++;
			}
		}

		// Token: 0x06015FC1 RID: 90049 RVA: 0x00691384 File Offset: 0x0068F584
		private void Grow(int minCapacity)
		{
			int num = 2 * this._nodes.Length;
			if (num > 2147483647)
			{
				num = int.MaxValue;
			}
			num = Math.Max(num, this._nodes.Length + 4);
			if (num < minCapacity)
			{
				num = minCapacity;
			}
			Array.Resize<ValueTuple<TElement, TPriority>>(ref this._nodes, num);
		}

		// Token: 0x06015FC2 RID: 90050 RVA: 0x006913D0 File Offset: 0x0068F5D0
		private void RemoveRootNode()
		{
			int num = this._size - 1;
			this._size = num;
			int num2 = num;
			this._version++;
			if (num2 > 0)
			{
				ValueTuple<TElement, TPriority> node = this._nodes[num2];
				if (this._comparer == null)
				{
					this.MoveDownDefaultComparer(node, 0);
				}
				else
				{
					this.MoveDownCustomComparer(node, 0);
				}
			}
			this._nodes[num2] = default(ValueTuple<TElement, TPriority>);
		}

		// Token: 0x06015FC3 RID: 90051 RVA: 0x0069143A File Offset: 0x0068F63A
		private static int GetParentIndex(int index)
		{
			return index - 1 >> 2;
		}

		// Token: 0x06015FC4 RID: 90052 RVA: 0x00691441 File Offset: 0x0068F641
		private static int GetFirstChildIndex(int index)
		{
			return (index << 2) + 1;
		}

		// Token: 0x06015FC5 RID: 90053 RVA: 0x00691448 File Offset: 0x0068F648
		private void Heapify()
		{
			ValueTuple<TElement, TPriority>[] nodes = this._nodes;
			int parentIndex = PriorityQueue<TElement, TPriority>.GetParentIndex(this._size - 1);
			if (this._comparer == null)
			{
				for (int i = parentIndex; i >= 0; i--)
				{
					this.MoveDownDefaultComparer(nodes[i], i);
				}
				return;
			}
			for (int j = parentIndex; j >= 0; j--)
			{
				this.MoveDownCustomComparer(nodes[j], j);
			}
		}

		// Token: 0x06015FC6 RID: 90054 RVA: 0x006914A8 File Offset: 0x0068F6A8
		private void MoveUpDefaultComparer([TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})] ValueTuple<TElement, TPriority> node, int nodeIndex)
		{
			ValueTuple<TElement, TPriority>[] nodes = this._nodes;
			while (nodeIndex > 0)
			{
				int parentIndex = PriorityQueue<TElement, TPriority>.GetParentIndex(nodeIndex);
				ValueTuple<TElement, TPriority> valueTuple = nodes[parentIndex];
				if (Comparer<TPriority>.Default.Compare(node.Item2, valueTuple.Item2) >= 0)
				{
					break;
				}
				nodes[nodeIndex] = valueTuple;
				nodeIndex = parentIndex;
			}
			nodes[nodeIndex] = node;
		}

		// Token: 0x06015FC7 RID: 90055 RVA: 0x00691500 File Offset: 0x0068F700
		private void MoveUpCustomComparer([TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})] ValueTuple<TElement, TPriority> node, int nodeIndex)
		{
			IComparer<TPriority> comparer = this._comparer;
			ValueTuple<TElement, TPriority>[] nodes = this._nodes;
			while (nodeIndex > 0)
			{
				int parentIndex = PriorityQueue<TElement, TPriority>.GetParentIndex(nodeIndex);
				ValueTuple<TElement, TPriority> valueTuple = nodes[parentIndex];
				if (comparer.Compare(node.Item2, valueTuple.Item2) >= 0)
				{
					break;
				}
				nodes[nodeIndex] = valueTuple;
				nodeIndex = parentIndex;
			}
			nodes[nodeIndex] = node;
		}

		// Token: 0x06015FC8 RID: 90056 RVA: 0x00691558 File Offset: 0x0068F758
		private void MoveDownDefaultComparer([TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})] ValueTuple<TElement, TPriority> node, int nodeIndex)
		{
			ValueTuple<TElement, TPriority>[] nodes = this._nodes;
			int size = this._size;
			int num;
			while ((num = PriorityQueue<TElement, TPriority>.GetFirstChildIndex(nodeIndex)) < size)
			{
				ValueTuple<TElement, TPriority> valueTuple = nodes[num];
				int num2 = num;
				int num3 = Math.Min(num + 4, size);
				while (++num < num3)
				{
					ValueTuple<TElement, TPriority> valueTuple2 = nodes[num];
					if (Comparer<TPriority>.Default.Compare(valueTuple2.Item2, valueTuple.Item2) < 0)
					{
						valueTuple = valueTuple2;
						num2 = num;
					}
				}
				if (Comparer<TPriority>.Default.Compare(node.Item2, valueTuple.Item2) <= 0)
				{
					break;
				}
				nodes[nodeIndex] = valueTuple;
				nodeIndex = num2;
			}
			nodes[nodeIndex] = node;
		}

		// Token: 0x06015FC9 RID: 90057 RVA: 0x006915F8 File Offset: 0x0068F7F8
		private void MoveDownCustomComparer([TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})] ValueTuple<TElement, TPriority> node, int nodeIndex)
		{
			IComparer<TPriority> comparer = this._comparer;
			ValueTuple<TElement, TPriority>[] nodes = this._nodes;
			int size = this._size;
			int num;
			while ((num = PriorityQueue<TElement, TPriority>.GetFirstChildIndex(nodeIndex)) < size)
			{
				ValueTuple<TElement, TPriority> valueTuple = nodes[num];
				int num2 = num;
				int num3 = Math.Min(num + 4, size);
				while (++num < num3)
				{
					ValueTuple<TElement, TPriority> valueTuple2 = nodes[num];
					if (comparer.Compare(valueTuple2.Item2, valueTuple.Item2) < 0)
					{
						valueTuple = valueTuple2;
						num2 = num;
					}
				}
				if (comparer.Compare(node.Item2, valueTuple.Item2) <= 0)
				{
					break;
				}
				nodes[nodeIndex] = valueTuple;
				nodeIndex = num2;
			}
			nodes[nodeIndex] = node;
		}

		// Token: 0x06015FCA RID: 90058 RVA: 0x0069169B File Offset: 0x0068F89B
		private static IComparer<TPriority> InitializeComparer(IComparer<TPriority> comparer)
		{
			if (!typeof(TPriority).IsValueType)
			{
				return comparer ?? Comparer<TPriority>.Default;
			}
			if (comparer == Comparer<TPriority>.Default)
			{
				return null;
			}
			return comparer;
		}

		// Token: 0x0400F37B RID: 62331
		[TupleElementNames(new string[]
		{
			"Element",
			"Priority"
		})]
		private ValueTuple<TElement, TPriority>[] _nodes;

		// Token: 0x0400F37C RID: 62332
		private readonly IComparer<TPriority> _comparer;

		// Token: 0x0400F37D RID: 62333
		private int _size;

		// Token: 0x0400F37E RID: 62334
		private int _version;

		// Token: 0x0400F37F RID: 62335
		private const int Arity = 4;

		// Token: 0x0400F380 RID: 62336
		private const int Log2Arity = 2;
	}
}
