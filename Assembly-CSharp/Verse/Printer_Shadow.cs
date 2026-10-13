using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048F RID: 1167
	public static class Printer_Shadow
	{
		// Token: 0x06001E84 RID: 7812 RVA: 0x0009FF33 File Offset: 0x0009E133
		public static void PrintShadow(SectionLayer layer, Vector3 center, ShadowData shadow, Rot4 rotation)
		{
			Printer_Shadow.PrintShadow(layer, center, shadow.volume, rotation);
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0009FF44 File Offset: 0x0009E144
		public static void PrintShadow(SectionLayer layer, Vector3 center, Vector3 volume, Rot4 rotation)
		{
			if (!DebugViewSettings.drawShadows)
			{
				return;
			}
			LayerSubMesh subMesh = layer.GetSubMesh(MatBases.SunShadowFade);
			Color32 item = new Color32(byte.MaxValue, 0, 0, (byte)Mathf.Min(255f * (volume.y + Printer_Shadow.GlobalShadowSizeOffsetY), 255f));
			Vector3 vector = (volume + new Vector3(Printer_Shadow.GlobalShadowSizeOffsetX, 0f, Printer_Shadow.GlobalShadowSizeOffsetZ)).RotatedBy(rotation).Abs() / 2f;
			float x = center.x;
			float z = center.z;
			int count = subMesh.verts.Count;
			subMesh.verts.Add(new Vector3(x - vector.x, 0f, z - vector.z));
			subMesh.verts.Add(new Vector3(x - vector.x, 0f, z + vector.z));
			subMesh.verts.Add(new Vector3(x + vector.x, 0f, z + vector.z));
			subMesh.verts.Add(new Vector3(x + vector.x, 0f, z - vector.z));
			subMesh.colors.Add(Printer_Shadow.LowVertexColor);
			subMesh.colors.Add(Printer_Shadow.LowVertexColor);
			subMesh.colors.Add(Printer_Shadow.LowVertexColor);
			subMesh.colors.Add(Printer_Shadow.LowVertexColor);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
			int count2 = subMesh.verts.Count;
			subMesh.verts.Add(new Vector3(x - vector.x, 0f, z - vector.z));
			subMesh.verts.Add(new Vector3(x - vector.x, 0f, z + vector.z));
			subMesh.colors.Add(item);
			subMesh.colors.Add(item);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count2);
			subMesh.tris.Add(count2 + 1);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count2 + 1);
			subMesh.tris.Add(count + 1);
			int count3 = subMesh.verts.Count;
			subMesh.verts.Add(new Vector3(x + vector.x, 0f, z + vector.z));
			subMesh.verts.Add(new Vector3(x + vector.x, 0f, z - vector.z));
			subMesh.colors.Add(item);
			subMesh.colors.Add(item);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count3);
			subMesh.tris.Add(count3 + 1);
			subMesh.tris.Add(count3 + 1);
			subMesh.tris.Add(count + 3);
			subMesh.tris.Add(count + 2);
			int count4 = subMesh.verts.Count;
			subMesh.verts.Add(new Vector3(x - vector.x, 0f, z - vector.z));
			subMesh.verts.Add(new Vector3(x + vector.x, 0f, z - vector.z));
			subMesh.colors.Add(item);
			subMesh.colors.Add(item);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 3);
			subMesh.tris.Add(count4);
			subMesh.tris.Add(count + 3);
			subMesh.tris.Add(count4 + 1);
			subMesh.tris.Add(count4);
		}

		// Token: 0x04001A49 RID: 6729
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);

		// Token: 0x04001A4A RID: 6730
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetX = 0f;

		// Token: 0x04001A4B RID: 6731
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetY = 0f;

		// Token: 0x04001A4C RID: 6732
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetZ = 0f;
	}
}
