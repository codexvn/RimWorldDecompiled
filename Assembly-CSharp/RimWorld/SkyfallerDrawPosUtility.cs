using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B54 RID: 11092
	public static class SkyfallerDrawPosUtility
	{
		// Token: 0x0600F812 RID: 63506 RVA: 0x00482A38 File Offset: 0x00480C38
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, flipHorizontal, offsetComp);
		}

		// Token: 0x0600F813 RID: 63507 RVA: 0x00482A70 File Offset: 0x00480C70
		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, flipHorizontal, offsetComp);
		}

		// Token: 0x0600F814 RID: 63508 RVA: 0x00482A98 File Offset: 0x00480C98
		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(ticksToImpact * ticksToImpact) * 0.00721f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, flipHorizontal, offsetComp);
		}

		// Token: 0x0600F815 RID: 63509 RVA: 0x00482AC8 File Offset: 0x00480CC8
		private static Vector3 PosAtDist(Vector3 center, float dist, float angle, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			Vector3 vector = Vector3Utility.FromAngleFlat(angle - 90f) * dist + ((offsetComp != null) ? offsetComp.Offset : Vector3.zero);
			if (flipHorizontal)
			{
				vector.x *= -1f;
			}
			vector += center;
			return vector;
		}
	}
}
