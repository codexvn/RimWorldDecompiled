using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000489 RID: 1161
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x0009EBDD File Offset: 0x0009CDDD
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0009F33E File Offset: 0x0009D53E
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0009F364 File Offset: 0x0009D564
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			if (this.scats == null)
			{
				this.scats = new List<SectionLayer_TerrainScatter.Scatterable>();
				this.GenerateScats();
			}
			this.scats.RemoveAll((SectionLayer_TerrainScatter.Scatterable scat) => !scat.IsOnValidTerrain);
			for (int i = 0; i < this.scats.Count; i++)
			{
				this.scats[i].PrintOnto(this);
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0009F3F0 File Offset: 0x0009D5F0
		private void GenerateScats()
		{
			foreach (IntVec3 intVec in SectionLayer_TerrainScatter.GetScatPoints(base.Map))
			{
				if (this.section.CellRect.Contains(intVec))
				{
					TerrainDef terrainDef = base.Map.terrainGrid.TerrainAt(intVec);
					if (terrainDef.scatterType != null)
					{
						if (!this.scatterDefsByType.ContainsKey(terrainDef.scatterType))
						{
							this.scatterDefsByType[terrainDef.scatterType] = new List<ScatterableDef>();
							foreach (ScatterableDef scatterableDef in DefDatabase<ScatterableDef>.AllDefs)
							{
								if (scatterableDef.scatterType == terrainDef.scatterType)
								{
									this.scatterDefsByType[terrainDef.scatterType].Add(scatterableDef);
								}
							}
						}
						if (!this.scatterDefsByType[terrainDef.scatterType].Empty<ScatterableDef>())
						{
							ScatterableDef scatterableDef2 = this.scatterDefsByType[terrainDef.scatterType].RandomElement<ScatterableDef>();
							if (Rand.Chance(scatterableDef2.scatterChance))
							{
								Vector3 loc = new Vector3((float)intVec.x + (Rand.Value - 0.5f), 0f, (float)intVec.z + (Rand.Value - 0.5f));
								SectionLayer_TerrainScatter.Scatterable scatterable = new SectionLayer_TerrainScatter.Scatterable(scatterableDef2, loc, base.Map);
								if (scatterable.IsOnValidTerrain)
								{
									this.scats.Add(scatterable);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0009F5BC File Offset: 0x0009D7BC
		private static List<IntVec3> GetScatPoints(Map map)
		{
			int key = map.ConstantRandSeed * Find.World.info.Seed;
			List<IntVec3> result;
			if (SectionLayer_TerrainScatter.cachedScatterablePoints.TryGetValue(key, out result))
			{
				return result;
			}
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 item in map.AllCells)
			{
				list.Add(item);
			}
			using (new ProfilerBlock("GenerateBlueNoisePoints"))
			{
				SectionLayer_TerrainScatter.cachedScatterablePoints[key] = SectionLayer_TerrainScatter.GenerateBlueNoisePoints(list, 5f);
			}
			return SectionLayer_TerrainScatter.cachedScatterablePoints[key];
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0009F684 File Offset: 0x0009D884
		private static List<IntVec3> GenerateBlueNoisePoints(List<IntVec3> candidatePoints, float minimumSpacing)
		{
			IntVec3[] array = new IntVec3[candidatePoints.Count];
			List<IntVec3> list = new List<IntVec3>();
			int num = 0;
			foreach (IntVec3 intVec in candidatePoints.InRandomOrder(null))
			{
				array[num] = intVec;
				num++;
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				bool flag = true;
				for (int j = 0; j < list.Count; j++)
				{
					IntVec3 otherLoc = list[j];
					if (array[i].InHorDistOf(otherLoc, minimumSpacing))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(array[i]);
				}
			}
			return list;
		}

		// Token: 0x04001A39 RID: 6713
		private const float MinimumSpacing = 5f;

		// Token: 0x04001A3A RID: 6714
		private List<SectionLayer_TerrainScatter.Scatterable> scats;

		// Token: 0x04001A3B RID: 6715
		private Dictionary<string, List<ScatterableDef>> scatterDefsByType = new Dictionary<string, List<ScatterableDef>>();

		// Token: 0x04001A3C RID: 6716
		private static readonly Dictionary<int, List<IntVec3>> cachedScatterablePoints = new Dictionary<int, List<IntVec3>>();

		// Token: 0x0200048A RID: 1162
		private class Scatterable
		{
			// Token: 0x06001E76 RID: 7798 RVA: 0x0009F75C File Offset: 0x0009D95C
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x06001E77 RID: 7799 RVA: 0x0009F7B0 File Offset: 0x0009D9B0
			public void PrintOnto(SectionLayer layer)
			{
				Material mat = this.def.mat;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Terrain, false, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, mat, this.rotation, false, uvs, null, 0.01f, 0f);
			}

			// Token: 0x17000586 RID: 1414
			// (get) Token: 0x06001E78 RID: 7800 RVA: 0x0009F80C File Offset: 0x0009DA0C
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 intVec = this.loc.ToIntVec3();
					if (this.def.scatterType != this.map.terrainGrid.TerrainAt(intVec).scatterType || intVec.Filled(this.map))
					{
						return false;
					}
					if (!this.def.placeUnderNaturalRoofs)
					{
						RoofDef roof = intVec.GetRoof(this.map);
						if (roof != null && roof.isNatural)
						{
							return false;
						}
					}
					foreach (IntVec3 c in CellRect.CenteredOn(intVec, Mathf.FloorToInt(this.size / 2f)).ClipInsideMap(this.map))
					{
						TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
						if (terrainDef.IsFloor || terrainDef.IsSubstructure || terrainDef == TerrainDefOf.Space)
						{
							return false;
						}
					}
					return true;
				}
			}

			// Token: 0x04001A3D RID: 6717
			private Map map;

			// Token: 0x04001A3E RID: 6718
			private ScatterableDef def;

			// Token: 0x04001A3F RID: 6719
			private Vector3 loc;

			// Token: 0x04001A40 RID: 6720
			private float size;

			// Token: 0x04001A41 RID: 6721
			private float rotation;
		}
	}
}
