using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D3D RID: 3389
	public static class SolidColorMaterials
	{
		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x0600562D RID: 22061 RVA: 0x001BD5E6 File Offset: 0x001BB7E6
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x001BD600 File Offset: 0x001BB800
		public static Material SimpleSolidColorMaterial(Color col, bool careAboutVertexColors = false)
		{
			col = col;
			Material material;
			if (careAboutVertexColors)
			{
				if (!SolidColorMaterials.simpleColorAndVertexColorMats.TryGetValue(col, out material))
				{
					material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.VertexColor);
					SolidColorMaterials.simpleColorAndVertexColorMats.Add(col, material);
				}
			}
			else if (!SolidColorMaterials.simpleColorMats.TryGetValue(col, out material))
			{
				material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.SolidColor);
				SolidColorMaterials.simpleColorMats.Add(col, material);
			}
			return material;
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x001BD670 File Offset: 0x001BB870
		public static Material NewSolidColorMaterial(Color col, Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a material from a different thread.");
				return null;
			}
			Material material = MaterialAllocator.Create(shader);
			material.color = col;
			string str = "SolidColorMat-";
			string name = shader.name;
			string str2 = "-";
			Color color = col;
			material.name = str + name + str2 + color.ToString();
			return material;
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x001BD6C7 File Offset: 0x001BB8C7
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06005631 RID: 22065 RVA: 0x001BD6D8 File Offset: 0x001BB8D8
		public static Texture2D NewSolidColorTexture(Color color)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a texture from a different thread.");
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2);
			string str = "SolidColorTex-";
			Color color2 = color;
			texture2D.name = str + color2.ToString();
			texture2D.SetPixel(0, 0, color);
			texture2D.SetPixel(1, 0, color);
			texture2D.SetPixel(0, 1, color);
			texture2D.SetPixel(1, 1, color);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x04003ED7 RID: 16087
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04003ED8 RID: 16088
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}
