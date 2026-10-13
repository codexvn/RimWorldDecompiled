using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000619 RID: 1561
	public class PawnRenderNode_Hair : PawnRenderNode
	{
		// Token: 0x060027D9 RID: 10201 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Hair(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000D3A44 File Offset: 0x000D1C44
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.hairDef : null) == null || pawn.story.hairDef.noGraphic)
			{
				return null;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x000D3A80 File Offset: 0x000D1C80
		public override Graphic GraphicFor(Pawn pawn)
		{
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.hairDef : null) == null || pawn.story.hairDef.noGraphic || pawn.DevelopmentalStage.Baby() || pawn.DevelopmentalStage.Newborn())
			{
				return null;
			}
			return pawn.story.hairDef.GraphicFor(pawn, this.ColorFor(pawn));
		}
	}
}
