using System;

namespace Verse
{
	// Token: 0x0200059B RID: 1435
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x0600255B RID: 9563 RVA: 0x000C0EF3 File Offset: 0x000BF0F3
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x04001EAD RID: 7853
		protected string attribute;
	}
}
