using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047D RID: 1149
	public class SectionLayer_GravshipMask : SectionLayer
	{
		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0009C0EB File Offset: 0x0009A2EB
		// (set) Token: 0x06001E22 RID: 7714 RVA: 0x0009C0F2 File Offset: 0x0009A2F2
		public static Building_GravEngine Engine { get; set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x0009C0FA File Offset: 0x0009A2FA
		// (set) Token: 0x06001E24 RID: 7716 RVA: 0x0009C101 File Offset: 0x0009A301
		public static SectionLayer_GravshipMask.MaskOverrideMode OverrideMode { get; set; }

		// Token: 0x06001E25 RID: 7717 RVA: 0x0009C109 File Offset: 0x0009A309
		public SectionLayer_GravshipMask(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.None;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0009C122 File Offset: 0x0009A322
		public static void ResetStaticData()
		{
			SectionLayer_GravshipMask.Engine = null;
			SectionLayer_GravshipMask.OverrideMode = SectionLayer_GravshipMask.MaskOverrideMode.None;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x0009C130 File Offset: 0x0009A330
		public static LayerSubMesh BakeGravshipShadowMask(Map map, Material mat, IEnumerable<IntVec3> cells)
		{
			LayerSubMesh layerSubMesh = MapDrawLayer.CreateFreeSubMesh(mat, null);
			SectionLayer_GravshipMask.RegenerateGravshipShadowMask(map, layerSubMesh, cells);
			if (layerSubMesh.verts.Count > 0)
			{
				layerSubMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
			return layerSubMesh;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0009C163 File Offset: 0x0009A363
		public static bool IsValidSubstructure(IntVec3 c)
		{
			return SectionLayer_GravshipMask.Engine != null && SectionLayer_GravshipMask.Engine.Spawned && SectionLayer_GravshipMask.Engine.ValidSubstructureNoRegen != null && SectionLayer_GravshipMask.Engine.ValidSubstructureNoRegen.Contains(c);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0009C198 File Offset: 0x0009A398
		public static LayerSubMesh BakeDummyShadowMask(Material mat, Vector3 origin, float width, float height)
		{
			LayerSubMesh layerSubMesh = MapDrawLayer.CreateFreeSubMesh(mat, null);
			Vector3 vector = new Vector3(width * 0.5f, 0f, height * 0.5f);
			layerSubMesh.verts.Add(origin - vector);
			layerSubMesh.verts.Add(origin + new Vector3(-vector.x, 0f, vector.z));
			layerSubMesh.verts.Add(origin + vector);
			layerSubMesh.verts.Add(origin + new Vector3(vector.x, 0f, -vector.z));
			int count = layerSubMesh.verts.Count;
			layerSubMesh.tris.Add(count - 4);
			layerSubMesh.tris.Add(count - 3);
			layerSubMesh.tris.Add(count - 2);
			layerSubMesh.tris.Add(count - 4);
			layerSubMesh.tris.Add(count - 2);
			layerSubMesh.tris.Add(count - 1);
			layerSubMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			return layerSubMesh;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0009C2A4 File Offset: 0x0009A4A4
		private void RegenerateGravshipMask(LayerSubMesh subMesh)
		{
			Building_GravEngine engine = SectionLayer_GravshipMask.Engine;
			if (engine == null || !engine.Spawned)
			{
				return;
			}
			HashSet<IntVec3> validSubstructure = SectionLayer_GravshipMask.Engine.ValidSubstructure;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				if (validSubstructure.Contains(intVec))
				{
					SectionLayer_IndoorMask.AppendQuadToMesh(subMesh, (float)intVec.x, (float)intVec.z, 0f);
				}
			}
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x0009C33C File Offset: 0x0009A53C
		private static void RegenerateGravshipShadowMask(Map map, LayerSubMesh subMesh, IEnumerable<IntVec3> cells)
		{
			string defName = TerrainDefOf.Space.defName;
			foreach (IntVec3 intVec in cells)
			{
				if (intVec.InBounds(map) && map.terrainGrid.TerrainAt(intVec).defName != defName)
				{
					SectionLayer_IndoorMask.AppendQuadToMesh(subMesh, (float)intVec.x, (float)intVec.z, 0f);
				}
			}
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0009C3C4 File Offset: 0x0009A5C4
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.GravshipMask);
			subMesh.Clear(MeshParts.All);
			switch (SectionLayer_GravshipMask.OverrideMode)
			{
			case SectionLayer_GravshipMask.MaskOverrideMode.None:
				if (WorldComponent_GravshipController.CutsceneInProgress || Find.GravshipController.IsGravshipTravelling)
				{
					SectionLayer_GravshipMask.RegenerateGravshipShadowMask(base.Map, subMesh, this.section.CellRect);
				}
				else
				{
					this.RegenerateGravshipMask(subMesh);
				}
				break;
			case SectionLayer_GravshipMask.MaskOverrideMode.Shadow:
				SectionLayer_GravshipMask.RegenerateGravshipShadowMask(base.Map, subMesh, this.section.CellRect);
				break;
			case SectionLayer_GravshipMask.MaskOverrideMode.Gravship:
				this.RegenerateGravshipMask(subMesh);
				break;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
		}

		// Token: 0x0200047E RID: 1150
		public enum MaskOverrideMode
		{
			// Token: 0x04001A0C RID: 6668
			None,
			// Token: 0x04001A0D RID: 6669
			Shadow,
			// Token: 0x04001A0E RID: 6670
			Gravship
		}
	}
}
