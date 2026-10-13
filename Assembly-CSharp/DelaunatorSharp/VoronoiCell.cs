using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02003ED9 RID: 16089
	public struct VoronoiCell : IVoronoiCell
	{
		// Token: 0x17003769 RID: 14185
		// (get) Token: 0x06016184 RID: 90500 RVA: 0x0069C65F File Offset: 0x0069A85F
		// (set) Token: 0x06016185 RID: 90501 RVA: 0x0069C667 File Offset: 0x0069A867
		public Vector2[] Points { readonly get; set; }

		// Token: 0x1700376A RID: 14186
		// (get) Token: 0x06016186 RID: 90502 RVA: 0x0069C670 File Offset: 0x0069A870
		// (set) Token: 0x06016187 RID: 90503 RVA: 0x0069C678 File Offset: 0x0069A878
		public int Index { readonly get; set; }

		// Token: 0x06016188 RID: 90504 RVA: 0x0069C681 File Offset: 0x0069A881
		public VoronoiCell(int triangleIndex, Vector2[] points)
		{
			this.Points = points;
			this.Index = triangleIndex;
		}
	}
}
