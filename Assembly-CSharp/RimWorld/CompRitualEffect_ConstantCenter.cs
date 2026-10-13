using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020027EE RID: 10222
	public class CompRitualEffect_ConstantCenter : CompRitualEffect_Constant
	{
		// Token: 0x0600DFD0 RID: 57296 RVA: 0x0041BB04 File Offset: 0x00419D04
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.Cell.ToVector3Shifted();
		}
	}
}
