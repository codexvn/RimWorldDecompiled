using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CC2 RID: 11458
	public class CompGravshipShieldGenerator : CompProjectileInterceptor
	{
		// Token: 0x170028AF RID: 10415
		// (get) Token: 0x06010156 RID: 65878 RVA: 0x004A74CC File Offset: 0x004A56CC
		private CompPowerTrader Power
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.power) == null)
				{
					result = (this.power = this.parent.TryGetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x170028B0 RID: 10416
		// (get) Token: 0x06010157 RID: 65879 RVA: 0x004A74F7 File Offset: 0x004A56F7
		private CompGravshipFacility Facility
		{
			get
			{
				return this.facility ?? this.parent.TryGetComp<CompGravshipFacility>();
			}
		}

		// Token: 0x170028B1 RID: 10417
		// (get) Token: 0x06010158 RID: 65880 RVA: 0x00002501 File Offset: 0x00000701
		protected override int NumInactiveDots
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170028B2 RID: 10418
		// (get) Token: 0x06010159 RID: 65881 RVA: 0x004A750E File Offset: 0x004A570E
		private bool ShouldCharge
		{
			get
			{
				return this.Power.PowerOn && this.Facility.LinkedBuildings.Count > 0;
			}
		}

		// Token: 0x170028B3 RID: 10419
		// (get) Token: 0x0601015A RID: 65882 RVA: 0x004A7532 File Offset: 0x004A5732
		protected override int HitPointsPerInterval
		{
			get
			{
				if (base.Active)
				{
					return 0;
				}
				if (this.ShouldCharge)
				{
					return 2;
				}
				return -1;
			}
		}

		// Token: 0x0400A808 RID: 43016
		private const int HitPointsPerIntervalPowered = 2;

		// Token: 0x0400A809 RID: 43017
		private const int HitPointsPerIntervalUnpowered = -1;

		// Token: 0x0400A80A RID: 43018
		private CompPowerTrader power;

		// Token: 0x0400A80B RID: 43019
		private CompGravshipFacility facility;
	}
}
