using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AA8 RID: 10920
	public class LavaEmergenceImmediate : LavaEmergence
	{
		// Token: 0x1700260A RID: 9738
		// (get) Token: 0x0600F349 RID: 62281 RVA: 0x0046A303 File Offset: 0x00468503
		protected override IntRange PoolSizeRange
		{
			get
			{
				return new IntRange(100, 150);
			}
		}

		// Token: 0x1700260B RID: 9739
		// (get) Token: 0x0600F34A RID: 62282 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool FireLetter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600F34B RID: 62283 RVA: 0x0046A311 File Offset: 0x00468511
		protected override void Tick()
		{
			while (this.lavaCells.Count < this.poolSize && this.openCells.Count > 0)
			{
				base.SpreadLava();
			}
			base.BeginCooling();
		}

		// Token: 0x0600F34C RID: 62284 RVA: 0x0046A342 File Offset: 0x00468542
		protected override IEnumerable<IntVec3> GetInitialCells(Map map)
		{
			yield return base.Position;
			yield break;
		}
	}
}
