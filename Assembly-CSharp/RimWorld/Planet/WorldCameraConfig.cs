using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02003C60 RID: 15456
	public abstract class WorldCameraConfig
	{
		// Token: 0x06014FE6 RID: 85990 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x06014FE7 RID: 85991 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x0400EA24 RID: 59940
		public float dollyRateKeys = 170f;

		// Token: 0x0400EA25 RID: 59941
		public float dollyRateScreenEdge = 125f;

		// Token: 0x0400EA26 RID: 59942
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x0400EA27 RID: 59943
		public float rotationSpeedScale = 0.3f;

		// Token: 0x0400EA28 RID: 59944
		public float zoomSpeed = 2.6f;

		// Token: 0x0400EA29 RID: 59945
		public float zoomPreserveFactor;

		// Token: 0x0400EA2A RID: 59946
		public bool smoothZoom;
	}
}
