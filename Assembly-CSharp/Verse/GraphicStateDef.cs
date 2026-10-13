using System;

namespace Verse
{
	// Token: 0x02000284 RID: 644
	public class GraphicStateDef : Def
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0004BF94 File Offset: 0x0004A194
		public Graphic DefaultGraphic
		{
			get
			{
				return this.defaultGraphicData.Graphic;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x0004BFA1 File Offset: 0x0004A1A1
		public GraphicData DefaultGraphicData
		{
			get
			{
				return this.defaultGraphicData;
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0004BFA9 File Offset: 0x0004A1A9
		public bool TryGetDefaultGraphic(out Graphic graphic)
		{
			GraphicData graphicData = this.defaultGraphicData;
			graphic = ((graphicData != null) ? graphicData.Graphic : null);
			return graphic != null;
		}

		// Token: 0x04000CC0 RID: 3264
		public GraphicData defaultGraphicData;

		// Token: 0x04000CC1 RID: 3265
		public bool applySkinColorTint;
	}
}
