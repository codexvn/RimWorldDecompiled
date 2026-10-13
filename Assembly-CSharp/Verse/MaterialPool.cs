using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000811 RID: 2065
	public static class MaterialPool
	{
		// Token: 0x06003486 RID: 13446 RVA: 0x00113C83 File Offset: 0x00111E83
		public static Material MatFrom(string texPath, bool reportFailure)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, reportFailure)));
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00113CA8 File Offset: 0x00111EA8
		public static Material MatFrom(string texPath)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true)));
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00113CCD File Offset: 0x00111ECD
		public static Material MatFrom(Texture2D srcTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex));
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x00113CDA File Offset: 0x00111EDA
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color));
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x00113CEC File Offset: 0x00111EEC
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x00113D11 File Offset: 0x00111F11
		public static Material MatFrom(string texPath, Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader));
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x00113D28 File Offset: 0x00111F28
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x00113D52 File Offset: 0x00111F52
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color));
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x00113D68 File Offset: 0x00111F68
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x00113D93 File Offset: 0x00111F93
		public static Material MatFrom(Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(shader));
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x00113DA0 File Offset: 0x00111FA0
		public static Material MatFrom(MaterialRequest req)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a material from a different thread.");
				return null;
			}
			if (req.mainTex == null && req.needsMainTex)
			{
				Log.Error("MatFrom with null sourceTex.");
				return BaseContent.BadMat;
			}
			if (req.shader == null)
			{
				Log.Warning("Matfrom with null shader.");
				return BaseContent.BadMat;
			}
			if (req.maskTex != null && !req.shader.SupportsMaskTex())
			{
				Log.Error("MaterialRequest has maskTex but shader does not support it. req=" + req.ToString());
				req.maskTex = null;
			}
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			Material material;
			if (!MaterialPool.matDictionary.TryGetValue(req, out material))
			{
				material = MaterialAllocator.Create(req.shader);
				material.name = req.shader.name;
				if (req.mainTex != null)
				{
					Material material2 = material;
					material2.name = material2.name + "_" + req.mainTex.name;
					material.mainTexture = req.mainTex;
				}
				material.color = req.color;
				if (req.maskTex != null)
				{
					material.SetTexture(ShaderPropertyIDs.MaskTex, req.maskTex);
					material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
				}
				else if (req.colorTwo != default(Color))
				{
					material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
				}
				if (req.secondaryTex != null)
				{
					material.SetTexture(ShaderPropertyIDs.SecondaryTex, req.secondaryTex);
				}
				if (req.renderQueue != 0)
				{
					material.renderQueue = req.renderQueue;
				}
				if (!req.shaderParameters.NullOrEmpty<ShaderParameter>())
				{
					for (int i = 0; i < req.shaderParameters.Count; i++)
					{
						req.shaderParameters[i].Apply(material);
					}
				}
				MaterialPool.matDictionary.Add(req, material);
				MaterialPool.matDictionaryReverse.Add(material, req);
				if (req.shader == ShaderDatabase.CutoutPlant || req.shader == ShaderDatabase.TransparentPlant)
				{
					WindManager.Notify_PlantMaterialCreated(material);
				}
			}
			return material;
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x00113FEB File Offset: 0x001121EB
		public static bool TryGetRequestForMat(Material material, out MaterialRequest request)
		{
			return MaterialPool.matDictionaryReverse.TryGetValue(material, out request);
		}

		// Token: 0x04002799 RID: 10137
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();

		// Token: 0x0400279A RID: 10138
		private static Dictionary<Material, MaterialRequest> matDictionaryReverse = new Dictionary<Material, MaterialRequest>();
	}
}
