using System;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007F8 RID: 2040
	public static class TextureAtlasHelper
	{
		// Token: 0x06003419 RID: 13337 RVA: 0x00111930 File Offset: 0x0010FB30
		public static Mesh CreateMeshForUV(Rect uv, float scale = 1f)
		{
			return new Mesh
			{
				vertices = new Vector3[]
				{
					new Vector3(-1f * scale, 0f, -1f * scale),
					new Vector3(-1f * scale, 0f, 1f * scale),
					new Vector3(1f * scale, 0f, 1f * scale),
					new Vector3(1f * scale, 0f, -1f * scale)
				},
				normals = new Vector3[]
				{
					Vector3.up,
					Vector3.up,
					Vector3.up,
					Vector3.up
				},
				uv = new Vector2[]
				{
					uv.min,
					new Vector2(uv.xMin, uv.yMax),
					uv.max,
					new Vector2(uv.xMax, uv.yMin)
				},
				triangles = new int[]
				{
					0,
					1,
					2,
					2,
					3,
					0
				}
			};
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x00111A7C File Offset: 0x0010FC7C
		public static void WriteDebugPNG(RenderTexture atlas, string path)
		{
			Texture2D texture2D = new Texture2D(atlas.width, atlas.height, TextureFormat.ARGB32, false);
			RenderTexture.active = atlas;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)atlas.width, (float)atlas.height), 0, 0);
			RenderTexture.active = null;
			File.WriteAllBytes(path, texture2D.EncodeToPNG());
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x00111ADC File Offset: 0x0010FCDC
		public static void WriteDebugPNG(Texture2D atlas, string path)
		{
			Texture2D texture2D = atlas.isReadable ? atlas : TextureAtlasHelper.MakeReadableTextureInstance(atlas);
			File.WriteAllBytes(path, texture2D.EncodeToPNG());
			if (texture2D != atlas)
			{
				UnityEngine.Object.Destroy(texture2D);
			}
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x00111B18 File Offset: 0x0010FD18
		public static Texture2D MakeReadableTextureInstance(Texture2D source)
		{
			DeepProfiler.Start("MakeReadableTextureInstance");
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
			temporary.name = "MakeReadableTexture_Temp";
			Graphics.Blit(source, temporary);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(source.width, source.height);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(temporary);
			DeepProfiler.End();
			return texture2D;
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x00111BAF File Offset: 0x0010FDAF
		public static TextureAtlasGroup ToAtlasGroup(this ThingCategory category)
		{
			switch (category)
			{
			case ThingCategory.Item:
				return TextureAtlasGroup.Item;
			case ThingCategory.Building:
				return TextureAtlasGroup.Building;
			case ThingCategory.Plant:
				return TextureAtlasGroup.Plant;
			case ThingCategory.Filth:
				return TextureAtlasGroup.Filth;
			}
			return TextureAtlasGroup.Misc;
		}
	}
}
