using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000611 RID: 1553
	public class DynamicPawnRenderNodeSetup_Traits : DynamicPawnRenderNodeSetup
	{
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HumanlikeOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000D3383 File Offset: 0x000D1583
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.traits : null) == null)
			{
				yield break;
			}
			foreach (Trait t in pawn.story.traits.allTraits)
			{
				if (!t.Suppressed && t.CurrentData.HasDefinedGraphicProperties)
				{
					foreach (PawnRenderNodeProperties pawnRenderNodeProperties in t.CurrentData.RenderNodeProperties)
					{
						if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties))
						{
							PawnRenderNode pawnRenderNode = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
							{
								pawn,
								pawnRenderNodeProperties,
								tree
							});
							pawnRenderNode.trait = t;
							yield return new ValueTuple<PawnRenderNode, PawnRenderNode>(pawnRenderNode, null);
						}
					}
					List<PawnRenderNodeProperties>.Enumerator enumerator2 = default(List<PawnRenderNodeProperties>.Enumerator);
					t = null;
				}
			}
			List<Trait>.Enumerator enumerator = default(List<Trait>.Enumerator);
			yield break;
			yield break;
		}
	}
}
