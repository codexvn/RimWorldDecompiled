using System;

namespace Verse
{
	// Token: 0x02000B56 RID: 2902
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06004AB5 RID: 19125 RVA: 0x0017C300 File Offset: 0x0017A500
		// (set) Token: 0x06004AB6 RID: 19126 RVA: 0x0017C308 File Offset: 0x0017A508
		public int Priority { get; set; }
	}
}
