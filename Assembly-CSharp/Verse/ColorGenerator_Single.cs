using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200022D RID: 557
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x06000E98 RID: 3736 RVA: 0x0004410C File Offset: 0x0004230C
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		// Token: 0x0400096A RID: 2410
		public Color color;
	}
}
