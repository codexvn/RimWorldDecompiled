using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x0200059F RID: 1439
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x06002563 RID: 9571 RVA: 0x000C1100 File Offset: 0x000BF300
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000C1164 File Offset: 0x000BF364
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000C1174 File Offset: 0x000BF374
		public override string ToString()
		{
			int num = (this.operations != null) ? this.operations.Count : 0;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				string str = text;
				string str2 = ", lastFailedOperation=";
				PatchOperation patchOperation = this.lastFailedOperation;
				text = str + str2 + ((patchOperation != null) ? patchOperation.ToString() : null);
			}
			return text + ")";
		}

		// Token: 0x04001EB0 RID: 7856
		private List<PatchOperation> operations;

		// Token: 0x04001EB1 RID: 7857
		private PatchOperation lastFailedOperation;
	}
}
