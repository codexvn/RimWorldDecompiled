using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BDA RID: 3034
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06004DAF RID: 19887 RVA: 0x0018AF04 File Offset: 0x00189104
		private string FinalText
		{
			get
			{
				string text;
				if (this.signal.textGetter != null)
				{
					try
					{
						text = this.signal.textGetter();
						goto IL_3E;
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString());
						text = "Error getting tip text.";
						goto IL_3E;
					}
				}
				text = this.signal.text;
				IL_3E:
				return text.TrimEnd();
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x0018AF68 File Offset: 0x00189168
		public Rect TipRect
		{
			get
			{
				string finalText = this.FinalText;
				Vector2 vector = Text.CalcSize(finalText);
				if (vector.x > 260f)
				{
					vector.x = 260f;
					vector.y = Text.CalcHeight(finalText, vector.x);
				}
				return new Rect(0f, 0f, vector.x, vector.y).ContractedBy(-4f).RoundedCeil();
			}
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x0018AFD9 File Offset: 0x001891D9
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0018AFE8 File Offset: 0x001891E8
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x0018B014 File Offset: 0x00189214
		public float DrawTooltip(Vector2 pos)
		{
			Text.Font = GameFont.Small;
			string finalText = this.FinalText;
			Rect bgRect = this.TipRect;
			bgRect.position = pos;
			if (!LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting)
			{
				Find.WindowStack.ImmediateWindow(153 * this.signal.uniqueId + 62346, bgRect, WindowLayer.Super, delegate
				{
					this.DrawInner(bgRect.AtZero(), finalText);
				}, false, false, 1f, null, false);
			}
			else
			{
				Widgets.DrawShadowAround(bgRect);
				Widgets.DrawWindowBackground(bgRect);
				this.DrawInner(bgRect, finalText);
			}
			return bgRect.height;
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x0018B0D1 File Offset: 0x001892D1
		private void DrawInner(Rect bgRect, string label)
		{
			Widgets.DrawAtlas(bgRect, ActiveTip.TooltipBGAtlas);
			Text.Font = GameFont.Small;
			Widgets.Label(bgRect.ContractedBy(4f), label);
		}

		// Token: 0x04003725 RID: 14117
		public TipSignal signal;

		// Token: 0x04003726 RID: 14118
		public double firstTriggerTime;

		// Token: 0x04003727 RID: 14119
		public int lastTriggerFrame;

		// Token: 0x04003728 RID: 14120
		private const int TipMargin = 4;

		// Token: 0x04003729 RID: 14121
		private const float MaxWidth = 260f;

		// Token: 0x0400372A RID: 14122
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}
