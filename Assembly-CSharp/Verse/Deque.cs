using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C7B RID: 3195
	internal class Deque<T>
	{
		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x060051B7 RID: 20919 RVA: 0x001A7EDC File Offset: 0x001A60DC
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x001A7EE7 File Offset: 0x001A60E7
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x001A7F0C File Offset: 0x001A610C
		public void PushFront(T item)
		{
			this.PushPrep();
			this.first--;
			if (this.first < 0)
			{
				this.first += this.data.Length;
			}
			this.count++;
			this.data[this.first] = item;
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001A7F6C File Offset: 0x001A616C
		public void PushBack(T item)
		{
			this.PushPrep();
			T[] array = this.data;
			int num = this.first;
			int num2 = this.count;
			this.count = num2 + 1;
			array[(num + num2) % this.data.Length] = item;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001A7FAC File Offset: 0x001A61AC
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x001A8009 File Offset: 0x001A6209
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x001A801C File Offset: 0x001A621C
		private void PushPrep()
		{
			if (this.count < this.data.Length)
			{
				return;
			}
			T[] destinationArray = new T[this.data.Length * 2];
			Array.Copy(this.data, this.first, destinationArray, 0, Mathf.Min(this.count, this.data.Length - this.first));
			if (this.first + this.count > this.data.Length)
			{
				Array.Copy(this.data, 0, destinationArray, this.data.Length - this.first, this.count - this.data.Length + this.first);
			}
			this.data = destinationArray;
			this.first = 0;
		}

		// Token: 0x04003ACC RID: 15052
		private T[] data;

		// Token: 0x04003ACD RID: 15053
		private int first;

		// Token: 0x04003ACE RID: 15054
		private int count;
	}
}
