using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BE0 RID: 3040
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x06004DD1 RID: 19921 RVA: 0x0018B9E0 File Offset: 0x00189BE0
		public static void HighlightTag(string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (tag.NullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				if (UIHighlighter.liveTags[i].First == tag && UIHighlighter.liveTags[i].Second == Time.frameCount)
				{
					return;
				}
			}
			UIHighlighter.liveTags.Add(new Pair<string, int>(tag, Time.frameCount));
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0018BA64 File Offset: 0x00189C64
		public static void HighlightOpportunity(Rect rect, string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				Pair<string, int> pair = UIHighlighter.liveTags[i];
				if (tag == pair.First && Time.frameCount == pair.Second + 1)
				{
					Widgets.DrawTextHighlight(rect, 0f, new Color?(Color.white.ToTransparent(Pulser.PulseBrightness(1.2f, 0.7f))));
				}
			}
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0018BAE8 File Offset: 0x00189CE8
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}

		// Token: 0x0400373E RID: 14142
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x0400373F RID: 14143
		private const float PulseFrequency = 1.2f;

		// Token: 0x04003740 RID: 14144
		private const float PulseAmplitude = 0.7f;
	}
}
