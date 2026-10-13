using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E95 RID: 11925
	public class CompProperties_ThrownMoteEmitter : CompProperties
	{
		// Token: 0x06010CBC RID: 68796 RVA: 0x004D5F10 File Offset: 0x004D4110
		public CompProperties_ThrownMoteEmitter()
		{
			this.compClass = typeof(CompThrownMoteEmitter);
		}

		// Token: 0x06010CBD RID: 68797 RVA: 0x004D5F62 File Offset: 0x004D4162
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompThrownMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		// Token: 0x0400AEDB RID: 44763
		public ThingDef mote;

		// Token: 0x0400AEDC RID: 44764
		public Vector3 offsetMin;

		// Token: 0x0400AEDD RID: 44765
		public Vector3 offsetMax;

		// Token: 0x0400AEDE RID: 44766
		public int emissionInterval = -1;

		// Token: 0x0400AEDF RID: 44767
		public int burstCount = 1;

		// Token: 0x0400AEE0 RID: 44768
		public Color colorA = Color.white;

		// Token: 0x0400AEE1 RID: 44769
		public Color colorB = Color.white;

		// Token: 0x0400AEE2 RID: 44770
		public FloatRange scale = FloatRange.One;

		// Token: 0x0400AEE3 RID: 44771
		public FloatRange rotationRate;

		// Token: 0x0400AEE4 RID: 44772
		public FloatRange velocityX;

		// Token: 0x0400AEE5 RID: 44773
		public FloatRange velocityY;
	}
}
