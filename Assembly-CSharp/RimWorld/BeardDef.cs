using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFF RID: 6911
	public class BeardDef : StyleItemDef
	{
		// Token: 0x06009A62 RID: 39522 RVA: 0x002CE3BC File Offset: 0x002CC5BC
		public Vector3 GetOffset(HeadTypeDef headType, Rot4 rot)
		{
			if (!headType.narrow || rot == Rot4.North)
			{
				return Vector3.zero;
			}
			if (rot == Rot4.South)
			{
				return this.offsetNarrowSouth;
			}
			if (rot == Rot4.East)
			{
				return this.offsetNarrowEast;
			}
			return new Vector3(-this.offsetNarrowEast.x, 0f, this.offsetNarrowEast.z);
		}

		// Token: 0x06009A63 RID: 39523 RVA: 0x002CE37B File Offset: 0x002CC57B
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

		// Token: 0x04006A5B RID: 27227
		public Vector3 offsetNarrowEast;

		// Token: 0x04006A5C RID: 27228
		public Vector3 offsetNarrowSouth;
	}
}
