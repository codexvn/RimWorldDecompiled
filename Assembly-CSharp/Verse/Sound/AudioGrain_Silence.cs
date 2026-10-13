using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DFE RID: 3582
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x06005B08 RID: 23304 RVA: 0x001D31CF File Offset: 0x001D13CF
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x001D31DF File Offset: 0x001D13DF
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		// Token: 0x040041EF RID: 16879
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
