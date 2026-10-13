using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02003ED6 RID: 16086
	public interface IVoronoiCell
	{
		// Token: 0x17003762 RID: 14178
		// (get) Token: 0x06016176 RID: 90486
		Vector2[] Points { get; }

		// Token: 0x17003763 RID: 14179
		// (get) Token: 0x06016177 RID: 90487
		int Index { get; }
	}
}
