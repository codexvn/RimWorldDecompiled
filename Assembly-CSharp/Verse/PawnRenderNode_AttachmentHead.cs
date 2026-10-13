using System;

namespace Verse
{
	// Token: 0x0200061A RID: 1562
	public class PawnRenderNode_AttachmentHead : PawnRenderNode
	{
		// Token: 0x060027DC RID: 10204 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_AttachmentHead(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000D3AE7 File Offset: 0x000D1CE7
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(pawn, 1f, 1f);
		}
	}
}
