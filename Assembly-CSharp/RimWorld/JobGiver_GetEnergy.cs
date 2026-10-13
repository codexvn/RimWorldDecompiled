using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001491 RID: 5265
	public abstract class JobGiver_GetEnergy : ThinkNode_JobGiver
	{
		// Token: 0x06007E8F RID: 32399 RVA: 0x002615B0 File Offset: 0x0025F7B0
		public static int GetMinAutorechargeThreshold(Pawn pawn)
		{
			int num = pawn.RaceProps.maxMechEnergy;
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (JobGiver_GetEnergy.UseGroupRechargeLimits(mechControlGroup))
			{
				num = Mathf.RoundToInt((float)num * mechControlGroup.mechRechargeThresholds.min);
			}
			return num;
		}

		// Token: 0x06007E90 RID: 32400 RVA: 0x002615F0 File Offset: 0x0025F7F0
		public static float GetMaxRechargeLimit(Pawn pawn)
		{
			int num = pawn.RaceProps.maxMechEnergy;
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (JobGiver_GetEnergy.UseGroupRechargeLimits(mechControlGroup))
			{
				num = Mathf.RoundToInt((float)num * mechControlGroup.mechRechargeThresholds.max);
			}
			return (float)num;
		}

		// Token: 0x06007E91 RID: 32401 RVA: 0x0026162E File Offset: 0x0025F82E
		private static bool UseGroupRechargeLimits(MechanitorControlGroup controlGroup)
		{
			return controlGroup != null && controlGroup.WorkMode != null && !controlGroup.WorkMode.ignoreGroupChargeLimits;
		}

		// Token: 0x06007E92 RID: 32402 RVA: 0x0026164C File Offset: 0x0025F84C
		protected virtual bool ShouldAutoRecharge(Pawn pawn)
		{
			Need_MechEnergy energy = pawn.needs.energy;
			return energy != null && (this.forced || energy.CurLevel + 0.1f < (float)JobGiver_GetEnergy.GetMinAutorechargeThreshold(pawn));
		}

		// Token: 0x06007E93 RID: 32403 RVA: 0x00261689 File Offset: 0x0025F889
		public override float GetPriority(Pawn pawn)
		{
			if (!this.ShouldAutoRecharge(pawn))
			{
				return 0f;
			}
			return 9.5f;
		}

		// Token: 0x040054DD RID: 21725
		public const int DefaultEnergyLevelThreshold = 15;

		// Token: 0x040054DE RID: 21726
		private const float MinAutoRechargeDelta = 0.1f;

		// Token: 0x040054DF RID: 21727
		public bool forced;
	}
}
