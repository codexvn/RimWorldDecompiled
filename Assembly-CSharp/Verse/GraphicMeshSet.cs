using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000817 RID: 2071
	public class GraphicMeshSet
	{
		// Token: 0x060034AE RID: 13486 RVA: 0x00114D1C File Offset: 0x00112F1C
		public GraphicMeshSet(Mesh normalMesh, Mesh leftMesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			this.meshes[2] = normalMesh;
			array[num] = (array2[num2] = normalMesh);
			this.meshes[3] = leftMesh;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x00114D64 File Offset: 0x00112F64
		public GraphicMeshSet(Mesh mesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			Mesh[] array3 = this.meshes;
			int num3 = 2;
			this.meshes[3] = mesh;
			array[num] = (array2[num2] = (array3[num3] = mesh));
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00114DB0 File Offset: 0x00112FB0
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00114E08 File Offset: 0x00113008
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x00114E68 File Offset: 0x00113068
		public GraphicMeshSet(Mesh northMesh, Mesh eastMesh, Mesh southMesh, Mesh westMesh)
		{
			this.meshes[0] = northMesh;
			this.meshes[1] = eastMesh;
			this.meshes[2] = southMesh;
			this.meshes[3] = westMesh;
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x00114EA1 File Offset: 0x001130A1
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}

		// Token: 0x040027B2 RID: 10162
		private Mesh[] meshes = new Mesh[4];
	}
}
