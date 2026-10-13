using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CAC RID: 15532
	public static class WorldRendererUtility
	{
		// Token: 0x1700349F RID: 13471
		// (get) Token: 0x060151DA RID: 86490 RVA: 0x00651DD0 File Offset: 0x0064FFD0
		public static WorldRenderMode CurrentWorldRenderMode
		{
			get
			{
				if (Find.World == null)
				{
					return WorldRenderMode.None;
				}
				if (WorldComponent_GravshipController.CutsceneInProgress)
				{
					if (Find.CurrentMap != null && Find.CurrentMap.generatorDef.renderWorld)
					{
						return WorldRenderMode.Background;
					}
					return WorldRenderMode.None;
				}
				else
				{
					if (Current.ProgramState == ProgramState.Playing && Find.CurrentMap == null)
					{
						return WorldRenderMode.Planet;
					}
					if (Find.World.renderer.wantedMode == WorldRenderMode.Planet)
					{
						return WorldRenderMode.Planet;
					}
					if (Find.CurrentMap != null && Find.CurrentMap.generatorDef.renderWorld)
					{
						return WorldRenderMode.Background;
					}
					return Find.World.renderer.wantedMode;
				}
			}
		}

		// Token: 0x170034A0 RID: 13472
		// (get) Token: 0x060151DB RID: 86491 RVA: 0x00651E57 File Offset: 0x00650057
		public static bool WorldRendered
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode > WorldRenderMode.None;
			}
		}

		// Token: 0x170034A1 RID: 13473
		// (get) Token: 0x060151DC RID: 86492 RVA: 0x00651E61 File Offset: 0x00650061
		public static bool WorldBackgroundNow
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode == WorldRenderMode.Background;
			}
		}

		// Token: 0x170034A2 RID: 13474
		// (get) Token: 0x060151DD RID: 86493 RVA: 0x00651E6B File Offset: 0x0065006B
		public static bool WorldSelected
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode == WorldRenderMode.Planet;
			}
		}

		// Token: 0x170034A3 RID: 13475
		// (get) Token: 0x060151DE RID: 86494 RVA: 0x00651E75 File Offset: 0x00650075
		public static bool DrawingMap
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode != WorldRenderMode.Planet;
			}
		}

		// Token: 0x060151DF RID: 86495 RVA: 0x00651E84 File Offset: 0x00650084
		public static void UpdateGlobalShadersParams()
		{
			Vector3 v = -GenCelestial.CurSunPositionInWorldSpace();
			float value = (WorldRendererUtility.WorldBackgroundNow || !PlanetLayer.Selected.IsRootSurface || Find.PlaySettings.usePlanetDayNightSystem) ? 1f : 0f;
			Shader.SetGlobalVector(ShaderPropertyIDs.PlanetSunLightDirection, v);
			Shader.SetGlobalFloat(ShaderPropertyIDs.PlanetSunLightEnabled, value);
			Shader.SetGlobalFloat(ShaderPropertyIDs.BackgroundModeEnabled, WorldRendererUtility.WorldBackgroundNow ? 1f : 0f);
		}

		// Token: 0x060151E0 RID: 86496 RVA: 0x00651F03 File Offset: 0x00650103
		public static void PrintQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, float rotation = 0f, bool printUVs = true)
		{
			WorldRendererUtility.PrintQuadTangentialToPlanet(pos, pos, size, altOffset, subMesh, counterClockwise, rotation, printUVs);
		}

		// Token: 0x060151E1 RID: 86497 RVA: 0x00651F18 File Offset: 0x00650118
		public static void PrintQuadTangentialToPlanet(Vector3 pos, Vector3 posForTangents, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, float rotation = 0f, bool printUVs = true)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(posForTangents, out a, out a2, rotation);
			Vector3 normalized = posForTangents.normalized;
			float d = size * 0.5f;
			Vector3 item = pos - a * d - a2 * d + normalized * altOffset;
			Vector3 item2 = pos - a * d + a2 * d + normalized * altOffset;
			Vector3 item3 = pos + a * d + a2 * d + normalized * altOffset;
			Vector3 item4 = pos + a * d - a2 * d + normalized * altOffset;
			int count = subMesh.verts.Count;
			subMesh.verts.Add(item);
			subMesh.verts.Add(item2);
			subMesh.verts.Add(item3);
			subMesh.verts.Add(item4);
			if (printUVs)
			{
				subMesh.uvs.Add(new Vector2(0f, 0f));
				subMesh.uvs.Add(new Vector2(0f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 0f));
			}
			if (counterClockwise)
			{
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count);
				return;
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x060151E2 RID: 86498 RVA: 0x00652168 File Offset: 0x00650368
		public static void DrawQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, Material material, float rotationAngle = 0f, bool counterClockwise = false, bool useSkyboxLayer = false, MaterialPropertyBlock propertyBlock = null)
		{
			if (material == null)
			{
				Log.Warning("Tried to draw quad with null material.");
				return;
			}
			Vector3 normalized = pos.normalized;
			Vector3 vector;
			if (counterClockwise)
			{
				vector = -normalized;
			}
			else
			{
				vector = normalized;
			}
			Quaternion rhs = Quaternion.LookRotation(Vector3.Cross(vector, Vector3.up), vector);
			Quaternion q = Quaternion.AngleAxis(rotationAngle, normalized) * rhs;
			Vector3 s = new Vector3(size, 1f, size);
			Matrix4x4 matrix = Matrix4x4.TRS(pos + normalized * altOffset, q, s);
			int layer = useSkyboxLayer ? WorldCameraManager.WorldSkyboxLayer : WorldCameraManager.WorldLayer;
			if (propertyBlock != null)
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, propertyBlock);
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer);
		}

		// Token: 0x060151E3 RID: 86499 RVA: 0x00652220 File Offset: 0x00650420
		public static void GetTangentsToPlanet(Vector3 pos, out Vector3 first, out Vector3 second, float rotation = 0f)
		{
			Vector3 normalized = pos.normalized;
			Vector3 upwards = Vector3.up;
			if (Mathf.Abs(Vector3.Dot(normalized, Vector3.up)) > 0.999f)
			{
				upwards = Vector3.right;
			}
			Quaternion rhs = Quaternion.LookRotation(normalized, upwards);
			Quaternion rotation2 = Quaternion.AngleAxis(rotation, normalized) * rhs;
			first = rotation2 * Vector3.up;
			second = rotation2 * Vector3.right;
		}

		// Token: 0x060151E4 RID: 86500 RVA: 0x00652290 File Offset: 0x00650490
		public static Vector3 ProjectOnQuadTangentialToPlanet(Vector3 center, Vector2 point)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(center, out a, out a2, 0f);
			return point.x * a + point.y * a2;
		}

		// Token: 0x060151E5 RID: 86501 RVA: 0x006522CC File Offset: 0x006504CC
		public static void GetTangentialVectorFacing(Vector3 root, Vector3 pointToFace, out Vector3 forward, out Vector3 right)
		{
			Quaternion rotation = Quaternion.LookRotation(root, pointToFace);
			forward = rotation * Vector3.up;
			right = rotation * Vector3.left;
		}

		// Token: 0x060151E6 RID: 86502 RVA: 0x00652304 File Offset: 0x00650504
		public static void PrintTextureAtlasUVs(int indexX, int indexY, int numX, int numY, LayerSubMesh subMesh)
		{
			float num = 1f / (float)numX;
			float num2 = 1f / (float)numY;
			float num3 = (float)indexX * num;
			float num4 = (float)indexY * num2;
			subMesh.uvs.Add(new Vector2(num3, num4));
			subMesh.uvs.Add(new Vector2(num3, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4));
		}

		// Token: 0x060151E7 RID: 86503 RVA: 0x00652398 File Offset: 0x00650598
		public static bool HiddenBehindTerrainNow(Vector3 pos)
		{
			Dictionary<int, PlanetLayer> dictionary = (Dictionary<int, PlanetLayer>)Find.WorldGrid.PlanetLayers;
			foreach (int key in dictionary.Keys)
			{
				PlanetLayer planetLayer = dictionary[key];
				if (planetLayer.Visible && planetLayer.Def.obstructsExpandingIcons && planetLayer.LineIntersects(Find.WorldCameraDriver.CameraPosition, pos, 1f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400EB99 RID: 60313
		private const float WorldIntersectionFactor = 1f;
	}
}
