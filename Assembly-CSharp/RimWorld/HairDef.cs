using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFE RID: 6910
	public class HairDef : StyleItemDef
	{
		// Token: 0x06009A60 RID: 39520 RVA: 0x002CE37B File Offset: 0x002CC57B
		public override Graphic GraphicFor(Pawn pawn, Color color)
		{
			if (this.noGraphic)
			{
				return null;
			}
			string texPath = this.texPath;
			ShaderTypeDef overrideShaderTypeDef = this.overrideShaderTypeDef;
			return GraphicDatabase.Get<Graphic_Multi>(texPath, ((overrideShaderTypeDef != null) ? overrideShaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutHair, Vector2.one, color);
		}
	}
}
