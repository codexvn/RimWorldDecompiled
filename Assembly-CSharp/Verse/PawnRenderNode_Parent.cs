using System;

namespace Verse
{
	// Token: 0x0200061D RID: 1565
	public class PawnRenderNode_Parent : PawnRenderNode
	{
		// Token: 0x060027E4 RID: 10212 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Parent(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00002C42 File Offset: 0x00000E42
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return null;
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00002C42 File Offset: 0x00000E42
		public override Graphic GraphicFor(Pawn pawn)
		{
			return null;
		}
	}
}
