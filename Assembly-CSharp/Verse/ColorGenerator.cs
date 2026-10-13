using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200022B RID: 555
	public abstract class ColorGenerator
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x000440E6 File Offset: 0x000422E6
		public virtual Color ExemplaryColor
		{
			get
			{
				Rand.PushState(764543439);
				Color result = this.NewRandomizedColor();
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x06000E94 RID: 3732
		public abstract Color NewRandomizedColor();
	}
}
