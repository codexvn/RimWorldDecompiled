using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B5 RID: 2229
	public abstract class Graphic_WithPropertyBlock : Graphic_Single
	{
		// Token: 0x06003866 RID: 14438 RVA: 0x0012CA33 File Offset: 0x0012AC33
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x040029C3 RID: 10691
		public MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
