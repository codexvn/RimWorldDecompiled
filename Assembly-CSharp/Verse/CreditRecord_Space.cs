using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C6C RID: 3180
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x06005144 RID: 20804 RVA: 0x001A6D4A File Offset: 0x001A4F4A
		public CreditRecord_Space()
		{
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x001A6D5D File Offset: 0x001A4F5D
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x001A6D77 File Offset: 0x001A4F77
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x000026EA File Offset: 0x000008EA
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x04003A9B RID: 15003
		private float height = 10f;
	}
}
