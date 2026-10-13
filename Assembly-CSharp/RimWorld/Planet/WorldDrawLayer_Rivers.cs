using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02003C8E RID: 15502
	public class WorldDrawLayer_Rivers : WorldDrawLayer_Paths
	{
		// Token: 0x1700346F RID: 13423
		// (get) Token: 0x06015109 RID: 86281 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003470 RID: 13424
		// (get) Token: 0x0601510A RID: 86282 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601510B RID: 86283 RVA: 0x0064DFE4 File Offset: 0x0064C1E4
		public WorldDrawLayer_Rivers()
		{
			this.pointyEnds = true;
		}

		// Token: 0x0601510C RID: 86284 RVA: 0x0064E09B File Offset: 0x0064C29B
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Rivers);
			LayerSubMesh subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
			List<WorldDrawLayer_Paths.OutputDirection> outputs = new List<WorldDrawLayer_Paths.OutputDirection>();
			List<WorldDrawLayer_Paths.OutputDirection> outputsBorder = new List<WorldDrawLayer_Paths.OutputDirection>();
			int num;
			for (int i = 0; i < this.planetLayer.TilesCount; i = num)
			{
				if (i % 1000 == 0)
				{
					yield return null;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Rivers);
					subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
				}
				SurfaceTile surfaceTile = (SurfaceTile)this.planetLayer[i];
				if (surfaceTile.potentialRivers != null)
				{
					outputs.Clear();
					outputsBorder.Clear();
					for (int j = 0; j < surfaceTile.potentialRivers.Count; j++)
					{
						outputs.Add(new WorldDrawLayer_Paths.OutputDirection
						{
							neighbor = surfaceTile.potentialRivers[j].neighbor,
							width = surfaceTile.potentialRivers[j].river.widthOnWorld - 0.2f
						});
						outputsBorder.Add(new WorldDrawLayer_Paths.OutputDirection
						{
							neighbor = surfaceTile.potentialRivers[j].neighbor,
							width = surfaceTile.potentialRivers[j].river.widthOnWorld
						});
					}
					base.GeneratePaths(subMesh, surfaceTile.tile, outputs, this.riverColor, true);
					base.GeneratePaths(subMeshBorder, surfaceTile.tile, outputsBorder, this.riverColor, true);
				}
				num = i + 1;
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0601510D RID: 86285 RVA: 0x0064E0AC File Offset: 0x0064C2AC
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.riverDisplacementX.GetValue(inp), this.riverDisplacementY.GetValue(inp), this.riverDisplacementZ.GetValue(inp)) * 0.1f).normalized * magnitude;
			return inp + inp.normalized * 0.01f;
		}

		// Token: 0x0400EAFA RID: 60154
		private readonly Color32 riverColor = new Color32(73, 82, 100, byte.MaxValue);

		// Token: 0x0400EAFB RID: 60155
		private const float PerlinFrequency = 0.6f;

		// Token: 0x0400EAFC RID: 60156
		private const float PerlinMagnitude = 0.1f;

		// Token: 0x0400EAFD RID: 60157
		private readonly ModuleBase riverDisplacementX = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

		// Token: 0x0400EAFE RID: 60158
		private readonly ModuleBase riverDisplacementY = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

		// Token: 0x0400EAFF RID: 60159
		private readonly ModuleBase riverDisplacementZ = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 91572032, QualityMode.Medium);
	}
}
