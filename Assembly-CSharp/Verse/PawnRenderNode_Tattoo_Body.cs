using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000606 RID: 1542
	public class PawnRenderNode_Tattoo_Body : PawnRenderNode_Tattoo
	{
		// Token: 0x0600276E RID: 10094 RVA: 0x000D21ED File Offset: 0x000D03ED
		public PawnRenderNode_Tattoo_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000D21F8 File Offset: 0x000D03F8
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.BodyTattoo : null) == null || pawn.style.BodyTattoo.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000D2234 File Offset: 0x000D0434
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!ModLister.CheckIdeology("Body tattoo"))
			{
				return null;
			}
			Pawn_StyleTracker style = pawn.style;
			if (((style != null) ? style.BodyTattoo : null) == null || pawn.style.BodyTattoo.noGraphic)
			{
				return null;
			}
			return pawn.style.BodyTattoo.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
