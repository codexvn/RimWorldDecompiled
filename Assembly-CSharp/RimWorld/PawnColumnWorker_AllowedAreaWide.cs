using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02003455 RID: 13397
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06012D80 RID: 77184 RVA: 0x00596185 File Offset: 0x00594385
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
