using System;

namespace Verse
{
	// Token: 0x02000B5A RID: 2906
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06004ABC RID: 19132 RVA: 0x0017C375 File Offset: 0x0017A575
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
