using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048C RID: 1164
	public static class Printer_Mesh
	{
		// Token: 0x06001E7C RID: 7804 RVA: 0x0009F938 File Offset: 0x0009DB38
		public static void PrintMesh(SectionLayer layer, Matrix4x4 TRS, Mesh mesh, Material mat)
		{
			LayerSubMesh subMesh = layer.GetSubMesh(mat);
			int count = subMesh.verts.Count;
			int vertexCount = mesh.vertexCount;
			Vector3[] vertices = mesh.vertices;
			Color32[] colors = mesh.colors32;
			Vector2[] uv = mesh.uv;
			for (int i = 0; i < vertexCount; i++)
			{
				subMesh.verts.Add(TRS.MultiplyPoint3x4(vertices[i]));
				if (colors.Length > i)
				{
					subMesh.colors.Add(colors[i]);
				}
				else
				{
					subMesh.colors.Add(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				}
				if (uv.Length > i)
				{
					subMesh.uvs.Add(uv[i]);
				}
				else
				{
					subMesh.uvs.Add(Vector2.zero);
				}
			}
			foreach (int num in mesh.triangles)
			{
				subMesh.tris.Add(count + num);
			}
		}
	}
}
