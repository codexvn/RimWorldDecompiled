using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D1E RID: 11550
	public class CompProperties_FleckEmitterLongTerm : CompProperties
	{
		// Token: 0x060103A7 RID: 66471 RVA: 0x004B1694 File Offset: 0x004AF894
		public CompProperties_FleckEmitterLongTerm()
		{
			this.compClass = typeof(CompFleckEmitterLongTerm);
		}

		// Token: 0x060103A8 RID: 66472 RVA: 0x004B16E3 File Offset: 0x004AF8E3
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.fleckDef == null)
			{
				yield return "CompLongTermFleckEmitter must have a fleck assigned.";
			}
			yield break;
		}

		// Token: 0x0400A97A RID: 43386
		public FleckDef fleckDef;

		// Token: 0x0400A97B RID: 43387
		public float spawnRadius;

		// Token: 0x0400A97C RID: 43388
		public float spawnChance;

		// Token: 0x0400A97D RID: 43389
		public Vector3 spawnOffsetFromCenter = Vector3.zero;

		// Token: 0x0400A97E RID: 43390
		public float prewarmCycles;

		// Token: 0x0400A97F RID: 43391
		public float fleckScale = 1f;

		// Token: 0x0400A980 RID: 43392
		public float minRotationRate;

		// Token: 0x0400A981 RID: 43393
		public float maxRotationRate;

		// Token: 0x0400A982 RID: 43394
		public float minVelocityAngle;

		// Token: 0x0400A983 RID: 43395
		public float maxVelocityAngle;

		// Token: 0x0400A984 RID: 43396
		public float minVelocitySpeed = 1f;

		// Token: 0x0400A985 RID: 43397
		public float maxVelocitySpeed = 1f;

		// Token: 0x0400A986 RID: 43398
		public bool forceEnabled;
	}
}
