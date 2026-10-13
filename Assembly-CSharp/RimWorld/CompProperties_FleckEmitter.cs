using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C8C RID: 11404
	public class CompProperties_FleckEmitter : CompProperties
	{
		// Token: 0x0600FFF2 RID: 65522 RVA: 0x004A2427 File Offset: 0x004A0627
		public CompProperties_FleckEmitter()
		{
			this.compClass = typeof(CompFleckEmitter);
		}

		// Token: 0x0600FFF3 RID: 65523 RVA: 0x004A2446 File Offset: 0x004A0646
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.fleck == null)
			{
				yield return "CompFleckEmitter must have a fleck assigned.";
			}
			yield break;
		}

		// Token: 0x0400A75C RID: 42844
		public FleckDef fleck;

		// Token: 0x0400A75D RID: 42845
		public Vector3 offset;

		// Token: 0x0400A75E RID: 42846
		public int emissionInterval = -1;

		// Token: 0x0400A75F RID: 42847
		public SoundDef soundOnEmission;

		// Token: 0x0400A760 RID: 42848
		[NoTranslate]
		public string saveKeysPrefix;
	}
}
