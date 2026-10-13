using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046D RID: 1133
	[StaticConstructorOnStartup]
	public class MapDrawLayer_ExteriorLightingOverlay : MapDrawLayer
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x00097D14 File Offset: 0x00095F14
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawLightingOverlay && !base.Map.DrawMapClippers;
			}
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00097D2D File Offset: 0x00095F2D
		public MapDrawLayer_ExteriorLightingOverlay(Map map) : base(map)
		{
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00097D38 File Offset: 0x00095F38
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.LightOverlay);
			subMesh.renderLayer = MapDrawLayer_ExteriorLightingOverlay.RenderLayer;
			MapDrawLayer_ExteriorLightingOverlay.MakeBaseGeometry(base.Map, subMesh);
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x00097D68 File Offset: 0x00095F68
		private static IEnumerable<Rect> GetClipperRects(Map map)
		{
			yield return new Rect(-200f, 0f, 200f, (float)map.Size.z);
			yield return new Rect((float)map.Size.x, 0f, 200f, (float)map.Size.z);
			yield return new Rect(-200f, (float)map.Size.z, (float)(map.Size.x + 400), 200f);
			yield return new Rect(-200f, -200f, (float)(map.Size.x + 400), 200f);
			yield break;
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x00097D78 File Offset: 0x00095F78
		private static void MakeBaseGeometry(Map map, LayerSubMesh sm)
		{
			float y = AltitudeLayer.LightingOverlay.AltitudeFor();
			sm.Clear(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
			foreach (Rect rect in MapDrawLayer_ExteriorLightingOverlay.GetClipperRects(map))
			{
				sm.verts.Add(new Vector3(rect.x, y, rect.y));
				sm.verts.Add(new Vector3(rect.x, y, rect.yMax));
				sm.verts.Add(new Vector3(rect.xMax, y, rect.yMax));
				sm.verts.Add(new Vector3(rect.xMax, y, rect.y));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.colors.Add(new Color32(0, 0, 0, 0));
				sm.tris.Add(sm.verts.Count - 4);
				sm.tris.Add(sm.verts.Count - 3);
				sm.tris.Add(sm.verts.Count - 2);
				sm.tris.Add(sm.verts.Count - 4);
				sm.tris.Add(sm.verts.Count - 2);
				sm.tris.Add(sm.verts.Count - 1);
			}
			sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
		}

		// Token: 0x040019D8 RID: 6616
		private static readonly int RenderLayer = LayerMask.NameToLayer("GravshipExclude");

		// Token: 0x040019D9 RID: 6617
		private const int Size = 200;
	}
}
