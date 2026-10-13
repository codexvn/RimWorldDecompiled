using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027EB RID: 10219
	public class CompProperties_RitualVisualEffect
	{
		// Token: 0x0600DFBC RID: 57276 RVA: 0x0041B464 File Offset: 0x00419664
		public CompProperties_RitualVisualEffect()
		{
		}

		// Token: 0x0600DFBD RID: 57277 RVA: 0x0041B4E0 File Offset: 0x004196E0
		public CompProperties_RitualVisualEffect(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x0600DFBE RID: 57278 RVA: 0x0041B562 File Offset: 0x00419762
		public RitualVisualEffectComp GetInstance()
		{
			return (RitualVisualEffectComp)Activator.CreateInstance(this.compClass);
		}

		// Token: 0x0400968E RID: 38542
		public Type compClass = typeof(RitualVisualEffectComp);

		// Token: 0x0400968F RID: 38543
		public ThingDef moteDef;

		// Token: 0x04009690 RID: 38544
		public FleckDef fleckDef;

		// Token: 0x04009691 RID: 38545
		public EffecterDef effecterDef;

		// Token: 0x04009692 RID: 38546
		public FloatRange velocity = FloatRange.Zero;

		// Token: 0x04009693 RID: 38547
		public Vector3 velocityDir = Vector3.zero;

		// Token: 0x04009694 RID: 38548
		public FloatRange rotation = FloatRange.Zero;

		// Token: 0x04009695 RID: 38549
		public FloatRange rotationRate = FloatRange.Zero;

		// Token: 0x04009696 RID: 38550
		public FloatRange scale = FloatRange.One;

		// Token: 0x04009697 RID: 38551
		public Vector3 offset = Vector3.zero;

		// Token: 0x04009698 RID: 38552
		public IntVec3 roomCheckOffset = IntVec3.Zero;

		// Token: 0x04009699 RID: 38553
		public bool scaleWithRoom;

		// Token: 0x0400969A RID: 38554
		public bool scalePositionWithRoom;

		// Token: 0x0400969B RID: 38555
		public bool onlySpawnInSameRoom;

		// Token: 0x0400969C RID: 38556
		public Color? colorOverride;

		// Token: 0x0400969D RID: 38557
		public List<Color> overrideColors = new List<Color>();
	}
}
