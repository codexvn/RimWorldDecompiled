using System;

namespace Verse
{
	// Token: 0x02000C18 RID: 3096
	public struct LabelValue
	{
		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06004F82 RID: 20354 RVA: 0x00198912 File Offset: 0x00196B12
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06004F83 RID: 20355 RVA: 0x0019891A File Offset: 0x00196B1A
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x00198922 File Offset: 0x00196B22
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x00198912 File Offset: 0x00196B12
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0400390A RID: 14602
		private string label;

		// Token: 0x0400390B RID: 14603
		private string value;
	}
}
