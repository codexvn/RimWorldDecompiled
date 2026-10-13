using System;

namespace Verse
{
	// Token: 0x02000B57 RID: 2903
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x06004AB8 RID: 19128 RVA: 0x0017C311 File Offset: 0x0017A511
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0400350A RID: 13578
		public float min;

		// Token: 0x0400350B RID: 13579
		public float max = 1f;
	}
}
