using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02003E28 RID: 15912
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06015BC4 RID: 89028 RVA: 0x0067FF2C File Offset: 0x0067E12C
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06015BC5 RID: 89029 RVA: 0x0067FF4B File Offset: 0x0067E14B
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft;
		}

		// Token: 0x06015BC6 RID: 89030 RVA: 0x0067FF64 File Offset: 0x0067E164
		public static float EnterCooldownHoursLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft * 24f;
		}

		// Token: 0x06015BC7 RID: 89031 RVA: 0x0067FF90 File Offset: 0x0067E190
		public static int EnterCooldownTicksLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return Mathf.CeilToInt((component != null) ? (component.DaysLeft * 60000f) : 0f);
		}
	}
}
