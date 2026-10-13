using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027FB RID: 10235
	public class CompRitualEffect_IntervalSpawnCircle : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x1700222E RID: 8750
		// (get) Token: 0x0600DFF0 RID: 57328 RVA: 0x0041C0F5 File Offset: 0x0041A2F5
		protected new CompProperties_RitualEffectIntervalSpawnCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnCircle)this.props;
			}
		}

		// Token: 0x0600DFF1 RID: 57329 RVA: 0x0041C104 File Offset: 0x0041A304
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map).Cells.RandomElementByWeight(delegate(IntVec3 c)
			{
				float f = Mathf.Max(Mathf.Abs((c - ritual.selectedTarget.Cell).LengthHorizontal - this.Props.radius), 1f);
				return 1f / Mathf.Pow(f, this.Props.concentration);
			}).ToVector3Shifted() + Rand.UnitVector3 * 0.5f;
		}
	}
}
