using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007FB RID: 2043
	public class PawnTextureAtlas
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x00111C25 File Offset: 0x0010FE25
		public RenderTexture RawTexture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x00111C2D File Offset: 0x0010FE2D
		public int FreeCount
		{
			get
			{
				return this.freeFrameSets.Count;
			}
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x00111C3C File Offset: 0x0010FE3C
		public PawnTextureAtlas()
		{
			this.texture = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32, 0);
			this.texture.name = "PawnTextureAtlas_" + 2048.ToString();
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < 2048; i += 128)
			{
				for (int j = 0; j < 2048; j += 128)
				{
					list.Add(new Rect((float)i / 2048f, (float)j / 2048f, 0.0625f, 0.0625f));
				}
			}
			while (list.Count >= 8)
			{
				PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = new PawnTextureAtlasFrameSet();
				pawnTextureAtlasFrameSet.uvRects = new Rect[]
				{
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>()
				};
				pawnTextureAtlasFrameSet.meshes = (from u in pawnTextureAtlasFrameSet.uvRects
				select TextureAtlasHelper.CreateMeshForUV(u, 1f)).ToArray<Mesh>();
				pawnTextureAtlasFrameSet.atlas = this.texture;
				this.freeFrameSets.Add(pawnTextureAtlasFrameSet);
			}
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x00111DD1 File Offset: 0x0010FFD1
		public bool ContainsFrameSet(Pawn pawn)
		{
			return this.frameAssignments.ContainsKey(pawn);
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x00111DE0 File Offset: 0x0010FFE0
		public bool TryGetFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew)
		{
			createdNew = false;
			if (this.frameAssignments.TryGetValue(pawn, out frameSet))
			{
				return true;
			}
			if (this.FreeCount == 0)
			{
				return false;
			}
			createdNew = true;
			frameSet = this.freeFrameSets.Pop<PawnTextureAtlasFrameSet>();
			for (int i = 0; i < frameSet.isDirty.Length; i++)
			{
				frameSet.isDirty[i] = true;
			}
			this.frameAssignments.Add(pawn, frameSet);
			return true;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x00111E48 File Offset: 0x00110048
		public void GC()
		{
			try
			{
				foreach (Pawn pawn in this.frameAssignments.Keys)
				{
					if (!pawn.SpawnedOrAnyParentSpawned)
					{
						PawnTextureAtlas.tmpPawnsToFree.Add(pawn);
					}
				}
				foreach (Pawn key in PawnTextureAtlas.tmpPawnsToFree)
				{
					this.freeFrameSets.Add(this.frameAssignments[key]);
					this.frameAssignments.Remove(key);
				}
			}
			finally
			{
				PawnTextureAtlas.tmpPawnsToFree.Clear();
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x00111F24 File Offset: 0x00110124
		public void Destroy()
		{
			foreach (PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet in this.frameAssignments.Values.Concat(this.freeFrameSets))
			{
				Mesh[] meshes = pawnTextureAtlasFrameSet.meshes;
				for (int i = 0; i < meshes.Length; i++)
				{
					UnityEngine.Object.Destroy(meshes[i]);
				}
			}
			this.frameAssignments.Clear();
			this.freeFrameSets.Clear();
			UnityEngine.Object.Destroy(this.texture);
		}

		// Token: 0x04002756 RID: 10070
		private RenderTexture texture;

		// Token: 0x04002757 RID: 10071
		private Dictionary<Pawn, PawnTextureAtlasFrameSet> frameAssignments = new Dictionary<Pawn, PawnTextureAtlasFrameSet>();

		// Token: 0x04002758 RID: 10072
		private List<PawnTextureAtlasFrameSet> freeFrameSets = new List<PawnTextureAtlasFrameSet>();

		// Token: 0x04002759 RID: 10073
		private static List<Pawn> tmpPawnsToFree = new List<Pawn>();

		// Token: 0x0400275A RID: 10074
		private const int AtlasSize = 2048;

		// Token: 0x0400275B RID: 10075
		public const int FrameSize = 128;

		// Token: 0x0400275C RID: 10076
		private const int PawnsHeldPerAtlas = 32;

		// Token: 0x0400275D RID: 10077
		private const int FramesPerPawn = 8;
	}
}
