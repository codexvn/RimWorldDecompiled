using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000767 RID: 1895
	public class Hediff_RapidRegeneration : Hediff
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002E76 RID: 11894 RVA: 0x000EEFC1 File Offset: 0x000ED1C1
		public override string SeverityLabel
		{
			get
			{
				return string.Format("{0:0} / {1:0}{2}", this.hpRemaining, this.hpCapacity, "HP".Translate());
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002E77 RID: 11895 RVA: 0x000EEFF2 File Offset: 0x000ED1F2
		public override bool ShouldRemove
		{
			get
			{
				return this.hpRemaining <= 0f;
			}
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000EF004 File Offset: 0x000ED204
		public void SetHpCapacity(float amount)
		{
			this.hpCapacity = amount;
			this.hpRemaining = amount;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x000EF021 File Offset: 0x000ED221
		public override void Notify_Regenerated(float hp)
		{
			this.hpRemaining = Mathf.Max(this.hpRemaining - hp, 0f);
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000EF03B File Offset: 0x000ED23B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.hpRemaining, "hpRemaining", 0f, false);
			Scribe_Values.Look<float>(ref this.hpCapacity, "hpCapacity", 0f, false);
		}

		// Token: 0x04002468 RID: 9320
		private float hpRemaining = 100f;

		// Token: 0x04002469 RID: 9321
		private float hpCapacity = 100f;
	}
}
