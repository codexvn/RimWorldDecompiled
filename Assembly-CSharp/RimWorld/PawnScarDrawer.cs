using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E0 RID: 8928
	public abstract class PawnScarDrawer : PawnOverlayDrawer
	{
		// Token: 0x17001DB3 RID: 7603
		// (get) Token: 0x0600C21A RID: 49690
		protected abstract string ScarTexturePath { get; }

		// Token: 0x17001DB4 RID: 7604
		// (get) Token: 0x0600C21B RID: 49691 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected virtual float ScaleFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600C21C RID: 49692 RVA: 0x00383429 File Offset: 0x00381629
		public PawnScarDrawer(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600C21D RID: 49693 RVA: 0x00383BF8 File Offset: 0x00381DF8
		protected override void WriteCache(PawnOverlayDrawer.CacheKey key, PawnDrawParms parms, List<PawnOverlayDrawer.DrawCall> writeTarget)
		{
			Rot4 pawnRot = key.pawnRot;
			Mesh bodyMesh = key.bodyMesh;
			PawnOverlayDrawer.OverlayLayer layer = key.layer;
			Graphic graphic = (layer == PawnOverlayDrawer.OverlayLayer.Body) ? this.pawn.Drawer.renderer.BodyGraphic : this.pawn.Drawer.renderer.HeadGraphic;
			if (graphic == null)
			{
				return;
			}
			Rand.PushState(this.pawn.thingIDNumber * (int)(layer + 1));
			try
			{
				Mesh mesh = ((graphic.EastFlipped && pawnRot == Rot4.East) || (graphic.WestFlipped && pawnRot == Rot4.West)) ? MeshPool.GridPlaneFlip(Vector2.one) : MeshPool.GridPlane(Vector2.one);
				Vector3 size = bodyMesh.bounds.size;
				float d = size.magnitude * this.ScaleFactor;
				Vector3 vector = mesh.bounds.size * d;
				Vector4 value = new Vector4(vector.x / size.x, vector.z / size.z);
				Material material = MaterialPool.MatFrom(this.ScarTexturePath, ShaderDatabase.Wound, Color.white);
				material = MaterialPool.MatFrom(new MaterialRequest
				{
					maskTex = (Texture2D)graphic.MatAt(pawnRot, null).mainTexture,
					mainTex = material.mainTexture,
					color = material.color,
					shader = material.shader
				});
				Vector3 vector2 = Rand.InsideUnitCircleVec3 / 2f;
				int rotation = Rand.Range(0, 360);
				writeTarget.Add(new PawnOverlayDrawer.DrawCall
				{
					overlayMat = material,
					matrix = Matrix4x4.Scale(size),
					overlayMesh = mesh,
					mainTexScale = new Vector4?(value),
					mainTexOffset = new Vector4?(new Vector4(vector2.x, vector2.z)),
					rotation = rotation
				});
			}
			finally
			{
				Rand.PopState();
			}
		}
	}
}
