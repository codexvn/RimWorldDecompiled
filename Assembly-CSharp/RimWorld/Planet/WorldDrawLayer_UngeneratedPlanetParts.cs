using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C9D RID: 15517
	public class WorldDrawLayer_UngeneratedPlanetParts : WorldDrawLayer
	{
		// Token: 0x0601516B RID: 86379 RVA: 0x0064FD9B File Offset: 0x0064DF9B
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Vector3 surfaceViewCenter = Find.WorldGrid.SurfaceViewCenter;
			float surfaceViewAngle = Find.WorldGrid.SurfaceViewAngle;
			if (surfaceViewAngle < 180f)
			{
				List<Vector3> collection;
				List<int> collection2;
				SphereGenerator.Generate(4, this.planetLayer.Radius + -0.16f, -surfaceViewCenter, 180f - Mathf.Min(surfaceViewAngle, 180f) + 10f, out collection, out collection2);
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.UngeneratedPlanetParts);
				subMesh.verts.AddRange(collection);
				subMesh.tris.AddRange(collection2);
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0400EB41 RID: 60225
		private const int SubdivisionsCount = 4;

		// Token: 0x0400EB42 RID: 60226
		private const float ViewAngleOffset = 10f;
	}
}
