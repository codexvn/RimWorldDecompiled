using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B6 RID: 2230
	public abstract class Graphic_WithPropertyBlockRandom : Graphic_Random
	{
		// Token: 0x06003868 RID: 14440 RVA: 0x0012CA84 File Offset: 0x0012AC84
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x040029C4 RID: 10692
		protected MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
