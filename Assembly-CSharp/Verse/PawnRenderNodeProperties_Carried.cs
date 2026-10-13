using System;

namespace Verse
{
	// Token: 0x0200062E RID: 1582
	public class PawnRenderNodeProperties_Carried : PawnRenderNodeProperties
	{
		// Token: 0x06002843 RID: 10307 RVA: 0x000D5620 File Offset: 0x000D3820
		public PawnRenderNodeProperties_Carried()
		{
			this.useGraphic = false;
			this.baseLayer = 90f;
			this.drawData = DrawData.NewWithData(new DrawData.RotationalData[]
			{
				new DrawData.RotationalData(new Rot4?(Rot4.North), -10f)
			});
		}
	}
}
