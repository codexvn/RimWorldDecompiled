using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B03 RID: 6915
	public class FurDef : StyleItemDef
	{
		// Token: 0x06009A69 RID: 39529 RVA: 0x002CE4EC File Offset: 0x002CC6EC
		public override Graphic GraphicFor(Pawn pawn, Color color)
		{
			if (this.noGraphic)
			{
				return null;
			}
			string furBodyGraphicPath = this.GetFurBodyGraphicPath(pawn);
			ShaderTypeDef overrideShaderTypeDef = this.overrideShaderTypeDef;
			return GraphicDatabase.Get<Graphic_Multi>(furBodyGraphicPath, ((overrideShaderTypeDef != null) ? overrideShaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutSkinOverlay, Vector2.one, color);
		}

		// Token: 0x06009A6A RID: 39530 RVA: 0x002CE528 File Offset: 0x002CC728
		public string GetFurBodyGraphicPath(Pawn pawn)
		{
			for (int i = 0; i < this.bodyTypeGraphicPaths.Count; i++)
			{
				if (this.bodyTypeGraphicPaths[i].bodyType == pawn.story.bodyType)
				{
					return this.bodyTypeGraphicPaths[i].texturePath;
				}
			}
			return null;
		}

		// Token: 0x04006A64 RID: 27236
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths;
	}
}
