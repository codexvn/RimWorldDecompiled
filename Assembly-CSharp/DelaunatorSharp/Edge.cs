using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02003ED7 RID: 16087
	public struct Edge : IEdge
	{
		// Token: 0x17003764 RID: 14180
		// (get) Token: 0x06016178 RID: 90488 RVA: 0x0069C5E3 File Offset: 0x0069A7E3
		// (set) Token: 0x06016179 RID: 90489 RVA: 0x0069C5EB File Offset: 0x0069A7EB
		public Vector2 P { readonly get; set; }

		// Token: 0x17003765 RID: 14181
		// (get) Token: 0x0601617A RID: 90490 RVA: 0x0069C5F4 File Offset: 0x0069A7F4
		// (set) Token: 0x0601617B RID: 90491 RVA: 0x0069C5FC File Offset: 0x0069A7FC
		public Vector2 Q { readonly get; set; }

		// Token: 0x17003766 RID: 14182
		// (get) Token: 0x0601617C RID: 90492 RVA: 0x0069C605 File Offset: 0x0069A805
		// (set) Token: 0x0601617D RID: 90493 RVA: 0x0069C60D File Offset: 0x0069A80D
		public int Index { readonly get; set; }

		// Token: 0x0601617E RID: 90494 RVA: 0x0069C616 File Offset: 0x0069A816
		public Edge(int e, Vector2 p, Vector2 q)
		{
			this.Index = e;
			this.P = p;
			this.Q = q;
		}
	}
}
