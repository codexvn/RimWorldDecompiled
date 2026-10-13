using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000494 RID: 1172
	[StaticConstructorOnStartup]
	public static class MapEdgeClipDrawer
	{
		// Token: 0x06001E95 RID: 7829 RVA: 0x000A095C File Offset: 0x0009EB5C
		public static void DrawClippers(Map map)
		{
			if (!map.DrawMapClippers)
			{
				return;
			}
			Material mapEdgeMaterial = map.MapEdgeMaterial;
			IntVec3 size = map.Size;
			Vector3 vector = new Vector3(500f, 1f, (float)size.z);
			Vector3 vector2 = new Vector3(-250f, 0f, (float)size.z / 2f);
			MapEdgeClipDrawer.horPropertyBlock.SetVector(ShaderPropertyIDs.MainTextureScale, vector);
			MapEdgeClipDrawer.horPropertyBlock.SetVector(ShaderPropertyIDs.MainTextureOffset, vector2);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector2.WithYOffset(MapEdgeClipDrawer.ClipAltitude), Quaternion.identity, vector);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mapEdgeMaterial, 0, null, 0, MapEdgeClipDrawer.horPropertyBlock);
			vector2 = new Vector3((float)size.x + 250f, 0f, (float)size.z / 2f);
			MapEdgeClipDrawer.horPropertyBlock.SetVector(ShaderPropertyIDs.MainTextureOffset, vector2);
			matrix = default(Matrix4x4);
			matrix.SetTRS(vector2.WithYOffset(MapEdgeClipDrawer.ClipAltitude), Quaternion.identity, vector);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mapEdgeMaterial, 0, null, 0, MapEdgeClipDrawer.horPropertyBlock);
			vector = new Vector3(1000f, 1f, 500f);
			vector2 = new Vector3((float)size.x / 2f, 0f, (float)size.z + 250f);
			MapEdgeClipDrawer.vertPropertyBlock.SetVector(ShaderPropertyIDs.MainTextureScale, vector);
			MapEdgeClipDrawer.vertPropertyBlock.SetVector(ShaderPropertyIDs.MainTextureOffset, vector2);
			matrix = default(Matrix4x4);
			matrix.SetTRS(vector2.WithYOffset(MapEdgeClipDrawer.ClipAltitude), Quaternion.identity, vector);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mapEdgeMaterial, 0, null, 0, MapEdgeClipDrawer.vertPropertyBlock);
			vector2 = new Vector3((float)size.x / 2f, 0f, -250f);
			MapEdgeClipDrawer.vertPropertyBlock.SetVector(ShaderPropertyIDs.MainTextureOffset, vector2);
			matrix = default(Matrix4x4);
			matrix.SetTRS(vector2.WithYOffset(MapEdgeClipDrawer.ClipAltitude), Quaternion.identity, vector);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mapEdgeMaterial, 0, null, 0, MapEdgeClipDrawer.vertPropertyBlock);
		}

		// Token: 0x04001A50 RID: 6736
		public static readonly Material ClipMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.1f, 0.1f, 0.1f), ShaderDatabase.MetaOverlay);

		// Token: 0x04001A51 RID: 6737
		public static readonly Material ClipMatMetalhell = SolidColorMaterials.NewSolidColorMaterial(new Color(0.03f, 0.04f, 0.04f), ShaderDatabase.MetaOverlay);

		// Token: 0x04001A52 RID: 6738
		private static readonly float ClipAltitude = AltitudeLayer.WorldClipper.AltitudeFor();

		// Token: 0x04001A53 RID: 6739
		private const float ClipSize = 500f;

		// Token: 0x04001A54 RID: 6740
		private static MaterialPropertyBlock vertPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04001A55 RID: 6741
		private static MaterialPropertyBlock horPropertyBlock = new MaterialPropertyBlock();
	}
}
