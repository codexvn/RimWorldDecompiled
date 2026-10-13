using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E92 RID: 11922
	public class CompProperties_ThrownFleckEmitter : CompProperties
	{
		// Token: 0x06010CAA RID: 68778 RVA: 0x004D5B0D File Offset: 0x004D3D0D
		public CompProperties_ThrownFleckEmitter()
		{
			this.compClass = typeof(CompThrownFleckEmitter);
		}

		// Token: 0x06010CAB RID: 68779 RVA: 0x004D5B49 File Offset: 0x004D3D49
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.fleck == null)
			{
				yield return "CompThrownFleckEmitter must have a fleck assigned.";
			}
			yield break;
		}

		// Token: 0x0400AECA RID: 44746
		public FleckDef fleck;

		// Token: 0x0400AECB RID: 44747
		public Vector3 offsetMin;

		// Token: 0x0400AECC RID: 44748
		public Vector3 offsetMax;

		// Token: 0x0400AECD RID: 44749
		public int emissionInterval = -1;

		// Token: 0x0400AECE RID: 44750
		public int burstCount = 1;

		// Token: 0x0400AECF RID: 44751
		public Color colorA = Color.white;

		// Token: 0x0400AED0 RID: 44752
		public Color colorB = Color.white;

		// Token: 0x0400AED1 RID: 44753
		public FloatRange scale;

		// Token: 0x0400AED2 RID: 44754
		public FloatRange rotationRate;

		// Token: 0x0400AED3 RID: 44755
		public FloatRange velocityX;

		// Token: 0x0400AED4 RID: 44756
		public FloatRange velocityY;
	}
}
