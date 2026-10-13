using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E3C RID: 3644
	public static class SoundSlotManager
	{
		// Token: 0x06005BED RID: 23533 RVA: 0x001D6204 File Offset: 0x001D4404
		public static bool CanPlayNow(string slotName)
		{
			if (slotName == "")
			{
				return true;
			}
			float num = 0f;
			return !SoundSlotManager.allowedPlayTimes.TryGetValue(slotName, out num) || Time.realtimeSinceStartup >= SoundSlotManager.allowedPlayTimes[slotName];
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x001D624C File Offset: 0x001D444C
		public static void Notify_Played(string slot, float duration)
		{
			if (slot == "")
			{
				return;
			}
			float a;
			if (SoundSlotManager.allowedPlayTimes.TryGetValue(slot, out a))
			{
				SoundSlotManager.allowedPlayTimes[slot] = Mathf.Max(a, Time.realtimeSinceStartup + duration);
				return;
			}
			SoundSlotManager.allowedPlayTimes[slot] = Time.realtimeSinceStartup + duration;
		}

		// Token: 0x0400429B RID: 17051
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();
	}
}
