using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000492 RID: 1170
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x06001E8E RID: 7822 RVA: 0x000A06EC File Offset: 0x0009E8EC
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000A0705 File Offset: 0x0009E905
		public override Material GetMaterialFor(CellTerrain terrain)
		{
			return terrain.def.waterDepthMaterial;
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000A0714 File Offset: 0x0009E914
		public override void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, SubcameraDefOf.WaterDepth.LayerId);
				}
			}
		}
	}
}
