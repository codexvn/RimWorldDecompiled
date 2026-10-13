using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B7D RID: 2941
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x06004B57 RID: 19287 RVA: 0x0017FD8B File Offset: 0x0017DF8B
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x0017FDA4 File Offset: 0x0017DFA4
		public override void FeedbackOnGUI()
		{
			string text;
			if (this.Amount >= 0)
			{
				text = "+";
			}
			else
			{
				text = "-";
			}
			text += this.Amount.ToString();
			base.DrawFloatingText(text, Color.red);
		}

		// Token: 0x040035A9 RID: 13737
		protected Pawn Healer;

		// Token: 0x040035AA RID: 13738
		protected int Amount;
	}
}
