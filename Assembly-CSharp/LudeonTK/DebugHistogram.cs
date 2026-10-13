using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000074 RID: 116
	public class DebugHistogram
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000DF97 File Offset: 0x0000C197
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000DFC8 File Offset: 0x0000C1C8
		public void Add(float val)
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				if (this.buckets[i] > val)
				{
					this.counts[i]++;
					return;
				}
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000E008 File Offset: 0x0000C208
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000E030 File Offset: 0x0000C230
		public void Display(StringBuilder sb)
		{
			int num = Mathf.Max(this.counts.Max(), 1);
			int num2 = this.counts.Aggregate((int a, int b) => a + b);
			for (int i = 0; i < this.buckets.Length; i++)
			{
				sb.AppendLine(string.Format("{0}    {1}: {2} ({3:F2}%)", new object[]
				{
					new string('#', this.counts[i] * 40 / num),
					this.buckets[i],
					this.counts[i],
					(double)this.counts[i] * 100.0 / (double)num2
				}));
			}
		}

		// Token: 0x04000170 RID: 368
		private float[] buckets;

		// Token: 0x04000171 RID: 369
		private int[] counts;
	}
}
