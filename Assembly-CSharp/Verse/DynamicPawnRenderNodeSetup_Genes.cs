using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x0200060B RID: 1547
	public class DynamicPawnRenderNodeSetup_Genes : DynamicPawnRenderNodeSetup
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HumanlikeOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000D2B9B File Offset: 0x000D0D9B
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				yield break;
			}
			foreach (Gene g in pawn.genes.GenesListForReading)
			{
				if (g.Active && g.def.HasDefinedGraphicProperties)
				{
					foreach (PawnRenderNodeProperties pawnRenderNodeProperties in g.def.RenderNodeProperties)
					{
						if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties))
						{
							PawnRenderNode pawnRenderNode = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
							{
								pawn,
								pawnRenderNodeProperties,
								tree
							});
							pawnRenderNode.gene = g;
							yield return new ValueTuple<PawnRenderNode, PawnRenderNode>(pawnRenderNode, null);
						}
					}
					List<PawnRenderNodeProperties>.Enumerator enumerator2 = default(List<PawnRenderNodeProperties>.Enumerator);
					g = null;
				}
			}
			List<Gene>.Enumerator enumerator = default(List<Gene>.Enumerator);
			yield break;
			yield break;
		}
	}
}
