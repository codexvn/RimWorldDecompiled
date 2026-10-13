using System;

namespace Verse
{
	// Token: 0x02000631 RID: 1585
	public class PawnRenderNodeProperties_BulbfreakTentacle : PawnRenderNodeProperties_Spastic
	{
		// Token: 0x06002847 RID: 10311 RVA: 0x000D5704 File Offset: 0x000D3904
		public PawnRenderNodeProperties_BulbfreakTentacle()
		{
			this.rotateFacing = true;
			this.baseLayer = 10f;
			this.scaleRange = new FloatRange(0.7f, 1.2f);
			this.rotationRange = new FloatRange(-25f, 25f);
			this.durationTicksRange = new IntRange(10, 35);
			this.nextSpasmTicksRange = new IntRange(0, 20);
		}
	}
}
