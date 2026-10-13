using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020025AB RID: 9643
	public class Capture
	{
		// Token: 0x17002086 RID: 8326
		// (get) Token: 0x0600D34E RID: 54094 RVA: 0x003E3BC3 File Offset: 0x003E1DC3
		public bool IsTerrainCapture
		{
			get
			{
				return this.engine == null;
			}
		}

		// Token: 0x17002087 RID: 8327
		// (get) Token: 0x0600D34F RID: 54095 RVA: 0x003E3BCE File Offset: 0x003E1DCE
		public bool IsGravshipCapture
		{
			get
			{
				return this.engine != null;
			}
		}

		// Token: 0x0600D350 RID: 54096 RVA: 0x003E3BD9 File Offset: 0x003E1DD9
		public Capture(Building_GravEngine engine)
		{
			this.engine = engine;
		}

		// Token: 0x04008F65 RID: 36709
		public Building_GravEngine engine;

		// Token: 0x04008F66 RID: 36710
		public SavedTexture2D capture;

		// Token: 0x04008F67 RID: 36711
		public Vector3 drawSize;

		// Token: 0x04008F68 RID: 36712
		public Vector3 captureCenter;

		// Token: 0x04008F69 RID: 36713
		public float minCameraSize;

		// Token: 0x04008F6A RID: 36714
		public float maxCameraSize;

		// Token: 0x04008F6B RID: 36715
		public CellRect captureBounds;
	}
}
