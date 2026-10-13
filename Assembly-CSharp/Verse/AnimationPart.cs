using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FE RID: 510
	public abstract class AnimationPart
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000DA3 RID: 3491
		protected abstract Type DefaultWorker { get; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0004118B File Offset: 0x0003F38B
		public Type WorkerType
		{
			get
			{
				return this.workerType ?? this.DefaultWorker;
			}
		}

		// Token: 0x040007F8 RID: 2040
		public Vector2 pivot = new Vector2(0.5f, 0.5f);

		// Token: 0x040007F9 RID: 2041
		public RotationMode rotationMode;

		// Token: 0x040007FA RID: 2042
		public Type workerType;
	}
}
