using System;

namespace Verse
{
	// Token: 0x02000D61 RID: 3425
	public class SimpleMovingAverage
	{
		// Token: 0x06005721 RID: 22305 RVA: 0x001C0E49 File Offset: 0x001BF049
		public SimpleMovingAverage(int numValues)
		{
			this.values = new float[numValues];
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x001C0E5D File Offset: 0x001BF05D
		public void AddValue(float value)
		{
			this.values[this.lastValueIndex] = value;
			this.lastValueIndex = (this.lastValueIndex + 1) % this.values.Length;
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x001C0E84 File Offset: 0x001BF084
		public float GetAverage()
		{
			float num = 0f;
			float[] array = this.values;
			int num2 = (array[array.Length - 1] == 0f) ? this.lastValueIndex : this.values.Length;
			for (int i = 0; i < num2; i++)
			{
				num += this.values[i];
			}
			return num / (float)num2;
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x001C0ED6 File Offset: 0x001BF0D6
		public void Reset()
		{
			this.lastValueIndex = 0;
			Array.Clear(this.values, 0, this.values.Length);
		}

		// Token: 0x04003F83 RID: 16259
		private readonly float[] values;

		// Token: 0x04003F84 RID: 16260
		private int lastValueIndex;
	}
}
