using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Verse
{
	// Token: 0x02000801 RID: 2049
	public class StaticTextureAtlas
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600343A RID: 13370 RVA: 0x00112394 File Offset: 0x00110594
		public Texture2D ColorTexture
		{
			get
			{
				return this.colorTexture;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x0011239C File Offset: 0x0011059C
		public Texture2D MaskTexture
		{
			get
			{
				return this.maskTexture;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x0600343C RID: 13372 RVA: 0x001123A4 File Offset: 0x001105A4
		public static int MaxPixelsPerAtlas
		{
			get
			{
				return StaticTextureAtlas.MaxAtlasSize / 2 * (StaticTextureAtlas.MaxAtlasSize / 2);
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x001123B5 File Offset: 0x001105B5
		public static int MaxAtlasSize
		{
			get
			{
				return SystemInfo.maxTextureSize;
			}
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x001123BC File Offset: 0x001105BC
		public StaticTextureAtlas(TextureAtlasGroupKey groupKey)
		{
			this.groupKey = groupKey;
			this.colorTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x001123FC File Offset: 0x001105FC
		public void Insert(Texture2D texture, Texture2D mask = null)
		{
			if (this.groupKey.hasMask && mask == null)
			{
				Log.Error("Tried to insert a mask-less texture into a static atlas which does have a mask atlas");
			}
			if (!this.groupKey.hasMask && mask != null)
			{
				Log.Error("Tried to insert a mask texture into a static atlas which does not have a mask atlas");
			}
			this.textures.Add(texture);
			if (mask != null && this.groupKey.hasMask)
			{
				this.masks.Add(texture, mask);
			}
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x00112478 File Offset: 0x00110678
		public void Bake(bool rebake = false)
		{
			using (new DeepProfilerScope("StaticTextureAtlas.Bake()"))
			{
				if (rebake)
				{
					foreach (KeyValuePair<Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
					{
						UnityEngine.Object.Destroy(keyValuePair.Value.mesh);
					}
					this.tiles.Clear();
				}
				Rect[] array = this.CalcRectsForAtlas();
				if (array.Length != this.textures.Count)
				{
					Log.Error("Texture packing failed! Clearing out atlas...");
					this.textures.Clear();
				}
				else
				{
					bool flag = !UnityData.ComputeShadersSupported;
					this.BlitTexturesToColorAtlas(array, flag);
					if (this.groupKey.hasMask)
					{
						this.BuildMaskAtlas(array, flag);
					}
					this.BuildMeshesForUvs(array);
					if (Prefs.TextureCompression)
					{
						this.ApplyTextureCompression(flag);
					}
					if (flag)
					{
						DeepProfiler.Start("Final Texture2D.Apply() for atlas textures");
						if (this.colorTexture != null)
						{
							this.colorTexture.Apply(false, true);
						}
						if (this.maskTexture != null)
						{
							this.maskTexture.Apply(false, true);
						}
						DeepProfiler.End();
					}
				}
			}
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x001125D4 File Offset: 0x001107D4
		private void ApplyTextureCompression(bool noGpuCompressionSupport)
		{
			DeepProfiler.Start("Compress atlas textures");
			if (this.colorTexture != null)
			{
				if (noGpuCompressionSupport)
				{
					this.colorTexture.Compress(true);
				}
				else
				{
					string name = this.colorTexture.name;
					this.colorTexture = StaticTextureAtlas.FastCompressDXT(this.colorTexture, true);
					this.colorTexture.name = name;
				}
			}
			if (this.maskTexture != null)
			{
				if (noGpuCompressionSupport)
				{
					this.maskTexture.Compress(true);
				}
				else
				{
					string name2 = this.maskTexture.name;
					this.maskTexture = StaticTextureAtlas.FastCompressDXT(this.maskTexture, true);
					this.maskTexture.name = name2;
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x00112684 File Offset: 0x00110884
		private void BuildMeshesForUvs(Rect[] uvRects)
		{
			for (int i = 0; i < this.textures.Count; i++)
			{
				Mesh mesh = TextureAtlasHelper.CreateMeshForUV(uvRects[i], 0.5f);
				mesh.name = "TextureAtlasMesh_" + this.groupKey.ToString() + "_" + mesh.GetInstanceID().ToString();
				this.tiles.Add(this.textures[i], new StaticTextureAtlasTile
				{
					atlas = this,
					mesh = mesh,
					uvRect = uvRects[i]
				});
			}
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x00112730 File Offset: 0x00110930
		private Rect[] CalcRectsForAtlas()
		{
			DeepProfiler.Start("Create dummy textures for atlas packing");
			Texture2D[] array = this.textures.Select(delegate(Texture2D t)
			{
				int width = t.width;
				int height = t.height;
				bool flag = width % 4 == 0 && height % 4 == 0;
				return new Texture2D(width, height, flag ? TextureFormat.DXT1 : TextureFormat.Alpha8, 1, false, true)
				{
					name = t.name + "_DummyForAtlas",
					filterMode = t.filterMode,
					wrapMode = t.wrapMode,
					anisoLevel = t.anisoLevel,
					mipMapBias = t.mipMapBias,
					minimumMipmapLevel = t.minimumMipmapLevel
				};
			}).ToArray<Texture2D>();
			DeepProfiler.End();
			DeepProfiler.Start("PackTextures() with dummy textures");
			Rect[] result = this.colorTexture.PackTextures(array, 8, StaticTextureAtlas.MaxAtlasSize, false);
			Texture2D texture2D = this.colorTexture;
			Texture2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(array2[i]);
			}
			int mipCount = Mathf.FloorToInt(Mathf.Log((float)Mathf.Max(texture2D.width, texture2D.height) / 512f, 2f)) + 1;
			this.colorTexture = new Texture2D(texture2D.width, texture2D.height, GraphicsFormat.R8G8B8A8_SRGB, mipCount, TextureCreationFlags.MipChain | TextureCreationFlags.DontInitializePixels);
			UnityEngine.Object.DestroyImmediate(texture2D);
			DeepProfiler.End();
			return result;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x00112814 File Offset: 0x00110A14
		private void BlitTexturesToColorAtlas(Rect[] uvRects, bool noGpuCompressionSupport)
		{
			RenderTexture renderTexture = new RenderTexture(this.colorTexture.width, this.colorTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			{
				name = "StaticTextureAtlas_Bake_Temp"
			};
			renderTexture.Create();
			RenderTexture active = RenderTexture.active;
			Graphics.SetRenderTarget(renderTexture);
			GL.Clear(true, true, Color.clear);
			GL.PushMatrix();
			GL.LoadPixelMatrix(0f, (float)renderTexture.width, (float)renderTexture.height, 0f);
			DeepProfiler.Start("Render real textures to RT");
			Material material = new Material(Shader.Find("Custom/BlitExact"));
			try
			{
				for (int i = 0; i < this.textures.Count; i++)
				{
					Rect rect = uvRects[i];
					Texture2D texture2D = this.textures[i];
					float num = (float)Mathf.RoundToInt(rect.x * (float)this.colorTexture.width);
					int num2 = Mathf.RoundToInt((1f - rect.y) * (float)this.colorTexture.height);
					num2 -= texture2D.height;
					int num3 = Mathf.RoundToInt(rect.width * (float)this.colorTexture.width);
					int num4 = Mathf.RoundToInt(rect.height * (float)this.colorTexture.height);
					Graphics.DrawTexture(new Rect(num, (float)num2, (float)num3, (float)num4), texture2D, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, material, -1);
				}
			}
			finally
			{
				UnityEngine.Object.Destroy(material);
			}
			GL.PopMatrix();
			DeepProfiler.End();
			if (noGpuCompressionSupport)
			{
				DeepProfiler.Start("GPU Readback from RenderTexture to Texture2D");
				RenderTexture.active = renderTexture;
				this.colorTexture.ReadPixels(new Rect(0f, 0f, (float)this.colorTexture.width, (float)this.colorTexture.height), 0, 0, false);
				DeepProfiler.End();
				DeepProfiler.Start("colorTexture.Apply() to generate mipmaps");
				this.colorTexture.Apply(true, false);
				DeepProfiler.End();
			}
			else
			{
				DeepProfiler.Start("CopyTexture from RenderTexture to Texture2D");
				Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, this.colorTexture.width, this.colorTexture.height, this.colorTexture, 0, 0, 0, 0);
				DeepProfiler.End();
				DeepProfiler.Start("Generate mipmaps with compute shader");
				this.GenerateMipmapsWithCompute(this.colorTexture);
				DeepProfiler.End();
			}
			RenderTexture.active = active;
			renderTexture.Release();
			UnityEngine.Object.DestroyImmediate(renderTexture);
			this.colorTexture.name = "TextureAtlas_" + this.groupKey.ToString() + "_" + this.colorTexture.GetInstanceID().ToString();
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x00112AC4 File Offset: 0x00110CC4
		private void BuildMaskAtlas(Rect[] uvRects, bool noGpuCompressionSupport)
		{
			this.maskTexture = new Texture2D(this.colorTexture.width, this.colorTexture.height, TextureFormat.ARGB32, false);
			RenderTexture renderTexture = new RenderTexture(this.maskTexture.width, this.maskTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			{
				name = "StaticTextureAtlas_MaskBake_Temp"
			};
			renderTexture.Create();
			RenderTexture active = RenderTexture.active;
			Graphics.SetRenderTarget(renderTexture);
			GL.Clear(true, true, Color.clear);
			GL.PushMatrix();
			GL.LoadPixelMatrix(0f, (float)renderTexture.width, (float)renderTexture.height, 0f);
			DeepProfiler.Start("Render mask textures to RT");
			Material material = new Material(Shader.Find("Custom/BlitExact"));
			try
			{
				for (int i = 0; i < this.textures.Count; i++)
				{
					Texture2D key = this.textures[i];
					Texture2D texture2D;
					if (this.masks.TryGetValue(key, out texture2D))
					{
						Rect rect = uvRects[i];
						float num = (float)Mathf.RoundToInt(rect.x * (float)this.maskTexture.width);
						int num2 = Mathf.RoundToInt((1f - rect.y) * (float)this.maskTexture.height);
						num2 -= texture2D.height;
						int num3 = Mathf.RoundToInt(rect.width * (float)this.maskTexture.width);
						int num4 = Mathf.RoundToInt(rect.height * (float)this.maskTexture.height);
						Graphics.DrawTexture(new Rect(num, (float)num2, (float)num3, (float)num4), texture2D, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, material, -1);
					}
				}
			}
			finally
			{
				UnityEngine.Object.Destroy(material);
			}
			GL.PopMatrix();
			DeepProfiler.End();
			if (noGpuCompressionSupport)
			{
				DeepProfiler.Start("GPU Readback from RenderTexture to mask Texture2D");
				RenderTexture.active = renderTexture;
				this.maskTexture.ReadPixels(new Rect(0f, 0f, (float)this.maskTexture.width, (float)this.maskTexture.height), 0, 0, false);
				DeepProfiler.End();
				DeepProfiler.Start("maskTexture.Apply()");
				this.maskTexture.Apply(true, false);
				DeepProfiler.End();
			}
			else
			{
				DeepProfiler.Start("CopyTexture from RenderTexture to mask Texture2D");
				Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, this.maskTexture.width, this.maskTexture.height, this.maskTexture, 0, 0, 0, 0);
				DeepProfiler.End();
			}
			RenderTexture.active = active;
			renderTexture.Release();
			UnityEngine.Object.DestroyImmediate(renderTexture);
			this.maskTexture.name = "Mask_" + this.colorTexture.name;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x00112D6C File Offset: 0x00110F6C
		public bool TryGetTile(Texture texture, out StaticTextureAtlasTile tile)
		{
			return this.tiles.TryGetValue(texture, out tile);
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x00112D7C File Offset: 0x00110F7C
		private void GenerateMipmapsWithCompute(Texture2D baseTexture)
		{
			ComputeShader computeShader = Resources.Load<ComputeShader>("Materials/Misc/Mipmapper");
			int kernelIndex = computeShader.FindKernel("GenerateMip");
			int num = Mathf.FloorToInt(Mathf.Log((float)Mathf.Max(baseTexture.width, baseTexture.height) / 512f, 2f));
			if (num < 1)
			{
				return;
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture renderTexture = new RenderTexture(baseTexture.width, baseTexture.height, 0, RenderTextureFormat.ARGB32);
			renderTexture.Create();
			Material material = new Material(Shader.Find("Custom/BlitExact"));
			Graphics.Blit(baseTexture, renderTexture, material);
			UnityEngine.Object.Destroy(material);
			RenderTexture[] array = new RenderTexture[num];
			RenderTexture renderTexture2 = renderTexture;
			for (int i = 0; i < num; i++)
			{
				int num2 = Mathf.Max(1, renderTexture2.width / 2);
				int num3 = Mathf.Max(1, renderTexture2.height / 2);
				array[i] = new RenderTexture(num2, num3, 0, RenderTextureFormat.ARGB32)
				{
					enableRandomWrite = true,
					name = string.Format("GenerateMipmapsWithCompute_Mip{0}", i + 1)
				};
				array[i].Create();
				computeShader.SetTexture(kernelIndex, "InputTexture", renderTexture2);
				computeShader.SetTexture(kernelIndex, "OutputMip", array[i]);
				computeShader.SetInts("InputSize", new int[]
				{
					renderTexture2.width,
					renderTexture2.height
				});
				computeShader.SetInts("OutputSize", new int[]
				{
					num2,
					num3
				});
				computeShader.Dispatch(kernelIndex, Mathf.CeilToInt((float)num2 / 8f), Mathf.CeilToInt((float)num3 / 8f), 1);
				renderTexture2 = array[i];
			}
			for (int j = 0; j < num; j++)
			{
				Graphics.CopyTexture(array[j], 0, 0, baseTexture, 0, j + 1);
				array[j].Release();
				UnityEngine.Object.DestroyImmediate(array[j]);
			}
			RenderTexture.active = active;
			renderTexture.Release();
			UnityEngine.Object.DestroyImmediate(renderTexture);
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x00112F68 File Offset: 0x00111168
		public static Texture2D FastCompressDXT(Texture2D texture, bool deleteOriginal = false)
		{
			int num = Mathf.Min(texture.width, texture.height);
			if (num <= 16)
			{
				return texture;
			}
			ComputeShader computeShader = Resources.Load<ComputeShader>("Materials/Misc/EncodeBCn");
			int kernelIndex = computeShader.FindKernel("EncodeBC3_AMD");
			int num2 = Mathf.FloorToInt(Mathf.Log((float)num / 16f, 2f)) + 1;
			num2 = Math.Min(num2, StaticTextureAtlas.CalculateMaxMipmapsForDxtSupport(texture));
			num2 = Mathf.Min(num2, texture.mipmapCount);
			num2 = Mathf.Max(num2, 1);
			Texture2D texture2D = new Texture2D(texture.width, texture.height, GraphicsFormat.RGBA_DXT5_UNorm, num2, TextureCreationFlags.DontInitializePixels | TextureCreationFlags.DontUploadUponCreate);
			texture2D.Apply(false, true);
			computeShader.SetFloat("_Quality", 0.9f);
			for (int i = 0; i < num2; i++)
			{
				int num3 = Mathf.Max(1, texture.width >> i);
				int num4 = Mathf.Max(1, texture.height >> i);
				RenderTexture renderTexture = new RenderTexture(num3 / 4, num4 / 4, 24)
				{
					graphicsFormat = GraphicsFormat.R32G32B32A32_SInt,
					enableRandomWrite = true,
					name = string.Format("FastCompressDXT_OutputRT_Mip{0}", i)
				};
				renderTexture.Create();
				computeShader.SetTexture(kernelIndex, "_Target", renderTexture);
				computeShader.SetTexture(kernelIndex, "_Source", texture, i);
				computeShader.SetInt("_mipLevel", i);
				computeShader.Dispatch(kernelIndex, num3 / 8, num4 / 8, 1);
				Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, renderTexture.width, renderTexture.height, texture2D, 0, i, 0, 0);
				renderTexture.Release();
				UnityEngine.Object.DestroyImmediate(renderTexture);
			}
			if (deleteOriginal)
			{
				UnityEngine.Object.DestroyImmediate(texture);
			}
			return texture2D;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x00113108 File Offset: 0x00111308
		public void Destroy()
		{
			UnityEngine.Object.Destroy(this.colorTexture);
			UnityEngine.Object.Destroy(this.maskTexture);
			foreach (KeyValuePair<Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
			{
				UnityEngine.Object.Destroy(keyValuePair.Value.mesh);
			}
			this.textures.Clear();
			this.tiles.Clear();
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x00113194 File Offset: 0x00111394
		public static int CalculateMaxMipmapsForDxtSupport(Texture2D tex)
		{
			int num = 0;
			int num2 = tex.width;
			int num3 = tex.height;
			while (num2 >= 4 && num3 >= 4 && num2 % 4 == 0 && num3 % 4 == 0)
			{
				num++;
				num2 >>= 1;
				num3 >>= 1;
			}
			if (num == 0)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x0400276E RID: 10094
		public readonly TextureAtlasGroupKey groupKey;

		// Token: 0x0400276F RID: 10095
		private List<Texture2D> textures = new List<Texture2D>();

		// Token: 0x04002770 RID: 10096
		private Dictionary<Texture2D, Texture2D> masks = new Dictionary<Texture2D, Texture2D>();

		// Token: 0x04002771 RID: 10097
		private Dictionary<Texture, StaticTextureAtlasTile> tiles = new Dictionary<Texture, StaticTextureAtlasTile>();

		// Token: 0x04002772 RID: 10098
		private Texture2D colorTexture;

		// Token: 0x04002773 RID: 10099
		private Texture2D maskTexture;

		// Token: 0x04002774 RID: 10100
		public const int MaxTextureSizeForTiles = 512;

		// Token: 0x04002775 RID: 10101
		public const int TexturePadding = 8;
	}
}
