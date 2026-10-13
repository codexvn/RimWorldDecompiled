using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000285 RID: 645
	public class HeadTypeDef : Def
	{
		// Token: 0x06001030 RID: 4144 RVA: 0x0004BFC4 File Offset: 0x0004A1C4
		public Graphic_Multi GetGraphic(Pawn pawn, Color color)
		{
			Shader shader = (pawn.Drawer.renderer.StatueColor != null) ? ShaderDatabase.Cutout : ShaderUtility.GetSkinShader(pawn);
			for (int i = 0; i < this.graphics.Count; i++)
			{
				if (color.IndistinguishableFrom(this.graphics[i].Key) && this.graphics[i].Value.Shader == shader)
				{
					return this.graphics[i].Value;
				}
			}
			Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(this.graphicPath, shader, Vector2.one, color);
			this.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
			return graphic_Multi;
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x0004C08F File Offset: 0x0004A28F
		public virtual Texture2D Icon
		{
			get
			{
				return ContentFinder<Texture2D>.Get(this.graphicPath + "_south", true);
			}
		}

		// Token: 0x04000CC2 RID: 3266
		public string graphicPath;

		// Token: 0x04000CC3 RID: 3267
		public Gender gender;

		// Token: 0x04000CC4 RID: 3268
		public bool narrow;

		// Token: 0x04000CC5 RID: 3269
		public Vector2 hairMeshSize = new Vector2(1.5f, 1.5f);

		// Token: 0x04000CC6 RID: 3270
		public Vector2 beardMeshSize = new Vector2(1.5f, 1.5f);

		// Token: 0x04000CC7 RID: 3271
		public Vector3 beardOffset;

		// Token: 0x04000CC8 RID: 3272
		public Vector3? eyeOffsetEastWest;

		// Token: 0x04000CC9 RID: 3273
		public float beardOffsetXEast;

		// Token: 0x04000CCA RID: 3274
		public float selectionWeight = 1f;

		// Token: 0x04000CCB RID: 3275
		public bool randomChosen = true;

		// Token: 0x04000CCC RID: 3276
		public List<GeneDef> requiredGenes;

		// Token: 0x04000CCD RID: 3277
		[Unsaved(false)]
		private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();
	}
}
