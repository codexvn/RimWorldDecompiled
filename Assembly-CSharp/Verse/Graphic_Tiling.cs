using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B4 RID: 2228
	public class Graphic_Tiling : Graphic_WithPropertyBlock
	{
		// Token: 0x06003863 RID: 14435 RVA: 0x0012C9FF File Offset: 0x0012ABFF
		public Graphic_Tiling WithTiling(Vector2 tiling)
		{
			this.Tiling = tiling;
			return this;
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0012CA09 File Offset: 0x0012AC09
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.propertyBlock.SetVector(ShaderPropertyIDs.Tiling, this.Tiling);
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x040029C2 RID: 10690
		public Vector2 Tiling;
	}
}
