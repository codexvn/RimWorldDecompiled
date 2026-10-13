using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB5 RID: 6837
	public class PlanetLayerSettings : IExposable
	{
		// Token: 0x0600992D RID: 39213 RVA: 0x002CA090 File Offset: 0x002C8290
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 100f, false);
			Scribe_Values.Look<float>(ref this.viewAngle, "viewAngle", 180f, false);
			Scribe_Values.Look<float>(ref this.extraCameraAltitude, "extraCameraAltitude", 0f, false);
			Scribe_Values.Look<int>(ref this.subdivisions, "subdivisions", 10, false);
			Scribe_Values.Look<bool>(ref this.useSurfaceViewAngle, "useSurfaceViewAngle", false, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraOffset, "backgroundWorldCameraOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraParallaxDistancePer100Cells, "backgroundWorldCameraParallaxDistancePer100Cells", 0f, false);
		}

		// Token: 0x0400681F RID: 26655
		public Vector3 origin;

		// Token: 0x04006820 RID: 26656
		public float radius = 100f;

		// Token: 0x04006821 RID: 26657
		public float viewAngle = 180f;

		// Token: 0x04006822 RID: 26658
		public float extraCameraAltitude;

		// Token: 0x04006823 RID: 26659
		public int subdivisions = 10;

		// Token: 0x04006824 RID: 26660
		public bool useSurfaceViewAngle;

		// Token: 0x04006825 RID: 26661
		public float backgroundWorldCameraOffset;

		// Token: 0x04006826 RID: 26662
		public float backgroundWorldCameraParallaxDistancePer100Cells;
	}
}
