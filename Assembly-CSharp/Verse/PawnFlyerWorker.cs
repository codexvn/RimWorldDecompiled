using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000248 RID: 584
	public class PawnFlyerWorker
	{
		// Token: 0x06000EF8 RID: 3832 RVA: 0x0004606E File Offset: 0x0004426E
		public PawnFlyerWorker(PawnFlyerProperties properties)
		{
			this.properties = properties;
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00046080 File Offset: 0x00044280
		public virtual float AdjustedProgress(float t)
		{
			AnimationCurve progressCurve = this.properties.ProgressCurve;
			if (progressCurve == null || progressCurve.length == 0)
			{
				return t;
			}
			return progressCurve.Evaluate(t);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x000460AD File Offset: 0x000442AD
		public virtual float GetHeight(float t)
		{
			return GenMath.InverseParabola(t);
		}

		// Token: 0x04000A2F RID: 2607
		public PawnFlyerProperties properties;
	}
}
