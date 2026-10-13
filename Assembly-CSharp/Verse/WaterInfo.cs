using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000466 RID: 1126
	public class WaterInfo : MapComponent
	{
		// Token: 0x06001D94 RID: 7572 RVA: 0x00095ED8 File Offset: 0x000940D8
		public WaterInfo(Map map) : base(map)
		{
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x00095EF7 File Offset: 0x000940F7
		public override void MapRemoved()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				UnityEngine.Object.Destroy(this.riverFlowTexture);
			});
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x00095F0C File Offset: 0x0009410C
		public void SetTextures()
		{
			Camera subcamera = Current.SubcameraDriver.GetSubcamera(SubcameraDefOf.WaterDepth);
			if (subcamera != null)
			{
				Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOutputTex, subcamera.targetTexture);
			}
			if (this.skyTex == null)
			{
				this.skyTex = ContentFinder<Texture2D>.Get("Other/WaterReflection", true);
			}
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterReflectionTex, this.skyTex);
			Camera camera = Find.Camera;
			Vector4 value = camera.targetTexture ? new Vector4((float)camera.targetTexture.width, (float)camera.targetTexture.height, 1f / (float)camera.targetTexture.width, 1f / (float)camera.targetTexture.height) : new Vector4((float)Screen.width, (float)Screen.height, 1f / (float)Screen.width, 1f / (float)Screen.height);
			Shader.SetGlobalVector(ShaderPropertyIDs.MainCameraScreenParams, value);
			Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
			Matrix4x4 value2 = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false) * worldToCameraMatrix;
			Shader.SetGlobalMatrix(ShaderPropertyIDs.MainCameraVP, value2);
			if (this.riverFlowTexture == null && this.riverFlowMap != null && this.riverFlowMap.Count > 0)
			{
				this.flowMapPixels = new Color[this.riverFlowMap.Count / 2];
				for (int i = 0; i < this.map.Size.x; i++)
				{
					for (int j = 0; j < this.map.Size.z; j++)
					{
						IntVec3 intVec = new IntVec3(i, 0, j);
						if (this.map.terrainGrid.BaseTerrainAt(intVec).IsRiver)
						{
							int num = this.map.cellIndices.CellToIndex(intVec);
							Vector2 vector = new Vector2(this.riverFlowMap[num * 2], this.riverFlowMap[num * 2 + 1]);
							int num2 = 1;
							foreach (IntVec3 intVec2 in GenAdjFast.AdjacentCells8Way(intVec))
							{
								if (intVec2.InBounds(this.map) && this.map.terrainGrid.BaseTerrainAt(intVec).IsRiver)
								{
									int num3 = intVec2.x * this.map.Size.x + intVec2.z;
									vector += new Vector2(this.riverFlowMap[num3 * 2], this.riverFlowMap[num3 * 2 + 1]);
									num2++;
								}
							}
							this.flowMapPixels[j * this.map.Size.x + i] = new Color(vector.x / (float)num2, vector.y / (float)num2, 0f);
						}
					}
				}
				this.riverFlowTexture = new Texture2D(this.map.Size.x, this.map.Size.z, TextureFormat.RGFloat, false);
				this.riverFlowTexture.SetPixels(this.flowMapPixels);
				this.riverFlowTexture.wrapMode = TextureWrapMode.Clamp;
				this.riverFlowTexture.Apply();
			}
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOffsetTex, this.riverFlowTexture);
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSize, new Vector4((float)this.map.Size.x, (float)this.map.Size.z));
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000962B8 File Offset: 0x000944B8
		public Vector3 GetWaterMovement(Vector3 position)
		{
			if (this.riverFlowMap == null)
			{
				return Vector3.zero;
			}
			IntVec3 c = position.ToIntVec3();
			int num = this.map.cellIndices.CellToIndex(c);
			return new Vector3(this.riverFlowMap[num * 2], 0f, this.riverFlowMap[num * 2 + 1]);
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00096314 File Offset: 0x00094514
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text = null;
				if (this.riverFlowMap != null)
				{
					text = Convert.ToBase64String(CompressUtility.Compress(MemoryMarshal.AsBytes<float>(this.riverFlowMap.AsSpan<float>()))).AddLineBreaksToLongString();
				}
				Scribe_Values.Look<string>(ref text, "riverFlowMapDeflate", null, false);
				return;
			}
			if (Scribe.EnterNode("riverFlowMapDeflate"))
			{
				Scribe.ExitNode();
				string text2 = null;
				Scribe_Values.Look<string>(ref text2, "riverFlowMapDeflate", null, false);
				if (text2 != null)
				{
					Span<float> span = MemoryMarshal.Cast<byte, float>(CompressUtility.Decompress(Convert.FromBase64String(text2)));
					this.riverFlowMap = new List<float>(span.Length);
					Span<float> destination = this.riverFlowMap.AsSpan(span.Length);
					span.CopyTo(destination);
					this.riverFlowMap.UnsafeSetCount(span.Length);
				}
			}
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000963E8 File Offset: 0x000945E8
		public void DebugDrawRiver()
		{
			if (this.riverGraph.NullOrEmpty<RiverNode>())
			{
				return;
			}
			if (DebugViewSettings.drawRiverDebug)
			{
				foreach (RiverNode riverNode in this.riverGraph)
				{
					GenDraw.DrawLineBetween(riverNode.start, riverNode.end, SimpleColor.Green, 5f);
					GenDraw.DrawArrowRotated(Vector3.Lerp(riverNode.start, riverNode.end, 0.5f), riverNode.start.AngleToFlat(riverNode.end) - 270f, false);
				}
			}
			if (DebugViewSettings.drawRiverFlowDebug)
			{
				foreach (IntVec3 c in this.map.AllCells)
				{
					int num = this.map.cellIndices.CellToIndex(c);
					Vector3 vector = new Vector3(this.riverFlowMap[num * 2], 0f, this.riverFlowMap[num * 2 + 1]);
					if (!(vector == Vector3.zero))
					{
						GenDraw.DrawArrowRotated(c.ToVector3Shifted(), vector.AngleFlat(), false);
					}
				}
			}
		}

		// Token: 0x040019AF RID: 6575
		public Texture2D riverFlowTexture;

		// Token: 0x040019B0 RID: 6576
		public IntVec3 lakeCenter = IntVec3.Invalid;

		// Token: 0x040019B1 RID: 6577
		public List<RiverNode> riverGraph = new List<RiverNode>();

		// Token: 0x040019B2 RID: 6578
		public List<float> riverFlowMap;

		// Token: 0x040019B3 RID: 6579
		[Obsolete]
		public CellRect riverFlowMapBounds;

		// Token: 0x040019B4 RID: 6580
		private Color[] flowMapPixels;

		// Token: 0x040019B5 RID: 6581
		private Texture2D skyTex;

		// Token: 0x040019B6 RID: 6582
		private const string ReflectionTexture = "Other/WaterReflection";
	}
}
