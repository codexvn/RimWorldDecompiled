using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000819 RID: 2073
	[StaticConstructorOnStartup]
	public static class MeshPool
	{
		// Token: 0x060034B8 RID: 13496 RVA: 0x00114ED8 File Offset: 0x001130D8
		static MeshPool()
		{
			MeshMakerCircles.MakePieMeshes(MeshPool.pies);
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x00114FD4 File Offset: 0x001131D4
		public static Mesh GridPlane(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planes.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, false, false, false);
				MeshPool.planes.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x00115008 File Offset: 0x00113208
		public static Mesh GridPlaneFlip(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planesFlip.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, true, false, false);
				MeshPool.planesFlip.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x0011503C File Offset: 0x0011323C
		public static Mesh GridPlaneFlip(Mesh mesh)
		{
			MeshPool.MeshMetaData metaData = MeshPool.GetMetaData(mesh);
			if (!metaData.flipped)
			{
				return MeshPool.GridPlaneFlip(metaData.size);
			}
			return MeshPool.GridPlane(metaData.size);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x0011506F File Offset: 0x0011326F
		public static Mesh GridPlane(Vector2 size, bool flip)
		{
			if (!flip)
			{
				return MeshPool.GridPlane(size);
			}
			return MeshPool.GridPlaneFlip(size);
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x00115084 File Offset: 0x00113284
		[DebugOutput("System", false)]
		public static void MeshPoolStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MeshPool stats:");
			stringBuilder.AppendLine("Planes: " + MeshPool.planes.Count.ToString());
			stringBuilder.AppendLine("PlanesFlip: " + MeshPool.planesFlip.Count.ToString());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x001150F2 File Offset: 0x001132F2
		public static GraphicMeshSet GetMeshSetForWidth(float width)
		{
			return MeshPool.GetMeshSetForSize(width, width);
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x001150FB File Offset: 0x001132FB
		public static GraphicMeshSet GetMeshSetForSize(Vector2 size)
		{
			if (!MeshPool.humanlikeMeshSet_Custom.ContainsKey(size))
			{
				MeshPool.humanlikeMeshSet_Custom[size] = new GraphicMeshSet(size.x, size.y);
			}
			return MeshPool.humanlikeMeshSet_Custom[size];
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00115131 File Offset: 0x00113331
		public static GraphicMeshSet GetMeshSetForSize(float width, float height)
		{
			return MeshPool.GetMeshSetForSize(new Vector2(width, height));
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x0011513F File Offset: 0x0011333F
		public static MeshPool.MeshMetaData GetMetaData(Mesh mesh)
		{
			return MeshPool.meshMetaData[mesh];
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x0011514C File Offset: 0x0011334C
		public static void EnsureMetaDataCached(Mesh mesh, Vector2 size, bool flipped)
		{
			if (MeshPool.meshMetaData.ContainsKey(mesh))
			{
				return;
			}
			MeshPool.meshMetaData[mesh] = new MeshPool.MeshMetaData(size, flipped);
		}

		// Token: 0x040027B4 RID: 10164
		public const float HumanlikeBodyWidth = 1.5f;

		// Token: 0x040027B5 RID: 10165
		public const float HumanlikeHeadAverageWidth = 1.5f;

		// Token: 0x040027B6 RID: 10166
		private static readonly Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x040027B7 RID: 10167
		private static readonly Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x040027B8 RID: 10168
		private static readonly Dictionary<Mesh, MeshPool.MeshMetaData> meshMetaData = new Dictionary<Mesh, MeshPool.MeshMetaData>();

		// Token: 0x040027B9 RID: 10169
		private static readonly Dictionary<Vector2, GraphicMeshSet> humanlikeMeshSet_Custom = new Dictionary<Vector2, GraphicMeshSet>();

		// Token: 0x040027BA RID: 10170
		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		// Token: 0x040027BB RID: 10171
		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		// Token: 0x040027BC RID: 10172
		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		// Token: 0x040027BD RID: 10173
		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		// Token: 0x040027BE RID: 10174
		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		// Token: 0x040027BF RID: 10175
		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		// Token: 0x040027C0 RID: 10176
		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		// Token: 0x040027C1 RID: 10177
		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		// Token: 0x040027C2 RID: 10178
		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		// Token: 0x040027C3 RID: 10179
		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		// Token: 0x040027C4 RID: 10180
		public static readonly Mesh[] pies = new Mesh[361];

		// Token: 0x040027C5 RID: 10181
		public static readonly Mesh wholeMapPlane;

		// Token: 0x0200081A RID: 2074
		public struct MeshMetaData
		{
			// Token: 0x060034C3 RID: 13507 RVA: 0x0011516E File Offset: 0x0011336E
			public MeshMetaData(Vector2 size, bool flipped)
			{
				this.size = size;
				this.flipped = flipped;
			}

			// Token: 0x040027C6 RID: 10182
			public Vector2 size;

			// Token: 0x040027C7 RID: 10183
			public bool flipped;
		}
	}
}
