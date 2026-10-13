using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EAB RID: 11947
	public class CompProperties_TreeConnection : CompProperties
	{
		// Token: 0x06010D64 RID: 68964 RVA: 0x004D8EA8 File Offset: 0x004D70A8
		public CompProperties_TreeConnection()
		{
			this.compClass = typeof(CompTreeConnection);
		}

		// Token: 0x0400AF45 RID: 44869
		public float spawnDays = 6f;

		// Token: 0x0400AF46 RID: 44870
		public PawnKindDef pawnKind;

		// Token: 0x0400AF47 RID: 44871
		public FloatRange initialConnectionStrengthRange;

		// Token: 0x0400AF48 RID: 44872
		public float connectionStrengthLossPerDryadDeath = 0.1f;

		// Token: 0x0400AF49 RID: 44873
		public float radiusToBuildingForConnectionStrengthLoss = 7.9f;

		// Token: 0x0400AF4A RID: 44874
		public int maxDryadsWild;

		// Token: 0x0400AF4B RID: 44875
		public SimpleCurve maxDryadsPerConnectionStrengthCurve;

		// Token: 0x0400AF4C RID: 44876
		public SimpleCurve connectionLossPerLevelCurve;

		// Token: 0x0400AF4D RID: 44877
		public SimpleCurve connectionLossDailyPerBuildingDistanceCurve;

		// Token: 0x0400AF4E RID: 44878
		public SimpleCurve connectionStrengthGainPerPlantSkill;

		// Token: 0x0400AF4F RID: 44879
		public float connectionStrengthGainPerHourPruningBase = 0.01f;

		// Token: 0x0400AF50 RID: 44880
		public Vector3 spawningPodOffset;

		// Token: 0x0400AF51 RID: 44881
		public FloatRange spawningPodSizeRange = FloatRange.One;
	}
}
