using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000608 RID: 1544
	public class DynamicPawnRenderNodeSetup_Apparel : DynamicPawnRenderNodeSetup
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002775 RID: 10101 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HumanlikeOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000D2297 File Offset: 0x000D0497
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			if (pawn.apparel == null || pawn.apparel.WornApparelCount == 0)
			{
				yield break;
			}
			Dictionary<PawnRenderNode, int> layerOffsets = new Dictionary<PawnRenderNode, int>();
			PawnRenderNode pawnRenderNode;
			PawnRenderNode headApparelNode = tree.TryGetNodeByTag(PawnRenderNodeTagDefOf.ApparelHead, out pawnRenderNode) ? pawnRenderNode : null;
			PawnRenderNode pawnRenderNode2;
			PawnRenderNode bodyApparelNode = tree.TryGetNodeByTag(PawnRenderNodeTagDefOf.ApparelBody, out pawnRenderNode2) ? pawnRenderNode2 : null;
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				if (DynamicPawnRenderNodeSetup_Apparel.ShouldAddApparelNode(apparel))
				{
					foreach (ValueTuple<PawnRenderNode, PawnRenderNode> result in DynamicPawnRenderNodeSetup_Apparel.ProcessApparel(pawn, tree, apparel, headApparelNode, bodyApparelNode, layerOffsets))
					{
						if (result.Item1 != null)
						{
							yield return result;
						}
						if (result.Item2 != null && !layerOffsets.TryAdd(result.Item2, 1))
						{
							Dictionary<PawnRenderNode, int> dictionary = layerOffsets;
							PawnRenderNode item = result.Item2;
							int num = dictionary[item];
							dictionary[item] = num + 1;
						}
						result = default(ValueTuple<PawnRenderNode, PawnRenderNode>);
					}
					IEnumerator<ValueTuple<PawnRenderNode, PawnRenderNode>> enumerator2 = null;
				}
			}
			List<Apparel>.Enumerator enumerator = default(List<Apparel>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000D22AE File Offset: 0x000D04AE
		private static bool ShouldAddApparelNode(Apparel gear)
		{
			return !gear.def.IsWeapon;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000D22BE File Offset: 0x000D04BE
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		private static IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> ProcessApparel(Pawn pawn, PawnRenderTree tree, Apparel ap, PawnRenderNode headApparelNode, PawnRenderNode bodyApparelNode, Dictionary<PawnRenderNode, int> layerOffsets)
		{
			if (ap.def.apparel.HasDefinedGraphicProperties)
			{
				foreach (PawnRenderNodeProperties pawnRenderNodeProperties in ap.def.apparel.RenderNodeProperties)
				{
					if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties))
					{
						PawnRenderNode pawnRenderNode = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
						{
							pawn,
							pawnRenderNodeProperties,
							tree
						});
						pawnRenderNode.apparel = ap;
						yield return new ValueTuple<PawnRenderNode, PawnRenderNode>(pawnRenderNode, null);
					}
				}
				List<PawnRenderNodeProperties>.Enumerator enumerator = default(List<PawnRenderNodeProperties>.Enumerator);
			}
			ApparelGraphicRecord apparelGraphicRecord;
			if (!ApparelGraphicRecordGetter.TryGetGraphicApparel(ap, pawn.story.bodyType, pawn.Drawer.renderer.StatueColor != null, out apparelGraphicRecord))
			{
				yield break;
			}
			PawnRenderNodeProperties pawnRenderNodeProperties2 = null;
			PawnRenderNode pawnRenderNode2 = null;
			DrawData drawData = ap.def.apparel.drawData;
			ApparelLayerDef lastLayer = ap.def.apparel.LastLayer;
			bool flag = lastLayer == ApparelLayerDefOf.Overhead || lastLayer == ApparelLayerDefOf.EyeCover;
			PawnRenderNode pawnRenderNode3;
			if (ap.def.apparel.parentTagDef != null && tree.TryGetNodeByTag(ap.def.apparel.parentTagDef, out pawnRenderNode3))
			{
				pawnRenderNode2 = pawnRenderNode3;
				if (headApparelNode != null && pawnRenderNode2 == headApparelNode)
				{
					flag = true;
				}
				else if (bodyApparelNode != null && pawnRenderNode2 == bodyApparelNode)
				{
					flag = false;
				}
			}
			if (headApparelNode != null && flag)
			{
				if (pawnRenderNode2 == null)
				{
					pawnRenderNode2 = headApparelNode;
				}
				int valueOrDefault = layerOffsets.GetValueOrDefault(pawnRenderNode2, 0);
				pawnRenderNodeProperties2 = new PawnRenderNodeProperties
				{
					debugLabel = ap.def.defName,
					workerClass = typeof(PawnRenderNodeWorker_Apparel_Head),
					baseLayer = pawnRenderNode2.Props.baseLayer + (float)valueOrDefault,
					drawData = drawData,
					parentTagDef = ap.def.apparel.parentTagDef
				};
			}
			else if (bodyApparelNode != null)
			{
				if (pawnRenderNode2 == null)
				{
					pawnRenderNode2 = bodyApparelNode;
				}
				int valueOrDefault2 = layerOffsets.GetValueOrDefault(pawnRenderNode2, 0);
				pawnRenderNodeProperties2 = new PawnRenderNodeProperties
				{
					debugLabel = ap.def.defName,
					workerClass = typeof(PawnRenderNodeWorker_Apparel_Body),
					baseLayer = pawnRenderNode2.Props.baseLayer + (float)valueOrDefault2,
					drawData = drawData,
					parentTagDef = ap.def.apparel.parentTagDef
				};
				if (drawData == null && !ap.def.apparel.shellRenderedBehindHead)
				{
					if (lastLayer == ApparelLayerDefOf.Shell)
					{
						pawnRenderNodeProperties2.drawData = DrawData.NewWithData(new DrawData.RotationalData[]
						{
							new DrawData.RotationalData(new Rot4?(Rot4.North), 88f)
						});
					}
					else if (ap.RenderAsPack())
					{
						pawnRenderNodeProperties2.drawData = DrawData.NewWithData(new DrawData.RotationalData[]
						{
							new DrawData.RotationalData(new Rot4?(Rot4.North), 93f),
							new DrawData.RotationalData(new Rot4?(Rot4.South), -3f)
						});
					}
				}
			}
			if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties2))
			{
				yield return new ValueTuple<PawnRenderNode, PawnRenderNode>(new PawnRenderNode_Apparel(pawn, pawnRenderNodeProperties2, tree, ap), pawnRenderNode2);
			}
			else
			{
				yield return new ValueTuple<PawnRenderNode, PawnRenderNode>(null, pawnRenderNode2);
			}
			yield break;
			yield break;
		}

		// Token: 0x040020B0 RID: 8368
		private const int ApparelLayerShellNorth = 88;
	}
}
