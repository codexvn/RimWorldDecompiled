using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA2 RID: 7842
	public class FireWatcher
	{
		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x0600AE07 RID: 44551 RVA: 0x00319FB0 File Offset: 0x003181B0
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x0600AE08 RID: 44552 RVA: 0x00319FB8 File Offset: 0x003181B8
		public bool LargeFireDangerPresent
		{
			get
			{
				if (this.fireDanger < 0f)
				{
					this.UpdateObservations();
				}
				return this.fireDanger > 90f;
			}
		}

		// Token: 0x0600AE09 RID: 44553 RVA: 0x00319FDA File Offset: 0x003181DA
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600AE0A RID: 44554 RVA: 0x00319FF4 File Offset: 0x003181F4
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x0600AE0B RID: 44555 RVA: 0x0031A010 File Offset: 0x00318210
		private void UpdateObservations()
		{
			this.fireDanger = 0f;
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.Fire);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				this.fireDanger += 0.5f + fire.fireSize;
			}
		}

		// Token: 0x040077B3 RID: 30643
		private Map map;

		// Token: 0x040077B4 RID: 30644
		private float fireDanger = -1f;

		// Token: 0x040077B5 RID: 30645
		private const int UpdateObservationsInterval = 426;

		// Token: 0x040077B6 RID: 30646
		private const float BaseDangerPerFire = 0.5f;
	}
}
