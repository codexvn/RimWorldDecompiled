using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BD4 RID: 3028
	public class ScrollPositioner
	{
		// Token: 0x06004D6E RID: 19822 RVA: 0x0018A126 File Offset: 0x00188326
		public void Arm(bool armed = true)
		{
			this.armed = armed;
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x0018A12F File Offset: 0x0018832F
		public void ClearInterestRects()
		{
			this.interestRect = null;
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0018A13D File Offset: 0x0018833D
		public void RegisterInterestRect(Rect rect)
		{
			if (this.interestRect != null)
			{
				this.interestRect = new Rect?(rect.Union(this.interestRect.Value));
				return;
			}
			this.interestRect = new Rect?(rect);
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x0018A175 File Offset: 0x00188375
		public void ScrollHorizontally(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, true, false);
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0018A181 File Offset: 0x00188381
		public void ScrollVertically(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, false, true);
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x0018A190 File Offset: 0x00188390
		public void Scroll(ref Vector2 scrollPos, Vector2 outRectSize, bool scrollHorizontally = true, bool scrollVertically = true)
		{
			if (Event.current.type != EventType.Layout)
			{
				return;
			}
			if (!this.armed)
			{
				return;
			}
			this.armed = false;
			if (this.interestRect == null)
			{
				return;
			}
			if (scrollHorizontally)
			{
				this.ScrollInDimension(ref scrollPos.x, outRectSize.x, this.interestRect.Value.xMin, this.interestRect.Value.xMax);
			}
			if (scrollVertically)
			{
				this.ScrollInDimension(ref scrollPos.y, outRectSize.y, this.interestRect.Value.yMin, this.interestRect.Value.yMax);
			}
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0018A240 File Offset: 0x00188440
		private void ScrollInDimension(ref float scrollPos, float scrollViewSize, float v0, float v1)
		{
			float num = v1 - v0;
			if (num <= scrollViewSize)
			{
				scrollPos = v0 + num / 2f - scrollViewSize / 2f;
				return;
			}
			scrollPos = v0;
		}

		// Token: 0x0400370C RID: 14092
		private Rect? interestRect;

		// Token: 0x0400370D RID: 14093
		private bool armed;
	}
}
