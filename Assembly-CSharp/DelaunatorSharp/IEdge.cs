using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02003ED4 RID: 16084
	public interface IEdge
	{
		// Token: 0x1700375D RID: 14173
		// (get) Token: 0x06016171 RID: 90481
		Vector2 P { get; }

		// Token: 0x1700375E RID: 14174
		// (get) Token: 0x06016172 RID: 90482
		Vector2 Q { get; }

		// Token: 0x1700375F RID: 14175
		// (get) Token: 0x06016173 RID: 90483
		int Index { get; }
	}
}
