using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000E3B RID: 3643
	public class SoundSizeAggregator
	{
		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06005BE9 RID: 23529 RVA: 0x001D6140 File Offset: 0x001D4340
		public float AggregateSize
		{
			get
			{
				if (this.reporters.Count == 0)
				{
					return this.testSize;
				}
				float num = 0f;
				foreach (ISizeReporter sizeReporter in this.reporters)
				{
					num += sizeReporter.CurrentSize();
				}
				return num;
			}
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x001D61B0 File Offset: 0x001D43B0
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x001D61E7 File Offset: 0x001D43E7
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06005BEC RID: 23532 RVA: 0x001D61F5 File Offset: 0x001D43F5
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x04004299 RID: 17049
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x0400429A RID: 17050
		private float testSize;
	}
}
