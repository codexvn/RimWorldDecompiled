using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200090F RID: 2319
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x06003C1F RID: 15391 RVA: 0x0013B4F0 File Offset: 0x001396F0
		public static void DelayedErrorWindowRequestOnGUI()
		{
			try
			{
				for (int i = 0; i < DelayedErrorWindowRequest.requests.Count; i++)
				{
					Find.WindowStack.Add(new Dialog_MessageBox(DelayedErrorWindowRequest.requests[i].text, "OK".Translate(), null, null, null, DelayedErrorWindowRequest.requests[i].title, false, null, null, WindowLayer.Dialog));
				}
			}
			finally
			{
				DelayedErrorWindowRequest.requests.Clear();
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x0013B57C File Offset: 0x0013977C
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x04002B44 RID: 11076
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x02000910 RID: 2320
		private struct Request
		{
			// Token: 0x04002B45 RID: 11077
			public string text;

			// Token: 0x04002B46 RID: 11078
			public string title;
		}
	}
}
