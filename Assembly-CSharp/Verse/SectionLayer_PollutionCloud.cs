using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000482 RID: 1154
	[StaticConstructorOnStartup]
	public class SectionLayer_PollutionCloud : SectionLayer_Gas
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001E42 RID: 7746 RVA: 0x0009D52D File Offset: 0x0009B72D
		protected override FloatRange VertexScaleOffsetRange
		{
			get
			{
				return new FloatRange(7f, 11f);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001E43 RID: 7747 RVA: 0x0009D53E File Offset: 0x0009B73E
		protected override FloatRange VertexPositionOffsetRange
		{
			get
			{
				return new FloatRange(-1f, 1f);
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001E44 RID: 7748 RVA: 0x0009D550 File Offset: 0x0009B750
		public override Material Mat
		{
			get
			{
				if (SectionLayer_PollutionCloud.matCached == null)
				{
					SectionLayer_PollutionCloud.matCached = MaterialPool.MatFrom(SectionLayer_PollutionCloud.PollutionTex.Texture, ShaderDatabase.PollutionCloud, Color.white, 3000);
					SectionLayer_PollutionCloud.matCached.SetTexture(ShaderPropertyIDs.FadeTex, SectionLayer_PollutionCloud.PollutionFadeTex.Texture);
					SectionLayer_PollutionCloud.matCached.SetVector(ShaderPropertyIDs.TexScrollSpeed, SectionLayer_PollutionCloud.FadeTexScrollSpeed);
					SectionLayer_PollutionCloud.matCached.SetVector(ShaderPropertyIDs.TexScale, SectionLayer_PollutionCloud.FadeTexScale);
				}
				return SectionLayer_PollutionCloud.matCached;
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0009D5E0 File Offset: 0x0009B7E0
		public SectionLayer_PollutionCloud(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Pollution;
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x0009D62D File Offset: 0x0009B82D
		public override bool Visible
		{
			get
			{
				return base.Visible && ModsConfig.BiotechActive;
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0009D640 File Offset: 0x0009B840
		public override Color ColorAt(IntVec3 cell)
		{
			float depth = base.Map.snowGrid.GetDepth(cell);
			cell.GetSandDepth(base.Map);
			TerrainDef terrainDef = base.Map.terrainGrid.TerrainAt(cell);
			if (depth >= 0.4f)
			{
				return SectionLayer_PollutionCloud.SnowPollutionCloudColor;
			}
			return terrainDef.pollutionCloudColor;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0009D690 File Offset: 0x0009B890
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			LayerSubMesh subMesh = base.GetSubMesh(this.Mat);
			float altitude = AltitudeLayer.Gas.AltitudeFor();
			int num = this.section.botLeft.x;
			foreach (IntVec3 c in this.AffectedCells(this.section.CellRect))
			{
				int count = subMesh.verts.Count;
				base.AddCell(c, num, count, subMesh, altitude);
				num++;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.All);
			}
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0009D748 File Offset: 0x0009B948
		private IEnumerable<IntVec3> AffectedCells(CellRect rect)
		{
			this.cellsTmp.Clear();
			int num = 0;
			foreach (IntVec3 cell in rect.Cells)
			{
				if (base.Map.pollutionGrid.IsPolluted(cell))
				{
					num++;
				}
			}
			if ((float)num / (float)rect.Area < this.MinPercentage)
			{
				yield break;
			}
			float numCellsToCover = SectionLayer_PollutionCloud.NumCellsPerSectionPollutionLevel.Evaluate((float)num);
			int i = 0;
			while ((float)i < numCellsToCover)
			{
				for (int j = 0; j < this.AttemptsPerCell; j++)
				{
					IntVec3 randomCell = rect.RandomCell;
					if (base.Map.pollutionGrid.IsPolluted(randomCell) && !base.Map.terrainGrid.TerrainAt(randomCell).IsWater)
					{
						bool flag = !this.cellsTmp.Contains(randomCell);
						if (flag)
						{
							foreach (IntVec3 intVec in this.cellsTmp)
							{
								if (!intVec.InHorDistOf(randomCell, this.MinCellDistance))
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							this.cellsTmp.Add(randomCell);
							yield return randomCell;
							break;
						}
					}
				}
				int num2 = i;
				i = num2 + 1;
			}
			yield break;
		}

		// Token: 0x04001A12 RID: 6674
		private static Material matCached;

		// Token: 0x04001A13 RID: 6675
		private float MinCellDistance = 5f;

		// Token: 0x04001A14 RID: 6676
		private int AttemptsPerCell = 15;

		// Token: 0x04001A15 RID: 6677
		private float MinPercentage = 0.35f;

		// Token: 0x04001A16 RID: 6678
		private static readonly SimpleCurve NumCellsPerSectionPollutionLevel = new SimpleCurve
		{
			{
				new CurvePoint(10f, 3f),
				true
			},
			{
				new CurvePoint(20f, 6f),
				true
			}
		};

		// Token: 0x04001A17 RID: 6679
		private static readonly Vector2 FadeTexScrollSpeed = new Vector2(0.035f, 0.0125f);

		// Token: 0x04001A18 RID: 6680
		private static readonly Vector2 FadeTexScale = new Vector2(0.15f, 0.15f);

		// Token: 0x04001A19 RID: 6681
		private static readonly CachedTexture PollutionTex = new CachedTexture("Other/PollutionCloud");

		// Token: 0x04001A1A RID: 6682
		private static readonly CachedTexture PollutionFadeTex = new CachedTexture("Other/PollutionCloudFade");

		// Token: 0x04001A1B RID: 6683
		private static readonly Color SnowPollutionCloudColor = new Color(1f, 1f, 1f, 0.66f);

		// Token: 0x04001A1C RID: 6684
		private const float SnowPollutionColorThreshold = 0.4f;

		// Token: 0x04001A1D RID: 6685
		private List<IntVec3> cellsTmp = new List<IntVec3>();
	}
}
