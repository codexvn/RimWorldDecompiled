using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02003ED5 RID: 16085
	public interface ITriangle
	{
		// Token: 0x17003760 RID: 14176
		// (get) Token: 0x06016174 RID: 90484
		IEnumerable<Vector2> Points { get; }

		// Token: 0x17003761 RID: 14177
		// (get) Token: 0x06016175 RID: 90485
		int Index { get; }
	}
}
