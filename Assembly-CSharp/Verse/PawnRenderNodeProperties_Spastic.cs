using System;

namespace Verse
{
	// Token: 0x02000630 RID: 1584
	public class PawnRenderNodeProperties_Spastic : PawnRenderNodeProperties
	{
		// Token: 0x06002846 RID: 10310 RVA: 0x000D5680 File Offset: 0x000D3880
		public PawnRenderNodeProperties_Spastic()
		{
			this.nodeClass = typeof(PawnRenderNode_Spastic);
			this.workerClass = typeof(PawnRenderNodeWorker_Spastic);
		}

		// Token: 0x0400216F RID: 8559
		public bool rotateFacing = true;

		// Token: 0x04002170 RID: 8560
		public FloatRange scaleRange = FloatRange.One;

		// Token: 0x04002171 RID: 8561
		public FloatRange rotationRange = FloatRange.Zero;

		// Token: 0x04002172 RID: 8562
		public FloatRange offsetRangeX = FloatRange.Zero;

		// Token: 0x04002173 RID: 8563
		public FloatRange offsetRangeZ = FloatRange.Zero;

		// Token: 0x04002174 RID: 8564
		public IntRange durationTicksRange = new IntRange(60, 60);

		// Token: 0x04002175 RID: 8565
		public IntRange nextSpasmTicksRange = new IntRange(60, 60);
	}
}
