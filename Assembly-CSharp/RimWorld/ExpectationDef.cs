using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A22 RID: 6690
	public class ExpectationDef : Def
	{
		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x060096D1 RID: 38609 RVA: 0x002C19D3 File Offset: 0x002BFBD3
		public bool WealthTriggered
		{
			get
			{
				return this.maxMapWealth >= 0f;
			}
		}

		// Token: 0x060096D2 RID: 38610 RVA: 0x002C19E5 File Offset: 0x002BFBE5
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.order < 0)
			{
				yield return "order not defined";
			}
			yield break;
		}

		// Token: 0x04006359 RID: 25433
		public int order = -1;

		// Token: 0x0400635A RID: 25434
		public int thoughtStage = -1;

		// Token: 0x0400635B RID: 25435
		public float maxMapWealth = -1f;

		// Token: 0x0400635C RID: 25436
		public float joyToleranceDropPerDay;

		// Token: 0x0400635D RID: 25437
		public float ritualQualityOffset;

		// Token: 0x0400635E RID: 25438
		public int joyKindsNeeded;

		// Token: 0x0400635F RID: 25439
		public bool forRoles;
	}
}
