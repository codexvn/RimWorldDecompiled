using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045D RID: 1117
	public class CellFXSpawner
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001D4A RID: 7498 RVA: 0x00094330 File Offset: 0x00092530
		// (set) Token: 0x06001D4B RID: 7499 RVA: 0x00094338 File Offset: 0x00092538
		public List<IntVec3> Cells { get; private set; }

		// Token: 0x06001D4C RID: 7500 RVA: 0x00094341 File Offset: 0x00092541
		public CellFXSpawner(float fxPerTilePerSec, Action<Vector3> emitterFunc)
		{
			this.Cells = new List<IntVec3>();
			this.emitterFunc = emitterFunc;
			this.fxPerTilePerTick = fxPerTilePerSec / 60f;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00094368 File Offset: 0x00092568
		public void Tick()
		{
			this.queue += this.fxPerTilePerTick * (float)this.Cells.Count;
			while (this.queue >= 1f)
			{
				this.emitterFunc(this.Cells.RandomElement<IntVec3>().ToVector3Shifted());
				this.queue -= 1f;
			}
		}

		// Token: 0x04001980 RID: 6528
		private float fxPerTilePerTick;

		// Token: 0x04001981 RID: 6529
		private Action<Vector3> emitterFunc;

		// Token: 0x04001982 RID: 6530
		private float queue;
	}
}
