using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02003C90 RID: 15504
	public class WorldDrawLayer_Roads : WorldDrawLayer_Paths
	{
		// Token: 0x17003473 RID: 13427
		// (get) Token: 0x06015118 RID: 86296 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003474 RID: 13428
		// (get) Token: 0x06015119 RID: 86297 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601511A RID: 86298 RVA: 0x0064E4CB File Offset: 0x0064C6CB
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Roads);
			List<RoadWorldLayerDef> roadLayerDefs = (from def in DefDatabase<RoadWorldLayerDef>.AllDefs
			orderby def.order
			select def).ToList<RoadWorldLayerDef>();
			int num;
			for (int i = 0; i < this.planetLayer.TilesCount; i = num)
			{
				if (i % 1000 == 0)
				{
					yield return null;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Roads);
				}
				SurfaceTile surfaceTile = (SurfaceTile)this.planetLayer[i];
				if (!surfaceTile.WaterCovered)
				{
					List<WorldDrawLayer_Paths.OutputDirection> list = new List<WorldDrawLayer_Paths.OutputDirection>();
					if (surfaceTile.potentialRoads != null)
					{
						bool allowSmoothTransition = true;
						for (int j = 0; j < surfaceTile.potentialRoads.Count - 1; j++)
						{
							if (surfaceTile.potentialRoads[j].road.worldTransitionGroup != surfaceTile.potentialRoads[j + 1].road.worldTransitionGroup)
							{
								allowSmoothTransition = false;
							}
						}
						for (int k = 0; k < roadLayerDefs.Count; k++)
						{
							bool flag = false;
							list.Clear();
							for (int l = 0; l < surfaceTile.potentialRoads.Count; l++)
							{
								RoadDef road = surfaceTile.potentialRoads[l].road;
								float layerWidth = road.GetLayerWidth(roadLayerDefs[k]);
								if (layerWidth > 0f)
								{
									flag = true;
								}
								list.Add(new WorldDrawLayer_Paths.OutputDirection
								{
									neighbor = surfaceTile.potentialRoads[l].neighbor,
									width = layerWidth,
									distortionFrequency = road.distortionFrequency,
									distortionIntensity = road.distortionIntensity
								});
							}
							if (flag)
							{
								base.GeneratePaths(subMesh, new PlanetTile(i, this.planetLayer), list, roadLayerDefs[k].color, allowSmoothTransition);
							}
						}
					}
				}
				num = i + 1;
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0601511B RID: 86299 RVA: 0x0064E4DC File Offset: 0x0064C6DC
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			Vector3 coordinate = inp * distortionFrequency;
			float magnitude = inp.magnitude;
			Vector3 a = new Vector3(this.roadDisplacementX.GetValue(coordinate), this.roadDisplacementY.GetValue(coordinate), this.roadDisplacementZ.GetValue(coordinate));
			if ((double)a.magnitude > 0.0001)
			{
				float d = (1f / (1f + Mathf.Exp(-a.magnitude / 1f * 2f)) * 2f - 1f) * 1f;
				a = a.normalized * d;
			}
			inp = (inp + a * distortionIntensity).normalized * magnitude;
			return inp + inp.normalized * 0.02f;
		}

		// Token: 0x0400EB0A RID: 60170
		private readonly ModuleBase roadDisplacementX = new Perlin(1.0, 2.0, 0.5, 3, 74173887, QualityMode.Medium);

		// Token: 0x0400EB0B RID: 60171
		private readonly ModuleBase roadDisplacementY = new Perlin(1.0, 2.0, 0.5, 3, 67515931, QualityMode.Medium);

		// Token: 0x0400EB0C RID: 60172
		private readonly ModuleBase roadDisplacementZ = new Perlin(1.0, 2.0, 0.5, 3, 87116801, QualityMode.Medium);
	}
}
