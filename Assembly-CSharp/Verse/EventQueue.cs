using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000138 RID: 312
	public class EventQueue
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0002D7BD File Offset: 0x0002B9BD
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x0002D7C5 File Offset: 0x0002B9C5
		public float Queued { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0002D7CE File Offset: 0x0002B9CE
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0002D7D6 File Offset: 0x0002B9D6
		public float Threshold { get; private set; }

		// Token: 0x06000998 RID: 2456 RVA: 0x0002D7DF File Offset: 0x0002B9DF
		public EventQueue(float threshold)
		{
			this.Threshold = threshold;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002D7EE File Offset: 0x0002B9EE
		public void Push(float amount)
		{
			this.Queued += amount;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002D7FE File Offset: 0x0002B9FE
		public bool Peek()
		{
			return this.Queued >= this.Threshold;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002D811 File Offset: 0x0002BA11
		public bool Pop()
		{
			if (this.Queued >= this.Threshold)
			{
				this.Queued -= this.Threshold;
				return true;
			}
			return false;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002D837 File Offset: 0x0002BA37
		public int PopAll()
		{
			int result = Mathf.FloorToInt(this.Queued / this.Threshold);
			this.Queued %= this.Threshold;
			return result;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002D85E File Offset: 0x0002BA5E
		public int Clear()
		{
			int result = Mathf.FloorToInt(this.Queued / this.Threshold);
			this.Queued = 0f;
			return result;
		}
	}
}
