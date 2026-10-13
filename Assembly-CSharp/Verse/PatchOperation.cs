using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000590 RID: 1424
	public class PatchOperation
	{
		// Token: 0x06002548 RID: 9544 RVA: 0x000C08FC File Offset: 0x000BEAFC
		public bool Apply(XmlDocument xml)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(base.GetType().FullName + " Worker");
			}
			bool flag = this.ApplyWorker(xml);
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
			if (this.success == PatchOperation.Success.Always)
			{
				flag = true;
			}
			else if (this.success == PatchOperation.Success.Never)
			{
				flag = false;
			}
			else if (this.success == PatchOperation.Success.Invert)
			{
				flag = !flag;
			}
			if (flag)
			{
				this.neverSucceeded = false;
			}
			return flag;
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000C0975 File Offset: 0x000BEB75
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail");
			return false;
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000C0984 File Offset: 0x000BEB84
		public virtual void Complete(string modIdentifier)
		{
			if (this.neverSucceeded)
			{
				string text = string.Format("[{0}] Patch operation {1} failed", modIdentifier, this);
				if (!string.IsNullOrEmpty(this.sourceFile))
				{
					text = text + "\nfile: " + this.sourceFile;
				}
				Log.Error(text);
			}
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x04001E97 RID: 7831
		public string sourceFile;

		// Token: 0x04001E98 RID: 7832
		private bool neverSucceeded = true;

		// Token: 0x04001E99 RID: 7833
		private PatchOperation.Success success;

		// Token: 0x02000591 RID: 1425
		private enum Success
		{
			// Token: 0x04001E9B RID: 7835
			Normal,
			// Token: 0x04001E9C RID: 7836
			Invert,
			// Token: 0x04001E9D RID: 7837
			Always,
			// Token: 0x04001E9E RID: 7838
			Never
		}
	}
}
