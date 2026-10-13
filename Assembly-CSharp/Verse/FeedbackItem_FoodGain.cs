using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B7C RID: 2940
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x06004B55 RID: 19285 RVA: 0x0017FD49 File Offset: 0x0017DF49
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x0017FD5C File Offset: 0x0017DF5C
		public override void FeedbackOnGUI()
		{
			string str = this.Amount.ToString() + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x040035A8 RID: 13736
		protected int Amount;
	}
}
