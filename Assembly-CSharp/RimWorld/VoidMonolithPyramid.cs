using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ACA RID: 10954
	public class VoidMonolithPyramid : ThingWithComps
	{
		// Token: 0x17002626 RID: 9766
		// (get) Token: 0x0600F40A RID: 62474 RVA: 0x0046EAB0 File Offset: 0x0046CCB0
		public override Vector3 DrawPos
		{
			get
			{
				Vector3 drawPos = base.DrawPos;
				float num = Mathf.Sin(GenTicks.TicksGame.TicksToSeconds() / 2f) * 0.1f;
				drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor() - 0.4f;
				drawPos.z += num;
				return drawPos;
			}
		}

		// Token: 0x0400A0E1 RID: 41185
		private const float PyramidMaxOffset = 0.1f;

		// Token: 0x0400A0E2 RID: 41186
		private const float PyramidOffsetTimeFactor = 2f;
	}
}
