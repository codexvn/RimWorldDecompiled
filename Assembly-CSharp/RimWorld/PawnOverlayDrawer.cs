using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022DC RID: 8924
	[StaticConstructorOnStartup]
	public abstract class PawnOverlayDrawer
	{
		// Token: 0x0600C20E RID: 49678 RVA: 0x003836E1 File Offset: 0x003818E1
		public PawnOverlayDrawer(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600C20F RID: 49679 RVA: 0x003836FB File Offset: 0x003818FB
		private static List<PawnOverlayDrawer.DrawCall> GetDrawCallList()
		{
			if (PawnOverlayDrawer.drawCallListPool.Count == 0)
			{
				return new List<PawnOverlayDrawer.DrawCall>();
			}
			List<PawnOverlayDrawer.DrawCall> result = PawnOverlayDrawer.drawCallListPool[PawnOverlayDrawer.drawCallListPool.Count - 1];
			PawnOverlayDrawer.drawCallListPool.RemoveAt(PawnOverlayDrawer.drawCallListPool.Count - 1);
			return result;
		}

		// Token: 0x0600C210 RID: 49680 RVA: 0x0038373B File Offset: 0x0038193B
		private static void ReturnDrawCallList(List<PawnOverlayDrawer.DrawCall> lst)
		{
			lst.Clear();
			PawnOverlayDrawer.drawCallListPool.Add(lst);
		}

		// Token: 0x0600C211 RID: 49681 RVA: 0x00383750 File Offset: 0x00381950
		public void ClearCache()
		{
			foreach (List<PawnOverlayDrawer.DrawCall> lst in this.drawCallCache.Values)
			{
				PawnOverlayDrawer.ReturnDrawCallList(lst);
			}
			this.drawCallCache.Clear();
		}

		// Token: 0x0600C212 RID: 49682
		protected abstract void WriteCache(PawnOverlayDrawer.CacheKey key, PawnDrawParms parms, List<PawnOverlayDrawer.DrawCall> writeTarget);

		// Token: 0x0600C213 RID: 49683 RVA: 0x003837B0 File Offset: 0x003819B0
		public void RenderPawnOverlay(Matrix4x4 matrix, Mesh bodyMesh, PawnOverlayDrawer.OverlayLayer layer, PawnDrawParms parms, bool? overApparel = null)
		{
			PawnOverlayDrawer.CacheKey key = new PawnOverlayDrawer.CacheKey(bodyMesh, parms.facing, layer);
			List<PawnOverlayDrawer.DrawCall> drawCallList;
			if (!this.drawCallCache.TryGetValue(key, out drawCallList))
			{
				drawCallList = PawnOverlayDrawer.GetDrawCallList();
				this.WriteCache(key, parms, drawCallList);
				this.drawCallCache.Add(key, drawCallList);
			}
			foreach (PawnOverlayDrawer.DrawCall drawCall in drawCallList)
			{
				if (overApparel != null)
				{
					bool? flag = overApparel;
					bool displayOverApparel = drawCall.displayOverApparel;
					if (!(flag.GetValueOrDefault() == displayOverApparel & flag != null))
					{
						continue;
					}
				}
				this.DoDrawCall(drawCall, matrix, parms.DrawNow);
			}
		}

		// Token: 0x0600C214 RID: 49684 RVA: 0x00383870 File Offset: 0x00381A70
		private void DoDrawCall(PawnOverlayDrawer.DrawCall call, Matrix4x4 matrix, bool drawNow)
		{
			if (drawNow)
			{
				if (call.maskTexOffset != null)
				{
					call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureOffset, call.maskTexOffset.Value);
					call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureScale, call.maskTexScale.Value);
				}
				if (call.mainTexOffset != null)
				{
					call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureOffset, call.mainTexOffset.Value);
				}
				if (call.mainTexScale != null)
				{
					call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureScale, call.mainTexScale.Value);
				}
				if (call.colorOverride != null)
				{
					call.overlayMat.SetColor(ShaderPropertyIDs.Color, call.colorOverride.Value);
				}
				call.overlayMat.SetInt(ShaderPropertyIDs.Rotation, call.rotation);
				call.overlayMat.SetPass(0);
				Graphics.DrawMeshNow(call.overlayMesh, matrix * call.matrix);
				call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureOffset, Vector4.zero);
				call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureScale, Vector4.one);
				call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureOffset, Vector4.zero);
				call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureScale, Vector4.one);
				if (call.colorOverride != null)
				{
					call.overlayMat.SetColor(ShaderPropertyIDs.Color, Color.white);
					return;
				}
			}
			else
			{
				PawnOverlayDrawer.propBlock.Clear();
				if (call.maskTexOffset != null)
				{
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MaskTextureOffset, call.maskTexOffset.Value);
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MaskTextureScale, call.maskTexScale.Value);
				}
				if (call.mainTexOffset != null)
				{
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MainTextureOffset, call.mainTexOffset.Value);
				}
				if (call.mainTexScale != null)
				{
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MainTextureScale, call.mainTexScale.Value);
				}
				if (call.colorOverride != null)
				{
					PawnOverlayDrawer.propBlock.SetColor(ShaderPropertyIDs.Color, call.colorOverride.Value);
				}
				PawnOverlayDrawer.propBlock.SetInt(ShaderPropertyIDs.Rotation, call.rotation);
				Graphics.DrawMesh(call.overlayMesh, matrix * call.matrix, call.overlayMat, 0, null, 0, PawnOverlayDrawer.propBlock);
			}
		}

		// Token: 0x0400846F RID: 33903
		protected Pawn pawn;

		// Token: 0x04008470 RID: 33904
		private static MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

		// Token: 0x04008471 RID: 33905
		protected Dictionary<PawnOverlayDrawer.CacheKey, List<PawnOverlayDrawer.DrawCall>> drawCallCache = new Dictionary<PawnOverlayDrawer.CacheKey, List<PawnOverlayDrawer.DrawCall>>();

		// Token: 0x04008472 RID: 33906
		protected static List<List<PawnOverlayDrawer.DrawCall>> drawCallListPool = new List<List<PawnOverlayDrawer.DrawCall>>();

		// Token: 0x020022DD RID: 8925
		public enum OverlayLayer
		{
			// Token: 0x04008474 RID: 33908
			Body,
			// Token: 0x04008475 RID: 33909
			Head
		}

		// Token: 0x020022DE RID: 8926
		protected struct DrawCall
		{
			// Token: 0x04008476 RID: 33910
			public Mesh overlayMesh;

			// Token: 0x04008477 RID: 33911
			public Matrix4x4 matrix;

			// Token: 0x04008478 RID: 33912
			public Material overlayMat;

			// Token: 0x04008479 RID: 33913
			public bool displayOverApparel;

			// Token: 0x0400847A RID: 33914
			public Color? colorOverride;

			// Token: 0x0400847B RID: 33915
			public Vector4? maskTexScale;

			// Token: 0x0400847C RID: 33916
			public Vector4? maskTexOffset;

			// Token: 0x0400847D RID: 33917
			public Vector4? mainTexScale;

			// Token: 0x0400847E RID: 33918
			public Vector4? mainTexOffset;

			// Token: 0x0400847F RID: 33919
			public int rotation;
		}

		// Token: 0x020022DF RID: 8927
		protected struct CacheKey : IEquatable<PawnOverlayDrawer.CacheKey>
		{
			// Token: 0x0600C216 RID: 49686 RVA: 0x00383B18 File Offset: 0x00381D18
			public CacheKey(Mesh bodyMesh, Rot4 pawnRot, PawnOverlayDrawer.OverlayLayer layer)
			{
				this.bodyMesh = bodyMesh;
				this.pawnRot = pawnRot;
				this.layer = layer;
				this.hash = ((bodyMesh == null) ? 0 : bodyMesh.GetHashCode());
				this.hash = Gen.HashCombineInt(pawnRot.GetHashCode(), this.hash);
				this.hash = Gen.HashCombineInt(layer.GetHashCode(), this.hash);
			}

			// Token: 0x0600C217 RID: 49687 RVA: 0x00383B8E File Offset: 0x00381D8E
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x0600C218 RID: 49688 RVA: 0x00383B96 File Offset: 0x00381D96
			public bool Equals(PawnOverlayDrawer.CacheKey other)
			{
				return other.bodyMesh == this.bodyMesh && other.pawnRot == this.pawnRot && other.layer == this.layer;
			}

			// Token: 0x0600C219 RID: 49689 RVA: 0x00383BD0 File Offset: 0x00381DD0
			public override bool Equals(object other)
			{
				if (other is PawnOverlayDrawer.CacheKey)
				{
					PawnOverlayDrawer.CacheKey other2 = (PawnOverlayDrawer.CacheKey)other;
					return this.Equals(other2);
				}
				return false;
			}

			// Token: 0x04008480 RID: 33920
			public Mesh bodyMesh;

			// Token: 0x04008481 RID: 33921
			public Rot4 pawnRot;

			// Token: 0x04008482 RID: 33922
			public PawnOverlayDrawer.OverlayLayer layer;

			// Token: 0x04008483 RID: 33923
			private int hash;
		}
	}
}
