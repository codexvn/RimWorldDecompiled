using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFF RID: 3071
	public static class OriginalEventUtility
	{
		// Token: 0x06004E76 RID: 20086 RVA: 0x0018F288 File Offset: 0x0018D488
		public static void RecordOriginalEvent(Event e)
		{
			OriginalEventUtility.originalType = new EventType?(e.type);
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06004E77 RID: 20087 RVA: 0x0018F29C File Offset: 0x0018D49C
		public static EventType EventType
		{
			get
			{
				EventType? eventType = OriginalEventUtility.originalType;
				if (eventType == null)
				{
					return Event.current.rawType;
				}
				return eventType.GetValueOrDefault();
			}
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x0018F2CA File Offset: 0x0018D4CA
		public static void Reset()
		{
			OriginalEventUtility.originalType = null;
		}

		// Token: 0x040037BF RID: 14271
		private static EventType? originalType;
	}
}
