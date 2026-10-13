using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200018D RID: 397
	public class DeathActionProperties
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00035C8B File Offset: 0x00033E8B
		public DeathActionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (Activator.CreateInstance(this.workerClass) as DeathActionWorker);
					this.workerInt.props = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00035CBD File Offset: 0x00033EBD
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x0400067A RID: 1658
		public Type workerClass = typeof(DeathActionWorker_Simple);

		// Token: 0x0400067B RID: 1659
		private DeathActionWorker workerInt;
	}
}
