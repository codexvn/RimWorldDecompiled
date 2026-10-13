using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B01 RID: 6913
	public class TattooDef : StyleItemDef
	{
		// Token: 0x06009A65 RID: 39525 RVA: 0x002CE430 File Offset: 0x002CC630
		public override Graphic GraphicFor(Pawn pawn, Color color)
		{
			if (this.noGraphic)
			{
				return null;
			}
			string maskPath = (this.tattooType == TattooType.Body) ? pawn.story.bodyType.bodyNakedGraphicPath : pawn.story.headType.graphicPath;
			string texPath = this.texPath;
			ShaderTypeDef overrideShaderTypeDef = this.overrideShaderTypeDef;
			return GraphicDatabase.Get<Graphic_Multi>(texPath, ((overrideShaderTypeDef != null) ? overrideShaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutSkinOverlay, Vector2.one, color, Color.white, null, maskPath);
		}

		// Token: 0x04006A60 RID: 27232
		public TattooType tattooType;

		// Token: 0x04006A61 RID: 27233
		public bool visibleNorth = true;
	}
}
