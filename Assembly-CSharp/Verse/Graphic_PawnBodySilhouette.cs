using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008AB RID: 2219
	public class Graphic_PawnBodySilhouette : Graphic_Mote
	{
		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06003824 RID: 14372 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x0012BB3C File Offset: 0x00129D3C
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			this.request = req;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x0012BB98 File Offset: 0x00129D98
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			Color color = this.color;
			color.a *= mote.Alpha;
			Corpse corpse = mote.link1.Target.Thing as Corpse;
			Pawn pawn = mote.link1.Target.Thing as Pawn;
			Pawn pawn2 = ((corpse != null) ? corpse.InnerPawn : null) ?? pawn;
			if (pawn2 == null)
			{
				pawn2 = this.lastPawn;
			}
			PawnRenderer pawnRenderer;
			if (pawn2 == null)
			{
				pawnRenderer = null;
			}
			else
			{
				Pawn_DrawTracker drawer = pawn2.Drawer;
				pawnRenderer = ((drawer != null) ? drawer.renderer : null);
			}
			PawnRenderer pawnRenderer2 = pawnRenderer;
			if (((pawnRenderer2 != null) ? pawnRenderer2.renderTree : null) == null || !pawnRenderer2.renderTree.Resolved)
			{
				return;
			}
			Rot4 rot2 = (pawn2.GetPosture() == PawnPosture.Standing) ? pawn2.Rotation : pawnRenderer2.LayingFacing();
			Vector3 vector = pawn2.DrawPos;
			Building_Bed building_Bed = pawn2.CurrentBed();
			if (building_Bed != null)
			{
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				vector -= rotation.FacingCell.ToVector3() * (pawn2.story.bodyType.bedOffset + pawn2.Drawer.renderer.BaseHeadOffsetAt(Rot4.South).z);
			}
			bool posture = pawn2.GetPosture() != PawnPosture.Standing;
			vector.y = mote.def.Altitude;
			if (this.lastPawn != pawn2 || this.lastFacing != rot2)
			{
				this.bodyMaterial = this.MakeMatFrom(this.request, pawnRenderer2.BodyGraphic.MatAt(rot2, null).mainTexture);
			}
			Mesh mesh;
			if (pawn2.RaceProps.Humanlike)
			{
				mesh = HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn2, 1f, 1f).MeshAt(rot2);
			}
			else
			{
				mesh = pawnRenderer2.BodyGraphic.MeshAt(rot2);
			}
			this.bodyMaterial.SetVector(ShaderPropertyIDs.PawnCenterWorld, new Vector4(vector.x, vector.z, 0f, 0f));
			this.bodyMaterial.SetVector(ShaderPropertyIDs.PawnDrawSizeWorld, new Vector4(mesh.bounds.size.x, mesh.bounds.size.z, 0f, 0f));
			this.bodyMaterial.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			this.bodyMaterial.SetColor(ShaderPropertyIDs.Color, color);
			Quaternion quaternion = Quaternion.AngleAxis((!posture) ? 0f : pawnRenderer2.BodyAngle(PawnRenderFlags.None), Vector3.up);
			if (building_Bed == null || building_Bed.def.building.bed_showSleeperBody)
			{
				GenDraw.DrawMeshNowOrLater(mesh, vector, quaternion, this.bodyMaterial, false);
			}
			if (pawn2.RaceProps.Humanlike)
			{
				bool flag = true;
				if (this.lastPawn != pawn2 || this.lastFacing != rot2)
				{
					if (pawnRenderer2.HeadGraphic == null)
					{
						flag = false;
						this.headMaterial = null;
					}
					else
					{
						this.headMaterial = this.MakeMatFrom(this.request, pawnRenderer2.HeadGraphic.MatAt(rot2, null).mainTexture);
					}
				}
				if (flag && this.headMaterial != null)
				{
					Vector3 b = quaternion * pawnRenderer2.BaseHeadOffsetAt(rot2) + new Vector3(0f, 0.001f, 0f);
					Mesh mesh2 = HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn2, 1f, 1f).MeshAt(rot2);
					this.headMaterial.SetVector(ShaderPropertyIDs.PawnCenterWorld, new Vector4(vector.x, vector.z, 0f, 0f));
					this.headMaterial.SetVector(ShaderPropertyIDs.PawnDrawSizeWorld, new Vector4(mesh2.bounds.size.x, mesh.bounds.size.z, 0f, 0f));
					this.headMaterial.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
					this.headMaterial.SetColor(ShaderPropertyIDs.Color, color);
					GenDraw.DrawMeshNowOrLater(mesh2, vector + b, quaternion, this.headMaterial, false);
				}
			}
			if (pawn2 != null)
			{
				this.lastPawn = pawn2;
			}
			this.lastFacing = rot2;
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x0012BFD8 File Offset: 0x0012A1D8
		private Material MakeMatFrom(GraphicRequest req, Texture mainTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest
			{
				mainTex = mainTex,
				shader = req.shader,
				color = this.color,
				colorTwo = this.colorTwo,
				renderQueue = req.renderQueue,
				shaderParameters = req.shaderParameters
			});
		}

		// Token: 0x040029B4 RID: 10676
		private GraphicRequest request;

		// Token: 0x040029B5 RID: 10677
		private Pawn lastPawn;

		// Token: 0x040029B6 RID: 10678
		private Rot4 lastFacing;

		// Token: 0x040029B7 RID: 10679
		private Material bodyMaterial;

		// Token: 0x040029B8 RID: 10680
		private Material headMaterial;
	}
}
