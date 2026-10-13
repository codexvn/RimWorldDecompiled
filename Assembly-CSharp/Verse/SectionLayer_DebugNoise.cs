using System;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000477 RID: 1143
	[StaticConstructorOnStartup]
	internal class SectionLayer_DebugNoise : SectionLayer
	{
		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001E03 RID: 7683 RVA: 0x0009AAB7 File Offset: 0x00098CB7
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldOverlays;
			}
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0009AAC0 File Offset: 0x00098CC0
		public SectionLayer_DebugNoise(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.None;
			SectionLayer_DebugNoise.material = (SectionLayer_DebugNoise.material ? SectionLayer_DebugNoise.material : new Material(ShaderDatabase.VertexColor));
			SectionLayer_DebugNoise.material.renderQueue = 3800;
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0009AB15 File Offset: 0x00098D15
		private bool TryGetWindow(out Dialog_DevNoiseMap window)
		{
			return Find.WindowStack.TryGetWindow<Dialog_DevNoiseMap>(out window);
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0009AB24 File Offset: 0x00098D24
		public override void DrawLayer()
		{
			Dialog_DevNoiseMap dialog_DevNoiseMap;
			if (!this.TryGetWindow(out dialog_DevNoiseMap) || dialog_DevNoiseMap.alpha <= 0f)
			{
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x0009AB50 File Offset: 0x00098D50
		public override void Regenerate()
		{
			float y = AltitudeLayer.Zone.AltitudeFor();
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			base.ClearSubMeshes(MeshParts.All);
			Dialog_DevNoiseMap dialog_DevNoiseMap;
			if (!this.TryGetWindow(out dialog_DevNoiseMap))
			{
				base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
				return;
			}
			byte a = (byte)Mathf.Clamp(Mathf.RoundToInt(dialog_DevNoiseMap.alpha * 255.999f), 0, 255);
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					byte b = (byte)Mathf.Clamp(Mathf.RoundToInt((float)dialog_DevNoiseMap.noise.GetValue((double)i, 0.0, (double)j) * 255.999f), 0, 255);
					Color32 item = new Color32(b, b, b, a);
					if (!dialog_DevNoiseMap.cutoffEnabled || dialog_DevNoiseMap.noise.GetValue((double)i, 0.0, (double)j) >= (double)dialog_DevNoiseMap.cutoff)
					{
						LayerSubMesh subMesh = base.GetSubMesh(SectionLayer_DebugNoise.material);
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						subMesh.colors.Add(item);
						subMesh.colors.Add(item);
						subMesh.colors.Add(item);
						subMesh.colors.Add(item);
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 1);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count + 3);
					}
				}
			}
			base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
		}

		// Token: 0x040019FD RID: 6653
		private static Material material;
	}
}
