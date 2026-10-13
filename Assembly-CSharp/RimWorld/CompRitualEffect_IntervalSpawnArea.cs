using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027F5 RID: 10229
	public class CompRitualEffect_IntervalSpawnArea : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x1700222B RID: 8747
		// (get) Token: 0x0600DFE2 RID: 57314 RVA: 0x0041BE30 File Offset: 0x0041A030
		protected new CompProperties_RitualEffectIntervalSpawnArea Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnArea)this.props;
			}
		}

		// Token: 0x0600DFE3 RID: 57315 RVA: 0x0041BE40 File Offset: 0x0041A040
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			CellRect cellRect = CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map);
			if (this.Props.smoothEdges)
			{
				return cellRect.Cells.RandomElementByWeight((IntVec3 c) => 1f / Mathf.Max((c - ritual.selectedTarget.Cell).LengthHorizontal, 1f)).ToVector3Shifted() + Rand.UnitVector3 * 0.5f;
			}
			return cellRect.RandomVector3;
		}
	}
}
