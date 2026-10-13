using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A7 RID: 2215
	public class Graphic_Multi : Graphic
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x0600380B RID: 14347 RVA: 0x0012B479 File Offset: 0x00129679
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x0600380C RID: 14348 RVA: 0x0012B481 File Offset: 0x00129681
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x0600380D RID: 14349 RVA: 0x0012B489 File Offset: 0x00129689
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x0600380E RID: 14350 RVA: 0x0012B493 File Offset: 0x00129693
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600380F RID: 14351 RVA: 0x0012B49D File Offset: 0x0012969D
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06003810 RID: 14352 RVA: 0x0012B4A7 File Offset: 0x001296A7
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003811 RID: 14353 RVA: 0x0012B4B1 File Offset: 0x001296B1
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06003812 RID: 14354 RVA: 0x0012B4B9 File Offset: 0x001296B9
		public override bool EastFlipped
		{
			get
			{
				return this.eastFlipped;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06003813 RID: 14355 RVA: 0x0012B4C1 File Offset: 0x001296C1
		public override bool ShouldDrawRotated
		{
			get
			{
				return (this.data == null || this.data.drawRotated) && (this.MatEast == this.MatNorth || this.MatWest == this.MatNorth);
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06003814 RID: 14356 RVA: 0x0012B500 File Offset: 0x00129700
		public override float DrawRotatedExtraAngleOffset
		{
			get
			{
				return this.drawRotatedExtraAngleOffset;
			}
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x0012B508 File Offset: 0x00129708
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			foreach (Material material in this.mats)
			{
				Texture2D mask = null;
				if (material.HasProperty(ShaderPropertyIDs.MaskTex))
				{
					mask = (Texture2D)material.GetTexture(ShaderPropertyIDs.MaskTex);
				}
				GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)material.mainTexture, mask);
			}
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x0012B564 File Offset: 0x00129764
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[this.mats.Length];
			array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
			array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
			array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
			array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
			if (array[0] == null)
			{
				if (array[2] != null)
				{
					array[0] = array[2];
					this.drawRotatedExtraAngleOffset = 180f;
				}
				else if (array[1] != null)
				{
					array[0] = array[1];
					this.drawRotatedExtraAngleOffset = -90f;
				}
				else if (array[3] != null)
				{
					array[0] = array[3];
					this.drawRotatedExtraAngleOffset = 90f;
				}
				else
				{
					array[0] = ContentFinder<Texture2D>.Get(req.path, false);
				}
			}
			if (array[0] == null)
			{
				Log.Error("Failed to find any textures at " + req.path + " while constructing " + this.ToStringSafe<Graphic_Multi>());
				this.mats[0] = (this.mats[1] = (this.mats[2] = (this.mats[3] = BaseContent.BadMat)));
				return;
			}
			if (array[2] == null)
			{
				array[2] = array[0];
			}
			if (array[1] == null)
			{
				if (array[3] != null)
				{
					array[1] = array[3];
					this.eastFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[1] = array[0];
				}
			}
			if (array[3] == null)
			{
				if (array[1] != null)
				{
					array[3] = array[1];
					this.westFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[3] = array[0];
				}
			}
			Texture2D[] array2 = new Texture2D[this.mats.Length];
			if (req.shader.SupportsMaskTex())
			{
				string str = this.maskPath.NullOrEmpty() ? this.path : this.maskPath;
				string str2 = this.maskPath.NullOrEmpty() ? "m" : string.Empty;
				array2[0] = ContentFinder<Texture2D>.Get(str + "_north" + str2, false);
				array2[1] = ContentFinder<Texture2D>.Get(str + "_east" + str2, false);
				array2[2] = ContentFinder<Texture2D>.Get(str + "_south" + str2, false);
				array2[3] = ContentFinder<Texture2D>.Get(str + "_west" + str2, false);
				if (array2[0] == null)
				{
					if (array2[2] != null)
					{
						array2[0] = array2[2];
					}
					else if (array2[1] != null)
					{
						array2[0] = array2[1];
					}
					else if (array2[3] != null)
					{
						array2[0] = array2[3];
					}
				}
				if (array2[2] == null)
				{
					array2[2] = array2[0];
				}
				if (array2[1] == null)
				{
					if (array2[3] != null)
					{
						array2[1] = array2[3];
					}
					else
					{
						array2[1] = array2[0];
					}
				}
				if (array2[3] == null)
				{
					if (array2[1] != null)
					{
						array2[3] = array2[1];
					}
					else
					{
						array2[3] = array2[0];
					}
				}
			}
			for (int i = 0; i < this.mats.Length; i++)
			{
				MaterialRequest req2 = default(MaterialRequest);
				req2.mainTex = array[i];
				req2.shader = req.shader;
				req2.color = this.color;
				req2.colorTwo = this.colorTwo;
				req2.maskTex = array2[i];
				req2.shaderParameters = req.shaderParameters;
				req2.renderQueue = req.renderQueue;
				this.mats[i] = MaterialPool.MatFrom(req2);
			}
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x0012B93B File Offset: 0x00129B3B
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, this.maskPath);
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x0012B960 File Offset: 0x00129B60
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Multi(initPath=";
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

		// Token: 0x06003819 RID: 14361 RVA: 0x0012B9CF File Offset: 0x00129BCF
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<string>(0, this.path), this.color), this.colorTwo);
		}

		// Token: 0x040029AB RID: 10667
		private Material[] mats = new Material[4];

		// Token: 0x040029AC RID: 10668
		private bool westFlipped;

		// Token: 0x040029AD RID: 10669
		private bool eastFlipped;

		// Token: 0x040029AE RID: 10670
		private float drawRotatedExtraAngleOffset;

		// Token: 0x040029AF RID: 10671
		public const string NorthSuffix = "_north";

		// Token: 0x040029B0 RID: 10672
		public const string SouthSuffix = "_south";

		// Token: 0x040029B1 RID: 10673
		public const string EastSuffix = "_east";

		// Token: 0x040029B2 RID: 10674
		public const string WestSuffix = "_west";
	}
}
