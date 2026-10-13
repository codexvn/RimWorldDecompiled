using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x0200060F RID: 1551
	public class DynamicPawnRenderNodeSetup_Mutants : DynamicPawnRenderNodeSetup
	{
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x00002501 File Offset: 0x00000701
		public override bool HumanlikeOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000D316F File Offset: 0x000D136F
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			if (!pawn.IsMutant || !pawn.mutant.Def.HasDefinedGraphicProperties)
			{
				yield break;
			}
			foreach (PawnRenderNodeProperties pawnRenderNodeProperties in pawn.mutant.Def.RenderNodeProperties)
			{
				if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties))
				{
					yield return new ValueTuple<PawnRenderNode, PawnRenderNode>((PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
					{
						pawn,
						pawnRenderNodeProperties,
						tree
					}), null);
				}
			}
			List<PawnRenderNodeProperties>.Enumerator enumerator = default(List<PawnRenderNodeProperties>.Enumerator);
			yield break;
			yield break;
		}
	}
}
