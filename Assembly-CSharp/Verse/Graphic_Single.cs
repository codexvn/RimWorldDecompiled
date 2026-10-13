using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008AF RID: 2223
	public class Graphic_Single : Graphic
	{
		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06003844 RID: 14404 RVA: 0x0012C530 File Offset: 0x0012A730
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06003845 RID: 14405 RVA: 0x0012C530 File Offset: 0x0012A730
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06003846 RID: 14406 RVA: 0x0012C530 File Offset: 0x0012A730
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06003847 RID: 14407 RVA: 0x0012C530 File Offset: 0x0012A730
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06003848 RID: 14408 RVA: 0x0012C530 File Offset: 0x0012A730
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06003849 RID: 14409 RVA: 0x0012C538 File Offset: 0x0012A738
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x0012C554 File Offset: 0x0012A754
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Texture2D mask = null;
			if (this.mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				mask = (Texture2D)this.mat.GetTexture(ShaderPropertyIDs.MaskTex);
			}
			GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)this.mat.mainTexture, mask);
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x0012C5A4 File Offset: 0x0012A7A4
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			MaterialRequest req2 = new MaterialRequest(req.texture ?? ContentFinder<Texture2D>.Get(req.path, true), req.shader, this.color)
			{
				colorTwo = this.colorTwo,
				renderQueue = req.renderQueue,
				shaderParameters = req.shaderParameters
			};
			if (req.shader.SupportsMaskTex())
			{
				req2.maskTex = ContentFinder<Texture2D>.Get(this.maskPath.NullOrEmpty() ? (this.path + Graphic_Single.MaskSuffix) : this.maskPath, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x0012C696 File Offset: 0x0012A896
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x0012C530 File Offset: 0x0012A730
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x0012C6B4 File Offset: 0x0012A8B4
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Single(path=";
			array[1] = this.path;
			array[2] = ", color=";
			int num = 3;
			Color color = this.color;
			array[num] = color.ToString();
			array[4] = ", colorTwo=";
			int num2 = 5;
			color = this.colorTwo;
			array[num2] = color.ToString();
			array[6] = ")";
			return string.Concat(array);
		}

		// Token: 0x040029BF RID: 10687
		protected Material mat;

		// Token: 0x040029C0 RID: 10688
		public static readonly string MaskSuffix = "_m";
	}
}
