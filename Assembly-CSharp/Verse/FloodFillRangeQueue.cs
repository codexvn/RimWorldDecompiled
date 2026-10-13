using System;

namespace Verse
{
	// Token: 0x020004E0 RID: 1248
	public class FloodFillRangeQueue
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x000A89A2 File Offset: 0x000A6BA2
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000A89AA File Offset: 0x000A6BAA
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x000A89C0 File Offset: 0x000A6BC0
		public string PerfDebugString
		{
			get
			{
				return string.Concat(new string[]
				{
					"NumTimesExpanded: ",
					this.debugNumTimesExpanded.ToString(),
					", MaxUsedSize= ",
					this.debugMaxUsedSpace.ToString(),
					", ClaimedSize=",
					this.array.Length.ToString(),
					", UnusedSpace=",
					(this.array.Length - this.debugMaxUsedSpace).ToString()
				});
			}
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000A8A41 File Offset: 0x000A6C41
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000A8A64 File Offset: 0x000A6C64
		public void Enqueue(FloodFillRange r)
		{
			if (this.count + this.head == this.array.Length)
			{
				FloodFillRange[] destinationArray = new FloodFillRange[2 * this.array.Length];
				Array.Copy(this.array, this.head, destinationArray, 0, this.count);
				this.array = destinationArray;
				this.head = 0;
				this.debugNumTimesExpanded++;
			}
			FloodFillRange[] array = this.array;
			int num = this.head;
			int num2 = this.count;
			this.count = num2 + 1;
			array[num + num2] = r;
			this.debugMaxUsedSpace = this.count + this.head;
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000A8B04 File Offset: 0x000A6D04
		public FloodFillRange Dequeue()
		{
			FloodFillRange result = default(FloodFillRange);
			if (this.count > 0)
			{
				result = this.array[this.head];
				this.array[this.head] = default(FloodFillRange);
				this.head++;
				this.count--;
			}
			return result;
		}

		// Token: 0x04001B8F RID: 7055
		private FloodFillRange[] array;

		// Token: 0x04001B90 RID: 7056
		private int count;

		// Token: 0x04001B91 RID: 7057
		private int head;

		// Token: 0x04001B92 RID: 7058
		private int debugNumTimesExpanded;

		// Token: 0x04001B93 RID: 7059
		private int debugMaxUsedSpace;
	}
}
