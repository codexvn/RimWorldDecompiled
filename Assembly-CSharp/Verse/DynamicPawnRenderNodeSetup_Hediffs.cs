using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x0200060D RID: 1549
	public class DynamicPawnRenderNodeSetup_Hediffs : DynamicPawnRenderNodeSetup
	{
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x00002501 File Offset: 0x00000701
		public override bool HumanlikeOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000D2E77 File Offset: 0x000D1077
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			Pawn_HealthTracker health = pawn.health;
			List<Hediff> list;
			if (health == null)
			{
				list = null;
			}
			else
			{
				HediffSet hediffSet = health.hediffSet;
				list = ((hediffSet != null) ? hediffSet.hediffs : null);
			}
			List<Hediff> list2 = list;
			if (list2 == null)
			{
				yield break;
			}
			foreach (Hediff h in list2)
			{
				if (h.Visible && h.def.HasDefinedGraphicProperties)
				{
					foreach (PawnRenderNodeProperties pawnRenderNodeProperties in h.def.RenderNodeProperties)
					{
						if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties))
						{
							PawnRenderNode pawnRenderNode = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
							{
								pawn,
								pawnRenderNodeProperties,
								tree
							});
							pawnRenderNode.hediff = h;
							pawnRenderNode.bodyPart = h.Part;
							yield return new ValueTuple<PawnRenderNode, PawnRenderNode>(pawnRenderNode, null);
						}
					}
					List<PawnRenderNodeProperties>.Enumerator enumerator2 = default(List<PawnRenderNodeProperties>.Enumerator);
					h = null;
				}
			}
			List<Hediff>.Enumerator enumerator = default(List<Hediff>.Enumerator);
			yield break;
			yield break;
		}
	}
}
