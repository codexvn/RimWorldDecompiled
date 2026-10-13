using System;

namespace Verse
{
	// Token: 0x02000618 RID: 1560
	public class PawnRenderNode_Stump : PawnRenderNode
	{
		// Token: 0x060027D6 RID: 10198 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Stump(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000D3A15 File Offset: 0x000D1C15
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000D3A27 File Offset: 0x000D1C27
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!pawn.health.hediffSet.HasHead)
			{
				return base.GraphicFor(pawn);
			}
			return null;
		}
	}
}
