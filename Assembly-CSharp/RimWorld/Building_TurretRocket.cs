using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02002987 RID: 10631
	public class Building_TurretRocket : Building_TurretGun
	{
		// Token: 0x170023B8 RID: 9144
		// (get) Token: 0x0600E9BE RID: 59838 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanSetForcedTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023B9 RID: 9145
		// (get) Token: 0x0600E9BF RID: 59839 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool HideForceTargetGizmo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023BA RID: 9146
		// (get) Token: 0x0600E9C0 RID: 59840 RVA: 0x0044054D File Offset: 0x0043E74D
		public override Material TurretTopMaterial
		{
			get
			{
				if (this.refuelableComp.IsFull)
				{
					return this.def.building.turretGunDef.building.turretTopLoadedMat;
				}
				return this.def.building.turretTopMat;
			}
		}
	}
}
