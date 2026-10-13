using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B7E RID: 2942
	public class FeedbackFloaters
	{
		// Token: 0x06004B59 RID: 19289 RVA: 0x0017FDEC File Offset: 0x0017DFEC
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0017FDFC File Offset: 0x0017DFFC
		public void FeedbackUpdate()
		{
			for (int i = this.feeders.Count - 1; i >= 0; i--)
			{
				this.feeders[i].Update();
				if (this.feeders[i].TimeLeft <= 0f)
				{
					this.feeders.Remove(this.feeders[i]);
				}
			}
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x0017FE64 File Offset: 0x0017E064
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x040035AB RID: 13739
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
