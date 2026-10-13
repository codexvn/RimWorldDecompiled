using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001B86 RID: 7046
	public static class PassAllQuestPartUtility
	{
		// Token: 0x06009D59 RID: 40281 RVA: 0x002D9C94 File Offset: 0x002D7E94
		public static bool AllReceived(List<string> inSignals, List<bool> signalsReceived)
		{
			if (inSignals.Count != signalsReceived.Count)
			{
				return false;
			}
			for (int i = 0; i < signalsReceived.Count; i++)
			{
				if (!signalsReceived[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
