using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000481 RID: 1153
	public class SectionLayer_Plans : SectionLayer
	{
		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0009AAB7 File Offset: 0x00098CB7
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldOverlays;
			}
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0009D2EB File Offset: 0x0009B4EB
		public SectionLayer_Plans(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Plan;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0009D304 File Offset: 0x0009B504
		public override void DrawLayer()
		{
			if (!Find.ScreenshotModeHandler.Active)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0009D318 File Offset: 0x0009B518
		public override void Regenerate()
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			PlanManager planManager = base.Map.planManager;
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			base.ClearSubMeshes(MeshParts.All);
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					Plan plan = planManager.PlanAt(new IntVec3(i, 0, j));
					if (plan != null && !plan.Hidden)
					{
						LayerSubMesh subMesh = base.GetSubMesh(plan.Material);
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
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
			}
			base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.UVs1);
		}
	}
}
