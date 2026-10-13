using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000246 RID: 582
	public class MoteProperties
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x00045F37 File Offset: 0x00044137
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x04000A0E RID: 2574
		public bool realTime;

		// Token: 0x04000A0F RID: 2575
		public float fadeInTime;

		// Token: 0x04000A10 RID: 2576
		public float solidTime = 1f;

		// Token: 0x04000A11 RID: 2577
		public float fadeOutTime;

		// Token: 0x04000A12 RID: 2578
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x04000A13 RID: 2579
		public float speedPerTime;

		// Token: 0x04000A14 RID: 2580
		public float growthRate;

		// Token: 0x04000A15 RID: 2581
		public bool collide;

		// Token: 0x04000A16 RID: 2582
		public float archHeight;

		// Token: 0x04000A17 RID: 2583
		public float archDuration;

		// Token: 0x04000A18 RID: 2584
		public float archStartOffset;

		// Token: 0x04000A19 RID: 2585
		public SoundDef landSound;

		// Token: 0x04000A1A RID: 2586
		public Vector3 unattachedDrawOffset = Vector3.zero;

		// Token: 0x04000A1B RID: 2587
		public Vector3 attachedDrawOffset;

		// Token: 0x04000A1C RID: 2588
		public bool needsMaintenance;

		// Token: 0x04000A1D RID: 2589
		public bool rotateTowardsTarget;

		// Token: 0x04000A1E RID: 2590
		public bool rotateTowardsMoveDirection;

		// Token: 0x04000A1F RID: 2591
		public bool scaleToConnectTargets;

		// Token: 0x04000A20 RID: 2592
		public bool attachedToHead;

		// Token: 0x04000A21 RID: 2593
		public bool fadeOutUnmaintained;

		// Token: 0x04000A22 RID: 2594
		public bool updateOffsetToMatchTargetRotation;

		// Token: 0x04000A23 RID: 2595
		public float yFightingOffsetScalar01 = 1f;

		// Token: 0x04000A24 RID: 2596
		public List<CurvedScaler> scalers;
	}
}
