using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000617 RID: 1559
	public class PawnRenderNode_Head : PawnRenderNode
	{
		// Token: 0x060027D3 RID: 10195 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Head(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000D394C File Offset: 0x000D1B4C
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			if (this.props.overrideMeshSize != null)
			{
				return MeshPool.GetMeshSetForSize(this.props.overrideMeshSize.Value.x, this.props.overrideMeshSize.Value.y);
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000D39AC File Offset: 0x000D1BAC
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!pawn.health.hediffSet.HasHead)
			{
				return null;
			}
			if (pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
			{
				return HeadTypeDefOf.Skull.GetGraphic(pawn, Color.white);
			}
			Pawn_StoryTracker story = pawn.story;
			if (story == null)
			{
				return null;
			}
			HeadTypeDef headType = story.headType;
			if (headType == null)
			{
				return null;
			}
			return headType.GetGraphic(pawn, this.ColorFor(pawn));
		}
	}
}
