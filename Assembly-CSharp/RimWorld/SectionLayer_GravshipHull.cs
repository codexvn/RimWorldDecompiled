using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F82 RID: 8066
	public class SectionLayer_GravshipHull : SectionLayer
	{
		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x0600B23E RID: 45630 RVA: 0x0032F72C File Offset: 0x0032D92C
		private static Shader WallShader
		{
			get
			{
				ShaderTypeDef shaderTypeDef = SectionLayer_GravshipHull.wallShader;
				return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutOverlay;
			}
		}

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x0600B23F RID: 45631 RVA: 0x0032F748 File Offset: 0x0032D948
		private static Shader SubstructureShader
		{
			get
			{
				ShaderTypeDef shaderTypeDef = SectionLayer_GravshipHull.substructureShader;
				return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.Transparent;
			}
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x0600B240 RID: 45632 RVA: 0x0032F764 File Offset: 0x0032D964
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive;
			}
		}

		// Token: 0x0600B241 RID: 45633 RVA: 0x0032F76B File Offset: 0x0032D96B
		public SectionLayer_GravshipHull(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Terrain | MapMeshFlagDefOf.Things | MapMeshFlagDefOf.Roofs);
		}

		// Token: 0x0600B242 RID: 45634 RVA: 0x0032F7A8 File Offset: 0x0032D9A8
		public static List<LayerSubMesh> BakeGravshipIndoorMesh(Map map, CellRect bounds, Vector3 center)
		{
			Dictionary<SectionLayer_GravshipHull.CornerType, LayerSubMesh> dictionary = new Dictionary<SectionLayer_GravshipHull.CornerType, LayerSubMesh>();
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 intVec in bounds)
			{
				SectionLayer_GravshipHull.CornerType cornerType;
				Color color;
				if (SectionLayer_GravshipHull.ShouldDrawCornerPiece(intVec, map, terrainGrid, out cornerType, out color) && SectionLayer_GravshipHull.IsCornerSubstructure(intVec, cornerType) && SectionLayer_GravshipHull.IsCornerIndoorMasked(intVec, cornerType, map))
				{
					Material material = SectionLayer_GravshipHull.GetMaterial(cornerType).Material;
					Texture2D srcTex = material.mainTexture as Texture2D;
					Color color2 = material.color;
					Material material2 = MaterialPool.MatFrom(srcTex, ShaderDatabase.IndoorMaskMasked, color2, 3185);
					IntVec3 offset = SectionLayer_GravshipHull.GetOffset(cornerType);
					LayerSubMesh sm;
					if (!dictionary.TryGetValue(cornerType, out sm))
					{
						dictionary.Add(cornerType, sm = MapDrawLayer.CreateFreeSubMesh(material2, map));
					}
					SectionLayer_GravshipHull.AddQuad(sm, (intVec + offset).ToVector3() - center, SectionLayer_GravshipHull.HullCornerScale, SectionLayer_GravshipHull.bakedAltitude, color);
				}
			}
			foreach (LayerSubMesh layerSubMesh in dictionary.Values)
			{
				layerSubMesh.FinalizeMesh(MeshParts.All);
			}
			return dictionary.Values.ToList<LayerSubMesh>();
		}

		// Token: 0x0600B243 RID: 45635 RVA: 0x0032F904 File Offset: 0x0032DB04
		public override void Regenerate()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				SectionLayer_GravshipHull.CornerType cornerType;
				Color color;
				if (SectionLayer_GravshipHull.ShouldDrawCornerPiece(intVec, map, terrainGrid, out cornerType, out color))
				{
					CachedMaterial material = SectionLayer_GravshipHull.GetMaterial(cornerType);
					IntVec3 offset = SectionLayer_GravshipHull.GetOffset(cornerType);
					bool addGravshipMask = SectionLayer_GravshipHull.IsCornerSubstructure(intVec, cornerType);
					bool addIndoorMask = SectionLayer_GravshipHull.IsCornerIndoorMasked(intVec, cornerType, map);
					this.AddQuad(material.Material, intVec + offset, SectionLayer_GravshipHull.HullCornerScale, SectionLayer_GravshipHull.cornerAltitude, color, addGravshipMask, addIndoorMask);
					TerrainDef terrainDef = terrainGrid.FoundationAt(intVec + IntVec3.South);
					bool substructureToSouth = terrainDef != null && terrainDef.IsSubstructure;
					this.AddSubstructure(cornerType, intVec, substructureToSouth, addGravshipMask, addIndoorMask);
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x0600B244 RID: 45636 RVA: 0x0032FA1C File Offset: 0x0032DC1C
		private static void EnsureInitialized()
		{
			if (SectionLayer_GravshipHull.initalized)
			{
				return;
			}
			SectionLayer_GravshipHull.initalized = true;
			SectionLayer_GravshipHull.mat_Corner_NW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_northwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Corner_NE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_northeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Corner_SW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_southwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Corner_SE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_southeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_NW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_NE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_SW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_SE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_SubStructure_W = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Full_west", SectionLayer_GravshipHull.SubstructureShader);
			SectionLayer_GravshipHull.mat_SubStructure_E = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Full_east", SectionLayer_GravshipHull.SubstructureShader);
			SectionLayer_GravshipHull.mat_SubStructureExtra_W = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_west", SectionLayer_GravshipHull.SubstructureShader);
			SectionLayer_GravshipHull.mat_SubStructureExtra_E = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_east", SectionLayer_GravshipHull.SubstructureShader);
		}

		// Token: 0x0600B245 RID: 45637 RVA: 0x0032FB27 File Offset: 0x0032DD27
		private static bool IsIndoorMasked(IntVec3 c, Map map)
		{
			return c.Roofed(map);
		}

		// Token: 0x0600B246 RID: 45638 RVA: 0x0032FB30 File Offset: 0x0032DD30
		private static bool IsCornerSubstructure(IntVec3 c, SectionLayer_GravshipHull.CornerType cornerType)
		{
			switch (cornerType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.North) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.West);
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.North) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.East);
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.South) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.West);
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.South) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.East);
			default:
				return false;
			}
		}

		// Token: 0x0600B247 RID: 45639 RVA: 0x0032FC00 File Offset: 0x0032DE00
		private static bool IsCornerIndoorMasked(IntVec3 c, SectionLayer_GravshipHull.CornerType cornerType, Map map)
		{
			switch (cornerType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.North, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.West, map);
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.North, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.East, map);
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.South, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.West, map);
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.South, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.East, map);
			default:
				return false;
			}
		}

		// Token: 0x0600B248 RID: 45640 RVA: 0x0032FCD8 File Offset: 0x0032DED8
		private static CachedMaterial GetMaterial(SectionLayer_GravshipHull.CornerType edgeType)
		{
			SectionLayer_GravshipHull.EnsureInitialized();
			CachedMaterial result;
			switch (edgeType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
				result = SectionLayer_GravshipHull.mat_Corner_NW;
				break;
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
				result = SectionLayer_GravshipHull.mat_Corner_NE;
				break;
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
				result = SectionLayer_GravshipHull.mat_Corner_SW;
				break;
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
				result = SectionLayer_GravshipHull.mat_Corner_SE;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				result = SectionLayer_GravshipHull.mat_Diagonal_NW;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				result = SectionLayer_GravshipHull.mat_Diagonal_NE;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				result = SectionLayer_GravshipHull.mat_Diagonal_SW;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				result = SectionLayer_GravshipHull.mat_Diagonal_SE;
				break;
			default:
				throw new ArgumentOutOfRangeException("edgeType", edgeType, null);
			}
			return result;
		}

		// Token: 0x0600B249 RID: 45641 RVA: 0x0032FD68 File Offset: 0x0032DF68
		private static IntVec3 GetOffset(SectionLayer_GravshipHull.CornerType cornerType)
		{
			switch (cornerType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				return new IntVec3(-1, 0, 0);
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				return new IntVec3(0, 0, 0);
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				return new IntVec3(-1, 0, -1);
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				return new IntVec3(0, 0, -1);
			default:
				return IntVec3.Zero;
			}
		}

		// Token: 0x0600B24A RID: 45642 RVA: 0x0032FDC8 File Offset: 0x0032DFC8
		private static void AddQuad(LayerSubMesh sm, Vector3 c, float scale, float altitude, Color color)
		{
			int count = sm.verts.Count;
			for (int i = 0; i < 4; i++)
			{
				sm.verts.Add(new Vector3(c.x + SectionLayer_GravshipHull.UVs[i].x * scale, altitude, c.z + SectionLayer_GravshipHull.UVs[i].y * scale));
				sm.uvs.Add(SectionLayer_GravshipHull.UVs[i % 4]);
				sm.colors.Add(color);
			}
			sm.tris.Add(count);
			sm.tris.Add(count + 1);
			sm.tris.Add(count + 2);
			sm.tris.Add(count);
			sm.tris.Add(count + 2);
			sm.tris.Add(count + 3);
		}

		// Token: 0x0600B24B RID: 45643 RVA: 0x0032FEB0 File Offset: 0x0032E0B0
		private void AddQuad(Material mat, IntVec3 c, float scale, float altitude, Color color, bool addGravshipMask, bool addIndoorMask)
		{
			LayerSubMesh subMesh = base.GetSubMesh(mat);
			SectionLayer_GravshipHull.AddQuad(subMesh, c.ToVector3(), scale, altitude, color);
			if (addGravshipMask)
			{
				Texture2D srcTex = subMesh.material.mainTexture as Texture2D;
				Color color2 = subMesh.material.color;
				Material material = MaterialPool.MatFrom(srcTex, ShaderDatabase.GravshipMaskMasked, color2);
				SectionLayer_GravshipHull.AddQuad(base.GetSubMesh(material), c.ToVector3(), scale, altitude, color);
			}
			if (addIndoorMask)
			{
				Texture2D srcTex2 = subMesh.material.mainTexture as Texture2D;
				Color color3 = subMesh.material.color;
				Material material2 = MaterialPool.MatFrom(srcTex2, ShaderDatabase.IndoorMaskMasked, color3);
				SectionLayer_GravshipHull.AddQuad(base.GetSubMesh(material2), c.ToVector3(), scale, altitude, color);
			}
		}

		// Token: 0x0600B24C RID: 45644 RVA: 0x0032FF6C File Offset: 0x0032E16C
		private void AddSubstructure(SectionLayer_GravshipHull.CornerType cornerType, IntVec3 c, bool substructureToSouth, bool addGravshipMask, bool addIndoorMask)
		{
			if (cornerType == SectionLayer_GravshipHull.CornerType.Corner_NW || cornerType == SectionLayer_GravshipHull.CornerType.Diagonal_NW)
			{
				this.AddQuad(SectionLayer_GravshipHull.mat_SubStructure_W.Material, c, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				if (!substructureToSouth)
				{
					this.AddQuad(SectionLayer_GravshipHull.mat_SubStructureExtra_W.Material, c + IntVec3.South, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				}
			}
			if (cornerType == SectionLayer_GravshipHull.CornerType.Corner_NE || cornerType == SectionLayer_GravshipHull.CornerType.Diagonal_NE)
			{
				this.AddQuad(SectionLayer_GravshipHull.mat_SubStructure_E.Material, c, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				if (!substructureToSouth)
				{
					this.AddQuad(SectionLayer_GravshipHull.mat_SubStructureExtra_E.Material, c + IntVec3.South, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				}
			}
		}

		// Token: 0x0600B24D RID: 45645 RVA: 0x00330034 File Offset: 0x0032E234
		public static bool ShouldDrawCornerPiece(IntVec3 pos, Map map, TerrainGrid terrGrid, out SectionLayer_GravshipHull.CornerType cornerType, out Color color)
		{
			cornerType = SectionLayer_GravshipHull.CornerType.None;
			color = Color.white;
			if (pos.GetEdifice(map) != null)
			{
				return false;
			}
			TerrainDef terrainDef = terrGrid.FoundationAt(pos);
			if (terrainDef != null && terrainDef.IsSubstructure)
			{
				return false;
			}
			for (int k = 0; k < SectionLayer_GravshipHull.Directions.Length; k++)
			{
				bool[] array = SectionLayer_GravshipHull.tmpChecks;
				int num = k;
				Building edificeSafe = (pos + SectionLayer_GravshipHull.Directions[k]).GetEdificeSafe(map);
				array[num] = (((edificeSafe != null) ? edificeSafe.def : null) == ThingDefOf.GravshipHull);
			}
			if (SectionLayer_GravshipHull.tmpChecks[0] && SectionLayer_GravshipHull.tmpChecks[3] && !SectionLayer_GravshipHull.tmpChecks[2] && !SectionLayer_GravshipHull.tmpChecks[1])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[4] ? SectionLayer_GravshipHull.CornerType.Corner_NW : SectionLayer_GravshipHull.CornerType.Diagonal_NW);
			}
			else if (SectionLayer_GravshipHull.tmpChecks[0] && SectionLayer_GravshipHull.tmpChecks[1] && !SectionLayer_GravshipHull.tmpChecks[2] && !SectionLayer_GravshipHull.tmpChecks[3])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[5] ? SectionLayer_GravshipHull.CornerType.Corner_NE : SectionLayer_GravshipHull.CornerType.Diagonal_NE);
			}
			else if (SectionLayer_GravshipHull.tmpChecks[2] && SectionLayer_GravshipHull.tmpChecks[1] && !SectionLayer_GravshipHull.tmpChecks[0] && !SectionLayer_GravshipHull.tmpChecks[3])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[6] ? SectionLayer_GravshipHull.CornerType.Corner_SE : SectionLayer_GravshipHull.CornerType.Diagonal_SE);
			}
			else if (SectionLayer_GravshipHull.tmpChecks[2] && SectionLayer_GravshipHull.tmpChecks[3] && !SectionLayer_GravshipHull.tmpChecks[0] && !SectionLayer_GravshipHull.tmpChecks[1])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[7] ? SectionLayer_GravshipHull.CornerType.Corner_SW : SectionLayer_GravshipHull.CornerType.Diagonal_SW);
			}
			if (cornerType == SectionLayer_GravshipHull.CornerType.None)
			{
				return false;
			}
			int[][] array2 = SectionLayer_GravshipHull.directionPairs;
			for (int j = 0; j < array2.Length; j++)
			{
				List<int> list = (from i in array2[j]
				where SectionLayer_GravshipHull.tmpChecks[i]
				select i).ToList<int>();
				if (list.Count > 0)
				{
					int num2 = list.First<int>();
					color = (pos + SectionLayer_GravshipHull.Directions[num2]).GetEdificeSafe(map).DrawColor;
					break;
				}
			}
			return true;
		}

		// Token: 0x040079E3 RID: 31203
		private static readonly Vector2[] UVs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x040079E4 RID: 31204
		[TweakValue("HullCorners", 0f, 2f)]
		private static float HullCornerScale = 2f;

		// Token: 0x040079E5 RID: 31205
		private const string TexPath_Corner_NW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_northwest";

		// Token: 0x040079E6 RID: 31206
		private const string TexPath_Corner_NE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_northeast";

		// Token: 0x040079E7 RID: 31207
		private const string TexPath_Corner_SW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_southwest";

		// Token: 0x040079E8 RID: 31208
		private const string TexPath_Corner_SE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_southeast";

		// Token: 0x040079E9 RID: 31209
		private const string TexPath_Diagonal_NW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northwest";

		// Token: 0x040079EA RID: 31210
		private const string TexPath_Diagonal_NE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northeast";

		// Token: 0x040079EB RID: 31211
		private const string TexPath_Diagonal_SW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southwest";

		// Token: 0x040079EC RID: 31212
		private const string TexPath_Diagonal_SE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southeast";

		// Token: 0x040079ED RID: 31213
		private const string TexPath_SubStructure_W = "Things/Building/Linked/GravshipHull/SubstructureCorner_Full_west";

		// Token: 0x040079EE RID: 31214
		private const string TexPath_SubStructure_E = "Things/Building/Linked/GravshipHull/SubstructureCorner_Full_east";

		// Token: 0x040079EF RID: 31215
		private const string TexPath_SubStructureExtra_W = "Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_west";

		// Token: 0x040079F0 RID: 31216
		private const string TexPath_SubStructureExtra_E = "Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_east";

		// Token: 0x040079F1 RID: 31217
		private const int BakedIndoorMaskRenderQueue = 3185;

		// Token: 0x040079F2 RID: 31218
		private static CachedMaterial mat_Corner_NW;

		// Token: 0x040079F3 RID: 31219
		private static CachedMaterial mat_Corner_NE;

		// Token: 0x040079F4 RID: 31220
		private static CachedMaterial mat_Corner_SW;

		// Token: 0x040079F5 RID: 31221
		private static CachedMaterial mat_Corner_SE;

		// Token: 0x040079F6 RID: 31222
		private static CachedMaterial mat_Diagonal_NW;

		// Token: 0x040079F7 RID: 31223
		private static CachedMaterial mat_Diagonal_NE;

		// Token: 0x040079F8 RID: 31224
		private static CachedMaterial mat_Diagonal_SW;

		// Token: 0x040079F9 RID: 31225
		private static CachedMaterial mat_Diagonal_SE;

		// Token: 0x040079FA RID: 31226
		private static CachedMaterial mat_SubStructure_W;

		// Token: 0x040079FB RID: 31227
		private static CachedMaterial mat_SubStructure_E;

		// Token: 0x040079FC RID: 31228
		private static CachedMaterial mat_SubStructureExtra_W;

		// Token: 0x040079FD RID: 31229
		private static CachedMaterial mat_SubStructureExtra_E;

		// Token: 0x040079FE RID: 31230
		private static readonly float cornerAltitude = AltitudeLayer.BuildingOnTop.AltitudeFor();

		// Token: 0x040079FF RID: 31231
		private static readonly float substructureAltitude = AltitudeLayer.TerrainEdges.AltitudeFor();

		// Token: 0x04007A00 RID: 31232
		private static readonly float bakedAltitude = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x04007A01 RID: 31233
		private static bool initalized;

		// Token: 0x04007A02 RID: 31234
		private static ShaderTypeDef wallShader;

		// Token: 0x04007A03 RID: 31235
		private static ShaderTypeDef substructureShader;

		// Token: 0x04007A04 RID: 31236
		private static readonly IntVec3[] Directions = new IntVec3[]
		{
			IntVec3.North,
			IntVec3.East,
			IntVec3.South,
			IntVec3.West,
			IntVec3.North + IntVec3.West,
			IntVec3.North + IntVec3.East,
			IntVec3.South + IntVec3.East,
			IntVec3.South + IntVec3.West
		};

		// Token: 0x04007A05 RID: 31237
		private static readonly int[][] directionPairs = new int[][]
		{
			new int[]
			{
				0,
				2
			},
			new int[]
			{
				1,
				3
			},
			new int[]
			{
				4,
				6
			},
			new int[]
			{
				5,
				7
			}
		};

		// Token: 0x04007A06 RID: 31238
		private static bool[] tmpChecks = new bool[SectionLayer_GravshipHull.Directions.Length];

		// Token: 0x02001F83 RID: 8067
		public enum CornerType
		{
			// Token: 0x04007A08 RID: 31240
			None,
			// Token: 0x04007A09 RID: 31241
			Corner_NW,
			// Token: 0x04007A0A RID: 31242
			Corner_NE,
			// Token: 0x04007A0B RID: 31243
			Corner_SW,
			// Token: 0x04007A0C RID: 31244
			Corner_SE,
			// Token: 0x04007A0D RID: 31245
			Diagonal_NW,
			// Token: 0x04007A0E RID: 31246
			Diagonal_NE,
			// Token: 0x04007A0F RID: 31247
			Diagonal_SW,
			// Token: 0x04007A10 RID: 31248
			Diagonal_SE
		}
	}
}
