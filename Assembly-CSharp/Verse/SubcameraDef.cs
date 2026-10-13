using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EA RID: 746
	public class SubcameraDef : Def
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x00055400 File Offset: 0x00053600
		public int LayerId
		{
			get
			{
				if (this.layerCached == -1)
				{
					this.layerCached = LayerMask.NameToLayer(this.layer);
				}
				return this.layerCached;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600124D RID: 4685 RVA: 0x00055424 File Offset: 0x00053624
		public RenderTextureFormat BestFormat
		{
			get
			{
				if (SystemInfo.SupportsRenderTextureFormat(this.format))
				{
					return this.format;
				}
				if (this.format == RenderTextureFormat.R8 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG16))
				{
					return RenderTextureFormat.RG16;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RG16) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
				{
					return RenderTextureFormat.ARGB32;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
				{
					return RenderTextureFormat.RGFloat;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat || this.format == RenderTextureFormat.RGFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
				{
					return RenderTextureFormat.ARGBFloat;
				}
				return this.format;
			}
		}

		// Token: 0x04001093 RID: 4243
		[NoTranslate]
		public string layer;

		// Token: 0x04001094 RID: 4244
		public int depth;

		// Token: 0x04001095 RID: 4245
		public RenderTextureFormat format;

		// Token: 0x04001096 RID: 4246
		public bool startEnabled = true;

		// Token: 0x04001097 RID: 4247
		public bool doNotUpdate;

		// Token: 0x04001098 RID: 4248
		public Color backgroundColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04001099 RID: 4249
		[Unsaved(false)]
		private int layerCached = -1;
	}
}
