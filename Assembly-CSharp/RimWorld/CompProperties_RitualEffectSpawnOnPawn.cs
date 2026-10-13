using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002803 RID: 10243
	public class CompProperties_RitualEffectSpawnOnPawn : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x040096BE RID: 38590
		public Vector3 westRotationOffset;

		// Token: 0x040096BF RID: 38591
		public Vector3 eastRotationOffset;

		// Token: 0x040096C0 RID: 38592
		public Vector3 northRotationOffset;

		// Token: 0x040096C1 RID: 38593
		public Vector3 southRotationOffset;

		// Token: 0x040096C2 RID: 38594
		[NoTranslate]
		public string requiredTag;
	}
}
