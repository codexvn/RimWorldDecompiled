using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200061C RID: 1564
	public class PawnRenderNode_Tattoo_Head : PawnRenderNode_Tattoo
	{
		// Token: 0x060027E1 RID: 10209 RVA: 0x000D21ED File Offset: 0x000D03ED
		public PawnRenderNode_Tattoo_Head(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000D3B81 File Offset: 0x000D1D81
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.FaceTattoo : null) == null || pawn.style.FaceTattoo.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000D3BBC File Offset: 0x000D1DBC
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!ModLister.CheckIdeology("Head tattoo"))
			{
				return null;
			}
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.FaceTattoo : null) == null || pawn.style.FaceTattoo.noGraphic)
			{
				return null;
			}
			return pawn.style.FaceTattoo.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
