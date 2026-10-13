using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000206 RID: 518
	public class KeyframeAnimationPart : AnimationPart
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00041A35 File Offset: 0x0003FC35
		protected override Type DefaultWorker
		{
			get
			{
				return typeof(AnimationWorker_Keyframes);
			}
		}

		// Token: 0x04000821 RID: 2081
		public List<Keyframe> keyframes;
	}
}
