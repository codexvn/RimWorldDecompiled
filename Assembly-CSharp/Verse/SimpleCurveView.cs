using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E0 RID: 224
	public class SimpleCurveView
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x0001E76B File Offset: 0x0001C96B
		public IEnumerable<float> DebugInputValues
		{
			get
			{
				if (this.debugInputValues == null)
				{
					yield break;
				}
				foreach (float num in this.debugInputValues.Values)
				{
					yield return num;
				}
				Dictionary<object, float>.ValueCollection.Enumerator enumerator = default(Dictionary<object, float>.ValueCollection.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001E77B File Offset: 0x0001C97B
		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001E78A File Offset: 0x0001C98A
		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001E7A8 File Offset: 0x0001C9A8
		public void SetViewRectAround(SimpleCurve curve)
		{
			if (curve.PointsCount == 0)
			{
				this.rect = SimpleCurveView.identityRect;
				return;
			}
			this.rect.xMin = (from pt in curve.Points
			select pt.Loc.x).Min();
			this.rect.xMax = (from pt in curve.Points
			select pt.Loc.x).Max();
			this.rect.yMin = (from pt in curve.Points
			select pt.Loc.y).Min();
			this.rect.yMax = (from pt in curve.Points
			select pt.Loc.y).Max();
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = this.rect.xMin * 2f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = this.rect.yMin * 2f;
			}
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = 1f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = 1f;
			}
			float width = this.rect.width;
			float height = this.rect.height;
			this.rect.xMin = this.rect.xMin - width * 0.1f;
			this.rect.xMax = this.rect.xMax + width * 0.1f;
			this.rect.yMin = this.rect.yMin - height * 0.1f;
			this.rect.yMax = this.rect.yMax + height * 0.1f;
		}

		// Token: 0x04000325 RID: 805
		public Rect rect;

		// Token: 0x04000326 RID: 806
		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		// Token: 0x04000327 RID: 807
		private const float ResetZoomBuffer = 0.1f;

		// Token: 0x04000328 RID: 808
		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);
	}
}
