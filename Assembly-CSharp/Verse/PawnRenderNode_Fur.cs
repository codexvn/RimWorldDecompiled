using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000604 RID: 1540
	public class PawnRenderNode_Fur : PawnRenderNode
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x000D20F1 File Offset: 0x000D02F1
		protected override Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.CutoutSkinOverlay;
			}
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Fur(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000D20F8 File Offset: 0x000D02F8
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!ModLister.CheckBiotech("Fur"))
			{
				return null;
			}
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.furDef : null) == null)
			{
				return null;
			}
			Pawn_StoryTracker story2 = pawn.story;
			return GraphicDatabase.Get<Graphic_Multi>((story2 != null) ? story2.furDef.GetFurBodyGraphicPath(pawn) : null, base.ShaderFor(pawn), Vector2.one, this.ColorFor(pawn));
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000D2159 File Offset: 0x000D0359
		public override Color ColorFor(Pawn pawn)
		{
			return pawn.story.HairColor;
		}
	}
}
