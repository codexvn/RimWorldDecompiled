using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C93 RID: 15507
	public class WorldDrawLayer_Satellites : WorldDrawLayer
	{
		// Token: 0x17003477 RID: 13431
		// (get) Token: 0x0601512A RID: 86314 RVA: 0x0064AF70 File Offset: 0x00649170
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x17003478 RID: 13432
		// (get) Token: 0x0601512B RID: 86315 RVA: 0x0064EA6B File Offset: 0x0064CC6B
		public override bool ShouldRegenerate
		{
			get
			{
				return this.lastUpdate != GenTicks.TicksGame;
			}
		}

		// Token: 0x0601512C RID: 86316 RVA: 0x0064EA7D File Offset: 0x0064CC7D
		public override IEnumerable Regenerate()
		{
			if (this.material == null)
			{
				this.material = MaterialPool.MatFrom("PlaceholderImage", ShaderDatabase.WorldOverlayTransparentLit, 3600);
			}
			if (this.orbits == null)
			{
				this.orbits = new List<WorldDrawLayer_Satellites.Orbit>
				{
					new WorldDrawLayer_Satellites.Orbit
					{
						inclination = 15f,
						angleOfAscendingNode = 0f,
						altitude = 25f,
						rotationAngle = 0f,
						angleDeltaSecond = -4f
					}
				};
			}
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			float num = (GenTicks.TicksGame - this.lastUpdate).TicksToSeconds();
			for (int i = 0; i < this.orbits.Count; i++)
			{
				WorldDrawLayer_Satellites.Orbit orbit = this.orbits[i];
				Vector3 pos = Quaternion.AngleAxis(orbit.angleOfAscendingNode, Vector3.up) * Quaternion.Euler(0f, 0f, orbit.inclination) * Quaternion.Euler(0f, orbit.rotationAngle, 0f) * (-Vector3.forward * (100f + orbit.altitude));
				LayerSubMesh subMesh = base.GetSubMesh(this.material);
				WorldRendererUtility.PrintQuadTangentialToPlanet(pos, this.planetLayer.AverageTileSize * 6f, 0f, subMesh, false, 0f, true);
				orbit.rotationAngle = (orbit.rotationAngle + orbit.angleDeltaSecond * num) % 360f;
				this.orbits[i] = orbit;
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			this.lastUpdate = GenTicks.TicksGame;
			yield break;
			yield break;
		}

		// Token: 0x0400EB17 RID: 60183
		private const float DistanceToOrbit = 25f;

		// Token: 0x0400EB18 RID: 60184
		private Material material;

		// Token: 0x0400EB19 RID: 60185
		private int lastUpdate;

		// Token: 0x0400EB1A RID: 60186
		private List<WorldDrawLayer_Satellites.Orbit> orbits;

		// Token: 0x02003C94 RID: 15508
		private struct Orbit
		{
			// Token: 0x0400EB1B RID: 60187
			public float inclination;

			// Token: 0x0400EB1C RID: 60188
			public float angleOfAscendingNode;

			// Token: 0x0400EB1D RID: 60189
			public float altitude;

			// Token: 0x0400EB1E RID: 60190
			public float rotationAngle;

			// Token: 0x0400EB1F RID: 60191
			public float angleDeltaSecond;
		}
	}
}
