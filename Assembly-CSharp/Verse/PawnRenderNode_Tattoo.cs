using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000620 RID: 1568
	public abstract class PawnRenderNode_Tattoo : PawnRenderNode
	{
		// Token: 0x060027ED RID: 10221 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Tattoo(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000D3E84 File Offset: 0x000D2084
		public override Color ColorFor(Pawn pawn)
		{
			Color result = base.ColorFor(pawn);
			result.a *= 0.8f;
			return result;
		}

		// Token: 0x04002105 RID: 8453
		protected const float TattooOpacity = 0.8f;
	}
}
