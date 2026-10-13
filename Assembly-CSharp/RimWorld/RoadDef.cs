using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD6 RID: 6870
	public class RoadDef : Def
	{
		// Token: 0x060099AA RID: 39338 RVA: 0x002CBECC File Offset: 0x002CA0CC
		public float GetLayerWidth(RoadWorldLayerDef def)
		{
			if (this.cachedLayerWidth == null)
			{
				this.cachedLayerWidth = new float[DefDatabase<RoadWorldLayerDef>.DefCount];
				for (int i = 0; i < DefDatabase<RoadWorldLayerDef>.DefCount; i++)
				{
					RoadWorldLayerDef roadWorldLayerDef = DefDatabase<RoadWorldLayerDef>.AllDefsListForReading[i];
					if (this.worldRenderSteps != null)
					{
						foreach (RoadDef.WorldRenderStep worldRenderStep in this.worldRenderSteps)
						{
							if (worldRenderStep.layer == roadWorldLayerDef)
							{
								this.cachedLayerWidth[(int)roadWorldLayerDef.index] = worldRenderStep.width;
							}
						}
					}
				}
			}
			return this.cachedLayerWidth[(int)def.index];
		}

		// Token: 0x060099AB RID: 39339 RVA: 0x002CBF80 File Offset: 0x002CA180
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedLayerWidth = null;
		}

		// Token: 0x04006939 RID: 26937
		public int priority;

		// Token: 0x0400693A RID: 26938
		public bool ancientOnly;

		// Token: 0x0400693B RID: 26939
		public float movementCostMultiplier = 1f;

		// Token: 0x0400693C RID: 26940
		public int tilesPerSegment = 15;

		// Token: 0x0400693D RID: 26941
		public RoadPathingDef pathingMode;

		// Token: 0x0400693E RID: 26942
		public List<RoadDefGenStep> roadGenSteps;

		// Token: 0x0400693F RID: 26943
		public List<RoadDef.WorldRenderStep> worldRenderSteps;

		// Token: 0x04006940 RID: 26944
		[NoTranslate]
		public string worldTransitionGroup = "";

		// Token: 0x04006941 RID: 26945
		public float distortionFrequency = 1f;

		// Token: 0x04006942 RID: 26946
		public float distortionIntensity;

		// Token: 0x04006943 RID: 26947
		[Unsaved(false)]
		private float[] cachedLayerWidth;

		// Token: 0x02001AD7 RID: 6871
		public class WorldRenderStep
		{
			// Token: 0x04006944 RID: 26948
			public RoadWorldLayerDef layer;

			// Token: 0x04006945 RID: 26949
			public float width;
		}
	}
}
