using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BCC RID: 3020
	public static class MessagesRepeatAvoider
	{
		// Token: 0x06004D52 RID: 19794 RVA: 0x00189289 File Offset: 0x00187489
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x00189298 File Offset: 0x00187498
		public static bool MessageShowAllowed(string tag, float minSecondsSinceLastShow)
		{
			float num;
			if (!MessagesRepeatAvoider.lastShowTimes.TryGetValue(tag, out num))
			{
				num = -99999f;
			}
			bool flag = RealTime.LastRealTime > num + minSecondsSinceLastShow;
			if (flag)
			{
				MessagesRepeatAvoider.lastShowTimes[tag] = RealTime.LastRealTime;
			}
			return flag;
		}

		// Token: 0x040036F5 RID: 14069
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}
