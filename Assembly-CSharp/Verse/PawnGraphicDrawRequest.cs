using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005F1 RID: 1521
	public struct PawnGraphicDrawRequest
	{
		// Token: 0x060026EE RID: 9966 RVA: 0x000CE764 File Offset: 0x000CC964
		public PawnGraphicDrawRequest(PawnRenderNode node, Mesh mesh = null, Material material = null)
		{
			this.node = node;
			this.mesh = mesh;
			this.material = material;
			this.preDrawnComputedMatrix = default(Matrix4x4);
		}

		// Token: 0x04002054 RID: 8276
		public readonly PawnRenderNode node;

		// Token: 0x04002055 RID: 8277
		public readonly Mesh mesh;

		// Token: 0x04002056 RID: 8278
		public Material material;

		// Token: 0x04002057 RID: 8279
		public Matrix4x4 preDrawnComputedMatrix;
	}
}
