using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C7D RID: 11389
	public class CompExplosivePowered : CompExplosive
	{
		// Token: 0x17002854 RID: 10324
		// (get) Token: 0x0600FF9E RID: 65438 RVA: 0x004A1094 File Offset: 0x0049F294
		protected override bool CanEverExplodeFromDamage
		{
			get
			{
				CompMechPowerCell compMechPowerCell = this.parent.TryGetComp<CompMechPowerCell>();
				return (compMechPowerCell == null || !compMechPowerCell.depleted) && base.CanEverExplodeFromDamage;
			}
		}
	}
}
