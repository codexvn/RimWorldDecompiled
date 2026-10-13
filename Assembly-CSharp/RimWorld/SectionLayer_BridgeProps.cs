using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F81 RID: 8065
	[StaticConstructorOnStartup]
	public class SectionLayer_BridgeProps : SectionLayer
	{
		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x0600B239 RID: 45625 RVA: 0x0009EBDD File Offset: 0x0009CDDD
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x0600B23A RID: 45626 RVA: 0x0032F3E7 File Offset: 0x0032D5E7
		public SectionLayer_BridgeProps(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x0600B23B RID: 45627 RVA: 0x0032F40C File Offset: 0x0032D60C
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float y = AltitudeLayer.TerrainScatter.AltitudeFor();
			foreach (IntVec3 intVec in cellRect)
			{
				if (this.ShouldDrawPropsBelow(intVec, terrainGrid))
				{
					ValueTuple<Material, Material> materials = this.GetMaterials(terrainGrid.FoundationAt(intVec));
					Material item = materials.Item1;
					Material item2 = materials.Item2;
					IntVec3 c = intVec;
					c.x++;
					Material material;
					if (c.InBounds(map) && this.ShouldDrawPropsBelow(c, terrainGrid))
					{
						material = item;
					}
					else
					{
						material = item2;
					}
					LayerSubMesh subMesh = base.GetSubMesh(material);
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)(intVec.z - 1)));
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)(intVec.z - 1)));
					subMesh.uvs.Add(new Vector2(0f, 0f));
					subMesh.uvs.Add(new Vector2(0f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 0f));
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x0600B23C RID: 45628 RVA: 0x0032F668 File Offset: 0x0032D868
		public ValueTuple<Material, Material> GetMaterials(TerrainDef def)
		{
			if (!this.propsMatCache.ContainsKey(def))
			{
				this.propsMatCache[def] = new ValueTuple<Material, Material>(def.bridgePropsLoopGraphic.MatSingle, def.bridgePropsRightGraphic.MatSingle);
			}
			return this.propsMatCache[def];
		}

		// Token: 0x0600B23D RID: 45629 RVA: 0x0032F6B8 File Offset: 0x0032D8B8
		private bool ShouldDrawPropsBelow(IntVec3 c, TerrainGrid terrGrid)
		{
			TerrainDef terrainDef = terrGrid.FoundationAt(c);
			if (terrainDef == null || !terrainDef.bridge)
			{
				return false;
			}
			IntVec3 c2 = c;
			c2.z--;
			Map map = base.Map;
			if (!c2.InBounds(map))
			{
				return false;
			}
			TerrainDef terrainDef2 = terrGrid.TerrainAt(c2);
			return terrGrid.FoundationAt(c2) == null && (terrainDef2.passability == Traversability.Impassable || c2.SupportsStructureType(map, TerrainDefOf.Bridge.terrainAffordanceNeeded));
		}

		// Token: 0x040079E2 RID: 31202
		private Dictionary<TerrainDef, ValueTuple<Material, Material>> propsMatCache = new Dictionary<TerrainDef, ValueTuple<Material, Material>>();
	}
}
