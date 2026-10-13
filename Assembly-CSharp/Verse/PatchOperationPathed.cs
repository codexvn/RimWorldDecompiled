using System;

namespace Verse
{
	// Token: 0x02000592 RID: 1426
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x0600254D RID: 9549 RVA: 0x000C09DA File Offset: 0x000BEBDA
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x04001E9F RID: 7839
		protected string xpath;
	}
}
