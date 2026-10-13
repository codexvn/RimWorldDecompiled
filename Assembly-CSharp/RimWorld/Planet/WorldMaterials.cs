using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CA6 RID: 15526
	[StaticConstructorOnStartup]
	public static class WorldMaterials
	{
		// Token: 0x17003495 RID: 13461
		// (get) Token: 0x0601519F RID: 86431 RVA: 0x00650930 File Offset: 0x0064EB30
		public static Material CurTargetingMat
		{
			get
			{
				WorldMaterials.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return WorldMaterials.TargetSquareMatSingle;
			}
		}

		// Token: 0x060151A0 RID: 86432 RVA: 0x00650948 File Offset: 0x0064EB48
		static WorldMaterials()
		{
			WorldMaterials.GenerateMats(ref WorldMaterials.matsFertility, WorldMaterials.FertilitySpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsTemperature, WorldMaterials.TemperatureSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsElevation, WorldMaterials.ElevationSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsRainfall, WorldMaterials.RainfallSpectrum, WorldMaterials.NumMatsPerMode);
		}

		// Token: 0x060151A1 RID: 86433 RVA: 0x00650E94 File Offset: 0x0064F094
		private static void GenerateMats(ref Material[] mats, Color[] colorSpectrum, int numMats)
		{
			mats = new Material[numMats];
			for (int i = 0; i < numMats; i++)
			{
				mats[i] = MatsFromSpectrum.Get(colorSpectrum, (float)i / (float)numMats);
			}
		}

		// Token: 0x060151A2 RID: 86434 RVA: 0x00650EC4 File Offset: 0x0064F0C4
		public static Material MatForFertilityOverlay(float fert)
		{
			int value = Mathf.FloorToInt(fert * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsFertility[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x060151A3 RID: 86435 RVA: 0x00650EF4 File Offset: 0x0064F0F4
		public static Material MatForTemperature(float temp)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(-50f, 50f, temp) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsTemperature[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x060151A4 RID: 86436 RVA: 0x00650F34 File Offset: 0x0064F134
		public static Material MatForElevation(float elev)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, elev) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsElevation[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x060151A5 RID: 86437 RVA: 0x00650F74 File Offset: 0x0064F174
		public static Material MatForRainfallOverlay(float rain)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, rain) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsRainfall[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x0400EB5A RID: 60250
		public static readonly Material WorldTerrain = MatLoader.LoadMat("World/WorldTerrain", 3500);

		// Token: 0x0400EB5B RID: 60251
		public static readonly Material WorldIce = MatLoader.LoadMat("World/WorldIce", 3500);

		// Token: 0x0400EB5C RID: 60252
		public static readonly Material WorldOcean = MatLoader.LoadMat("World/WorldOcean", 3500);

		// Token: 0x0400EB5D RID: 60253
		public static readonly Material UngeneratedPlanetParts = MatLoader.LoadMat("World/UngeneratedPlanetParts", 3500);

		// Token: 0x0400EB5E RID: 60254
		public static readonly Material Clouds = MatLoader.LoadMat("World/Clouds", 3700);

		// Token: 0x0400EB5F RID: 60255
		public static readonly Material Rivers = MatLoader.LoadMat("World/Rivers", 3530);

		// Token: 0x0400EB60 RID: 60256
		public static readonly Material RiversBorder = MatLoader.LoadMat("World/RiversBorder", 3520);

		// Token: 0x0400EB61 RID: 60257
		public static readonly Material Roads = MatLoader.LoadMat("World/Roads", 3540);

		// Token: 0x0400EB62 RID: 60258
		public const int DebugTileRenderQueue = 3510;

		// Token: 0x0400EB63 RID: 60259
		public const int WorldObjectRenderQueue = 3550;

		// Token: 0x0400EB64 RID: 60260
		public const int WorldLineRenderQueue = 3590;

		// Token: 0x0400EB65 RID: 60261
		public const int DynamicObjectRenderQueue = 3600;

		// Token: 0x0400EB66 RID: 60262
		public const int FeatureNameRenderQueue = 3610;

		// Token: 0x0400EB67 RID: 60263
		public static readonly Material MouseTile = MaterialPool.MatFrom("World/MouseTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x0400EB68 RID: 60264
		public static readonly Material SelectedTile = MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x0400EB69 RID: 60265
		public static readonly Material ClosestTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x0400EB6A RID: 60266
		public static readonly Material CurrentMapTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayTransparent, 3560);

		// Token: 0x0400EB6B RID: 60267
		public static readonly Material Stars = MatLoader.LoadMat("World/Stars", -1);

		// Token: 0x0400EB6C RID: 60268
		public static readonly Material Sun = MatLoader.LoadMat("World/Sun", -1);

		// Token: 0x0400EB6D RID: 60269
		public static readonly Material SmallHills = MaterialPool.MatFrom("World/Hills/SmallHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x0400EB6E RID: 60270
		public static readonly Material LargeHills = MaterialPool.MatFrom("World/Hills/LargeHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x0400EB6F RID: 60271
		public static readonly Material Mountains = MaterialPool.MatFrom("World/Hills/Mountains", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x0400EB70 RID: 60272
		public static readonly Material ImpassableMountains = MaterialPool.MatFrom("World/Hills/Impassable", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x0400EB71 RID: 60273
		public static readonly Material VertexColor = MatLoader.LoadMat("World/WorldVertexColor", -1);

		// Token: 0x0400EB72 RID: 60274
		public static readonly Material VertexColorTransparent = MatLoader.LoadMat("World/WorldVertexColorTransparent", 3545);

		// Token: 0x0400EB73 RID: 60275
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent, 3560);

		// Token: 0x0400EB74 RID: 60276
		private static int NumMatsPerMode = 50;

		// Token: 0x0400EB75 RID: 60277
		public static Material OverlayModeMatOcean = SolidColorMaterials.NewSolidColorMaterial(new Color(0.09f, 0.18f, 0.2f), ShaderDatabase.Transparent);

		// Token: 0x0400EB76 RID: 60278
		private static Material[] matsFertility;

		// Token: 0x0400EB77 RID: 60279
		private static readonly Color[] FertilitySpectrum = new Color[]
		{
			new Color(0f, 1f, 0f, 0f),
			new Color(0f, 1f, 0f, 0.5f)
		};

		// Token: 0x0400EB78 RID: 60280
		private const float TempRange = 50f;

		// Token: 0x0400EB79 RID: 60281
		private static Material[] matsTemperature;

		// Token: 0x0400EB7A RID: 60282
		private static readonly Color[] TemperatureSpectrum = new Color[]
		{
			new Color(1f, 1f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.25f, 0.25f, 1f),
			new Color(0.6f, 0.6f, 1f),
			new Color(0.5f, 0.5f, 0.5f),
			new Color(0.5f, 0.3f, 0f),
			new Color(1f, 0.6f, 0.18f),
			new Color(1f, 0f, 0f)
		};

		// Token: 0x0400EB7B RID: 60283
		private const float ElevationMax = 5000f;

		// Token: 0x0400EB7C RID: 60284
		private static Material[] matsElevation;

		// Token: 0x0400EB7D RID: 60285
		private static readonly Color[] ElevationSpectrum = new Color[]
		{
			new Color(0.224f, 0.18f, 0.15f),
			new Color(0.447f, 0.369f, 0.298f),
			new Color(0.6f, 0.6f, 0.6f),
			new Color(1f, 1f, 1f)
		};

		// Token: 0x0400EB7E RID: 60286
		private const float RainfallMax = 5000f;

		// Token: 0x0400EB7F RID: 60287
		private static Material[] matsRainfall;

		// Token: 0x0400EB80 RID: 60288
		private static readonly Color[] RainfallSpectrum = new Color[]
		{
			new Color(0.9f, 0.9f, 0.9f),
			GenColor.FromBytes(190, 190, 190, 255),
			new Color(0.58f, 0.58f, 0.58f),
			GenColor.FromBytes(196, 112, 110, 255),
			GenColor.FromBytes(200, 179, 150, 255),
			GenColor.FromBytes(255, 199, 117, 255),
			GenColor.FromBytes(255, 255, 84, 255),
			GenColor.FromBytes(145, 255, 253, 255),
			GenColor.FromBytes(0, 255, 0, 255),
			GenColor.FromBytes(63, 198, 55, 255),
			GenColor.FromBytes(13, 150, 5, 255),
			GenColor.FromBytes(5, 112, 94, 255)
		};
	}
}
