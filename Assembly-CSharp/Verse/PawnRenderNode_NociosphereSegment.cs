using System;

namespace Verse
{
	// Token: 0x02000616 RID: 1558
	public class PawnRenderNode_NociosphereSegment : PawnRenderNode
	{
		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060027D0 RID: 10192 RVA: 0x000D391A File Offset: 0x000D1B1A
		public new PawnRenderNodeProperties_NociosphereSegment Props
		{
			get
			{
				return (PawnRenderNodeProperties_NociosphereSegment)this.props;
			}
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_NociosphereSegment(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000D3927 File Offset: 0x000D1B27
		public override Graphic GraphicFor(Pawn pawn)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.Props.texPath, ShaderDatabase.CutoutComplexBlend, this.Props.maskPath);
		}
	}
}
