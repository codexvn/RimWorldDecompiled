using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000350 RID: 848
	public class TimeSlower
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x00066A96 File Offset: 0x00064C96
		public bool ForcedNormalSpeed
		{
			get
			{
				return !DebugViewSettings.neverForceNormalSpeed && Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00066AB3 File Offset: 0x00064CB3
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Mathf.Max(new int[]
			{
				Find.TickManager.TicksGame + 800
			});
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x00066AD9 File Offset: 0x00064CD9
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Mathf.Max(this.forceNormalSpeedUntil, Find.TickManager.TicksGame + 240);
		}

		// Token: 0x04001421 RID: 5153
		private int forceNormalSpeedUntil;

		// Token: 0x04001422 RID: 5154
		private const int ForceTicksStandard = 800;

		// Token: 0x04001423 RID: 5155
		private const int ForceTicksShort = 240;
	}
}
