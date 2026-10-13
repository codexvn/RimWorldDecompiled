using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F85 RID: 8069
	public class SectionLayer_SubstructureProps : SectionLayer
	{
		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x0600B252 RID: 45650 RVA: 0x003303AD File Offset: 0x0032E5AD
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive && DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x0600B253 RID: 45651 RVA: 0x003303BD File Offset: 0x0032E5BD
		public SectionLayer_SubstructureProps(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlagDefOf.Terrain | MapMeshFlagDefOf.Buildings);
		}

		// Token: 0x0600B254 RID: 45652 RVA: 0x003303E4 File Offset: 0x0032E5E4
		public override void Regenerate()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float altitude = AltitudeLayer.TerrainScatter.AltitudeFor();
			LayerSubMesh subMesh = base.GetSubMesh(SectionLayer_SubstructureProps.Bottom.Material);
			foreach (IntVec3 intVec in cellRect)
			{
				SectionLayer_SubstructureProps.EdgeDirections edgeDirections;
				SectionLayer_SubstructureProps.CornerDirections cornerDirections;
				if (this.ShouldDrawPropsOn(intVec, terrainGrid, out edgeDirections, out cornerDirections))
				{
					this.DrawEdges(intVec, edgeDirections, altitude);
					this.DrawCorners(intVec, cornerDirections, edgeDirections, altitude);
					SectionLayer_GravshipHull.CornerType cornerType;
					Color color;
					SectionLayer_GravshipHull.ShouldDrawCornerPiece(intVec + IntVec3.South, map, terrainGrid, out cornerType, out color);
					bool flag = cornerType == SectionLayer_GravshipHull.CornerType.Corner_NW || cornerType == SectionLayer_GravshipHull.CornerType.Diagonal_NW || cornerType == SectionLayer_GravshipHull.CornerType.Corner_NE || cornerType == SectionLayer_GravshipHull.CornerType.Diagonal_NE;
					if (edgeDirections.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.South) && !flag)
					{
						this.AddQuad(subMesh, intVec + IntVec3.South, altitude, Rot4.North, SectionLayer_GravshipMask.IsValidSubstructure(intVec));
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x0600B255 RID: 45653 RVA: 0x00330514 File Offset: 0x0032E714
		private void DrawEdges(IntVec3 c, SectionLayer_SubstructureProps.EdgeDirections edgeDirs, float altitude)
		{
			ValueTuple<CachedMaterial, Rot4>[] array;
			if (SectionLayer_SubstructureProps.EdgeMats.TryGetValue(edgeDirs, out array))
			{
				foreach (ValueTuple<CachedMaterial, Rot4> valueTuple in array)
				{
					CachedMaterial item = valueTuple.Item1;
					Rot4 item2 = valueTuple.Item2;
					this.AddQuad(base.GetSubMesh(item.Material), c, altitude, item2, false);
				}
			}
		}

		// Token: 0x0600B256 RID: 45654 RVA: 0x00330568 File Offset: 0x0032E768
		private void DrawCorners(IntVec3 c, SectionLayer_SubstructureProps.CornerDirections cornerDirections, SectionLayer_SubstructureProps.EdgeDirections edgeDirs, float altitude)
		{
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.NorthWest) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.North) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.West))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.South, false);
			}
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.NorthEast) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.North) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.East))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.West, false);
			}
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.SouthEast) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.South) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.East))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.North, false);
			}
			if (cornerDirections.HasFlag(SectionLayer_SubstructureProps.CornerDirections.SouthWest) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.South) && !edgeDirs.HasFlag(SectionLayer_SubstructureProps.EdgeDirections.West))
			{
				this.AddQuad(base.GetSubMesh(SectionLayer_SubstructureProps.CornerInner.Material), c, altitude, Rot4.East, false);
			}
		}

		// Token: 0x0600B257 RID: 45655 RVA: 0x003306D8 File Offset: 0x0032E8D8
		private void AddQuad(LayerSubMesh sm, IntVec3 c, float altitude, Rot4 rotation, bool addGravshipMask)
		{
			int count = sm.verts.Count;
			int num = Mathf.Abs(4 - rotation.AsInt);
			for (int i = 0; i < 4; i++)
			{
				sm.verts.Add(new Vector3((float)c.x + SectionLayer_SubstructureProps.UVs[i].x, altitude, (float)c.z + SectionLayer_SubstructureProps.UVs[i].y));
				sm.uvs.Add(SectionLayer_SubstructureProps.UVs[(num + i) % 4]);
			}
			sm.tris.Add(count);
			sm.tris.Add(count + 1);
			sm.tris.Add(count + 2);
			sm.tris.Add(count);
			sm.tris.Add(count + 2);
			sm.tris.Add(count + 3);
			if (addGravshipMask)
			{
				Texture2D srcTex = sm.material.mainTexture as Texture2D;
				Color color = sm.material.color;
				Material material = MaterialPool.MatFrom(srcTex, ShaderDatabase.GravshipMaskMasked, color);
				this.AddQuad(base.GetSubMesh(material), c, altitude, rotation, false);
			}
		}

		// Token: 0x0600B258 RID: 45656 RVA: 0x003307FC File Offset: 0x0032E9FC
		private bool ShouldDrawPropsOn(IntVec3 c, TerrainGrid terrGrid, out SectionLayer_SubstructureProps.EdgeDirections edgeEdgeDirections, out SectionLayer_SubstructureProps.CornerDirections cornerDirections)
		{
			edgeEdgeDirections = SectionLayer_SubstructureProps.EdgeDirections.None;
			cornerDirections = SectionLayer_SubstructureProps.CornerDirections.None;
			TerrainDef terrainDef = terrGrid.FoundationAt(c);
			if (terrainDef == null || !terrainDef.IsSubstructure)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				if (!c2.InBounds(base.Map))
				{
					edgeEdgeDirections |= (SectionLayer_SubstructureProps.EdgeDirections)(1 << i);
				}
				else
				{
					TerrainDef terrainDef2 = terrGrid.FoundationAt(c2);
					if (terrainDef2 == null || !terrainDef2.IsSubstructure)
					{
						edgeEdgeDirections |= (SectionLayer_SubstructureProps.EdgeDirections)(1 << i);
					}
				}
			}
			for (int j = 0; j < GenAdj.DiagonalDirections.Length; j++)
			{
				IntVec3 c3 = c + GenAdj.DiagonalDirections[j];
				if (!c3.InBounds(base.Map))
				{
					cornerDirections |= (SectionLayer_SubstructureProps.CornerDirections)(1 << j);
				}
				else
				{
					TerrainDef terrainDef3 = terrGrid.FoundationAt(c3);
					if (terrainDef3 == null || !terrainDef3.IsSubstructure)
					{
						cornerDirections |= (SectionLayer_SubstructureProps.CornerDirections)(1 << j);
					}
				}
			}
			return edgeEdgeDirections != SectionLayer_SubstructureProps.EdgeDirections.None || cornerDirections > SectionLayer_SubstructureProps.CornerDirections.None;
		}

		// Token: 0x04007A13 RID: 31251
		private static readonly CachedMaterial OShape = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_OShape", ShaderDatabase.Transparent);

		// Token: 0x04007A14 RID: 31252
		private static readonly CachedMaterial UShape = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_UShape", ShaderDatabase.Transparent);

		// Token: 0x04007A15 RID: 31253
		private static readonly CachedMaterial CornerInner = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_CornerInner", ShaderDatabase.Transparent);

		// Token: 0x04007A16 RID: 31254
		private static readonly CachedMaterial CornerOuter = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_CornerOuter", ShaderDatabase.Transparent);

		// Token: 0x04007A17 RID: 31255
		private static readonly CachedMaterial Flat = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureBorder_Flat", ShaderDatabase.Transparent);

		// Token: 0x04007A18 RID: 31256
		private static readonly CachedMaterial Bottom = new CachedMaterial("Terrain/Surfaces/Substructure/SubstructureProps_Bottom", ShaderDatabase.Transparent);

		// Token: 0x04007A19 RID: 31257
		private static readonly Vector2[] UVs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x04007A1A RID: 31258
		private static readonly Dictionary<SectionLayer_SubstructureProps.EdgeDirections, ValueTuple<CachedMaterial, Rot4>[]> EdgeMats = new Dictionary<SectionLayer_SubstructureProps.EdgeDirections, ValueTuple<CachedMaterial, Rot4>[]>
		{
			{
				SectionLayer_SubstructureProps.EdgeDirections.North,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.South)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.West)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.West)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.CornerOuter, Rot4.South)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.South),
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.West),
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.West)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.North)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.East)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.UShape, Rot4.South)
				}
			},
			{
				SectionLayer_SubstructureProps.EdgeDirections.North | SectionLayer_SubstructureProps.EdgeDirections.East | SectionLayer_SubstructureProps.EdgeDirections.South | SectionLayer_SubstructureProps.EdgeDirections.West,
				new ValueTuple<CachedMaterial, Rot4>[]
				{
					new ValueTuple<CachedMaterial, Rot4>(SectionLayer_SubstructureProps.OShape, Rot4.North)
				}
			}
		};

		// Token: 0x02001F86 RID: 8070
		[Flags]
		private enum EdgeDirections
		{
			// Token: 0x04007A1C RID: 31260
			None = 0,
			// Token: 0x04007A1D RID: 31261
			North = 1,
			// Token: 0x04007A1E RID: 31262
			East = 2,
			// Token: 0x04007A1F RID: 31263
			South = 4,
			// Token: 0x04007A20 RID: 31264
			West = 8
		}

		// Token: 0x02001F87 RID: 8071
		[Flags]
		private enum CornerDirections
		{
			// Token: 0x04007A22 RID: 31266
			None = 0,
			// Token: 0x04007A23 RID: 31267
			SouthWest = 1,
			// Token: 0x04007A24 RID: 31268
			NorthWest = 2,
			// Token: 0x04007A25 RID: 31269
			NorthEast = 4,
			// Token: 0x04007A26 RID: 31270
			SouthEast = 8
		}
	}
}
