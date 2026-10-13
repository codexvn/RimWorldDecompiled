using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005E5 RID: 1509
	public class AnimationWorker_RevenantHypnotise : AnimationWorker_Keyframes
	{
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x000CCCDB File Offset: 0x000CAEDB
		private Material LensFlareMat
		{
			get
			{
				if (this.lensFlareMat == null)
				{
					this.lensFlareMat = MaterialPool.MatFrom("Things/Pawn/Revenant/Revenant_LensFlare", ShaderDatabase.MoteGlow);
				}
				return this.lensFlareMat;
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000CCD08 File Offset: 0x000CAF08
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
			foreach (Vector3 a in ((parms.facing == Rot4.North || parms.facing == Rot4.South) ? AnimationWorker_RevenantHypnotise.EyePositionsPxNS : AnimationWorker_RevenantHypnotise.EyePositionsPxEW))
			{
				a.y = 1f;
				if (parms.facing == Rot4.North || parms.facing == Rot4.West)
				{
					a.x *= -1f;
				}
				if (parms.facing == Rot4.North)
				{
					a.y = -1f;
				}
				float x = (float)(Find.TickManager.TicksGame - node.tree.animationStartTick) % 30f / 30f;
				this.LensFlareMat.SetColor(ShaderPropertyIDs.Color, new Color(1f, 1f, 1f, AnimationWorker_RevenantHypnotise.LensFlareOpacity.Evaluate(x)));
				GenDraw.DrawMeshNowOrLater(MeshPool.GridPlane(Vector2.one * 0.7f), matrix * Matrix4x4.Translate(a / 100f), this.LensFlareMat, parms.DrawNow, null);
			}
			Pawn pawn = node.tree.pawn.mindState.enemyTarget as Pawn;
			if (pawn != null)
			{
				Rot4 rotation = (pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing();
				Vector3 b = pawn.Drawer.renderer.BaseHeadOffsetAt(rotation).RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
				Vector3 loc = (pawn.DrawPos + b).WithYOffset(PawnRenderUtility.AltitudeForLayer(80f));
				Quaternion quat = Quaternion.Euler(Vector3.up * pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
				GenDraw.DrawMeshNowOrLater(MeshPool.GridPlane(Vector2.one * 2f), loc, quat, this.LensFlareMat, parms.DrawNow);
			}
		}

		// Token: 0x04002006 RID: 8198
		private const float LensFlareScale = 0.7f;

		// Token: 0x04002007 RID: 8199
		private const float LensFlareAnimPeriod = 30f;

		// Token: 0x04002008 RID: 8200
		private const float Layer_AboveHead = 80f;

		// Token: 0x04002009 RID: 8201
		private const float PxToUnitScale = 100f;

		// Token: 0x0400200A RID: 8202
		private static readonly List<Vector3> EyePositionsPxNS = new List<Vector3>
		{
			new Vector3(-8f, 0f, -3f),
			new Vector3(12f, 0f, -5f),
			new Vector3(6f, 0f, 12f)
		};

		// Token: 0x0400200B RID: 8203
		private static readonly List<Vector3> EyePositionsPxEW = new List<Vector3>
		{
			new Vector3(19f, 0f, 9f),
			new Vector3(18f, 0f, -7f)
		};

		// Token: 0x0400200C RID: 8204
		private static readonly SimpleCurve LensFlareOpacity = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.4f),
				true
			},
			{
				new CurvePoint(0.5f, 0.8f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};

		// Token: 0x0400200D RID: 8205
		private Material lensFlareMat;
	}
}
