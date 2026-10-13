using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200061B RID: 1563
	public class PawnRenderNode_Beard : PawnRenderNode
	{
		// Token: 0x060027DE RID: 10206 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Beard(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000D3AF9 File Offset: 0x000D1CF9
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.beardDef : null) == null || pawn.style.beardDef.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBeardSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000D3B34 File Offset: 0x000D1D34
		public override Graphic GraphicFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.beardDef : null) == null || pawn.style.beardDef.noGraphic)
			{
				return null;
			}
			return pawn.style.beardDef.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
