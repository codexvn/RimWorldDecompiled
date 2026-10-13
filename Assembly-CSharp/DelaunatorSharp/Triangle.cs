using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02003ED8 RID: 16088
	public struct Triangle : ITriangle
	{
		// Token: 0x17003767 RID: 14183
		// (get) Token: 0x0601617F RID: 90495 RVA: 0x0069C62D File Offset: 0x0069A82D
		// (set) Token: 0x06016180 RID: 90496 RVA: 0x0069C635 File Offset: 0x0069A835
		public int Index { readonly get; set; }

		// Token: 0x17003768 RID: 14184
		// (get) Token: 0x06016181 RID: 90497 RVA: 0x0069C63E File Offset: 0x0069A83E
		// (set) Token: 0x06016182 RID: 90498 RVA: 0x0069C646 File Offset: 0x0069A846
		public IEnumerable<Vector2> Points { readonly get; set; }

		// Token: 0x06016183 RID: 90499 RVA: 0x0069C64F File Offset: 0x0069A84F
		public Triangle(int t, IEnumerable<Vector2> points)
		{
			this.Points = points;
			this.Index = t;
		}
	}
}
