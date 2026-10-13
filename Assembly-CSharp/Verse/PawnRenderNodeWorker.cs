using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064F RID: 1615
	public class PawnRenderNodeWorker
	{
		// Token: 0x0600289E RID: 10398 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return true;
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000D687C File Offset: 0x000D4A7C
		public virtual bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			if (!node.Props.rotDrawMode.HasFlag(parms.rotDrawMode))
			{
				return false;
			}
			if (node.Props.visibleFacing != null && !node.Props.visibleFacing.Contains(parms.facing))
			{
				return false;
			}
			if (node.Props.skipFlag != RenderSkipFlagDefOf.None && parms.skipFlags.HasFlag(node.Props.skipFlag))
			{
				return false;
			}
			BodyPartRecord bodyPart = node.bodyPart;
			if (((bodyPart != null) ? bodyPart.visibleHediffRots : null) != null && !node.bodyPart.visibleHediffRots.Contains(parms.facing))
			{
				return false;
			}
			Rot4 rot = Rot4.Invalid;
			if (node.Props.side != PawnRenderNodeProperties.Side.Center)
			{
				rot = ((node.Props.side == PawnRenderNodeProperties.Side.Left) ? Rot4.East : Rot4.West);
			}
			if (node.bodyPart != null && node.bodyPart.def.IsMirroredPart)
			{
				rot = (node.bodyPart.flipGraphic ? Rot4.East : Rot4.West);
			}
			if (rot != Rot4.Invalid && node.Props.flipGraphic && rot.IsHorizontal)
			{
				rot = rot.Opposite;
			}
			return !(parms.facing == rot) && (node.Props.linkedBodyPartsGroup == null || parms.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.groups.NotNullAndContains(node.Props.linkedBodyPartsGroup))) && node.DebugEnabled;
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000D6A64 File Offset: 0x000D4C64
		public virtual void PostDraw(PawnRenderNode node, PawnDrawParms parms, Mesh mesh, Matrix4x4 matrix)
		{
			node.TryAnimationPostDraw(parms, matrix);
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000D6A70 File Offset: 0x000D4C70
		public virtual Material GetFinalizedMaterial(PawnRenderNode node, PawnDrawParms parms)
		{
			Material material = this.GetMaterial(node, parms);
			foreach (PawnRenderSubWorker pawnRenderSubWorker in node.Props.SubWorkers)
			{
				pawnRenderSubWorker.EditMaterial(node, parms, ref material);
			}
			return material;
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000D6AD4 File Offset: 0x000D4CD4
		public virtual void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
		{
			Material finalizedMaterial = this.GetFinalizedMaterial(node, parms);
			if (finalizedMaterial == null)
			{
				return;
			}
			Mesh mesh = node.GetMesh(parms);
			if (mesh == null)
			{
				return;
			}
			requests.Add(new PawnGraphicDrawRequest(node, mesh, finalizedMaterial));
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000D6B14 File Offset: 0x000D4D14
		protected virtual Graphic GetGraphic(PawnRenderNode node, PawnDrawParms parms)
		{
			GraphicStateDef graphicState = this.GetGraphicState(node, parms);
			if (graphicState == null)
			{
				return node.PrimaryGraphic;
			}
			return node.GraphicForState(graphicState);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000D6B3C File Offset: 0x000D4D3C
		protected virtual GraphicStateDef GetGraphicState(PawnRenderNode node, PawnDrawParms parms)
		{
			GraphicStateDef result;
			if (!parms.flags.FlagSet(PawnRenderFlags.Portrait) && node.TryGetAnimationGraphicState(parms, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000D6B68 File Offset: 0x000D4D68
		public virtual MaterialPropertyBlock GetMaterialPropertyBlock(PawnRenderNode node, Material material, PawnDrawParms parms)
		{
			if (this.GetGraphic(node, parms) == null)
			{
				return null;
			}
			MaterialPropertyBlock matPropBlock = node.MatPropBlock;
			if (parms.Statue)
			{
				matPropBlock.SetColor(ShaderPropertyIDs.Color, parms.statueColor.Value);
			}
			else
			{
				matPropBlock.SetColor(ShaderPropertyIDs.Color, parms.tint * material.color);
			}
			if (material.shader == ShaderDatabase.CutoutWithOverlay)
			{
				if (parms.pawn.Faction != null && material.GetMaskTexture() != null)
				{
					PawnRenderUtility.SetMatPropBlockOverlay(matPropBlock, parms.pawn.Faction.AllegianceColor, 0.5f);
				}
				else
				{
					PawnRenderUtility.SetMatPropBlockOverlay(matPropBlock, Color.white, 0f);
				}
			}
			return matPropBlock;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000D6C24 File Offset: 0x000D4E24
		protected virtual Material GetMaterial(PawnRenderNode node, PawnDrawParms parms)
		{
			Graphic graphic = this.GetGraphic(node, parms);
			if (graphic == null)
			{
				return null;
			}
			if (node.Props.flipGraphic && parms.facing.IsHorizontal)
			{
				parms.facing = parms.facing.Opposite;
			}
			Material material = graphic.NodeGetMat(parms);
			if (material != null && !parms.Portrait && parms.flags.FlagSet(PawnRenderFlags.Invisible))
			{
				material = InvisibilityMatPool.GetInvisibleMat(material);
			}
			return material;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000D6C9D File Offset: 0x000D4E9D
		public virtual void PreDraw(PawnRenderNode node, Material mat, PawnDrawParms parms)
		{
			node.MatPropBlock.SetColor(ShaderPropertyIDs.Color, parms.tint * mat.color);
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000D6CC0 File Offset: 0x000D4EC0
		public virtual Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = Vector3.zero;
			pivot = this.PivotFor(node, parms);
			if (node.Props.drawData != null)
			{
				if (node.Props.drawData.useBodyPartAnchor)
				{
					if (node.bodyPart == null)
					{
						Log.ErrorOnce(string.Format("Attempted to use a body part anchor but no body-part record has been assigned to this node {0}", node), node.GetHashCode());
						return vector;
					}
					foreach (BodyTypeDef.WoundAnchor anchor in PawnDrawUtility.FindAnchors(parms.pawn, node.bodyPart))
					{
						if (PawnDrawUtility.AnchorUsable(parms.pawn, anchor, parms.facing))
						{
							float num;
							PawnDrawUtility.CalcAnchorData(parms.pawn, anchor, parms.facing, out vector, out num);
						}
					}
				}
				Vector3 vector2 = node.Props.drawData.OffsetForRot(parms.facing);
				if (node.Props.drawData.scaleOffsetByBodySize && parms.pawn.story != null)
				{
					Vector2 bodyGraphicScale = parms.pawn.story.bodyType.bodyGraphicScale;
					float d = (bodyGraphicScale.x + bodyGraphicScale.y) / 2f;
					vector2 *= d;
				}
				vector += vector2;
			}
			vector += node.DebugOffset;
			Vector3 b;
			if (!parms.flags.FlagSet(PawnRenderFlags.Portrait) && node.TryGetAnimationOffset(parms, out b))
			{
				vector += b;
			}
			return vector;
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000D6E3C File Offset: 0x000D503C
		protected virtual Vector3 PivotFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Vector3 vector = Vector3.zero;
			if (node.Props.drawData != null)
			{
				vector -= (node.Props.drawData.PivotForRot(parms.facing) - DrawData.PivotCenter).ToVector3();
			}
			AnimationPart animationPart;
			if (node.tree.TryGetAnimationPartForNode(node, out animationPart))
			{
				vector = (animationPart.pivot - DrawData.PivotCenter).ToVector3();
			}
			if (node.debugPivotOffset != DrawData.PivotCenter)
			{
				vector.x += node.debugPivotOffset.x - DrawData.PivotCenter.x;
				vector.z += node.debugPivotOffset.y - DrawData.PivotCenter.y;
			}
			return vector;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000D6F04 File Offset: 0x000D5104
		public float AltitudeFor(PawnRenderNode node, PawnDrawParms parms)
		{
			float layer = this.LayerFor(node, parms);
			foreach (PawnRenderSubWorker pawnRenderSubWorker in node.Props.SubWorkers)
			{
				pawnRenderSubWorker.TransformLayer(node, parms, ref layer);
			}
			return PawnRenderUtility.AltitudeForLayer(layer);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000D6F6C File Offset: 0x000D516C
		public virtual float LayerFor(PawnRenderNode node, PawnDrawParms parms)
		{
			DrawData drawData = node.Props.drawData;
			return ((drawData != null) ? drawData.LayerForRot(parms.facing, node.Props.baseLayer) : node.Props.baseLayer) + node.debugLayerOffset;
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000D6FA8 File Offset: 0x000D51A8
		public virtual Quaternion RotationFor(PawnRenderNode node, PawnDrawParms parms)
		{
			float num = node.DebugAngleOffset;
			if (node.Props.drawData != null)
			{
				num += node.Props.drawData.RotationOffsetForRot(parms.facing);
			}
			float num2;
			if (!parms.flags.FlagSet(PawnRenderFlags.Portrait) && node.TryGetAnimationRotation(parms, out num2))
			{
				num += num2;
			}
			if (node.FlipGraphic(parms))
			{
				num *= -1f;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000D701C File Offset: 0x000D521C
		public virtual Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Vector3 vector = Vector3.one;
			vector.x *= node.Props.drawSize.x * node.debugScale;
			vector.z *= node.Props.drawSize.y * node.debugScale;
			if (!parms.flags.FlagSet(PawnRenderFlags.Portrait))
			{
				Vector3 b;
				if (node.TryGetAnimationScale(parms, out b))
				{
					vector = vector.ScaledBy(b);
				}
				GraphicStateDef graphicState = this.GetGraphicState(node, parms);
				Graphic graphic;
				if (graphicState != null && graphicState.TryGetDefaultGraphic(out graphic))
				{
					vector = vector.ScaledBy(new Vector3(graphic.drawSize.x, 1f, graphic.drawSize.y));
				}
			}
			if (node.Props.drawData != null)
			{
				vector *= node.Props.drawData.ScaleFor(parms.pawn);
			}
			return vector;
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000D70FC File Offset: 0x000D52FC
		public static List<PawnRenderSubWorker> GetSubWorkerList(List<Type> types)
		{
			if (types == null)
			{
				return PawnRenderNodeWorker.EmptySubWorkers;
			}
			List<PawnRenderSubWorker> list = new List<PawnRenderSubWorker>();
			foreach (Type type in types)
			{
				list.Add(GenWorker<PawnRenderSubWorker>.Get(type));
			}
			return list;
		}

		// Token: 0x04002179 RID: 8569
		private static readonly List<PawnRenderSubWorker> EmptySubWorkers = new List<PawnRenderSubWorker>();
	}
}
