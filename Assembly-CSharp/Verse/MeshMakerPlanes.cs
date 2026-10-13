using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000814 RID: 2068
	public static class MeshMakerPlanes
	{
		// Token: 0x060034A1 RID: 13473 RVA: 0x00114612 File Offset: 0x00112812
		public static Mesh NewPlaneMesh(float size)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, false);
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x0011461B File Offset: 0x0011281B
		public static Mesh NewPlaneMesh(float size, bool flipped)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, flipped, false);
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x00114625 File Offset: 0x00112825
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, false);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x00114636 File Offset: 0x00112836
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift, bool twist)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, twist);
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00114648 File Offset: 0x00112848
		public static Mesh NewPlaneMesh(Vector2 size, bool flipped, bool backLift, bool twist)
		{
			Vector3[] array = new Vector3[4];
			Vector2[] array2 = new Vector2[4];
			int[] array3 = new int[6];
			array[0] = new Vector3(-0.5f * size.x, 0f, -0.5f * size.y);
			array[1] = new Vector3(-0.5f * size.x, 0f, 0.5f * size.y);
			array[2] = new Vector3(0.5f * size.x, 0f, 0.5f * size.y);
			array[3] = new Vector3(0.5f * size.x, 0f, -0.5f * size.y);
			if (backLift)
			{
				array[1].y = 0.0018292684f;
				array[2].y = 0.0018292684f;
				array[3].y = 0.0007317074f;
			}
			if (twist)
			{
				array[0].y = 0.0009146342f;
				array[1].y = 0.0004573171f;
				array[2].y = 0f;
				array[3].y = 0.0004573171f;
			}
			if (!flipped)
			{
				array2[0] = new Vector2(0f, 0f);
				array2[1] = new Vector2(0f, 1f);
				array2[2] = new Vector2(1f, 1f);
				array2[3] = new Vector2(1f, 0f);
			}
			else
			{
				array2[0] = new Vector2(1f, 0f);
				array2[1] = new Vector2(1f, 1f);
				array2[2] = new Vector2(0f, 1f);
				array2[3] = new Vector2(0f, 0f);
			}
			array3[0] = 0;
			array3[1] = 1;
			array3[2] = 2;
			array3[3] = 0;
			array3[4] = 2;
			array3[5] = 3;
			Mesh mesh = new Mesh();
			mesh.name = "NewPlaneMesh()";
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.SetTriangles(array3, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			MeshPool.EnsureMetaDataCached(mesh, size, flipped);
			return mesh;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00114894 File Offset: 0x00112A94
		public static Mesh NewWholeMapPlane()
		{
			Mesh mesh = MeshMakerPlanes.NewPlaneMesh(2000f, false, false);
			Vector2[] array = new Vector2[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = mesh.uv[i] * 200f;
			}
			mesh.uv = array;
			return mesh;
		}

		// Token: 0x040027AB RID: 10155
		private const float BackLiftAmount = 0.0018292684f;

		// Token: 0x040027AC RID: 10156
		private const float TwistAmount = 0.0009146342f;
	}
}
