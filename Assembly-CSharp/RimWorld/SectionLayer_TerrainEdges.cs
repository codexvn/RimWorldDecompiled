using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F88 RID: 8072
	public class SectionLayer_TerrainEdges : SectionLayer
	{
		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x0600B25A RID: 45658 RVA: 0x0009EBDD File Offset: 0x0009CDDD
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x0600B25B RID: 45659 RVA: 0x0009EBE4 File Offset: 0x0009CDE4
		public SectionLayer_TerrainEdges(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x0600B25C RID: 45660 RVA: 0x00330C30 File Offset: 0x0032EE30
		public override void Regenerate()
		{
			if (base.Map.Tile.Valid && !base.Map.Tile.LayerDef.isSpace)
			{
				return;
			}
			base.ClearSubMeshes(MeshParts.All);
			TerrainGrid terrainGrid = base.Map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float altitude = AltitudeLayer.TerrainScatter.AltitudeFor();
			float altitude2 = AltitudeLayer.TerrainEdges.AltitudeFor();
			foreach (IntVec3 intVec in cellRect)
			{
				SectionLayer_TerrainEdges.EdgeDirections edgeDirections;
				SectionLayer_TerrainEdges.CornerDirections corners;
				if (this.ShouldDrawRockEdges(intVec, terrainGrid, out edgeDirections, out corners))
				{
					TerrainDef terrain = terrainGrid.BaseTerrainAt(intVec);
					this.DrawEdges(terrain, intVec, edgeDirections, altitude);
					this.DrawCorners(terrain, intVec, edgeDirections, corners, altitude);
					if (this.ShouldDrawPassthrough(intVec, terrainGrid, out edgeDirections, out corners))
					{
						this.DrawLoop(intVec + IntVec3.South, terrainGrid, edgeDirections, corners, altitude2);
					}
				}
				else if (this.ShouldDrawLoop(intVec, terrainGrid, out edgeDirections, out corners))
				{
					this.DrawLoop(intVec, terrainGrid, edgeDirections, corners, altitude2);
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x0600B25D RID: 45661 RVA: 0x00330D64 File Offset: 0x0032EF64
		private void DrawEdges(TerrainDef terrain, IntVec3 c, SectionLayer_TerrainEdges.EdgeDirections edgeDirs, float altitude)
		{
			ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[] array;
			if (SectionLayer_TerrainEdges.EdgeMats.TryGetValue(edgeDirs, out array))
			{
				foreach (ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4> valueTuple in array)
				{
					SectionLayer_TerrainEdges.EdgeType item = valueTuple.Item1;
					Rot4 item2 = valueTuple.Item2;
					this.AddQuad(terrain, item, c, altitude, item2, 0);
				}
			}
		}

		// Token: 0x0600B25E RID: 45662 RVA: 0x00330DB0 File Offset: 0x0032EFB0
		private void DrawCorners(TerrainDef terrain, IntVec3 c, SectionLayer_TerrainEdges.EdgeDirections edges, SectionLayer_TerrainEdges.CornerDirections corners, float altitude)
		{
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.West))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.East, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.East))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.South, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.SouthEast) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.South) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.East))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.West, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.SouthWest) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.South) && !edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.West))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.CornerInner, c, altitude, Rot4.North, 0);
			}
		}

		// Token: 0x0600B25F RID: 45663 RVA: 0x00330EEC File Offset: 0x0032F0EC
		private void DrawLoop(IntVec3 c, TerrainGrid grid, SectionLayer_TerrainEdges.EdgeDirections edges, SectionLayer_TerrainEdges.CornerDirections corners, float altitude)
		{
			if (!edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North))
			{
				return;
			}
			TerrainDef terrain = grid.BaseTerrainAt(c + IntVec3.North);
			float num = (float)c.z / (float)base.Map.Size.z;
			altitude += 0.03658537f - num * 0.03658537f;
			if (!corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && !corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.LoopSingle, c + IntVec3.South, altitude, Rot4.North, 0);
			}
			if (corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.Loop, c + IntVec3.South, altitude, Rot4.North, c.GetHashCode());
			}
			if (!corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest) && corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.LoopLeft, c + IntVec3.South, altitude, Rot4.North, 0);
			}
			if (!corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthEast) && corners.HasFlag(SectionLayer_TerrainEdges.CornerDirections.NorthWest))
			{
				this.AddQuad(terrain, SectionLayer_TerrainEdges.EdgeType.LoopRight, c + IntVec3.South, altitude, Rot4.North, 0);
			}
		}

		// Token: 0x0600B260 RID: 45664 RVA: 0x00331064 File Offset: 0x0032F264
		private void AddQuad(TerrainDef terrain, SectionLayer_TerrainEdges.EdgeType edgeType, IntVec3 c, float altitude, Rot4 rotation, int listIndexOffset = 0)
		{
			Material material = terrain.spaceEdgeGraphicData.GetMaterial(terrain, edgeType, listIndexOffset);
			LayerSubMesh subMesh = base.GetSubMesh(material);
			int count = subMesh.verts.Count;
			float num = Mathf.Max((float)material.mainTexture.width / (float)material.mainTexture.height, 1f);
			float num2 = Mathf.Max((float)material.mainTexture.height / (float)material.mainTexture.width, 1f);
			int num3 = Mathf.Abs(4 - rotation.AsInt);
			for (int i = 0; i < 4; i++)
			{
				subMesh.verts.Add(new Vector3((float)c.x + SectionLayer_TerrainEdges.UVs[i].x * num, altitude, (float)c.z + SectionLayer_TerrainEdges.UVs[i].y * num2));
				subMesh.uvs.Add(SectionLayer_TerrainEdges.UVs[(num3 + i) % 4]);
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x0600B261 RID: 45665 RVA: 0x003311B8 File Offset: 0x0032F3B8
		private bool ShouldDrawRockEdges(IntVec3 c, TerrainGrid grid, out SectionLayer_TerrainEdges.EdgeDirections edges, out SectionLayer_TerrainEdges.CornerDirections corners)
		{
			edges = SectionLayer_TerrainEdges.EdgeDirections.None;
			corners = SectionLayer_TerrainEdges.CornerDirections.None;
			TerrainDef terrainDef = grid.BaseTerrainAt(c);
			if (terrainDef == null || terrainDef.spaceEdgeGraphicData == null)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				if (!c2.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
				else
				{
					TerrainDef terrainDef2 = grid.TerrainAt(c2);
					if (terrainDef2 == null || terrainDef2.dontRender)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
			}
			for (int j = 0; j < GenAdj.DiagonalDirections.Length; j++)
			{
				IntVec3 c3 = c + GenAdj.DiagonalDirections[j];
				if (!c3.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << j);
					}
				}
				else
				{
					TerrainDef terrainDef3 = grid.TerrainAt(c3);
					if (terrainDef3 == null || terrainDef3.dontRender)
					{
						corners |= (SectionLayer_TerrainEdges.CornerDirections)(1 << j);
					}
				}
			}
			return edges != SectionLayer_TerrainEdges.EdgeDirections.None || corners > SectionLayer_TerrainEdges.CornerDirections.None;
		}

		// Token: 0x0600B262 RID: 45666 RVA: 0x003312D0 File Offset: 0x0032F4D0
		private bool ShouldDrawPassthrough(IntVec3 c, TerrainGrid grid, out SectionLayer_TerrainEdges.EdgeDirections edges, out SectionLayer_TerrainEdges.CornerDirections corners)
		{
			edges = SectionLayer_TerrainEdges.EdgeDirections.None;
			corners = SectionLayer_TerrainEdges.CornerDirections.None;
			IntVec3 c2 = c + IntVec3.North;
			if (!c2.InBounds(base.Map))
			{
				return false;
			}
			TerrainDef terrainDef = grid.BaseTerrainAt(c2);
			if (terrainDef == null || terrainDef.spaceEdgeGraphicData == null)
			{
				return false;
			}
			IntVec3 c3 = c + IntVec3.West;
			IntVec3 c4 = c + IntVec3.East;
			if (!c3.InBounds(base.Map) || !c4.InBounds(base.Map))
			{
				return false;
			}
			TerrainDef terrainDef2 = grid.TerrainAt(c3);
			TerrainDef terrainDef3 = grid.TerrainAt(c4);
			if ((terrainDef3 != null && terrainDef3.dontRender) || (terrainDef2 != null && terrainDef2.dontRender))
			{
				return false;
			}
			corners = (SectionLayer_TerrainEdges.CornerDirections.NorthWest | SectionLayer_TerrainEdges.CornerDirections.NorthEast);
			edges = SectionLayer_TerrainEdges.EdgeDirections.North;
			return true;
		}

		// Token: 0x0600B263 RID: 45667 RVA: 0x00331384 File Offset: 0x0032F584
		private bool ShouldDrawLoop(IntVec3 c, TerrainGrid grid, out SectionLayer_TerrainEdges.EdgeDirections edges, out SectionLayer_TerrainEdges.CornerDirections corners)
		{
			edges = SectionLayer_TerrainEdges.EdgeDirections.None;
			corners = SectionLayer_TerrainEdges.CornerDirections.None;
			IntVec3 c2 = c + IntVec3.North;
			TerrainDef terrainDef = grid.TerrainAt(c);
			if ((terrainDef != null && !terrainDef.dontRender) || !c2.InBounds(base.Map))
			{
				return false;
			}
			TerrainDef terrainDef2 = grid.BaseTerrainAt(c2);
			if (terrainDef2 == null || terrainDef2.spaceEdgeGraphicData == null)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c3 = c + GenAdj.CardinalDirections[i];
				if (!c3.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
				else
				{
					TerrainDef terrainDef3 = grid.TerrainAt(c3);
					if (terrainDef3 != null && !terrainDef3.dontRender)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << i);
					}
				}
			}
			for (int j = 0; j < GenAdj.DiagonalDirections.Length; j++)
			{
				IntVec3 c4 = c + GenAdj.DiagonalDirections[j];
				if (!c4.InBounds(base.Map))
				{
					if (!base.Map.DrawMapClippers)
					{
						edges |= (SectionLayer_TerrainEdges.EdgeDirections)(1 << j);
					}
				}
				else
				{
					TerrainDef terrainDef4 = grid.TerrainAt(c4);
					if (terrainDef4 != null && !terrainDef4.dontRender)
					{
						corners |= (SectionLayer_TerrainEdges.CornerDirections)(1 << j);
					}
				}
			}
			return edges.HasFlag(SectionLayer_TerrainEdges.EdgeDirections.North);
		}

		// Token: 0x04007A27 RID: 31271
		private static readonly Vector2[] UVs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x04007A28 RID: 31272
		private static readonly Dictionary<SectionLayer_TerrainEdges.EdgeDirections, ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]> EdgeMats = new Dictionary<SectionLayer_TerrainEdges.EdgeDirections, ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]>
		{
			{
				SectionLayer_TerrainEdges.EdgeDirections.North,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.South)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.West)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.West)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.CornerOuter, Rot4.South)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.South),
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.West),
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.Flat, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.West)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.North)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.East)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.UShape, Rot4.South)
				}
			},
			{
				SectionLayer_TerrainEdges.EdgeDirections.North | SectionLayer_TerrainEdges.EdgeDirections.East | SectionLayer_TerrainEdges.EdgeDirections.South | SectionLayer_TerrainEdges.EdgeDirections.West,
				new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>[]
				{
					new ValueTuple<SectionLayer_TerrainEdges.EdgeType, Rot4>(SectionLayer_TerrainEdges.EdgeType.OShape, Rot4.North)
				}
			}
		};

		// Token: 0x02001F89 RID: 8073
		[Flags]
		private enum EdgeDirections
		{
			// Token: 0x04007A2A RID: 31274
			None = 0,
			// Token: 0x04007A2B RID: 31275
			North = 1,
			// Token: 0x04007A2C RID: 31276
			East = 2,
			// Token: 0x04007A2D RID: 31277
			South = 4,
			// Token: 0x04007A2E RID: 31278
			West = 8
		}

		// Token: 0x02001F8A RID: 8074
		[Flags]
		private enum CornerDirections
		{
			// Token: 0x04007A30 RID: 31280
			None = 0,
			// Token: 0x04007A31 RID: 31281
			SouthWest = 1,
			// Token: 0x04007A32 RID: 31282
			NorthWest = 2,
			// Token: 0x04007A33 RID: 31283
			NorthEast = 4,
			// Token: 0x04007A34 RID: 31284
			SouthEast = 8
		}

		// Token: 0x02001F8B RID: 8075
		public enum EdgeType
		{
			// Token: 0x04007A36 RID: 31286
			OShape,
			// Token: 0x04007A37 RID: 31287
			UShape,
			// Token: 0x04007A38 RID: 31288
			CornerInner,
			// Token: 0x04007A39 RID: 31289
			CornerOuter,
			// Token: 0x04007A3A RID: 31290
			Flat,
			// Token: 0x04007A3B RID: 31291
			LoopLeft,
			// Token: 0x04007A3C RID: 31292
			LoopRight,
			// Token: 0x04007A3D RID: 31293
			LoopSingle,
			// Token: 0x04007A3E RID: 31294
			Loop
		}
	}
}
