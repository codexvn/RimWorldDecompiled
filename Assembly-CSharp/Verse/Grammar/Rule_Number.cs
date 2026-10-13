using System;

namespace Verse.Grammar
{
	// Token: 0x02000DF0 RID: 3568
	public class Rule_Number : Rule
	{
		// Token: 0x06005AD5 RID: 23253 RVA: 0x001D250C File Offset: 0x001D070C
		public override Rule DeepCopy()
		{
			Rule_Number rule_Number = (Rule_Number)base.DeepCopy();
			rule_Number.range = this.range;
			rule_Number.selectionWeight = this.selectionWeight;
			return rule_Number;
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x001D2531 File Offset: 0x001D0731
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06005AD7 RID: 23255 RVA: 0x001D253C File Offset: 0x001D073C
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06005AD8 RID: 23256 RVA: 0x001D255C File Offset: 0x001D075C
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		// Token: 0x040041BD RID: 16829
		private IntRange range = IntRange.Zero;

		// Token: 0x040041BE RID: 16830
		public int selectionWeight = 1;
	}
}
