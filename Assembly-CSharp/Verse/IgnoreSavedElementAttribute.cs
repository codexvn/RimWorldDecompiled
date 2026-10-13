using System;

namespace Verse
{
	// Token: 0x02000B62 RID: 2914
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x06004AC5 RID: 19141 RVA: 0x0017C42C File Offset: 0x0017A62C
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x04003510 RID: 13584
		public string elementToIgnore;
	}
}
