using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E19 RID: 3609
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06005B4F RID: 23375 RVA: 0x001D3CC0 File Offset: 0x001D1EC0
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x001D3CC7 File Offset: 0x001D1EC7
		public override float ValueFor(Sample samp)
		{
			if (this.timeType == TimeType.RealtimeSeconds)
			{
				return Time.realtimeSinceStartup - samp.ParentStartRealTime;
			}
			if (this.timeType == TimeType.Ticks && Find.TickManager != null)
			{
				return (float)Find.TickManager.TicksGame - samp.ParentStartTick;
			}
			return 0f;
		}

		// Token: 0x04004223 RID: 16931
		public TimeType timeType;
	}
}
