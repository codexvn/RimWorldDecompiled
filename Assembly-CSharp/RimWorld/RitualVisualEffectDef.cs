using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027E9 RID: 10217
	public class RitualVisualEffectDef : Def
	{
		// Token: 0x0600DFB3 RID: 57267 RVA: 0x0041B1A3 File Offset: 0x004193A3
		public RitualVisualEffect GetInstance()
		{
			RitualVisualEffect ritualVisualEffect = (RitualVisualEffect)Activator.CreateInstance(this.workerClass);
			ritualVisualEffect.def = this;
			return ritualVisualEffect;
		}

		// Token: 0x04009686 RID: 38534
		public Type workerClass = typeof(RitualVisualEffect);

		// Token: 0x04009687 RID: 38535
		public List<CompProperties_RitualVisualEffect> comps;

		// Token: 0x04009688 RID: 38536
		public Color tintColor = Color.white;
	}
}
