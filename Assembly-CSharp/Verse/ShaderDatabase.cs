using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000160 RID: 352
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00031607 File Offset: 0x0002F807
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00031610 File Offset: 0x0002F810
		public static Shader LoadShader(string shaderPath)
		{
			if (ShaderDatabase.lookup == null)
			{
				ShaderDatabase.lookup = new Dictionary<string, Shader>();
			}
			if (ShaderDatabase.uiLookup == null)
			{
				ShaderDatabase.uiLookup = new Dictionary<Shader, Shader>();
			}
			Shader result;
			ShaderDatabase.TryLoadShader(shaderPath, out result);
			return result;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0003164C File Offset: 0x0002F84C
		public static Shader LoadShader(ShaderTypeDef shaderDef)
		{
			Shader result;
			Shader shader;
			ShaderDatabase.LoadShader(shaderDef, out result, out shader);
			return result;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00031664 File Offset: 0x0002F864
		public static void LoadShader(ShaderTypeDef shaderDef, out Shader result, out Shader uiShader)
		{
			if (shaderDef == null)
			{
				Log.Warning("Tried to LoadShader with a null ShaderTypeDef. Returning default shader instead.");
				result = ShaderDatabase.DefaultShader;
				uiShader = null;
				return;
			}
			ShaderDatabase.LoadShader(shaderDef.shaderPath, shaderDef.uiShaderPath, out result, out uiShader);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00031691 File Offset: 0x0002F891
		public static void LoadShader(string shaderPath, string uiShaderPath, out Shader result, out Shader uiShader)
		{
			uiShader = null;
			if (!string.IsNullOrEmpty(uiShaderPath))
			{
				ShaderDatabase.TryLoadShader(uiShaderPath, out uiShader);
			}
			if (ShaderDatabase.TryLoadShader(shaderPath, out result) && uiShader != null)
			{
				ShaderDatabase.uiLookup[result] = uiShader;
			}
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000316C7 File Offset: 0x0002F8C7
		public static bool TryGetUIShader(Shader shader, out Shader uiShader)
		{
			return ShaderDatabase.uiLookup.TryGetValue(shader, out uiShader);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x000316D8 File Offset: 0x0002F8D8
		private static bool TryLoadShader(string shaderPath, out Shader result)
		{
			if (!ShaderDatabase.lookup.ContainsKey(shaderPath))
			{
				Shader shader = Resources.Load("Materials/" + shaderPath, typeof(Shader)) as Shader;
				if (shader != null)
				{
					ShaderDatabase.lookup[shaderPath] = shader;
				}
				else
				{
					Shader shader2 = ContentFinder<Shader>.TryFindAssetInModBundles(shaderPath);
					if (shader2 != null)
					{
						ShaderDatabase.lookup[shaderPath] = shader2;
					}
					else
					{
						ShaderDatabase.lookup[shaderPath] = null;
					}
				}
			}
			result = ShaderDatabase.lookup[shaderPath];
			if (result == null)
			{
				Log.Warning("Could not load shader " + shaderPath + " in resources or mod bundles. Using default shader instead.");
				result = ShaderDatabase.DefaultShader;
				return false;
			}
			return true;
		}

		// Token: 0x04000541 RID: 1345
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		// Token: 0x04000542 RID: 1346
		public static readonly Shader CutoutHair = ShaderDatabase.LoadShader("Map/CutoutHair");

		// Token: 0x04000543 RID: 1347
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		// Token: 0x04000544 RID: 1348
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		// Token: 0x04000545 RID: 1349
		public static readonly Shader CutoutComplexUI = ShaderDatabase.LoadShader("Map/CutoutComplexUI");

		// Token: 0x04000546 RID: 1350
		public static readonly Shader ExpandingIconUI = ShaderDatabase.LoadShader("Map/ExpandingIconUI");

		// Token: 0x04000547 RID: 1351
		public static readonly Shader CutoutComplexBlend = ShaderDatabase.LoadShader("Map/CutoutComplexBlend");

		// Token: 0x04000548 RID: 1352
		public static readonly Shader CutoutSkinOverlay = ShaderDatabase.LoadShader("Map/CutoutSkinOverlay");

		// Token: 0x04000549 RID: 1353
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		// Token: 0x0400054A RID: 1354
		public static readonly Shader Wound = ShaderDatabase.LoadShader("Map/Wound");

		// Token: 0x0400054B RID: 1355
		public static readonly Shader WoundSkin = ShaderDatabase.LoadShader("Map/WoundSkin");

		// Token: 0x0400054C RID: 1356
		public static readonly Shader CutoutSkinColorOverride = ShaderDatabase.LoadShader("Map/CutoutSkinOverride");

		// Token: 0x0400054D RID: 1357
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		// Token: 0x0400054E RID: 1358
		public static readonly Shader CutoutFlying01 = ShaderDatabase.LoadShader("Map/CutoutFlying01");

		// Token: 0x0400054F RID: 1359
		public static readonly Shader FirefoamOverlay = ShaderDatabase.LoadShader("Map/FirefoamOverlay");

		// Token: 0x04000550 RID: 1360
		public static readonly Shader CutoutWithOverlay = ShaderDatabase.LoadShader("Map/CutoutWithOverlay");

		// Token: 0x04000551 RID: 1361
		public static readonly Shader CutoutOverlay = ShaderDatabase.LoadShader("Map/CutoutOverlay");

		// Token: 0x04000552 RID: 1362
		public static readonly Shader TerrainEdge = ShaderDatabase.LoadShader("Map/TerrainEdge");

		// Token: 0x04000553 RID: 1363
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		// Token: 0x04000554 RID: 1364
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		// Token: 0x04000555 RID: 1365
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		// Token: 0x04000556 RID: 1366
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		// Token: 0x04000557 RID: 1367
		public static readonly Shader MotePulse = ShaderDatabase.LoadShader("Map/MotePulse");

		// Token: 0x04000558 RID: 1368
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		// Token: 0x04000559 RID: 1369
		public static readonly Shader MoteGlowPulse = ShaderDatabase.LoadShader("Map/MoteGlowPulse");

		// Token: 0x0400055A RID: 1370
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		// Token: 0x0400055B RID: 1371
		public static readonly Shader MoteGlowDistorted = ShaderDatabase.LoadShader("Map/MoteGlowDistorted");

		// Token: 0x0400055C RID: 1372
		public static readonly Shader MoteGlowDistortBG = ShaderDatabase.LoadShader("Map/MoteGlowDistortBackground");

		// Token: 0x0400055D RID: 1373
		public static readonly Shader MoteProximityScannerRadius = ShaderDatabase.LoadShader("Map/MoteProximityScannerRadius");

		// Token: 0x0400055E RID: 1374
		public static readonly Shader GasRotating = ShaderDatabase.LoadShader("Map/GasRotating");

		// Token: 0x0400055F RID: 1375
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		// Token: 0x04000560 RID: 1376
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		// Token: 0x04000561 RID: 1377
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		// Token: 0x04000562 RID: 1378
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		// Token: 0x04000563 RID: 1379
		public static readonly Shader TerrainHardPolluted = ShaderDatabase.LoadShader("Map/TerrainHardLinearBurn");

		// Token: 0x04000564 RID: 1380
		public static readonly Shader TerrainFadePolluted = ShaderDatabase.LoadShader("Map/TerrainFadeLinearBurn");

		// Token: 0x04000565 RID: 1381
		public static readonly Shader TerrainFadeRoughPolluted = ShaderDatabase.LoadShader("Map/TerrainFadeRoughLinearBurn");

		// Token: 0x04000566 RID: 1382
		public static readonly Shader PollutionCloud = ShaderDatabase.LoadShader("Map/PollutionCloud");

		// Token: 0x04000567 RID: 1383
		public static readonly Shader MapEdgeTerrain = ShaderDatabase.LoadShader("Map/MapEdgeTerrain");

		// Token: 0x04000568 RID: 1384
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		// Token: 0x04000569 RID: 1385
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		// Token: 0x0400056A RID: 1386
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		// Token: 0x0400056B RID: 1387
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		// Token: 0x0400056C RID: 1388
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		// Token: 0x0400056D RID: 1389
		public static readonly Shader WorldOverlayTransparentLitPollution = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLitPollution");

		// Token: 0x0400056E RID: 1390
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		// Token: 0x0400056F RID: 1391
		public static readonly Shader WorldOverlayAdditiveTwoSided = ShaderDatabase.LoadShader("World/WorldOverlayAdditiveTwoSided");

		// Token: 0x04000570 RID: 1392
		public static readonly Shader PlanetGlow = ShaderDatabase.LoadShader("World/PlanetGlow");

		// Token: 0x04000571 RID: 1393
		public static readonly Shader Clouds = ShaderDatabase.LoadShader("World/Clouds");

		// Token: 0x04000572 RID: 1394
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		// Token: 0x04000573 RID: 1395
		public static readonly Shader MetaOverlayDesaturated = ShaderDatabase.LoadShader("Map/MetaOverlayDesaturated");

		// Token: 0x04000574 RID: 1396
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		// Token: 0x04000575 RID: 1397
		public static readonly Shader SolidColorBehind = ShaderDatabase.LoadShader("Map/SolidColorBehind");

		// Token: 0x04000576 RID: 1398
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		// Token: 0x04000577 RID: 1399
		public static readonly Shader RitualStencil = ShaderDatabase.LoadShader("Map/RitualStencil");

		// Token: 0x04000578 RID: 1400
		public static readonly Shader Invisible = ShaderDatabase.LoadShader("Misc/Invisible");

		// Token: 0x04000579 RID: 1401
		public static readonly Shader Silhouette = ShaderDatabase.LoadShader("Misc/Silhouette");

		// Token: 0x0400057A RID: 1402
		public static readonly Shader Metalblood = ShaderDatabase.LoadShader("Misc/Metalblood");

		// Token: 0x0400057B RID: 1403
		public static readonly Shader CaveExitRope = ShaderDatabase.LoadShader("Map/CaveExitRope");

		// Token: 0x0400057C RID: 1404
		public static readonly Shader BioferriteHarvester = ShaderDatabase.LoadShader("Map/BioferriteHarvester");

		// Token: 0x0400057D RID: 1405
		public static readonly Shader GrayscaleGUI = ShaderDatabase.LoadShader("Misc/GrayscaleGUI");

		// Token: 0x0400057E RID: 1406
		public static readonly Shader GravshipMaskMasked = ShaderDatabase.LoadShader("Map/GravshipMaskMasked");

		// Token: 0x0400057F RID: 1407
		public static readonly Shader IndoorMaskMasked = ShaderDatabase.LoadShader("Map/IndoorMaskMasked");

		// Token: 0x04000580 RID: 1408
		private static Dictionary<string, Shader> lookup;

		// Token: 0x04000581 RID: 1409
		private static Dictionary<Shader, Shader> uiLookup;
	}
}
