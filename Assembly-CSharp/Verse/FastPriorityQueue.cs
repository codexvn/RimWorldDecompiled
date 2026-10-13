using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CC8 RID: 3272
	public class FastPriorityQueue<T>
	{
		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06005390 RID: 21392 RVA: 0x001AED64 File Offset: 0x001ACF64
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x001AED71 File Offset: 0x001ACF71
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x001AED8F File Offset: 0x001ACF8F
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x001AEDAC File Offset: 0x001ACFAC
		public void Push(T item)
		{
			int num = this.innerList.Count;
			this.innerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.CompareElements(num, num2) >= 0)
				{
					break;
				}
				this.SwapElements(num, num2);
				num = num2;
			}
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x001AEDF4 File Offset: 0x001ACFF4
		public T Pop()
		{
			T result = this.innerList[0];
			int num = 0;
			int count = this.innerList.Count;
			this.innerList[0] = this.innerList[count - 1];
			this.innerList.RemoveAt(count - 1);
			count = this.innerList.Count;
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = num3 + 1;
				if (num3 < count && this.CompareElements(num, num3) > 0)
				{
					num = num3;
				}
				if (num4 < count && this.CompareElements(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwapElements(num, num2);
			}
			return result;
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x001AEE96 File Offset: 0x001AD096
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x001AEEA4 File Offset: 0x001AD0A4
		protected void SwapElements(int i, int j)
		{
			List<T> list = this.innerList;
			List<T> list2 = this.innerList;
			T value = this.innerList[j];
			T value2 = this.innerList[i];
			list[i] = value;
			list2[j] = value2;
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x001AEEF3 File Offset: 0x001AD0F3
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x001AEF18 File Offset: 0x001AD118
		public bool Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x04003C4B RID: 15435
		protected List<T> innerList = new List<T>();

		// Token: 0x04003C4C RID: 15436
		protected IComparer<T> comparer;
	}
}
